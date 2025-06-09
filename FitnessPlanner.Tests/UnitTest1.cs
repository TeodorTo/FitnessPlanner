using Xunit;
using Moq;
using FitnessPlanner.BL.Services;
using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task GetAllWorkoutsAsync_Returns_AllWorkouts()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout { Id = "1", Name = "Morning Workout" },
                new Workout { Id = "2", Name = "Evening Workout" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(workouts);

            // Act
            var result = await _workoutService.GetAllWorkoutsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_Returns_CorrectWorkout()
        {
            var workout = new Workout { Id = "1", Name = "Morning Workout" };
            _mockRepo.Setup(repo => repo.GetByIdAsync("1")).ReturnsAsync(workout);

            var result = await _workoutService.GetWorkoutByIdAsync("1");

            Assert.NotNull(result);
            Assert.Equal("Morning Workout", result.Name);
        }

        [Fact]
        public async Task CreateWorkoutAsync_Calls_CreateAsync()
        {
            var workout = new Workout { Id = "1", Name = "New Workout" };

            await _workoutService.CreateWorkoutAsync(workout);

            _mockRepo.Verify(repo => repo.CreateAsync(workout), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutAsync_Calls_UpdateAsync()
        {
            var workout = new Workout { Id = "1", Name = "Updated Workout" };

            await _workoutService.UpdateWorkoutAsync(workout);

            _mockRepo.Verify(repo => repo.UpdateAsync(workout), Times.Once);
        }

        [Fact]
        public async Task DeleteWorkoutAsync_Calls_DeleteAsync()
        {
            var workout = new Workout { Id = "1", Name = "Workout to Remove" };

            await _workoutService.DeleteWorkoutAsync("1");

            _mockRepo.Verify(repo => repo.DeleteAsync("1"), Times.Once);
        }
    }
}
