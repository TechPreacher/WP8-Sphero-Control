//
// Written in 2012 by Sascha Corti.
//
// Licensed under the Microsoft Public License (Ms-PL).
// You may se this file in compliance with the License.
// Obtain a copy of the License at:
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP8_Sphero.Resources;
using System.Windows.Media;
using WP8_Joystick;


namespace WP8_Sphero
{
    public partial class MainPage : PhoneApplicationPage
    {
        int currentDirection = 0;
        bool isAppJustStarted = true;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            LayoutRoot.DataContext = App.spheroHelper;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.settings.ShowAboutPage && isAppJustStarted)
            {
                NavigateTo("/AboutPage.xaml");
                isAppJustStarted = false;
                return;
            }


            joystick.StartJoystick();

            isAppJustStarted = false;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            joystick.StopJoystick();
            
            base.OnNavigatedFrom(e);
        }


        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            App.spheroHelper.Bluetooth_ConnectToSphero();
        }
        
        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            App.spheroHelper.Bluetooth_Disconnect();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/AboutPage.xaml");
        }

        private void btnChangeColor_Click(object sender, RoutedEventArgs e)
        {
            if (!App.spheroHelper.IsConnected)
            {
                MessageBox.Show("Connect to Sphero first.");
                return;
            }

            NavigateTo("/ColorPage.xaml");

        }

        // CALIBRATION

        private void btnBackLedOn_Click(object sender, RoutedEventArgs e)
        {
            App.spheroHelper.Bluetooth_ShowBackLed(255);
        }

        private void btnBackLedOff_Click(object sender, RoutedEventArgs e)
        {
            App.spheroHelper.Bluetooth_SetHeading(0);
            App.spheroHelper.Bluetooth_ShowBackLed(0);
        }


        private void btnSetHeadingLeft_Click(object sender, RoutedEventArgs e)
        {
            App.spheroHelper.Bluetooth_SetHeading(22);
        }

        private void btnSetHeadingRight_Click(object sender, RoutedEventArgs e)
        {
            App.spheroHelper.Bluetooth_SetHeading(338);
        }
        
        // DRIVING


        private void joystick_NewCoordinates(object sender, EventArgs e)
        {
            currentDirection = ((MyCoordinates)e).Direction;
            App.spheroHelper.Bluetooth_Roll(currentDirection, ((MyCoordinates)e).Speed);
        }

        private void joystick_Stop(object sender, EventArgs e)
        {
            App.spheroHelper.Bluetooth_Roll(currentDirection, 0);
        }

        private void NavigateTo(string page)
        {
            NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }

    }
}