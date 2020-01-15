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

        private void SetUnitComboBox(ComboBox unitsComboBox_)
        {
            List<BE.HostingUnit> units = _bl.AccordingTo(delegate (BE.HostingUnit u) { return true; });
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

        private void SetDetailsDataContext()
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

            ///////
            /// for Debug only
            SetHostsComboBox(HostsComboBox);
            SetHostsComboBox(hostsComboBox);
            ///////
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // check if any host has been selected
            if (hostsComboBox.SelectedItem == null)
            {
                loginErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                loginErrorMessage.Text = "Please Select an Host in order to contine";
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

            // show _host in detail tab
            SetDetailsDataContext();

            // makes tabs enabled 
            AddHostingUnitTab.IsEnabled = true;
            UpdateTab.IsEnabled = true;
            deleteTab.IsEnabled = true;
            SignInTab.IsEnabled = false;

            /////////
            /// for Degub only
            // show in comboBox of delete tab all units of selected host
            List<BE.HostingUnit> units = _bl.AccordingTo(delegate (BE.HostingUnit unit){
                return unit.Owner.HostKey == _host.HostKey; });
            List<string> unitNames = (from unit in units
                                      select $"({unit.HostingUnitKey}){unit.HostingUnitName}").ToList();
            HosthingUnitsComboBox.ItemsSource = unitNames;
            /////////
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // prepare to get new BE.Host
            ClearSignUp();

            // make fits tabs enabled
            AddHostingUnitTab.IsEnabled = false;
            UpdateTab.IsEnabled = false;
            deleteTab.IsEnabled = false;
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

            // prepare to get more new BE.Host
            ClearAddHostingUnit();

            /////////
            /// for Degub only
            // show in comboBox of delete tab all units of selected host
            List<BE.HostingUnit> units = _bl.AccordingTo(delegate (BE.HostingUnit unit) {
                return unit.Owner.HostKey == _host.HostKey;
            });
            List<string> unitNames = (from unit in units
                                      select $"({unit.HostingUnitKey}){unit.HostingUnitName}").ToList();
            HosthingUnitsComboBox.ItemsSource = unitNames;
            /////////
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

            /// for Debug only
            SetHostsComboBox(HostsComboBox);
            ///
        }

        private void UpdateHostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            // todo check if there is an selected hosting unit
            try
            {
                _bl.UpdateHostingUnit(_hostingUnit);
            }
            catch (ArgumentException exp)
            {
                errorMessageDetailsHost.Foreground = new SolidColorBrush(Colors.Red);
                errorMessageDetailsHost.Text = exp.Message;
                return;
            }
            errorMessageDetailsHost.Foreground = new SolidColorBrush(Colors.Green);
            errorMessageDetailsHost.Text = "Update successfully";

            SetUnitComboBox(HosthingUnitsComboBox);
        }
        #endregion
    }
}
