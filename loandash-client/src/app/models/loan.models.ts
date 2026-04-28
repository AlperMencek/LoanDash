/*
    Loan models, reference api DTOs LoanDash.Application/DTOs
*/
export interface LoanListItem {
  loanId: number;
  borrowerFullName: string;
  principalAmount: number;
  outstandingBalance: number;
  interestRate: number;
  status: string;
  originationDate: string;
  maturityDate: string;
  totalPayments: number;
}

export interface PaymentDto {
  paymentId: number;
  paymentDate: string;
  amountPaid: number;
  principalPortion: number;
  interestPortion: number;
  remainingBalance: number;
}

export interface LoanDetail {
  loanId: number;
  principalAmount: number;
  outstandingBalance: number;
  interestRate: number;
  termMonths: number;
  originationDate: string;
  maturityDate: string;
  status: string;
  borrowerId: number;
  borrowerFullName: string;
  borrowerEmail: string;
  creditScore: number;
  payments: PaymentDto[];
}

export interface PortfolioSummary {
  totalOutstandingBalance: number;
  activeCount: number;
  delinquentCount: number;
  paidOffCount: number;
  defaultCount: number;
  weightedAvgInterestRate: number;
  delinquencyRate: number;
  monthlyPaymentVolume: MonthlyPaymentVolume[];
}

export interface MonthlyPaymentVolume {
  year: number;
  month: number;
  totalAmount: number;
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}