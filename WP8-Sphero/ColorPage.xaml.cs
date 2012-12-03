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