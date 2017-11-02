using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDriveClient
{
    public partial class RegistrationForm : Form
    {

        MyDriveService.AccessServiceClient user = new MyDriveService.AccessServiceClient();
        Form1 mainForm;
        MyDriveService.AnswerUserResponse answer = new MyDriveService.AnswerUserResponse()
        {
            Code = MyDriveService.AnswerCode.Failed
        };
        public RegistrationForm()
        {
            InitializeComponent();
        }

        public RegistrationForm(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }
        private void RegistrationForm_Load(object sender, EventArgs e)
        {

        }

        private void registBox_Click(object sender, EventArgs e)
        {
            var answer = user.UserRegistration(new MyDriveService.User()
            {
                Login = loginBox.Text,
                Password = passwordBox.Text,
                Role = (typeBox.Text == "Free") ? MyDriveService.Roles.trial : MyDriveService.Roles.maximum
            });

            if(answer.Code == MyDriveService.AnswerCode.Complete)
            {
                mainForm._User = answer._User;
                this.Close();
            }
            else
            {
                MessageBox.Show(answer.Message);               
            }
        }
    }
}
