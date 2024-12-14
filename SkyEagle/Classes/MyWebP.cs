using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

internal static class MyWebP
{
	/// <param name="quality">0 -> 100, null or 100 is lossless</param>
	internal static async Task<byte[]> EncodeFromMemoryStreamAsync(MemoryStream inputStream, int? quality = 80, CancellationToken ct = default)
	{
		inputStream.Position = 0;
		using Image image = await Image.LoadAsync(inputStream, ct);
		using MemoryStream outputStream = new();
		await EncodeImageAsync(image, outputStream, quality, ct);
		return outputStream.ToArray();
	}

	private static async Task<bool> EncodeImageAsync(Image image, Stream outputStream, int? quality, CancellationToken ct)
	{
		WebpEncoder encoder = GetWebPEncoder(quality);
		await image.SaveAsync(outputStream, encoder, ct);
		return outputStream.Length > 0;
	}

	private static WebpEncoder GetWebPEncoder(int? quality)
	{
		return quality == null || quality == 100
			? new WebpEncoder { FileFormat = WebpFileFormatType.Lossless }
			: new WebpEncoder { Quality = quality.Value };
	}
}