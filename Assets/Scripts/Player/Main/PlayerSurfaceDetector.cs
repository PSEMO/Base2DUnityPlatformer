using UnityEngine;

namespace PSEMO.Player
{
    public class PlayerSurfaceDetector
    {
        private readonly Vector2 groundCheckBoxSize;
        private readonly Vector2 wallCheckBoxSize;

        private readonly ContactFilter2D groundFilter;
        private readonly ContactFilter2D wallFilter;

        private readonly float groundCheckDistance;
        private readonly float wallCheckDistance;

        private readonly RaycastHit2D[] physicsHits = new RaycastHit2D[5];

        public PlayerSurfaceDetector(Collider2D col, PlayerSO data)
        {
            groundCheckBoxSize = new Vector2(col.bounds.size.x * 0.9f, col.bounds.size.y);
            wallCheckBoxSize = new Vector2(col.bounds.size.x, col.bounds.size.y * 0.9f);

            groundCheckDistance = data.groundCheckDistance;
            wallCheckDistance = data.wallCheckDistance;

            groundFilter = new ContactFilter2D
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = data.groundLayer
            };

            wallFilter = new ContactFilter2D
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = data.wallLayer
            };
        }

        public bool IsOnGround(Vector2 center)
        {
            int hitCount = Physics2D.BoxCast(
                center,
                groundCheckBoxSize,
                0f,
                Vector2.down,
                groundFilter,
                physicsHits,
                groundCheckDistance);

            return hitCount > 0;
        }

        public bool IsFacingWall(Vector2 center, int facing)
        {
            int hitCount = Physics2D.BoxCast(
                center,
                wallCheckBoxSize,
                0f,
                Vector2.right * facing,
                wallFilter,
                physicsHits,
                wallCheckDistance);

            return hitCount > 0;
        }
    }
}
