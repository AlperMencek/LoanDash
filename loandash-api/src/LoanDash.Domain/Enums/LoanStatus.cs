/*
    LoanStatus enums defining states of loans, 
    Starting from 1 for data integrity.
*/
namespace LoanDash.Domain.Enums
{
    public enum LoanStatus
    {
        Active = 1,
        Delinquent = 2,
        PaidOff = 3,
        Defaulted = 4
       
    }
}