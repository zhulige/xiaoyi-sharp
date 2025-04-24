using OpenCvSharp;
using RestSharp;
using System.Text.RegularExpressions;
using XiaoYiSharp;

namespace XiaoYiSharp_BlazorApp.Pages
{
    public partial class Index
    {
        ////private XiaoYiSharp.XiaoYiAgent _xiaoyiAgent;
        ////private string _serverUrl = "http://192.168.31.113:8888/";
        ////private static VideoCapture _capture = new VideoCapture(0);
        //private static bool _isFirst = true;
        ////private Mat _frame = new Mat();
        //private System.Timers.Timer _timer = new System.Timers.Timer(1000) { Enabled = true };

        ////public string Message = string.Empty;
        ////public string Image { get { 
        ////    string str = "http://192.168.31.113:8888/xiaoyi/v1/image/stream/" + _xiaoyiAgent.DeivceId;
        ////        return str;
        ////    } 
        ////}

        //protected override async Task OnInitializedAsync()
        //{
        //    //if (_isFirst)
        //    //{
        //    //    _isFirst = false;

        //        _timer.Elapsed += Timer_Elapsed;
        //    //}
        //    //await base.OnInitializedAsync();
        //}

        //private Task XiaoyiAgent_OnMessageEvent(string message)
        //{
        //    Message = message;
        //    Console.WriteLine(message);
        //    return Task.CompletedTask; 
        //}

        //private async void Timer_Elapsed(object sender, EventArgs args)
        //{
        //    await Task.Factory.StartNew(() =>
        //    {
        //        InvokeAsync(() => { StateHasChanged(); return Task.CompletedTask; });
        //    });
        //}
        //private bool _isPostImage = true;
        //private async Task PostImage()
        //{
        //    try
        //    {
        //        // 检查摄像头是否成功打开
        //        if (_capture.IsOpened())
        //        {
        //            if (_isPostImage)
        //            {
        //                _isPostImage = false;
        //                // 读取摄像头的一帧
        //                _capture.Read(_frame);

        //                Console.WriteLine(_frame.Width);

        //                // 将 OpenCvSharp 的 Mat 转换为字节数组
        //                byte[] imageData = _frame.ImEncode(".jpg");

        //                // 创建 RestSharp 客户端和请求
        //                var client = new RestClient(_serverUrl);
        //                var request = new RestRequest("xiaoyi/v1/image/stream/" + _xiaoyiAgent.DeivceId, Method.Post);

        //                // 添加请求头和图像数据
        //                request.AddHeader("Content-Type", "multipart/form-data");
        //                request.AddFile("file", imageData, "image.jpg", "image/jpeg");

        //                // 发送请求并获取响应
        //                var response = await client.ExecuteAsync(request);

        //                _isPostImage = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _isPostImage = true;
        //        Console.WriteLine($"Error in PostImage: {ex.Message}");
        //    }
        //}
        //private async Task SendMessage()
        //{
        //    if (_xiaoyiAgent != null)
        //    {
        //        await _xiaoyiAgent.SendMessage("帮我看看图里有什么内容？");
        //    }
        //}

        //private async Task SendImage()
        //{
        //    if (_xiaoyiAgent != null)
        //    {
        //        await _xiaoyiAgent.SendMessage("帮我看看图里有什么内容？", "https://wx.sviin.cn/logo.png");
        //    }
        //}


        //void IDisposable.Dispose()
        //{
        //    //_timer.Close();
        //}
    }
}
