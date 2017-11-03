using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        string base_address,current_path;
 

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
            base_address = @"C:\Users\Ruslanchik\Desktop\DriveRepositiry/" + user.Login;
            current_path = base_address;
           // base_address = @"root/" + user.Login;
            treeView1.Nodes.AddRange(InitializeTreeView(base_address).ToArray());
            listView1.Items.AddRange(InitializeListView(base_address).ToArray());
        }

        private List<TreeNode> InitializeTreeView(string path)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            try
            {
                clientStorrage.OpenFolder(path).Files.Select(file => file.Name).ToList().ForEach(item => nodes.Add(new TreeNode() { Text = item}));
                return nodes;
            }
            catch { return null; }
        }

        private List<ListViewItem> InitializeListView(string path)
        {
             List<ListViewItem> items = new List<ListViewItem>();
      
        
                //clientStorrage.ReadAsync(path).ContinueWith(callback =>
                //{

                //    callback.Result.Files.Select(file=>file.Name).ToList().ForEach(item => items.Add(new ListViewItem() { Text = item }));
                //    listView1.Items.AddRange(items.ToArray());
                //});

                try
                {
                    clientStorrage.ReadAll(path).Files.Select(file => file.Name).ToList().ForEach(item => items.Add(new ListViewItem() { Text =Path.GetFileName(item) }));
                    return items;
                }            
                catch{
                    return null;
                }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var folders = InitializeTreeView(e.Node.FullPath);

            if(folders!=null)
                e.Node.Nodes.AddRange(folders.ToArray());
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            current_path +="/"+ listView1.SelectedItems[0].Text;
            var items = InitializeListView(current_path);
            if (items != null)
            {
                listView1.Items.Clear();
                listView1.Items.AddRange(items.ToArray());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
