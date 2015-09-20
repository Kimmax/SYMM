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

        /// <summary>
        /// The user want's to download by channelname. Show the dialog for that.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCollectByChanname_Click(object sender, RoutedEventArgs e)
        {
            // Init channelname dialog
            CollectByNameDialog dialog = new CollectByNameDialog();

            // Show the diialog
            var result = dialog.ShowDialog();

            // Setup to move to next page. Channelname gets passed by GET parameter
            string url = "/Pages/Downloaden.xaml?method=channelname&extra=" + dialog.ChannelName;

            // HACK: Move to next page passing parameter
            BBCodeBlock bs = new BBCodeBlock();
            bs.LinkNavigator.Navigate(new Uri(url, UriKind.Relative), this);
        }

        /// <summary>
        /// The user want's to download by URL. Show the dialog for that.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCollectByURL_Click(object sender, RoutedEventArgs e)
        {
            // Init url dialog
            CollectByURL dialog = new CollectByURL();

            // Show the dialog
            var result = dialog.ShowDialog();

            // Setup to move to next page. Channelname gets passed by GET parameter
            string url = "/Pages/Downloaden.xaml?method=url&extra=" + dialog.URL;

            // HACK: Move to next page passing param
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
            if(!String.IsNullOrEmpty(Properties.Settings.Default.savePath))
            {
                txtSPath.Text = Properties.Settings.Default.savePath;
            }
            else
            {
                txtSPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\SYMM";
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSPath.Text))
            {
                Properties.Settings.Default.savePath = txtSPath.Text;
                Properties.Settings.Default.Save();
            }
        }
    }
}
