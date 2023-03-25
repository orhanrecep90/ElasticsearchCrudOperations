using Nest;

namespace ElasticCrudPractices.Service.Model;

public class Customer:Document
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int Age { get; set; }
    public string Detail { get; set; }
    public Address Address { get; set; } = new Address();
}


public class Address:Document
{ 
    public string Street {get; set;}
    public string City {get; set;}
    public string State {get; set;}
    public string ZipCode {get; set;}
}

public abstract class Document
{
    public JoinField Join { get; set; }
}