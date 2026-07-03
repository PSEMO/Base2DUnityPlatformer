namespace PSEMO.Core.Persistence
{
    public interface IPersistable
    {
        public void LoadData(string jsonData);

        public string SaveData();
    }
}