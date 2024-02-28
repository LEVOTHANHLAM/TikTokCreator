using LDPlayerAndADBController.ADBClient;
using LDPlayerAndADBController.AdbController;
using TikTokCreator.Models;
using TikTokCreator.Repositories;

namespace TikTokCreator.Services
{
    public class TikTokServices
    {
        private static string Package = "com.zhiliaoapp.musically.go";
        private DeviceInfo _device;
        private IAccountRepository _accountRepository;
        private ProfileModel _model;
        public TikTokServices(DeviceInfo device, IAccountRepository accountRepository, ProfileModel model)
        {
            _device = device;
            _accountRepository = accountRepository;
            _model = model;
        }
        public string Create()
        {
            try
            {
                if (ADBClientController.FindElementIsExistOrClickByClass(_device.Data, _device.AdbClient, "Use phone or email", "android.widget.TextView", 60, true))
                {
                    if(!string.IsNullOrEmpty(_model.Email))
                    {
                        if (ADBClientController.FindElementIsExistOrClickByClass(_device.Data, _device.AdbClient, "Email", "android.widget.TextView", 15, true))
                        {
                            ADBHelper.Delay();
                            ADBHelper.InputTextWithADBKeyboard(_device.Id, _model.Email);

                        }
                    }
                  
                }
            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }
    }
}
