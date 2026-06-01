using GenerativeAI;

namespace DocumentScannerAPI.Services;

/// <summary>
/// Service for analyzing documents using Google Gemini AI model.
/// </summary>
public class AiAnalysisService : IAiAnalysisService
{
    private readonly GenerativeModel _model;
    private const string SystemPrompt = """
        You are an expert financial contract lawyer with deep expertise in contract law, corporate finance, and risk management.
        Analyze the provided contract thoroughly and answer the user's questions with precise, actionable insights.

        Guidelines:
        - Identify potential risks, obligations, and liabilities clearly
        - Highlight key financial terms, payment schedules, and conditions
        - Explain complex clauses in simple, understandable language
        - Point out missing or unusual provisions when relevant
        - Provide practical recommendations where applicable
        - Use professional terminology but remain accessible

        Always prioritize accuracy and highlight any ambiguities that may require legal review.
        """;

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
        ArgumentException.ThrowIfNullOrEmpty(contractText);
        ArgumentException.ThrowIfNullOrEmpty(userQuestion);

        var prompt = $"""
            {SystemPrompt}

            CONTRACT:
            {contractText}

            USER QUESTION:
            {userQuestion}
            """;

        var response = await _model.GenerateContentAsync(prompt);

        return response.Text ?? throw new InvalidOperationException("AI model returned empty response.");
    }
}
