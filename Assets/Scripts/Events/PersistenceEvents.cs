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

        public static event Action<string> OnCreateEmptySceneFile;
        public static void InvokeCreateEmptySceneFile(string sceneName) => OnCreateEmptySceneFile?.Invoke(sceneName);

        public static event Action<Persister> OnPersistsObjectAdded;
        public static void InvokePersistsObjectAdded(Persister objToAdd) => OnPersistsObjectAdded?.Invoke(objToAdd);

        public static event Action<Persister> OnPersistsObjectRemoved;
        public static void InvokePersistsObjectRemoved(Persister objToRemove) => OnPersistsObjectRemoved?.Invoke(objToRemove);

        public static event Action<string> OnSaveSlotChanged;
        public static void InvokeSaveSlotChanged(string slotName) => OnSaveSlotChanged?.Invoke(slotName);
    }
}