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