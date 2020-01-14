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

        public BL.IBL _bl = BL.FactoryBL.Instance;
        public BE.Host _host;
        public BE.HostingUnit _uostingUnit;
        private BE.BankBranch _myBank;

        private void SetSignUpDataContext()
        {
            /*key*/
            hostKeyTextBlock.DataContext = _host;

            privateNameTextBox.DataContext = _host;
            familyNameTextBox.DataContext = _host;
            emailTextBox.DataContext = _host;
            phoneNumberTextBox.DataContext = _host;

            balanceTextBlock.DataContext = _host;
            bankAccountNumberTextBox.DataContext = _host;

            /*key*/
            bankNumberTextBlock.DataContext = _host.BankBranchDetails;
            bankNameTextBox.DataContext = _host.BankBranchDetails;
            branchNumberTextBox.DataContext = _host.BankBranchDetails;
            branchAddressTextBox.DataContext = _host.BankBranchDetails;
            branchCityTextBox.DataContext = _host.BankBranchDetails;

            collectionClearanceCheckBox.DataContext = _host;
        }
        private void SetComboBox()
        {
            List<BE.Host> hosts = _bl.GetAllHosts();
            List<string> hostsNames = (from host in hosts
                                       select $"({host.HostKey}){host.PrivateName} {host.FamilyName}").ToList();
            hostsComboBox.ItemsSource = hostsNames;
        }
        private void SetAddHostingUnitDataContext()
        {
            areaComboBox.DataContext = _uostingUnit;
            this.areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Area));
            areaComboBox.SelectedIndex = 0;

            hostingUnitKeyTextBlock.DataContext = _uostingUnit;

            hostingUnitNameTextBox.DataContext = _uostingUnit;

            typeComboBox.DataContext = _uostingUnit;
            this.typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.UnitType));
            typeComboBox.SelectedIndex = 0;

            emailTextBlock.DataContext = _uostingUnit.Owner;

            familyNameTextBlock.DataContext = _uostingUnit.Owner;

            hostKeyTextBlock.DataContext = _uostingUnit;

            phoneNumberTextBlock.DataContext = _uostingUnit.Owner;

            privateNameTextBlock.DataContext = _uostingUnit.Owner;
        }

        public AddHostWin()
        {
            InitializeComponent();
            _uostingUnit = new BE.HostingUnit
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                Owner = _host
            };
            _myBank = new BE.BankBranch()
            {
                BankNumber = ++BE.Configuration.BankNumber,
            };
            _host = new BE.Host()
            {
                HostKey = ++BE.Configuration.HostKey,
                Balance = 100,
                BankBranchDetails = _myBank
            };

            SetAddHostingUnitDataContext();
            SetSignUpDataContext();
            SetComboBox();
        }


        private void AddHostButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bl.AddHost(_host);
                
                SignUpErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
                SignUpErrorMessage.Text = "Host Add successfully!";
            }
            catch (ArgumentException exp)
            {
                SignUpErrorMessage.Text = exp.Message;
            }
        }

        private void AddUostingUnitButton_Click(object sender, RoutedEventArgs e)
        {
            AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            try
            {
                _bl.AddHostingUnit(_uostingUnit);
                AddHostingUnitErrorMessage.Text = "Add hosting unit successfully!";
            }
            catch (ArgumentException exp)
            {
                AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                AddHostingUnitErrorMessage.Text = exp.Message;
            }
            catch (Exception exp)
            {
                AddHostingUnitErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                AddHostingUnitErrorMessage.Text = exp.Message;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (hostsComboBox.SelectedItem == null)
            {
                loginErrorMessage.Foreground = new SolidColorBrush(Colors.Red);
                loginErrorMessage.Text = "Please Select an Host in order to contine";
                return;
            }
            //TODO...
            // format of hostsComboBox items: <(hostKey)><hostPrivateName> <hostFamliyName>
            string hostDetails = hostsComboBox.SelectedItem as string;
            // so in the splited string's first place will be the hostKey
            double hostKey = double.Parse(hostDetails.Split('(', ')')[0]);
            //_host = from host in _bl.GetAllHosts()
            //        where host.HostKey == hostKey
            //        select host;
            loginErrorMessage.Foreground = new SolidColorBrush(Colors.Green);
            loginErrorMessage.Text = "";
            AddHostingUnitTab.IsEnabled = true;
            UpdateTab.IsEnabled = true;
            deleteTab.IsEnabled = true;
        }
    }
}
