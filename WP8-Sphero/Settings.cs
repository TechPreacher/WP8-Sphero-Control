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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WP8_Sphero
{
    public class Settings
    {
        public bool ShowAboutPage { get; set; }

        public Settings()
        {
            this.ShowAboutPage = true;
        }

        public bool Save(bool showOnStart)
        {
            this.ShowAboutPage = showOnStart;

            bool bSuccess = false;

            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream fs = isf.CreateFile("Settings.dat"))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(Settings));
                        ser.Serialize(fs, this);
                        bSuccess = true;
                    }
                }
            }
            catch
            {
            }

            return bSuccess;
        }

        public bool Load()
        {
            bool bSuccess = false;

            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists("Settings.dat"))
                    {
                        using (IsolatedStorageFileStream fs = isf.OpenFile("Settings.dat", System.IO.FileMode.Open))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(Settings));
                            object SettingsLoaded = ser.Deserialize(fs);

                            if (null != SettingsLoaded && SettingsLoaded is Settings)
                            {
                                this.ShowAboutPage = (bool)((Settings)SettingsLoaded).ShowAboutPage;
                                bSuccess = true;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return bSuccess;
        }
    }
}
