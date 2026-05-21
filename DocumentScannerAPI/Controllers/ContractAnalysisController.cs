using Microsoft.AspNetCore.Mvc;
using DocumentScannerAPI.Models;
using DocumentScannerAPI.Services;

namespace DocumentScannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractAnalysisController : ControllerBase
    {
        private readonly IOcrService _ocrService;
        private readonly IAiAnalysisService _aiAnalysisService;

        public ContractAnalysisController(
            IOcrService ocrService,
            IAiAnalysisService aiAnalysisService)
        {
            _ocrService = ocrService;
            _aiAnalysisService = aiAnalysisService;
        }

        [HttpPost]
        public async Task<IActionResult> AnalyzeContract([FromForm] DocumentRequest request)
        {
            if (request?.ContractPhoto == null)
            {
                return BadRequest(new DocumentResponse
                {
                    Success = false,
                    ErrorMessage = "Please, upload the photo"
                });
            }

            if (string.IsNullOrWhiteSpace(request.UserQuestion))
            {
                return BadRequest(new DocumentResponse
                {
                    Success = false,
                    ErrorMessage = "Please, add your question"
                });
            }

            try
            {
                string extractedText;
                using (var stream = request.ContractPhoto.OpenReadStream())
                {
                    extractedText = await _ocrService.ExtractTextFromImageAsync(stream);
                }

                if (string.IsNullOrWhiteSpace(extractedText))
                {
                    return BadRequest(new DocumentResponse
                    {
                        Success = false,
                        ErrorMessage = "Incorrect photo, please try add new photo"
                    });
                }

                var answer = await _aiAnalysisService.AnalyzeContractAsync(
                    extractedText,
                    request.UserQuestion
                );

                return Ok(new DocumentResponse
                {
                    Question = request.UserQuestion,
                    ExtractedText = extractedText,
                    Answer = answer,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DocumentResponse
                {
                    Success = false,
                    ErrorMessage = $"Error: {ex.Message}"
                });
            }
        }
    }
}
