using PSEMO.Player;
using UnityEngine;
using PSEMO.Environment.Functionality;

namespace PSEMO.Core.Persistence
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerPersister : Persists
    {
        PlayerController ctx;

        void Awake()
        {
            ctx = GetComponent<PlayerController>();
        }

        public override void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(jsonData);
            
            ctx.transform.position = data.playerPosition;
            ctx.respawnPos = data.playerRespawnPosition;
            ctx.ableToRun = data.ableToRun;
            ctx.ableToJump = data.ableToJump;
            ctx.ableToDash = data.ableToDash;
            ctx.ableToInteract = data.ableToInteract;
            ctx.maxJumpCount = data.maxJumpCount;

            if (data.ResetOnLoad)
            {
                ctx.Respawn();
            }
        }

        public override string SaveData()
        {
            PlayerSaveData data = new()
            {
                playerPosition = ctx.transform.position,
                playerRespawnPosition = ctx.respawnPos,
                ableToRun = ctx.ableToRun,
                ableToJump = ctx.ableToJump,
                ableToDash = ctx.ableToDash,
                ableToInteract = ctx.ableToInteract,
                maxJumpCount = ctx.maxJumpCount
            };
            return JsonUtility.ToJson(data);
        }
    }
}