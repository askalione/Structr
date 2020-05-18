namespace Structr.Configuration
{
    public interface ISettingsProvider<TSettings> where TSettings : class, new()
    {
        TSettings GetSettings();        
        void SetSettings(TSettings settings);
        bool IsSettingsChanged();
    }
}
