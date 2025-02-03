using FitnessPlanner.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using FitnessPlanner.Models.Configurations;

namespace FitnessPlanner.DL.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly IMongoCollection<Workout> _workouts;

        public WorkoutRepository(IOptionsMonitor<MongoDbConfiguration> mongoConfig)
        {
            var client = new MongoClient(mongoConfig.CurrentValue.ConnectionString);
            var database = client.GetDatabase(mongoConfig.CurrentValue.DatabaseName);
            _workouts = database.GetCollection<Workout>("Workouts");  // Използваме Workouts колекция
        }

        public IEnumerable<Workout> GetAll()
        {
            return _workouts.Find(workout => true).ToList();
        }

        public Workout GetById(string id)
        {
            return _workouts.Find(workout => workout.Id == id).FirstOrDefault();
        }

        public void Create(Workout workout)
        {
            _workouts.InsertOne(workout);
        }

        public void Update(Workout workout)
        {
            var existing = _workouts.Find(w => w.Id == workout.Id).FirstOrDefault();
            if (existing != null)
            {
                _workouts.ReplaceOne(w => w.Id == workout.Id, workout);
            }
        }

        public void Delete(string id)
        {
            _workouts.DeleteOne(workout => workout.Id == id);
        }
    }
}