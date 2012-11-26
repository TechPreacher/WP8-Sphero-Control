using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WP8_Sphero
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.settings.ShowAboutPage)
                checkboxShow.IsChecked = true;

            base.OnNavigatedTo(e);
        }

        private void checkboxShow_Checked(object sender, RoutedEventArgs e)
        {
            App.settings.Save(true);

        }

        private void checkboxShow_Unchecked(object sender, RoutedEventArgs e)
        {
            App.settings.Save(false);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        
    }
}