using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TikTokCreator.Entities;
using TikTokCreator.Forms;
using TikTokCreator.Helper;
using TikTokCreator.Repositories;

namespace TikTokCreator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        private static readonly IHost _host = CreateHostBuilder();
        private static string dateFile = Path.Combine(Environment.CurrentDirectory, "Data\\Database\\DataAccount.mdf");
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.File("LOGSAPP/myapp.txt", rollingInterval: RollingInterval.Day)
                 .CreateLogger();
            string folder = Path.Combine(Environment.CurrentDirectory, "DataImport\\User");
            string folderSession = Path.Combine(Environment.CurrentDirectory, "DataImport\\Admin\\Session");
            Log.Information("Application start 1");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!Directory.Exists(folderSession))
            {
                Directory.CreateDirectory(folderSession);
            }

            if (string.IsNullOrEmpty((string)Properties.Settings.Default["SetupEnvironment"]))
            {
                try
                {
                    InstallerHelper.CheckAndInstallDependencies();
                }
                catch (Exception)
                {
                }
                Properties.Settings.Default["SetupEnvironment"] = "Done";
                Properties.Settings.Default.Save();
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            try
            {
                _host.Start();
                //Đoạn này mặc định của winform kệ nó thôi.
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Lấy ra cái Form1 đã được khai báo trong services
                try
                {
                    var form1 = _host.Services.GetRequiredService<fLogin>();
                    //Lệnh chạy gốc là: Application.Run(new Form1);
                    //Đã được thay thế bằng lệnh sử dụng service khai báo trong host
                    Application.Run(form1);
                    Log.Information("Application start");
                }
                catch (Exception ex)
                {

                    Log.Error(ex.Message);
                    if (ex.InnerException != null)
                    {
                        Log.Error(ex.ToString());
                        Log.Error(ex.InnerException.Message);
                    }
                }

                //Khi form chính (form1) bị đóng <==> chương trình kết thúc ấy
                //thì dừng host
                _host.StopAsync().GetAwaiter().GetResult();
                //và giải phóng tài nguyên host đã sử dụng.
                _host.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                if (ex.InnerException != null)
                {
                    Log.Error(ex.ToString());
                    Log.Error(ex.InnerException.Message);
                }
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        static IHost CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<fLogin>();
                    services.AddTransient<IAccountRepository, AccountRepository>();
                    //services.AddTransient<IInstagramController, InstagramController>();
                    services.AddSingleton<fMain>();
                    services.AddDbContext<ApplicationDbContext>(s => s.UseSqlServer($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dateFile};Integrated Security=True;Connect Timeout=30"));
                }).Build();
        }
    }
}