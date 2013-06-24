#include "stc12c5a60s2.h"
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
#define START 1
#define SETTING 2
#define NETWORKING 3

#define GET 1
#define SEND 2

#define CMD_IDLE 0
#define CMD_READ 1
#define CMD_PROGRAM 2
#define CMD_ERASE 3
#define ENABLE_IAP 0x82
#define IAP_ADDRESS 0x0000

// Bit Definition
sbit LED_OPEN=P2^3;
sbit key1=P3^4;
sbit key2=P3^5;
sbit key3=P3^6;
sbit key4=P3^7;

// Variable Definition
unsigned char mode;
unsigned int address;
unsigned int queryCount;
unsigned char receiveDataCount;
unsigned int concentration[20];
unsigned int index;
unsigned char queryMode;

/*------------------------------Function Part------------------------------*/
// Delay Function
void Delay_us(unsigned int us)
{
	do
	{
		_nop_();
	}while (--us>0);
}

void Delay_ms(unsigned int ms)
{
	do
	{
		Delay_us(1000);
	}while (--ms>0);
}

// EEPROM Function
void IapIdle()
{
	IAP_CONTR=0;
	IAP_CMD=0;
	IAP_TRIG=0;
	IAP_ADDRH=0x80;
	IAP_ADDRL=0x00;
}

unsigned char IapReadByte(unsigned int addr)
{
	unsigned char byte;
	IAP_CONTR=ENABLE_IAP;
	IAP_CMD=CMD_READ;
	IAP_ADDRL=addr+IAP_ADDRESS;
	IAP_ADDRH=(addr+IAP_ADDRESS)>>8;
	IAP_TRIG=0x5A;
	IAP_TRIG=0xA5;
	_nop_();
	byte=IAP_DATA;
	IapIdle();
	return byte;
}

unsigned long IapReadInt32(unsigned int addr)
{
	unsigned long int32=0;
	unsigned char i;
	for (i=0; i<4; i++)
	{
		int32<<=8;
		int32+=IapReadByte(addr+i);
	}
	return int32;
}

void IapProgramByte(unsigned int addr, unsigned char byte)
{
	IAP_CONTR=ENABLE_IAP;
	IAP_CMD=CMD_PROGRAM;
	IAP_ADDRL=addr+IAP_ADDRESS;
	IAP_ADDRH=(addr+IAP_ADDRESS)>>8;
	IAP_DATA=byte;
	IAP_TRIG=0x5A;
	IAP_TRIG=0xA5;
	_nop_();
	IapIdle();
}

unsigned int IapProgramInt32(unsigned int addr, unsigned long int32)
{
	unsigned char i;
	for (i=0; i<4; i++)
		IapProgramByte(addr+i,int32>>(8*(3-i)));
	return addr+i;
}

void IapEraseSector(unsigned int addr)
{
	IAP_CONTR=ENABLE_IAP;
	IAP_CMD=CMD_ERASE;
	IAP_ADDRL=addr+IAP_ADDRESS;
	IAP_ADDRH=(addr+IAP_ADDRESS)>>8;
	IAP_TRIG=0x5A;
	IAP_TRIG=0xA5;
	_nop_();
	IapIdle();
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
		case KEY4:
			mode=SETTING;
			break;
		case KEY3:
			mode=NETWORKING;
			break;
		default:
			break;
	}
}

void ProcKey_Setting(unsigned char key)
{
	switch (key)
	{
		case KEY4:
			mode=NETWORKING;
			break;
		default:
			break;
	}
}

// UART Function
void UART1_Init()	// 38400
{
	SCON=0x50;
	TH1=TL1=0xFD;
	TR1=1;
	ES=1;
}

void UART1_SendByte(unsigned char byte)
{
	SBUF=byte;
	while (TI==0);
	TI=0;
}

void UART1_SendIntString(unsigned long number)
{
	while (number/10!=0)
	{
		UART1_SendByte(number%10+0x30);
		number/=10;
	}
	UART1_SendByte(number+0x30);
}

void UART1_SendInt16(unsigned int number)
{
	UART1_SendByte(number>>8);
	UART1_SendByte(number);
}

void UART1_SendString(unsigned char* str)
{
	unsigned int i=0;
	while (str[i]!='\0')
	{
		UART1_SendByte(str[i]);
		i++;
	}
}

void UART2_Init()	// 38400
{
	S2CON=0x50;
	BRT=0xF7;
	IE2=0x01;
}

void UART2_SendByte(unsigned char byte)
{
	S2BUF=byte;
	while ((S2CON&0x02)==0);
	S2CON&=0xFD;
}

void UART2_Start(unsigned char recv)
{
	unsigned char i;
	switch (receiveDataCount)
	{
		case 4:
			for (i=4; i<address; i+=5)
			{
				if (IapReadByte(i)==recv)
				{
					index=(i-4)/5;
					break;
				}
			}
			break;
		case 5:
			concentration[index]=recv;
			concentration[index]<<=8;
			break;
		case 6:
			concentration[index]+=recv;
			receiveDataCount=0;
			break;
	}
}

void UART2_Networking(unsigned char recv)
{
	unsigned char i;
	switch (receiveDataCount)
	{
		case 4:
			for (i=4; i<address; i+=5)
			{
				if (IapReadByte(i)==recv)
				{
					concentration[(i-4)/5]=1;
					break;
				}
			}
			receiveDataCount=0;
			break;
		default:
			break;
	}
}

// Timer Function
void Timer0_Init()	// 10 ms timer
{
	TL0=(65536-10000)%256;
	TH0=(65536-10000)/256;
	ET0=0x01;
}

void Timer0_Start()
{
	TR0=1;
}

