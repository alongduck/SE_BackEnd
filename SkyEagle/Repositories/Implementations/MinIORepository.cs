using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SkyDTO.Commons;
using SkyEagle.Classes;
using SkyEagle.Repositories.Interfaces;
using SkyModel;
using SkyModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Implementations
{
	public class MinIORepository(SkyDbContext context) : IMinIORepository
	{
		private readonly SkyDbContext _context = context;
		private const string URI = INIT.MyIP + ":9000";
		internal static string MinIOAccessKey = string.Empty;
		internal static string MinIOSecretKey = string.Empty;
		private const string Bucket = "skyeagle";

		internal static MyMinIO CreateMinIOInstance() => new(URI, MinIOAccessKey, MinIOSecretKey, Bucket);

		public async Task<KeyValuePair<string, byte[]>?> GetFileAsync(long id, CancellationToken ct = default)
		{
			MinIO? minIO = await _context.MinIOs.FindAsync([id], cancellationToken: ct);
			if (minIO == null)
				return null;
			using MyMinIO myMinIO = CreateMinIOInstance();
			byte[] data = await myMinIO.GetFileBytesAsync(minIO.FileName, ct);
			if (data.Length == 0)
				return null;
			return new KeyValuePair<string, byte[]>(minIO.FileName, data);
		}

		public async Task<ImageDTO> UploadImageAsync(IFormFile image, CancellationToken ct)
		{
			// Initialize MinIO instance
			using MyMinIO myMinIO = CreateMinIOInstance();

			// Validate size hình
			if (image.Length <= 0)
				throw new InvalidOperationException($"File không hợp lệ: {image.FileName}");

			// Tạo tên file
			string fileName = $"{DateTime.Now.Ticks}-{image.FileName}";

			using MemoryStream memoryStream = new();
			await image.CopyToAsync(memoryStream, ct);
			memoryStream.Seek(0, SeekOrigin.Begin);
			IImageFormat detectedFormat = Image.DetectFormat(memoryStream) ?? throw new InvalidOperationException($"Định dạng ảnh không được hỗ trợ: {image.FileName}");
			byte[] fileBytes = [];

			try
			{
				// Convert file ảnh thành webp
				fileBytes = await MyWebP.EncodeFromMemoryStreamAsync(memoryStream, ct: ct);
			}
			catch
			{
				throw new InvalidOperationException("Có Lỗi đã xảy ra trong lúc upload file hình ảnh");
			}
			fileName = Path.ChangeExtension(fileName, ".webp");

			// Upload file lên minIO
			var response = await myMinIO.UploadFileAsync(
				fileName,
				fileBytes,
				detectedFormat.DefaultMimeType,
				ct
			);

			// Check có etag hay không
			if (string.IsNullOrEmpty(response?.Etag))
				throw new Exception($"Upload file thất bại: {image.FileName}");

			// Thêm hình vào db
			MinIO result = new()
			{
				FileName = fileName,
				ETag = response.Etag,
				Size = response.Size
			};
			_context.MinIOs.Add(result);
			await _context.SaveChangesAsync(ct);
			return new ImageDTO
			{
				Id = result.Id,
				Src = CommonFunctions.GetMinIODisplayPath(result.Id)
			};
		}

		public async Task DeleteFileAsync(long id, CancellationToken ct = default)
		{
			MinIO? minIO = await _context.MinIOs.FirstOrDefaultAsync(x => x.Id == id, ct);
			if (minIO != null)
			{
				using MyMinIO myMinIO = CreateMinIOInstance();
				await myMinIO.RemoveFileAsync(minIO.FileName, ct);
				_context.MinIOs.Remove(minIO);
				await _context.SaveChangesAsync(ct);
			}
		}
	}
}
