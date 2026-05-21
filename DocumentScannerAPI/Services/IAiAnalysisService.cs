namespace DocumentScannerAPI.Services
{
    public interface IAiAnalysisService
    {
        Task<string> AnalyzeContractAsync(string contractText, string userQuestion);
    }
}
