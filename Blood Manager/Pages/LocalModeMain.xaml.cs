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
using System.IO;

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
            appDirectory = AppDomain.CurrentDomain.BaseDirectory;
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
        string fileToLoad, appDirectory;
        bool hasLoadedFile = false;
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
                hasLoadedFile = true;
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
                p.MedicalNotes = xNode.SelectSingleNode("MedicalNotes").InnerText;
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
                    medicalNotesTxtBox.Text = people[listBoxNames.SelectedIndex].MedicalNotes;
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
            if (listBoxNames.Items.Count == 0)
            {
                MessageBox.Show("You haven't loaded a file and/or there are no entries to perfrom changes to.", "Invalid Request", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (listBoxNames.SelectedIndex == -1)
            {
                MessageBox.Show("You haven't selected any items.", "No Item Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                changeReadOnly(false);
            }
        }

        private void deleteEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxNames.SelectedIndex != -1)
                {
                    people.RemoveAt(listBoxNames.SelectedIndex);
                    listBoxNames.Items.Remove(listBoxNames.SelectedItems[0]);
                }
                else if (listBoxNames.SelectedIndex == -1)
                {
                    MessageBox.Show("You have no entry selected.", "No Entry Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void saveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxNames.Items.Count == 0)
            {
                MessageBox.Show("You haven't loaded a file and/or there are no entries to perfrom changes to.", "Invalid Request", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                changeReadOnly(true);

                people[listBoxNames.SelectedIndex].Surname = surnameTxtBox.Text;
                people[listBoxNames.SelectedIndex].Forename = forenameTxtBox.Text;
                people[listBoxNames.SelectedIndex].BloodGroup = bloodGroupTxtBox.Text;
                people[listBoxNames.SelectedIndex].RhD = rhdTxtBox.Text;
                people[listBoxNames.SelectedIndex].Address = addressTxtBox.Text;
                people[listBoxNames.SelectedIndex].Phone = phoneTxtBox.Text;
                people[listBoxNames.SelectedIndex].Mobile = mobileTxtBox.Text;
                people[listBoxNames.SelectedIndex].MedicalNotes = medicalNotesTxtBox.Text;

                listBoxNames.Items[listBoxNames.SelectedIndex] = forenameTxtBox.Text + " " + surnameTxtBox.Text;
            }
        }

        private void saveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (hasLoadedFile == true)
            {
                XmlNode xNode = xDoc.SelectSingleNode("People");
                xNode.RemoveAll(); // remove existing content

                foreach (Person p in people)
                {
                    XmlNode xTop = xDoc.CreateElement("Person");
                    XmlNode xSurname = xDoc.CreateElement("Surname");
                    XmlNode xForename = xDoc.CreateElement("Forename");
                    XmlNode xBloodGroup = xDoc.CreateElement("BloodGroup");
                    XmlNode xRhD = xDoc.CreateElement("RhD");
                    XmlNode xAddress = xDoc.CreateElement("Address");
                    XmlNode xPhone = xDoc.CreateElement("Phone");
                    XmlNode xMobile = xDoc.CreateElement("Mobile");
                    XmlNode xMedicalNotes = xDoc.CreateElement("MedicalNotes");

                    xSurname.InnerText = p.Surname;
                    xForename.InnerText = p.Forename;
                    xBloodGroup.InnerText = p.BloodGroup;
                    xRhD.InnerText = p.RhD;
                    xAddress.InnerText = p.Address;
                    xPhone.InnerText = p.Phone;
                    xMobile.InnerText = p.Mobile;
                    xMedicalNotes.InnerText = p.MedicalNotes;

                    xTop.AppendChild(xSurname);
                    xTop.AppendChild(xForename);
                    xTop.AppendChild(xBloodGroup);
                    xTop.AppendChild(xRhD);
                    xTop.AppendChild(xAddress);
                    xTop.AppendChild(xPhone);
                    xTop.AppendChild(xMobile);
                    xTop.AppendChild(xMedicalNotes);
                    xDoc.DocumentElement.AppendChild(xTop);
                }

                xDoc.Save(fileToLoad);
            }
            else if (hasLoadedFile == false)
            {
                if (File.Exists(appDirectory + "Databases\\") == false)
                {
                    Directory.CreateDirectory(appDirectory + "Databases\\");
                    string filePath = appDirectory + "Databases\\bloodBank.xml";

                    // create and write xml doc
                    XmlTextWriter xWrite = new XmlTextWriter(filePath, Encoding.UTF8);
                    xWrite.WriteStartElement("People");
                    xWrite.WriteEndElement();
                    xWrite.Close();

                    XmlDocument xDocToWrite = new XmlDocument();
                    xDocToWrite.Load(filePath);

                    foreach (Person p in people)
                    {
                        XmlNode xTop = xDocToWrite.CreateElement("Person");
                        XmlNode xSurname = xDocToWrite.CreateElement("Surname");
                        XmlNode xForename = xDocToWrite.CreateElement("Forename");
                        XmlNode xBloodGroup = xDocToWrite.CreateElement("BloodGroup");
                        XmlNode xRhD = xDocToWrite.CreateElement("RhD");
                        XmlNode xAddress = xDocToWrite.CreateElement("Address");
                        XmlNode xPhone = xDocToWrite.CreateElement("Phone");
                        XmlNode xMobile = xDocToWrite.CreateElement("Mobile");
                        XmlNode xMedicalNotes = xDocToWrite.CreateElement("MedicalNotes");

                        xSurname.InnerText = p.Surname;
                        xForename.InnerText = p.Forename;
                        xBloodGroup.InnerText = p.BloodGroup;
                        xRhD.InnerText = p.RhD;
                        xAddress.InnerText = p.Address;
                        xPhone.InnerText = p.Phone;
                        xMobile.InnerText = p.Mobile;
                        xMedicalNotes.InnerText = p.MedicalNotes;

                        xTop.AppendChild(xSurname);
                        xTop.AppendChild(xForename);
                        xTop.AppendChild(xBloodGroup);
                        xTop.AppendChild(xRhD);
                        xTop.AppendChild(xAddress);
                        xTop.AppendChild(xPhone);
                        xTop.AppendChild(xMobile);
                        xTop.AppendChild(xMedicalNotes);
                        xDocToWrite.DocumentElement.AppendChild(xTop);
                    }

                    xDocToWrite.Save(filePath);
                }
            }

        }

    }
}
