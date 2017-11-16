using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDriveWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ServiceReference1.User user = new ServiceReference1.User();

        FileSystemWatcher fw;

        public ServiceReference1.User _User
        {
            get { return user; }
            set { user = value; }
        }

        ServiceReference1.StorrageServiceClient clientStorrage = new ServiceReference1.StorrageServiceClient();
        string base_address, current_path;

        public string Current_path
        {
            get { return current_path; }
            set
            {
                current_path = value;
                OnPropertyChanged("Current_path");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        List<string> all,directories;

        public List<string> All
        {
            get { return all; }
            set
            {
                all = value;
                OnPropertyChanged("All");
            }
        }
        public List<string> Directories
        {
            get { return directories; }
            set
            {
                directories = value;
                OnPropertyChanged("Directories");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            new Window1(this).ShowDialog();                 

            base_address = @"D:\root\UserFolder\";

            Current_path = user.Login;
            if (!Directory.Exists(Current_path)) Directory.CreateDirectory(Current_path);

            SyncDirectory.Synchronize(base_address+Current_path, base_address);
            SyncFile.Synchronize(base_address+Current_path, base_address);

            fw = new FileSystemWatcher(base_address + Current_path);
            fw.Changed += Fw_Changed;
            fw.Created += Fw_Created;
            fw.Deleted += Fw_Deleted; ;
            fw.Renamed += Fw_Renamed;
            fw.IncludeSubdirectories = true;
            fw.EnableRaisingEvents = true;
             All = InitializeListView(base_address + Current_path);
           
        }

        private void Fw_Deleted(object sender, FileSystemEventArgs e)
        {

                clientStorrage.Delete(e.FullPath.Remove(0, base_address.Length));
            All = InitializeListView(base_address + Current_path);
        }

        private void Fw_Created(object sender, FileSystemEventArgs e)
        {
            if (Directory.Exists(e.FullPath))
            {
                clientStorrage.CreateFolder(e.FullPath.Remove(0, base_address.Length));
            }
            else
            {
                clientStorrage.Create(null,e.FullPath.Remove(0, base_address.Length));
            }
            All = InitializeListView(base_address + Current_path);
        }

        private void Fw_Renamed(object sender, RenamedEventArgs e)
        {
            clientStorrage.RenameFolder(e.OldFullPath.Remove(0,base_address.Length), e.FullPath.Remove(0, base_address.Length));
            All = InitializeListView(base_address + Current_path);
        }


        private void Fw_Changed(object sender, FileSystemEventArgs e)
        {
            bool b = true;
            while (b)
            {
                try
                {
                    if (File.Exists(e.FullPath))
                        clientStorrage.Update(File.ReadAllBytes(e.FullPath), e.FullPath.Remove(0, base_address.Length));
                    b = false;
                }
                catch { }
            }
            All = InitializeListView(base_address + Current_path);
        }

      

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string str = "NewFolder";
            Directory.CreateDirectory(base_address + Current_path + "\\" + str);

            clientStorrage.CreateFolder(current_path+"\\" + str);
            All = InitializeListView(base_address + Current_path);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                var path = base_address + current_path + "\\" + listView.SelectedItem.ToString();
                Directory.Move(path, base_address + current_path + "\\" +RenameBox.Text);
                clientStorrage.RenameFolder(current_path+"\\"+ listView.SelectedItem.ToString(), current_path + "\\" +RenameBox.Text);
                All = InitializeListView(base_address + Current_path);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listView.SelectedItem!=null)
            RenameBox.Text = listView.SelectedItem.ToString();
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            string str = "NewFile";
            File.Create(base_address + Current_path + "\\" + str);

            clientStorrage.Create(null, current_path + "\\" + str);
            All = InitializeListView(base_address + Current_path);
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = base_address + Current_path+"\\" + listView.SelectedItem.ToString();
                if (File.Exists(path))
                {
                    if (MessageBox.Show("are you sure you want delete this file?", "Delete file", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        File.Delete(path);
                    }
                }
                else
                {
                    if (MessageBox.Show("are you sure you want delete this directory?", "Delete directory", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Directory.Delete(path,true);
                    }
                }

                clientStorrage.Delete(path.Remove(0, base_address.Length));
                All = InitializeListView(base_address+Current_path);
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Chose File or Directory");
            }
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {

            if (Current_path.Length > this._User.Login.Length)
            {
                Current_path = System.IO.Path.GetDirectoryName(Current_path);
                All = InitializeListView(base_address + Current_path);
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string path = base_address + Current_path+"\\" + listView.SelectedItem.ToString();
            if (Directory.Exists(path)){
                All = InitializeListView(path);
                fw.Path = base_address + Current_path;
            }
            else
            {
                Process.Start(path);
            }
        }   

        private List<string> InitializeListView(string path)
        {

            try
            {
                var all = Directory.GetDirectories(path).ToList();
                all.AddRange(Directory.GetFiles(path));

                for(int i= 0; i < all.Count; i++)
                    all[i] = all[i].Split('\\').Last();

                Current_path = path.Remove(0,base_address.Length);
                return all;
               
            }
            catch
            {
                return null;
            }
        }



  
    }
}
