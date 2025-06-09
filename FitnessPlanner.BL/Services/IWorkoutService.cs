using FitnessPlanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPlanner.BL.Services
{
    public interface IWorkoutService
    {
        Task<IEnumerable<Workout>> GetAllWorkoutsAsync();
        Task<Workout> GetWorkoutByIdAsync(string id);
        Task CreateWorkoutAsync(Workout workout);
        Task UpdateWorkoutAsync(Workout workout);
        Task DeleteWorkoutAsync(string id);
    }
}