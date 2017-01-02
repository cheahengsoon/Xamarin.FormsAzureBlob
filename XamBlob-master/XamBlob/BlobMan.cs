﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace XamBlob
{
	public class BlobMan
	{
		public static BlobMan Instance { get; } = new BlobMan();

		private BlobMan()
		{
            //_fullResContainer = _blobClient.GetContainerReference("fullres");
            //_lowResContainer = _blobClient.GetContainerReference("lowres");
            _fullResContainer = _blobClient.GetContainerReference("petuser");
            _lowResContainer = _blobClient.GetContainerReference("petuser");
        }

        // Get this from the Azure Portal by clicking the "key" icon of the storage.
        //const string connectionString = "DefaultEndpointsProtocol=https;AccountName=krumelurtest1;AccountKey=Ys7kxm6biyF9ZDCLR+i9X9qPlf9ZMkPkJVbLkOYvnr/sFWJtqgIGL+BNbY6lqYElEnV0GnUBRTSUB2yuhzBvAA==;";
        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=petfrenzblob;AccountKey=zFc5dXh6f5CH13K3IIvBphEyCumAwl/8dXOJz+BLS/Sbg2AjhNQMDOBT65LYe8OfnADfFfeKjiK2azpaq3SdZw==";

        // Create the blob client.
        CloudBlobClient _blobClient = CloudStorageAccount
			.Parse(connectionString)
			.CreateCloudBlobClient();


		CloudBlobContainer _fullResContainer;
		CloudBlobContainer _lowResContainer;

		public async Task<List<Uri>> GetAllBlobUrisAsync()
		{
			// Not quite perfect: requires multiple queries if there are many blobs.
			var contToken = new BlobContinuationToken();
			//var allBlobs = await _fullResContainer.ListBlobsSegmentedAsync(contToken).ConfigureAwait(false);
			var allBlobs = await _lowResContainer.ListBlobsSegmentedAsync(contToken).ConfigureAwait(false);


			var uris = allBlobs.Results.Select(b => b.Uri).ToList();

			return uris;
		}

		public async Task UploadFileAsync(string localPath)
		{
			string uniqueBlobName = Guid.NewGuid().ToString();
			uniqueBlobName += Path.GetExtension(localPath);
			var blobRef = _fullResContainer.GetBlockBlobReference(uniqueBlobName);

			// Can upload files, streams, text, ...
			await blobRef.UploadFromFileAsync(localPath).ConfigureAwait(false);
		}
	}
}
