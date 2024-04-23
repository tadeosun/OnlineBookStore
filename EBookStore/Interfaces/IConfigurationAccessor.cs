namespace EBookStore.Interfaces
{
    public interface IConfigurationAccessor
    {
        T GetValue<T>(string key);
    }
}
