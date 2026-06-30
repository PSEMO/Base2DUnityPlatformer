using UnityEngine;

namespace PSEMO.Environment.Functionality.Collectible
{
    [CreateAssetMenu(fileName = "CollectibleData", menuName = "SO/Collectible")]
    public class CollectibleSO : ScriptableObject
    {
        public string group;
        public int totalAmountOfThisGroup;
        public string displayName;
    }
}