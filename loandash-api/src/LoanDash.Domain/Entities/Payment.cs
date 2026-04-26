
/*
    Payment.cs 
    Defines the payment made towards a loan,
    Payment belongs to one loan only.
    
    - PaymentId: Unique identifier for the payment.
    - LoanId: Foreign key linking to the loan.
    - PaymentDate: The date when the payment was made.
    - AmountPaid: The total amount paid in this transaction.
    - PrincipalPortion: The portion of the payment that goes towards the principal.
    - InterestPortion: The portion of the payment that goes towards interest.
    - RemainingBalance: The remaining balance on the loan after this payment.
    - Loan: Navigation property linking to the loan entity (navigation property).
*/
namespace LoanDash.Domain.Entities;

public class Payment
{
    public int PaymentId {get; set;}
    public int LoanId {get; set;}
    public DateTime PaymentDate {get; set;}
    public decimal AmountPaid {get; set;}
    public decimal PrincipalPortion {get; set;}
    public decimal InterestPortion {get; set;}
    public decimal RemainingBalance {get; set;}
    // navigation property
    public Loan Loan {get; set;} = null!;   

}