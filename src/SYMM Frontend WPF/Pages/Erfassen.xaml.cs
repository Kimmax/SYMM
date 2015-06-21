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
using FirstFloor.ModernUI.Windows;

namespace SYMM_Frontend_WPF.Pages
{
    /// <summary>
    /// Interaktionslogik für Erfassen.xaml
    /// </summary>
    public partial class Erfassen : Page, IContent 
    {
        public Erfassen()
        {
            InitializeComponent();
        }

        private void btnCollectByChanname_Click(object sender, RoutedEventArgs e)
        {
            CollectByNameDialog dialog = new CollectByNameDialog();
            var result = dialog.ShowDialog();

            string url = "/Pages/Downloaden.xaml?method=channelname&extra=" + dialog.ChannelName;
            BBCodeBlock bs = new BBCodeBlock();
            bs.LinkNavigator.Navigate(new Uri(url, UriKind.Relative), this);
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }
    }
}
