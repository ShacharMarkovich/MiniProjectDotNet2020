using System;
using System.Threading;
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
    /// Interaction logic for AddHostWin.xaml
    /// </summary>
    public partial class AddHostWin : Window
    {
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // guestRequestViewSource.Source = [generic data source]
        }

        #region class proprties
        const int maxBanks = 100;
        public BL.IBL _bl = BL.FactoryBL.Instance;
        public BE.Host _host;
        public BE.HostingUnit _hostingUnit;
        #endregion

        /// <summary>
        /// class c'tor
        /// </summary>
        public AddHostWin()
        {
            InitializeComponent();

            this.FontFamily = new FontFamily("Agency FB");
            this.FontSize = 18;
            // create new temp host
            // do it just in case that the user will add new host.
            //      if he do it - update _host to his new properties
            //      else - the user login to exists host so we will update to the exists host properties
            _host = new BE.Host()
            {
                HostKey = ++BE.Configuration.HostKey,
                Balance = 100,
                // TODO: in part 3, change to real bank
                BankBranchDetails = new BE.BankBranch()
            };

            // create temp BE.HostingUnit. because of the same reason as above
            _hostingUnit = new BE.HostingUnit
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host,
                DatesRange = new List<CalendarDateRange>()
            };

            SetSignUpDataContext();
            SetHostsComboBox(hostsComboBox);

            SetBanksComboBox(banksComboBox);
            SetBanksComboBox(detailsBanksComboBox);

            // make the comboBox contains the fit enum's opptions
            typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.UnitType));
            typeComboBox.SelectedIndex = 0;
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Area));
            areaComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// displays on the given ComboBox the names and keys of the hosting units according to the given term
        /// </summary>
        /// <param name="unitsComboBox_">hosting unit comboBox</param>
        /// <param name="term">delegate which get BE.HostingUnit and return bool</param>
        private void SetUnitComboBox(ComboBox unitsComboBox_, BE.Configuration.Term<BE.HostingUnit> term = null)
        {
            List<BE.HostingUnit> units;
            if (term == null) // get all BE.HostingUnit from DS
                units = _bl.AccordingTo(delegate (BE.HostingUnit u) { return true; });
            else // get BE.HostingUnit according to the term
                units = _bl.AccordingTo(term);

            List<string> unitNames = (from unit in units
                                      select $"({unit.HostingUnitKey}){unit.HostingUnitName}").ToList();
            // display
            unitsComboBox_.ItemsSource = unitNames;
        }

        /// <summary>
        /// displays on the given ComboBox all hosts' full name and key
        /// </summary>
        /// <param name="hostsComboBox_">host conboBox</param>
        private void SetHostsComboBox(ComboBox hostsComboBox_)
        {
            List<BE.Host> hosts = _bl.GetAllHosts();
            List<string> hostsNames = (from host in hosts
                                       select $"({host.HostKey}){host.PrivateName} {host.FamilyName}").ToList();
            hostsComboBox_.ItemsSource = hostsNames;
        }

        private void SetBanksComboBox(ComboBox banksComboBox_)
        {
            List<string> stringBanks = _bl.GetAllBanksAsDetailsString();
            if (stringBanks.Count > maxBanks)
                stringBanks = stringBanks.Take(maxBanks).ToList();

            // display
            banksComboBox_.ItemsSource = stringBanks;
        }

        #region Initialize DataContext functions

        /// <summary>
        /// Connects _host to the variables on SignUp tab,
        /// and make them show _host properties
        /// </summary>
        private void SetSignUpDataContext()
        {
            privateNameTextBox.DataContext = _host;
            familyNameTextBox.DataContext = _host;
            emailTextBox.DataContext = _host;
            phoneNumberTextBox.DataContext = _host;

            bankAccountNumberTextBox.DataContext = _host;
            collectionClearanceCheckBox.DataContext = _host;
        }

        /// <summary>
        /// Connects _hostingUnit to the variables on AddHostingUnit tab,
        /// and make them show _hostingUnit properties
        /// </summary>
        private void SetAddHostingUnitDataContext()
        {
            familyNameTextBlock.DataContext = _hostingUnit.Owner;
            privateNameTextBlock.DataContext = _hostingUnit.Owner;

            hostingUnitNameTextBox.DataContext = _hostingUnit;
            areaComboBox.DataContext = _hostingUnit;
            typeComboBox.DataContext = _hostingUnit;
        }

        /// <summary>
        /// Connects _host to the variables on Details tab,
        /// and make them show _host properties
        /// </summary>
        private void SetHostDetailsDataContext()
        {
            detailsPrivateNameTextBox.DataContext = _host;
            detailsFamilyNameTextBox.DataContext = _host;
            detailsEmailTextBox.DataContext = _host;
            detailsPhoneNumberTextBox.DataContext = _host;

            detailsBalanceTextBlock.DataContext = _host;
            detailsBankAccountNumberTextBox.DataContext = _host;
            detailsCollectionClearanceCheckBox.DataContext = _host;
        }

        /// <summary>
        /// Connects _hostingUnit to the variables on Details tab,
        /// and make them show _hostingUnit properties
        /// </summary>
        private void SetHostingUnitDetailsDataContext()
        {
            hostingUnitNameTextBoxDetails.DataContext = _hostingUnit;
            areaTextBoxDetails.DataContext = _hostingUnit;
            typeTextBoxDetails.DataContext = _hostingUnit;
        }

        private void SetOrdersDataContext()
        {
            // choose all _host hosting units
            List<BE.HostingUnit> hostingUnits = _bl.AccordingTo(delegate (BE.HostingUnit unit) { return unit.Owner.HostKey == _host.HostKey; });
            hostingUnitDataGrid.ItemsSource = hostingUnits;
            // get all open BE.GuestRequest
            guestRequestDataGrid.ItemsSource = _bl.AccordingTo(delegate (BE.GuestRequest gR) { return !_bl.IsGuestRequestClose(gR); });
            // get all BE.Order that are belongs to _host
            ordersDataGrid.ItemsSource = _bl.AccordingTo(delegate (BE.Order order)
            // run through all orders
            {
                // run through all  _host's hosting units,
                // ans foreach open order - check if this order belongs to one of _host hosting units
                // if not found return –1.
                int index = hostingUnits.FindIndex(unit => unit.HostingUnitKey == order.HostingUnitKey);
                if (index == -1)
                    return false;
                return true;
            });
        }

        /// <summary>
        /// create new _hostingUnit and show it on AddHostingUnit Tab
        /// </summary>
        private void ClearAddHostingUnit()
        {
            _hostingUnit = new BE.HostingUnit()
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host,
                DatesRange = new List<CalendarDateRange>()
            };
            SetAddHostingUnitDataContext();
        }

        /// <summary>
        /// create new _host and show it on SignIn Tab
        /// </summary>
        private void ClearSignUp()
        {
            _host = new BE.Host()
            {
                HostKey = ++BE.Configuration.HostKey,
                Balance = 100,
                BankBranchDetails = new BE.BankBranch()
            };
            SetSignUpDataContext();
        }


        /// <summary>
        /// create new null hosting unit in order to show no data on this section
        /// </summary>
        private void ClearHostingUnitDetails()
        {
            _hostingUnit = new BE.HostingUnit()
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host,
                DatesRange = new List<CalendarDateRange>()
            };
            SetHostingUnitDetailsDataContext();
        }
        #endregion

        /// <summary>
        /// check if any bank was selected
        /// </summary>
        /// <param name="bankComboBox_">banks ComboBox as string details</param>
        /// <param name="errorMessage_">error TextBlock message</param>
        private bool BankCheckSelectedItem(ComboBox bankComboBox_, TextBlock errorMessage_)
        {
            // check if any bank was selected
            if (bankComboBox_.SelectedIndex == -1)
            {
                // show fit message,
                errorMessage_.Foreground = new SolidColorBrush(Colors.Red);
                errorMessage_.Text = "please choose a bank";
                errorMessage_.FontSize = 25;
                errorMessage_.Fade();
                return false; // and finish evet
            }

            // update _host Bank Branch Details to selected item
            string bankDetails = bankComboBox_.SelectedItem as string;
            double selectedBankNumber = double.Parse(bankDetails.Split(' ')[0]);
            double selectedBranchNumber = double.Parse(bankDetails.Split(' ')[1]);
            List<BE.BankBranch> banksList = _bl.AccordingTo(banks => banks.BankNumber == selectedBankNumber &&
                                                                     banks.BranchNumber == selectedBranchNumber);
            BE.BankBranch bank = banksList.First();
            _host.BankBranchDetails = bank;
            return true;
        }

        #region Buttons Click events
        /// <summary>
        /// try to add _host to dataBase when user click to add his new host
        /// </summary>
        private void AddHostButton_Click(object sender, RoutedEventArgs e)
        {
            if (BankCheckSelectedItem(banksComboBox, SignUpErrorMessage))
            {
                try
                {
                    _bl.AddHost(_host);
                }
                catch (ArgumentException exp)
                {
                    // show fit comment
                    SignUpErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                    SignUpErrorMessage.Text = exp.Message;
                    SignUpErrorMessage.FontSize = 25;
                    SignUpErrorMessage.Fade();
                    return;
                }

                // prepare to get more new BE.Host
                ClearSignUp();
                SetHostsComboBox(hostsComboBox);
                detailsBanksComboBox.SelectedIndex = banksComboBox.SelectedIndex;
                banksComboBox.SelectedIndex = -1;

                // show fit comment
                SignUpErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
                SignUpErrorMessage.Text = "Host Add successfully!";
                SignUpErrorMessage.FontSize = 25;
                SignUpErrorMessage.Fade();
            }
        }

        /// <summary>
        /// try to login to exists host that the user chose in hostsComboBox
        /// make new (temp) _hostingUnit
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // check if any host has been selected
            if (hostsComboBox.SelectedItem == null)
            {
                loginErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                loginErrorMessage.Text = "Please Select a Host in order to contine";
                loginErrorMessage.FontSize = 25;
                loginErrorMessage.Fade();
                return;
            }

            // show fit comment
            loginErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            loginErrorMessage.Text = "Login successfully";
            loginErrorMessage.FontSize = 25;
            loginErrorMessage.Fade();

            // get selected hostkey:

            // format of hostsComboBox items: <(hostKey)><hostPrivateName> <hostFamliyName>
            string hostDetails = hostsComboBox.SelectedItem as string;
            // so in the splited string's second place will be the hostKey
            double hostKey = double.Parse(hostDetails.Split('(', ')')[1]);
            // and no update _host to selected host
            _host = (from host in _bl.GetAllHosts()
                     where host.HostKey == hostKey
                     select host).First();

            // prepare to contain new BE.HostingUnit
            _hostingUnit = new BE.HostingUnit
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host,
                DatesRange = new List<CalendarDateRange>()
            };

            // update data context
            SetAddHostingUnitDataContext();
            SetHostDetailsDataContext();
            SetHostingUnitDetailsDataContext();
            SetOrdersDataContext();

            detailsBanksComboBox.SelectedItem = _host.BankBranchDetails.AsString();

            SetHostsComboBox(hostingUnitDetails);
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit u) { return u.Owner.HostKey == _host.HostKey; });

            // makes tabs enabled 
            AddHostingUnitTab.IsEnabled = true;
            UpdateTab.IsEnabled = true;
            OrderTub.IsEnabled = true;
            SignupTab.IsEnabled = false;
        }


        /// <summary>
        /// logout from exists host.
        /// make new (temp) _host
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // prepare to get new BE.Host
            ClearSignUp();

            // make fits tabs enabled
            AddHostingUnitTab.IsEnabled = false;
            UpdateTab.IsEnabled = false;
            SignupTab.IsEnabled = true;
            OrderTub.IsEnabled = false;
        }

        /// <summary>
        /// try to add the _hostingUnit to dataBase
        /// if succeeded - prepare to get more new BE.HostingUnit
        /// </summary>
        private void AddHostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            _hostingUnit.Area = (BE.Enums.Area)areaComboBox.SelectedIndex;
            _hostingUnit.type = (BE.Enums.UnitType)typeComboBox.SelectedIndex;
            try
            {
                _bl.AddHostingUnit(_hostingUnit);
            }
            catch (ArgumentException exp)
            {
                // show fit comment
                AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                AddHostingUnitErrorMessage.Text = exp.Message;
                AddHostingUnitErrorMessage.FontSize = 25;
                AddHostingUnitErrorMessage.Fade();
                return;
            }
            catch (Exception exp)
            {
                // show fit comment
                AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                AddHostingUnitErrorMessage.Text = exp.Message;
                AddHostingUnitErrorMessage.FontSize = 25;
                AddHostingUnitErrorMessage.Fade();
                return;
            }

            // show fit comment
            AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            AddHostingUnitErrorMessage.Text = "Add hosting unit successfully!";
            AddHostingUnitErrorMessage.FontSize = 25;
            AddHostingUnitErrorMessage.Fade();

            // prepare to get more new BE.HostingUnit
            ClearAddHostingUnit();
            //
            SetOrdersDataContext();

            // add new hosting unit to hostingUnitDetails comboBox
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit u) { return u.Owner.HostKey == _host.HostKey; });

        }

        /// <summary>
        /// update 
        /// and show fit message
        /// </summary>
        private void UpdateHostButton_Click(object sender, RoutedEventArgs e)
        {
            if (BankCheckSelectedItem(detailsBanksComboBox, errorMessageDetailsHost))
            {
                try
                {
                    _bl.UpdateHost(_host);
                }
                catch (ArgumentException exp)
                {
                    errorMessageDetailsHost.Foreground = new SolidColorBrush(Colors.Red);
                    errorMessageDetailsHost.Text = exp.Message;
                    errorMessageDetailsHost.FontSize = 25;
                    errorMessageDetailsHost.Fade();
                    return;
                }
                errorMessageDetailsHost.Foreground = new SolidColorBrush(Colors.Green);
                errorMessageDetailsHost.Text = "Update successfully";
                errorMessageDetailsHost.FontSize = 25;
                errorMessageDetailsHost.Fade();

                // update comboBox in Login Tab
                SetHostsComboBox(hostsComboBox);
            }
        }

        /// <summary>
        /// update (exists) hostingUnit details,
        /// and show fit message
        /// </summary>
        private void UpdateHostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // if no hostingUnit chose:
                if (hostingUnitDetails.SelectedIndex == -1)
                    throw new ArgumentException("please select a hosting unit");

                _bl.UpdateHostingUnit(_hostingUnit);
            }
            catch (ArgumentNullException exp)
            {
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = exp.Message;
                errorMessageDetailsHostingUnit.FontSize = 25;
                errorMessageDetailsHostingUnit.Fade();
                return; // finish event
            }
            catch (ArgumentException exp)
            {
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = exp.Message;
                errorMessageDetailsHostingUnit.FontSize = 25;
                errorMessageDetailsHostingUnit.Fade();
                return; // finish event
            }

            //
            ClearHostingUnitDetails();
            ClearAddHostingUnit();
            SetOrdersDataContext();

            hostingUnitDiary.BlackoutDates.Clear();

            // update hostin unit comboBox in this Tab
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit unit) { return unit.Owner.HostKey == _host.HostKey; });

            // show fit message
            errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Green);
            errorMessageDetailsHostingUnit.Text = "Hosting Unit was update successfully";
            errorMessageDetailsHostingUnit.FontSize = 25;
            errorMessageDetailsHostingUnit.Fade();

            if (hostingUnitDetails.Items.Count == 1)
                hostingUnitDetails.SelectedIndex = -1; // update selection to none
        }

        /// <summary>
        /// delete exists BE.hostingUnit from data base.
        /// the BE.hostingUnit is held in _hostingUnit
        /// </summary>
        private void DeleteHostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.DeleteHostingUnit(_hostingUnit);
            }
            catch (ArgumentException exp)
            {
                // show fit message
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = exp.Message;
                errorMessageDetailsHostingUnit.FontSize = 25;
                errorMessageDetailsHostingUnit.Fade();
                return; // finish event
            }

            // clear this section
            ClearHostingUnitDetails();
            ClearAddHostingUnit();

            //
            SetOrdersDataContext();

            // update hosting units comboBox
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit unit) { return unit.Owner.HostKey == _host.HostKey; });

            hostingUnitDiary.BlackoutDates.Clear();

            // show fit message
            errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Green);
            errorMessageDetailsHostingUnit.Text = "Hosting unit successfully removed";
            errorMessageDetailsHostingUnit.FontSize = 25;
            errorMessageDetailsHostingUnit.Fade();
        }

        /// <summary>
        /// create new order according to selected hostingUnit and guestRequest
        /// </summary>
        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            BE.HostingUnit unit = hostingUnitDataGrid.SelectedItem as BE.HostingUnit;
            BE.GuestRequest guestRequest = guestRequestDataGrid.SelectedItem as BE.GuestRequest;
            // check if both unit and guestRequest  was selected
            if (unit == null || guestRequest == null)
            {
                newOrderErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                newOrderErrorMessage.Text = "hosting unit or guest request not selected!";
                newOrderErrorMessage.FontSize = 25;
                newOrderErrorMessage.Fade();
                return;
            }

            // create temp new order
            BE.Order order = new BE.Order()
            {
                OrderKey = ++BE.Configuration.OrderKey,
                CreateDate = DateTime.Now,
                GuestRequestKey = guestRequest.GuestRequestKey,
                HostingUnitKey = unit.HostingUnitKey,
                OrderDate = new DateTime(),
                Status = BE.Enums.Status.NotYetApproved
            };
            try
            {
                _bl.CreateOrder(order);
            }
            catch (ArgumentException exp)
            {
                newOrderErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                newOrderErrorMessage.Text = exp.Message;
                newOrderErrorMessage.FontSize = 25;
                newOrderErrorMessage.Fade();
                return;
            }

            newOrderErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            newOrderErrorMessage.Text = "Order create successfully";
            newOrderErrorMessage.FontSize = 25;
            newOrderErrorMessage.Fade();

            //maybe TODO: more things...?
            SetOrdersDataContext();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendEmail_OrderList_Button_Click(object sender, RoutedEventArgs e)
        {
            BE.Order order = ordersDataGrid.SelectedItem as BE.Order;
            try
            {
                _bl.UpdateOrder(order, BE.Enums.Status.MailSent);
                _bl.SendEmail(order);
            }
            catch (ArgumentException exp)
            {
                OrderListErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                OrderListErrorMessage.Text = exp.Message;
                OrderListErrorMessage.FontSize = 25;
                OrderListErrorMessage.Fade();
                return;
            }

            OrderListErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            OrderListErrorMessage.Text = "Email Sent Successfully!";
            OrderListErrorMessage.FontSize = 25;
            OrderListErrorMessage.Fade();

            //TODO: send real email
            SetOrdersDataContext();
        }

        private void Approved_OrderList_Button_Click(object sender, RoutedEventArgs e)
        {
            BE.Order order = ordersDataGrid.SelectedItem as BE.Order;
            try
            {
                _bl.ApprovedOrder(order);
            }
            catch (ArgumentException exp)
            {
                OrderListErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                OrderListErrorMessage.Text = exp.Message;
                OrderListErrorMessage.FontSize = 25;
                OrderListErrorMessage.Fade();
                return;
            }

            OrderListErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            OrderListErrorMessage.Text = "Order Approved Successfully!";
            OrderListErrorMessage.FontSize = 25;
            OrderListErrorMessage.Fade();


            SetOrdersDataContext();
        }
        #endregion

        /// <summary>
        /// show the selected  hosting unit on Details tab
        /// </summary>
        private void HostingUnitDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (errorMessageDetailsHostingUnit.Text == "Hosting unit successfully removed" ||
                errorMessageDetailsHostingUnit.Text == "Hosting Unit was update successfully")
            {
                // PUSAE THIS THREAD
                errorMessageDetailsHostingUnit.Text = "";
                //return;
            }

            // chech if any hosting unit selected
            if (hostingUnitDetails.SelectedIndex == -1)
            {
                // show fit message,
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = "please choose a hosting unit first";
                errorMessageDetailsHostingUnit.FontSize = 25;
                errorMessageDetailsHostingUnit.Fade();
                return; // and finish evet

            }

            // get the selected hostingUnitkey
            // format of hostsComboBox items: <(hostingUnitKey)><hostingUnitName>
            string hostingUnitName = hostingUnitDetails.SelectedItem as string;
            // so in the splited string's second place will be the hostKey
            double hostingUnitKey;
            try
            {
                hostingUnitKey = double.Parse(hostingUnitName.Split('(', ')')[1]);
            }
            catch
            {
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = "please choose a hosting unit first";
                errorMessageDetailsHostingUnit.FontSize = 25;
                errorMessageDetailsHostingUnit.Fade();
                return;
            }

            // and no update _hosting unit to selected hosting unit
            List<BE.HostingUnit> hostingUnitlst = _bl.AccordingTo(delegate (BE.HostingUnit unit)
            {
                return unit.HostingUnitKey == hostingUnitKey;
            });

            /// (if this exception is throw  - there is a big problem
            /// this exception should not be throwon, But it's still here because of the little 
            /// chance that it will happens, and as a result we'll know to fix it)
            if (hostingUnitlst.Count() != 1)
                throw new ArgumentOutOfRangeException("More than one hosting unit with same key");

            // show the selected hoting unit on screen
            _hostingUnit = hostingUnitlst.First();
            SetHostingUnitDetailsDataContext();

            hostingUnitDiary.BlackoutDates.Clear();
            for (int i = 0; i < _hostingUnit.DatesRange.Count; i++)
                hostingUnitDiary.BlackoutDates.Add(_hostingUnit.DatesRange[i]);
        }


        public List<BE.BankBranch> GetAllBanksByKeywords(string[] keywords)
        {
            IEnumerable<BE.BankBranch> lst = from branch in _bl.GetAllBanks()
                                      where keywords.ToList().Exists(str => branch.Contains(str))
                                      select branch;
            return lst.ToList();
        }

        private void BanksComboBox_TextChanged(object sender, TextChangedEventArgs e) => ShowRelevantBanks(banksComboBox);

        private void DetailsBanksComboBox_TextChanged(object sender, TextChangedEventArgs e) => ShowRelevantBanks(banksComboBox);

        private void ShowRelevantBanks(ComboBox banksComboBox_)
        {
            List<BE.BankBranch> results = GetAllBanksByKeywords(banksComboBox_.Text.Split(' '));
            List<string> banksAsDetailsString = (from bank in results
                                                 select bank.AsString()).Distinct().ToList();

            if (banksAsDetailsString.Capacity > 100)
                banksComboBox_.ItemsSource = banksAsDetailsString.Take(100).ToList();
            else
                banksComboBox_.ItemsSource = banksAsDetailsString;
            banksComboBox_.IsDropDownOpen = true;
        }
    }
}
