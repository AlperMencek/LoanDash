import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { LoanService } from '../../services/loan.service';
import { LoanListItem } from '../../models/loan.models';
import { RouterLink } from '@angular/router';
import { error } from 'console';
@Component({
  selector: 'app-loans',
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSelectModule,
    MatFormFieldModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    RouterLink
  ],
  templateUrl: './loans.html',
  styleUrl: './loans.scss',
})
export class Loans implements OnInit {
  displayedColumns: string[] = [  'borrowerFullName',
  'principalAmount',
  'outstandingBalance',
  'interestRate',
  'status',
  'originationDate',
  'totalPayments',
  'actions'];
  loans: LoanListItem[] = [];
  totalLoans = 0;
  pageSize = 10;
  pageIndex = 1;
  loading = false;
  selectedStatus = '';
  statuses = ['', 'Active', 'Delinquent', 'PaidOff', 'Defaulted'];

  constructor(private loanService: LoanService) {}

  ngOnInit() {
    this.loadLoans();
  }

  loadLoans() {
    this.loading = true;
    this.loanService.getLoans(this.pageIndex , this.pageSize, this.selectedStatus).subscribe(response => {
      this.loans = response.data;
      this.totalLoans = response.totalCount;
      this.loading = false;
    }, error => {
      console.error('Error loading loans', error);
      this.loading = false;
    });
  }


  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex+1;
    this.pageSize = event.pageSize;
    this.loadLoans();
  }

  onStatusChange(status: string) {
    this.selectedStatus = status;
    this.pageIndex = 1
    this.loadLoans();
  }

  //Get status
  getStatusClass(status: string): string {
    switch (status) {
      case 'Active':
        return 'status-active';
      case 'Delinquent':
        return 'status-delinquent';
      case 'PaidOff':
        return 'status-paidoff';
      case 'Defaulted':
        return 'status-defaulted';
      default:
        return '';
    }


  }
}