using FitnessPlanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPlanner.DL.Repositories
{


    public interface IWorkoutRepository
    {
        Task<IEnumerable<Workout>> GetAllAsync();
        Task<Workout> GetByIdAsync(string id);
        Task CreateAsync(Workout workout);
        Task UpdateAsync(Workout workout);
        Task DeleteAsync(string id);
    }

}