import { Routes } from '@angular/router';
import { ProcessorPageComponent } from '../features/document-processor/pages/processor-page.component';

export const routes: Routes = [
  {
    path: '',
    component: ProcessorPageComponent
  },
  {
    path: '**',
    redirectTo: ''
  }
];