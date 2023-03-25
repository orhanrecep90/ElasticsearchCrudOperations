namespace ElasticCrudPractices.Service.Dtos;


public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int Age { get; set; }
    public string Detail { get; set; }
    public AddressDto Address { get; set; } = new AddressDto();
}


public class AddressDto
{ 
    public string Street {get; set;}
    public string City {get; set;}
    public string State {get; set;}
    public string ZipCode {get; set;}
}