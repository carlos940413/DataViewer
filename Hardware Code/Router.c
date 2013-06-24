#include "stc.h"
#include "intrins.h"

/*------------------------------Definition Part------------------------------*/
// Macro Definition
#define KEY_SEARCH 0
#define KEY_ACK 1
#define KEY_RELEASE 2

#define KEY1 1
#define KEY2 2
#define KEY3 3
#define KEY4 4

#define WAITING 0
#define SETTING 1
#define NETWORKING 2
#define START 3

#define EEPROM_START_ADDRESS 0x2000

// Bit Definition
sbit LED_OPEN=P2^3;
sbit key1=P3^4;
sbit key2=P3^5;
sbit key3=P3^6;
sbit key4=P3^7;
sbit sig=P1^7;

//Variable Definition
unsigned int total;
unsigned int low;
unsigned char mode;
unsigned char uartCounter;
unsigned char busy;
unsigned char id;

/*------------------------------Function Part------------------------------*/
// Delay Function
void Delay_us(unsigned int us)
{
	do
	{
		_nop_();
	}while(--us>0);
}

void Delay_ms(unsigned int ms)
{
	do
	{
		Delay_us(1000);
	}while(--ms>0);
}

// Timer Function
void Timer0_Init()
{
	TH0=(65536-1000)/256;
	TL0=(65536-1000)%256;
	TMOD=0x01;
	ET0=1;
}

void Timer0_Start()
{
	TR0=1;
}

void Timer0_Stop()
{
	TR0=0;
}

// UART Function
void UART_Init()
{
	SCON=0x50;
	T2CON=0x34;
	RCAP2L=0xF7;
	RCAP2H=0xFF;
	
	ES=1;
}

void UART_SendByte(unsigned char byte)
{
	SBUF=byte;
	while(TI==0);
	TI=0;
}

void UART_SendInt(unsigned int number)
{
	UART_SendByte(number>>8);
	UART_SendByte(number);
}

// Key Function
unsigned char ScanKey()
{
	if (key1==0)
		return KEY1;
	else if (key2==0)
		return KEY2;
	else if (key3==0)
		return KEY3;
	else if (key4==0)
		return KEY4;
	return 0;
}

void ProcKey_Waiting(unsigned char key)
{
	switch (key)
	{
		case KEY3:
			mode=NETWORKING;
			break;
		case KEY4:
			mode=SETTING;
			break;
	}
}

unsigned char ProcKey_Setting(unsigned char key)
{
	switch (key)
	{
		case KEY1:
			return 1;
		case KEY2:
			return 5;
		case KEY3:
			return 10;
		case KEY4:
			mode=NETWORKING;
			break;
	}
	return 0;
}

// EEPROM Function
void EEPROMEnable()
{
	ISP_CONTR=0x81;
}

void EEPROMDisable()
{
	ISP_CONTR=0x00;
	ISP_CMD=0x00;
	ISP_TRIG=0x00;
	ISP_ADDRH=0x00;
	ISP_ADDRL=0x00;
}

void EEPROMSetAddress(unsigned int addr)
{
	addr+=EEPROM_START_ADDRESS;
	ISP_ADDRH=addr>>8;
	ISP_ADDRL=addr;
}

void EEPROMStart()
{
	ISP_TRIG=0x46;
	ISP_TRIG=0xB9;
}

unsigned char EEPROMReadByte(unsigned int addr)
{
	ISP_DATA=0x00;
	ISP_CMD=0x01;

	EEPROMEnable();
	EEPROMSetAddress(addr);
	EEPROMStart();

	Delay_us(10);

	EEPROMDisable();

	return (ISP_DATA);
}

void EEPROMWriteByte(unsigned int addr, unsigned char byte)
{
	EEPROMEnable();

	ISP_CMD=0x02;
	EEPROMSetAddress(addr);
	ISP_DATA=byte;
	EEPROMStart();
	Delay_us(60);

	EEPROMDisable();
}

void EEPROMSectorErase(unsigned int addr)
{
	ISP_CMD=0x03;
	EEPROMEnable();
	EEPROMSetAddress(addr);
	EEPROMStart();

	Delay_ms(10);

	EEPROMDisable();
}

// Watch Dog Function
void WatchDogInit()
{
	WDT_CONTR=0x35;
}

void WatchDogFeed()
{
	WDT_CONTR=0x35;
}

// Zigbee Function
void Zigbee_Reset()
{
	UART_SendByte(0xFE);
	UART_SendByte(0x03);
	UART_SendByte(0x26);
	UART_SendByte(0x05);
	UART_SendByte(0x03);
	UART_SendByte(0x01);
	UART_SendByte(0x03);
	UART_SendByte(0x21);
}

