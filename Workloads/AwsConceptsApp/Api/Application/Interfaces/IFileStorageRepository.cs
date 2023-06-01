namespace Application.Interfaces
{
    public interface IFileStorageRepository
    {
        Task PutFile(Stream File, string FileKey, string ContentType, CancellationToken cancellationToken);
        Task<Tuple<Stream, string>> GetFile(string FileKey, CancellationToken cancellationToken);
    }
}
