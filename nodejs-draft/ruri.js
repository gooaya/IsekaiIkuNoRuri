module.exports={
  account: {
    login(param) {
      return {
        version: "0.7.8",
        result: 0,
        secret: "ffffffffffffffffffffffffffffffff"
      };
    },
    getResVersion() {
      return {
        curResVersion: 15,
        curResPackageUrl: "http://nonostatic.b0.upaiyun.com/assets/"
      };
    }
  },
  user: {
    syncData() {
      const userData = require("./userData");
      return { ...userData, serverTime: Math.round(Date.now() / 1000) };
    },
    checkStatus() {
      return { playerDataDelta: { modified: {}, deleted: {} } };
    },
    getGifts() {
      return {
        giftCnt: 21,
        gifts: [
          {
            giftId: 10238311,
            ifrom: 521,
            from: "Login bonus(daily)",
            item: [],
            createTime: 1506514494,
            status: 0
          }
        ],
        playerDataDelta: { modified: {}, deleted: {} }
      };
    },
    changeNickName() {
      return {};
    }
  },
  story: {
    completeStory({ storykey }) {
      return {
        playerDataDelta: {
          modified: { story: { vars: { [`${storykey}_End`]: 1 } } },
          deleted: {}
        }
      };
    }
  },
  battle: {
    getGlobalBuff() {
      return { buffs: [] };
    },
    submitBattleData() {
      return {
        subTargetList: ["HpGoal", "TimeGoalLong", "HpGoalEx"],
        treasure: [],
        playerDataDelta: {
          modified: {},
          deleted: {}
        }
      };
    },
    beginBattle({ stageId }) {
      return {
        result: 0,
        buffs: null,
        battleInfo: {
          battleId: Date.now(),
          stageDropTable: {
            [stageId]: {
              resource: {},
              treasureCnt: 0
            }
          }
        },
        playerDataDelta: {
          modified: null,
          deleted: null
        }
      };
    }
  }
}
