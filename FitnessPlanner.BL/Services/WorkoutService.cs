using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("Creating workout via service: {0} at {1}", workout.Id, DateTime.UtcNow);
            await _workoutRepository.CreateAsync(workout);
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            var existingWorkout = await _workoutRepository.GetByIdAsync(workout.Id);
            if (existingWorkout == null)
            {
                throw new Exception($"Workout with Id {workout.Id} not found");
            }

            // Пропусни актуализация, ако няма промени
            if (existingWorkout.Name == workout.Name && 
                existingWorkout.Exercises.SequenceEqual(workout.Exercises, new ExerciseEqualityComparer()))
            {
                Console.WriteLine("No changes detected for workout {0}, skipping update", workout.Id);
                return;
            }

            Console.WriteLine("Updating workout via service: {0} at {1}", workout.Id, DateTime.UtcNow);
            await _workoutRepository.UpdateAsync(workout);
        }

        public async Task DeleteWorkoutAsync(string id)
        {
            Console.WriteLine("Deleting workout via service: {0} at {1}", id, DateTime.UtcNow);
            await _workoutRepository.DeleteAsync(id);
        }
    }

    // Помощен клас за сравнение на Exercises
    public class ExerciseEqualityComparer : IEqualityComparer<Exercise>
    {
        public bool Equals(Exercise x, Exercise y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Id == y.Id && x.Name == y.Name && x.MuscleGroup == y.MuscleGroup && x.Duration == y.Duration;
        }

        public int GetHashCode(Exercise obj)
        {
            return HashCode.Combine(obj.Id, obj.Name, obj.MuscleGroup, obj.Duration);
        }
    }
}