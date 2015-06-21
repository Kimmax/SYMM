using FirstFloor.ModernUI.Windows.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SYMM_Frontend_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for ModernDialog1.xaml
    /// </summary>
    public partial class CollectByNameDialog : ModernDialog
    {
        public string ChannelName
        {
            get { return this.txtChannelName.Text; }
        }

        public CollectByNameDialog()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
        }
    }
}
