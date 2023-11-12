using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Com.FastEffect.UI.Video
{

    public class OnEndOfVideoEvent : MonoBehaviour
    {
        public VideoPlayer m_player;
        [SerializeField]
        private bool m_LoopVideo = false;
        private bool m_PreviouslyLooping;
        public UnityEvent m_OnEndOfVideo;

        void OnEnable()
        {
            if (m_player == null)
            { 
               if(!TryGetComponent<VideoPlayer>(out m_player))
                {
                    Debug.LogError("No Video Player Found!");
                    return;
                }
            }
            m_PreviouslyLooping = m_player.isLooping;
            m_player.isLooping = true;
            m_player.loopPointReached += OnEndOfVideo;
        }

        public void SkipVideo()
        {
            SkipVideo(m_player);
        }

        public void SkipVideo(VideoPlayer vp)
        {
            if (vp != null)
            {
                vp.Stop();
            }
            m_OnEndOfVideo.Invoke();
        }

        public void OnEndOfVideo(VideoPlayer vp)
        {
            if (!m_LoopVideo)
            {
                vp.Stop();
            }
            m_OnEndOfVideo.Invoke();
        }

        void OnDisable()
        {
            if (m_player != null)
            {
                m_player.isLooping = m_PreviouslyLooping;
                m_player.loopPointReached -= OnEndOfVideo;
            }
        }
    }
}