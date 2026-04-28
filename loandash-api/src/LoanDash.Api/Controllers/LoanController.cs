/*
    * LoanController - handles endpoints under 
    route api/loan for loan related endpoints
    - GET /api/loan?page=1&pageSize=10&status=Active - get paginated list of loans with optional status filter
    returns list of loans with basic info (id, borrower name, amount, interest rate, status) and total count for pagination
    - GET /api/loan/{id} - get loan details by id
    returns loan details including payment history and borrower info
    
*/
using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
using LoanDash.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanDash.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;
    public LoanController(ILoanService loanService)    {
        _loanService = loanService;
    }   

    [HttpGet]
    public async Task<IActionResult> GetLoans([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] LoanStatus? status = null)
    {
        var (loans, totalCount) = await _loanService.GetLoansAsync(page, pageSize, status);
        return Ok(new { data = loans, totalCount, page, pageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanById(int id)
    {
        var loan = await _loanService.GetLoanByIdAsync(id);
        if (loan == null) return NotFound();
        return Ok(loan);    
    }
}