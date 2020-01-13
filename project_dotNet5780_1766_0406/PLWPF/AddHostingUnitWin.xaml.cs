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
    /// Interaction logic for AddHostingUnitWin.xaml
    /// </summary>
    public partial class AddHostingUnitWin : Window
    {
        public BL.IBL _bl = BL.FactoryBL.Instance;
        public BE.HostingUnit _uostingUnit;
        BE.Host temp_host;
        public AddHostingUnitWin()
        {
            InitializeComponent();
          
            temp_host = new BE.Host()
            {
                HostKey = ++BE.Configuration.HostKey,
                Balance = 1000,
                PrivateName = "name1",
                FamilyName = "pname1",
                PhoneNumber = "phone1",
                Email = "smarkovi@g.jct.ac.il",
                BankAccountNumber = 120159,
                CollectionClearance = false,
                BankBranchDetails = new BE.BankBranch()
                {
                    BankNumber = ++BE.Configuration.BankNumber,
                    BankName = "bank1",
                    BranchNumber = 1,
                    BranchAddress = "address1",
                    BranchCity = "city1"
                }
            };
            _uostingUnit = new BE.HostingUnit
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey, 
                Owner=temp_host
            };

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostingUnitViewSource.Source = [generic data source]
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           errorMessage.Foreground = new SolidColorBrush(Colors.Green);
           try
           {
                _bl.AddHostingUnit(_uostingUnit);
               errorMessage.Text = "Add hosting unit successfully!";
           }
           catch (ArgumentException exp)
           {
                errorMessage.Foreground = new SolidColorBrush(Colors.Red);
                errorMessage.Text = exp.Message;
           }
           catch (Exception exp)
           {
                errorMessage.Foreground = new SolidColorBrush(Colors.Red);
                errorMessage.Text = exp.Message;
           }
        }
    }
}
