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
        }

        #region class proprties
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
                {
                    BankNumber = ++BE.Configuration.BankNumber,
                }
            };

            // create temp BE.HostingUnit. because of the same reason as above
            _hostingUnit = new BE.HostingUnit
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
            };

            SetSignUpDataContext();
            SetHostsComboBox(hostsComboBox);

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

        #region Initialize DataContext functions

        /// <summary>
        /// Connects _host to the variables on SignUp tab,
        /// and make them show _host properties
        /// </summary>
        private void SetSignUpDataContext()
        {
            hostKeyTextBlock.DataContext = _host;
            privateNameTextBox.DataContext = _host;
            familyNameTextBox.DataContext = _host;
            emailTextBox.DataContext = _host;
            phoneNumberTextBox.DataContext = _host;

            balanceTextBlock.DataContext = _host;
            bankAccountNumberTextBox.DataContext = _host;
            collectionClearanceCheckBox.DataContext = _host;

            bankNumberTextBlock.DataContext = _host.BankBranchDetails;
            bankNameTextBox.DataContext = _host.BankBranchDetails;
            branchNumberTextBox.DataContext = _host.BankBranchDetails;
            branchAddressTextBox.DataContext = _host.BankBranchDetails;
            branchCityTextBox.DataContext = _host.BankBranchDetails;
        }

        /// <summary>
        /// Connects _hostingUnit to the variables on AddHostingUnit tab,
        /// and make them show _hostingUnit properties
        /// </summary>
        private void SetAddHostingUnitDataContext()
        {
            addhostKeyTextBlock.DataContext = _hostingUnit;
            familyNameTextBlock.DataContext = _hostingUnit.Owner;
            privateNameTextBlock.DataContext = _hostingUnit.Owner;
            emailTextBlock.DataContext = _hostingUnit.Owner;
            phoneNumberTextBlock.DataContext = _hostingUnit.Owner;

            hostingUnitKeyTextBlock.DataContext = _hostingUnit;
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
            detailsHostKeyTextBlock.DataContext = _host;
            detailsPrivateNameTextBox.DataContext = _host;
            detailsFamilyNameTextBox.DataContext = _host;
            detailsEmailTextBox.DataContext = _host;
            detailsPhoneNumberTextBox.DataContext = _host;

            detailsBalanceTextBlock.DataContext = _host;
            detailsBankAccountNumberTextBox.DataContext = _host;
            detailsCollectionClearanceCheckBox.DataContext = _host;

            detailsBankNumberTextBlock.DataContext = _host.BankBranchDetails;
            detailsBankNameTextBox.DataContext = _host.BankBranchDetails;
            detailsBranchNumberTextBox.DataContext = _host.BankBranchDetails;
            detailsBranchAddressTextBox.DataContext = _host.BankBranchDetails;
            detailsBranchCityTextBox.DataContext = _host.BankBranchDetails;
        }

        /// <summary>
        /// Connects _hostingUnit to the variables on Details tab,
        /// and make them show _hostingUnit properties
        /// </summary>
        private void SetHostingUnitDetailsDataContext()
        {
            hostingUnitKeyTextBlockDetails.DataContext = _hostingUnit;
            hostingUnitNameTextBoxDetails.DataContext = _hostingUnit;
            areaTextBoxDetails.DataContext = _hostingUnit;
            typeTextBoxDetails.DataContext = _hostingUnit;
        }


        /// <summary>
        /// create new _hostingUnit and show it on AddHostingUnit Tab
        /// </summary>
        private void ClearAddHostingUnit()
        {
            _hostingUnit = new BE.HostingUnit()
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
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
                {
                    BankNumber = ++BE.Configuration.BankNumber,
                }
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
                Owner = _host
            };
            SetHostingUnitDetailsDataContext();
        }


        #endregion

        #region Buttons Click events

        /// <summary>
        /// try to add _host to dataBase when user click to add his new host
        /// </summary>
        private void AddHostButton_Click(object sender, RoutedEventArgs e)
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
                return;
            }

            // show fit comment
            SignUpErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            SignUpErrorMessage.Text = "Host Add successfully!";

            // prepare to get more new BE.Host
            ClearSignUp();
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
                return;
            }
            
            // show fit comment
            loginErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            loginErrorMessage.Text = "Login successfully";

            // get selected hostkey:

            // format of hostsComboBox items: <(hostKey)><hostPrivateName> <hostFamliyName>
            string hostDetails = hostsComboBox.SelectedItem as string;
            // so in the splited string's second place will be the hostKey
            double hostKey = double.Parse(hostDetails.Split('(', ')')[1]);
            // and no update _host to selected host
            _host = (from host in _bl.GetAllHosts()
                    where host.HostKey == hostKey
                    select host).Single();

            // prepare to contain new BE.HostingUnit
            _hostingUnit = new BE.HostingUnit
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
            };

            // update data context
            SetAddHostingUnitDataContext();
            SetHostingUnitDetailsDataContext();
            SetHostDetailsDataContext();
            SetHostingUnitDetailsDataContext();

            SetHostsComboBox(hostingUnitDetails);
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit u) { return u.Owner.HostKey == _host.HostKey; });
            
            // makes tabs enabled 
            AddHostingUnitTab.IsEnabled = true;
            UpdateTab.IsEnabled = true;
            SignInTab.IsEnabled = false;
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
            SignInTab.IsEnabled = true;
        }

        /// <summary>
        /// try to add the _hostingUnit to dataBase
        /// if succeeded - prepare to get more new BE.HostingUnit
        /// </summary>
        private void AddHostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.AddHostingUnit(_hostingUnit);
            }
            catch (ArgumentException exp)
            {
                // show fit comment
                AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                AddHostingUnitErrorMessage.Text = exp.Message;
                return;
            }
            catch (Exception exp)
            {
                // show fit comment
                AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                AddHostingUnitErrorMessage.Text = exp.Message;
                return;
            }

            // show fit comment
            AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            AddHostingUnitErrorMessage.Text = "Add hosting unit successfully!";

            // prepare to get more new BE.HostingUnit
            ClearAddHostingUnit();
            // add new hosting unit to hostingUnitDetails comboBox
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit u) { return u.Owner.HostKey == _host.HostKey; });

        }

        /// <summary>
        /// update 
        /// and show fit message
        /// </summary>
        private void UpdateHostButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.UpdateHost(_host);
            }
            catch (ArgumentException exp)
            {
                errorMessageDetailsHost.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHost.Text = exp.Message;
                return;
            }
            errorMessageDetailsHost.Foreground = new SolidColorBrush(Colors.Green);
            errorMessageDetailsHost.Text = "Update successfully";

            // update comboBox in Login Tab
            SetHostsComboBox(hostsComboBox);
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

                return; // finish event
            }
            catch (ArgumentException exp)
            {
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = exp.Message;

                return; // finish event
            }

            ClearHostingUnitDetails();

            // update hostin unit comboBox in this Tab
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit unit) { return unit.Owner.HostKey == _host.HostKey; });

            // show fit message
            errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Green);
            errorMessageDetailsHostingUnit.Text = "Hosting Unit was update successfully";

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

                return; // finish event
            }

            // show fit message
            errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Green);
            errorMessageDetailsHostingUnit.Text = "Hosting unit successfully removed";

            // clear this section
            ClearHostingUnitDetails();

            // update hosting units comboBox
            SetUnitComboBox(hostingUnitDetails, delegate(BE.HostingUnit unit) { return unit.Owner.HostKey == _host.HostKey; });
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
                return;
            }

            // and no update _hosting unit to selected hosting unit
            List<BE.HostingUnit> hostingUnitlst = _bl.AccordingTo(delegate (BE.HostingUnit unit) {
                return unit.HostingUnitKey == hostingUnitKey;
            });

            /// (if this exception is throw  - there is a big problem
            /// this exception should not be throwon, But it's still here because of the little 
            /// chance that it will happens, and as a result we'll know to fix it)
            if (hostingUnitlst.Count() != 1)
                throw new ArgumentOutOfRangeException("More than one hosting unit with same key");

            // show the selected hoting unit on screen
            _hostingUnit = hostingUnitlst.Single();
            SetHostingUnitDetailsDataContext();
        }
    }
}
