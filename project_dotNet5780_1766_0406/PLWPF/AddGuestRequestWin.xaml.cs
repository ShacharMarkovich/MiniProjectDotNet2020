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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddGuestRequestWin.xaml
    /// </summary>
    public partial class AddGuestRequestWin : Window
    {
        public BL.IBL _bl = BL.FactoryBL.Instance;
        public AddGuestRequestWin()
        {
            InitializeComponent();

            //data blinding
            this.guestRequestKeyTextBlock.Text = (BE.Configuration.GuestRequestKey + 1).ToString();
            registrationDateDatePicker.SelectedDate = DateTime.Now;
            this.areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Area));
            this.statComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Status));
            this.typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.UnitType));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // guestRequestViewSource.Source = [generic data source]
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BE.GuestRequest gR = new BE.GuestRequest();
            // convert comboBox value to maych enum
            Enum.TryParse(areaComboBox.Text, out BE.Enums.Area area);
            Enum.TryParse(statComboBox.Text, out BE.Enums.Status status);
            Enum.TryParse(typeComboBox.Text, out BE.Enums.UnitType unitType);

            gR.GuestRequestKey = int.Parse(guestRequestKeyTextBlock.Text);

            gR.RegistrationDate = (DateTime)registrationDateDatePicker.SelectedDate;
            gR.EntryDate = (DateTime)entryDateDatePicker.SelectedDate;
            gR.ReleaseDate = (DateTime)releaseDateDatePicker.SelectedDate;
            try
            {
            gR.Adults = int.Parse(this.adultsTextBox.Text);
            gR.Children = int.Parse(this.childrenTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("number of children/adults must be a NUMBER");
            }

            gR.ChildrenAttractions = (bool)this.childrenAttractionsCheckBox.IsChecked;
            gR.Garden = (bool)this.gardenCheckBox.IsChecked;
            gR.Jecuzzi = (bool)this.jecuzziCheckBox.IsChecked;
            gR.Pool = (bool)this.poolCheckBox.IsChecked;

            try
            {
                _bl.AddGuestRequest(gR);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
    }
}
