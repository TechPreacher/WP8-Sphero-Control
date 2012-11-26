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
    public partial class ColorPage : PhoneApplicationPage
    {
        public ColorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            colorPicker.Color = App.spheroHelper.SpheroColor;
        }

        private void ColorPicker_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            App.spheroHelper.SpheroColor = color;
            App.spheroHelper.Bluetooth_SpheroToFixedColor(color);
        }
    }
}