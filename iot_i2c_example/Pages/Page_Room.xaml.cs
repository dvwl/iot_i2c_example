using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iot_i2c_example.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page_Room : Page
    {
        public Page_Room()
        {
            this.InitializeComponent();

            UpdateListBox();
        }

        private void UpdateListBox()
        {
            if (MainPage.MyHome.Rooms.Count > 0)
            {
                foreach (var Room in MainPage.MyHome.Rooms)
                {
                    LV_Rooms.Items.Add(Room);
                }
            }
        }

        private void LV_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainPage.SharedFrame.Navigate(typeof(Pages.Page_Devices), new object[] { LV_Rooms.SelectedItem });
        }
    }
}
