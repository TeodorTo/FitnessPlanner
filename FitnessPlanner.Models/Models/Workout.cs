using System.Collections.Generic;

namespace FitnessPlanner.Models
{
    public class Workout
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}