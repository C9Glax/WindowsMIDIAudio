using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Collections.Generic;
using System.Linq;

namespace WindowsSoundControl
{
    public class AudioDeviceInfo
    {
        public static DeviceInfo[] GetAudioDevices()
        {
            List<CoreAudioDevice> audiodevices = new CoreAudioController().GetDevices(DeviceState.Active).ToList();
            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
            foreach (CoreAudioDevice device in audiodevices)
                deviceInfos.Add(new DeviceInfo()
                {
                    name = device.FullName,
                    guid = device.RealId.Replace("}.{", "@").Split('@')[1].Replace("}", "")
                });
            return deviceInfos.ToArray();
        }

        public struct DeviceInfo
        {
            public string name, guid;

            public override string ToString()
            {
                return this.name;
            }
        }
    }
}
