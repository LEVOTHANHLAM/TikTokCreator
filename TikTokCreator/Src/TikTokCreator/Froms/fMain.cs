using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Models;
using AppDestop.TelegramCreator.yuenankaApi;
using ICSharpCode.SharpZipLib.Zip;
using LDPlayerAndADBController;
using LDPlayerAndADBController.ADBClient;
using LDPlayerAndADBController.AdbController;
using Serilog;
using SmorcIRL.TempMail;
using SmorcIRL.TempMail.Models;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using TikTokCreator.ApiQnibot;
using TikTokCreator.Entities;
using TikTokCreator.FiveSimApi;
using TikTokCreator.Forms;
using TikTokCreator.Froms;
using TikTokCreator.GetSmsCode;
using TikTokCreator.Helper;
using TikTokCreator.Models;
using TikTokCreator.MuiltiTask;
using TikTokCreator.OtpServices;
using TikTokCreator.OtpServices.CountryHelper;
using TikTokCreator.Repositories;
using TikTokCreator.Sms365Api;
using TikTokCreator.ViotpApi;
using Button = System.Windows.Forms.Button;
using Label = System.Windows.Forms.Label;
using Panel = System.Windows.Forms.Panel;

namespace TikTokCreator
{
    public partial class fMain : Form
    {
        private readonly IAccountRepository _accountRepository;
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        private int columnCount = 0;
        private int rowCount = 0;
        private int originalFormWidth;
        private List<string> _listProxy = new List<string>();
        private List<string> _listEmail = new List<string>();
        private List<string> _listEmailRecovery = new List<string>();
        private List<string> _listFirtname = new List<string>();
        private List<string> _listLastname = new List<string>();
        private List<string> _listUsername = new List<string>();
        private List<string> _listAvatar = new List<string>();
        private List<string> _listPost = new List<string>();
        private object _lockObject;
        private List<ProfileModel> profileModels;
        private JsonHelper jsonHelper;
        private Random random;
        private CancellationTokenSource cancellationTokenSource;
        private string _OtpCountry = string.Empty;
        private List<CountryCode> listCountryCode;
        private List<OtpServices.CountryHelper.ComboBoxItem> listItem;
        private bool ckbCatchAll = false;
        private int _numberFollowForm;
        private int _numberFollowTo;
        private List<Task> _tasks = new List<Task>();
        public fMain(IAccountRepository accountRepository)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "settings\\configGeneral.json");
            jsonHelper = new JsonHelper(path, isJsonString: false);
            InitializeComponent();
            _accountRepository = accountRepository;
            InitializeSavedValues();
            originalFormWidth = this.Width;
            btnDisplay.Text = "5";
            writeFile();
            profileModels = new List<ProfileModel>();
            _lockObject = new object();
            RichTextBoxHelper._RichTextBox = rtbLogs;
            txtFolderAvatar.Enabled = cbUpdateAvatar.Checked;
            random = new Random();
            listItem = new List<OtpServices.CountryHelper.ComboBoxItem>();
            listCountryCode = new List<CountryCode>();
        }
        private void writeFile()
        {
            if (File.Exists(GlobalModels.PathProxy))
            {
                var lines = File.ReadAllLines(GlobalModels.PathProxy);
                _listProxy.Clear();
                foreach (var line in lines)
                {
                    _listProxy.Add(line);
                }
            }
            if (File.Exists(GlobalModels.PathFirstName))
            {
                var lines = File.ReadAllLines(GlobalModels.PathFirstName);
                _listFirtname.Clear();
                foreach (var line in lines)
                {
                    _listFirtname.Add(line);
                }
            }
            if (File.Exists(GlobalModels.PathLastName))
            {
                var lines = File.ReadAllLines(GlobalModels.PathLastName);
                _listLastname.Clear();
                foreach (var line in lines)
                {
                    _listLastname.Add(line);
                }
            }
            if (File.Exists(GlobalModels.PathUserName))
            {
                var lines = File.ReadAllLines(GlobalModels.PathUserName);
                _listUsername.Clear();
                foreach (var line in lines)
                {
                    _listUsername.Add(line);
                }
            }
            if (File.Exists(GlobalModels.PathEmail))
            {
                var lines = File.ReadAllLines(GlobalModels.PathEmail);
                _listEmail.Clear();
                foreach (var line in lines)
                {
                    _listEmail.Add(line);
                }
            }
            if (File.Exists(GlobalModels.PathEmailRecovery))
            {
                var lines = File.ReadAllLines(GlobalModels.PathEmailRecovery);
                _listEmailRecovery.Clear();
                foreach (var line in lines)
                {
                    _listEmailRecovery.Add(line);
                }
            }
        }

        private void InitializeSavedValues()
        {
            numberAccount.Value = jsonHelper.GetIntType("numberAccount", 50);
            numberThread.Value = jsonHelper.GetIntType("numberThread", 5);
            rdoNoProxy.Checked = jsonHelper.GetBooleanValue("rdoNoProxy", true);
            rdoProxy.Checked = jsonHelper.GetBooleanValue("rdoProxy", false);
            radioCustomizePass.Checked = jsonHelper.GetBooleanValue("radioCustomizePass", false);
            txtPass.Text = jsonHelper.GetValuesFromInputString("txtPass");
            radioRandomPass.Checked = jsonHelper.GetBooleanValue("radioRandomPass", true);
            NumberPass.Value = jsonHelper.GetIntType("NumberPass", 10);
            cb2FA.Checked = jsonHelper.GetBooleanValue("cb2FA", false);
            radioRadomFullNameUs.Checked = jsonHelper.GetBooleanValue("radioRadomFullNameUs", true);
            radioCustomizeFullName.Checked = jsonHelper.GetBooleanValue("radioCustomizeFullName", false);
            radioRadomFullNameVN.Checked = jsonHelper.GetBooleanValue("radioRadomFullNameVN", false);
            radioRandomUserName.Checked = jsonHelper.GetBooleanValue("radioRandomUserName", true);
            radioCustomizeUserName.Checked = jsonHelper.GetBooleanValue("radioCustomizeUserName", false);
            cbUpdateAvatar.Checked = jsonHelper.GetBooleanValue("cbUpdateAvatar", false);
            txtFolderAvatar.Text = jsonHelper.GetValuesFromInputString("txtFolderAvatar");
            rbRegWithPhone.Checked = true;
            rbRegWithEmail.Checked = false;
            gbPhone.Visible = true;
            btnAddAccount.Visible = false;
            var num = jsonHelper.GetIntType("Action", 0);
            if (num == 1)
            {
                rbRegWithPhone.Checked = false;
                rbRegWithEmail.Checked = true;
                gbPhone.Visible = false;
                btnAddAccount.Visible = true;
                rbRegWithTempMail.Checked = false;
            }
            else if (num == 2)
            {
                rbRegWithPhone.Checked = false;
                rbRegWithEmail.Checked = false;
                gbPhone.Visible = false;
                btnAddAccount.Visible = false;
                rbRegWithTempMail.Checked = true;
            }
            _OtpCountry = jsonHelper.GetValuesFromInputString("OtpCountry", "+84");
            CBoxOtpService.SelectedIndex = jsonHelper.GetIntType("Service", 0);
            txtApikey.Text = jsonHelper.GetValuesFromInputString("Key");
            rbViettel.Checked = true;
            rbLaos.Checked = false;
            rbMobi.Checked = false;
            rbITelecom.Checked = false;
            rbVina.Checked = false;
            rbVNMB.Checked = false;
            var contries = jsonHelper.GetIntType("CountryViotp", 0);
            switch (contries)
            {
                case 1:
                    {
                        rbViettel.Checked = false;
                        rbLaos.Checked = true;
                        rbMobi.Checked = false;
                        rbITelecom.Checked = false;
                        rbVina.Checked = false;
                        rbVNMB.Checked = false;
                        break;
                    }
                case 2:
                    {
                        rbLaos.Checked = false;
                        rbViettel.Checked = false;
                        rbMobi.Checked = true;
                        rbITelecom.Checked = false;
                        rbVina.Checked = false;
                        rbVNMB.Checked = false;
                        break;
                    }
                case 3:
                    {
                        rbLaos.Checked = false;
                        rbViettel.Checked = false;
                        rbMobi.Checked = false;
                        rbITelecom.Checked = true;
                        rbVina.Checked = false;
                        rbVNMB.Checked = false;
                        break;
                    }
                case 4:
                    {
                        rbLaos.Checked = false;
                        rbViettel.Checked = false;
                        rbMobi.Checked = false;
                        rbITelecom.Checked = false;
                        rbVina.Checked = true;
                        rbVNMB.Checked = false;
                        break;
                    }
                case 5:
                    {
                        rbLaos.Checked = false;
                        rbViettel.Checked = false;
                        rbMobi.Checked = false;
                        rbITelecom.Checked = false;
                        rbVina.Checked = false;
                        rbVNMB.Checked = true;
                        break;
                    }
            }

            ckbAddEmail.Checked = jsonHelper.GetBooleanValue("ckbAddEmail", false);
            btnSettingEmail.Enabled = ckbAddEmail.Checked;
            ckbCatchAll = SettingsTool.GetSettings("configGeneral").GetBooleanValue("ckbCatchAll");
            ckbUploadPost.Checked = jsonHelper.GetBooleanValue("ckbUploadPost", false);
            txtFolderPost.Enabled = ckbUploadPost.Checked;
            ckbFollowSuggested.Checked = jsonHelper.GetBooleanValue("ckbFollowSuggested", false);
            plFollow.Enabled = ckbFollowSuggested.Checked;
            txtFolderPost.Text = jsonHelper.GetValuesFromInputString("txtFolderPost");
            numberFollowForm.Value = jsonHelper.GetIntType("numberFollowForm", 5);
            numberFollowTo.Value = jsonHelper.GetIntType("numberFollowTo", 5);

        }
        private void saveProperties()
        {
            SettingsTool.GetSettings("configGeneral").AddValue("numberThread", numberThread.Value);
            SettingsTool.GetSettings("configGeneral").AddValue("rdoNoProxy", rdoNoProxy.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("rdoProxy", rdoProxy.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("radioCustomizePass", radioCustomizePass.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("txtPass", txtPass.Text.Trim());
            SettingsTool.GetSettings("configGeneral").AddValue("radioRandomPass", radioRandomPass.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("NumberPass", NumberPass.Value);
            SettingsTool.GetSettings("configGeneral").AddValue("cb2FA", cb2FA.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("radioRadomFullNameUs", radioRadomFullNameUs.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("radioCustomizeFullName", radioCustomizeFullName.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("radioRadomFullNameVN", radioRadomFullNameVN.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("radioRandomUserName", radioRandomUserName.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("radioCustomizeUserName", radioCustomizeUserName.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("cbUpdateAvatar", cbUpdateAvatar.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("txtFolderAvatar", txtFolderAvatar.Text.Trim());
            int num = 0;
            if (rbRegWithEmail.Checked)
            {
                num = 1;
            }
            else if (rbRegWithTempMail.Checked)
            {
                num = 2;
            }
            SettingsTool.GetSettings("configGeneral").AddValue("Action", num);
            SettingsTool.GetSettings("configGeneral").AddValue("OtpCountry", _OtpCountry);
            SettingsTool.GetSettings("configGeneral").AddValue("Service", CBoxOtpService.SelectedIndex);
            SettingsTool.GetSettings("configGeneral").AddValue("Key", txtApikey.Text.Trim());
            int ctountryViotp = 0;
            if (rbLaos.Checked)
            {
                ctountryViotp = 1;
            }
            else if (rbMobi.Checked)
            {
                ctountryViotp = 2;
            }
            else if (rbITelecom.Checked)
            {
                ctountryViotp = 3;
            }
            else if (rbVina.Checked)
            {
                ctountryViotp = 4;
            }
            else if (rbVNMB.Checked)
            {
                ctountryViotp = 5;
            }
            SettingsTool.GetSettings("configGeneral").AddValue("CountryViotp", ctountryViotp);
            SettingsTool.GetSettings("configGeneral").AddValue("ckbAddEmail", ckbAddEmail.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("numberAccount", numberAccount.Value);
            SettingsTool.GetSettings("configGeneral").AddValue("ckbUploadPost", ckbUploadPost.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("ckbFollowSuggested", ckbFollowSuggested.Checked);
            SettingsTool.GetSettings("configGeneral").AddValue("numberFollowForm", numberFollowForm.Value);
            SettingsTool.GetSettings("configGeneral").AddValue("numberFollowTo", numberFollowTo.Value);
            SettingsTool.GetSettings("configGeneral").AddValue("txtFolderPost", txtFolderPost.Text);
            SettingsTool.UpdateSetting("configGeneral");
        }
        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                btnStart.BackColor = Color.DarkSeaGreen;
                if (!AdbServer.Instance.GetStatus().IsRunning)
                {
                    AdbServer server = new AdbServer();
                    StartServerResult result = server.StartServer($"{LDController.PathFolderLDPlayer}\\adb.exe", false);
                    if (result != StartServerResult.Started)
                    {
                        MessageCommon.ShowMessageBox("Can't start adb server", 2);
                        btnStart.BackColor = Color.Lime;
                        return;
                    }
                }
                saveProperties();
                _numberFollowForm = (int)numberFollowForm.Value;
                _numberFollowTo = (int)numberFollowTo.Value;
                GlobalModels.Service = CBoxOtpService.Text.Replace("(recommend)", "");
                tableLayoutPanel.Dock = DockStyle.Fill;
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                tableLayoutPanel.AutoScroll = true;
                var ldplayers = LDController.GetDevices2();
                if (ldplayers.Count == 0 || ldplayers.Count < (int)numberThread.Value)
                {
                    MessageCommon.ShowMessageBox("Please check LDPlayer!", 2);
                    btnStart.Enabled = true;
                    btnStart.BackColor = Color.Lime;
                    return;
                }
                if (rbRegWithEmail.Checked && _listEmail.Count < 1 && _listEmail.Count < numberAccount.Value)
                {
                    MessageCommon.ShowMessageBox("Please check list email import!", 2);
                    btnStart.Enabled = true;
                    btnStart.BackColor = Color.Lime;
                    return;
                }

                Random random = new Random();
                if (rdoNoProxy.Checked)
                {
                    _listProxy.Clear();
                }
                if (radioRandomUserName.Checked)
                {
                    var usernames = FileHelper.ReadFile(GlobalModels.PathDataUserName);
                    if (usernames.Count > 0)
                    {
                        _listUsername.Clear();
                        _listUsername.AddRange(usernames);
                    }
                }
                if (radioRadomFullNameVN.Checked)
                {
                    var firtsnames = FileHelper.ReadFile(GlobalModels.PathDataFirstNameVN);
                    var lastnames = FileHelper.ReadFile(GlobalModels.PathDataLastNameVN);
                    if (firtsnames.Count > 0 && lastnames.Count > 0)
                    {
                        _listFirtname.Clear();
                        _listLastname.Clear();
                        _listFirtname.AddRange(firtsnames);
                        _listLastname.AddRange(lastnames);
                    }

                }
                else if (radioRadomFullNameUs.Checked == true)
                {
                    var firtsnames = FileHelper.ReadFile(GlobalModels.PathDataFirstNameUS);
                    var lastnames = FileHelper.ReadFile(GlobalModels.PathDataLastNameUs);
                    if (firtsnames.Count > 0 && lastnames.Count > 0)
                    {
                        _listFirtname.Clear();
                        _listLastname.Clear();
                        _listFirtname.AddRange(firtsnames);
                        _listLastname.AddRange(lastnames);
                    }
                }
                //-----------------------------------------------//
                List<string> list = new List<string>();
                Queue<string> queueProxy = new Queue<string>(); ;
                if (rdoProxy.Checked)
                {
                    if (_listProxy.Count < 1)
                    {
                        MessageCommon.ShowMessageBox("Please check list Proxy import!", 2);
                        btnStart.Enabled = true;
                        btnStart.BackColor = Color.Lime;
                        return;
                    }
                    list.AddRange(_listProxy);
                    while (numberAccount.Value > list.Count)
                    {
                        list.AddRange(_listProxy);
                        Log.Information($"porxy {list.Count} numberAccount.Value {numberAccount.Value}");
                    }
                    queueProxy = new Queue<string>(list);
                }
                Queue<string> queueAvatar = new Queue<string>();
                if (cbUpdateAvatar.Checked && Directory.Exists(txtFolderAvatar.Text.Trim()))
                {
                    _listAvatar.Clear();
                    var files = FileHelper.ReadImageFiles(txtFolderAvatar.Text.Trim());
                    if (files.Count == 0)
                    {
                        MessageCommon.ShowMessageBox("Please check list  Avatar import!", 2);
                        btnStart.Enabled = true;
                        btnStart.BackColor = Color.Lime;
                        return;
                    }
                    _listAvatar.AddRange(files);
                    while (numberAccount.Value > _listAvatar.Count)
                    {
                        _listAvatar.AddRange(files);
                        Log.Information($"files {files.Count} numberAccount.Value {numberAccount.Value}");
                    }
                    queueAvatar = new Queue<string>(_listAvatar);
                }
                Queue<string> queuePost = new Queue<string>();
                if (ckbUploadPost.Checked && Directory.Exists(txtFolderPost.Text.Trim()))
                {
                    _listPost.Clear();
                    var files = FileHelper.ReadImageFiles(txtFolderPost.Text.Trim());
                    if (files.Count == 0)
                    {
                        MessageCommon.ShowMessageBox("Please check list  Post import!", 2);
                        btnStart.Enabled = true;
                        btnStart.BackColor = Color.Lime;
                        return;
                    }
                    _listPost.AddRange(files);
                    while (numberAccount.Value > _listPost.Count)
                    {
                        _listPost.AddRange(files);
                    }
                    queuePost = new Queue<string>(_listPost);
                }
                profileModels.Clear();
                Queue<string> queueEmail = new Queue<string>();
                if (rbRegWithEmail.Checked && ckbCatchAll)
                {
                    var files = _listEmail;
                    if (files.Count == 0)
                    {
                        MessageCommon.ShowMessageBox("Please check list  Email  import!", 2);
                        btnStart.Enabled = true;
                        btnStart.BackColor = Color.Lime;
                        return;
                    }
                    while (numberAccount.Value > files.Count)
                    {
                        files.AddRange(_listEmail);
                    }
                    queueEmail = new Queue<string>(files);
                }
                else if (rbRegWithEmail.Checked)
                {
                    queueEmail = new Queue<string>(_listEmail);
                }
                Queue<string> queueEmailRecovery = new Queue<string>();

                if (rbRegWithPhone.Checked && ckbAddEmail.Checked)
                {
                    var files = _listEmailRecovery;
                    if (files.Count == 0)
                    {
                        MessageCommon.ShowMessageBox("Please check list  Email Recovery import!", 2);
                        btnStart.Enabled = true;
                        btnStart.BackColor = Color.Lime;
                        return;
                    }
                    while (numberAccount.Value > files.Count)
                    {
                        files.AddRange(_listEmailRecovery);
                    }
                    queueEmailRecovery = new Queue<string>(files);
                }

                for (int i = 0; i < numberAccount.Value; i++)
                {
                    Log.Information($"Add profile {i}");
                    ProfileModel profile = new ProfileModel();
                    profile.Backup = Path.Combine(Environment.CurrentDirectory, "DataImport\\Admin\\Backup");
                    profile.Firstname = _listFirtname[random.Next(0, _listFirtname.Count)];
                    profile.Lastname = _listLastname[random.Next(0, _listLastname.Count)];
                    profile.Username = _listUsername[random.Next(0, _listLastname.Count)] + Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", random.Next(5, 10));
                    if (radioCustomizePass.Checked)
                    {
                        profile.Password = txtPass.Text.Trim();
                    }
                    else
                    {
                        profile.Password = Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", (int)NumberPass.Value);
                    }
                    if (_listProxy.Count > 0 && rdoProxy.Checked)
                    {
                        string text = queueProxy.Dequeue().Replace("'", "''");
                        profile.Proxy = text;
                    }
                    if (_listAvatar.Count > 0 && cbUpdateAvatar.Checked)
                    {
                        string text = queueAvatar.Dequeue().Replace("'", "''");
                        profile.FileImage = text;
                    }
                    if (_listPost.Count > 0 && ckbUploadPost.Checked)
                    {
                        string text = queuePost.Dequeue().Replace("'", "''");
                        profile.FileImagePost = text;
                    }
                    profile.IsUsing = false;
                    if (_listEmail.Count > 0 && rbRegWithEmail.Checked)
                    {
                        string text = queueEmail.Dequeue().Replace("'", "''");
                        var account = text.Split('|');
                        if (account != null && account.Length > 3)
                        {
                            profile.Email = account[0];
                            profile.PasswordEmail = account[1];
                            profile.Port = account[3];
                            profile.Server = account[2];
                            if (ckbCatchAll)
                            {
                                profile.Domain = account[0];
                                profile.Email = profile.Username + profile.Domain;
                            }
                        }
                    }
                    if (rbRegWithPhone.Checked && ckbAddEmail.Checked)
                    {
                        string text = queueEmailRecovery.Dequeue().Replace("'", "''");
                        var account = text.Split('|');
                        if (account != null && account.Length > 3)
                        {
                            if (ckbCatchAll)
                            {
                                profile.Domain = account[0];
                            }
                            else
                            {
                                profile.Email = account[0];
                            }
                            profile.PasswordEmail = account[1];
                            profile.Port = account[3];
                            profile.Server = account[2];
                        }
                    }
                    profileModels.Add(profile);
                }

                Log.Information($"Start Close ALL");
                LDController.CloseAll();
                Log.Information($"End Close ALL");
                GlobalModels.Devices.Clear();
                foreach (var item in ldplayers)
                {
                    DeviceInfo device = new DeviceInfo();
                    device.IndexLDPlayer = item.index.ToString();
                    device.AdbClient = new AdbClient();
                    device.Data = new DeviceData();
                    device.view = new ViewInfo();
                    device.view.Embeddedpanel = new Panel();
                    device.view.StatusLabel = new Label();
                    device.view.LdplayerHandle = new IntPtr();
                    device.view.Panel = new Panel();
                    device.view.PanelButton = new Panel();
                    device.view.BtnClose = new Button();
                    GlobalModels.Devices.Add(device);
                    if (GlobalModels.Devices.Count >= (int)numberThread.Value)
                    {
                        break;
                    }
                }
                if (GlobalModels.Devices.Count <= 0)
                {
                    MessageCommon.ShowMessageBox("Please check LDPlayer.", 3);
                    btnStart.Enabled = true;
                    btnStart.BackColor = Color.Lime;
                    return;
                }
                cancellationTokenSource = new CancellationTokenSource();
                foreach (var item in GlobalModels.Devices)
                {
                    Log.Information($"{item.IndexLDPlayer} Config File");
                    try
                    {
                        lock (_lockObject)
                        {
                            File.Delete($"{LDController.PathFolderLDPlayer}\\vms\\leidian{item.IndexLDPlayer}\\data.vmdk");
                            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(Environment.CurrentDirectory, "LDplayer\\data.zip"), $"{LDController.PathFolderLDPlayer}\\vms\\leidian{item.IndexLDPlayer}");
                            File.Delete($"{LDController.PathFolderLDPlayer}\\vms\\config\\leidian{item.IndexLDPlayer}.config");
                            string newPath = Path.Combine($"{LDController.PathFolderLDPlayer}\\vms\\config", $"leidian{item.IndexLDPlayer}.config");
                            File.Copy(Path.Combine(Environment.CurrentDirectory, "LDplayer\\leidian.txt"), newPath);
                        }
                    }
                    catch (Exception)
                    {

                    }

                    int i = 6;
                    while (i > 0)
                    {
                        LDController.Open("index", item.IndexLDPlayer);
                        Log.Information($"Open {item.IndexLDPlayer}");
                        ADBHelper.Delay(5, 10);
                        Log.Information($"Delay {item.IndexLDPlayer}");
                        if (LDController.IsDevice_Running("index", item.IndexLDPlayer))
                        {
                            Log.Information($"IsDevice_Running {item.IndexLDPlayer}");
                            if (columnCount >= int.Parse(btnDisplay.Text.Trim()))
                            {
                                rowCount++;
                                columnCount = 0;
                                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                            }
                            item.view.originalParentHandle = IntPtr.Zero;
                            addViewControl(item, tableLayoutPanel);
                            columnCount++;
                            ADBHelper.Delay(1, 5);
                            pDevice.Controls.Add(tableLayoutPanel);

                            _tasks.Add(Task.Run(async () =>
                            {
                                await StartDevice(item);
                            }, cancellationTokenSource.Token));
                            break;
                        }
                        else
                        {
                            Log.Information($"No Open {item.IndexLDPlayer}");
                        }
                        i--;
                    }
                }
                btnStart.Enabled = true;
                btnStart.BackColor = Color.Lime;
                await Task.WhenAll(_tasks);
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                Log.Error(ex, ex.Message);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
            }
        }
        private async Task StartDevice(DeviceInfo device)
        {
            Log.Information($"StartDevice {device.IndexLDPlayer}");
            try
            {
                ADBHelper.Delay(4, 8);
                while (true)
                {
                    //////////////////////check//////////////////////////////////
                    int randomNumber = random.Next(10);

                    // Chỉ thực hiện kiểm tra nếu số ngẫu nhiên là 0
                    if (randomNumber == 0)
                    {
                        HttpHelper httpHelper = new HttpHelper();
                        string hardwareId = httpHelper.GetHardwareId();
                        var softwareId = Constant.SoftwareId;
                        var checkLicenseResult = await httpHelper.CheckLicense(Constant.licenseKey, hardwareId, softwareId);
                        if (checkLicenseResult != null)
                        {
                            if (checkLicenseResult.Data is false)
                            {
                                LDController.CloseAll();
                                MessageCommon.ShowMessageBox(checkLicenseResult.Message, 3);
                                StopAll();

                                return;
                            }
                        }
                        else
                        {
                            LDController.CloseAll();
                            MessageCommon.ShowMessageBox("There's an error, please try again later.", 3);
                            StopAll();
                            return;
                        }
                    }


                    ProfileModel selectAccount;
                    writeLog(device, $"Connect", null, device.view.StatusLabel, 0);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    while (true)
                    {
                        ADBHelper.Delay();
                        if (DeviceHelper.Connect(device))
                        {
                            //  DataGridViewHelper.SetCellValueByColumnValue(dtgvLDPlayers, device.IndexLDPlayer, "tIndex", "tDeviceId", device.Id, 2);
                            //   DataGridViewHelper.SetCellValueByColumnValue(dtgvLDPlayers, device.IndexLDPlayer, "tIndex", "tName", device.Data.Model, 2);
                            writeLog(device, "Connect Success", null, device.view.StatusLabel, 2);
                            ADBHelper.TurnOnADBKeyboard(device.Id);
                            device.Status = "Connect";
                            ADBHelper.ClearCaches(device.Id, "com.instagram.android");
                            sw.Stop();
                            break;
                        }
                        if (sw.ElapsedMilliseconds > 60000)
                        {
                            device.Status = "reboot";
                            sw.Stop(); break;
                        }
                    }
                    if (device.Status == "Connect")
                    {
                        lock (_lockObject)
                        {
                            selectAccount = profileModels.FirstOrDefault(x => x.IsUsing == false);
                        }
                        if (selectAccount == null)
                        {
                            LDController.Close("index", device.IndexLDPlayer);
                            tableLayoutPanel.Invoke((MethodInvoker)delegate
                            {
                                tableLayoutPanel.Controls.Remove(device.view.Panel);
                            });
                            RepositionLDPlayers();
                            return;
                        }
                        selectAccount.IsUsing = true;
                        if (!rdoNoProxy.Checked && !string.IsNullOrEmpty(selectAccount.Proxy))
                        {
                            writeLog(device, "Connect Proxy", null, device.view.StatusLabel);
                            string[] proxy = selectAccount.Proxy.Split(':');
                            string ip = string.Empty, port = string.Empty, username = string.Empty, password = string.Empty;
                            if (proxy.Length > 0)
                            {
                                ip = proxy[0];
                                port = proxy[1];
                                if (proxy.Length > 3)
                                {
                                    username = proxy[2];
                                    password = proxy[3];
                                }
                                if (!string.IsNullOrEmpty(ip) && !string.IsNullOrEmpty(port))
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (CheckProxy(ip, port, username, password))
                                        {
                                            writeLog(device, $"{selectAccount.Proxy}", null, device.view.StatusLabel);
                                            await ChangeProxy(device.IndexLDPlayer, device.Id, ip, port, username, password);
                                            ADBHelper.Delay(10);
                                            device.Status = "";
                                            break;
                                        }
                                        else
                                        {
                                            writeLog(device, $"Proxy Fail: {selectAccount.Proxy}", null, device.view.StatusLabel, 1);
                                            selectAccount.Proxy = _listProxy[random.Next(0, _listProxy.Count)];
                                            device.Status = "reboot";
                                        }
                                    }

                                }
                            }
                        }
                        if (device.Status != "reboot")
                        {
                            //////////////////////////////////////////Reg Instagram///////////////////////////////////////
                            writeLog(device, "Instagram", null, device.view.StatusLabel);
                            var checkLayout = _instagramController.CheckLayoutInstagram(device.Data, device.AdbClient);
                            if (checkLayout == 1)
                            {
                                writeLog(device, "Import Fullname", null, device.view.StatusLabel);
                                if (_instagramController.ImportFullnameInstagram(device.Data, device.AdbClient, selectAccount.Firstname, selectAccount.Lastname))
                                {
                                    writeLog(device, $"{selectAccount.Firstname} {selectAccount.Lastname}", null, device.view.StatusLabel);
                                    writeLog(device, $"Import Password", null, device.view.StatusLabel);
                                    if (_instagramController.ImportPasswrodInstagram(device.Data, device.AdbClient, selectAccount.Password))
                                    {
                                        writeLog(device, $"{selectAccount.Password}", null, device.view.StatusLabel);
                                        if (_instagramController.SelecBirthDayInstagram(device.Data, device.AdbClient))
                                        {
                                            writeLog(device, $"Import Username", null, device.view.StatusLabel);
                                            if (_instagramController.ImportUsernameInstagram(device.Data, device.AdbClient, selectAccount.Username))
                                            {
                                                writeLog(device, $"{selectAccount.Username}", null, device.view.StatusLabel);
                                                string idPhone = string.Empty, phonenumber = string.Empty, code = string.Empty;
                                                if (rbRegWithEmail.Checked)
                                                {
                                                    writeLog(device, $"Import Email", null, device.view.StatusLabel);
                                                    if (ADBClientController.FindElementIsExistOrClickByClass(device.Data, device.AdbClient, "Sign up with email", "android.widget.Button", 30, true))
                                                    {
                                                        if (_instagramController.ImportEmailOrPhoneInstagram(device.Data, device.AdbClient, selectAccount.Email))
                                                        {
                                                            DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, null, "cIdAccount", "cStatus", "Confirmation Code");
                                                            writeLog(device, $"Confirmation Code", null, device.view.StatusLabel);
                                                            string email = selectAccount.Email, codeEmail = string.Empty;
                                                            if (ckbCatchAll)
                                                            {
                                                                email = selectAccount.Domain;
                                                            }
                                                            for (int i = 0; i < 7; i++)
                                                            {
                                                                try
                                                                {
                                                                    writeLog(device, $"Getcode Email {selectAccount.Email}", null, device.view.StatusLabel);
                                                                    codeEmail = MailKitsHelper.GetCodeEmail(email, selectAccount.PasswordEmail, selectAccount.Server, int.Parse(selectAccount.Port), "Instagram", selectAccount.Email);
                                                                    if (!string.IsNullOrEmpty(codeEmail) && codeEmail.ToString().Length == 6)
                                                                    {
                                                                        writeLog(device, $"code {codeEmail}", null, device.view.StatusLabel);
                                                                        break;
                                                                    }
                                                                    writeLog(device, $"No code {selectAccount.Email}", null, device.view.StatusLabel, 1);
                                                                    ADBHelper.Delay();
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Log.Error(ex, ex.Message);
                                                                    writeLog(device, ex.Message, null, device.view.StatusLabel, 1);
                                                                    break;
                                                                }
                                                            }
                                                            if (codeEmail != null && codeEmail.ToString().Length == 6)
                                                            {
                                                                writeLog(device, $"import {codeEmail}", null, device.view.StatusLabel);
                                                                if (!_instagramController.ImportCodeInstagram(device.Data, device.AdbClient, codeEmail))
                                                                {
                                                                    DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, null, "cIdAccount", "cStatus", "Fail", 1);
                                                                    Log.Information(device.AdbClient.DumpScreen(device.Data).OuterXml);
                                                                    writeLog(device, $"Fail", null, device.view.StatusLabel, 1);
                                                                }
                                                                else
                                                                {
                                                                    device.Status = "Done";
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                                else if (rbRegWithPhone.Checked)
                                                {
                                                    var service = GlobalModels.Service;
                                                    int GetPhone = 10;
                                                    while (GetPhone > 0)
                                                    {
                                                        switch (service)
                                                        {
                                                            case "5sim.net":
                                                                {
                                                                    var httpResult = await FiveSimHttpHelper.BuyPhoneNumber(GlobalModels.Contry, txtApikey.Text.Trim());
                                                                    if (httpResult != null)
                                                                    {
                                                                        if (httpResult.StatusCode == 200)
                                                                        {
                                                                            var getBuyPhone = (FiveResult)httpResult.Data;
                                                                            phonenumber = getBuyPhone.phone;
                                                                            idPhone = getBuyPhone.id.ToString();
                                                                            GetPhone = 0;
                                                                            break;
                                                                        }
                                                                        else
                                                                        {
                                                                            var message = httpResult.Data;
                                                                            if (message != null)
                                                                            {
                                                                                writeLog(device, message.ToString(), null, device.view.StatusLabel, 1);
                                                                            }
                                                                            else
                                                                            {
                                                                                writeLog(device, "No Phone", null, device.view.StatusLabel, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        writeLog(device, "HttpClient Timeout", null, device.view.StatusLabel, 1);
                                                                    }

                                                                    GetPhone--;
                                                                    continue;
                                                                }
                                                            case "Viotp.com":
                                                                {
                                                                    string appId = "36&network=VIETTEL";
                                                                    if (rbLaos.Checked)
                                                                    {
                                                                        appId = "36&country=la";
                                                                    }
                                                                    else if (rbMobi.Checked)
                                                                    {
                                                                        appId = "36&network=MOBIFONE";
                                                                    }
                                                                    else if (rbITelecom.Checked)
                                                                    {
                                                                        appId = "36&network=ITELECOM";
                                                                    }
                                                                    else if (rbVina.Checked)
                                                                    {
                                                                        appId = "36&network=VINAPHONE";
                                                                    }
                                                                    else if (rbVNMB.Checked)
                                                                    {
                                                                        appId = "36&network=VIETNAMOBILE";
                                                                    }
                                                                    var getBuyPhone = await ViotpHttpHelper.BuyPhoneNumber(txtApikey.Text.Trim(), appId);
                                                                    if (getBuyPhone != null)
                                                                    {
                                                                        if (getBuyPhone.success == true)
                                                                        {
                                                                            idPhone = getBuyPhone.data.request_id.ToString();
                                                                            phonenumber = "84" + getBuyPhone.data.phone_number;
                                                                            if (rbLaos.Checked)
                                                                            {
                                                                                phonenumber = "856" + getBuyPhone.data.phone_number;
                                                                            }
                                                                            GetPhone = 0;
                                                                            break;
                                                                        }
                                                                        else
                                                                        {
                                                                            writeLog(device, getBuyPhone.message, null, device.view.StatusLabel, 1);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        writeLog(device, "HttpClient Timeout", null, device.view.StatusLabel, 1);
                                                                    }
                                                                    GetPhone--;
                                                                    continue;
                                                                }
                                                            case "365sms.org":
                                                                {
                                                                    var getBuyPhone = await Sms365HttpHelper.BuyPhoneNumber(txtApikey.Text.Trim(), GlobalModels.Contry);
                                                                    if (getBuyPhone != null)
                                                                    {
                                                                        string[] result = getBuyPhone.Split(':');
                                                                        if (result[0] == "ACCESS_NUMBER")
                                                                        {
                                                                            phonenumber = result[2];
                                                                            idPhone = result[1];
                                                                            GetPhone = 0;
                                                                            break;
                                                                        }
                                                                        else
                                                                        {
                                                                            writeLog(device, getBuyPhone, null, device.view.StatusLabel, 1);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        writeLog(device, "HttpClient Timeout", null, device.view.StatusLabel, 1);
                                                                    }
                                                                    GetPhone--;
                                                                    continue;
                                                                }
                                                            case "Getsmscode.io":
                                                                {
                                                                    var getBuyPhone = await GetSmsCodeHttpHelper.BuyPhoneNumber(txtApikey.Text.Trim(), GlobalModels.Contry);
                                                                    if (getBuyPhone == null)
                                                                    {
                                                                        writeLog(device, "Error", null, device.view.StatusLabel, 1);
                                                                    }
                                                                    else if (getBuyPhone.Status)
                                                                    {
                                                                        phonenumber = getBuyPhone.data.Phone_number;
                                                                        idPhone = getBuyPhone.data.ActivationId;
                                                                        GetPhone = 0;
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        writeLog(device, getBuyPhone.errors.error_mess, null, device.view.StatusLabel, 1);
                                                                    }
                                                                    GetPhone--;
                                                                    continue;
                                                                }
                                                            case "Yuenanka.com":
                                                                {
                                                                    string appId = "1010&carrier=Viettel";
                                                                    if (rbMobi.Checked)
                                                                    {
                                                                        appId = "1010&carrier=Mobi";
                                                                    }
                                                                    else if (rbITelecom.Checked)
                                                                    {
                                                                        appId = "1010&carrier=ITelecom";
                                                                    }
                                                                    else if (rbVina.Checked)
                                                                    {
                                                                        appId = "1010&carrier=Vina";
                                                                    }
                                                                    else if (rbVNMB.Checked)
                                                                    {
                                                                        appId = "1010&carrier=VNMB";
                                                                    }

                                                                    var getBuyPhone = await YuenankaHttpHelper.BuyPhoneNumber(txtApikey.Text.Trim(), appId);
                                                                    if (getBuyPhone != null && getBuyPhone.ResponseCode == 0)
                                                                    {
                                                                        phonenumber = "84" + getBuyPhone.Result.Number;
                                                                        idPhone = getBuyPhone.Result.Id;
                                                                        GetPhone = 0;
                                                                        break;
                                                                    }
                                                                    else if (getBuyPhone == null)
                                                                    {

                                                                        writeLog(device, $"Error", null, device.view.StatusLabel, 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        writeLog(device, getBuyPhone.Msg, null, device.view.StatusLabel, 1);
                                                                    }
                                                                    GetPhone--;
                                                                    continue;
                                                                }
                                                            default:
                                                                {
                                                                    GetPhone--;
                                                                    continue;
                                                                }
                                                        }
                                                    }
                                                    ADBClientController.ClickElement(device.Data, device.AdbClient, "class='android.widget.EditText'", 30);
                                                    ADBHelper.Delay(1, 3);
                                                    ADBHelper.InputText(device.Id, phonenumber);
                                                    ADBHelper.Delay(1, 3);
                                                    if (ADBClientController.FindElementIsExistOrClickByClass(device.Data, device.AdbClient, "Next", "android.view.View", 30, true))
                                                    {
                                                        if (!string.IsNullOrEmpty(idPhone) && !string.IsNullOrEmpty(phonenumber))
                                                        {
                                                            writeLog(device, $"{phonenumber}", null, device.view.StatusLabel);
                                                            GetPhone = 50;
                                                            while (GetPhone > 0)
                                                            {
                                                                writeLog(device, $" Get Code {service}", null, device.view.StatusLabel);
                                                                switch (service)
                                                                {
                                                                    case "5sim.net":
                                                                        {
                                                                            ADBHelper.Delay(10, 20);
                                                                            var httpResult = await FiveSimHttpHelper.GetOtp(txtApikey.Text.Trim(), idPhone);
                                                                            if (httpResult.StatusCode == 200)
                                                                            {
                                                                                var getOtp = (FiveResult)httpResult.Data;
                                                                                code = getOtp.sms[0].code;
                                                                                GetPhone = 0;
                                                                                break;
                                                                            }
                                                                            else
                                                                            {
                                                                                var message = httpResult.Data;
                                                                                if (message != null)
                                                                                {
                                                                                    writeLog(device, message.ToString(), null, device.view.StatusLabel, 1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    writeLog(device, "No Code", null, device.view.StatusLabel, 1);
                                                                                }
                                                                                ADBHelper.Delay(5, 10);
                                                                            }
                                                                            GetPhone--;
                                                                            continue;
                                                                        }
                                                                    case "Viotp.com":
                                                                        {
                                                                            var getOtp = await ViotpHttpHelper.GetOtp(txtApikey.Text.Trim(), idPhone);
                                                                            if (getOtp != null)
                                                                            {
                                                                                if (getOtp.success == true)
                                                                                {
                                                                                    if (getOtp.data?.Status == 1)
                                                                                    {
                                                                                        code = getOtp.data.Code;
                                                                                        GetPhone = 0;
                                                                                        break;
                                                                                    }
                                                                                    writeLog(device, getOtp.message, null, device.view.StatusLabel, 1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    writeLog(device, getOtp.message, null, device.view.StatusLabel, 1);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                writeLog(device, "HttpClient Timeout", null, device.view.StatusLabel, 1);
                                                                            }
                                                                            ADBHelper.Delay(10, 20);
                                                                            GetPhone--;
                                                                            continue;
                                                                        }
                                                                    case "365sms.org":
                                                                        {
                                                                            ADBHelper.Delay(10, 20);
                                                                            var getOtp = await Sms365HttpHelper.GetOtp(txtApikey.Text.Trim(), idPhone);
                                                                            if (getOtp != null)
                                                                            {
                                                                                string[] result = getOtp.Split(':');
                                                                                if (result[0] == "STATUS_OK")
                                                                                {
                                                                                    code = result[1];
                                                                                    GetPhone = 0;
                                                                                    break;
                                                                                }
                                                                                else
                                                                                {
                                                                                    writeLog(device, getOtp, null, device.view.StatusLabel, 1);
                                                                                    ADBHelper.Delay(5, 10);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                writeLog(device, "HttpClient Timeout", null, device.view.StatusLabel, 1);
                                                                            }
                                                                            GetPhone--;
                                                                            continue;
                                                                        }
                                                                    case "Getsmscode.io":
                                                                        {
                                                                            ADBHelper.Delay(20, 40);
                                                                            var getOtp = await GetSmsCodeHttpHelper.GetOtp(txtApikey.Text.Trim(), idPhone);
                                                                            if (getOtp != null && getOtp.Status)
                                                                            {
                                                                                code = getOtp.sms_code;
                                                                                GetPhone = 0;
                                                                                break;
                                                                            }
                                                                            else if (getOtp == null)
                                                                            {
                                                                                writeLog(device, "HttpClient Timeout", null, device.view.StatusLabel, 1);
                                                                            }
                                                                            else
                                                                            {
                                                                                writeLog(device, getOtp.error, null, device.view.StatusLabel, 1);
                                                                                ADBHelper.Delay(5, 10);
                                                                            }
                                                                            GetPhone--;
                                                                            continue;
                                                                        }
                                                                    case "Yuenanka.com":
                                                                        {
                                                                            var getOtp = await YuenankaHttpHelper.GetOtp(txtApikey.Text.Trim(), idPhone);
                                                                            if (getOtp != null && getOtp.ResponseCode == 0)
                                                                            {
                                                                                code = getOtp.Result.Code;
                                                                                writeLog(device, getOtp.Result.SMS.ToString(), null, device.view.StatusLabel, 2);
                                                                                GetPhone = 0;
                                                                                break;
                                                                            }
                                                                            else if (getOtp == null)
                                                                            {
                                                                                writeLog(device, "HttpClient Timeout", null, device.view.StatusLabel, 1);
                                                                            }
                                                                            else
                                                                            {
                                                                                writeLog(device, getOtp.Msg, null, device.view.StatusLabel, 1);
                                                                                ADBHelper.Delay(10, 20);
                                                                            }
                                                                            GetPhone--;
                                                                            continue;
                                                                        }
                                                                    default:
                                                                        {
                                                                            continue;
                                                                        }
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(code))
                                                            {
                                                                writeLog(device, $"import {code}", null, device.view.StatusLabel);
                                                                if (!_instagramController.ImportCodeInstagram(device.Data, device.AdbClient, code))
                                                                {
                                                                    writeLog(device, $"{phonenumber} CheckPoint 282", null, device.view.StatusLabel, 1);
                                                                    Log.Information(device.AdbClient.DumpScreen(device.Data).OuterXml);
                                                                }
                                                                else
                                                                {
                                                                    device.Status = "Done";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (rbRegWithTempMail.Checked)
                                                {
                                                    writeLog(device, $"Import Email", null, device.view.StatusLabel);
                                                    if (ADBClientController.FindElementIsExistOrClickByClass(device.Data, device.AdbClient, "Sign up with email", "android.widget.Button", 30, true))
                                                    {
                                                        string userEmail = selectAccount.Username + Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", random.Next(8, 20));
                                                        MailClient client = new MailClient();
                                                        var domain = await client.GetAvailableDomains();
                                                        selectAccount.Email = $"{userEmail}@{domain.FirstOrDefault().Domain}";
                                                        selectAccount.PasswordEmail = Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", random.Next(8, 20));
                                                        selectAccount.Domain = "TemMail";
                                                        try
                                                        {
                                                            await client.Register(selectAccount.Email, selectAccount.PasswordEmail);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            writeLog(device, $"{ex.Message}", null, device.view.StatusLabel, 1);
                                                        }
                                                        if (_instagramController.ImportEmailOrPhoneInstagram(device.Data, device.AdbClient, selectAccount.Email))
                                                        {
                                                            DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, null, "cIdAccount", "cStatus", "Confirmation Code");
                                                            writeLog(device, $"Confirmation Code", null, device.view.StatusLabel);
                                                            string email = selectAccount.Email, codeEmail = string.Empty;
                                                            if (ckbCatchAll)
                                                            {
                                                                email = selectAccount.Domain;
                                                            }
                                                            for (int i = 0; i < 7; i++)
                                                            {
                                                                try
                                                                {
                                                                    writeLog(device, $"Getcode Email {selectAccount.Email}", null, device.view.StatusLabel);
                                                                    await client.Login(selectAccount.Email, selectAccount.PasswordEmail);
                                                                    MessageInfo[] messages = await client.GetAllMessages();
                                                                    foreach (var message in messages)
                                                                    {
                                                                        if (message.Subject.Contains("Instagram"))
                                                                        {
                                                                            codeEmail = message.Subject.Substring(0, 6);
                                                                            writeLog(device, $"code {codeEmail}", null, device.view.StatusLabel);
                                                                            i = 9;
                                                                            break;
                                                                        }
                                                                    }
                                                                    writeLog(device, $"No code {selectAccount.Email}", null, device.view.StatusLabel, 1);
                                                                    ADBHelper.Delay();
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Log.Error(ex, ex.Message);
                                                                    writeLog(device, ex.Message, null, device.view.StatusLabel, 1);
                                                                    break;
                                                                }
                                                            }
                                                            if (codeEmail != null && codeEmail.ToString().Length == 6)
                                                            {
                                                                writeLog(device, $"import {codeEmail}", null, device.view.StatusLabel);
                                                                if (!_instagramController.ImportCodeInstagram(device.Data, device.AdbClient, codeEmail))
                                                                {
                                                                    DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, null, "cIdAccount", "cStatus", "Fail", 1);
                                                                    Log.Information(device.AdbClient.DumpScreen(device.Data).OuterXml);
                                                                    writeLog(device, $"Fail", null, device.view.StatusLabel, 1);
                                                                }
                                                                else
                                                                {
                                                                    device.Status = "Done";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (device.Status == "Done")
                                                {
                                                    string username = string.Empty;
                                                    Account account = new Account();
                                                    account.Id = new Guid();
                                                    if (rbRegWithPhone.Checked)
                                                    {
                                                        account.Phonenumber = phonenumber;
                                                        username = phonenumber;
                                                    }
                                                    else if (rbRegWithEmail.Checked)
                                                    {
                                                        account.Email = selectAccount.Email;
                                                        account.PasswordEmail = selectAccount.PasswordEmail;
                                                        account.Server = selectAccount.Server;
                                                        account.Port = selectAccount.Port;
                                                        username = selectAccount.Email;
                                                    }
                                                    else if (rbRegWithTempMail.Checked)
                                                    {
                                                        account.Email = selectAccount.Email;
                                                        account.PasswordEmail = selectAccount.PasswordEmail;
                                                        account.Server = selectAccount.Domain;
                                                        username = selectAccount.Email;
                                                    }
                                                    account.FullName = $"{selectAccount.Firstname} {selectAccount.Lastname}";
                                                    account.Status = "Done";
                                                    account.CreateDate = DateTime.Now;
                                                    account.Password = selectAccount.Password;
                                                    account.Proxy = selectAccount.Proxy;
                                                    account.Username = selectAccount.Username;
                                                    try
                                                    {
                                                        _accountRepository.Add(account);
                                                        loadAccount();
                                                        writeLog(device, $"Done", account.Id.ToString(), device.view.StatusLabel, 2);

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        writeLog(device, $"Fail add Account error: {ex.Message}", null, device.view.StatusLabel, 1);
                                                        Log.Error(ex, ex.Message);
                                                    }
                                                    string userBackup = selectAccount.Email;
                                                    if (rbRegWithPhone.Checked)
                                                    {
                                                        userBackup = phonenumber;
                                                    }
                                                    //   await BackupHelper.BackupAccountAsync(device.Id, "com.instagram.android", userBackup);
                                                    //   await BackupHelper.BackupDeviceAsync(device.IndexLDPlayer, userBackup);
                                                    // account.Backup = Path.Combine(Environment.CurrentDirectory, $"BackupData\\{userBackup}");
                                                    // _accountRepository.UpdateBackupByIdAccount(account.Id, Path.Combine(Environment.CurrentDirectory, $"BackupData\\{userBackup}"));
                                                    // DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, account.Id.ToString(), "cIdAccount", "cBackup", Path.Combine(Environment.CurrentDirectory, $"BackupData\\{userBackup}"));
                                                    //writeLog(device, Path.Combine(Environment.CurrentDirectory, $"BackupData\\{userBackup}"), account.Id.ToString(), device.view.StatusLabel, 2);
                                                    if (cb2FA.Checked)
                                                    {
                                                        writeLog(device, "tun on 2FA", account.Id.ToString(), device.view.StatusLabel, 2);
                                                        var towFA = await _instagramController.GetPasswordTowFAInstagramAsync(device.IndexLDPlayer, device.Data, device.AdbClient, account.Proxy);
                                                        if (!string.IsNullOrEmpty(towFA))
                                                        {
                                                            _accountRepository.UpdateTowFAByIdAccount(account.Id, towFA);
                                                            DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, account.Id.ToString(), "cIdAccount", "c2FA", towFA, 2);
                                                            writeLog(device, towFA, account.Id.ToString(), device.view.StatusLabel, 2);
                                                        }
                                                    }
                                                    if (rbRegWithPhone.Checked && ckbAddEmail.Checked)
                                                    {
                                                        writeLog(device, "Change Email", account.Id.ToString(), device.view.StatusLabel, 2);
                                                        string emailRecovery = string.Empty, domain = string.Empty;
                                                        if (ckbCatchAll)
                                                        {
                                                            emailRecovery = account.Username + selectAccount.Domain;
                                                            domain = selectAccount.Domain;
                                                        }
                                                        else
                                                        {
                                                            emailRecovery = selectAccount.Email;
                                                            domain = selectAccount.Email;
                                                        }
                                                        if (!string.IsNullOrEmpty(emailRecovery))
                                                        {
                                                            if (MailKitsHelper.CheckLogin(domain, selectAccount.PasswordEmail, selectAccount.Server, int.Parse(selectAccount.Port)))
                                                            {
                                                                if (_instagramController.ChangeEmail(device.IndexLDPlayer, device.Data, device.AdbClient, emailRecovery))
                                                                {
                                                                    string codeEmailRecovery = string.Empty;
                                                                    for (int i = 0; i < 10; i++)
                                                                    {
                                                                        codeEmailRecovery = MailKitsHelper.GetCodeEmail(domain, selectAccount.PasswordEmail, selectAccount.Server, int.Parse(selectAccount.Port), "Instagram", emailRecovery);
                                                                        if (!string.IsNullOrEmpty(codeEmailRecovery))
                                                                        {
                                                                            break;
                                                                        }
                                                                        ADBHelper.Delay(5);
                                                                    }
                                                                    if (!string.IsNullOrEmpty(codeEmailRecovery))
                                                                    {
                                                                        if (_instagramController.ImportCodeChangeEmail(device.IndexLDPlayer, device.Data, device.AdbClient, codeEmailRecovery))
                                                                        {
                                                                            account.Email = emailRecovery;
                                                                            account.PasswordEmail = selectAccount.PasswordEmail;
                                                                            account.Server = selectAccount.Server;
                                                                            account.Port = selectAccount.Port;
                                                                            _accountRepository.UpdateEmail(account);
                                                                            loadAccount();
                                                                            writeLog(device, $"Done Change Email: {emailRecovery}", account.Id.ToString(), device.view.StatusLabel, 2);
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                            else
                                                            {
                                                                writeLog(device, $"Email Fail: {emailRecovery}", null, device.view.StatusLabel, 1);
                                                            }
                                                        }
                                                    }
                                                    if (cbUpdateAvatar.Checked && File.Exists(selectAccount.FileImage))
                                                    {
                                                        writeLog(device, "Upload Avatar", null, device.view.StatusLabel, 2);
                                                        var updateAvatar = _instagramController.UploadAvatarInstagram(device.IndexLDPlayer, device.Data, device.AdbClient, selectAccount.FileImage);
                                                        if (updateAvatar)
                                                        {
                                                            _accountRepository.UpdateAvatarByIdAccount(account.Id, selectAccount.FileImage);
                                                            DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, account.Id.ToString(), "cIdAccount", "cAvatar", selectAccount.FileImage, 2);
                                                            writeLog(device, selectAccount.FileImage, account.Id.ToString(), device.view.StatusLabel, 2);
                                                        }
                                                    }
                                                    if (ckbUploadPost.Checked && File.Exists(selectAccount.FileImagePost))
                                                    {
                                                        writeLog(device, $"upload Post", account.Id.ToString(), device.view.StatusLabel, 2);
                                                        if (_instagramController.CreatPostInstagram(device.IndexLDPlayer, device.Data, device.AdbClient, selectAccount.FileImagePost))
                                                        {
                                                            writeLog(device, $"Done upload Post {selectAccount.FileImagePost}", account.Id.ToString(), device.view.StatusLabel, 2);
                                                        }
                                                    }
                                                    if (ckbFollowSuggested.Checked)
                                                    {
                                                        writeLog(device, $"Follow Suggested", account.Id.ToString(), device.view.StatusLabel, 2);
                                                        int countFollow = 1;
                                                        countFollow = random.Next(_numberFollowForm, _numberFollowTo);
                                                        if (_instagramController.FollowSuggestedInstagram(device.IndexLDPlayer, device.Data, device.AdbClient, countFollow))
                                                        {
                                                            writeLog(device, $"Follow Suggested {countFollow}", account.Id.ToString(), device.view.StatusLabel, 2);
                                                        }
                                                    }
                                                    string message = $"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss\t")}    {username}|{account.Password}|{account.TowFA}|{account.Proxy}|{account.Username}|{account.FullName}";
                                                    rtbResult.Invoke((MethodInvoker)delegate
                                                    {
                                                        rtbResult.InvokeEx(s => s.AppendText(message, Color.Black, rtbResult.Font, true));
                                                    });
                                                }
                                                else if (rbRegWithPhone.Checked)
                                                {

                                                    selectAccount.Firstname = _listFirtname[random.Next(_listFirtname.Count)];
                                                    selectAccount.Lastname = _listLastname[random.Next(_listLastname.Count)];
                                                    selectAccount.Proxy = _listProxy[random.Next(0, _listProxy.Count)];
                                                    selectAccount.Username = _listUsername[random.Next(_listUsername.Count)] + Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", random.Next(5, 10));
                                                    selectAccount.IsUsing = false;
                                                }
                                            }
                                            else
                                            {
                                                selectAccount.Firstname = _listFirtname[random.Next(_listFirtname.Count)];
                                                selectAccount.Lastname = _listLastname[random.Next(_listLastname.Count)];
                                                selectAccount.Proxy = _listProxy[random.Next(0, _listProxy.Count)];
                                                selectAccount.Username = _listUsername[random.Next(_listUsername.Count)] + Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", random.Next(5, 10));
                                                selectAccount.IsUsing = false;
                                            }

                                        }
                                    }
                                }
                            }
                            else if (checkLayout == 2)
                            {
                                selectAccount.Firstname = _listFirtname[random.Next(_listFirtname.Count)];
                                selectAccount.Lastname = _listLastname[random.Next(_listLastname.Count)];
                                selectAccount.Proxy = _listProxy[random.Next(0, _listProxy.Count)];
                                selectAccount.Username = _listUsername[random.Next(_listUsername.Count)] + Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", random.Next(5, 10));
                                selectAccount.IsUsing = false;
                            }
                            else
                            {
                                selectAccount.Firstname = _listFirtname[random.Next(_listFirtname.Count)];
                                selectAccount.Lastname = _listLastname[random.Next(_listLastname.Count)];
                                selectAccount.Proxy = _listProxy[random.Next(0, _listProxy.Count)];
                                selectAccount.Username = _listUsername[random.Next(_listUsername.Count)] + Helpers.GenerateRandomString("abcdefghijklmnopqrstuvwxyz0123456789", random.Next(5, 10));
                                selectAccount.IsUsing = false;
                            }
                        }

                    }
                    ///////////////////////////Reboot/////////////////////////////////
                    writeLog(device, $"Reboot", null, device.view.StatusLabel);
                    LDController.Close("index", device.IndexLDPlayer);
                    ADBHelper.Delay(7);
                    lock (_lockObject)
                    {
                        try
                        {
                            File.Delete($"{LDController.PathFolderLDPlayer}\\vms\\leidian{device.IndexLDPlayer}\\data.vmdk");
                            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(Environment.CurrentDirectory, "LDPlayer\\data.zip"), $"{LDController.PathFolderLDPlayer}\\vms\\leidian{device.IndexLDPlayer}");
                            Thread.Sleep(7000);
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(3000);
                            File.Delete($"{LDController.PathFolderLDPlayer}\\vms\\leidian{device.IndexLDPlayer}\\data.vmdk");
                            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(Environment.CurrentDirectory, "LDPlayer\\data.zip"), $"{LDController.PathFolderLDPlayer}\\vms\\leidian{device.IndexLDPlayer}");
                        }
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            if (this.InvokeRequired) // Kiểm tra xem hiện tại có đang ở UI thread hay không
                            {
                                this.Invoke((Action)(() =>
                                {
                                    LDController.Close("index", device.IndexLDPlayer);
                                    tableLayoutPanel.Controls.Remove(device.view.Panel);
                                    RepositionLDPlayers();

                                }));
                                return;
                            }
                            else
                            {
                                LDController.Close("index", device.IndexLDPlayer);
                                tableLayoutPanel.Controls.Remove(device.view.Panel);
                                RepositionLDPlayers();
                                return;
                            }
                        }
                    }
                    rebootLDPlayer(device, true); // reboot lại 
                }
            }
            catch (Exception ex)
            {
                writeLog(device, ex.Message, null, device.view.StatusLabel, 1);
                Log.Error(ex, ex.Message);
                LDController.Close("index", device.IndexLDPlayer);
                _tasks.Add(Task.Run(async () =>
                {
                    await StartDevice(device);
                }, cancellationTokenSource.Token));
            }
        }
        private bool CheckProxy(string host, string port, string? username, string? password)
        {
            try
            {
                string proxyAddress = host + ":" + port;

                // Create a WebRequest using proxy
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api64.ipify.org/");
                request.Proxy = new WebProxy(proxyAddress);

                // Set proxy credentials if provided
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    request.Proxy.Credentials = new NetworkCredential(username, password);
                }

                // Send a GET request to check the proxy
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Check the HTTP status code to determine proxy validity
                    HttpStatusCode statusCode = response.StatusCode;

                    // Return true if status code indicates success
                    return statusCode >= HttpStatusCode.OK && statusCode < HttpStatusCode.Ambiguous;
                }
            }
            catch (WebException)
            {
                // An error occurred when using the proxy
                return false;
            }
        }

        private void rtbLogs_TextChanged(object sender, EventArgs e)
        {
            rtbLogs.SelectionStart = rtbLogs.Text.Length;
            rtbLogs.ScrollToCaret();
        }

        private void rtbResult_TextChanged(object sender, EventArgs e)
        {
            rtbResult.SelectionStart = rtbResult.Text.Length;
            rtbResult.ScrollToCaret();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            try
            {
                txtLDPath.Text = LDController.PathFolderLDPlayer;
                loadAccount();
                loadFileJsonService();
                GetConfigDatagridview();
                if (cbUpdateAvatar.Checked)
                {
                    txtFolderAvatar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                MessageCommon.ShowMessageBox(ex.Message, 4);
            }


        }
        private async void loadDevice()
        {
            try
            {
                var listLDPlayer = DeviceHelper.GetLDplayers();
                dtgvLDPlayers.Invoke((MethodInvoker)delegate
                {
                    if (listLDPlayer.Count > 0)
                    {
                        // Clear existing rows
                        dtgvLDPlayers.Rows.Clear();
                        foreach (var device in listLDPlayer)
                        {
                            if (int.Parse(device.Index) < 999)
                            {
                                // Add rows with indexes 1, 5, and 8
                                dtgvLDPlayers.Rows.Add(device.Index, device.Name, device.DeviceId, device.Status);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }
        }
        private async void dtgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btnLastName_Click(object sender, EventArgs e)
        {
            fAddFile fAddFile = new fAddFile("Last Name", _listLastname);
            fAddFile.ShowDialog();
            _listLastname.Clear();
            try
            {
                using (StreamWriter writer = new StreamWriter(GlobalModels.PathLastName))
                {
                    foreach (var line in fAddFile._list)
                    {
                        _listLastname.Add(line);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex, ex.Message);
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 2);
            }
        }

        private void btnAddFirstname_Click(object sender, EventArgs e)
        {
            fAddFile fAddFile = new fAddFile("First Name", _listFirtname);
            fAddFile.ShowDialog();
            _listFirtname.Clear();
            try
            {
                using (StreamWriter writer = new StreamWriter(GlobalModels.PathFirstName))
                {
                    foreach (var line in fAddFile._list)
                    {
                        _listFirtname.Add(line);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex, ex.Message);
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 2);
            }
        }
        private void radioRadomFullNameUs_CheckedChanged(object sender, EventArgs e)
        {
            plTenTuDat.Enabled = false;
        }

        private void radioRadomFullNameVN_CheckedChanged(object sender, EventArgs e)
        {
            plTenTuDat.Enabled = false;
        }

        private void radioCustomizeFullNameUs_CheckedChanged(object sender, EventArgs e)
        {
            plTenTuDat.Enabled = true;
        }

        private void radioRandomPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.Enabled = false;
            NumberPass.Enabled = true;
        }

        private void radioCustomizePass_CheckedChanged(object sender, EventArgs e)
        {
            NumberPass.Enabled = false;
            txtPass.Enabled = true;
        }

        private void btnSettingUserName_Click(object sender, EventArgs e)
        {
            fAddFile fAddFile = new fAddFile("User Name", _listUsername);
            fAddFile.ShowDialog();
            _listUsername.Clear();
            try
            {
                using (StreamWriter writer = new StreamWriter(GlobalModels.PathUserName))
                {
                    foreach (var line in fAddFile._list)
                    {
                        _listUsername.Add(line);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex, ex.Message);
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 2);
            }
        }
        private async void btnStop_Click(object sender, EventArgs e)
        {
            StopAll();
            try
            {
                tableLayoutPanel.Controls.Clear();
                pDevice.Controls.Clear();
            }
            catch (Exception ex)
            {

            }
        }
        private void StopAll()
        {
            cancellationTokenSource?.Cancel();
        }
        private async void btnSetup_Click(object sender, EventArgs e)
        {
            btnSetup.Enabled = false;
            var ldplayer = LDController.GetDevices2().FirstOrDefault();
            DeviceInfo device = new DeviceInfo();
            device.IndexLDPlayer = ldplayer.index.ToString();
            device.AdbClient = new AdbClient();
            device.Data = new DeviceData();
            device.view = new ViewInfo();
            device.view.Embeddedpanel = new Panel();
            device.view.StatusLabel = new Label();
            device.view.LdplayerHandle = new IntPtr();
            device.view.Panel = new Panel();
            device.view.PanelButton = new Panel();
            device.view.BtnClose = new Button();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanel.AutoScroll = true;
            LDController.EditFileConfigLDPlayer();
            LDController.SettingLDPlayerByIndex(device.IndexLDPlayer);
            ADBHelper.Delay(1);
            bool isRunning = false;
            for (int i = 0; i < 10; i++)
            {
                LDController.Open("index", device.IndexLDPlayer);
                ADBHelper.Delay(2);
                if (LDController.IsDevice_Running("index", device.IndexLDPlayer))
                {
                    if (columnCount >= int.Parse(btnDisplay.Text.Trim()))
                    {
                        rowCount++;
                        columnCount = 0;
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    }
                    addViewControl(device, tableLayoutPanel);
                    writeLog(device, "Running", null, device.view.StatusLabel);
                    columnCount++;
                    ADBHelper.Delay(7);
                    pDevice.Controls.Add(tableLayoutPanel);
                    isRunning = true;
                    break;
                }
            }
            if (isRunning)
            {
                cancellationTokenSource = new CancellationTokenSource();
                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Run(async () =>
                {
                    await SetupLDPlayer(device);
                }, cancellationTokenSource.Token));
                await Task.WhenAll(tasks);
                btnSetup.Enabled = true;
                tableLayoutPanel.Controls.Clear();
                pDevice.Controls.Clear();
            }
        }
        private async Task SetupLDPlayer(DeviceInfo device)
        {
            string adbKeyboard = Path.Combine(Environment.CurrentDirectory, "App\\ADBKeyboard.apk");
            string vpnProxy = Path.Combine(Environment.CurrentDirectory, "App\\VAT-VpnProxy_v1.0.0.apk");
            string youtube = Path.Combine(Environment.CurrentDirectory, "App\\Instagram.apk");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                ADBHelper.Delay();
                if (DeviceHelper.Connect(device))
                {
                    writeLog(device, "Done Connect", null, device.view.StatusLabel);
                    DataGridViewHelper.SetCellValueByColumnValue(dtgvLDPlayers, device.IndexLDPlayer, "tIndex", "tDeviceId", device.Id, 2);
                    DataGridViewHelper.SetCellValueByColumnValue(dtgvLDPlayers, device.IndexLDPlayer, "tIndex", "tName", device.Data.Model, 2);
                    device.Status = "Connect";
                    sw.Stop();
                    break;
                }
                if (sw.ElapsedMilliseconds > 60000)
                {
                    writeLog(device, "Connect Fail", null, device.view.StatusLabel, 1);
                    device.Status = "reboot";
                    sw.Stop();
                    break;
                }
            }
            if (device.Status == "Connect")
            {
                sw.Start();
                while (true)
                {
                    LDController.InstallApp_File("index", device.IndexLDPlayer, adbKeyboard);
                    ADBHelper.Delay(1);
                    LDController.InstallApp_File("index", device.IndexLDPlayer, vpnProxy);
                    ADBHelper.Delay(1);
                    LDController.InstallApp_File("index", device.IndexLDPlayer, youtube);
                    ADBHelper.Delay(15);
                    var cmdResutl = LDController.ADB("index", device.IndexLDPlayer, "shell pm list package");
                    if (!string.IsNullOrEmpty(cmdResutl) && cmdResutl.Contains("com.instagram.android") && cmdResutl.Contains("com.android.adbkeyboard") && cmdResutl.Contains("com.vat.vpn"))
                    {
                        writeLog(device, "Done InstallApp", null, device.view.StatusLabel, 2);
                        device.Status = "Done InstallApp";
                        sw.Stop();
                        break;
                    }
                    if (sw.ElapsedMilliseconds > 250000)
                    {
                        writeLog(device, "Fail InstallApp", null, device.view.StatusLabel, 1);
                        device.Status = "reboot";
                        sw.Stop();
                        break;
                    }
                }

            }
            if (device.Status == "Done InstallApp")
            {
                writeLog(device, "Setting", null, device.view.StatusLabel);
                await ChangeProxy(device.IndexLDPlayer, device.Id, "127.0.0.1.256", "8080", "", "");
                if (BackupDataLDplayer($"{LDController.PathFolderLDPlayer}\\vms\\leidian{device.IndexLDPlayer}\\data.vmdk", Path.Combine(Environment.CurrentDirectory, $"LDplayer")))
                {
                    writeLog(device, "Done", null, device.view.StatusLabel, 2);
                    LDController.Close("index", device.IndexLDPlayer);
                    return;
                }
            }
            if (device.Status == "reboot")
            {
                writeLog(device, "Error", null, device.view.StatusLabel, 1);
                LDController.Close("index", device.IndexLDPlayer);
                return;
            }
        }
        private bool BackupDataLDplayer(string fileData, string foler)
        {
            try
            {
                // Tạo thư mục đích nếu nó không tồn tại
                if (!Directory.Exists(foler))
                {
                    Directory.CreateDirectory(foler);
                }

                // Tạo đường dẫn đến tệp Zip
                string zipFilePath = Path.Combine(foler, "data.zip");

                // Sao chép tệp nguồn đến thư mục đích
                string destinationFilePath = Path.Combine(foler, Path.GetFileName(fileData));
                File.Copy(fileData, destinationFilePath, true);

                // Tạo một FileStream để ghi dữ liệu vào tệp Zip
                using (FileStream fs = new FileStream(zipFilePath, FileMode.Create))
                {
                    using (ZipOutputStream zipStream = new ZipOutputStream(fs))
                    {
                        // Tạo một đối tượng ZipEntry cho tệp bạn muốn nén
                        ZipEntry entry = new ZipEntry(Path.GetFileName(destinationFilePath));
                        zipStream.PutNextEntry(entry);

                        // Đọc dữ liệu từ tệp nguồn và sao chép vào tệp Zip
                        byte[] buffer = new byte[4096];
                        using (FileStream sourceStream = new FileStream(destinationFilePath, FileMode.Open, FileAccess.Read))
                        {
                            int bytesRead;
                            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                zipStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }

                // Xoá tệp nguồn ở thư mục chỉ định
                File.Delete(destinationFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(fMain)}, params; {nameof(BackupDataLDplayer)}, Error; {ex.Message}, Exception; {ex}");
                return false;
            }

        }
        private void dtgvLDPlayers_BindingContextChanged(object sender, EventArgs e)
        {
            DataGridView gridView = sender as DataGridView;
            if (null != gridView)
            {
                foreach (DataGridViewRow r in gridView.Rows)
                {
                    gridView.Rows[r.Index].HeaderCell.Value = (r.Index + 1).ToString();
                }
            }
        }

        private void fMain_Resize(object sender, EventArgs e)
        {
            if (this.Width < originalFormWidth)
            {
                // Giới hạn việc thu kéo Form lại không vượt quá Width ban đầu của Form
                this.Width = originalFormWidth;
            }
        }

        private void btnSettingProxy_Click(object sender, EventArgs e)
        {
            fAddFile fAddFile = new fAddFile("Proxy", _listProxy);
            fAddFile.ShowDialog();
            _listProxy.Clear();
            try
            {
                using (StreamWriter writer = new StreamWriter(GlobalModels.PathProxy))
                {
                    foreach (var line in fAddFile._list)
                    {
                        _listProxy.Add(line);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex, ex.Message);
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 2);
            }
        }

        private void cbUpdateAvatar_CheckedChanged(object sender, EventArgs e)
        {
            txtFolderAvatar.Enabled = cbUpdateAvatar.Checked;
        }
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerformAction("All");
        }

        private void selectAllHighlightedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerformAction("SelectHighline");
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerformAction("UnAll");
        }

        private void ExportFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetListSelect().Count == 0)
            {
                MessageCommon.ShowMessageBox("Please select the account to export!", 3);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                WriteDataGridViewToFile(saveFileDialog.FileName, dtgvAccount);
            }
        }
        private void WriteDataGridViewToFile(string filePath, DataGridView dataGridView)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string columnHeader = "";
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (!column.Visible || column.Index == 0 || column.Index == 1 || column.Index == dataGridView.Columns.Count - 1 || column.Index == dataGridView.Columns.Count - 2) continue;
                    if (!string.IsNullOrEmpty(columnHeader)) columnHeader += "|";
                    columnHeader += column.HeaderText;
                }
                writer.WriteLine(columnHeader);
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    string rowData = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (!cell.Visible || cell.ColumnIndex == 0 || cell.ColumnIndex == 1 || cell.ColumnIndex == row.Cells.Count - 1 || cell.ColumnIndex == row.Cells.Count - 2) continue;
                        if (!string.IsNullOrEmpty(rowData)) rowData += "|";
                        rowData += cell.Value != null ? cell.Value.ToString() : "";
                    }
                    writer.WriteLine(rowData);
                }
            }
        }

        private void deleteAccToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = GetListSelect();
                if (list.Count == 0)
                {
                    MessageCommon.ShowMessageBox("Please select the account to delete!", 4);
                    return;
                }
                if (MessageCommon.ShowConfirmationBox(string.Format("Do you want to delete the {0} selected accounts?", list.Count)) == DialogResult.No)
                {
                    return;
                }
                var check = _accountRepository.DeleteRange(list);
                if (check)
                {
                    MessageCommon.ShowMessageBox("Account deleted successfully!");
                    loadAccount();
                }
                else
                {
                    MessageCommon.ShowMessageBox("Delete failed, please try again later!", 4);
                }
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }
        }
        private void PerformAction(string action)
        {
            try
            {
                switch (action)
                {
                    case "ToggleCheck":
                        {
                            for (int k = 0; k < dtgvAccount.SelectedRows.Count; k++)
                            {
                                int index = dtgvAccount.SelectedRows[k].Index;
                                SetCellAccount(index, "ChooseCol", !Convert.ToBoolean(GetCellValue(index, "ChooseCol")));
                            }
                            break;
                        }
                    case "SelectHighline":
                        {
                            DataGridViewSelectedRowCollection selectedRows = dtgvAccount.SelectedRows;
                            for (int j = 0; j < selectedRows.Count; j++)
                            {
                                SetCellAccount(selectedRows[j].Index, "ChooseCol", true);
                            }
                            break;
                        }
                    case "UnAll":
                        {
                            for (int l = 0; l < dtgvAccount.RowCount; l++)
                            {
                                SetCellAccount(l, "ChooseCol", false);
                            }
                            break;
                        }
                    case "All":
                        {
                            for (int i = 0; i < dtgvAccount.RowCount; i++)
                            {
                                SetCellAccount(i, "ChooseCol", true);
                            }
                            break;
                        }

                }
                numberAccount.Value = GetListSelect().Count;

            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                Log.Error(ex, ex.Message);
            }

        }
        private void SetCellAccount(int rowIndex, string columnName, object cellValue, bool allowNull = true)
        {
            if (allowNull || !(cellValue.ToString().Trim() == ""))
            {
                var status = DataGridViewHelper.GetCellValue(dtgvAccount, rowIndex, "cStatus");
                switch (status)
                {
                    case "Done":
                        {
                            DataGridViewHelper.SetCellValue(dtgvAccount, rowIndex, columnName, cellValue, 2);
                            break;
                        }
                    case "new":
                        {
                            DataGridViewHelper.SetCellValue(dtgvAccount, rowIndex, columnName, cellValue);
                            break;
                        }
                    default:
                        {
                            DataGridViewHelper.SetCellValue(dtgvAccount, rowIndex, columnName, cellValue, 1);
                            break;
                        }
                }

            }
        }
        private string GetCellValue(int rowIndex, string columnName)
        {
            return DataGridViewHelper.GetCellValue(dtgvAccount, rowIndex, columnName);
        }
        private void btnDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tableLayoutPanel.Controls.Count > 0)
            {
                RepositionLDPlayers(int.Parse(btnDisplay.Text.Trim()));
            }

        }
        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageCommon.ShowConfirmationBox("Are you sure you want to close the software?");
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true; // Hủy việc đóng Form nếu người dùng không đồng ý
                }
                else
                {
                    saveProperties();
                    Application.ExitThread();
                    Application.Exit();
                }
            }
        }
        private List<string> GetListSelect()
        {
            List<string> list = new List<string>();
            try
            {
                for (int i = 0; i < dtgvAccount.RowCount; i++)
                {
                    if (Convert.ToBoolean(dtgvAccount.Rows[i].Cells["ChooseCol"].Value))
                    {
                        list.Add(dtgvAccount.Rows[i].Cells["cIdAccount"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }
            return list;
        }
        private void loadAccount()
        {
            try
            {
                lock (_lockObject)
                {
                    var accounts = _accountRepository.GetAll();
                    dtgvAccount.Invoke((MethodInvoker)delegate
                    {
                        dtgvAccount.Rows.Clear();
                        if (accounts.Count > 0)
                        {
                            int i = 1;
                            foreach (var a in accounts)
                            {
                                dtgvAccount.Rows.Add(false, i, a.Phonenumber, a.Email, a.PasswordEmail, a.Server, a.Port, a.Username, a.Password, a.FullName, a.Proxy, a.TowFA, a.Backup, a.Avatar, a.Status, a.CreateDate, a.Id);
                                switch (a.Status)
                                {
                                    case "new":
                                        {
                                            dtgvAccount.Rows[i - 1].DefaultCellStyle.BackColor = Color.White;
                                            i++;
                                            continue;
                                        }
                                    case "Done":
                                        {
                                            dtgvAccount.Rows[i - 1].DefaultCellStyle.BackColor = Color.LightGreen;
                                            i++;
                                            continue;
                                        }
                                    default:
                                        {
                                            dtgvAccount.Rows[i - 1].DefaultCellStyle.BackColor = Color.Pink;
                                            i++;
                                            continue;
                                        }

                                }

                            }
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
            }
        }
        private void dtgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                try
                {
                    dtgvAccount.CurrentRow.Cells["ChooseCol"].Value = !Convert.ToBoolean(dtgvAccount.CurrentRow.Cells["ChooseCol"].Value);
                    numberAccount.Value = GetListSelect().Count;
                }
                catch (Exception ex)
                {
                    MessageCommon.ShowMessageBox(ex.Message, 4);
                    RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                    Log.Error(ex, ex.Message);
                }
            }
        }

        private void dtgvAccount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                try
                {
                    dtgvAccount.CurrentRow.Cells["ChooseCol"].Value = !Convert.ToBoolean(dtgvAccount.CurrentRow.Cells["ChooseCol"].Value);
                    numberAccount.Value = GetListSelect().Count;
                }
                catch (Exception ex)
                {
                    MessageCommon.ShowMessageBox(ex.Message, 4);
                    RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                    Log.Error(ex, ex.Message);
                }
            }
        }

        private void addViewControl(DeviceInfo device, TableLayoutPanel tableLayoutPanel)
        {
            try
            {
                //IntPtr ldPlayerHandle = IntPtr.Zero; // Lưu trữ handle của cha trước khi gim
                string name = $"Qnibot{device.IndexLDPlayer}";
                LDController.ReName("index", device.IndexLDPlayer, name);
                Log.Information($"IsDevice_Running {name}");
                bool result = true;
                while (result)
                {
                    Thread.Sleep(2000);
                    // Thử tìm LDPlayer với tên "name"
                    var procs = Process.GetProcessesByName("dnplayer");
                    Log.Information($"addViewControl {procs.Count()}");
                    foreach (var proc in procs)
                    {
                        Log.Information($"proc {proc.MainWindowTitle}");
                        if (proc.MainWindowTitle == name)
                        {
                            device.view.LdplayerHandle = proc.MainWindowHandle;

                            // Hiển thị LDPlayer trong panel nếu nó đã bị gỡ gim
                            if (!device.view.IsPinned && device.view.LdplayerHandle != IntPtr.Zero)
                            {
                                device.view.Panel.Size = new Size(200, 400);
                                device.view.Embeddedpanel.Size = new Size(200, 350);
                                device.view.Embeddedpanel.Location = new Point(0, 0);
                                device.view.Embeddedpanel.Enabled = false;
                                SetParent(device.view.LdplayerHandle, device.view.Embeddedpanel.Handle);
                                MoveWindow(device.view.LdplayerHandle, 0, 0, device.view.Embeddedpanel.Width, device.view.Embeddedpanel.Height, true);
                                device.view.Panel.Controls.Add(device.view.Embeddedpanel);

                                device.view.PanelButton.Size = new Size(200, 50);
                                device.view.PanelButton.Location = new Point(0, device.view.Embeddedpanel.Height);

                                // Thêm Label cho mỗi LDPlayer
                                device.view.StatusLabel.ForeColor = Color.Green;
                                device.view.StatusLabel.Text = "Running";
                                device.view.StatusLabel.Dock = DockStyle.Bottom;
                                device.view.PanelButton.Controls.Add(device.view.StatusLabel);

                                // Thêm Button để gỡ gim hoặc gim lại
                                device.view.BtnClose.Text = device.view.IsPinned ? "Pin" : "Unpin";
                                device.view.BtnClose.Dock = DockStyle.Bottom;
                                PictureBox iconPictureBox = new PictureBox();
                                iconPictureBox.Size = new Size(16, 16); // Kích thước biểu tượng
                                iconPictureBox.Location = new Point(5, 5); // Vị trí của biểu tượng trong nút
                                iconPictureBox.Image = Properties.Resources.UnpinIcon; // Thay đổi thành biểu tượng của bạn
                                device.view.BtnClose.Click += (sender, args) =>
                                {
                                    device.view.IsPinned = !device.view.IsPinned;
                                    // Đảo ngược trạng thái của LDPlayer (gỡ gim hoặc gim lại)
                                    if (device.view.IsPinned)
                                    {
                                        // Gỡ gim LDPlayer
                                        device.view.Embeddedpanel.Invoke((Action)(() =>
                                        {
                                            SetParent(device.view.LdplayerHandle, device.view.originalParentHandle);
                                        }));
                                        //SetParent(device.view.LdplayerHandle, device.view.originalParentHandle); // Đặt lại cha của LDPlayer
                                        MoveWindow(device.view.LdplayerHandle, 0, 0, device.view.Embeddedpanel.Width, device.view.Embeddedpanel.Height, true);
                                        iconPictureBox.Image = Properties.Resources.PinIcon;
                                    }
                                    else
                                    {
                                        // Lưu trữ cha hiện tại của LDPlayer và gỡ gim
                                        device.view.originalParentHandle = NativeMethods.GetParent(device.view.LdplayerHandle);
                                        device.view.Embeddedpanel.Invoke((Action)(() =>
                                        {
                                            SetParent(device.view.LdplayerHandle, device.view.Embeddedpanel.Handle);
                                        }));
                                        //SetParent(device.view.LdplayerHandle, device.view.Embeddedpanel.Handle);
                                        MoveWindow(device.view.LdplayerHandle, 0, 0, device.view.Embeddedpanel.Width, device.view.Embeddedpanel.Height, true);
                                        iconPictureBox.Image = Properties.Resources.UnpinIcon;
                                    }
                                    device.view.BtnClose.Text = device.view.IsPinned ? "Pin" : "Unpin";
                                };
                                device.view.BtnClose.Controls.Add(iconPictureBox);
                                device.view.PanelButton.Controls.Add(device.view.BtnClose);
                                device.view.Panel.Controls.Add(device.view.PanelButton);
                                tableLayoutPanel.Controls.Add(device.view.Panel);
                                tableLayoutPanel.SetCellPosition(device.view.Panel, new TableLayoutPanelCellPosition(columnCount, rowCount));
                                pDevice.Controls.Add(tableLayoutPanel);
                                result = false;
                                return;
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }
        }
        private async Task ChangeProxy(string index, string deviceId, string ip, string port, string? username, string? password)
        {
            try
            {
                string cmd = $"shell am broadcast -a com.vat.vpn.CONNECT_PROXY -n com.vat.vpn/.ui.ProxyReceiver --es address {ip} --es port {port}";
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    cmd = $"shell am broadcast -a com.vat.vpn.CONNECT_PROXY -n com.vat.vpn/.ui.ProxyReceiver --es address {ip} --es port {port}  --es username {username} --es password {password}";
                }
                var connectProxie = ADBHelper.ADB(deviceId, cmd);
                if (string.IsNullOrEmpty(connectProxie) || !connectProxie.Contains("successful"))
                {
                    int i = 6;
                    while (i > 0)
                    {
                        LDController.RunApp("index", index, "com.vat.vpn");
                        string connect = Path.Combine(Environment.CurrentDirectory, "Database\\ImagesClick\\Proxy\\Connect.PNG");
                        string oke = Path.Combine(Environment.CurrentDirectory, "Database\\ImagesClick\\Proxy\\Ok.PNG");
                        string disconnection = Path.Combine(Environment.CurrentDirectory, "Database\\ImagesClick\\Proxy\\Disconnection.PNG");
                        if (ADBHelper.FindImage(deviceId, connect, 0.9, 30000))
                        {
                            ADBHelper.TapByPercent(deviceId, 35.6, 39.2);
                            Thread.Sleep(200);
                            ADBHelper.ClearInputWithADBKeyboard(deviceId);
                            Thread.Sleep(200);
                            ADBHelper.InputTextWithADBKeyboard(deviceId, ip);
                            Thread.Sleep(200);
                            ADBHelper.TapByPercent(deviceId, 79.8, 39.1);
                            Thread.Sleep(200);
                            ADBHelper.ClearInputWithADBKeyboard(deviceId);
                            Thread.Sleep(200);
                            ADBHelper.InputTextWithADBKeyboard(deviceId, port);
                            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                            {
                                username = "";
                                password = "";
                            }
                            Thread.Sleep(200);
                            ADBHelper.TapByPercent(deviceId, 49.4, 51.6);
                            Thread.Sleep(200);
                            ADBHelper.ClearInputWithADBKeyboard(deviceId);
                            Thread.Sleep(200);
                            ADBHelper.InputTextWithADBKeyboard(deviceId, username);
                            Thread.Sleep(200);
                            ADBHelper.TapByPercent(deviceId, 49.7, 61.1);
                            Thread.Sleep(200);
                            ADBHelper.ClearInputWithADBKeyboard(deviceId);
                            Thread.Sleep(200);
                            ADBHelper.InputTextWithADBKeyboard(deviceId, password);
                            Thread.Sleep(200);
                            if (ADBHelper.FindImageTap(deviceId, connect, 0.9, 30000))
                            {
                                if (ADBHelper.FindImageTap(deviceId, oke, 0.9, 30000))
                                {
                                    if (ADBHelper.FindImage(deviceId, disconnection, 0.25, 30000))
                                    {
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        LDController.KillApp("index", index, "com.vat.vpn");
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"ERROR: {nameof(fMain)}, params; {nameof(StartDevice)},Device; {index}, Proxy; {ip}:{port}:{username}:{password}, Error; {ex.Message}, Exception; {ex}");
                return;
            }
        }
        public class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetParent(IntPtr hWnd);
        }
        private void RepositionLDPlayers(int isRow = 5)
        {
            try
            {
                tableLayoutPanel.Invoke((MethodInvoker)delegate
                {
                    columnCount = 0;
                    rowCount = 0;
                    foreach (System.Windows.Forms.Control control in tableLayoutPanel.Controls)
                    {
                        if (control is Panel embeddedPanel)
                        {
                            int column = tableLayoutPanel.GetColumn(embeddedPanel);
                            int row = tableLayoutPanel.GetRow(embeddedPanel);

                            if (columnCount >= isRow)
                            {
                                rowCount++;
                                columnCount = 0;
                            }

                            tableLayoutPanel.SetCellPosition(embeddedPanel, new TableLayoutPanelCellPosition(columnCount, rowCount));
                            columnCount++;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        public void EmbedLDPlayer(IntPtr ldPlayerHandle, IntPtr panelHandle)
        {
            try
            {
                SetParent(ldPlayerHandle, panelHandle);
                const int GWL_STYLE = -16;
                const int WS_VISIBLE = 0x10000000;
                const int WS_CHILD = 0x40000000;
                int style = GetWindowLong(ldPlayerHandle, GWL_STYLE);
                style = style & ~WS_VISIBLE;
                style = style | WS_CHILD;
                SetWindowLong(ldPlayerHandle, GWL_STYLE, new IntPtr(style));
                MoveWindow(ldPlayerHandle, 0, 0, Width, Height, true);
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }

        }
        private void rebootLDPlayer(DeviceInfo device, bool isChange = true)
        {
            try
            {
                bool result = true;
                while (result)
                {
                    LDController.Open("index", device.IndexLDPlayer, isChange);
                    Thread.Sleep(5000);
                    var name = $"Qnibot{device.IndexLDPlayer}";
                    LDController.ReName("index", device.IndexLDPlayer, name);
                    var proc1 = Process.GetProcessesByName("dnplayer");
                    Parallel.ForEach(proc1, proc =>
                    {
                        if (proc.MainWindowTitle == name)
                        {
                            Thread.Sleep(1000);
                            device.view.LdplayerHandle = proc.MainWindowHandle;
                            device.view.Embeddedpanel.Invoke((Action)(() =>
                            {
                                SetParent(device.view.LdplayerHandle, device.view.Embeddedpanel.Handle);
                            }));
                            MoveWindow(device.view.LdplayerHandle, 0, 0, device.view.Embeddedpanel.Width, device.view.Embeddedpanel.Height, true);
                            Thread.Sleep(3000);
                            result = false;
                            device.view.IsPinned = false; // Hoặc true tùy thuộc vào trạng thái sau reboot
                            device.view.BtnClose.Text = device.view.IsPinned ? "Pin" : "Unpin";
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }
        }


        private void rtbLogs_DoubleClick(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("Log.txt"))
            {
                if (!File.Exists("Log.txt"))
                {
                    File.WriteAllBytes("Log.txt", new byte[0]);
                }

                writer.Write(rtbLogs.Text);
                writer.Close();
            }
            string notepadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "notepad.exe");
            Process.Start(notepadPath, Path.Combine("Log.txt"));
        }

        private void rtbResult_DoubleClick(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("Result.txt"))
            {
                if (!File.Exists("Result.txt"))
                {
                    File.WriteAllBytes("Result.txt", new byte[0]);
                }
                writer.Write(rtbResult.Text);
                writer.Close();
            }
            string notepadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "notepad.exe");
            Process.Start(notepadPath, Path.Combine("Result.txt"));
        }

        private void btnLoadDevice_Click(object sender, EventArgs e)
        {
            loadDevice();
        }

        private void dtgvLDPlayers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                try
                {
                    dtgvLDPlayers.CurrentRow.Cells["cChose"].Value = !Convert.ToBoolean(dtgvLDPlayers.CurrentRow.Cells["cChose"].Value);
                }
                catch (Exception ex)
                {
                    MessageCommon.ShowMessageBox(ex.Message, 4);
                    RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                    Log.Error(ex, ex.Message);
                }
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            fAddFile fAddFile = new fAddFile("Accounts", _listEmail);
            fAddFile.ShowDialog();
            _listEmail.Clear();
            try
            {
                using (StreamWriter writer = new StreamWriter(GlobalModels.PathEmail))
                {
                    foreach (var line in fAddFile._list)
                    {
                        _listEmail.Add(line);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex, ex.Message);
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 2);
            }
        }

        private void dtgvAccount_SelectionChanged(object sender, EventArgs e)
        {
        }
        private void writeLog(DeviceInfo device, string message, string? idAccount, Label? label, int type = 0, bool isWriteRichTextBox = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(idAccount))
                {
                    DataGridViewHelper.SetCellValueByColumnValue(dtgvAccount, idAccount, "cIdAccount", "cStatus", message, type);
                }
                if (isWriteRichTextBox)
                {
                    RichTextBoxHelper.WriteLogRichTextBox($"LDPLayer: {device.IndexLDPlayer}  {message}", type);
                }
                //if (!string.IsNullOrEmpty(device.IndexLDPlayer))
                //{
                //    DataGridViewHelper.SetCellValueByColumnValue(dtgvLDPlayers, device.IndexLDPlayer, "tIndex", "tStatus", message, type);
                //}
                if (label != null)
                {
                    LabaleHelper.WriteLabale(label, message, type);
                }

            }
            catch (Exception ex)
            {
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }

        }

        private void CBoxOtpService_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void CBoxOtpService_TextChanged(object sender, EventArgs e)
        {
            if (CBoxOtpService.Text == "Viotp.com")
            {
                CBServiceCode.Visible = false;
                plContry.Visible = true;
                rbLaos.Visible = true;
            }
            else if (CBoxOtpService.Text == "Yuenanka.com")
            {
                CBServiceCode.Visible = false;
                rbLaos.Visible = false;
                plContry.Visible = true;
            }
            else
            {
                CBServiceCode.Visible = true;
                plContry.Visible = false;
            }
        }

        private void CBServiceCode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CBServiceCode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_OtpCountry))
            {
                _OtpCountry = CBServiceCode.SelectedValue.ToString();
                SettingsTool.GetSettings("configGeneral").AddValue("OtpCountry", _OtpCountry);
                SettingsTool.UpdateSetting("configGeneral");
                if (CBoxOtpService.Text == ServicesOtp.Web5Sim)
                {

                    var line = listCountryCode.Where(x => x.Dial_code == _OtpCountry).FirstOrDefault().alias;
                    if (!string.IsNullOrEmpty(line))
                    {
                        GlobalModels.Contry = line;
                    }
                    else
                    {
                        MessageCommon.ShowMessageBox(" No Support Country", 3);
                        return;
                    }

                }
                if (CBoxOtpService.Text == ServicesOtp.Sms365 || CBoxOtpService.Text.Contains(ServicesOtp.Getsmscode))
                {
                    var line = listCountryCode.Where(x => x.Dial_code == _OtpCountry).FirstOrDefault().countryId;
                    if (!string.IsNullOrEmpty(line))
                    {
                        GlobalModels.Contry = line;
                    }
                    else
                    {
                        MessageCommon.ShowMessageBox("No Support Country", 3);
                        return;
                    }
                }
            }
        }
        private void loadFileJsonService()
        {
            CountryCodeHelper cn = new CountryCodeHelper();
            listCountryCode = cn.GetCountryCodes();
            foreach (var code in listCountryCode)
            {
                listItem.Add(new OtpServices.CountryHelper.ComboBoxItem
                {
                    Value = code.Dial_code,
                    DisplayText = code.Name + " " + code.Dial_code
                });
            }
            CBServiceCode.DataSource = listItem;
            var t = jsonHelper.GetValuesFromInputString("OtpCountry");
            if (!string.IsNullOrEmpty(t))
            {
                var countryCodeSelected = listCountryCode.FirstOrDefault(s => s.Dial_code == t);
                if (countryCodeSelected != null)
                {
                    var index = listCountryCode.IndexOf(countryCodeSelected);
                    CBServiceCode.SelectedIndex = index;
                }
            }
            CBoxOtpService_TextChanged(null, null);
        }

        private void rbRegWithPhone_CheckedChanged(object sender, EventArgs e)
        {
            gbPhone.Visible = true;
            btnAddAccount.Visible = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void rbRegWithEmail_CheckedChanged(object sender, EventArgs e)
        {
            gbPhone.Visible = false;
            btnAddAccount.Visible = true;
        }

        private void btnDisPlayConfiguration_Click(object sender, EventArgs e)
        {
            FormHelper.ShowFormWithoutTaskbar(new fDisplayConf());
            GetConfigDatagridview();
        }
        private void GetConfigDatagridview()
        {
            try
            {
                dtgvAccount.Columns["cPhone"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbPhonenumber");
                dtgvAccount.Columns["cEmail"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbEmail");
                dtgvAccount.Columns["cPasswordEmail"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbPasswordEmail");
                dtgvAccount.Columns["cServer"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbServer");
                dtgvAccount.Columns["cPort"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbPort");
                dtgvAccount.Columns["cUsername"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbUsername");
                dtgvAccount.Columns["cPassword"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbPassword");
                dtgvAccount.Columns["cFullname"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbFullname");
                dtgvAccount.Columns["cProxy"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbProxy");
                dtgvAccount.Columns["c2FA"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbTowFa");
                dtgvAccount.Columns["cBackup"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbBackup");
                dtgvAccount.Columns["cStatus"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbStatus");
                dtgvAccount.Columns["cAvatar"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbAvatar");
                dtgvAccount.Columns["cCreateAt"].Visible = SettingsTool.GetSettings("configDatagridview").GetBooleanValue("ckbDay");
            }
            catch (Exception ex)
            {
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 1);
                Log.Error(ex, ex.Message);
            }

        }

        private void ckbAddEmail_CheckedChanged(object sender, EventArgs e)
        {
            btnSettingEmail.Enabled = ckbAddEmail.Checked;
        }

        private void btnSettingEmail_Click(object sender, EventArgs e)
        {
            fAddFile fAddFile = new fAddFile("EmailRecovery", _listEmailRecovery);
            fAddFile.ShowDialog();
            _listEmailRecovery.Clear();
            try
            {
                using (StreamWriter writer = new StreamWriter(GlobalModels.PathEmailRecovery))
                {
                    foreach (var line in fAddFile._list)
                    {
                        _listEmailRecovery.Add(line);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex, ex.Message);
                MessageCommon.ShowMessageBox(ex.Message, 4);
                RichTextBoxHelper.WriteLogRichTextBox(ex.Message, 2);
            }
        }

        private void ckbUploadPost_CheckedChanged(object sender, EventArgs e)
        {
            txtFolderPost.Enabled = ckbUploadPost.Checked;
        }

        private void ckbFollowSuggested_CheckedChanged(object sender, EventArgs e)
        {
            plFollow.Enabled = ckbFollowSuggested.Checked;
        }

        private void rtbLogs_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gbPhone.Visible = false;
            btnAddAccount.Visible = false;
        }
    }
}