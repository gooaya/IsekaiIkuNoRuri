using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ruri
{
    public class User
    {
        private readonly Ruri ruri;
        string UserData
        {
            get
            {
                return this.ruri.UserData;
            }
            set
            {
                this.ruri.UserData = value;
            }

        }
        public User(Ruri ruri)
        {
            this.ruri = ruri;
        }
        public string SyncData(string param)
        {
            return this.UserData;
        }
        public string CheckStatus(string param)
        {
            return @"{'playerDataDelta':{'modified':{},'deleted':{}}}".Replace('\'', '"');
        }
        public string GetGifts(string param)
        {
            return @"{""giftCnt"":21,""gifts"":[{""giftId"":10238311,""ifrom"":521,""from"":""Login bonus(daily)"",""item"":[],""createTime"":1506514494,""status"":0}],""playerDataDelta"":{""modified"":{},""deleted"":{}}}";
        }
    };
    public class Account
    {
        private readonly Ruri ruri;
        string UserData
        {
            get
            {
                return this.ruri.UserData;
            }
            set
            {
                this.ruri.UserData = value;
            }

        }
        public Account(Ruri ruri)
        {
            this.ruri = ruri;
        }
        public string Login(string param)
        {
            JObject p = JObject.Parse(param);
            string ver = (string)p["version"];
            return @"{'version':'0.7.8','result':0,'secret':'t1PwOOP5OgilGe4St1PwPedQOgj5OGnL'}".Replace('\'', '"');
        }
        public string GetResVersion(string param)
        {
            return @"{'curResVersion':15,'curResPackageUrl':'http://nonostatic.b0.upaiyun.com/assets/'}".Replace('\'', '"');
        }
    }
    public class Battle
    {
        private readonly Ruri ruri;
        string UserData
        {
            get
            {
                return this.ruri.UserData;
            }
            set
            {
                this.ruri.UserData = value;
            }

        }
        public Battle(Ruri ruri)
        {
            this.ruri = ruri;
        }
        public string GetGlobalBuff(string param)
        {
            return @"{""buffs"":[]}";
        }
        public string BeginBattle(string param)
        {
            JObject p = JObject.Parse(param);
            return "{\"result\":0,\"buffs\":null,\"battleInfo\":{\"battleId\":1546178269550,\"stageDropTable\":{\"" + (int)p["stageId"] + "\":{\"resource\":{},\"treasureCnt\":0}}},\"playerDataDelta\":{\"modified\":null,\"deleted\":null}}";

        }
        public string SubmitBattleData(string param)
        {
            return @"{""subTargetList"":[""HpGoal"",""TimeGoalLong"",""HpGoalEx""],""treasure"":[],""playerDataDelta"":{""modified"":{},""deleted"":{}}}";
        }
    }
    public class Ruri
    {
        public string UserData { get; set; }

        public Ruri(string userData = "{}")
        {
            this.UserData = userData;
            this.User = new User(this);
            this.Account = new Account(this);
            this.Battle = new Battle(this);
        }
        public User User { get; }
        public Account Account { get; }
        public Battle Battle { get; }
    }
}

