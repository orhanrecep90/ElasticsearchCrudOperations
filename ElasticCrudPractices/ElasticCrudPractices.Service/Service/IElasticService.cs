using ElasticCrudPractices.Service.Model;

namespace ElasticCrudPractices.Service.Service;

public interface IElasticService
{
    Task CheckIndex(string indexName);
    Task InsertDocument(string indexName, Customer customer);
    Task DeleteIndex(string indexName);
    Task DeleteDocumentById(string indexName, Customer customer);
    Task InsertBulkDocuments(string indexName, List<Customer> customers);
    Task<Customer> GetDocument(string indexName, string id);
    Task<List<Customer>> GetDocuments(string indexName);


}