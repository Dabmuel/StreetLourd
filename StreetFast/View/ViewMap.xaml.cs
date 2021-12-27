using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StreetLourd.View
{
    /// <summary>
    /// Logique d'interaction pour ViewMap.xaml
    /// </summary>
    public partial class ViewMap : Window
    {
        public ViewMap()
        {
            InitializeComponent();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach(Frame item in this.List.Items)
            {
                ((ViewRun)item.Content).Width = this.List.ActualWidth - 30;
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.List.SelectedItem != null)
            {
                this.BtAddRun.IsEnabled = true;
                this.BtChangeCar.IsEnabled = true;
                this.BtDeletCar.IsEnabled = true;
            }
            else
            {
                this.BtAddRun.IsEnabled = false;
                this.BtChangeCar.IsEnabled = false;
                this.BtDeletCar.IsEnabled = false;
            }
        }
    }
}
