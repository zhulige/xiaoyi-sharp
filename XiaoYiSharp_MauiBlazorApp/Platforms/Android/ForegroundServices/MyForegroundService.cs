using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using ForegroundService = Android.Content.PM.ForegroundService;

namespace XiaoYiSharp_MauiBlazorApp.Platforms.Android.ForegroundServices
{
    [Service(Label = "ForegroundService", Name = "com.nbeenet.xiaoyiApp.MyForegroundService", Exported = true)]
    [IntentFilter(new string[] { "com.nbeenet.xiaoyiApp.ForegroundService" })]
    public sealed class MyForegroundService : Service
    {
        private const int SERVICE_ID = 1001;
        private const string CHANNEL_ID = "ForegroundServiceChannel";
        private CancellationTokenSource _cts;

        public override void OnCreate()
        {
            base.OnCreate();
            _cts = new CancellationTokenSource();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // 创建通知渠道（Android 8.0+ 需要）
            CreateNotificationChannel();

            // 创建通知
            var notification = new NotificationCompat.Builder(this, CHANNEL_ID)
                .SetContentTitle("服务运行中")
                .SetContentText("后台任务执行中...")
                //.SetSmallIcon(Resource.Drawable.ic_notification)
                .SetOngoing(true)
                .Build();

            // 检查 Android 版本以避免 CA1416 警告
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q) // Android 10 (API 29)
            {
                StartForeground(SERVICE_ID, notification,ForegroundService.TypeDataSync);
            }
            else
            {
                StartForeground(SERVICE_ID, notification);
            }

            // 启动后台任务
            Task.Run(() => RunBackgroundTask(_cts.Token));

            // 告诉系统在服务被杀死后不要重启
            return StartCommandResult.Sticky;
        }

        private async Task RunBackgroundTask(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    // 执行后台任务，例如数据同步、位置跟踪等
                    System.Diagnostics.Debug.WriteLine($"后台任务执行中: {DateTime.Now}");

                    // 模拟耗时操作
                    await Task.Delay(5000, token);
                }
            }
            catch
            {
                // 任务被取消
            }
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    CHANNEL_ID,
                    "Foreground Service Channel",
                    NotificationImportance.Default);

                var manager = GetSystemService(NotificationService) as NotificationManager;
                manager.CreateNotificationChannel(channel);
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null; // 不支持绑定
        }

        public override void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            base.OnDestroy();
        }
    }
}
