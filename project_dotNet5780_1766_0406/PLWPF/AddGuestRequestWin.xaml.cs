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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
        }

        public BL.IBL _bl = BL.FactoryBL.Instance;
        public BE.GuestRequest _guestRequest;

        //data blinding
        private void SetDataContext()
        {
            entryDateDatePicker.DataContext = _guestRequest;
            releaseDateDatePicker.DataContext = _guestRequest;

            privateNameTextBox.DataContext = _guestRequest;
            familyNameTextBox.DataContext = _guestRequest;
            emailTextBox.DataContext = _guestRequest;

            areaComboBox.DataContext = _guestRequest;
            typeComboBox.DataContext = _guestRequest;

            adultsTextBox.DataContext = _guestRequest;
            childrenTextBox.DataContext = _guestRequest;

            gardenCheckBox.DataContext = _guestRequest;
            childrenAttractionsCheckBox.DataContext = _guestRequest;
            jecuzziCheckBox.DataContext = _guestRequest;
            poolCheckBox.DataContext = _guestRequest;

        }

        public AddGuestRequestWin()
        {
            InitializeComponent();
            _guestRequest = new BE.GuestRequest
            {
                GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                RegistrationDate = DateTime.Now,
                EntryDate = DateTime.Now,
                ReleaseDate = DateTime.Now,
                Stat = BE.Enums.Status.NotYetApproved
            };
            SetDataContext();

            // Default
            this.areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Area));
            this.typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.UnitType));
            areaComboBox.SelectedIndex = 0;
            typeComboBox.SelectedIndex = 0;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _guestRequest.Area = (BE.Enums.Area)areaComboBox.SelectedIndex;
            _guestRequest.type = (BE.Enums.UnitType)typeComboBox.SelectedIndex;
            try
            {
                int people= 0;
                if (int.TryParse(this.adultsTextBox.Text, out people))
                    _guestRequest.Adults = people;
                else
                    throw new ArgumentException("please enter a number in Adults field!");

                if (int.TryParse(this.childrenTextBox.Text, out people))
                    _guestRequest.Children = people;
                else
                    throw new ArgumentException("please enter a number in Adults field!");

                _bl.AddGuestRequest(_guestRequest);
            }
            catch (ArgumentException exp)
            {
                // show fit comment
                errorMessage.Foreground = new SolidColorBrush(Colors.Red);
                errorMessage.FontSize = 25;
                errorMessage.Text = exp.Message;
                errorMessage.Fade();
                return;
            }
            catch (Exception exp)
            {
                // show fit comment
                errorMessage.Foreground = new SolidColorBrush(Colors.Red);
                errorMessage.FontSize = 25;
                errorMessage.Text = exp.Message;
                errorMessage.Fade();
                return;
            }

            // show fit comment
            errorMessage.Foreground = new SolidColorBrush(Colors.Green);
            errorMessage.Text = "Guest Request Add successfully!";
            errorMessage.FontSize = 25;
            errorMessage.Fade();

            // prepare to get more new BE.GuestRequest
            _guestRequest = new BE.GuestRequest
            {
                GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                RegistrationDate = DateTime.Now,
                EntryDate = DateTime.Now,
                ReleaseDate = DateTime.Now,
                Stat = BE.Enums.Status.NotYetApproved
            };
            SetDataContext();
        }
    }
}
