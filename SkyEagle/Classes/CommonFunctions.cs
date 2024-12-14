namespace SkyEagle.Classes;

public class CommonFunctions
{
	public static string GetMinIODisplayPath(long fileId) => $"{INIT.Domain}minio/file/{fileId}";
}