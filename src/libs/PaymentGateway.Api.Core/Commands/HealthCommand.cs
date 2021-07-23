using Api.Core.Commands;
using Nest;
using PaymentGateway.Api.Core.Data.Dtos;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Commands
{
    public class HealthCommand : ModelCommand<Health>
    {
        private readonly IElasticClient _client;

        public HealthCommand(IElasticClient client)
        {
            _client = client;
        }

        public override Task Validate(Health input)
        {
            return Task.CompletedTask;
        }

        public override async Task<Health> Run(Health input)
        {
            var health = await _client.Cluster.HealthAsync();

            return new Health
            {
                Status = health.Status.ToString(),
                Name = health.ClusterName,
                Timeout = health.TimedOut,
                Nodes = health.NumberOfNodes,
                WaitTime = health.TaskMaxWaitTimeInQueueInMilliseconds,
            };
        }
    }
}
