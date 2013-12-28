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

        // MAIN EVENTS
        private void loadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFD = new Microsoft.Win32.OpenFileDialog();
            openFD.DefaultExt = ".xml";
            openFD.Filter = "XML Document (.xml)|*.xml";

            Nullable<bool> result = openFD.ShowDialog(); 
            if (result == true) // check a file was selected
            {
                fileToLoad = openFD.FileName;
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileToLoad);
            foreach (XmlNode xNode in xDoc.SelectNodes("People/Person"))
            {
                Person p = new Person();
                p.Surename = xNode.SelectSingleNode("Surname").InnerText;
                p.Forename = xNode.SelectSingleNode("Forename").InnerText;
                p.BloodGroup = xNode.SelectSingleNode("BloodGroup").InnerText;
                p.RhD = xNode.SelectSingleNode("RhD").InnerText;
                p.Address = xNode.SelectSingleNode("Address").InnerText;
                p.Phone = xNode.SelectSingleNode("Phone").InnerText;
                p.Mobile = xNode.SelectSingleNode("Mobile").InnerText;
                people.Add(p);

                string fullName = p.Forename + " " + p.Surename;
                listBoxNames.Items.Add(fullName);
            }
        }
    }
}
