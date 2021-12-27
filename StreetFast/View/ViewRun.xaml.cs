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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StreetLourd.View
{
    /// <summary>
    /// Logique d'interaction pour ViewRun.xaml
    /// </summary>
    public partial class ViewRun : Page
    {
        public ViewRun()
        {
            InitializeComponent();
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 500)
            {
                this.TxName.Margin = new Thickness(10, 5, 0, 0);
                this.TxClasse.Margin = new Thickness(200, 5, 0, 0);
                this.TxTime.Margin = new Thickness(0, 5, 10, 0);
                this.TxDate.Margin = new Thickness(0, 5, 80, 0);
                this.TxNb.Margin = new Thickness(0, 5, 170, 0);
                this.Height = 25;
            }
            else
            {
                this.TxName.Margin = new Thickness(10, 5, 0, 0);
                this.TxClasse.Margin = new Thickness(15, 25, 0, 0);
                this.TxTime.Margin = new Thickness(0, 5, 10, 0);
                this.TxDate.Margin = new Thickness(0, 25, 10, 0);
                this.TxNb.Margin = new Thickness(0, 15, 100, 0);
                this.Height = 48;
            }
        }
    }
}
