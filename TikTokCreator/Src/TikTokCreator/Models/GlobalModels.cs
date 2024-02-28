using static TikTokCreator.ProxyDroid.ProxyHelper;

namespace TikTokCreator.Models
{
    public class GlobalModels
    {
        public static List<DeviceInfo> Devices = new List<DeviceInfo>();
        //--------------------------------------------------------//
        public static string PathLastName = Path.Combine(Environment.CurrentDirectory, "DataImport\\User\\LastName.txt");
        public static string PathFirstName = Path.Combine(Environment.CurrentDirectory, "DataImport\\User\\FirstName.txt");
        public static string PathFolderAvatar = Path.Combine(Environment.CurrentDirectory, "DataImport\\User\\FolderAvatar");
        public static string PathProxy = Path.Combine(Environment.CurrentDirectory, "DataImport\\User\\Proxy.txt");
        public static string PathUserName = Path.Combine(Environment.CurrentDirectory, "DataImport\\User\\UserName.txt");
        public static string PathEmail = Path.Combine(Environment.CurrentDirectory, "DataImport\\User\\Email.txt");
        public static string PathEmailRecovery = Path.Combine(Environment.CurrentDirectory, "DataImport\\User\\EmailRecovery.txt");


        //-------------------------------------------------------------//
        public static string PathDataFirstNameUS = Path.Combine(Environment.CurrentDirectory, "DataImport\\Admin\\Us\\FirstName.txt");
        public static string PathDataLastNameUs = Path.Combine(Environment.CurrentDirectory, "DataImport\\Admin\\Us\\Lastnames.txt");

        public static string PathDataFirstNameVN = Path.Combine(Environment.CurrentDirectory, "DataImport\\Admin\\Vn\\FirstName.txt");
        public static string PathDataLastNameVN = Path.Combine(Environment.CurrentDirectory, "DataImport\\Admin\\Vn\\Lastnames.txt");

        public static string PathDataUserName = Path.Combine(Environment.CurrentDirectory, "DataImport\\Admin\\UserName.txt");
        public static string NamePasswrod { get;set; }
        public static int NumberPasswrod { get;set; }
        //--------------------------------------------------------//
        public static List<ProxyInfo> Proxies = new List<ProxyInfo>();
        public static List<UserNameInfo> Usernames = new List<UserNameInfo>();
        public static List<string>UserNameRandom =  new List<string>();
        public static List<string> Firstnames = new List<string>();
        public static List<string> Lastnames = new List<string>();
        //-------------------------------------------------------//
        public static string Contry { get;set; }
        public static string Service { get; set; }
    }
}
