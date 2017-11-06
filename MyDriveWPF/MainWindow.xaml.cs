using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
        ServiceReference1.User user = new ServiceReference1.User()
        {
            Login="User1"
        };

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
            this.Hide();

            base_address = @"D:\UserFolder\" + user.Login;
            current_path = base_address;

            All = InitializeListView(current_path);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string str = "NewFolder";
            Directory.CreateDirectory(current_path + "\\" + str);
            All = InitializeListView(Current_path);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string path = listView.SelectedItem.ToString();
            if (Directory.Exists(path)){
                All = InitializeListView(path);
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
                return all;
               
            }
            catch
            {
                return null;
            }
        }

  
    }
}
