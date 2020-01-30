﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BL.IBL _bl = null;
        public MainWindow()
        {
            InitializeComponent();
            _bl = BL.FactoryBL.Instance;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException("func StatusComboBox_SelectionChanged Not implemented");
        }

        private void AddGuestRequestButton_Click(object sender, RoutedEventArgs e)
        {
            new AddGuestRequestWin().ShowDialog();
        }
        private void AddHostButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_bl.IsBanksLoad())
                _bl.Join();

            new AddHostWin().ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _bl.UpdateConfig();
        }

        private void HostAreaButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_bl.IsBanksLoad())
                MessageBox.Show("can't tauch this");
        }
    }
}
