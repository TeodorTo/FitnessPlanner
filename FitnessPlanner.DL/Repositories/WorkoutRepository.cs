using FitnessPlanner.Models;
using FitnessPlanner.Models.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPlanner.DL.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly IMongoCollection<Workout> _workouts;

        public WorkoutRepository(IOptionsMonitor<MongoDbConfiguration> mongoConfig)
        {
            var client = new MongoClient(mongoConfig.CurrentValue.ConnectionString);
            var database = client.GetDatabase(mongoConfig.CurrentValue.DatabaseName);
            _workouts = database.GetCollection<Workout>("Workouts");
        }

        public async Task<IEnumerable<Workout>> GetAllAsync()
        {
            Console.WriteLine("Fetching all workouts from MongoDB at {0}", DateTime.UtcNow);
            return await _workouts.Find(workout => true).ToListAsync();
        }

        public async Task<Workout> GetByIdAsync(string id)
        {
            return await _workouts.Find(workout => workout.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Workout workout)
        {
            if (string.IsNullOrEmpty(workout.Id))
            {
                workout.Id = Guid.NewGuid().ToString();
            }
            workout.LastModified = DateTime.UtcNow;
            Console.WriteLine("Creating workout {0} with LastModified {1}", workout.Id, workout.LastModified);
            await _workouts.InsertOneAsync(workout);
        }

        public async Task UpdateAsync(Workout workout)
        {
            var existingWorkout = await GetByIdAsync(workout.Id);
            if (existingWorkout == null)
            {
                throw new Exception($"Workout with Id {workout.Id} not found");
            }

            // Пропусни актуализация, ако няма промени
            if (existingWorkout.Name == workout.Name && 
                existingWorkout.Exercises.SequenceEqual(workout.Exercises))
            {
                Console.WriteLine("No changes detected for workout {0}, skipping update", workout.Id);
                return;
            }

            workout.LastModified = DateTime.UtcNow;
            Console.WriteLine("Updating workout {0} with LastModified {1}", workout.Id, workout.LastModified);
            await _workouts.ReplaceOneAsync(w => w.Id == workout.Id, workout);
        }

        public async Task DeleteAsync(string id)
        {
            Console.WriteLine("Deleting workout {0}", id);
            await _workouts.DeleteOneAsync(workout => workout.Id == id);
        }
    }
}