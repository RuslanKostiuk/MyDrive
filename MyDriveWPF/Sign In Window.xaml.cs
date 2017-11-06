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

namespace MyDriveWPF
{
    /// <summary>
    /// Логика взаимодействия для Sign_In_Window.xaml
    /// </summary>
    public partial class Sign_In_Window : Window
    {
        ServiceReference1.AccessServiceClient client = new ServiceReference1.AccessServiceClient();
        MainWindow mainWindow;
        public Sign_In_Window(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Window1(mainWindow).ShowDialog();
           
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var userAnswer = client.UserRegistration(new ServiceReference1.User()
            {
                Login = loginBox.Text,
                Password = passwordBox.Password,
                Role = (roleBox.SelectedItem.ToString() == "Simple User") ?
                        ServiceReference1.Roles.simple : ServiceReference1.Roles.maximum
            });

            if (userAnswer.Code == ServiceReference1.AnswerCode.Complete)
            {
                mainWindow._User = userAnswer._User;
                this.Close();
            }
            else
            {
                MessageBox.Show(userAnswer.Message);
            }
        }
    }
}
