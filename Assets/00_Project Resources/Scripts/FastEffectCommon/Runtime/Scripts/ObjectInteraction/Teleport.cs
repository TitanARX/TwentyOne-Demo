using Com.FastEffect.DataTypes;
using Com.FastEffect.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FastEffect.ObjectInteraction
{
    public class Teleport : MonoBehaviour, IOneArgEventListener<int>
    {
        [Header("Pre-Set Teleport Destinations")]
        [Tooltip("These Transforms will set this object's position and rotation when teleported to")]
        public List<Transform> Destinations = new List<Transform>();
        [Header("Value that determines if the target can teleport")]
        public BoolReference canTeleport = new BoolReference();
        [Header("Teleport Positions Index Reference")]
        [SerializeReference]
        private IntValue m_SignalWatcher = null;

        //Injected Behaviour
        TeleportBase GetTeleportBase;

        private void Awake()
        {
            Init();
        }

        #region Initialize Teleport Controller
        public void Init()
        {
            GetTeleportBase = new TeleportBase(Destinations, this.transform);
            if (m_SignalWatcher != null)
            {
                m_SignalWatcher.Value = 0;
            }
        }
        #endregion

        #region Teleport Behaviour
        public void TeleportPlayer(int destination = 0)
        {
            if (canTeleport.Value && GetTeleportBase != null)
            {
                GetTeleportBase.TeleportTransform(destination);
            }
        }

        #endregion

        #region IntEventSetup
        public void OnEnable()
        {
            if (m_SignalWatcher != null)
            {
                m_SignalWatcher.Subscribe(this);
            }
        }

        public void OnDisable()
        {
            if (m_SignalWatcher != null)
            {
                m_SignalWatcher.Unsubscribe(this);
            }
        }

        public void OnEventRaised(int arg)
        {
            TeleportPlayer(arg);
        }
        #endregion

    }
}