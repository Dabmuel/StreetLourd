using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Media;
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
    /// Logique d'interaction pour ViewFastBoi.xaml
    /// </summary>
    public partial class ViewFastBoi : Window
    {
        public ViewFastBoi()
        {
            InitializeComponent();

            SoundPlayer prout = new SoundPlayer(StreetLourd.Properties.Resources.prout);

            prout.Play();

            this.MouseDoubleClick += (a, b) =>
            {
                this.Close();
            };
        }
    }
}
