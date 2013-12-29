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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Blood_Manager.Pages
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class LocalModeMain : Page
    {
        public LocalModeMain()
        {
            InitializeComponent();
        }

        // Exit and minimize button control
        private void exitApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void minimizeApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }
        
        // GLOBALS
        string fileToLoad;
        List<Person> people = new List<Person>();
        XmlDocument xDoc = new XmlDocument();
        

        public static Person personFromAddDialog
        {
            get;
            set;
        }

        // GENERAL METHODS
        private void clearTextBoxes(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                if (obj is TextBox)
                    ((TextBox)obj).Text = null;
                clearTextBoxes(VisualTreeHelper.GetChild(obj, i));
            }
        }

        private void changeReadOnly(bool readOnly)
        {
            LoopControls ccChildren = new LoopControls();

            foreach (object obj in ccChildren.GetChildren(this, 5))
            {
                if (obj.GetType() == typeof(TextBox))
                {
                    TextBox txtBox = (TextBox)obj;
                    txtBox.IsReadOnly = readOnly;
                }
            }
        }

        // TRIGGERS
        private void loadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            listBoxNames.Items.Clear(); // clear list before hand
            clearTextBoxes(this);

            Microsoft.Win32.OpenFileDialog openFD = new Microsoft.Win32.OpenFileDialog();
            openFD.DefaultExt = ".xml";
            openFD.Filter = "XML Document (.xml)|*.xml";

            Nullable<bool> result = openFD.ShowDialog(); 
            if (result == true) // check a file was selected
            {
                fileToLoad = openFD.FileName;
                xDoc.Load(fileToLoad);
            }

            foreach (XmlNode xNode in xDoc.SelectNodes("People/Person"))
            {
                Person p = new Person();
                p.Surname = xNode.SelectSingleNode("Surname").InnerText;
                p.Forename = xNode.SelectSingleNode("Forename").InnerText;
                p.BloodGroup = xNode.SelectSingleNode("BloodGroup").InnerText;
                p.RhD = xNode.SelectSingleNode("RhD").InnerText;
                p.Address = xNode.SelectSingleNode("Address").InnerText;
                p.Phone = xNode.SelectSingleNode("Phone").InnerText;
                p.Mobile = xNode.SelectSingleNode("Mobile").InnerText;
                people.Add(p);

                string fullName = p.Forename + " " + p.Surname;
                listBoxNames.Items.Add(fullName);
            }
        }

        private void listBoxNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listBoxNames.SelectedIndex != -1) // prevents selected index error
                {
                    surnameTxtBox.Text = people[listBoxNames.SelectedIndex].Surname;
                    forenameTxtBox.Text = people[listBoxNames.SelectedIndex].Forename;
                    bloodGroupTxtBox.Text = people[listBoxNames.SelectedIndex].BloodGroup;
                    rhdTxtBox.Text = people[listBoxNames.SelectedIndex].RhD;
                    addressTxtBox.Text = people[listBoxNames.SelectedIndex].Address;
                    phoneTxtBox.Text = people[listBoxNames.SelectedIndex].Phone;
                    mobileTxtBox.Text = people[listBoxNames.SelectedIndex].Mobile;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void newEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            NewEntryWindow newEntry = new NewEntryWindow();
            newEntry.ShowDialog();

            if (newEntry.DialogResult.HasValue && newEntry.DialogResult.Value)
            {
                people.Add(personFromAddDialog);
                string fullName = personFromAddDialog.Forename + " " + personFromAddDialog.Surname;
                listBoxNames.Items.Add(fullName);
                MessageBox.Show("New entry successfully added.", "Successfully Added", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void editEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            changeReadOnly(false);
        }

    }
}
