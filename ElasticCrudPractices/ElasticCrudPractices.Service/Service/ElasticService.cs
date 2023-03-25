
using ElasticCrudPractices.Service.Configurations;
using Microsoft.Extensions.Configuration;
using ElasticCrudPractices.Service.Model;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticCrudPractices.Service.Service;

public class ElasticService:IElasticService
{
    private readonly IElasticClient client;
    public ElasticService(IOptions<ElasticsearchSettings> elasticsearchSettings)
    {
        var settings = new ConnectionSettings(new Uri( elasticsearchSettings.Value.Host + ":" + elasticsearchSettings.Value.Port));
        client = new ElasticClient(settings); 
    }
    public async Task CheckIndex(string indexName)
    {
        var any = client.Indices.ExistsAsync(indexName);
        if (any.Result.Exists)
            return;

        var response = await client.Indices.CreateAsync(indexName, c =>
            c.Map<Document>(m => m
                .AutoMap<Customer>()
                .AutoMap(typeof(Address))));
    }

    public async Task InsertDocument(string indexName, Customer customer)
    {
        var response = await client.CreateAsync(customer, q => q.Index(indexName));
        if (response.ApiCall?.HttpStatusCode==409)
        {
            await client.UpdateAsync<Customer>(customer.Id, q => q.Index(indexName).Doc(customer));
        }
    }

    public async Task DeleteIndex(string indexName)
    {
        await client.Indices.DeleteAsync(indexName);
    }

    public async Task DeleteDocumentById(string indexName, Customer customer)
    {
        var response = await client.CreateAsync(customer, q => q.Index(indexName));
        if (response.ApiCall?.HttpStatusCode==409)
        {
            await client.DeleteAsync(DocumentPath<Customer>.Id(customer.Id).Index(indexName));
        }
    }

    public async Task InsertBulkDocuments(string indexName, List<Customer> customers)
    {
        await client.IndexManyAsync(customers, index:indexName);
    }

    public async Task<Customer> GetDocument(string indexName, string id)
    {
        var response = client.GetAsync<Customer>(id, q => q.Index(indexName));
        return response.Result.Source;
    }
    public async Task<List<Customer>> GetDocuments(string indexName)
    {

        #region wildcard fills the character
        // var response = await client.SearchAsync<Customer>(s => s
        //     .From(0)
        //     .Take(10)
        //     .Index(indexName)
        //     .Query(q => q
        //         .Bool(b => b
        //             .Should(s => s
        //                 .Wildcard(w => w
        //                     .Field("name")
        //                     .Value("b*ll"))))));

        #endregion
        
        #region wildcard fills the character
        // var response = await client.SearchAsync<Customer>(s => s
        //     .Index(indexName)
        //     .Query(q => q
        //         .Fuzzy(f=>f
        //             .Field("name")
        //             .Value("bil").Fuzziness(Fuzziness.EditDistance(4)))));
        
        // var response = await client.SearchAsync<Customer>(s => s
        //     .Index(indexName)
        //     .Query(q => q
        //         .Fuzzy(f=>f
        //             .Field("name")
        //             .Value("bll").Transpositions(true))));
        #endregion
        
        #region MatchPhrasePrefix finds the starth with, faster than wildcard
        // var response = await client.SearchAsync<Customer>(s => s
        //     .Index(indexName)
        //     .Query(q => q
        //         .MatchPhrasePrefix(f=>f
        //             .Field("name")
        //             .Query("bi").MaxExpansions(20))));
        #endregion
        
        #region Multimatch query in multi field
        // var response = await client.SearchAsync<Customer>(s => s
        //     .Index(indexName)
        //     .Query(q => q
        //         .MultiMatch(mm=>mm
        //             .Fields(f=>f
        //                 .Field("name")
        //                 .Field("surName"))
        //             .Operator(Operator.Or)
        //             .Type(TextQueryType.Phrase)
        //             .Query("bill")
        //             .MaxExpansions(10))));
        #endregion
        
        #region Term case sensitive
        // var response = await client.SearchAsync<Customer>(s => s
        //     .Index(indexName)
        //     .Size(1000)
        //     .Query(q => q
        //         .Term(t=>t.
        //             Field(f=>f.Name).Value("bill"))));
        #endregion
        
        #region Match not case sensitive
        // var response = await client.SearchAsync<Customer>(s => s
        //     .Index(indexName)
        //     .Size(1000)
        //     .Query(q => q
        //         .Match(t=>t.
        //             Field(f=>f.Name).Query("BILL"))));
        #endregion
        
        #region AnalyzeWildcard fills characters
        var response = await client.SearchAsync<Customer>(s => s
            .Index(indexName)
            .Query(q => q
                .QueryString(qs=>qs
                    .AnalyzeWildcard()
                    .Query("*il*")
                    .Fields(fs=>fs.Field(f=>f.Name)))));
        #endregion
        
        return response.Documents.ToList();
    }
}