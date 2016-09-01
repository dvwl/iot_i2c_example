using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace iot_i2c_example.Library.Core
{
    [DataContract]
    public class Home
    {
        /// <summary>
        /// Get or set home name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Provides access to list of rooms
        /// </summary>
        [DataMember]
        public List<Room> Rooms;

        public Home()
        {
            Rooms = new List<Room>();
            Name = "My Home";
        }

        /// <summary>
        /// Saves the current home configuration
        /// </summary>
        /// <param name="MyHome">Provide home object to be save for later use</param>
        public static async void SaveHome(Home MyHome)
        {
            MemoryStream _MemoryStream = new MemoryStream();
            DataContractSerializer Serializer = new DataContractSerializer(typeof(Home));
            Serializer.WriteObject(_MemoryStream, MyHome);

            StorageFile _File = await ApplicationData.Current.LocalFolder.CreateFileAsync("Home.bin", CreationCollisionOption.ReplaceExisting);

            using (Stream fileStream = await _File.OpenStreamForWriteAsync())
            {
                _MemoryStream.Seek(0, SeekOrigin.Begin);
                await _MemoryStream.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Dispose();
            }
        }

        /// <summary>
        /// Returns the last saved home configuration if available else returns the new Home object
        /// </summary>
        /// <returns>Last saved Home object if available or else new Home object</returns>
        public static async Task<Home> LoadHome()
        {
            StorageFolder _Folder = ApplicationData.Current.LocalFolder;
            StorageFile _File;

            try
            {
                _File = await _Folder.GetFileAsync("Home.bin");

                Stream stream = await _File.OpenStreamForReadAsync();

                DataContractSerializer Serializer = new DataContractSerializer(typeof(Home));

                return (Home)Serializer.ReadObject(stream);
            }
            catch (FileNotFoundException)
            {
                return new Home();
            }
        }
    }
}
