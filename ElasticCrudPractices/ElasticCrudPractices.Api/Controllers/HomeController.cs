using ElasticCrudPractices.Service.Dtos;
using ElasticCrudPractices.Service.Helper;
using ElasticCrudPractices.Service.Model;
using ElasticCrudPractices.Service.Service;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ElasticCrudPractices.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : Controller
{

    private readonly IElasticService elasticService;
    private readonly IFakeGenerator fakeGenerator;

    public HomeController(IElasticService elasticService, IFakeGenerator fakeGenerator)
    {
        this.elasticService = elasticService;
        this.fakeGenerator = fakeGenerator;
    }

    [HttpPost("insertall")]
    public async Task<IActionResult> InsertAllData()
    {
        var customers = fakeGenerator.GetCustomers(100000);
        await elasticService.InsertBulkDocuments("customers", customers);
        return Ok();
    }

    [HttpPost("createorupdate")]
    public async Task<IActionResult> Update(CustomerDto customerDto)
    {
        var customer = customerDto.Adapt<Customer>();
        await elasticService.InsertDocument("customers", customer);
        return Ok();
    }
    [HttpDelete("deletedocument")]
    public async Task<IActionResult> Delete(CustomerDto customerDto)
    {
        var customer = customerDto.Adapt<Customer>();
        await elasticService.DeleteDocumentById("customers", customer);
        return Ok();
    }
    [HttpDelete("deleteindex")]
    public async Task<IActionResult> Delete(string id)
    {
        await elasticService.DeleteIndex(id);
        return Ok();
    }
    [HttpGet("getdocument")]
    public async Task<IActionResult> Get(string id)
    {
       var customer= await elasticService.GetDocument("customers", id);
       var customerDto = customer.Adapt<CustomerDto>();
        return Ok(customerDto);
    }
    [HttpGet("getdocuments")]
    public async Task<IActionResult> Get()
    {
        var customers= await elasticService.GetDocuments("customers");
        var customerDto = customers.Adapt<List<CustomerDto>>();
        return Ok(customerDto);
    }
}