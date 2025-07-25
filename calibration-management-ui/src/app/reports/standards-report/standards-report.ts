import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReportService, StandardsReportData } from '../services/report.service';

@Component({
  selector: 'app-standards-report',
  templateUrl: './standards-report.html',
  styleUrls: ['./standards-report.scss']
})
export class StandardsReport implements OnInit {
  isGenerating = false;
  standardsData: StandardsReportData[] = [];
  previewMode = false;

  displayedColumns: string[] = [
    'name', 'sensor', 'model', 'serialNumber', 'calibrationDate', 
    'dueDate', 'range', 'accuracy', 'uncertainty', 'manufacturer', 
    'calibratedBy', 'calibrationInterval'
  ];

  constructor(
    private reportService: ReportService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {}

  async generateReport(): Promise<void> {
    this.isGenerating = true;

    try {
      this.standardsData = this.reportService.getMockStandardsData();
      this.previewMode = true;
      this.snackBar.open('Standards report data loaded successfully', 'Close', { duration: 3000 });
    } catch (error) {
      console.error('Error generating standards report:', error);
      this.snackBar.open('Error loading standards report data', 'Close', { duration: 5000 });
    } finally {
      this.isGenerating = false;
    }
  }

  async downloadPdf(): Promise<void> {
    this.isGenerating = true;

    try {
      const pdfBlob = await this.reportService.generateStandardsReportPdf();
      const url = window.URL.createObjectURL(pdfBlob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `Calibration_Standards_Report_${new Date().toISOString().split('T')[0]}.pdf`;
      link.click();
      window.URL.revokeObjectURL(url);
      
      this.snackBar.open('Standards report downloaded successfully', 'Close', { duration: 3000 });
    } catch (error) {
      console.error('Error downloading standards report:', error);
      this.snackBar.open('Error downloading standards report', 'Close', { duration: 5000 });
    } finally {
      this.isGenerating = false;
    }
  }

  async printReport(): Promise<void> {
    try {
      window.print();
    } catch (error) {
      console.error('Error printing standards report:', error);
      this.snackBar.open('Error printing standards report', 'Close', { duration: 5000 });
    }
  }

  resetReport(): void {
    this.standardsData = [];
    this.previewMode = false;
  }

  getStatusClass(dueDate: string): string {
    const due = new Date(dueDate);
    const today = new Date();
    const daysUntilDue = Math.ceil((due.getTime() - today.getTime()) / (1000 * 3600 * 24));
    
    if (daysUntilDue < 0) return 'status-overdue';
    if (daysUntilDue <= 30) return 'status-due-soon';
    return 'status-current';
  }

  getStatusText(dueDate: string): string {
    const due = new Date(dueDate);
    const today = new Date();
    const daysUntilDue = Math.ceil((due.getTime() - today.getTime()) / (1000 * 3600 * 24));
    
    if (daysUntilDue < 0) return 'OVERDUE';
    if (daysUntilDue <= 30) return 'DUE SOON';
    return 'CURRENT';
  }

  getDueSoonCount(): number {
    return this.standardsData.filter(s => this.getStatusClass(s.dueDate) === 'status-due-soon').length;
  }

  getOverdueCount(): number {
    return this.standardsData.filter(s => this.getStatusClass(s.dueDate) === 'status-overdue').length;
  }
}
