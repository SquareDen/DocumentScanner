namespace DocumentScannerAPI.Services
{
    public interface IOcrService
    {
        Task<string> ExtractTextFromImageAsync(Stream imageStream, CancellationToken cancellationToken = default);
    }
}
