using System.Collections.Generic;
using UnityEngine;

namespace PSEMO.Core.Persistence
{
    [RequireComponent(typeof(IPersistable))]
    public class GeneralPersister : Persister
    {
        IPersistable[] ctxes;

        void Awake()
        {
            ctxes = GetComponents<IPersistable>();
        }

        public override void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            SerializableDictionary dict = JsonUtility.FromJson<SerializableDictionary>(jsonData);
            if (dict == null) return;

            Dictionary<string, int> typeCounts = new Dictionary<string, int>();

            foreach (IPersistable ctx in ctxes)
            {
                //naming for the dict keys
                string uniqueName = GetUniqueName(ctx, typeCounts);

                int index = dict.keys.IndexOf(uniqueName);
                if (index >= 0)
                {
                    ctx.LoadData(dict.values[index]);
                }
            }
        }

        public override string SaveData()
        {
            SerializableDictionary dict = new();
            Dictionary<string, int> typeCounts = new();
            
            foreach (IPersistable ctx in ctxes)
            {
                //naming for the dict keys
                string uniqueName = GetUniqueName(ctx, typeCounts);

                dict.keys.Add(uniqueName);
                dict.values.Add(ctx.SaveData());
            }

            return JsonUtility.ToJson(dict);
        }

        private string GetUniqueName(IPersistable ctx, Dictionary<string, int> typeCounts)
        {
            string typeName = ctx.GetType().FullName;
            if (typeCounts.ContainsKey(typeName))
                typeCounts[typeName]++;
            else
                typeCounts[typeName] = 0;
            return $"{typeName}_{typeCounts[typeName]}";
        }
    }
}