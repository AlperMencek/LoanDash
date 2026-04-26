/*
    Loan.cs 
    represents the borrowers loan. A loan belongs to one borrower only, but a borrower can have multiple loans.
    
    - LoanId: Unique identifier for the loan.
    - BorrowerId: Foreign key linking to the borrower.
    - PrincipalAmount: The original amount of the loan.
    - InterestRate: The annual interest rate applied to the loan.
    - TermMonths: The duration of the loan in months.
    - OriginationDate: The date when the loan was issued.
    - MaturityDate: The date when the loan is due to be fully repaid.
    - Status: The current status of the loan (reference enums/LoanStatus enum).
    - OutstandingBalance: The remaining balance on the loan.
    - Borrower: Navigation property linking to the borrower entity (navigation property).
    - Payments: Collection/list of payments made towards the loan (navigation property). 
*/
using LoanDash.Domain.Enums;
namespace LoanDash.Domain.Entities;

public class Loan
{
    public int LoanId {get; set;}
    // Foreign key to Borrower
    public int BorrowerId {get; set;}
    public decimal PrincipalAmount {get; set;}
    public decimal InterestRate {get; set;}

    public int TermMonths {get; set;}

    public DateTime OriginationDate {get; set;}
    public DateTime MaturityDate {get; set;}
    //LoanStatus from enums 
    public  LoanStatus Status {get; set;}

    public decimal OutstandingBalance {get; set;}
    // navigation property
    public Borrower Borrower {get; set;} = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

}