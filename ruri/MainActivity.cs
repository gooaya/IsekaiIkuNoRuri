using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using FiddlerCore.NetCore;
using Android.Content;
using System.IO;
using Android.Util;
using System;

namespace ruri
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ProxyServiceConnection serviceConnection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            FindViewById<Switch>(Resource.Id.switchBeacon).CheckedChange += (o, e) =>
            {
                if (e.IsChecked)
                    this.serviceConnection?.Start();
                else
                    this.serviceConnection?.Stop();
            };
        }
        protected override void OnStart()
        {
            base.OnStart();
            if (serviceConnection == null)
            {
                this.serviceConnection = new ProxyServiceConnection(this);
            }
            Intent serviceToStart = new Intent(this, typeof(ProxyService));
            BindService(serviceToStart, this.serviceConnection, Bind.AutoCreate);
        }
        protected override void OnStop()
        {
            base.OnStop();
            this.serviceConnection?.Save();
        }
    }

    [Service]
    [IntentFilter(new String[] { "com.xamarin.ProxyService" })]
    public class ProxyService : Service
    {
        string _userDataPath;

        static readonly ProxyController controller = new ProxyController();
        static readonly string TAG = typeof(ProxyService).FullName;
        public IBinder Binder { get; private set; }

        public override void OnCreate()
        {
            // This method is optional to implement
            base.OnCreate();
            Log.Debug(TAG, "OnCreate");

            if (!controller.Inited)
            {
                _userDataPath = Path.Combine(ApplicationContext.GetExternalFilesDir(null).Path, "userData-0.7.8.json");
                if (!File.Exists(_userDataPath))
                {
                    using (StreamReader sr = new StreamReader(this.Assets.Open("userData.json")))
                    {
                        // File.WriteAllText(_userDataPath, @"{""userData"":" + sr.ReadToEnd() + @",""serverData"":{},""version"":""0.0.2""}");
                        controller.Init(@"{""userData"":" + sr.ReadToEnd() + @",""serverData"":{},""version"":""0.0.2""}");
                    }
                }
                else
                {
                    string userData = File.ReadAllText(_userDataPath);
                    controller.Init(userData);
                }
            }
        }

        // Magical code that makes the service do wonderful things.
        public override IBinder OnBind(Intent intent)
        {
            // This method must always be implemented
            Log.Debug(TAG, "OnBind");
            this.Binder = new ProxyBinder(this);
            return this.Binder;
        }

        public void Start()
        {
            controller.StartProxy();
        }

        public void Stop()
        {
            controller.Stop();
        }

        public void Save()
        {
            File.WriteAllText(_userDataPath, controller.DataSnapshot());
        }
    }

    public class ProxyBinder : Binder
    {
        ProxyService service;
        public ProxyBinder(ProxyService service)
        {
            this.service = service;
        }

        public void Start()
        {
            service?.Start();
        }
        public void Stop()
        {
            service?.Stop();
        }

        internal void Save()
        {
            service?.Save();
        }
    }

    public class ProxyServiceConnection : Java.Lang.Object, IServiceConnection
    {
        static readonly string TAG = typeof(ProxyServiceConnection).FullName;

        MainActivity mainActivity;
        public ProxyServiceConnection(MainActivity activity)
        {
            IsConnected = false;
            Binder = null;
            mainActivity = activity;
        }

        public bool IsConnected { get; private set; }
        public ProxyBinder Binder { get; private set; }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            Binder = service as ProxyBinder;
            IsConnected = this.Binder != null;

            string message = "onServiceConnected - ";
            Log.Debug(TAG, $"OnServiceConnected {name.ClassName}");

            if (IsConnected)
            {
                message = message + " bound to service " + name.ClassName;
                // mainActivity.UpdateUiForBoundService();
            }
            else
            {
                message = message + " not bound to service " + name.ClassName;
                // mainActivity.UpdateUiForUnboundService();
            }

            Log.Info(TAG, message);
            // mainActivity.ProxyMessageTextView.Text = message;

        }

        public void OnServiceDisconnected(ComponentName name)
        {
            Log.Debug(TAG, $"OnServiceDisconnected {name.ClassName}");
            IsConnected = false;
            Binder = null;
            // mainActivity.UpdateUiForUnboundService();
        }

        public void Start()
        {
            if (IsConnected)
            {
                Binder?.Start();
            }
        }

        public void Stop()
        {
            if (IsConnected)
            {
                Binder?.Stop();
            }
        }

        public void Save()
        {
            if (IsConnected)
            {
                Binder.Save();
            }
        }
    }
}