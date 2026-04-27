/*
    LoanRepository.cs
    Implements the ILoanRepository interface, providing methods to interact with the Loan data in the database.
    Complex queries specific to loans handled here.
*/
using LoanDash.Application.DTOs;
using LoanDash.Application.Services.Interfaces;
using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
using LoanDash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace LoanDash.Infrastructure.Repositories;

public class LoanRepository : BaseRepository<Loan>, ILoanRepository
{
    public LoanRepository(LoanDashDbContext context) : base(context)
    {
    }
    //get paginated list of loans with optional filter on status
    public async Task<(IEnumerable<Loan>, int)> GetLoansAsync(int page, int pageSize, LoanStatus? status = null)
    {   //base query with borrower included 
        var query = _context.Loans
        .Include(l => l.Borrower).AsQueryable(); // ef join

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status.Value);
        }
        
        var totalCount = await query.CountAsync();
        //pagination with sorting by origination date (most recent first)
        var loans = await query.OrderByDescending(l => l.OriginationDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize).ToListAsync();

        return (loans, totalCount);
    }

    //get single loan with payment history
    public async Task<Loan?> GetLoanWithPaymentsAsync(int loanId)
    {
        return await _context.Loans
        .Include(l => l.Borrower)
        .Include(l => l.Payments.OrderByDescending(p => p.PaymentDate))
        .FirstOrDefaultAsync(l => l.LoanId == loanId);
    }

    // Get portfolio summary data for the dashboard
    public async Task<PortfolioSummaryData> GetPortfolioSummaryAsync()
    {
        var loans = await _context.Loans.ToListAsync();

        var totalBalance     = loans.Sum(l => l.OutstandingBalance);
        var activeLoans      = loans.Count(l => l.Status == LoanStatus.Active);
        var delinquentLoans  = loans.Count(l => l.Status == LoanStatus.Delinquent);
        var paidOffLoans     = loans.Count(l => l.Status == LoanStatus.PaidOff);
        var defaultedLoans   = loans.Count(l => l.Status == LoanStatus.Defaulted);
        var totalLoans       = loans.Count;

        var activeWithRate = loans.Where(l =>
            l.Status == LoanStatus.Active && l.OutstandingBalance > 0).ToList();

        var weightedAvgRate = activeWithRate.Any()
            ? activeWithRate.Sum(l => l.InterestRate * l.OutstandingBalance)
              / activeWithRate.Sum(l => l.OutstandingBalance)
            : 0;

        var delinquencyRate = totalLoans > 0
            ? (decimal)delinquentLoans / totalLoans * 100
            : 0;

        // mont over month payment volume for last 6 months calc
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        var monthlyPayments = await _context.Payments
            .Where(p => p.PaymentDate >= sixMonthsAgo)
            .GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
            .Select(g => new MonthlyPaymentVolumeDTO
            {
                Year        = g.Key.Year,
                Month       = g.Key.Month,
                TotalAmount = g.Sum(p => p.AmountPaid)
            })
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .ToListAsync();

        return new PortfolioSummaryData
        {
            TotalOutstandingBalance = totalBalance,
            ActiveCount             = activeLoans,
            DelinquentCount         = delinquentLoans,
            PaidOffCount            = paidOffLoans,
            DefaultCount            = defaultedLoans,
            WeightedAvgInterestRate = Math.Round(weightedAvgRate, 4),
            DelinquencyRate         = Math.Round(delinquencyRate, 2),
            MonthlyPaymentVolume    = monthlyPayments
        };
    }


    
}