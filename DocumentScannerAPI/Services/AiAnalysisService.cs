using GenerativeAI;

namespace DocumentScannerAPI.Services;

/// <summary>
/// Service for analyzing documents using Google Gemini AI model.
/// </summary>
public class AiAnalysisService : IAiAnalysisService
{
    private readonly GenerativeModel _model;

    public AiAnalysisService(IConfiguration configuration)
    {
        var apiKey = configuration["GoogleGemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini API key is not configured.");

        _model = new GenerativeModel(model: "gemini-2.5-flash-lite", apiKey: apiKey);
    }

    /// <summary>
    /// Analyzes contract text and answers user questions about it.
    /// </summary>
    /// <param name="contractText">The contract text to analyze.</param>
    /// <param name="userQuestion">The user's question about the contract.</param>
    /// <returns>The AI-generated response.</returns>
    public async Task<string> AnalyzeContractAsync(string contractText, string userQuestion)
    {
        var prompt = $"""
            {contractText}

            {userQuestion}
            """;

        var response = await _model.GenerateContentAsync(prompt);

        return response.Text ?? throw new InvalidOperationException("AI model returned empty response.");
    }
}
