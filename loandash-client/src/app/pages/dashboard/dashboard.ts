import { CommonModule } from '@angular/common';
import {MatCardModule} from '@angular/material/card';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { BaseChartDirective } from 'ng2-charts';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LoanService } from '../../services/loan.service';
import { PortfolioSummary } from '../../models/loan.models';
import { ChartConfiguration, ChartData, plugins } from 'chart.js';
import {
  Chart,
  CategoryScale,
  LinearScale,
  BarController,
  BarElement,
  LineController,
  LineElement,
  PointElement,
  Filler,
  Tooltip,
  Legend
} from 'chart.js';

Chart.register(
  CategoryScale,
  LinearScale,
  BarController,
  BarElement,
  LineController,
  LineElement,
  PointElement,
  Filler,
  Tooltip,
  Legend
);
@Component({
  selector: 'app-dashboard',
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    BaseChartDirective
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})

 export class Dashboard implements OnInit {
  loading = true;
  summary:PortfolioSummary|null = null;

  //barchart data
  barChartData: ChartData<'bar'> = {
    labels: ['Active', 'Delinquent', 'Paid Off', 'Defaulted'],
    datasets: [
      {
        label: 'Loans by status',
        data: [],
        backgroundColor: ['#42A5F5', '#FFA726', '#66BB6A', '#EF5350']
      }
    ]
  };
  barChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        display: false
      }
    }
  };  
  //linechart data
  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{
      data: [],
        label: 'Monthly Payment Volume',
        fill: true,
        backgroundColor: 'rgba(66, 165, 245, 0.2)',
        tension: 0.4,
        borderColor: '#42A5F5'
      }
    ]
  }
  lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: { legend: { display: false } }
  };

  constructor(private loanService: LoanService,
      private cdr: ChangeDetectorRef
    
  ) {}
  ngOnInit(): void {
    this.loanService.getPortfolioSummary().subscribe({
      next: (data) => {
        setTimeout(() => {
        this.summary = data;
        this.updateCharts(data);
        this.loading = false;
      },0);
      },
      error: (e) => {
        console.error('Failed to load portfolio summary', e);
        this.loading = false;
      }
    });

    
  }
    updateCharts(data: PortfolioSummary): void {
    this.barChartData = {
      ...this.barChartData,
      datasets: [{
        ...this.barChartData.datasets[0],
        data: [data.activeCount, data.delinquentCount, data.paidOffCount, data.defaultCount]
      }]
    };

    this.lineChartData = {
      labels: data.monthlyPaymentVolume.map(m =>
        new Date(m.year, m.month - 1).toLocaleString('default', { month: 'short', year: '2-digit' })
      ),
      datasets: [{
        ...this.lineChartData.datasets[0],
        data: data.monthlyPaymentVolume.map(m => m.totalAmount)
      }]
    };
    this.cdr.markForCheck();
    }
 }
