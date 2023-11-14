using System;
using System.IO;
using UnityEngine;

    public class FileDataHandler
    {
        private string _dataPath = "";
        private string _dataFileName ="";
        private bool _useEncryption = false;
        private readonly string _enctyptionCodeWord = "kitty";

        public FileDataHandler (string dataPath, string dataFileName, bool useEncryption)
        {
            _dataPath = dataPath;
            _dataFileName = dataFileName;
            _useEncryption = useEncryption;
        }

        private string EncryptDecryptData(string dataToEncrypt)
        {
            string encryptedData = "";
            for (int i = 0; i < dataToEncrypt.Length; i++)
            {
                int charValue = Convert.ToInt32(dataToEncrypt[i]);
                charValue += Convert.ToInt32(_enctyptionCodeWord[i % _enctyptionCodeWord.Length]);
                encryptedData += char.ConvertFromUtf32(charValue);
            }
            return encryptedData;
        }

        public GameData Load(DataPersistanceType dataPersistanceType = DataPersistanceType.JSON) 
        {
            string fullPath = Path.Combine(_dataPath, _dataFileName);
            GameData loadedData = null;
            if(File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    loadedData = DeseralizeJsonToData(dataToLoad);

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($" Error occured when trying to load data from file: {fullPath}, \n {e}");
                }               
            }
            return loadedData;
        }

        public void Save(GameData data, DataPersistanceType dataPersistanceType = DataPersistanceType.JSON )
        {
            string fullPath = Path.Combine(_dataPath, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(data, true);

                dataToStore = SeralizeDataToJSON(data);

                if (_useEncryption)
                {
                    dataToStore = EncryptDecryptData(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($" Error occured when trying to save data to file: {fullPath}, \n {e}");
            }
        }

        private string SeralizeDataToJSON(GameData data) 
        {
            return JsonUtility.ToJson(data, true);
        }
        private GameData DeseralizeJsonToData(string dataToLoad)
        {
            return JsonUtility.FromJson<GameData>(dataToLoad);
        }
    }

    public enum DataPersistanceType
    {
        JSON,
        Binary,
        PlainText,
        PlayerPrefs
    }
