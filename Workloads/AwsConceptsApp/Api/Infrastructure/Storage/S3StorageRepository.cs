using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces;

namespace Infrastructure.Storage
{
    public class S3StorageRepository : IFileStorageRepository
    {
        private readonly IAmazonS3 s3Client;
        const string bucketname = "awsconcepts";
        public S3StorageRepository(IAmazonS3 S3Client)
        {
            s3Client = S3Client;
        }
        public async Task<Tuple<Stream, string>> GetFile(string FileKey, CancellationToken cancellationToken)
        {
            GetObjectResponse s3Object = await s3Client.GetObjectAsync(bucketname, FileKey, cancellationToken);
            return new Tuple<Stream, string>(s3Object.ResponseStream, s3Object.Headers.ContentType);
        }

        public async Task PutFile(Stream File, string FileKey, string ContentType, CancellationToken cancellationToken)
        {

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketname,
                Key = FileKey,
                InputStream = File
            };
            request.Metadata.Add("Content-Type", ContentType);
            await s3Client.PutObjectAsync(request, cancellationToken);
        }
    }
}
