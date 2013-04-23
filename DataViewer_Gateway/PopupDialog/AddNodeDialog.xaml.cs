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

namespace DataViewer_ConfigureTool.PopupDialog
{
    /// <summary>
    /// AddNodeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddNodeDialog : Window
    {
		private Project project;

        public AddNodeDialog(int projectID)
        {
            InitializeComponent();

            #region Initialize Control State
            Confirm_Button.IsEnabled = false;
            #endregion

            project = Project.Get_ByID(projectID);
        }

        private void On_ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
			Node node = Node.Get_ByAreaID_HardwareID(project.ID, Int32.Parse(HardwareID_TextBox.Text));
			node.Project = project;
			node.HardwareID = Int32.Parse(HardwareID_TextBox.Text);
            node.Description = Description_TextBox.Text;
            node.Save();
            this.Close();
        }

        private void On_HardwareIDTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int hardwareID;
            if (!Int32.TryParse(HardwareID_TextBox.Text, out hardwareID))
                HardwareID_TextBox.Text = "";
        }

        private void On_HardwareIDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HardwareID_TextBox.Text == "")
                Confirm_Button.IsEnabled = false;
            else
                Confirm_Button.IsEnabled = true;
        }

        private void On_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
