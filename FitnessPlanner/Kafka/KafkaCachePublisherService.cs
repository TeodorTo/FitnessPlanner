using Confluent.Kafka;
using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace FitnessPlanner.Kafka
{
    public class KafkaCachePublisherService : BackgroundService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly ILogger<KafkaCachePublisherService> _logger;
        private readonly IProducer<Null, string> _producer;

        public KafkaCachePublisherService(
            IWorkoutRepository workoutRepository,
            ILogger<KafkaCachePublisherService> logger)
        {
            _workoutRepository = workoutRepository;
            _logger = logger;

            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workouts = await _workoutRepository.GetAllAsync();

                foreach (var workout in workouts)
                {
                    var message = JsonSerializer.Serialize(workout);
                    await _producer.ProduceAsync("workout-cache", new Message<Null, string> { Value = message }, stoppingToken);
                    _logger.LogInformation("Published workout to Kafka: {Name}", workout.Name);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        public override void Dispose()
        {
            _producer?.Dispose();
            base.Dispose();
        }
    }
}