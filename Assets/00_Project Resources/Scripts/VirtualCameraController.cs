using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using System;

public class VirtualCameraController : MonoBehaviour
{
    [Space]
    [Header("Collection of Virtual Cameras in Scene")]
    public List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();

    [Space]
    [Header("Teleport Positions Index Reference, Used for Active Camera Index Arg")]
    [Tooltip("Index Value used for the Teleport Positions and Virtual Cameras since they share the same array count")]
    public int CameraIndexReference = 0;

    [Header("Delay Before Switching To New Camera")]
    public float TransitionDelay = 0;

    #region Initialize Virtual Camera Controller

    void Awake()
    {
        List<CinemachineVirtualCamera> childCameras = new List<CinemachineVirtualCamera>(GetComponentsInChildren<CinemachineVirtualCamera>(true));
        foreach(CinemachineVirtualCamera cCam in childCameras)
        {
            if(!virtualCameras.Contains(cCam))
            {
                virtualCameras.Add(cCam);
            }
        }
    }

    void Start()
    {
        InitCameras();
    }

    //Setup 
    public void Init()
    {
        InitCameras();
    }
    
    public void InitCameras()
    {

        for (int i = 1; i < virtualCameras.Count; i++)
        {
            virtualCameras[i].gameObject.SetActive(false);
        }
    }

    #endregion

    #region Camera Movement

    public void UpdateCurrentCamera(int cameraToUse)
    {
        if(virtualCameras.Count == 0)
        {
            // TODO: empty camera list handleing
            Debug.LogWarning("virtualCameras is empty! There is no usable index to set the current camera to!");
        }
        else if(cameraToUse < 0)
        {
            // TODO: negative handleing
            Debug.LogWarningFormat("cameraToUse [{0}] is less than zero! that's too low to use as an index of virtualCameras!",cameraToUse);
        }
        else if(cameraToUse >= virtualCameras.Count)
        {
            // TODO: out of range handling
            Debug.LogWarningFormat("cameraToUse [{0}] is too high to use as an index of virtualCameras! [{1}]",cameraToUse,virtualCameras.Count);
        }

        if(cameraToUse == CameraIndexReference)
        {
            // TODO: case: we are trying to change to the already currently active camera
        }
        CameraIndexReference = cameraToUse;

        SetCurrentCamera();
    }

    //Activates a Virtual Camera based on Area 
    public void SetCurrentCamera()
    {
        StartCoroutine(DelayedCameraTransition());
    }

    //Camera Transition With Particle VFX and Inital Delay
    IEnumerator DelayedCameraTransition()
    {
        yield return new WaitForSeconds(0);

        for(int i = virtualCameras.Count - 1; i >= 0; i--)
        {
            if(virtualCameras[i] == null)
            {
                virtualCameras.RemoveAt(i);
            }
            else if (i == CameraIndexReference)
            {
                virtualCameras[i].gameObject.SetActive(true);
            }
            else if (virtualCameras[i].gameObject.activeInHierarchy)
            {
                virtualCameras[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
