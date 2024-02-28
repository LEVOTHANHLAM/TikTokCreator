﻿using AdvancedSharpAdbClient;
using TikTokCreator.Models;
using LDPlayerAndADBController;
using LDPlayerAndADBController.ADBClient;
using LDPlayerAndADBController.AdbController;
using Serilog;
using AdvancedSharpAdbClient.Models;

namespace TikTokCreator.MuiltiTask
{
    public class DeviceHelper
    {
        public static bool Connect(DeviceInfo device)
        {
            try
            {
                string idTemp = (int.Parse(device.IndexLDPlayer) * 2 + 5555).ToString();
                string deviceIdTemp = "127.0.0.1:" + idTemp;
                string emulator = "emulator-" + (int.Parse(idTemp) - 1);
                ADBHelper.ExecuteADB_Result($"disconnect {deviceIdTemp}");
                ADBHelper.ExecuteADB_Result($"connect {deviceIdTemp}");
                var adbClient = new AdbClient();
                adbClient.Connect("127.0.0.1:62001");
                var data = adbClient.GetDevices();
                foreach (var deviceData in data)
                {
                    if (deviceData.Serial == emulator || deviceData.Serial == deviceIdTemp)
                    {
                        if (deviceData.State == DeviceState.Online)
                        {
                            device.Id = deviceData.Serial;
                            device.Data = deviceData;
                            device.AdbClient = adbClient;
                            device.IndexLDPlayer = device.IndexLDPlayer;
                            return true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(DeviceHelper)}, params; {nameof(Connect)},deviceId; {device.Data.Serial}, Error; {ex.Message}, Exception; {ex}");
                return false;
            }
            return false;
        }
        public static List<LDplayerInfo> GetLDplayers()
        {
            List<LDplayerInfo> result = new List<LDplayerInfo>();
            try
            {
                var listLdplayer = LDController.GetDevices2();
                foreach (var ldplayer in listLdplayer)
                {
                    LDplayerInfo lDplayerInfo = new LDplayerInfo();
                    lDplayerInfo.Index = ldplayer.index.ToString();
                    lDplayerInfo.Name = ldplayer.name;
                    if (LDController.IsDevice_Running("index", lDplayerInfo.Index) == true)
                    {
                        lDplayerInfo.Status = "Online";
                    }
                    else
                    {
                        lDplayerInfo.Status = "Offline";
                    }
                    result.Add(lDplayerInfo);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return result;
            }
            return result;
        }
    }
}
