using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
using LoanDash.Application.DTOs;
using LoanDash.Application.Services.Interfaces;

namespace LoanDash.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    public LoanService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }
    //Get paginated list of loans optional filter by status from repository (db to DTO mapping)
    public async Task<(IEnumerable<LoanListItemDTO>, int totalCount)> GetLoansAsync(int page, 
    int pageSize, LoanStatus? status = null)
    {
        var (loans, totalCount) = await _loanRepository.GetLoansAsync(page, pageSize, status);
          var dtos = loans.Select(l => new LoanListItemDTO
        {
            LoanId             = l.LoanId,
            BorrowerFullName   = $"{l.Borrower.FirstName} {l.Borrower.LastName}",
            PrincipalAmount    = l.PrincipalAmount,
            OutstandingBalance = l.OutstandingBalance,
            InterestRate       = l.InterestRate,
            Status             = l.Status.ToString(),
            OriginationDate    = l.OriginationDate,
            MaturityDate       = l.MaturityDate
        });
        return (dtos, totalCount);
    }

    //Get loan details with loanId from repository (db to DTO mapping)
    public async Task<LoanDetailDTO?> GetLoanByIdAsync(int loanId)
    {
        var loan = await _loanRepository.GetLoanWithPaymentsAsync(loanId);

        if (loan == null) return null;

        return new LoanDetailDTO
        {
            LoanId             = loan.LoanId,
            PrincipalAmount    = loan.PrincipalAmount,
            OutstandingBalance = loan.OutstandingBalance,
            InterestRate       = loan.InterestRate,
            TermMonths         = loan.TermMonths,
            OriginationDate    = loan.OriginationDate,
            MaturityDate       = loan.MaturityDate,
            Status             = loan.Status.ToString(),
            BorrowerId         = loan.Borrower.BorrowerId,
            BorrowerFullName   = $"{loan.Borrower.FirstName} {loan.Borrower.LastName}",
            BorrowerEmail      = loan.Borrower.Email,
            CreditScore        = loan.Borrower.CreditScore,
            Payments           = loan.Payments.Select(p => new PaymentDTO
            {
                PaymentId        = p.PaymentId,
                PaymentDate      = p.PaymentDate,
                AmountPaid       = p.AmountPaid,
                PrincipalPortion = p.PrincipalPortion,
                InterestPortion  = p.InterestPortion,
                RemainingBalance = p.RemainingBalance
            }).ToList()
        };
    }
}