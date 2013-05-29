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

        /// <summary>
        /// 区域名称下拉菜单有选中项时，更新硬件节点Datagrid，添加、注册、删除节点按钮可用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 添加节点，弹出添加采集节点对话框，刷新节点列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void On_AddNodeButton_Click(object sender, RoutedEventArgs e)
		{
			AddNodeDialog dialog = new AddNodeDialog(Area_ComboBox.SelectedItem as Area);
			dialog.Owner = this;
			dialog.ShowDialog();
			Node_DataGrid.ItemsSource = Node.Get_ByAreaID((Area_ComboBox.SelectedItem as Project).ID);
		}

        /// <summary>
        /// 删除节点，刷新节点列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void On_DeleteNodeButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (Node node in Node_DataGrid.SelectedItems)
			{
				node.Delete();
			}
			Node_DataGrid.ItemsSource = Node.Get_ByAreaID((Area_ComboBox.SelectedItem as Project).ID);
		}

        /// <summary>
        /// 注册节点，打开端口，把所有节点ID写入端口。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 工程名称下拉菜单有选中项时，更新区域名称下拉菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 新建区域，弹出新建区域对话框，刷新区域名称下拉菜单。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
