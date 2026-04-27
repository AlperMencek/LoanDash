namespace LoanDash.Application.DTOs;
public class PortfolioSummaryData
{
    public decimal TotalOutstandingBalance { get; set; }
    public int ActiveCount { get; set; }
    public int DelinquentCount { get; set; }
    public int PaidOffCount { get; set; }
    public int DefaultCount { get; set; }
    public decimal WeightedAvgInterestRate { get; set; }
    public decimal DelinquencyRate { get; set; }
    public List<MonthlyPaymentVolumeDTO> MonthlyPaymentVolume { get; set; } = new();
}

//payment volume for how much was paid
public class MonthlyPaymentVolumeDTO
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalAmount { get; set; }
}
