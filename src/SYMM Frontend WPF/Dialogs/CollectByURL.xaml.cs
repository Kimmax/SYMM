﻿using FirstFloor.ModernUI.Windows.Controls;
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
    /// Interaction logic for URL Dialog
    /// </summary>
    public partial class CollectByURL : ModernDialog
    {
        // Public getter for the textbox
        public string URL
        {
            get { return this.txtURL.Text; }
        }

        public CollectByURL()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
        }
    }
}
