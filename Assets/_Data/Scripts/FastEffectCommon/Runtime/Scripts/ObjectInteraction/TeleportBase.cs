using System.Collections.Generic;
using UnityEngine;

namespace Com.FastEffect.ObjectInteraction
{
    public class TeleportBase
    {
        public Transform ObjectToTeleport;
        public List<Transform> DestinationPoints;

        public TeleportBase(List<Transform> points,Transform objToTeleport)
        {
            this.ObjectToTeleport = objToTeleport;
            this.DestinationPoints = points;
        }

        public void TeleportTransform(int DestinationIndex)
        {
            if (DestinationIndex < DestinationPoints.Count && DestinationIndex >=0 && DestinationPoints[DestinationIndex] != null)
            {
                ObjectToTeleport.transform.SetPositionAndRotation(DestinationPoints[DestinationIndex].position, DestinationPoints[DestinationIndex].rotation);
                Physics.SyncTransforms();
            }
        }

    }
}