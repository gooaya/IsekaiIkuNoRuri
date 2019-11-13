using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using FiddlerCore.NetCore;
using Android.Content;
using System.IO;
using Android.Util;
using System;
using Android.Support.V4.App;

namespace ruri
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private bool isStarted;

        // ProxyServiceConnection serviceConnection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            OnNewIntent(this.Intent);

            if (savedInstanceState != null)
            {
                isStarted = savedInstanceState.GetBoolean(Constants.SERVICE_STARTED_KEY, false);
            }

            Intent startServiceIntent = new Intent(this, typeof(ProxyService));
            startServiceIntent.SetAction(Constants.ACTION_START_SERVICE);

            Intent stopServiceIntent = new Intent(this, typeof(ProxyService));
            stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);

            var switchView = FindViewById<Switch>(Resource.Id.switchBeacon);
            switchView.Checked = isStarted;
            switchView.CheckedChange += (o, e) =>
            {
                if (e.IsChecked)
                    StartService(startServiceIntent);
                else
                    StartService(stopServiceIntent);
            };

            FindViewById<Button>(Resource.Id.buttonConsole).Click += (o, e) =>
            {
                var uri = Android.Net.Uri.Parse("http://console.nono.nyanbox.com/IsekaiIkuNoRuri/index.html");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

            FindViewById<Button>(Resource.Id.buttonGithub).Click += (o, e) =>
            {
                var uri = Android.Net.Uri.Parse("https://github.com/gooaya/IsekaiIkuNoRuri");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };
        }

        protected override void OnNewIntent(Intent intent)
        {
            var bundle = intent == null ? null : intent.Extras;
            if (bundle == null) return;
            isStarted = bundle.ContainsKey(Constants.SERVICE_STARTED_KEY);
        }

        protected override void OnDestroy()
        {
            if (this.IsFinishing)
            {
                Intent stopServiceIntent = new Intent(this, typeof(ProxyService));
                stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);
                StartService(stopServiceIntent);
            }
            base.OnDestroy();
        }
    }

    public static class Constants
    {
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string SERVICE_STARTED_KEY = "has_service_been_started";

        public const string ACTION_START_SERVICE = "ruri.action.START_SERVICE";
        public const string ACTION_STOP_SERVICE = "ruri.action.STOP_SERVICE";
        public const string ACTION_RESTART_TIMER = "ruri.action.RESTART_TIMER";
        public const string ACTION_MAIN_ACTIVITY = "ruri.action.MAIN_ACTIVITY";
    }


    // https://github.com/xamarin/monodroid-samples/tree/master/ApplicationFundamentals/ServiceSamples/ForegroundServiceDemo
    [Service]
    [IntentFilter(new String[] { "com.xamarin.ProxyService" })]
    public class ProxyService : Service
    {
        string _userDataPath;

        static readonly ProxyController controller = new ProxyController();
        static readonly string TAG = typeof(ProxyService).FullName;
        public IBinder Binder { get; private set; }
        bool isStarted;

        public override void OnCreate()
        {
            // This method is optional to implement
            base.OnCreate();
            Log.Debug(TAG, "OnCreate");
            _userDataPath = Path.Combine(ApplicationContext.GetExternalFilesDir(null).Path, "userData-0.7.8.json");
            if (!controller.Inited)
            {
                var packageInfo = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0);
                var packageVersion = packageInfo.VersionName.ToString();

                if (!File.Exists(_userDataPath))
                {
                    using (StreamReader sr = new StreamReader(this.Assets.Open("userData.json")))
                    {
                        controller.Init(@"{""userData"":" + sr.ReadToEnd() + @",""serverData"":{},""version"":""" + packageVersion + @"""}", packageVersion);
                    }
                }
                else
                {
                    string userData = File.ReadAllText(_userDataPath);
                    controller.Init(userData, packageVersion);
                }
            }
        }

        // Magical code that makes the service do wonderful things.
        public override IBinder OnBind(Intent intent)
        {
            // This method must always be implemented
            // Log.Debug(TAG, "OnBind");
            // this.Binder = new ProxyBinder(this);
            // return this.Binder;
            return null;
        }
        NotificationManager manager;
        NotificationManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = (NotificationManager)GetSystemService(NotificationService);
                }
                return manager;
            }
        }

        String createNotificationChannel(String channelId, String channelName)
        {
            var chan1 = new NotificationChannel(channelId, channelName, NotificationImportance.Default);
            // chan1.LightColor = Color.Green;
            chan1.LockscreenVisibility = NotificationVisibility.Private;
            Manager.CreateNotificationChannel(chan1);
            return channelId;
        }

        void RegisterForegroundService()
        {

            Notification notification = new NotificationCompat.Builder(this, createNotificationChannel("ruri_channel", "Ruri Channel"))
                .SetContentTitle(Resources.GetString(Resource.String.desc_connected))
                .SetSmallIcon(Resource.Mipmap.ic_launcher_foreground)
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                .AddAction(BuildStopServiceAction())
                .Build();


            // Enlist this instance of the service as a foreground service
            StartForeground(Constants.SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }

        PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return pendingIntent;
        }

        NotificationCompat.Action BuildStopServiceAction()
        {
            var stopServiceIntent = new Intent(this, GetType());
            stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);
            var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

            var builder = new NotificationCompat.Action.Builder(Android.Resource.Drawable.IcMediaPause,
                                                          GetText(Resource.String.action_disconnect),
                                                          stopServicePendingIntent);
            return builder.Build();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals(Constants.ACTION_START_SERVICE))
            {
                if (isStarted)
                {
                    Log.Info(TAG, "OnStartCommand: The service is already running.");
                }
                else
                {
                    Log.Info(TAG, "OnStartCommand: The service is starting.");
                    RegisterForegroundService();
                    controller.StartProxy();
                    isStarted = true;
                }
            }
            else if (intent.Action.Equals(Constants.ACTION_STOP_SERVICE))
            {
                Log.Info(TAG, "OnStartCommand: The service is stopping.");
                controller.Stop();
                StopForeground(true);
                StopSelf();
                isStarted = false;
            }

            // This tells Android not to restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
        }

        public void Save()
        {
            File.WriteAllText(_userDataPath, controller.DataSnapshot());
        }
        public override void OnDestroy()
        {
            this.Save();
            controller.Stop();
            base.OnDestroy();
        }
    }

}