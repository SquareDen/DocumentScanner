using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.Text;

namespace DocumentScannerAPI.Services
{
    public class OcrService : IOcrService
    {
        private readonly string _visionEndpoint;
        private readonly string _visionApiKey;

        public OcrService(IConfiguration configuration)
        {
            _visionEndpoint = GetConfigurationValue(configuration, "AzureVision:Endpoint");
            _visionApiKey = GetConfigurationValue(configuration, "AzureVision:Key");
        }

        public async Task<string> ExtractTextFromImageAsync(Stream imageStream, CancellationToken cancellationToken = default)
        {
            var imageBytes = await BinaryData.FromStreamAsync(imageStream, cancellationToken);
            var analysisResult = await AnalyzeImageAsync(imageBytes, cancellationToken);

            return ParseExtractedText(analysisResult);
        }

        private async Task<ImageAnalysisResult> AnalyzeImageAsync(BinaryData imageData, CancellationToken cancellationToken)
        {
            var visionClient = new ImageAnalysisClient(
                new Uri(_visionEndpoint),
                new AzureKeyCredential(_visionApiKey)
            );

            var response = await visionClient.AnalyzeAsync(
                imageData,
                VisualFeatures.Read,
                default,
                cancellationToken
            );

            return response.Value;
        }

        private static string ParseExtractedText(ImageAnalysisResult analysisResult)
        {
            if (analysisResult.Read?.Blocks is null or { Count: 0 })
                return string.Empty;

            var textContent = new StringBuilder();

            foreach (var block in analysisResult.Read.Blocks)
            {
                foreach (var textLine in block.Lines)
                {
                    textContent.AppendLine(textLine.Text);
                }
            }

            return textContent.ToString().TrimEnd();
        }

        private string GetConfigurationValue(IConfiguration configuration, string key)
            => configuration[key] ?? throw new InvalidOperationException($"{key} is not configured.");
    }
}
