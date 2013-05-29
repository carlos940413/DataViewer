using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataViewer_Entity;
using DataViewer_ConfigureTool.PopupDialog;
using System.IO.Ports;


namespace DataViewer_ConfigureTool
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			#region Initialize ComboBox
			Project_ComboBox.ItemsSource = Project.Get_ByState(false);
			Port_ComboBox.ItemsSource = SerialPort.GetPortNames();
			Port_ComboBox.SelectedIndex = 0;
			Project_ComboBox.SelectedIndex = 0;
			#endregion
		}

		private void On_AreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Area_ComboBox.SelectedIndex >= 0)
			{
				Node_DataGrid.ItemsSource = Node.Get_ByAreaID((Area_ComboBox.SelectedItem as Area).ID);
				AddNode_Button.IsEnabled = true;
				Register_Button.IsEnabled = true;
				DeleteNode_Button.IsEnabled = true;
			}
			else
			{
				Node_DataGrid.ItemsSource = null;
				AddNode_Button.IsEnabled = false;
				Register_Button.IsEnabled = false;
				DeleteNode_Button.IsEnabled = false;
			}
		}

		private void On_AddNodeButton_Click(object sender, RoutedEventArgs e)
		{
			AddNodeDialog dialog = new AddNodeDialog(Area_ComboBox.SelectedItem as Area);
			dialog.Owner = this;
			dialog.ShowDialog();
			Node_DataGrid.ItemsSource = Node.Get_ByAreaID((Area_ComboBox.SelectedItem as Project).ID);
		}

		private void On_DeleteNodeButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (Node node in Node_DataGrid.SelectedItems)
			{
				node.Delete();
			}
			Node_DataGrid.ItemsSource = Node.Get_ByAreaID((Area_ComboBox.SelectedItem as Project).ID);
		}

		private void On_RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			SerialPort port = new SerialPort(Port_ComboBox.SelectedItem.ToString(), 9600, Parity.None, 8, StopBits.One);
			try
			{
				port.Open();
				foreach (Node node in Node_DataGrid.Items)
				{
					byte[] node_information = new byte[5];
					node_information[0] = Convert.ToByte(node.HardwareID);
					byte[] nodeID = BitConverter.GetBytes(node.ID).Reverse<byte>().ToArray<byte>();
					for (int i = 0; i < 4; i++)
					{
						node_information[i + 1] = nodeID[i];
					}
					port.Write(node_information, 0, node_information.Length);
				}
				MessageBox.Show("注册成功");
			}
			catch (Exception)
			{
				MessageBox.Show("端口无法打开!");
			}
			finally
			{
				port.Close();
			}
		}

		private void On_PojectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Project_ComboBox.SelectedIndex >= 0)
			{
				Area_ComboBox.ItemsSource = Area.Get_ByProjectID((Project_ComboBox.SelectedItem as Project).ID);
				Area_ComboBox.SelectedIndex = 0;
			}
			else
			{
				Area_ComboBox.ItemsSource = null;
				Area_ComboBox.SelectedIndex = -1;
			}
		}

		private void On_NewAreaButton_Click(object sender, RoutedEventArgs e)
		{
			AddAreaDialog dialog = new AddAreaDialog(Project_ComboBox.SelectedItem as Project);
			dialog.Owner = this;
			dialog.ShowDialog();
			Area_ComboBox.ItemsSource = Area.Get_ByProjectID((Project_ComboBox.SelectedItem as Project).ID);
			Area_ComboBox.SelectedIndex = 0;
		}
	}
}
