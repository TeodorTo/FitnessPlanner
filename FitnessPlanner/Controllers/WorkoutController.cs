using FitnessPlanner.BL.Services;
using FitnessPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public ActionResult<IEnumerable<Workout>> Get()
        {
            return Ok(_workoutService.GetAllWorkouts());
        }


        [HttpGet("{id}")]
        public ActionResult<Workout> Get(string id)
        {
            var workout = _workoutService.GetWorkoutById(id);
            if (workout == null)
                return NotFound();

            return Ok(workout);
        }


        [HttpPost]
        public ActionResult Post([FromBody] Workout workout)
        {
            _workoutService.CreateWorkout(workout);
            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }


        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Workout workout)
        {
            if (id != workout.Id)
                return BadRequest();

            _workoutService.UpdateWorkout(workout);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var existingWorkout = _workoutService.GetWorkoutById(id);
            if (existingWorkout == null)
                return NotFound();

            _workoutService.DeleteWorkout(id);
            return NoContent();
        }
    }
}