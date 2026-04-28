/*
    PortfolioController.cs
    API controller for portfolio related endpoints.
    - GET /api/portfolio - get portfolio summary data for dashboard
*/

using LoanDash.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanDash.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;
    public PortfolioController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }  

    [HttpGet]
    public async Task<IActionResult> GetPortfolioSummary()
    {
        var summary = await _portfolioService.GetPortfolioSummaryAsync();
        return Ok(summary);
    }
}