/*
    DataSeeder.cs
    Seeds the database with initial data for testing and development.
    This is where we can generate random borrowers, loans, and payments to populate the database.
*/  

using  LoanDash.Domain.Entities;
using LoanDash.Domain.Enums;
namespace LoanDash.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task Seed(LoanDashDbContext context)
    {
        //Check if Borrowers exist, if not, seed 
        if (!context.Borrowers.Any())
        {
            var random = new Random(42);
            //sample data to generate random borrowers
            var firstNames = new[] { "John", "Jane", "Michael",
             "Emily", "David", "Sarah", "Robert", "Jessica", "William", "Ashley",
             "James", "Amanda", "Joseph", "Jennifer", "Charles", "Elizabeth" 
            };
            var lastNames = new[] { "Smith", "Johnson", "Brown",
             "Taylor", "Anderson", "Thomas", "Jackson", "White",
             "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson", "Clark", "Rodriguez"}; 
            //Generate 50 randome borrowers
            var borrowers = Enumerable.Range(1, 50).Select(i => new Borrower
            {
                FirstName = firstNames[random.Next(firstNames.Length)],
                LastName = lastNames[random.Next(lastNames.Length)],
                Email = $"borrower{i}@email.com",
                CreditScore = random.Next(300, 850),
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 365))
            }).ToList();
        await context.Borrowers.AddRangeAsync(borrowers);
        await context.SaveChangesAsync();

        //Generate Loans
        //weighted statuses for a realistic distribution 
        var Statuses = new [] { LoanStatus.Active, LoanStatus.Active, LoanStatus.Active, LoanStatus.Active,
         LoanStatus.PaidOff, LoanStatus.Delinquent, LoanStatus.Delinquent,
          LoanStatus.Defaulted };
        var loans = borrowers.Select(borrower => new Loan
        {
            BorrowerId = borrower.BorrowerId,
            PrincipalAmount = Math.Round((decimal)(random.Next(5000, 75000)), 2),// $5,000 to $75,000
            InterestRate = Math.Round((decimal)(random.Next(350, 1200)) / 10000, 4),// 3.5% to 12% interest rate
            OutstandingBalance = 0,
            TermMonths =new[] { 36, 60, 84, 120, 180, 360 }[random.Next(6)],//months 
            Status = Statuses[random.Next(Statuses.Length)],
            OriginationDate = DateTime.UtcNow.AddDays(-random.Next(0, 365))

        }).ToList();
        //Calculate outstanding balance and maturity date for each loan
        foreach (var loan in loans)        {
            loan.MaturityDate = loan.OriginationDate.AddMonths(loan.TermMonths);

            var monthsElapsed = (int)((DateTime.UtcNow - loan.OriginationDate).TotalDays / 30);
            var payoffFraction = (decimal)monthsElapsed / loan.TermMonths;
            loan.OutstandingBalance = loan.Status == LoanStatus.PaidOff
            ? 0: Math.Round(loan.PrincipalAmount * (1 - payoffFraction * 0.6m), 2); // assume 60% of principal is paid off over time for active loans

        }
        await context.Loans.AddRangeAsync(loans);
        await context.SaveChangesAsync();
        // Clear tracked entities so EF doesn't try to set identity IDs on payments
        context.ChangeTracker.Clear();

        //Generate Payments 
        var payments = new List<Payment>();
        foreach (var loan in loans){
            var monthsElapsed = (int)((DateTime.UtcNow - loan.OriginationDate).TotalDays / 30);
            // For delinquent loans, we assume they have missed 1-3 payments, for defaulted loans we assume they have missed 1-4 payments, for active loans we assume they are up to date with payments
            var paymentCount = loan.Status == LoanStatus.Delinquent
                ? Math.Max(1, monthsElapsed - random.Next(1, 3))
                : loan.Status == LoanStatus.Defaulted
                    ? random.Next(1, 4)
                    : monthsElapsed; 

            var monthlyPayment = Math.Round(
                loan.PrincipalAmount / loan.TermMonths * 1.15m, 2);

            var balance = loan.PrincipalAmount;

            for (int m = 0; m < paymentCount; m++)
            {
                var interestPortion  = Math.Round(balance * (loan.InterestRate / 12), 2);
                var principalPortion = Math.Round(monthlyPayment - interestPortion, 2);

                if (principalPortion > balance) principalPortion = balance;
                balance = Math.Max(0, balance - principalPortion);

                payments.Add(new Payment
                {
                    LoanId           = loan.LoanId,
                    PaymentDate      = loan.OriginationDate.AddMonths(m + 1),
                    AmountPaid       = monthlyPayment,
                    PrincipalPortion = principalPortion,
                    InterestPortion  = interestPortion,
                    RemainingBalance = balance
                });
                }

        
            }
        await context.Payments.AddRangeAsync(payments);
        await context.SaveChangesAsync();
        } 
    }
}
