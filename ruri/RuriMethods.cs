using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ruri
{
    public class User
    {
        private readonly Ruri ruri;
        JObject UserData
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
            var res = new JObject();
            res["user"] = this.UserData;
            res["serverTime"] = 1545752883;
            string aa = res.ToString(Formatting.None);
            return res.ToString(Formatting.None);
        }
        public string CheckStatus(string param)
        {
            return @"{'playerDataDelta':{'modified':{},'deleted':{}}}".Replace('\'', '"');
        }
        public string GetGifts(string param)
        {
            return @"{""giftCnt"":0,""gifts"":[],""playerDataDelta"":{""modified"":{},""deleted"":{}}}";
        }
    };
    public class Account
    {
        private readonly Ruri ruri;
        JObject UserData
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
        JObject UserData
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

    public class Story
    {
        private readonly Ruri ruri;
        JObject UserData
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
        public Story(Ruri ruri)
        {
            this.ruri = ruri;
        }
        public string CompleteStory(string param)
        {
            JObject p = JObject.Parse(param);
            string key = (string)p["storykey"] + "_End";
            this.UserData["story"]["vars"][key] = 1;
            return @"{""playerDataDelta"":{""modified"":{""story"":{""vars"":{""" + key + @""":1}}},""deleted"":{}}}";
        }
    }

    public class Character
    {
        private readonly Ruri ruri;
        JObject UserData
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
        public Character(Ruri ruri)
        {
            this.ruri = ruri;
        }
        public string LoadWeapon(string param)
        {
            JObject p = JObject.Parse(param);
            var targetCharacterId = (int)p["targetCharacterId"];
            var targetWeaponId = (int)p["targetWeaponId"];
            var oldWeaponId = (int)this.UserData["character"]["characters"][targetCharacterId.ToString()]["inventoryWeaponId"];
            this.UserData["equipInventory"]["weapons"][oldWeaponId.ToString()]["loadedCharacterId"] = -1;
            this.UserData["character"]["characters"][targetCharacterId.ToString()]["inventoryWeaponId"] = targetWeaponId;
            this.UserData["equipInventory"]["weapons"][targetWeaponId.ToString()]["loadedCharacterId"] = targetCharacterId;

            var res = JObject.Parse(@"{""playerDataDelta"":{""modified"":{""character"":{""charaters"":{}},""equipInventory"":{""weapons"":{}}},""deleted"":null}}");
            res["playerDataDelta"]["modified"]["character"]["charaters"][targetCharacterId.ToString()] = this.UserData["character"]["characters"][targetCharacterId.ToString()];
            res["playerDataDelta"]["modified"]["equipInventory"]["weapons"][targetWeaponId.ToString()] = this.UserData["equipInventory"]["weapons"][targetWeaponId.ToString()];
            res["playerDataDelta"]["modified"]["equipInventory"]["weapons"][oldWeaponId.ToString()] = this.UserData["equipInventory"]["weapons"][oldWeaponId.ToString()];
            return res.ToString(Formatting.Indented);
        }
        public string LoadEquip(string param)
        {
            JObject p = JObject.Parse(param);
            var targetCharacterId = (int)p["targetCharacterId"];
            var targetEquipId = (int)p["targetEquipId"];
            var oldEquipId = (int)this.UserData["character"]["characters"][targetCharacterId.ToString()]["inventoryEquipId"];
            this.UserData["equipInventory"]["equips"][oldEquipId.ToString()]["loadedCharacterId"] = -1;
            this.UserData["character"]["characters"][targetCharacterId.ToString()]["inventoryEquipId"] = targetEquipId;
            this.UserData["equipInventory"]["equips"][targetEquipId.ToString()]["loadedCharacterId"] = targetCharacterId;

            var res = JObject.Parse(@"{""playerDataDelta"":{""modified"":{""character"":{""charaters"":{}},""equipInventory"":{""equips"":{}}},""deleted"":null}}");
            res["playerDataDelta"]["modified"]["character"]["charaters"][targetCharacterId.ToString()] = this.UserData["character"]["characters"][targetCharacterId.ToString()];
            res["playerDataDelta"]["modified"]["equipInventory"]["equips"][targetEquipId.ToString()] = this.UserData["equipInventory"]["equips"][targetEquipId.ToString()];
            res["playerDataDelta"]["modified"]["equipInventory"]["equips"][oldEquipId.ToString()] = this.UserData["equipInventory"]["equips"][oldEquipId.ToString()];
            return res.ToString(Formatting.Indented);
        }
        public string LoadBracer(string param)
        {
            JObject p = JObject.Parse(param);
            var targetCharacterId = (int)p["targetCharacterId"];
            var targetBracerId = (int)p["targetBracerId"];
            var oldBracerId = (int)this.UserData["character"]["characters"][targetCharacterId.ToString()]["inventoryBracerI"];
            this.UserData["equipInventory"]["bracers"][oldBracerId.ToString()]["loadedCharacterId"] = -1;
            this.UserData["character"]["characters"][targetCharacterId.ToString()]["inventoryBracerId"] = targetBracerId;
            this.UserData["equipInventory"]["bracers"][targetBracerId.ToString()]["loadedCharacterId"] = targetCharacterId;

            var res = JObject.Parse(@"{""playerDataDelta"":{""modified"":{""character"":{""charaters"":{}},""equipInventory"":{""bracers"":{}}},""deleted"":null}}");
            res["playerDataDelta"]["modified"]["character"]["charaters"][targetCharacterId.ToString()] = this.UserData["character"]["characters"][targetCharacterId.ToString()];
            res["playerDataDelta"]["modified"]["equipInventory"]["bracers"][targetBracerId.ToString()] = this.UserData["equipInventory"]["bracers"][targetBracerId.ToString()];
            res["playerDataDelta"]["modified"]["equipInventory"]["bracers"][oldBracerId.ToString()] = this.UserData["equipInventory"]["bracers"][oldBracerId.ToString()];
            return res.ToString(Formatting.Indented);
        }
    }

    public class Food
    {
        private readonly Ruri ruri;
        JObject UserData
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
        public Food(Ruri ruri)
        {
            this.ruri = ruri;
        }
        public string EatFood(string param)
        {
            return @"{""isFull"":0,""expired"":0,""playerDataDelta"":{""modified"":null,""deleted"":null}}";
        }
    }

    public class Present
    {
        private readonly Ruri ruri;
        JObject UserData
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
        public Present(Ruri ruri)
        {
            this.ruri = ruri;
        }
        public string GivePresentItem(string param)
        {
            return @"{""isFull"":0,""expired"":0,""playerDataDelta"":{""modified"":null,""deleted"":null}}";
        }
    }


    public class Ruri
    {
        string packageVersion;
        public JObject UserData { get; set; }
        public JObject ServerData { get; set; }

        public Ruri(string data, string packageVersion)
        {
            this.packageVersion = packageVersion;
            JObject json = JObject.Parse(data);

            this.UserData = (JObject)json["userData"];
            this.ServerData = (JObject)json["serverData"];
            this.User = new User(this);
            this.Account = new Account(this);
            this.Battle = new Battle(this);
            this.Story = new Story(this);
            this.Character = new Character(this);
            this.Food = new Food(this);
            this.Present = new Present(this);

        }
        public string DataSnapshot()
        {
            JObject data = new JObject();
            data["userData"] = this.UserData;
            data["serverData"] = this.ServerData;
            data["version"] = this.packageVersion;
            return data.ToString(Formatting.Indented);
        }
        public User User { get; }
        public Account Account { get; }
        public Battle Battle { get; }
        public Story Story { get; }
        public Character Character { get; }
        public Food Food { get; }
        public Present Present { get; }

    }
}

