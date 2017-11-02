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
    public partial class EnterForm : Form
    {
        MyDriveService.AccessServiceClient user = new MyDriveService.AccessServiceClient();
        Form1 mainForm;
        MyDriveService.AnswerUserResponse answer = new MyDriveService.AnswerUserResponse()
        {
            Code = MyDriveService.AnswerCode.Failed
        };
        public EnterForm()
        {
            InitializeComponent();
        }

        public EnterForm(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            answer = user.UserAuth(loginBox.Text, passwordBox.Text);
            if (answer.Code == MyDriveService.AnswerCode.Failed)
            {
                MessageBox.Show(answer.Message);
            }
            else
            {
                mainForm._User = answer._User;
                mainForm.Show();
                this.Close();
            }

        }

        private void EnterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (answer.Code != MyDriveService.AnswerCode.Complete) mainForm.Dispose();
        }

        private void registBox_Click(object sender, EventArgs e)
        {
            new RegistrationForm(mainForm).ShowDialog();
            this.Close();
        }
    }
}
