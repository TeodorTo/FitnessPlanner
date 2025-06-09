using FitnessPlanner.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CachedWorkoutsController : ControllerBase
{
    private readonly KafkaCacheConsumerService _cacheService;

    public CachedWorkoutsController(KafkaCacheConsumerService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var workouts = await _cacheService.GetAllCachedWorkoutsAsync();
        return Ok(workouts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var workout = await _cacheService.GetWorkoutFromCacheAsync(id);
        return workout == null ? NotFound() : Ok(workout);
    }
}