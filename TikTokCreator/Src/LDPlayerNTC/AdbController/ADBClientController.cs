using AdvancedSharpAdbClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LDPlayerAndADBController.AdbController
{
    public class ADBClientController
    {
        public static bool ClearTextElement(DeviceData deviceData, AdbClient adbClient, string xpath, int charCount = 30, int timeout = 30)
        {
            try
            {
                for (int j = 0; j < timeout; j++)
                {
                    Element element = adbClient.FindElement(deviceData, "//node[@" + xpath + "]", TimeSpan.FromSeconds(1));
                    if (element != null)
                    {
                        element.Click();
                        element.ClearInput(30);
                        return true;
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ADBClientController)}, params; {nameof(ClearTextElement)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
            }
            return false;
        }
        public static bool ClickElement(DeviceData deviceData, AdbClient adbClient, string xpath, int timeout)
        {
            try
            {
                for (int j = 0; j < timeout; j++)
                {
                    Element element = adbClient.FindElement(deviceData, "//node[@" + xpath + "]", TimeSpan.FromSeconds(1));
                    if (element != null)
                    {
                        element.Click();
                        return true;
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ADBClientController)}, params; {nameof(ClickElement)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
            }
            return false;
        }
        public static Element FindElement(DeviceData deviceData, AdbClient adbClient, string xpath, int timeout)
        {
            try
            {
                for (int j = 0; j < timeout; j++)
                {
                    Element element = adbClient.FindElement(deviceData, "//node[@" + xpath + "]", TimeSpan.FromSeconds(1));
                    if (element != null)
                    {
                        return element;
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ADBClientController)}, params; {nameof(FindElement)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
            }
            return null;
        }
        public static bool InputElement(DeviceData deviceData, AdbClient adbClient, string xpath, string text, int timeout = 30)
        {
            try
            {
                for (int j = 0; j < timeout; j++)
                {
                    Element element = adbClient.FindElement(deviceData, "//node[@" + xpath + "]", TimeSpan.FromSeconds(1));
                    if (element != null)
                    {
                        element.Click();
                        element.ClearInput(30);
                        element.SendText(text);
                        return true;
                    }
                    Thread.Sleep(500);

                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ADBClientController)}, params; {nameof(InputElement)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
            }
            return false;
        }

        public static bool SwipeElement(DeviceData deviceData, AdbClient adbClient, string xpathFirst, string xpathSecond, int speed, int timeout)
        {
            try
            {
                for (int j = 0; j < timeout; j++)
                {
                    var start = adbClient.FindElement(deviceData, "//node[@" + xpathFirst + "]", TimeSpan.FromSeconds(1));
                    var end = adbClient.FindElement(deviceData, "//node[@" + xpathSecond + "]", TimeSpan.FromSeconds(1));
                    if (start != null && end != null)
                    {
                        adbClient.Swipe(deviceData, start, end, timeout);
                        return true;
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ADBClientController)}, params; {nameof(SwipeElement)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
            }
            return false;
        }
        public static List<Element> FindElements(DeviceData deviceData, AdbClient adbClient, string xpath, int timeout)
        {
            List<Element> result = new List<Element>();
            try
            {
                for (int j = 0; j < timeout; j++)
                {
                    var elements = adbClient.FindElements(deviceData, "//node[@" + xpath + "]", TimeSpan.FromSeconds(1));
                    result.Clear();
                    if (elements != null)
                    {
                        foreach (Element element in elements)
                        {
                            result.Add(element);
                        }
                        if (result.Count > 0)
                        {
                            return result;
                        }
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ADBClientController)}, params; {nameof(FindElements)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
            }
            return result;
        }
        public static bool FindElementIsExistOrClickByClass(DeviceData deviceData, AdbClient adbClient, string name, string nameClass, int timeout = 15, bool isClick = false)
        {
            try
            {
                for (int j = 0; j < timeout; j++)
                {
                    var elements = adbClient.FindElements(deviceData, $"//node[@class='{nameClass}']", TimeSpan.FromSeconds(1));
                    if (elements != null && elements.Any())
                    {
                        foreach (Element element in elements)
                        {
                            if (element.Attributes.ContainsValue(name))
                            {
                                if (isClick)
                                {
                                    element.Click();
                                }
                                return true;
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($" {nameof(ADBClientController)}, params; {nameof(FindElementIsExistOrClickByClass)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
                return false;
            }
        }
        public static bool StartApp(AdbClient adbClient, DeviceData deviceData, string package)
        {
            try
            {
                adbClient.StartApp(deviceData, package);
                Thread.Sleep(2000);
                if (adbClient.IsAppRunning(deviceData, package))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"End {nameof(ADBClientController)}, params; {nameof(StartApp)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
                return false;
            }
        }
        public static bool StoptApp(AdbClient adbClient, DeviceData deviceData, string package)
        {
            try
            {
                adbClient.StopApp(deviceData, package);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"End {nameof(ADBClientController)}, params; {nameof(StoptApp)},deviceId; {deviceData.Serial}, Error; {ex.Message}, Exception; {ex}");
                return false;
            }
        }
    }
}
