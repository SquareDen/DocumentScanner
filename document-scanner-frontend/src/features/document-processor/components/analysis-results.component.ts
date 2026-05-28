import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnalysisResponse } from '../../../shared/models/document-analysis.model';

@Component({
  selector: 'app-analysis-results',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './analysis-results.component.html',
  styleUrls: ['./analysis-results.component.scss']
})
export class AnalysisResultsComponent {
  @Input() result: AnalysisResponse | null = null;
  @Input() isLoading: boolean = false;
  @Output() newAnalysis = new EventEmitter<void>();

  onStartNewAnalysis(): void {
    this.newAnalysis.emit();
  }

  expandedSection: 'question' | 'text' | 'answer' | null = null;

  toggleSection(section: 'question' | 'text' | 'answer'): void {
    this.expandedSection = this.expandedSection === section ? null : section;
  }
}