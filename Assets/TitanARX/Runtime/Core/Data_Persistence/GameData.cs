
    public class GameData
    {
        public SerializableDictionary<string, PlayerData> PlayerProfiles = new SerializableDictionary<string, PlayerData>();
        public SettingsData PlayerSettings = new SettingsData();

        public GameData()
        {
            PlayerProfiles.Clear();
        }
    }
