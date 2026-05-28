import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DocumentInputComponent } from '../components/document-input.component';
import { AnalysisResultsComponent } from '../components/analysis-results.component';
import { DocumentAnalysisService } from '../../../core/services/document-analysis.service';
import { AnalysisRequest, AnalysisResponse } from '../../../shared/models/document-analysis.model';

@Component({
  selector: 'app-processor-page',
  standalone: true,
  imports: [CommonModule, DocumentInputComponent, AnalysisResultsComponent],
  templateUrl: './processor-page.component.html',
  styleUrls: ['./processor-page.component.scss']
})
export class ProcessorPageComponent implements OnInit, OnDestroy {
  private analysisService = inject(DocumentAnalysisService);
  private destroy$ = new Subject<void>();

  analysisResult: AnalysisResponse | null = null;
  isLoading: boolean = false;
  apiError: string = '';

  ngOnInit(): void {
    // Component initialization
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onFileSelected(request: AnalysisRequest): void {
    this.isLoading = true;
    this.apiError = '';
    this.analysisResult = null;
    console.log('🔄 Starting analysis...', request);

    this.analysisService.analyze(request.file, request.userQuery)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response: AnalysisResponse) => {
          console.log('✅ Response received:', response);
          this.analysisResult = response;
          this.isLoading = false;
          console.log('isLoading set to:', this.isLoading);
        },
        error: (error: any) => {
          console.error('❌ API Error:', error);
          this.isLoading = false;
          this.apiError = 'Connection error. Make sure API is running on http://localhost:5056';
        },
        complete: () => {
          console.log('✔️ Observable completed');
        }
      });
  }

  onNewAnalysis(): void {
    this.analysisResult = null;
    this.apiError = '';
  }
}