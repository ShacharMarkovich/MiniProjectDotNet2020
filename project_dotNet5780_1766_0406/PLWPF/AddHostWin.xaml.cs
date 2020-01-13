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
        public BL.IBL _bL = BL.FactoryBL.Instance;
        public BE.Host _host;
        private BE.BankBranch _myBank;

        private void SettingDataContext()
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
            List<BE.Host> hosts;
            try
            {
            hosts = _bL.GetAllHosts();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            //IEnumerable<string> hostsNames = (from host in hosts
            //                                  select $"{host.PrivateName} {host.FamilyName}").ToList();
            //hostsComboBox.ItemsSource = hostsNames;
        }

        public AddHostWin()
        {
            InitializeComponent();

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

            SettingDataContext();
            SetComboBox();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
        }

        private void AddHostButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bL.AddHost(_host);
                errorMessage.Foreground = new SolidColorBrush(Colors.Green);
                errorMessage.Text = "Host Add successfully!";
            }
            catch (ArgumentException exp)
            {
                errorMessage.Text = exp.Message;
            }
        }
    }
}
