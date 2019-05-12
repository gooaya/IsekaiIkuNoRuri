import React from "react";
import axios from "axios";
import { get, set as _ldSet, merge } from "lodash";
import { Layout, Button, Select, Card, Switch, Table, message } from "antd";
const { Content } = Layout;
const { Option } = Select;

function set(obj, path, value) {
  const tmp = _ldSet({}, path, value);
  return merge({}, obj, tmp);
}

const STORY = [
  "LobbyT_EquipWork_End",
  "WarpT_BattleChars_End",
  "M0_T001_S01_End",
  "M0_T002_S01_End",
  "M0_T003_S02_End",
  "M0_T004_S01_End",
  "LobbyT_ToWarp_End",
  "WarpT_WarpSystem_End",
  "M0_T004_S03_End",
  "M1_T001_S01_End",
  "M1_T002_S01_End",
  "LobbyT_CoreLink_End",
  "M1_T002_S02_End",
  "M1_T002_S03_End",
  "M1_T002_S04_End",
  "LobbyT_Mission_End",
  "LobbyT_ChangeEquip_End",
  "LobbyT_Gacha_End",
  "M1_T003_S01_End",
  "M1_T003_S02_End",
  "M1_T003_S03_End",
  "WarpT_Chapter_End",
  "M1_T004_S01_End",
  "LobbyT_Cooking_End",
  "M1_T005_S01_End",
  "M1_T005_S02_End",
  "M1_T005_S03_End",
  "LobbyT_Gift_End",
  "LobbyT_Talent_End",
  "LobbyT_CharRoom_End",
  "M1_T006_S03_End",
  "M1_T006_S04_End",
  "LobbyT_Expedition_End",
  "M1_T007_S02_End",
  "M1_T007_S03_End",
  "M1_T007_S04_End",
  "LobbyT_Exploration_End",
  "LobbyT_ExpeditionMap_End",
  "LobbyT_Recipe_End",
  "M1_T008_S01_End",
  "M1_T008_S02_End",
  "M1_T008_S03_End",
  "LobbyT_TalentMap_End",
  "M1_T009_S01_End",
  "M1_T009_S02_End",
  "M1_T009_S03_End",
  "M2_T001_S01_End",
  "WarpT_SpecialStage_End",
  "M2_T001_S02_End",
  "M2_T001_S03_End",
  "M2_T001_S04_End",
  "M2_T002_S01_End",
  "M2_T002_S02_End",
  "M2_T002_S03_End",
  "M2_T002_S04_End",
  "M2_T002_S05_End",
  "M2_T002_S06_End",
  "Event_BeachTrip_T001_End",
  "Event_BeachTrip_T002_End",
  "LobbyT_CharRoom_Eat_End",
  "Event_BeachTrip_T003_End",
  "Event_BeachTrip_T004_End",
  "LobbyT_EquipEvolve_End",
  "M2_T003_S01_End",
  "M2_T003_S02_End",
  "M2_T003_S03_End",
  "M2_T003_S04_End",
  "M2_T004_S01_End",
  "M2_T004_S02_End",
  "M2_T004_S03_End",
  "M2_T004_S04_End",
  "M2_T004_S05_End",
  "M2_T005_S01_End",
  "M2_T005_S02_End",
  "M3_UpdateNotice_End",
  "M2_Side1_T001_S01_End",
  "M2_Side1_T001_S03_End",
  "M2_Side1_T001_S04_End",
  "M2_Side1_T002_S01_End",
  "M2_Side1_T003_S01_End",
  "M2_Side1_T003_S02_End",
  "M2_Side1_T004_S02_End",
  "M2_Side1_T004_S03_End",
  "M2_Side1_T005_S01_End",
  "LobbyT_TalentSingle_End",
  "LobbyT_TalentDouble_End",
  "M2_Side2_T001_S01_End",
  "M2_Side2_T002_S01_End",
  "M2_Side2_T003_S01_End",
  "M2_Side2_T004_S01_End",
  "Event_ChristmasEnd_T001_End",
  "M2_Side2_T005_S01_End"
];

