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
using System.ServiceModel.Security;
using System.IdentityModel;

namespace MyDriveWPF
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MainWindow mainWindow;
        ServiceReference1.AccessServiceClient client = new ServiceReference1.AccessServiceClient();
        public Window1(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.ClientCredentials.UserName.UserName = loginBox.Text;
            client.ClientCredentials.UserName.Password = passwordBox.Password;
            client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            var userAnswer = client.UserAuth(loginBox.Text, passwordBox.Password);

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Sign_In_Window(mainWindow).ShowDialog();
            this.Close();
        }
    }
}
