using FitnessPlanner.Models;

namespace FitnessPlanner.BL.Services
{
    public interface IWorkoutService
    {
        IEnumerable<Workout> GetAllWorkouts();
        Workout GetWorkoutById(string id);
        void CreateWorkout(Workout workout);
        void UpdateWorkout(Workout workout);
        void DeleteWorkout(string id);
    }
}