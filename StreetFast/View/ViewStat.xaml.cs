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
    /// Logique d'interaction pour ViewStat.xaml
    /// </summary>
    public partial class ViewStat : Window
    {
        public ViewStat()
        {
            InitializeComponent();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TxBxResearch.Width = this.ActualWidth - 436;
            foreach (Frame item in this.List.Items)
            {
                ((ViewRun)item.Content).Width = this.List.ActualWidth - 30;
            }
        }
    }
}
