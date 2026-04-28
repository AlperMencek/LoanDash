/*
  loan.service.ts
  service for connecting to the backend API to fetch loan data and portfolio summary
  -getLoans: fetches a paginated list of loans with optional status filter
  -getLoanById: fetches detailed information for a specific loan by ID
  -getPortfolioSummary: fetches summary statistics for the loan portfolio
  returns observables based on the models defined in loan.models.ts
*/
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  LoanListItem,
  LoanDetail,
  PortfolioSummary,
  PaginatedResponse
} from '../models/loan.models';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}
  
  getLoans(page: number = 1, pageSize: number = 10, status?: string): Observable<PaginatedResponse<LoanListItem>> {
    // Build query parameters for pagination and optional status filter
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    // If a status filter is provided, add it to the query parameters
    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<PaginatedResponse<LoanListItem>>(`${this.apiUrl}/loans`, { params });
  }

  getLoanById(id: number): Observable<LoanDetail> {
    return this.http.get<LoanDetail>(`${this.apiUrl}/loans/${id}`);
  }

  getPortfolioSummary(): Observable<PortfolioSummary> {
    return this.http.get<PortfolioSummary>(`${this.apiUrl}/portfolio/summary`);
  }
}