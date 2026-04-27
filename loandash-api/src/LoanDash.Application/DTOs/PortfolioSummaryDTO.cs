namespace LoanDash.Application.DTOs;
public class PortfolioSummaryDTO
{
    public decimal TotalOutstandingBalance { get; set; }
    public int ActiveCount { get; set; }
    public int DelinquentCount { get; set; }
    public int PaidOffCount { get; set; }
    public int DefaultCount { get; set; }
    public decimal WeightedAvgInterestRate { get; set; }
    public decimal DelinquencyRate { get; set; }
    public List<MonthlyPaymentVolumeDto> MonthlyPaymentVolume { get; set; } = new();
}

//payment volume for how much was paid
public class MonthlyPaymentVolumeDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalAmount { get; set; }
}
