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
    /// Logique d'interaction pour ViewAddCar.xaml
    /// </summary>
    public partial class ViewAddCar : Window
    {
        public ViewAddCar()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
