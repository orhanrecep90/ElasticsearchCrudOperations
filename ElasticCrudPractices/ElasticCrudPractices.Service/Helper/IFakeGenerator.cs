using ElasticCrudPractices.Service.Model;

namespace ElasticCrudPractices.Service.Helper;

public interface IFakeGenerator
{
    List<Customer> GetCustomers(int size);
}