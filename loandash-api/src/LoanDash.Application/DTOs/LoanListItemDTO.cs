namespace LoanDash.Application.DTOs;
public class LoanListItemDto
{
    public int LoanId { get; set; }
    public string BorrowerFullName { get; set; } = string.Empty;
    public decimal PrincipalAmount { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal InterestRate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OriginationDate { get; set; }
    public DateTime MaturityDate { get; set; }
}