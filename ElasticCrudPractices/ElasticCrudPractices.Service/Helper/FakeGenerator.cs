using Bogus;
using ElasticCrudPractices.Service.Model;

namespace ElasticCrudPractices.Service.Helper;

public class FakeGenerator:IFakeGenerator
{
    public List<Customer> GetCustomers(int size)
    {
        try
        {

            var addressFaker = new Faker<Address>()
                .RuleFor(c => c.City, f => f.Person.Address.City)
                .RuleFor(c => c.State, f => f.Person.Address.State)
                .RuleFor(c => c.Street, f => f.Person.Address.Street)
                .RuleFor(c => c.ZipCode, f => f.Person.Address.ZipCode);

            var customerFaker = new Faker<Customer>()
                .RuleFor(c => c.Id, f => f.IndexGlobal)
                .RuleFor(c => c.Name, f => f.Person.FirstName)
                .RuleFor(c => c.SurName, f => f.Person.LastName)
                .RuleFor(c => c.Age, f => f.Person.Random.Short(18, 50))
                .RuleFor(c => c.Address, f => addressFaker.Generate())
                .RuleFor(c => c.Detail, f => f.Lorem.Sentences(10));

            return customerFaker.Generate(size);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    } 
}