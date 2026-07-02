using System;
using PSEMO.Core.Persistence;

namespace PSEMO.Events
{
    public static class PersistenceEvents
    {
        public static event Action OnGameSave;
        public static void InvokeGameSave() => OnGameSave?.Invoke();

        public static event Action OnGameSaveDelete;
        public static void InvokeGameSaveDelete() => OnGameSaveDelete?.Invoke();

        public static event Action<Persists> OnPersistsObjectAdded;
        public static void InvokePersistsObjectAdded(Persists objToAdd) => OnPersistsObjectAdded?.Invoke(objToAdd);

        public static event Action<Persists> OnPersistsObjectRemoved;
        public static void InvokePersistsObjectRemoved(Persists objToAdd) => OnPersistsObjectRemoved?.Invoke(objToAdd);
    }
}