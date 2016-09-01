using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
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
    public sealed partial class Page_Configuration : Page
    {
        public Page_Configuration()
        {
            this.InitializeComponent();

            Status_TB.Text = "";

            UpdateListBox();
            UpdateImages();

            if (MainPage.MyHome.Rooms.Count > 0)
            {
                UpdateRoomDetailPane(MainPage.MyHome.Rooms[0]);
                if (MainPage.MyHome.Rooms[0].Devices.Count > 0)
                {
                    UpdateDeviceDetailPane(MainPage.MyHome.Rooms[0], true);
                }
            }
        }

        /// <summary>
        /// Updates items in listboxes.
        /// </summary>
        /// <param name="Mode">Select 0 to update both Room and Device listbox. 1 for Room and 2 for Device only</param>
        private void UpdateListBox(byte Mode = 0)
        {
            Status_TB.Text = "";

            if (Mode == 0 || Mode == 1)
            {
                LB_Rooms.Items.Clear();
            }

            if (MainPage.MyHome.Rooms.Count > 0)
            {
                if (Mode == 0 || Mode == 1)
                {
                    foreach (var Room in MainPage.MyHome.Rooms)
                    {
                        LB_Rooms.Items.Add(Room);
                    }

                    LB_Rooms.SelectedIndex = 0;
                }

                if (Mode == 0 || Mode == 2)
                {
                    LB_Devices.Items.Clear();
                    if (MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].Devices.Count > 0)
                    {
                        foreach (var Device in MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].Devices)
                        {
                            LB_Devices.Items.Add(Device);
                            Debug.WriteLine(Device.Id);
                            Debug.WriteLine(Device.Name);
                            Debug.WriteLine(Device.Pin.ToString());
                            Debug.WriteLine(Device.ImagePath);
                        }
                        LB_Devices.SelectedIndex = 0;
                    }
                }
            }
        }

        private void UpdateImages()
        {
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Attic.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Basement.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Fireplace.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Flower1.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Flower2.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Flower3.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Kitchen.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Pillow.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Room.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/TV.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Window.png" });

            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Device/Bulb_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Device/Fan_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Device/Lamp1_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Device/Lamp2_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Device/Plug_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Resources/Image/Tiles/Device/Socket_130.png" });
        }

        public class ImageListClass
        {
            public string ImagePath { get; set; }
        }

        private void Btn_AddRoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Library.Core.Room NewRoom = new Library.Core.Room();

            NewRoom.I2C_Slave_Address = 0x40;
            NewRoom.ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Flower1.png";
            NewRoom.Name = "My Room";

            MainPage.MyHome.Rooms.Add(NewRoom);

            UpdateListBox(1);
        }

        private void Btn_RemoveRoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Rooms.Items.Count > 0 && LB_Rooms.SelectedIndex != -1)
            {
                MainPage.MyHome.Rooms.Remove((Library.Core.Room)LB_Rooms.SelectedItem);
                LB_Rooms.Items.Remove(LB_Rooms.SelectedItem);
            }
            UpdateListBox();
        }

        private void Btn_AddDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Library.Core.Device NewDevice = new Library.Core.Device();

            NewDevice.Name = "My Device";
            NewDevice.ImagePath = "ms-appx:///Resources/Image/Tiles/Device/Plug_130.png";
            NewDevice.Pin = Library.Core.Device.PinsEnum.D0;

            MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].AddDevice(NewDevice);

            // Only update device listboc
            UpdateListBox(2);
        }

        private void Btn_RemoveDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Devices.Items.Count > 0 && LB_Devices.SelectedIndex != -1)
            {
                MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].Devices.Remove((Library.Core.Device)LB_Devices.SelectedItem);
                LB_Devices.Items.Remove(LB_Devices.SelectedItem);
            }
            UpdateListBox();
        }

        private void UpdateRoomDetailPane(Library.Core.Room SelectedRoom)
        {
            Txt_RoomName.Text = SelectedRoom.Name;
            Txt_RoomI2CAddress.Text = "0x" + SelectedRoom.I2C_Slave_Address.ToString("X");

            foreach (var _Image in LV_RoomImage.Items)
            {
                if (((ImageListClass)_Image).ImagePath == SelectedRoom.ImagePath)
                {
                    LV_RoomImage.SelectedItem = _Image;
                    break;
                }
            }
        }

        private void UpdateDeviceDetailPane(Library.Core.Room SelectedRoom, bool SelectFirst = false, bool SkipClear = false)
        {
            if (SkipClear == false)
            {
                ClearDeviceDetailPane();
            }
            if (SelectedRoom.Devices.Count > 0)
            {
                if (SkipClear == false)
                {
                    foreach (var _Device in SelectedRoom.Devices)
                    {
                        LB_Devices.Items.Add(_Device);
                    }
                }

                if (SelectFirst)
                {
                    LB_Devices.SelectedIndex = 0;
                }

                Library.Core.Device SelectedDevice = (Library.Core.Device)LB_Devices.SelectedItem;

                txt_DeviceName.Text = SelectedDevice.Name;

                foreach (ComboBoxItem item in cmb_DevicePin.Items)
                {
                    if ((string)item.Content == SelectedDevice.Pin.ToString())
                    {
                        cmb_DevicePin.SelectedItem = item;
                        break;
                    }
                }

                foreach (var _Image in LV_DeviceImage.Items)
                {
                    if (((ImageListClass)_Image).ImagePath == SelectedDevice.ImagePath)
                    {
                        LV_DeviceImage.SelectedItem = _Image;
                        break;
                    }
                }
            }
        }

        private void ClearDeviceDetailPane()
        {
            if (LB_Devices.Items != null && LB_Devices.Items.Count > 0)
            {
                LB_Devices.Items.Clear();
            }
            txt_DeviceName.Text = "";
            cmb_DevicePin.SelectedIndex = -1;
            LV_DeviceImage.SelectedIndex = -1;
        }

        private void LB_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Rooms.Items.Count > 0)
            {
                UpdateRoomDetailPane((Library.Core.Room)LB_Rooms.SelectedItem);
                UpdateDeviceDetailPane((Library.Core.Room)LB_Rooms.SelectedItem, true);
            }
        }

        private void LB_Devices_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Devices.Items.Count > 0)
            {
                UpdateDeviceDetailPane((Library.Core.Room)LB_Rooms.SelectedItem, false, true);
            }
        }

        private void Btn_Save_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Rooms.Items.Count > 0 && LB_Rooms.SelectedIndex != -1)
            {
                // Update Room Detail
                Library.Core.Room SelectedRoom = (Library.Core.Room)LB_Rooms.SelectedItem;

                SelectedRoom.Name = Txt_RoomName.Text;

                int NewAddress = 0;
                IFormatProvider _Culture = new CultureInfo("en-US");
                int.TryParse(Txt_RoomI2CAddress.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, _Culture, out NewAddress);
                SelectedRoom.I2C_Slave_Address = NewAddress;

                SelectedRoom.ImagePath = ((ImageListClass)LV_RoomImage.SelectedItem).ImagePath;

                // Update Device Detail
                if (LB_Devices.Items.Count > 0)
                {
                    if (LB_Devices.SelectedIndex != -1)
                    {
                        if (cmb_DevicePin.SelectedIndex != -1)
                        {
                            Library.Core.Device SelectedDevice = (Library.Core.Device)LB_Devices.SelectedItem;

                            SelectedDevice.Name = txt_DeviceName.Text;
                            SelectedDevice.Pin = (Library.Core.Device.PinsEnum)Enum.Parse(typeof(Library.Core.Device.PinsEnum), ((ComboBoxItem)cmb_DevicePin.SelectedItem).Content.ToString());

                            SelectedDevice.ImagePath = ((ImageListClass)LV_DeviceImage.SelectedItem).ImagePath;
                        }
                        else
                        {
                            Status_TB.Text = "Error. Device pin not selected.";
                            return;
                        }
                    }
                    else
                    {
                        Status_TB.Text = "Error. Device not selected.";
                        return;
                    }
                }
            }

            // Save
            Status_TB.Text = "Changes have been successfully saved.";
            Library.Core.Home.SaveHome(MainPage.MyHome);
            return;
        }
    }
}
