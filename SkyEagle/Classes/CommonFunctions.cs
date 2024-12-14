namespace SkyEagle.Classes;

internal class CommonFunctions
{
	internal static string GetMinIODisplayPath(long fileId) => $"{INIT.Domain}minio/file/{fileId}";
}