const BGM = [
  "BGM_Win",
  "BGM_Warp02",
  "BGM_Warp01",
  "BGM_Title",
  "BGM_RoyalCity",
  "BGM_NonoFailed",
  "BGM_Mood_UnknownWorld2Harp",
  "BGM_Mood_Suspense5",
  "BGM_Mood_Suspense3",
  "BGM_Mood_Suspense2",
  "BGM_Mood_Suspense",
  "BGM_Mood_Scary03",
  "BGM_Mood_Sakuya02",
  "BGM_Mood_Refreshing",
  "BGM_Mood_NoWay2",
  "BGM_Mood_Memory03",
  "BGM_Mood_Memory02",
  "BGM_Mood_Memory01",
  "BGM_Mood_Melancholy2",
  "BGM_Mood_LostPlace04",
  "BGM_Mood_LetsParty",
  "BGM_Mood_Investigation02",
  "BGM_Mood_Investigation01",
  "BGM_Mood_Holiday01",
  "BGM_Mood_Guild01",
  "BGM_Mood_Fear01",
  "BGM_Mood_Deep_Woods2",
  "BGM_Mood_Beach",
  "BGM_Gacha",
  "BGM_Field_WorldTree02_Loop",
  "BGM_Field_WorldTree02_Intro",
  "BGM_Field_WorldTree01",
  "BGM_Field_SnowFactory",
  "BGM_Field_FinalWave02",
  "BGM_Field_FinalWave01",
  "BGM_Field_ElfGarden01",
  "BGM_Event_NightKingdomAnchient",
  "BGM_Cooking",
  "BGM_City_Geisterwald",
  "BGM_City_ElfVillage",
  "BGM_Char_Yurika",
  "BGM_Char_Ruri",
  "BGM_Char_Miyu",
  "BGM_Char_Kuroeru",
  "BGM_Boss03_Loop",
  "BGM_Boss03_Intro",
  "BGM_Boss02",
  "BGM_Boss01",
  "BGM_Battle_Rush01_Loop",
  "BGM_Battle_Rush01_Intro",
  "BGM_Base01"
];

const columns = [
  {
    key: "label",
    title: "Story",
    dataIndex: "key"
  },
  {
    key: "ended",
    title: "Ended",
    dataIndex: "ended",
    render: value => <Switch checked={Boolean(value)} />
  }
];

class Editor extends React.Component {
  handleBgmChange = value => {
    this.props.onChange(
      set(this.props.userData, "userData.story.strs.BGM_Base_Loop", value)
    );
  };
  handleResetStory = (key, ended) => {
    this.props.onChange(
      set(this.props.userData, `userData.story.vars.${key}`, ended)
    );
  };
  render() {
    const {
      userData: { userData }
    } = this.props;
    const story = get(userData, "story.vars");

    return (
      <>
        <Card title="BGM">
          <Select
            value={get(userData, "story.strs.BGM_Base_Loop")}
            style={{ width: "100%" }}
            onChange={this.handleBgmChange}
          >
            {BGM.map(key => (
              <Option key={key} value={key}>
                {key}
              </Option>
            ))}
          </Select>
        </Card>
        <Card title="Story">
          <Table
            columns={columns}
            dataSource={STORY.map(key => ({ key, ended: story[key] }))}
            onRow={({ key, ended }) => ({
              onClick: event => this.handleResetStory(key, ended ^ 1)
            })}
          />
        </Card>
      </>
    );
  }
}

class App extends React.Component {
  state = {
    userDate: null,
    isSaving: false,
    savedUserData: null
  };
  handleChange = userData => {
    this.setState({ userData });
  };
  loadUserData = async () => {
    const userData = (await axios.get("/IsekaiIkuNoRuri/userData")).data;
    this.setState({ userData, savedUserData: userData });
  };
  saveUserData = async () => {
    try {
      const { userData } = this.state;
      this.setState({ isSaving: true });
      await axios.post("/IsekaiIkuNoRuri/userData", userData);
      this.setState({ isSaving: false, savedUserData: userData });
    } catch (e) {
      message.error(
        decodeURIComponent(
          "%E6%9C%89%E4%BB%80%E4%B9%88%E4%B8%9C%E8%A5%BF%E7%82%B8%E4%BA%86%EF%BC%8C%E6%88%91%E5%8F%AA%E7%9F%A5%E9%81%93%E8%BF%99%E4%B9%88%E5%A4%9A%E3%80%82"
        )
      );
      this.setState({ isSaving: false });
    }
  };
  componentDidMount() {
    this.loadUserData();
  }
  render() {
    const { userData, savedUserData, isLoading } = this.state;
    return (
      <Layout className="layout">
        <Content style={{ padding: "0 0px" }}>
          <div style={{ background: "#fff", padding: 24, minHeight: 280 }}>
            {userData ? (
              <>
                <Button
                  loading={isLoading}
                  type="primary"
                  style={{ width: "100%" }}
                  onClick={this.saveUserData}
                  disabled={userData === savedUserData}
                >
                  Save
                </Button>
                <Editor userData={userData} onChange={this.handleChange} />
                <Button
                  loading={isLoading}
                  type="primary"
                  style={{ width: "100%" }}
                  onClick={this.saveUserData}
                  disabled={userData === savedUserData}
                >
                  Save
                </Button>{" "}
              </>
            ) : (
              <div>Loading...</div>
            )}
          </div>
        </Content>
      </Layout>
    );
  }
}

export default App;
