namespace LoanDash.Application.DTOs;
public class LoanDetailDTO
{
    public int LoanId { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal InterestRate { get; set; }
    public int TermMonths { get; set; }
    public DateTime OriginationDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public string Status { get; set; } = string.Empty;

    // Borrower info
    public int BorrowerId { get; set; }
    public string BorrowerFullName { get; set; } = string.Empty;
    public string BorrowerEmail { get; set; } = string.Empty;
    public int CreditScore { get; set; }

    // Payment history
    public List<PaymentDTO> Payments { get; set; } = new();
}