// Zigbee Function
void Zigbee_Reset()
{
	UART2_SendByte(0xFE);
	UART2_SendByte(0x03);
	UART2_SendByte(0x26);
	UART2_SendByte(0x05);
	UART2_SendByte(0x03);
	UART2_SendByte(0x01);
	UART2_SendByte(0x03);
	UART2_SendByte(0x21);
}

void Zigbee_Restart()
{
	UART2_SendByte(0xFE);
	UART2_SendByte(0x01);
	UART2_SendByte(0x41);
	UART2_SendByte(0x00);
	UART2_SendByte(0x01);
	UART2_SendByte(0x41);
}

// State Function
void WaitState()
{
	unsigned char keyValue,keyTempValue,keyStatus,wait;
	keyValue=keyTempValue=0;
	keyStatus=KEY_SEARCH;

	wait=0;
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
					ProcKey_Waiting(keyValue);
					keyStatus=KEY_SEARCH;
				}
				break;
		}
		if (++wait>=200)
			mode=START;
	}
}

void SettingState()
{
	unsigned char keyValue,keyTempValue,keyStatus;
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
					ProcKey_Setting(keyValue);
					keyStatus=KEY_SEARCH;
				}
				break;
		}
	}
	IapProgramInt32(0,address);
}

void NetworkingState()
{
	unsigned char i;
	unsigned char total;
	unsigned char all;

	address=IapReadInt32(0);
	all=(address-4)/5;

	// Initialize Zigbee
	Zigbee_Reset();
	Zigbee_Restart();

	while (mode==NETWORKING)
	{
		total=0;
		Delay_ms(90);
		for (i=0; i<(address-4)/5; i++)
			if (concentration[i])
				++total;
		if (total==all)
		{
			UART2_SendByte(0x00);
			mode=START;
		}
	}
}

// SIM Function
void SetLocalPort()
{
	UART1_SendString("AT+CLPORT=\"TCP\",\"2022\"\r");
}

void Connect()
{
	// Delay_ms(100);
	UART1_SendString("AT+CIPSTART=\"TCP\",\"42.121.120.41\",\"5050\"\r");
}

void SendStart()
{
	UART1_SendString("AT+CIPSEND\r");
}

void SendEnd()
{
	UART1_SendByte(0x1A);
}

void ShutDown()
{
	UART1_SendString("AT+CIPCLOSE\r");
	Delay_ms(5000);
	UART1_SendString("AT+CIPSHUT\r");
}

/*------------------------------Main Part------------------------------*/
void main()
{
	unsigned char i;
	// Hardware, Register Initialize
	// T0 used as 16 bit timer
	// T1 used as 8 bit auto reload for baurd rate generator
	TMOD=0x21;
	// Use 1T timer1, independent baurd rate generator
	AUXR=0x14;
	// Initialize
	UART1_Init();
	UART2_Init();
	Timer0_Init();
	// Enable All interrupt
	EA=1;
	// Enable LED
	LED_OPEN=0;

	// Initialize Variables
	queryCount=0;
	mode=WAITING;

	P0=0;
	// Wait State
	WaitState();

	switch (mode)
	{
		case SETTING:
			IapEraseSector(0);
			address=4;			// keep the first 4 bytes to save address
			P0=~(0x01);
			SettingState();
			P0=~(0x40);
			NetworkingState();
			break;
		case NETWORKING:
			P0=~(0x40);
			NetworkingState();
			break;
		case START:
			address=IapReadInt32(0);
			break;
	}

	queryMode=GET;
	// Stop UART1 Receive
	SCON=0x40;
	ES=0;

	SetLocalPort();
	Timer0_Start();
	P0=~(0x80);
	while (1);
}

/*------------------------------IRQ Part------------------------------*/
void Timer0_IRQ() interrupt 1
{
	unsigned char i;
	unsigned char length;
	TL0=(65536-10000)%256;
	TH0=(65536-10000)/256;
	++queryCount;
	if (queryCount==6000)
	{
		switch (queryMode)
		{
			case GET:
				receiveDataCount=0;
				for (i=0; i<20; ++i)
					concentration[i]=0;
				UART2_SendByte(0x01);
				queryMode=SEND;
				P0=~(0xC0);
				break;
			case SEND:
				// Connect to Server
				Connect();
				Delay_ms(5000);
				// Send Data
				SendStart();
				length=(address-4)/5;
				for (i=0; i<length; i++)
					if (concentration[i])
					{
						UART1_SendIntString(IapReadInt32(i*5+5));
						UART1_SendByte(':');
						UART1_SendIntString(concentration[i]);
						UART1_SendByte(' ');
					}
				UART1_SendByte(';');
				SendEnd();
				Delay_ms(5000);
				// Close Connection and Shutdown TCP
				ShutDown();
				queryMode=GET;
				P0=~(0x80);
				break;
		}
		queryCount=0;
	}
}

// Receive Setting
void UART1_IRQ() interrupt 4
{
	unsigned char recv;
	LED_OPEN=1;
	if (RI)
	{
		RI=0;
		recv=SBUF;
		switch (mode)
		{
			case SETTING:
				IapProgramByte(address,recv);
				++address;
				break;
		}
	}
	LED_OPEN=0;
}

// Get data from Zigbee
void UART2_IRQ() interrupt 8
{
	unsigned char recv;
	if (S2CON & 0x01)
	{
		S2CON&=~(0x01);
		recv=S2BUF;
		switch (mode)
		{
			case NETWORKING:
				++receiveDataCount;
				UART2_Networking(recv);
				break;
			case START:
				++receiveDataCount;
				UART2_Start(recv);
				break;
		}
	}
}
