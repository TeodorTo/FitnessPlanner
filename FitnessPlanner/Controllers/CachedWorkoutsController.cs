using FitnessPlanner.Kafka;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAll()
    {
        var workouts = _cacheService.GetAllCachedWorkouts();
        return Ok(workouts);
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var workout = _cacheService.GetWorkoutFromCache(id);
        return workout == null ? NotFound() : Ok(workout);
    }
}