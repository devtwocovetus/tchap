//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Services;
//using Google.Apis.Storage.v1;
//using Google.Cloud.Storage.V1;
//using System;
//using System.IO;
//using System.Threading.Tasks;
//using System.Web;

namespace TheCloudHealth.Lib
{
    public class ImageUploader
    {
        //private readonly string _bucketName;
        //private readonly StorageClient _storageClient;
        //private readonly string fileName;
        //private readonly string filepath;
        //public ImageUploader(string bucketName)
        //{
        //    fileName = "TheCloudHealth-b707b121554c.json";
        //    filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ConfigFile\", fileName);
        //    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
        //    string projectId = "thecloudhealthcare";
        //    _bucketName = bucketName;
        //    // [START storageclient]
        //    //_storageClient = StorageClient.Create();
        //    // [END storageclient]
        //}



        //public StorageClient AuthImplicit(string projectId)
        //{
        //    // If you don't specify credentials when constructing the client, the
        //    // client library will look for credentials in the environment.
        //    //var credential = GoogleCredential.GetApplicationDefault();
        //    //return StorageClient.Create(credential);

        //    // Make an authenticated API request.

        //    //var buckets = storage.ListBuckets(projectId);
        //    //foreach (var bucket in buckets)
        //    //{
        //    //   Console.WriteLine(bucket.Name);
        //    //}
        //    //return null;
        //}


        // [START uploadimage]
        //public async Task<String> UploadImage(HttpPostedFile image, long id)
        //{
        //    var imageAcl = PredefinedObjectAcl.PublicRead;

        //    var imageObject = await _storageClient.UploadObjectAsync(
        //        bucket: _bucketName,
        //        objectName: id.ToString(),
        //        contentType: image.ContentType,
        //        source: image.InputStream,
        //        options: new UploadObjectOptions { PredefinedAcl = imageAcl }
        //    );

        //    return imageObject.MediaLink;
        //}
        //// [END uploadimage]

        //public async Task DeleteUploadedImage(long id)
        //{
        //    try
        //    {
        //        await _storageClient.DeleteObjectAsync(_bucketName, id.ToString());
        //    }
        //    catch (Google.GoogleApiException exception)
        //    {
        //        // A 404 error is ok.  The image is not stored in cloud storage.
        //        if (exception.Error.Code != 404)
        //            throw;
        //    }
        //}
    }
}