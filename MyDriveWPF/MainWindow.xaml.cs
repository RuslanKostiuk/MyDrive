using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Login="Test3"
        }
            ;
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
            base_address = @"C:\Users\Ruslanchik\Desktop\DriveRepositiry\" + user.Login;
            current_path = base_address;

            All = InitializeListView(current_path);
            Directories = InitializeTreeView(current_path);
        }

        private List<string> InitializeTreeView(string path)
        {
       
            try
            {
               return clientStorrage.OpenFolder(path).Files.Select(file => file.Name).ToList();
 
            }
            catch { return null; }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string str = "TestCreateFolder1";
            clientStorrage.Create(null, Current_path + "/" + str);
            All = InitializeListView(Current_path);
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Current_path = listView.SelectedItem.ToString();
            All = InitializeListView(current_path);
        }

        private List<string> InitializeListView(string path)
        {

            try
            {
                return clientStorrage.ReadAll(path).Files.Select(file => file.Name).ToList();
               
            }
            catch
            {
                return null;
            }
        }
    }
}
