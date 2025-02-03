using Xunit;
using Moq;
using FitnessPlanner.BL.Services;
using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitnessPlanner.Tests
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IWorkoutRepository> _mockRepo;
        private readonly IWorkoutService _workoutService;

        public WorkoutServiceTests()
        {
            _mockRepo = new Mock<IWorkoutRepository>();
            _workoutService = new WorkoutService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllWorkouts_Returns_AllWorkouts()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout { Id = "1", Name = "Morning Workout" },
                new Workout { Id = "2", Name = "Evening Workout" }
            };
            _mockRepo.Setup(repo => repo.GetAll()).Returns(workouts);

            // Act
            var result = _workoutService.GetAllWorkouts();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetWorkoutById_Returns_CorrectWorkout()
        {

            var workout = new Workout { Id = "1", Name = "Morning Workout" };
            _mockRepo.Setup(repo => repo.GetById("1")).Returns(workout);


            var result = _workoutService.GetWorkoutById("1");


            Assert.NotNull(result);
            Assert.Equal("Morning Workout", result.Name);
        }

        [Fact]
        public void AddWorkout_Calls_InsertMethod()
        {

            var workout = new Workout { Id = "1", Name = "New Workout" };


            _workoutService.CreateWorkout(workout);


            _mockRepo.Verify(repo => repo.Create(workout), Times.Once); 
        }

        [Fact]
        public void UpdateWorkout_Calls_UpdateMethod()
        {

            var workout = new Workout { Id = "1", Name = "Updated Workout" };
            _mockRepo.Setup(repo => repo.GetById("1")).Returns(workout);


            _workoutService.UpdateWorkout(workout);


            _mockRepo.Verify(repo => repo.Update(workout), Times.Once);
        }

        [Fact]
        public void RemoveWorkout_Calls_DeleteMethod()
        {

            var workout = new Workout { Id = "1", Name = "Workout to Remove" };
            _mockRepo.Setup(repo => repo.GetById("1")).Returns(workout);


            _workoutService.DeleteWorkout("1");


            _mockRepo.Verify(repo => repo.Delete("1"), Times.Once);
        }
    }
}
