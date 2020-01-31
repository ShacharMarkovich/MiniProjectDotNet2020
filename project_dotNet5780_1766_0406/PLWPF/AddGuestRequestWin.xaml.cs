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

        /// <summary>
        /// c'tor
        /// </summary>
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

            //set data contexts:
            SetDataContext();
            SetGuestRequestComboBox();

            this.areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Area));
            this.typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.UnitType));
            areaComboBox.SelectedIndex = 0;
            typeComboBox.SelectedIndex = 0;

            clearButton.Click += ClearButton_Click;
            closeButton.Click += CloseButtonClick;
        }

        /// <summary>
        /// Connects _guestRequest to the variables on screen,
        /// and make them show _guestRequest properties
        /// </summary>
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

        /// <summary>
        /// Connects _guestRequestComboBox to data in data source,
        /// </summary>
        private void SetGuestRequestComboBox()
        {
            // get GuestRequests to string
            List<BE.GuestRequest> guestRequestlst = _bl.AccordingTo(gR => !_bl.IsGuestRequestClose(gR));
            List<string> hostsNames = (from gR in guestRequestlst
                                       select $"({gR.GuestRequestKey}){gR.PrivateName} {gR.FamilyName}").ToList();

            // we change the selected index,
            // but we don't want the event to start,
            // so we rmove it and add it :)
            guestRequestComboBox.SelectionChanged -= GuestRequestComboBox_SelectionChanged;
            guestRequestComboBox.ItemsSource = hostsNames;
            guestRequestComboBox.SelectedIndex = -1;
            guestRequestComboBox.SelectionChanged += GuestRequestComboBox_SelectionChanged;
        }

        #region Buttons Click events

        /// <summary>
        /// Add GuestRequest event.
        /// check if legal and add to data source
        /// </summary>
        private void AddGuestRequestButton_Click(object sender, RoutedEventArgs e)
        {
            _guestRequest.Area = (BE.Enums.Area)areaComboBox.SelectedIndex;
            _guestRequest.type = (BE.Enums.UnitType)typeComboBox.SelectedIndex;
            try
            {
                if (areaComboBox.SelectedIndex == -1 || typeComboBox.SelectedIndex == -1)
                    throw new ArgumentException("please choose area and type!");

                int people = 0;
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
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = exp.Message;
                errorMessage.Fade();
                return;
            }
            catch (Exception exp)
            {
                // show fit comment
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = exp.Message;
                errorMessage.Fade();
                return;
            }

            // show fit comment
            errorMessage.Foreground = Brushes.Green;
            errorMessage.Text = "Guest Request Add successfully!";
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
            SetGuestRequestComboBox();
        }

        /// <summary>
        /// update GuestRequest event.
        /// check if the new properties are legal and update in data source
        /// </summary>
        private void UpdateGuestRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (guestRequestComboBox.SelectedIndex == -1)
            {
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = "Selecet GuestRequest first";
                errorMessage.Fade();
                return;
            }
            _guestRequest.Area = (BE.Enums.Area)areaComboBox.SelectedIndex;
            _guestRequest.type = (BE.Enums.UnitType)typeComboBox.SelectedIndex;
            try
            {
              
                int people = 0;
                if (int.TryParse(this.adultsTextBox.Text, out people))
                    _guestRequest.Adults = people;
                else
                    throw new ArgumentException("please enter a number in Adults field!");

                if (int.TryParse(this.childrenTextBox.Text, out people))
                    _guestRequest.Children = people;
                else
                    throw new ArgumentException("please enter a NUMBER in Children field!");

                if (areaComboBox.SelectedIndex == -1 || typeComboBox.SelectedIndex == -1)
                    throw new ArgumentException("please choose area and type!");

                _bl.UpdateGuestRequest(_guestRequest);
            }
            catch (ArgumentException exp)
            {
                // show fit comment
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = exp.Message;
                errorMessage.Fade();
                return;
            }
            catch (Exception exp)
            {
                // show fit comment
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = exp.Message;
                errorMessage.Fade();
                return;
            }

            // show fit comment
            errorMessage.Foreground = Brushes.Green;
            errorMessage.Text = "Guest Request Update successfully!";
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
            SetGuestRequestComboBox();
        }

        /// <summary>
        /// close GuestRequest event.
        /// close exists GuestRequest.
        /// if there are open orders - close them too
        /// </summary>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (guestRequestComboBox.SelectedIndex == -1)
            {
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = "Selecet GuestRequest first";
                errorMessage.Fade();
                return;
            }
            try
            {
                _bl.UpdateGuestRequest(_guestRequest, BE.Enums.Status.CloseByClient);
            }
            catch (ArgumentException exp)
            {
                // show fit comment
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = exp.Message;
                errorMessage.Fade();
                return;
            }

            // show fit comment
            errorMessage.Foreground = Brushes.Green;
            errorMessage.Text = "Guest Request Closed.";
            errorMessage.Fade();

            _guestRequest = new BE.GuestRequest
            {
                GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                RegistrationDate = DateTime.Now,
                EntryDate = DateTime.Now,
                ReleaseDate = DateTime.Now,
                Stat = BE.Enums.Status.NotYetApproved
            };
            SetDataContext();
            SetGuestRequestComboBox();
            areaComboBox.SelectedIndex = -1;
            typeComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// clear scrren
        /// </summary>
        public void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _guestRequest = new BE.GuestRequest
            {
                GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                RegistrationDate = DateTime.Now,
                EntryDate = DateTime.Now,
                ReleaseDate = DateTime.Now,
                Stat = BE.Enums.Status.NotYetApproved
            };
            SetDataContext();
            SetGuestRequestComboBox();
            areaComboBox.SelectedIndex = -1;
            typeComboBox.SelectedIndex = -1;
        }
        #endregion 

        /// <summary>
        /// load the chose GuestRequest to screen
        /// </summary>
        private void GuestRequestComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the selected guestRequestkey
            // format of guestRequestComboBox items: <(guestRequestNameKey)><guestRequest Full Name>
            string guestRequestName = guestRequestComboBox.SelectedItem as string;
            // so in the splited string's second place will be the guestRequestKey
            double guestRequestKey;
            try
            {
                guestRequestKey = double.Parse(guestRequestName.Split('(', ')')[1]);
            }
            catch
            {
                errorMessage.Foreground = Brushes.Red;
                errorMessage.Text = "please choose a guest Request first";
                errorMessage.Fade();
                return;
            }

            // and no update _guestRequest to selected
            List<BE.GuestRequest> guestRequestlst = _bl.AccordingTo(delegate (BE.GuestRequest gR)
            {
                return gR.GuestRequestKey == guestRequestKey;
            });

            if (guestRequestlst.Count() != 1)
                throw new ArgumentOutOfRangeException("More than one hosting unit with same key");

            // show the selected hoting unit on screen
            _guestRequest = guestRequestlst.First();
            SetDataContext();
            areaComboBox.SelectedItem = _guestRequest.Area;
            typeComboBox.SelectedItem = _guestRequest.type;
        }
    }
}
