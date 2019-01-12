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
        public string DataSnapshot()
        {
            return this.ruri?.DataSnapshot();
        }
        private void OnRequest(Session oS)
        {
            FiddlerApplication.Log.LogFormat("{0} {1}", oS.RequestMethod, oS.fullUrl);
            if(oS.fullUrl== "http://ad2.nono.nyanbox.com:7080/static/server-list.json")
            {
                oS.utilCreateResponseAndBypassServer();
                oS.oResponse.headers.SetStatus(200, "Ok");
                oS.oResponse["Content-Type"] = "application/json; charset=UTF-8";
                oS.utilSetResponseBody(@"[{""id"":""397"",""domain"":""androidprod.nono.nyanbox.com"",""name"":""test"",""utcOffset"":28800,""downloadLink"":""http://localhost/nono/nono_60112.apk"",""curVersion"":""0.7.8"",""minVersion"":""0.7.8"",""noticeUrl"":""http://notice.nyanbox.com:3300/"",""userTermsUrl"":""http://notice.nyanbox.com:3300/""}]");
                return;
            }
            if (oS.fullUrl.StartsWith("http://as1.nono.nyanbox.com:8089/u8server/user/getToken"))
            {
                oS.utilCreateResponseAndBypassServer();
                oS.oResponse.headers.SetStatus(200, "Ok");
                oS.oResponse["Content-Type"] = "text/json; charset=UTF-8";
                oS.utilSetResponseBody(@"{""data"":{""extension"":"""",""platID"":0,""sdkUserName"":""1000000000000"",""userID"":100000,""sdkUserID"":""100000000"",""username"":""1000000000000.isekai"",""token"":""ffffffffffffffffffffffffffffffff"",""timestamp"":""1546185625589""},""state"":1}");
                return;
            }
            if (oS.fullUrl.StartsWith("http://trace2144.2144.cn/__beacon_sdk.gif"))
            {
                oS.utilCreateResponseAndBypassServer();
                oS.oResponse.headers.SetStatus(200, "Ok");
                oS.oResponse["Content-Type"] = "image/gif";
                return;
            }
            if (oS.fullUrl.StartsWith("http://mapi.2144.cn/hook/start"))
            {
                oS.utilCreateResponseAndBypassServer();
                oS.oResponse.headers.SetStatus(200, "Ok");
                oS.oResponse["Content-Type"] = "application/json; charset=utf-8";
                oS.utilSetResponseBody(@"{""ali_pay"":1,""weixin_pay"":1,""sdk_status"":0,""ball"":0,""welfare"":0,""game_id"":10012,""slug"":""nono"",""time"":1545898091,""mobile_find_password"":""0"",""email_find_password"":""0"",""contact_customer_servicer"":""000000000"",""customer_tel"":""0000-0000-00"",""customer_time"":""7*24小时"",""weixin_login"":0,""welfare_category_id"":4,""welfare_type_id"":1,""anti_addiction"":""1"",""ios_pay_third"":""0"",""user_center_default"":""3.0.0"",""yibao_pay"":1,""yibao_pay_card"":1,""guest_recharge"":0,""game_audit_version"":"""",""h5_weixin_pay"":1,""h5_ali_pay"":1,""h5_yibao_pay"":1,""h5_wechat_pay"":0,""i_p_url"":""https://tieba.baidu.com/f?kw=%E8%AF%BA%E8%AF%BA%E6%9D%A5%E8%87%AA%E5%BC%82%E4%B8%96%E7%95%8C&ie=utf-8"",""reyun_filter_slug"":""dhmy,dtxxz,tlz,ylws"",""real_auth"":1,""toutiao_android_app_id"":0,""social_data_version"":""sdk"",""status"":""success""}");
                return;
            }
            if (oS.fullUrl == "http://cpa.mapi.2144.cn/v1/receive/info_login")
            {
                oS.utilCreateResponseAndBypassServer();
                oS.oResponse.headers.SetStatus(200, "Ok");
                oS.oResponse["Content-Type"] = "application/json; charset=utf-8";
                oS.utilSetResponseBody(@"{""status"":""success""}");
                return;
            }
            if (oS.fullUrl == "http://cpa.mapi.2144.cn/v1/receive/first_start")
            {
                oS.utilCreateResponseAndBypassServer();
                oS.oResponse.headers.SetStatus(200, "Ok");
                oS.oResponse["Content-Type"] = "application/x-www-form-urlencoded; charset=utf-8";
                return;
            }
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