using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
using LoanDash.Application.DTOs;

namespace LoanDash.Application.Services.Interfaces;

public interface IPortfolioService
{
    Task<PortfolioSummaryDTO> GetPortfolioSummaryAsync();
}