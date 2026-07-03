using System;
using System.Collections.Generic;

namespace PSEMO.Core.Persistence
{
    [Serializable]
    public class CollectibleTrackerSaveData
    {
        public List<string> keys = new();
        public List<int> values = new();
    }
}
