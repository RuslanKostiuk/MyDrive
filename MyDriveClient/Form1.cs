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

        string base_address;
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
            base_address = @"root/" + user.Login;
           treeView1.Nodes.AddRange(InitializeTreeView(base_address).ToArray());
        }

        private List<TreeNode> InitializeTreeView(string path)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            clientStorrage.ReadAll(path).Files.Select(file => file.Name).ToList().ForEach(item => nodes.Add(new TreeNode() { Text = item }));
            return nodes;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.Nodes.AddRange(InitializeTreeView(e.Node.FullPath).ToArray());
        }
    }
}
