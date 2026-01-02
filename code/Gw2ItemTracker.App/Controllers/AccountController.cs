using System.Text.Json;
using Gw2ItemTracker.App.Application;
using Gw2ItemTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : Controller
{
    private readonly IAccountApplication _accountApplication;
    private string? _apiKey;

    public AccountController(IAccountApplication accountApplication)
    {
        _accountApplication = accountApplication;
    }

    [HttpGet("materials")]
    public async Task<IActionResult> GetMaterialsAsync()
    {
        if (!ApiKeyHeaderExists()) 
            return Unauthorized();

        var materials = await _accountApplication.GetAccountMaterialsAsync(_apiKey);
        return Ok(materials);
    }
    [HttpGet("characters")]
    public async Task<IActionResult> GetAllCharactersAsync()
    {
        if (!ApiKeyHeaderExists()) 
            return Unauthorized();
        
        var characters = await _accountApplication.GetAllCharactersAsync(_apiKey);
        return Ok(characters);
    }
    
    [HttpGet("characters/{id}")]
    public async Task<IActionResult> GetAllCharactersAsync(string id)
    {
        if (!ApiKeyHeaderExists()) 
            return Unauthorized();
        
        var character = await _accountApplication.GetCharacterByIdAsync(id, _apiKey);
        return Ok(character);
    }
    
    private bool ApiKeyHeaderExists()
    {
        _apiKey = HttpContext.Request.Headers["gw2-api-key"].FirstOrDefault();

        if(_apiKey is null)
        {
            return false;
        }

        return true;
    }

}