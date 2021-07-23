using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using PaymentGateway.Api.Core.Extensions;

namespace PaymentGateway.Api.Core.Configurations
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchConfiguration =
                configuration.GetSection(ElasticSearchConfiguration.ElasticSearch);
            var endPoint = elasticSearchConfiguration[nameof(ElasticSearchConfiguration.EndPoint)];
            var index = elasticSearchConfiguration[nameof(ElasticSearchConfiguration.Index)];

            var node = new UriBuilder(endPoint);
            var client =
                new ElasticClient(new ConnectionSettings(node.Uri).DefaultIndex(index));

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
