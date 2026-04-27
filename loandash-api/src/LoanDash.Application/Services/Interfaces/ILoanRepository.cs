using LoanDash.Application.DTOs;
using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;

namespace LoanDash.Application.Services.Interfaces;

public interface ILoanRepository
{
    Task<(IEnumerable<Loan> Loans, int TotalCount)> GetLoansAsync(
        int page, int pageSize, LoanStatus? status = null);
    Task<Loan?> GetLoanWithPaymentsAsync(int loanId);
    Task<PortfolioSummaryData> GetPortfolioSummaryAsync();
}