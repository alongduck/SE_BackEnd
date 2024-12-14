namespace SkyEagle;

internal static class INIT
{
#if DEBUG
	public const string MyIP = "127.0.0.1";
	public const string DomainShort = "localhost:5042";
#else
	public const string MyIP = "192.168.1.101";
	public const string DomainShort = "";
#endif
	public const string Domain = $"https://{DomainShort}/";
}