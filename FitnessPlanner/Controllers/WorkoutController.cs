using FitnessPlanner.BL.Services;
using FitnessPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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
            Console.WriteLine("Fetching all workouts via API at {0}", DateTime.UtcNow);
            var workouts = await _workoutService.GetAllWorkoutsAsync();
            return Ok(workouts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Workout>> Get(string id)
        {
            Console.WriteLine("Fetching workout {0} via API at {1}", id, DateTime.UtcNow);
            var workout = await _workoutService.GetWorkoutByIdAsync(id);
            if (workout == null)
                return NotFound();

            return Ok(workout);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Workout workout)
        {
            Console.WriteLine("Creating workout via API: {0} at {1}", workout.Id, DateTime.UtcNow);
            await _workoutService.CreateWorkoutAsync(workout);
            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Workout workout)
        {
            if (id != workout.Id)
                return BadRequest();

            Console.WriteLine("Updating workout {0} via API at {1}", id, DateTime.UtcNow);
            await _workoutService.UpdateWorkoutAsync(workout);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Console.WriteLine("Deleting workout {0} via API at {1}", id, DateTime.UtcNow);
            var existingWorkout = await _workoutService.GetWorkoutByIdAsync(id);
            if (existingWorkout == null)
                return NotFound();

            await _workoutService.DeleteWorkoutAsync(id);
            return NoContent();
        }
    }
}