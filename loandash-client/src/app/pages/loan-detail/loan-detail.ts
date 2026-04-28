import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoanService } from '../../services/loan.service';
import { LoanDetail } from '../../models/loan.models';

@Component({
  selector: 'app-loan-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatChipsModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './loan-detail.html',
  styleUrl: './loan-detail.scss'
})
export class LoanDetailComponent implements OnInit {
  loan: LoanDetail | null = null;
  loading = true;
  notFound = false;

  paymentColumns = [
    'paymentDate',
    'amountPaid',
    'principalPortion',
    'interestPortion',
    'remainingBalance'
  ];

  constructor(
    private route: ActivatedRoute,
    private loanService: LoanService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.notFound = true;
      this.loading = false;
      this.cdr.markForCheck();
      return;
    }

    this.loanService.getLoanById(+id).subscribe({
      next: (data) => {
        this.loan = data;
        this.loading = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Failed to load loan', err);
        this.notFound = true;
        this.loading = false;
        this.cdr.markForCheck();
      }
    });
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