import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AnalysisRequest } from '../../../shared/models/document-analysis.model';

@Component({
  selector: 'app-document-input',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './document-input.component.html',
  styleUrls: ['./document-input.component.scss']
})
export class DocumentInputComponent {
  @Output() fileSelected = new EventEmitter<AnalysisRequest>();

  selectedFile: File | null = null;
  userQuery: string = '';
  fileName: string = '';
  fileError: string = '';

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const files = input.files;

    if (files && files.length > 0) {
      const file = files[0];
      const validTypes = ['image/jpeg', 'image/png', 'image/webp', 'application/pdf'];

      if (!validTypes.includes(file.type)) {
        this.fileError = 'Only JPEG, PNG, WebP and PDF files are supported';
        this.selectedFile = null;
        this.fileName = '';
        return;
      }

      if (file.size > 5 * 1024 * 1024) { // 5MB
        this.fileError = 'File size must not exceed 5MB';
        this.selectedFile = null;
        this.fileName = '';
        return;
      }

      this.selectedFile = file;
      this.fileName = file.name;
      this.fileError = '';
    }
  }

  onSubmit(): void {
    if (!this.selectedFile || !this.userQuery.trim()) {
      if (!this.selectedFile) {
        this.fileError = 'Please select a file';
      }
      return;
    }

    const request: AnalysisRequest = {
      file: this.selectedFile,
      userQuery: this.userQuery.trim()
    };

    this.fileSelected.emit(request);
    this.resetForm();
  }

  resetForm(): void {
    this.selectedFile = null;
    this.userQuery = '';
    this.fileName = '';
    this.fileError = '';
  }

  removeFile(): void {
    this.selectedFile = null;
    this.fileName = '';
    this.fileError = '';
  }
}