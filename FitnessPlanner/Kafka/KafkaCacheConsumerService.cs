using Confluent.Kafka;
using FitnessPlanner.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FitnessPlanner.Kafka
{
    public class KafkaCacheConsumerService : BackgroundService
    {
        private readonly ILogger<KafkaCacheConsumerService> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ConcurrentDictionary<string, Workout> _cache = new();

        public KafkaCacheConsumerService(ILogger<KafkaCacheConsumerService> logger)
        {
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "workout-cache-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false 
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe("workout-cache");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);
                        if (consumeResult != null)
                        {
                            _logger.LogInformation("Received message: {Message}, Partition: {Partition}, Offset: {Offset}",
                                consumeResult.Message.Value, consumeResult.Partition, consumeResult.Offset);
                            var workout = JsonSerializer.Deserialize<Workout>(consumeResult.Message.Value);
                            if (workout != null)
                            {
                                _cache.AddOrUpdate(workout.Id, workout, (key, old) => workout);
                                _consumer.Commit(consumeResult); 
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                   
                }
                finally
                {
                    _consumer.Close();
                }
            }, stoppingToken);
        }

        public override void Dispose()
        {
            _consumer?.Dispose();
            base.Dispose();
        }

        public Task<Workout?> GetWorkoutFromCacheAsync(string id)
        {
            _cache.TryGetValue(id, out var workout);
            return Task.FromResult(workout);
        }

        public Task<IEnumerable<Workout>> GetAllCachedWorkoutsAsync()
        {
            return Task.FromResult<IEnumerable<Workout>>(_cache.Values);
        }
    }
}