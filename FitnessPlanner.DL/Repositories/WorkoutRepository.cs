using FitnessPlanner.Models;
using FitnessPlanner.Models.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
            return await _workouts.Find(workout => true).ToListAsync();
        }

        public async Task<Workout> GetByIdAsync(string id)
        {
            return await _workouts.Find(workout => workout.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Workout workout)
        {
            await _workouts.InsertOneAsync(workout);
        }

        public async Task UpdateAsync(Workout workout)
        {
            await _workouts.ReplaceOneAsync(w => w.Id == workout.Id, workout);
        }

        public async Task DeleteAsync(string id)
        {
            await _workouts.DeleteOneAsync(workout => workout.Id == id);
        }
    }
}