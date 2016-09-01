using iot_i2c_example.Library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace iot_i2c_example.Library.Communication
{
    public class I2C_Helper
    {
        private static bool Lock = false;
        private static string AQS;
        private static DeviceInformationCollection DIS;
        /// <summary>
        /// Defines protocol mode for the communication. Based off: https://www.hackster.io/AnuragVasanwala/home-automation
        /// </summary>
        public enum Mode : byte
        {
            /// <summary>
            /// Retrieves sensor data from specified I2C slave Arduino
            /// </summary>
            Mode0 = 0,
            /// <summary>
            /// Retrieves devices state from specified I2C slave Arduino
            /// </summary>
            Mode1 = 1,
            /// <summary>
            /// Sends IO signal to pin of specified I2C slave Arduino
            /// </summary>
            Mode2 = 2
        }

        /// <summary>
        /// Sends control signal to the specific Arduino and retrieves response bytes.
        /// </summary>
        /// <param name="_Room">Room to which data to be send</param>
        /// <param name="ControlMode">Select specific control mode.</param>
        /// <param name="Pin">Pin to be set. ONLY VALID FOR MODE-2</param>
        /// <param name="PinValue">Value to be set. ONLY VALID FOR MODE-2</param>
        /// <returns>Returns twelve response byte.</returns>
        public static async System.Threading.Tasks.Task<byte[]> WriteRead(Room _Room, Mode ControlMode, byte Pin = 0, byte PinValue = 0)
        {
            while (Lock != false)
            {

            }

            Lock = true;
            // Create response byte array of fourteen
            byte[] Response = new byte[11];

            try
            {
                // Initialize I2C
                var Settings = new I2cConnectionSettings(_Room.I2C_Slave_Address);
                Settings.BusSpeed = I2cBusSpeed.StandardMode;

                if (AQS == null || DIS == null)
                {
                    // I2C1 - Raspberry Pi, I2C5 - MinnowBoard Max, I2C0 or I2C1 - Dragonboard 410C
                    AQS = I2cDevice.GetDeviceSelector("I2C5");
                    DIS = await DeviceInformation.FindAllAsync(AQS);
                }

                using (I2cDevice Device = await I2cDevice.FromIdAsync(DIS[0].Id, Settings))
                {
                    Device.Write(new byte[] { byte.Parse(ControlMode.ToString().Replace("Mode", "")), Pin, PinValue });

                    Device.Read(Response);
                }
            }
            catch (Exception)
            {
                // SUPPRESS ERROR AND RETURN EMPTY RESPONSE ARRAY
            }

            Lock = false;
            return Response;
        }

        /// <summary>
        /// Sends control signal to the specific Arduino and retrieves response bytes.
        /// </summary>
        /// <param name="I2C_Slave_Address">Slave Arduino's address</param>
        /// <param name="ControlMode">Select specific control mode.</param>
        /// <param name="Pin">Pin to be set. ONLY VALID FOR MODE-2</param>
        /// <param name="PinValue">Value to be set. ONLY VALID FOR MODE-2</param>
        /// <returns>Returns twelve response byte.</returns>
        public static async System.Threading.Tasks.Task<byte[]> WriteRead(int I2C_Slave_Address, Mode ControlMode, byte Pin = 0, byte PinValue = 0)
        {
            while (Lock != false)
            {

            }

            Lock = true;
            // Create response byte array of eleven
            byte[] Response = new byte[11];

            try
            {
                // Initialize I2C
                var Settings = new I2cConnectionSettings(I2C_Slave_Address);
                Settings.BusSpeed = I2cBusSpeed.StandardMode;

                if (AQS == null || DIS == null)
                {
                    // I2C1 - Raspberry Pi, I2C5 - MinnowBoard Max, I2C0 or I2C1 - Dragonboard 410C
                    AQS = I2cDevice.GetDeviceSelector("I2C5");
                    DIS = await DeviceInformation.FindAllAsync(AQS);
                }

                using (I2cDevice Device = await I2cDevice.FromIdAsync(DIS[0].Id, Settings))
                {
                    Device.Write(new byte[] { byte.Parse(ControlMode.ToString().Replace("Mode", "")), Pin, PinValue });

                    Device.Read(Response);
                }
            }
            catch (Exception)
            {
                // SUPPRESS ERROR AND RETURN EMPTY RESPONSE ARRAY
            }

            Lock = false;
            return Response;
        }
    }
}