void Zigbee_Restart()
{
	UART_SendByte(0xFE);
	UART_SendByte(0x01);
	UART_SendByte(0x41);
	UART_SendByte(0x00);
	UART_SendByte(0x01);
	UART_SendByte(0x41);
}

void Zigbee_SetRouter()
{
	UART_SendByte(0xFC);
	UART_SendByte(0x00);
	UART_SendByte(0x91);
	UART_SendByte(0x0A);
	UART_SendByte(0xBA);
	UART_SendByte(0xDA);
	UART_SendByte(0x2B);
}

// MCU Mode Function
void WaitingState()
{
	unsigned char wait=0;
	unsigned char keyValue, keyTempValue, keyStatus;
	keyValue=keyTempValue=0;
	keyStatus=KEY_SEARCH;
	while (mode==WAITING)
	{
		Delay_ms(9);
		keyTempValue=ScanKey();
		switch (keyStatus)
		{
			case KEY_SEARCH:
				if (keyTempValue)
					keyStatus=KEY_ACK;
				break;
			case KEY_ACK:
				if (keyTempValue)
				{
					keyValue=keyTempValue;
					keyStatus=KEY_RELEASE;
				}
				else
					keyStatus=KEY_SEARCH;
				break;
			case KEY_RELEASE:
				if (!keyTempValue)
				{
					keyStatus=KEY_SEARCH;
					ProcKey_Waiting(keyValue);
				}
				break;
		}
		if (++wait==200)
			mode=START;
	}
}

unsigned char SettingState()
{
	unsigned char id=0;
	unsigned char keyValue, keyTempValue, keyStatus;
	keyValue=keyTempValue=0;
	keyStatus=KEY_SEARCH;
	while (mode==SETTING)
	{
		Delay_ms(9);
		keyTempValue=ScanKey();
		switch (keyStatus)
		{
			case KEY_SEARCH:
				if (keyTempValue)
					keyStatus=KEY_ACK;
				break;
			case KEY_ACK:
				if (keyTempValue)
				{
					keyValue=keyTempValue;
					keyStatus=KEY_RELEASE;
				}
				else
					keyStatus=KEY_SEARCH;
				break;
			case KEY_RELEASE:
				if (!keyTempValue)
				{
					keyStatus=KEY_SEARCH;
					id+=ProcKey_Setting(keyValue);
				}
				break;
		}
	}
	return id;
}

void NetworkingState()
{
	// Initialize Zigbee
	P0=~(0x60);
	Zigbee_Reset();
	Zigbee_Restart();
	busy=1;
	while (busy)
	{
		Zigbee_SetRouter();
		Delay_ms(80);
	}
	Zigbee_Restart();
	P0=~(0x40);
	while (mode==NETWORKING)
	{
		Delay_ms(90);
		UART_SendByte(id);
	}
}

/*------------------------------Main Part------------------------------*/
void main()
{
	// Initialize Hardware, Register
	TMOD=0x01;	// User Timer0 16bit
	// Function Initialize
	Timer0_Init();
	UART_Init();
	EA=1;		// Enable All interrupt
	LED_OPEN=0;	// Enable LED

	// Initialize Variable
	total=0;
	low=0;
	uartCounter=0;

	mode=WAITING;
	P0=0;
	WaitingState();

	switch (mode)
	{
		case SETTING:
			P0=~(0x01);
			id=SettingState();
			EEPROMSectorErase(0);
			EEPROMWriteByte(0,id);
			P0=~(0x40);
			NetworkingState();
			break;
		case NETWORKING:
			P0=~(0x40);
			id=EEPROMReadByte(0);
			NetworkingState();
			break;
	}
	P0=~(0x80);

	while (1);
}

/*------------------------------IRQ Part------------------------------*/
// Sig Timer
void Timer0_IRQ() interrupt 1
{
	TH0=(65536-1000)/256;
	TL0=(65536-1000)%256;
	if (sig==0)
		low++;
	if (++total==30000)
	{
		UART_SendByte(EEPROMReadByte(0));
		UART_SendInt(low);
		low=0;
		total=0;
		Timer0_Stop();
		P0=~(0x80);
	}
	/*
	if (total==1000)
	{
		UART_SendByte(id);
		total=0;
	}
	*/
}

void UART_IRQ() interrupt 4
{
	unsigned char recv;
	if (RI)
	{
		busy=0;
		RI=0;
		recv=SBUF;
		if (++uartCounter==4)
		{
			switch (mode)
			{
				case NETWORKING:
					if (recv==0x00)
					{
						mode=START;
					}
					break;
				case START:
					if (recv==0x01)
					{
						Timer0_Start();
						P0=~(0xC0);
					}
					break;
			}
			uartCounter=0;
		}
	}
}
