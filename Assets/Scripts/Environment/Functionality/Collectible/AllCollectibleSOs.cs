using System.Collections.Generic;
using UnityEngine;

namespace PSEMO.Environment.Functionality.Collectible
{
    [CreateAssetMenu(fileName = "AllCollectibleDatas", menuName = "SO/AllCollectibles")]
    public class AllCollectibleSOs : ScriptableObject
    {
        public List<CollectibleSO> collectibles;
    }
}