using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/recommend/semantic")]
public class SemanticRecommendController : ControllerBase
{
    private readonly TourRecommendationService _aiService;
    public SemanticRecommendController(TourRecommendationService aiService) => _aiService = aiService;

    [HttpPost]
    public async Task<IActionResult> Recommend([FromBody] SemanticQuery model)
    {
        var tours = await _aiService.GetSemanticRecommendationsAsync(model.Query, model.TopK);
        return Ok(tours);
    }
}
