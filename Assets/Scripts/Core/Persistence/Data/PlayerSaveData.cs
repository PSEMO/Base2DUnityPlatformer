using System;
using UnityEngine;

namespace PSEMO.Core.Persistence
{
    [Serializable]
    public class PlayerSaveData
    {
        public Vector3 playerPosition;
        public Vector3 playerRespawnPosition;
        public bool ableToRun;
        public bool ableToJump;
        public bool ableToDash;
        public bool ableToInteract;
        public int maxJumpCount;
    }
}