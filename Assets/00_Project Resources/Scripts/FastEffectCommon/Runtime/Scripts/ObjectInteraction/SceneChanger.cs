using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.FastEffect.ObjectInteraction
{
    public class SceneChanger : MonoBehaviour
    {
        public string m_defaultSceneToLoad = "";
        private string m_loadingScene = "";
        private string m_unloadingScene = "";
        private bool m_sceneLoaded = false;

        public void LoadScene()
        {
            Debug.LogFormat("Loading scene [{0}]", m_defaultSceneToLoad);
            SceneManager.LoadScene(m_defaultSceneToLoad);
        }
        public void LoadScene(string sceneToLoad)
        {
            Debug.LogFormat("Loading scene [{0}]", sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }

        public void LoadSceneAdditive()
        {
            if(string.IsNullOrWhiteSpace(m_loadingScene) && !m_sceneLoaded) 
            {
                Debug.LogFormat("Loading scene [{0}] additively", m_defaultSceneToLoad);
                SceneManager.LoadScene(m_defaultSceneToLoad,LoadSceneMode.Additive);
                m_loadingScene = m_defaultSceneToLoad;
                SceneManager.sceneLoaded += this.OnLoadedScene;
            }
        }

        public void LoadSceneAdditive(string sceneToLoad)
        {
            if(string.IsNullOrWhiteSpace(m_loadingScene) && !m_sceneLoaded) 
            {
                Debug.LogFormat("Loading scene [{0}] additively", sceneToLoad);
                SceneManager.LoadScene(sceneToLoad,LoadSceneMode.Additive);
                m_loadingScene = sceneToLoad;
                SceneManager.sceneLoaded += this.OnLoadedScene;
            }
        }

        public void OnLoadedScene(Scene loadedScene, LoadSceneMode mode)
        {
            if(loadedScene.name == m_loadingScene)
            {
                m_sceneLoaded = true;
                m_loadingScene = "";
                SceneManager.sceneLoaded -= this.OnLoadedScene;
            }
        }

        public void UnloadScene()
        {
            if(string.IsNullOrWhiteSpace(m_unloadingScene) && m_sceneLoaded)
            {
                Debug.LogFormat("Unloading scene [{0}]", m_defaultSceneToLoad);
                SceneManager.UnloadSceneAsync(m_defaultSceneToLoad);
                m_unloadingScene = m_defaultSceneToLoad;
                SceneManager.sceneUnloaded += this.OnUnloadedScene;
            }
        }

        public void UnloadScene(string scene)
        {
            if(string.IsNullOrWhiteSpace(m_unloadingScene) && m_sceneLoaded)
            {
                Debug.LogFormat("Unloading scene [{0}]", scene);
                SceneManager.UnloadSceneAsync(scene);
                m_unloadingScene = scene;
                SceneManager.sceneUnloaded += this.OnUnloadedScene;
            }
        }

        public void OnUnloadedScene(Scene unloadedScene)
        {
            if(unloadedScene.name == m_unloadingScene)
            {
                m_sceneLoaded = false;
                m_unloadingScene = "";
                SceneManager.sceneUnloaded -= this.OnUnloadedScene;
            }
        }
    }
}