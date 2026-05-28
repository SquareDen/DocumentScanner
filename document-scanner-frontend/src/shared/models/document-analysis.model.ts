export interface AnalysisRequest {
  file: File;
  userQuery: string;
}

export interface AnalysisResponse {
  question: string;
  extractedText: string;
  answer: string;
  success: boolean;
  errorMessage?: string;
}