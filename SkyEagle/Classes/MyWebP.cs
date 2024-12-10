using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

internal static class MyWebP
{
	/// <param name="quality">0 -> 100, null or 100 is lossless</param>
	public static bool Encode(string fullPathInputFile, string fullPathOutputFile, int? quality = 80)
	{
		using Image image = Image.Load(fullPathInputFile);
		return EncodeImage(image, fullPathOutputFile, quality);
	}

	/// <param name="quality">0 -> 100, null or 100 is lossless</param>
	public static async Task<bool> EncodeAsync(string fullPathInputFile, string fullPathOutputFile, int? quality = 80, CancellationToken ct = default)
	{
		using Image image = await Image.LoadAsync(fullPathInputFile, ct);
		return await EncodeImageAsync(image, fullPathOutputFile, quality, ct);
	}

	public static bool Decode(string fullPathInputFile, string fullPathOutputFile)
	{
		using Image image = Image.Load(fullPathInputFile);
		image.Save(fullPathOutputFile);
		return File.Exists(fullPathOutputFile);
	}

	public static async Task<bool> DecodeAsync(string fullPathInputFile, string fullPathOutputFile, CancellationToken ct = default)
	{
		using Image image = await Image.LoadAsync(fullPathInputFile, ct);
		await image.SaveAsync(fullPathOutputFile, ct);
		return File.Exists(fullPathOutputFile);
	}

	/// <param name="quality">0 -> 100, null or 100 is lossless</param>
	public static byte[] EncodeFromMemoryStream(MemoryStream inputStream, int? quality = 80)
	{
		inputStream.Position = 0;
		using Image image = Image.Load(inputStream);
		using MemoryStream outputStream = new();
		EncodeImage(image, outputStream, quality);
		return outputStream.ToArray();
	}

	/// <param name="quality">0 -> 100, null or 100 is lossless</param>
	public static async Task<byte[]> EncodeFromMemoryStreamAsync(MemoryStream inputStream, int? quality = 80, CancellationToken ct = default)
	{
		inputStream.Position = 0;
		using Image image = await Image.LoadAsync(inputStream, ct);
		using MemoryStream outputStream = new();
		await EncodeImageAsync(image, outputStream, quality, ct);
		return outputStream.ToArray();
	}

	public static byte[] DecodeFromMemoryStream(MemoryStream inputStream)
	{
		inputStream.Position = 0;
		using Image image = Image.Load(inputStream);
		using MemoryStream outputStream = new();
		image.Save(outputStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
		return outputStream.ToArray();
	}

	public static async Task<byte[]> DecodeFromMemoryStreamAsync(MemoryStream inputStream, CancellationToken ct = default)
	{
		inputStream.Position = 0;
		using Image image = await Image.LoadAsync(inputStream, ct);
		using MemoryStream outputStream = new();
		await image.SaveAsync(outputStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder(), ct);
		return outputStream.ToArray();
	}

	private static bool EncodeImage(Image image, string fullPathOutputFile, int? quality)
	{
		WebpEncoder encoder = GetWebPEncoder(quality);
		image.Save(fullPathOutputFile, encoder);
		return File.Exists(fullPathOutputFile);
	}

	private static bool EncodeImage(Image image, Stream outputStream, int? quality)
	{
		WebpEncoder encoder = GetWebPEncoder(quality);
		image.Save(outputStream, encoder);
		return outputStream.Length > 0;
	}

	private static async Task<bool> EncodeImageAsync(Image image, string fullPathOutputFile, int? quality, CancellationToken ct)
	{
		WebpEncoder encoder = GetWebPEncoder(quality);
		await image.SaveAsync(fullPathOutputFile, encoder, ct);
		return File.Exists(fullPathOutputFile);
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