using System.Text.Json;
using Gw2ItemTracker.App.Application;
using Gw2ItemTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("account")]
public class AccountController : Controller
{
    private readonly IAccountApplication _accountApplication;

    public AccountController(IAccountApplication accountApplication)
    {
        _accountApplication = accountApplication;
    }

    [HttpGet("materials")]
    public async Task<IActionResult> GetMaterialsAsync([FromHeader(Name = "gw2-api-key")]  string? apiKey)
    {
        if (string.IsNullOrEmpty(apiKey)) 
            return Unauthorized();

        var materials = await _accountApplication.GetAccountMaterialsAsync(apiKey);
        return Ok(materials);
    }
    [HttpGet("characters")]
    public async Task<IActionResult> GetAllCharactersAsync([FromHeader(Name = "gw2-api-key")]  string? apiKey)
    {
        if (string.IsNullOrEmpty(apiKey)) 
            return Unauthorized();
        
        var characters = await _accountApplication.GetAllCharactersAsync(apiKey);
        return Ok(characters);
    }
    
    [HttpGet("characters/{id}")]
    public async Task<IActionResult> GetAllCharactersAsync(
        [FromHeader(Name = "gw2-api-key")]  string? apiKey,
        string id)
    {
        if (string.IsNullOrEmpty(apiKey)) 
            return Unauthorized();
        
        var character = await _accountApplication.GetCharacterByIdAsync(id, apiKey);
        return Ok(character);
    }
}