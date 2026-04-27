using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
using LoanDash.Application.DTOs;
namespace LoanDash.Application.Services.Interfaces;

public interface ILoanService
{
    Task<(IEnumerable<LoanListItemDTO> , int totalCount)> GetLoansAsync(int page, int pageSize, LoanStatus? status = null);
    Task<LoanDetailDTO?> GetLoanByIdAsync(int loanId);

}