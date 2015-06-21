using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
using SYMM_Frontend_WPF.Dialogs;

namespace SYMM_Frontend_WPF.Pages
{
    /// <summary>
    /// Interaktionslogik für Erfassen.xaml
    /// </summary>
    public partial class Erfassen : UserControl
    {
        public Erfassen()
        {
            InitializeComponent();
        }

        private void btnCollectByChanname_Click(object sender, RoutedEventArgs e)
        {
            CollectByNameDialog dialog = new CollectByNameDialog();
            dialog.ShowDialog();
        }
    }
}
