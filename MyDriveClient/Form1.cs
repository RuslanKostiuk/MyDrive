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
        MyDriveService.AccessServiceClient clientAuth = new MyDriveService.AccessServiceClient();
        MyDriveService.StorrageServiceClient clientStorrage = new MyDriveService.StorrageServiceClient();
        public Form1()
        {
            InitializeComponent();
            List<TreeNode> nodes = new List<TreeNode>();
            clientStorrage.ReadAll("root").Files.Select(file => file.Name).ToList().ForEach(item => nodes.Add(new TreeNode() { Text = item}));
            treeView1.Nodes.AddRange(nodes.ToArray());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
