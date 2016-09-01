using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iot_i2c_example.Library.Core
{
    [DataContract]
    public class Room
    {
        /// <summary>
        /// Get or set user-friendly name for room
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        /// <summary>
        /// Get or set Arduino's slave I2C address for the room
        /// </summary>
        [DataMember]
        public int I2C_Slave_Address { get; set; }


        /// <summary>
        /// Maintains list of devices, room have
        /// </summary>
        [DataMember]
        public List<Device> Devices;

        /// <summary>
        /// Provides access to the sensors of the room
        /// </summary>
        public struct SensorStruct
        {
            public Sensors.AmbientLight AmbientLight;
            public Sensors.PassiveIR PassiveIR;
            public Sensors.Temperature Temperature;
        }

        /// <summary>
        /// Provides access to the sensors of the room
        /// </summary>
        public SensorStruct Sensors = new SensorStruct();

        public Room()
        {
            Devices = new List<Device>();
            Sensors.AmbientLight = new Sensors.AmbientLight();
            Sensors.PassiveIR = new Sensors.PassiveIR();
            Sensors.Temperature = new Sensors.Temperature();
        }

        /// <summary>
        /// Use this method instead of direct calling to Add method of 'Devices'.
        /// </summary>
        public void AddDevice(Device NewDevice)
        {
            NewDevice.Id = (ushort)Devices.Count;
            NewDevice.I2C_Slave_Address = I2C_Slave_Address;
            Devices.Add(NewDevice);
        }

        /// <summary>
        /// Updates room devices. Call this method after updating room parameters.
        /// </summary>
        public void UpdateDevices()
        {
            foreach (var Device in Devices)
            {
                Device.I2C_Slave_Address = I2C_Slave_Address;
            }
        }
    }
}
