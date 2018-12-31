using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

using Fiddler;
using Ruri;
using Newtonsoft.Json;
using Android.Content.Res;

namespace FiddlerCore.NetCore
{
    public class ProxyController
    {
        ushort iPort = 8080;
        FiddlerCoreStartupSettings startupSettings;
        internal string userData;
        Ruri.Ruri ruri;

        public ProxyController()
        {
            startupSettings =
                new FiddlerCoreStartupSettingsBuilder()
                    .ListenOnPort(iPort)
                    //.RegisterAsSystemProxy()
                    //.DecryptSSL()
                    //.AllowRemoteClients()
                    //.ChainToUpstreamGateway()
                    //.MonitorAllConnections()
                    //.HookUsingPACFile()
                    //.CaptureLocalhostTraffic()
                    //.CaptureFTP()
                    .OptimizeThreadPool()
                    //.SetUpstreamGatewayTo("http=CorpProxy:80;https=SecureProxy:443;ftp=ftpGW:20")
                    .Build();

        }
        public bool Inited
        {
            get
            {
                return ruri != null;
            }
        }
        public void Init(string userData)
        {
            ruri = new Ruri.Ruri(userData);
        }
        private void OnRequest(Session oS)
        {
            FiddlerApplication.Log.LogFormat("{0} {1}", oS.RequestMethod, oS.fullUrl);
            if (oS.host == "androidprod.nono.nyanbox.com:8082")
            {
                try
                {
                    string response = null;
                    var paths = oS.PathAndQuery.Split('/');
                    var className = paths[1];
                    var methodName = paths[2];
                    var param = System.Text.Encoding.UTF8.GetString(oS.RequestBody);
                    if (className == "user")
                    {
                        if (methodName == "syncData")
                            response = ruri.User.SyncData(param);
                        if (methodName == "checkStatus")
                            response = ruri.User.CheckStatus(param);
                        if (methodName == "getGifts")
                            response = ruri.User.GetGifts(param);
                    }
                    if (className == "account")
                    {
                        if (methodName == "login")
                            response = ruri.Account.Login(param);
                        if (methodName == "getResVersion")
                            response = ruri.Account.GetResVersion(param);

                    }
                    if (className == "battle")
                    {
                        if (methodName == "beginBattle")
                            response = ruri.Battle.BeginBattle(param);
                        if (methodName == "submitBattleData")
                            response = ruri.Battle.SubmitBattleData(param);
                        if (methodName == "getGlobalBuff")
                            response = ruri.Battle.GetGlobalBuff(param);

                    }
                    oS.utilCreateResponseAndBypassServer();
                    if (response != null)
                    {
                        oS.oResponse.headers.SetStatus(200, "Ok");
                        oS.oResponse["Content-Type"] = "application/json; charset=UTF-8";
                        oS.utilSetResponseBody(response);
                    }
                    else
                    {
                        oS.oResponse.headers.SetStatus(404, "Not Found.");
                    }
                }
                catch (Exception err)
                {
                    FiddlerApplication.Log.LogString(err.ToString());
                }
            }
        }

        public void StartProxy()
        {
            FiddlerApplication.BeforeRequest += OnRequest;
            FiddlerApplication.Startup(startupSettings);
            FiddlerApplication.Log.LogFormat("Created endpoint listening on port {0}", iPort);
        }
        public void Stop()
        {
            FiddlerApplication.Shutdown();
            FiddlerApplication.BeforeRequest -= OnRequest;
        }
    }
}