using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPlanner.BL.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<IEnumerable<Workout>> GetAllWorkoutsAsync()
        {
            return await _workoutRepository.GetAllAsync();
        }

        public async Task<Workout> GetWorkoutByIdAsync(string id)
        {
            return await _workoutRepository.GetByIdAsync(id);
        }

        public async Task CreateWorkoutAsync(Workout workout)
        {
            await _workoutRepository.CreateAsync(workout);
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            await _workoutRepository.UpdateAsync(workout);
        }

        public async Task DeleteWorkoutAsync(string id)
        {
            await _workoutRepository.DeleteAsync(id);
        }
    }
}