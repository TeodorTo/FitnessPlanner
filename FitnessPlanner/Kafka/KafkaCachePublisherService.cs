using Confluent.Kafka;
using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessPlanner.Kafka
{
    public class KafkaCachePublisherService : BackgroundService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly ILogger<KafkaCachePublisherService> _logger;
        private readonly IProducer<Null, string> _producer;
        private readonly ConcurrentDictionary<string, DateTime> _lastPublished = new();

        public KafkaCachePublisherService(
            IWorkoutRepository workoutRepository,
            ILogger<KafkaCachePublisherService> logger)
        {
            _workoutRepository = workoutRepository;
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                EnableIdempotence = true,
                Acks = Acks.All
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workouts = await _workoutRepository.GetAllAsync();
                foreach (var workout in workouts)
                {
                    if (!_lastPublished.TryGetValue(workout.Id, out var lastPublishedTime) || 
                        workout.LastModified.Ticks > lastPublishedTime.Ticks)
                    {
                        var message = JsonSerializer.Serialize(workout);
                        await _producer.ProduceAsync("workout-cache", new Message<Null, string> { Value = message }, stoppingToken);
                        _logger.LogInformation("Published workout to Kafka: {Name}", workout.Name);
                        _lastPublished.AddOrUpdate(workout.Id, workout.LastModified, (key, old) => workout.LastModified);
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public override void Dispose()
        {
            _producer?.Dispose();
            base.Dispose();
        }

        public async Task PublishWorkoutAsync(Workout workout, CancellationToken cancellationToken = default)
        {
            var message = JsonSerializer.Serialize(workout);
            await _producer.ProduceAsync("workout-cache", new Message<Null, string> { Value = message }, cancellationToken);
            _logger.LogInformation("Published workout to Kafka: {Name}", workout.Name);
            _lastPublished.AddOrUpdate(workout.Id, workout.LastModified, (key, old) => workout.LastModified);
        }
    }
}