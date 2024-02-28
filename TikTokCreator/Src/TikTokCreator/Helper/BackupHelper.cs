using LDPlayerAndADBController;
using LDPlayerAndADBController.ADBClient;
using Serilog;
using System.Diagnostics;

namespace TikTokCreator.Helper
{
    public class BackupHelper
    {
        public static async Task BackupAccountAsync(string deviceId, string packageApp, string username)
        {
            CompressedFileZip(deviceId, packageApp);
            await LDController.DelayAsync(7);
            string copyBackup = $"shell su -c 'cp /data/data/{packageApp}/backup.tar.gz /sdcard/backup.tar.gz'";
            ADBHelper.ADB(deviceId, copyBackup);
            await LDController.DelayAsync(5);
            string fromFile = Path.Combine(Environment.CurrentDirectory, $"BackupData\\{username}");
            if (!Directory.Exists(fromFile))
            {
                Directory.CreateDirectory(fromFile);
            }
            ADBHelper.Delay(2);
            string pull = $" -s {deviceId} pull /sdcard/backup.tar.gz {fromFile}\\backup.tar.gz";
            RunCommand(pull);
            await LDController.DelayAsync(1);
            await ClearCashFacebookAsync(deviceId);
        }
        public static async Task BackupDeviceAsync(string index, string username)
        {
            string fromFile = Path.Combine(Environment.CurrentDirectory, $"BackupData\\{username}");
            string pathConfig = LDController.PathFolderLDPlayer + $"\\vms\\config\\leidian{index}.config";
            if (!Directory.Exists(fromFile))
            {
                Directory.CreateDirectory(fromFile);
            }
            string srcConfig = $"{fromFile}\\leidian.txt";
            var doc = File.ReadAllLines(pathConfig);
            File.WriteAllBytes(srcConfig, new byte[0]);
            File.WriteAllLines(srcConfig, doc);
        }
        public static async Task ClearCashFacebookAsync(string deviceId)
        {
            string cmd = "adb --deviceId {0} --command shell su -c rm -rf {1}";
            ADBHelper.ADB( deviceId, "shell su -c rm -rf /data/data/com.instagram.android/backup.tar.gz");
            ADBHelper.Delay(1);
            ADBHelper.ADB(deviceId, "shell su -c rm -rf /sdcard/backup.tar.gz");
            ADBHelper.Delay(1);
            ADBHelper.ADB(deviceId, "shell su -c rm -rf /data/data/com.instagram.android/databases");
            ADBHelper.Delay(1);
            ADBHelper.ADB(deviceId, "shell su -c rm -rf /data/data/com.instagram.android/app_light_prefs");
            ADBHelper.Delay(1);
            ADBHelper.ADB(deviceId, "shell su -c rm -rf /data/data/com.instagram.android/shared_prefs");
            ADBHelper.Delay(1);
            ADBHelper.ADB(deviceId, "shell su -c rm -rf /data/data/com.instagram.android/files/mobileconfig");
             ADBHelper.Delay(1);
        }
        public static async void CompressedFileZip(string deviceId, string packageApp)
        {
            string cmdCompressedFileZip = " -s " + deviceId + "  shell su -c 'tar -czvpf /data/data/" + packageApp + "/backup.tar.gz /data/data/" + packageApp + "/databases /data/data/" + packageApp + "/app_light_prefs /data/data/" + packageApp + "/shared_prefs /data/data/" + packageApp + "/files/mobileconfig'";
            RunCommand(cmdCompressedFileZip);

        }
        public static string RunCommand(string command)
        {
            string output = string.Empty;
            // Tạo một ProcessStartInfo để cấu hình lệnh cmd
            ProcessStartInfo processStartInfo = new ProcessStartInfo($"{LDController.PathFolderLDPlayer}\\adb.exe", command);
            Process process = new Process();
            try
            {
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                // Tạo một Process để thực thi lệnh cmd
                process.StartInfo = processStartInfo;
                process.Start();

                // Đọc dữ liệu đầu ra từ quá trình cmd
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi nếu có
                Log.Error(ex, ex.Message);
            }
            process.Close();
            process.Dispose();
            return output;
        }

    }
}
