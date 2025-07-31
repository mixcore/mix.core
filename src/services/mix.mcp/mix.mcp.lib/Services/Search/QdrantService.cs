using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Qdrant.Client.Grpc.Conditions;

namespace Mix.MCP.Lib.Services.Search
{
    public class QdrantService
    {
        private readonly QdrantClient _client;
        private readonly string? _clusterId;
        private readonly ILogger<QdrantService> _logger;

        public QdrantService(IConfiguration configuration, ILogger<QdrantService> logger)
        {
            var endpoint = configuration["KnowledgeBase:VectorDb:Endpoint"];
            var apiKey = configuration["KnowledgeBase:VectorDb:ApiKey"];
            _clusterId = configuration["KnowledgeBase:VectorDb:ClusterId"];
            _logger = logger;

            //var channel = QdrantChannel.ForAddress(endpoint, new ClientConfiguration { ApiKey = apiKey });
            //var grpcClient = new QdrantGrpcClient(channel);
            _client = new QdrantClient(
              host: endpoint,
              https: true,
              apiKey: apiKey
            );
        }

        public async Task<List<SearchDocument>> GetAllDocumentsAsync()
        {
            var docs = new List<SearchDocument>();
            var collections = await _client.ListCollectionsAsync();

            foreach (var collection in collections)
            {
                var response = await _client.ScrollAsync(collection, null, 1000, null, true, false);
                foreach (var point in response.Result)
                {
                    if (point.Payload != null && point.Payload.TryGetValue("document", out var docPayload))
                    {
                        string? docJson = docPayload?.ToString();
                        if (!string.IsNullOrEmpty(docJson))
                        {
                            try
                            {
                                var doc = JsonSerializer.Deserialize<SearchDocument>(docJson);
                                if (doc != null)
                                    docs.Add(doc);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to deserialize SearchDocument from Qdrant payload.");
                            }
                        }
                    }
                }
            }

            return docs;
        }

        public async Task UpsertDocumentAsync(SearchDocument document, float[] vector)
        {
            var point = new PointStruct
            {
                Id = ulong.TryParse(document.Id, out var id) ? id : (ulong)document.Id.GetHashCode(),
                Vectors = vector,
                Payload = { ["document"] = JsonSerializer.Serialize(document) }
            };
            await _client.UpsertAsync(document.Collection, new[] { point });
        }

        public async Task RemoveDocumentAsync(string documentId)
        {
            var id = ulong.TryParse(documentId, out var uid) ? uid : (ulong)documentId.GetHashCode();
            await _client.DeleteAsync("mixcore", new[] { id });
        }

        public async Task<List<SearchResult>> SearchAsync(float[] queryVector, ulong limit = 10, ulong offset = 0)
        {
            var results = new List<SearchResult>();
            var response = await _client.SearchAsync("mixcore",

                queryVector,
                null, // filter
                null,
                limit, // limit
                0, // offset
                null, // searchParams
                null, // withPayload
                null  // withVectors
            );
            foreach (var point in response)
            {
                if (point.Payload != null && point.Payload.TryGetValue("document", out var docPayload))
                {
                    string? docJson = docPayload?.ToString();
                    if (!string.IsNullOrEmpty(docJson))
                    {
                        try
                        {
                            var doc = JsonSerializer.Deserialize<SearchDocument>(docJson);
                            if (doc != null)
                            {
                                results.Add(new SearchResult
                                {
                                    Id = doc.Id,
                                    Title = doc.Title,
                                    Content = doc.Content,
                                    Category = doc.Category,
                                    Source = doc.Source,
                                    Metadata = doc.Metadata,
                                    Score = point.Score,
                                    Snippet = doc.Content.Length > 150 ? doc.Content.Substring(0, 150) + "..." : doc.Content
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to deserialize SearchDocument from Qdrant payload.");
                        }
                    }
                }
            }
            return results;
        }

        public async Task UpsertCollectionAsync(string collectionName, uint shardNumber = 1, bool onDisk = true)
        {
            try
            {
                // Check if collection exists
                var collections = await _client.ListCollectionsAsync();
                if (!collections.Contains(collectionName))
                {
                    // Create collection if it does not exist
                    await _client.CreateCollectionAsync(collectionName, shardNumber: shardNumber, onDiskPayload: onDisk);
                    _logger.LogInformation("Created new Qdrant collection: {Collection}", collectionName);
                }
                else
                {
                    _logger.LogInformation("Qdrant collection already exists: {Collection}", collectionName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert Qdrant collection: {Collection}", collectionName);
                throw;
            }
        }
    }
}
