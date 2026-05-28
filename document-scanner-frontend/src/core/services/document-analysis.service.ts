import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnalysisResponse } from '../../shared/models/document-analysis.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DocumentAnalysisService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/ContractAnalysis`;

  analyze(file: File, question: string): Observable<AnalysisResponse> {
    const formData = new FormData();
    formData.append('contractPhoto', file);
    formData.append('userQuestion', question);

    return this.http.post<AnalysisResponse>(this.apiUrl, formData);
  }
}