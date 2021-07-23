using Elasticsearch.Net;
using Logging.Interfaces;
using Nest;
using PaymentGateway.Api.Core.Data.Model;
using PaymentGateway.Api.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Data.Service
{
    public class PaymentRecordDataService : IPaymentRecordDataService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<PaymentRecordDataService> _logger;


        public PaymentRecordDataService(IElasticClient elasticClient, ILogger<PaymentRecordDataService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(PaymentRecord paymentRecord)
        {
            try
            {
                var response = await _elasticClient.IndexDocumentAsync<PaymentRecord>(paymentRecord);

                return response.Result == Result.Created || response.Result == Result.Updated;
            }
            catch (ElasticsearchClientException ex)
            {
                _logger.LogError($"Debug infomration: {ex.DebugInformation}, Failure reason {ex.FailureReason}, Reqest data {ex.Request}, Response Data: {ex.Response}", ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Couldnt perform operation in elastic search ", ex);
                throw;
            }
        }


        public async Task<PaymentRecord> GetByIdAsync(string paymentRecordId)
        {
            try
            {
                var boolQuery = new BoolQuery()
                {
                    Filter = new List<QueryContainer>()
                    {
                        new TermQuery()
                        {
                            Field = $"{nameof(PaymentRecord.PaymentRecordId).ToCamelCase()}.keyword",
                            Value = paymentRecordId,
                            Name = nameof(PaymentRecord.PaymentRecordId).ToCamelCase()

                        }
                    }

                };

                var result = await _elasticClient.SearchAsync<PaymentRecord>(s => s
                    .Query(q => q.Bool(b => b.Filter(boolQuery))));


                if (!result.IsValid)
                {
                    throw new ElasticsearchClientException(result.DebugInformation);
                }

                _logger.LogDebug($"Result Total from ES: {result.Total}");

                _logger.LogDebug(
                    $"Request was successful, it took {result.Took} and API request : {result.ApiCall}");

                return result.Documents.Any() ? result.Documents.First() : null;
            }
            catch (ElasticsearchClientException ex)
            {
                _logger.LogError(
                    $"Debug information: {ex.DebugInformation}, Failure reason {ex.FailureReason}, " +
                    $"Request data {ex.Request}, Response Data: {ex.Response}",
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Couldn't perform operation in elastic search", ex);
            }

            return null;
        }

    }
}
