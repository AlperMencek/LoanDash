using LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
using LoanDash.Application.DTOs;
using LoanDash.Application.Services.Interfaces;

namespace LoanDash.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly ILoanRepository _loanRepository;
    public PortfolioService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }
    //Get portfolio summary data from repository (db to DTO mapping)
    public async Task<PortfolioSummaryData> GetPortfolioSummaryAsync()
    {
        var data = await _loanRepository.GetPortfolioSummaryAsync();

        return new PortfolioSummaryData
        {
            TotalOutstandingBalance   = data.TotalOutstandingBalance,
            ActiveCount              = data.ActiveCount,
            DelinquentCount         = data.DelinquentCount,
            PaidOffCount            = data.PaidOffCount,
            DefaultCount            = data.DefaultCount,
            WeightedAvgInterestRate = data.WeightedAvgInterestRate,
            DelinquencyRate         = data.DelinquencyRate,
            MonthlyPaymentVolume    = data.MonthlyPaymentVolume.Select(m => new MonthlyPaymentVolumeDTO
            {
                Year        = m.Year,
                Month       = m.Month,
                TotalAmount = m.TotalAmount
            }).ToList()
        };
    }
}