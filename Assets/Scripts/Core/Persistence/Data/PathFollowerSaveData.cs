using System;
using UnityEngine;

namespace PSEMO.Core.Persistence
{
    [Serializable]
    public class PathFollowerSaveData
    {
        public Vector3 position;
        public int currentWaypointIndex;
    }
}