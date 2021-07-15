using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class SceneInitializer : MonoBehaviour, ISceneInitializer
{
    [Header("Prerequisite Setup Delay")]
    [SerializeField]
    private float prerequisiteDelay = 0;

    [Space]
    [Header("Prerequisite Event Calls")]
    public UnityEvent PrerequisiteEvents;

    [Header("Postrequisite Setup Delay")]
    [SerializeField]
    private float postrequisiteDelay = 0.25f;

    [Space]
    [Header("Postrequisite Events Calls")]
    public UnityEvent PostrequisiteEvents;

    [Header("Post Initialization Setup Delay")]
    [SerializeField]
    private float initializedEventsDelay = 1.25f;

    [Space]
    [Header("Post Initialization Methods")]
    public UnityEvent InitializedEvents;

    [Space]
    [Header("Scene Cleanup Event")]
    public UnityEvent EndLevelEvents;

    public void OpenEvents()
    {
        PrerequisiteEvents.Invoke();
    }

    public void CloseEvents()
    {
        EndLevelEvents.Invoke();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(prerequisiteDelay);

        PrerequisiteEvents.Invoke();

        yield return new WaitForSeconds(postrequisiteDelay);

        PostrequisiteEvents.Invoke();

        yield return new WaitForSeconds(initializedEventsDelay);

        InitializedEvents.Invoke();
    }



}
