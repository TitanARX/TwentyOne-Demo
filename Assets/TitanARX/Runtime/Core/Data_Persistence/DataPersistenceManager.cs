using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Debugging")]
        [SerializeField] private bool _initializeDataIfNull = false;

        [Header("File Storage Config")]
        [SerializeField] private string _fileName = "data.sav";
        private FileDataHandler _fileDataHandler;
        public static DataPersistenceManager Instance => _instance;
        private static DataPersistenceManager _instance = null;

        public GameData GameData;

        public List<IDataPersistance> DataPersistanceObjects = new List<IDataPersistance>();
        public bool useEncryption = false;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Awake()
        {
            if(!CreateInstance())
            {
                return;
            }
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, useEncryption);
        }

        private List<IDataPersistance> FindAllDataPersistanceObjects()
        {
            IEnumerable<IDataPersistance> dataPersistanceObjects = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IDataPersistance>();
            return new List<IDataPersistance>(dataPersistanceObjects);
        }

        public bool CreateInstance()
        {
            _instance = _instance != null ? _instance : this;
            if (_instance != this)
            {
                DestroyImmediate(gameObject);
                return false;
            }
            DontDestroyOnLoad(gameObject);
            return true;
        }

        public void NewGame()
        {
            GameData = new GameData();
            Debug.Log("New Game Created");
        }

        public void SaveGame()
        {
            if(GameData == null)
            {
                Debug.Log("No Game Data Found");
                return;
            }

            foreach (IDataPersistance dataPersistanceObj in DataPersistanceObjects)
            {
                dataPersistanceObj.SaveData(GameData);
            }

            _fileDataHandler.Save(GameData);

            Debug.Log("Game Saved");
        }

        public void LoadGame()
        {
            GameData = _fileDataHandler.Load();
           
            if (_initializeDataIfNull && GameData == null)
            {
                Debug.Log("No Data Found");
                NewGame();
            }
  
            if (GameData == null)
            {
                NewGame();
            }

            foreach (IDataPersistance dataPersistanceObject in DataPersistanceObjects)
            {
                dataPersistanceObject.LoadData(GameData);
            }

            Debug.Log("Game Loaded");
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DataPersistanceObjects = FindAllDataPersistanceObjects();
            LoadGame();
        }

        public void OnApplicationQuit()
        {
            SaveGame();
        }
    }
