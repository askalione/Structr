namespace Structr.Configuration
{
    public interface IConfiguration<out TSettings> where TSettings : class, new()
    {
        TSettings Settings { get; }
    }
}
