﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using System.Net.NetworkInformation;
using System.Text;
using XiaoYiSharp_MauiBlazorApp.Platforms.Android.ForegroundServices;

namespace XiaoYiSharp_MauiBlazorApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            var androidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var formattedAndroidId = FormatAndroidIdToMacFormat(androidId);
            Global.DeivceId = formattedAndroidId;

            // 启动服务
            var intent = new Intent(this, typeof(MyForegroundService));
            //if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            //{
            StartForegroundService(intent);
            //}
            //else
            //{
            //    StartService(intent);
            //}


            // 停止服务
            //StopService(new Intent(this, typeof(MyForegroundService)));

            base.OnCreate(savedInstanceState);
        }

        // 将 Android ID 格式化为 MAC 地址格式
        public string FormatAndroidIdToMacFormat(string androidId)
        {
            if (string.IsNullOrEmpty(androidId))
            {
                return string.Empty;
            }

            StringBuilder formattedId = new StringBuilder();
            for (int i = 0; i < 12; i++)
            {
                formattedId.Append(androidId[i]);
                if ((i + 1) % 2 == 0 && i < 12 - 1)
                {
                    formattedId.Append(":");
                }
            }
            return formattedId.ToString();
        }
    }
}