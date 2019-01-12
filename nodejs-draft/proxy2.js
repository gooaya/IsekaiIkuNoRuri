// var Promise = require('bluebird');
// var mongodb = require('mongodb');

var url = require("url");

var httpProxy = require("http-proxy");
var proxy = httpProxy.createProxyServer();
var http = require("http");
// var zlib = require("zlib");
// var fs = require("fs");

function run() {
  http
    .createServer(async function(req, res) {
      /* target NOT forward
   *  target === final server to receive request
   *  forward === another proxy server to pass request through
   */
      console.log(req.method, req.url);

      if (
        req.url === "http://ad2.nono.nyanbox.com:7080/static/server-list.json"
      ) {
        res.writeHead(200, {
          "Content-Type": "application/json; charset=utf-8"
        });
        res.write(
          JSON.stringify([
            {
              id: "397",
              domain: "androidprod.nono.nyanbox.com",
              name: "test",
              utcOffset: 28800,
              downloadLink: "http://down.2144gy.com/nono/nono_60112.apk",
              curVersion: "0.7.8",
              minVersion: "0.7.8",
              noticeUrl: "http://notice.nyanbox.com:3300/",
              userTermsUrl: "http://notice.nyanbox.com:3300/"
            }
          ])
        );
        res.end();
        return;
      }
      if (
        req.url.startsWith(
          "http://as1.nono.nyanbox.com:8089/u8server/user/getToken"
        )
      ) {
        res.writeHead(200, { "Content-Type": "text/json; charset=utf-8" });
        res.write(
          JSON.stringify({
            data: {
              extension: "",
              platID: 0,
              sdkUserName: "1000000000000",
              userID: 100000,
              sdkUserID: "100000000",
              username: "1000000000000.isekai",
              token: "ffffffffffffffffffffffffffffffff",
              timestamp: `${Date.now()}`
            },
            state: 1
          })
        );
        res.end();
        return;
      }
      if (req.headers.host.includes("2144")) {
        if(await h2144(req, res)){
          return;
        };
      }

      if (req.headers.host === "androidprod.nono.nyanbox.com:8082") {
        await hnono(req, res);
        return;
      }

      var options = {
        target: `http://${req.headers.host}`
      };
      proxy.web(req, res, options); // errorCallback is optional
    })
    .listen(8080);

  // httpProxy.createProxyServer({target:'http://www.baidu.com'}).listen(8080); //
}

proxy.on("error", function(e) {
  console.error(e);
});

async function h2144(req, res) {
  if (req.url.startsWith("http://trace2144.2144.cn/__beacon_sdk.gif")) {
    res.writeHead(200, { "Content-Type": "image/gif" });
    res.end();
    return true;
  }
  /*
  var options = {
    target: `http://${req.headers.host}`
  };
  proxy.web(req, res, options); // errorCallback is optional
  return
*/
  if (req.url.startsWith("http://mapi.2144.cn/hook/start")) {
    res.writeHead(200, { "Content-Type": "application/json; charset=utf-8" });
    res.write(
      JSON.stringify({
        ali_pay: 1,
        weixin_pay: 1,
        sdk_status: 0,
        ball: 0,
        welfare: 0,
        game_id: 10012,
        slug: "nono",
        time: 1545898091.0,
        mobile_find_password: "0",
        email_find_password: "0",
        contact_customer_servicer: "800050781",
        customer_tel: "4008-2144-26",
        customer_time: "7*24Сʱ",
        weixin_login: 0,
        welfare_category_id: 4,
        welfare_type_id: 1,
        anti_addiction: "1",
        ios_pay_third: "0",
        user_center_default: "3.0.0",
        yibao_pay: 1,
        yibao_pay_card: 1,
        guest_recharge: 0,
        game_audit_version: "",
        h5_weixin_pay: 1,
        h5_ali_pay: 1,
        h5_yibao_pay: 1,
        h5_wechat_pay: 0,
        i_p_url: "https://mpay.2144.cn/v2/h5/ios",
        reyun_filter_slug: "dhmy,dtxxz,tlz,ylws",
        real_auth: 1,
        toutiao_android_app_id: 0,
        social_data_version: "sdk",
        status: "success"
      })
    );
    res.end();
  }
  /*
  // WARNING: this will override you local account info and you will lost your 2144 account if it's not binded
  if (req.url === "http://mapi.2144.cn/user/autologin") {
    res.writeHead(200, { "Content-Type": "application/json; charset=utf-8" });
    res.write(
      `{"status":"success","uid":100000000,"username":"ruri","usertoken":"ffffffffffffffffffffffffffffffff","usertype":4,"framework":0,"loginauth":"ffffffffffffffffffffffffffffffff"}`
    );
    res.end();
    return true;
  }
  */
  if (req.url === "http://cpa.mapi.2144.cn/v1/receive/info_login") {
    res.writeHead(200, { "Content-Type": "application/json; charset=utf-8" });
    res.write(
      `{"status":"success"}`
    );
    res.end();
    return true;
  }
  if (req.url === "http://cpa.mapi.2144.cn/v1/receive/first_start") {
    res.writeHead(200, {
      "Content-Type": "application/x-www-form-urlencoded; charset=utf-8"
    });
    // res.write("");
    res.end();
    return true;
  }
  return false;
}

async function hnono(req, res) {
  const host = req.headers.host;
  const method = req.method;
  let body = "";
  if (req.method === "POST") {
    await new Promise((resolve, reject) => {
      req.on("data", chunk => {
        body += chunk.toString(); // convert Buffer to string
      });
      req.on("end", () => {
        req.body = body;
        resolve();
      });
      req.on("error", err => {
        console.log(err);
        reject(err);
      });
    });
  }

  const { path } = url.parse(req.url);
  const [_, c, m] = path.split("/");

  const handler = (routes[c] || {})[m];
  console.log(req.url);
  console.log(body);

  if (!handler) {
    const e = new Error(`${c}.${m} not found`);
    console.error(e);
    throw e;
  }
  res.writeHead(200, { "Content-Type": "application/json; charset=utf-8" });
  const resBody = handler(JSON.parse(body));
  res.write(JSON.stringify(resBody));
  res.end();
}

const routes = require('./ruri');

//http://androidprod.nono.nyanbox.com:8082/account/login

var ls = Object.keys;
run();
