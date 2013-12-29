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
using System.Windows.Shapes;

namespace Blood_Manager
{
    /// <summary>
    /// Interaction logic for NewEntryWindow.xaml
    /// </summary>
    public partial class NewEntryWindow : Window
    {
        public NewEntryWindow()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Hide();

            Person personToAdd = new Person();
            personToAdd.Surname = surnameTxtBox.Text;
            personToAdd.Forename = forenameTxtBox.Text;
            personToAdd.BloodGroup = bloodGroupTxtBox.Text;
            personToAdd.RhD = rhdTxtBox.Text;
            personToAdd.Address = addressTxtBox.Text;
            personToAdd.Phone = phoneTxtBox.Text;
            personToAdd.Mobile = mobileTxtBox.Text;

            Pages.LocalModeMain.personFromAddDialog = personToAdd;
        }
    }
}
