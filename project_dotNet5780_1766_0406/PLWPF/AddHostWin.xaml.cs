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

        public AddHostWin()
        {
            InitializeComponent();

            _host = new BE.Host()
            {
                HostKey = ++BE.Configuration.HostKey,
                Balance = 100,
                BankBranchDetails = new BE.BankBranch()
                {
                    BankNumber = ++BE.Configuration.BankNumber,
                }
        };
            _hostingUnit = new BE.HostingUnit
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
            };

            SetSignUpDataContext();
            SetHostsComboBox(hostsComboBox);

            typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.UnitType));
            typeComboBox.SelectedIndex = 0;
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Area));
            areaComboBox.SelectedIndex = 0;
        }

        private void SetUnitComboBox(ComboBox unitsComboBox_, BE.Configuration.Term<BE.HostingUnit> term = null)
        {
            List<BE.HostingUnit> units;
            if (term == null)
                units = _bl.AccordingTo(delegate (BE.HostingUnit u) { return true; });
            else
                units = _bl.AccordingTo(term);

            List<string> unitNames = (from unit in units
                                      select $"({unit.HostingUnitKey}){unit.HostingUnitName}").ToList();
            unitsComboBox_.ItemsSource = unitNames;
        }

        private void SetHostsComboBox(ComboBox hostsComboBox_)
        {
            List<BE.Host> hosts = _bl.GetAllHosts();
            List<string> hostsNames = (from host in hosts
                                       select $"({host.HostKey}){host.PrivateName} {host.FamilyName}").ToList();
            hostsComboBox_.ItemsSource = hostsNames;
        }

        #region Initialize DataContext functions

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

        private void SetAddHostingUnitDataContext()
        {
            hostKeyTextBlock.DataContext = _hostingUnit;
            familyNameTextBlock.DataContext = _hostingUnit.Owner;
            privateNameTextBlock.DataContext = _hostingUnit.Owner;
            emailTextBlock.DataContext = _hostingUnit.Owner;
            phoneNumberTextBlock.DataContext = _hostingUnit.Owner;

            hostingUnitKeyTextBlock.DataContext = _hostingUnit;
            hostingUnitNameTextBox.DataContext = _hostingUnit;
            areaComboBox.DataContext = _hostingUnit;
            typeComboBox.DataContext = _hostingUnit;
        }

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
        private void SetHostingUnitDetailsDataContext()
        {
            hostingUnitKeyTextBlockDetails.DataContext = _hostingUnit;
            hostingUnitNameTextBoxDetails.DataContext = _hostingUnit;
            areaTextBoxDetails.DataContext = _hostingUnit;
            typeTextBoxDetails.DataContext = _hostingUnit;
        }
        
        
        private void ClearAddHostingUnit()
        {
            _hostingUnit = new BE.HostingUnit()
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
            };
            SetAddHostingUnitDataContext();
        }

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

        #endregion
        
        #region Buttons Click events

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

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // prepare to get new BE.Host
            ClearSignUp();

            // make fits tabs enabled
            AddHostingUnitTab.IsEnabled = false;
            UpdateTab.IsEnabled = false;
            SignInTab.IsEnabled = true;
        }

        private void AddUostingUnitButton_Click(object sender, RoutedEventArgs e)
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

            // update ocmboBox in Login Tab
            SetHostsComboBox(hostsComboBox);
        }

        private void UpdateHostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (hostingUnitDetails.SelectedIndex == -1)
                    throw new ArgumentException("please select a hosting unit");

                _bl.UpdateHostingUnit(_hostingUnit);
            }
            catch (ArgumentNullException exp)
            {
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = exp.Message;
                return;
            }
            catch (ArgumentException exp)
            {
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = exp.Message;
                return;
            }

            _hostingUnit = new BE.HostingUnit()
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
            };
            SetHostingUnitDetailsDataContext();
            SetUnitComboBox(hostingUnitDetails, delegate (BE.HostingUnit unit) { return unit.Owner.HostKey == _host.HostKey; });

            errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Green);
            errorMessageDetailsHostingUnit.Text = "Update successfully";
            hostingUnitDetails.SelectedIndex = -1;
        }

        private void DeleteHostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.DeleteHostingUnit(_hostingUnit);
            }
            catch (ArgumentException exp)
            {
                errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHostingUnit.Text = exp.Message;
                return;
            }

            errorMessageDetailsHostingUnit.Foreground = new SolidColorBrush(Colors.Red);
            errorMessageDetailsHostingUnit.Text = "Hosting unit successfully removed";

            _hostingUnit = new BE.HostingUnit()
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
            };
            SetHostingUnitDetailsDataContext();
            SetUnitComboBox(hostingUnitDetails, delegate(BE.HostingUnit unit) { return unit.Owner.HostKey == _host.HostKey; });

        }

        #endregion

        private void HostingUnitDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO: regognize who call the event


            // get selected hostingUnitkey:
            
            // format of hostsComboBox items: <(hostingUnitKey)><hostingUnitName>
            string hostingUnitName = hostingUnitDetails.SelectedItem as string;
            if (hostingUnitName == null)
            {

            }
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
            if (hostingUnitlst.Count() != 1)
                throw new ArgumentOutOfRangeException("More than one hosting unit with same key");
            _hostingUnit = hostingUnitlst.Single();
            SetHostingUnitDetailsDataContext();
            // TODO: show in hosting unit tab
        }

        private void SignIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
