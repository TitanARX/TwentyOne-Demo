using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoTextureReleaser : MonoBehaviour
{
    private VideoPlayer m_player = null;

    void Awake()
    {
        m_player = GetComponent<VideoPlayer>();
    }

    public void StopAndRelease()
    {
        m_player.Stop();
        m_player.targetTexture.Release();
    }
    
}
