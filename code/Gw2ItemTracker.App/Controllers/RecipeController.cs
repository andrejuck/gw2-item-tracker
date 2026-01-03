using Gw2ItemTracker.App.Application;
using Gw2ItemTracker.App.Views;
using Libs.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("recipe")]
public class RecipeController : Controller
{
    private readonly IRecipeApplication _application;

    public RecipeController(IRecipeApplication application)
    {
        _application = application;
    }

    [HttpGet("{recipeId:int}")]
    public async Task<IActionResult> GetItemByIdAsync(int recipeId)
    {
        var view = await _application.GetByIdAsync(recipeId);
        //test
        if (view is null) return NoContent();
        
        return Ok(view);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] PagedRequest pagedRequest, [FromQuery] string? searchString)
    {
        PagedResponse<RecipeView> pagedResponse = await _application.GetPagedAsync(pagedRequest, searchString);
        return Ok(pagedResponse);
    }
}