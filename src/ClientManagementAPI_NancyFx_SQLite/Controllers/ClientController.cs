using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Domain.Dtos;
using Domain.Entities;

namespace ClientManagementAPI_NancyFx_SQLite.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ClientDto clientDto)
    {
        await _clientService.CreateAsync(clientDto);

        return Created("Client created", clientDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _clientService.GetAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok(await _clientService.GetByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateByIdAsync(int id, ClientDto clientDto)
    {
        await _clientService.UpdateByIdAsync(id, clientDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _clientService.DeleteAsync(id);

        return Ok();
    }

    [HttpGet("/history")]
    public async Task<IActionResult> GetHistoryAsync()
    {
        return Ok(await _clientService.GetHistoryAsync());
    }
}