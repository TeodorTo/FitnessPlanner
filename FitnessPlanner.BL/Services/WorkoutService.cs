using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models;

namespace FitnessPlanner.BL.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public IEnumerable<Workout> GetAllWorkouts()
        {
            return _workoutRepository.GetAll();
        }

        public Workout GetWorkoutById(string id)
        {
            return _workoutRepository.GetById(id);
        }

        public void CreateWorkout(Workout workout)
        {
            _workoutRepository.Create(workout);
        }

        public void UpdateWorkout(Workout workout)
        {
            _workoutRepository.Update(workout);
        }

        public void DeleteWorkout(string id)
        {
            _workoutRepository.Delete(id);
        }
    }
}