using FitnessPlanner.Models;
using System.Collections.Generic;

namespace FitnessPlanner.DL.Repositories
{
    public interface IWorkoutRepository
    {
        IEnumerable<Workout> GetAll();
        Workout GetById(string id);
        void Create(Workout workout);
        void Update(Workout workout);
        void Delete(string id);
    }
}