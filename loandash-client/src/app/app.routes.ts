import { Routes } from '@angular/router';
import { App } from './app';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
    },
    {  
        path: 'dashboard',
        loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard)
    },
    {
        path: 'loans',
        loadComponent: () => import('./pages/loans/loans').then(m => m.Loans)
    },
    {
        path: 'loans/:id',
        loadComponent: () => import('./pages/loan-detail/loan-detail').then(m => m.LoanDetailComponent)
    },
    {
        path: '**',
        redirectTo: 'dashboard'
    }

];
