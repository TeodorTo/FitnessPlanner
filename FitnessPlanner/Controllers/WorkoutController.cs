using FitnessPlanner.BL.Services;
using FitnessPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workout>>> Get()
        {
            var workouts = await _workoutService.GetAllWorkoutsAsync();
            return Ok(workouts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Workout>> Get(string id)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id);
            if (workout == null)
                return NotFound();

            return Ok(workout);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Workout workout)
        {
            await _workoutService.CreateWorkoutAsync(workout);
            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Workout workout)
        {
            if (id != workout.Id)
                return BadRequest();

            await _workoutService.UpdateWorkoutAsync(workout);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var existingWorkout = await _workoutService.GetWorkoutByIdAsync(id);
            if (existingWorkout == null)
                return NotFound();

            await _workoutService.DeleteWorkoutAsync(id);
            return NoContent();
        }
    }
}