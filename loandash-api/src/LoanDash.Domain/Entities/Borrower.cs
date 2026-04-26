/*
    Borrower.cs
    Borrower entity, defines the main properties of a customer within the system.
    Borrower can have multiple loans, but each loan belongs to one borrower only.
    
    - BorrowerId: Unique identifier for the borrower.
    - FirstName: Borrower's first name.
    - LastName: Borrower's last name.
    - Email: Contact email for the borrower.
    - CreatedAt: Timestamp of when the borrower was created in the system.
    - Loans: Collection of loans associated with the borrower (navigation property).
*/

namespace LoanDash.Domain.Entities;

public class Borrower
{
    public string BorrowerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>(); 
}
