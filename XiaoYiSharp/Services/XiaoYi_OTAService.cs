using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace XiaoYiSharp.Services
{
    public class XiaoYi_OTAService
    {
        public string OTA_VERSION_URL { get; set; } = "http://coze.nbee.net/xiaoyi/ota"; //"https://api.tenclass.net/xiaozhi/ota/";
        public dynamic? OTA_INFO { get; set; }
        public string DeviceId { get; set; } = Utils.SystemInfo.GetMacAddress();

        public XiaoYi_OTAService()
        {
            
        }

        public void Start()
        {
            Thread _otaThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Get_OTA_Info();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Thread.Sleep(1000 * 60);
                }
            });
            _otaThread.Start();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        private void Get_OTA_Info()
        {
            try
            {
                var client = new RestClient(OTA_VERSION_URL);
                var request = new RestRequest();
                request.AddHeader("Device-Id", DeviceId);
                request.AddHeader("Content-Type", "application/json");

                DateTime currentUtcTime = DateTime.UtcNow;
                string format = "MMM dd yyyyT HH:mm:ssZ";
                string formattedTime = currentUtcTime.ToString(format, System.Globalization.CultureInfo.InvariantCulture);

                var postData = new
                {

                    application = new
                    {
                        name = "XiaoYi",
                        version = "1.5.5"
                    }
                };

                request.AddJsonBody(postData);

                var response = client.Post(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("获取OTA版本信息失败!");
                }
                if (response.Content != null && response.Content != "")
                {
                    OTA_INFO = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    if (OTA_INFO != null && OTA_INFO.activation != null)
                    {
                        Console.WriteLine($"请先登录xiaozhi.me,绑定Code：{(string)OTA_INFO.activation.code}");
                    }
                    //Console.WriteLine(response.Content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取OTA版本信息时发生异常: {ex.Message}，确保 {OTA_VERSION_URL} 内设备管理中你的MAC地址配置正确：{DeviceId}");
            }
        }
    }
}
