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
    public partial class Form1 : Form
    {
        MyDriveService.User user = new MyDriveService.User();
        MyDriveService.StorrageServiceClient clientStorrage = new MyDriveService.StorrageServiceClient();

        public MyDriveService.User _User
        {
            get { return this.user; }
            set { this.user = value; }
        }
        public Form1()
        {
            InitializeComponent();

            new EnterForm(this).ShowDialog();
            this.Hide();
            InitializeTreeView();
        }

        private void InitializeTreeView()
        {
            List<TreeNode> nodes = new List<TreeNode>();
            clientStorrage.ReadAll("root").Files.Select(file => file.Name).ToList().ForEach(item => nodes.Add(new TreeNode() { Text = item }));
            treeView1.Nodes.AddRange(nodes.ToArray());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
