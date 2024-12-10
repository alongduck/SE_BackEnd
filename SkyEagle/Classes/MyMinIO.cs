using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

internal class MyMinIO(string uri, string access, string secret, string bucket) : IDisposable
{
	private readonly string URI = uri;
	private readonly string Access = access;
	private readonly string Secret = secret;
	private readonly string Bucket = bucket;
	private IMinioClient? _minIO;
	private IMinioClient MinIO => _minIO ??= new MinioClient().WithEndpoint(URI).WithCredentials(Access, Secret).Build();

	public async Task<PutObjectResponse?> UploadFileAsync(string onlineFilePath, string localFilePath, string contentType, CancellationToken ct = default)
	{
		// Upload a file to bucket.
		PutObjectArgs putObjectArgs = new PutObjectArgs()
			.WithBucket(Bucket)
			.WithObject(onlineFilePath.Replace('\\', '/'))
			.WithFileName(localFilePath)
			.WithContentType(contentType);
		try
		{
			PutObjectResponse response = await MinIO.PutObjectAsync(putObjectArgs, ct);
			return response;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
	}

	public async Task<PutObjectResponse?> UploadFileAsync(string onlineFilePath, byte[] fileData, string contentType, CancellationToken ct = default)
	{
		// Upload a file to bucket using byte array.
		using MemoryStream memoryStream = new MemoryStream(fileData);
		PutObjectArgs putObjectArgs = new PutObjectArgs()
			.WithBucket(Bucket)
			.WithObject(onlineFilePath.Replace('\\', '/'))
			.WithStreamData(memoryStream)
			.WithObjectSize(fileData.Length)
			.WithContentType(contentType);
		try
		{
			PutObjectResponse response = await MinIO.PutObjectAsync(putObjectArgs, ct);
			return response;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
	}

	public async Task<ObjectStat?> GetFileAsync(string onlineFilePath, string localFilePath, CancellationToken ct = default)
	{
		// Get a file from bucket.
		GetObjectArgs args = new GetObjectArgs()
				.WithBucket(Bucket)
				.WithObject(onlineFilePath.Replace('\\', '/'))
				.WithFile(localFilePath);
		try
		{
			ObjectStat response = await MinIO.GetObjectAsync(args, ct);
			return response;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
	}

	public async Task<byte[]> GetFileBytesAsync(string onlineFilePath, CancellationToken ct = default)
	{
		// Get a file bytes from bucket.
		MemoryStream memoryStream = new MemoryStream();
		GetObjectArgs args = new GetObjectArgs()
				.WithBucket(Bucket)
				.WithObject(onlineFilePath.Replace('\\', '/'))
				.WithCallbackStream(stream => stream.CopyTo(memoryStream));
		try
		{
			await MinIO.GetObjectAsync(args, ct);
			byte[] data = memoryStream.ToArray();
			memoryStream.Dispose();
			return data;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return Array.Empty<byte>();
		}
	}

	public async Task<bool> RemoveFileAsync(string onlineFilePath, CancellationToken ct = default)
	{
		// Remove a file from bucket.
		RemoveObjectArgs args = new RemoveObjectArgs()
				.WithBucket(Bucket)
				.WithObject(onlineFilePath.Replace('\\', '/'));
		try
		{
			await MinIO.RemoveObjectAsync(args, ct);
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return false;
		}
	}

	public async Task<List<string>> RemoveFilesAsync(IList<string> onlineFilePaths, CancellationToken ct = default)
	{
		// Remove files from bucket.
		List<string> removed = new();
		for (int i = 0; i < onlineFilePaths.Count; i++)
		{
			onlineFilePaths[i] = onlineFilePaths[i].Replace('\\', '/');
			removed.Add(onlineFilePaths[i]);
		}
		RemoveObjectsArgs args = new RemoveObjectsArgs()
			.WithBucket(Bucket)
			.WithObjects(removed);
		try
		{
			foreach (DeleteError objDeleteError in await MinIO.RemoveObjectsAsync(args, ct))
				removed.Remove(objDeleteError.Key);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
		return removed;
	}

	public void Dispose() => _minIO?.Dispose();
}