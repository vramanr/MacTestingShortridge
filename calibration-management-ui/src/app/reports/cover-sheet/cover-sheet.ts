import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReportService } from '../services/report.service';

export interface CoverSheetData {
  calNo: string;
  companyName: string;
  city: string;
  state: string;
  modelNumber: string;
  serialNumber: string;
  calibrationDate: string;
  reportTitle: string;
}

@Component({
  selector: 'app-cover-sheet',
  templateUrl: './cover-sheet.html',
  styleUrls: ['./cover-sheet.scss']
})
export class CoverSheet implements OnInit {
  coverSheetForm: FormGroup;
  isGenerating = false;
  coverSheetData: CoverSheetData | null = null;
  previewMode = false;

  constructor(
    private fb: FormBuilder,
    private reportService: ReportService,
    private snackBar: MatSnackBar
  ) {
    this.coverSheetForm = this.fb.group({
      calNo: ['', [Validators.required, Validators.pattern(/^\d+$/)]]
    });
  }

  ngOnInit(): void {}

  async generateCoverSheet(): Promise<void> {
    if (this.coverSheetForm.invalid) {
      this.snackBar.open('Please enter a valid calibration number', 'Close', { duration: 3000 });
      return;
    }

    this.isGenerating = true;
    const calNo = parseInt(this.coverSheetForm.get('calNo')?.value);

    try {
      const reportData = await this.reportService.getCalibrationReportData(calNo);
      this.coverSheetData = {
        calNo: reportData.calNo,
        companyName: reportData.companyName,
        city: reportData.city,
        state: reportData.state,
        modelNumber: reportData.modelNumber,
        serialNumber: reportData.serialNumber,
        calibrationDate: reportData.calibrationDate,
        reportTitle: 'Calibration Report'
      };
      this.previewMode = true;
      this.snackBar.open('Cover sheet data loaded successfully', 'Close', { duration: 3000 });
    } catch (error) {
      console.error('Error generating cover sheet:', error);
      this.snackBar.open('Error loading cover sheet data', 'Close', { duration: 5000 });
    } finally {
      this.isGenerating = false;
    }
  }

  async downloadPdf(): Promise<void> {
    if (!this.coverSheetData) return;

    this.isGenerating = true;
    const calNo = parseInt(this.coverSheetForm.get('calNo')?.value);

    try {
      const pdfBlob = await this.reportService.generateCoverSheetPdf(calNo);
      const url = window.URL.createObjectURL(pdfBlob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `Cover_Sheet_${calNo}.pdf`;
      link.click();
      window.URL.revokeObjectURL(url);
      
      this.snackBar.open('Cover sheet downloaded successfully', 'Close', { duration: 3000 });
    } catch (error) {
      console.error('Error downloading cover sheet:', error);
      this.snackBar.open('Error downloading cover sheet', 'Close', { duration: 5000 });
    } finally {
      this.isGenerating = false;
    }
  }

  async printCoverSheet(): Promise<void> {
    try {
      window.print();
    } catch (error) {
      console.error('Error printing cover sheet:', error);
      this.snackBar.open('Error printing cover sheet', 'Close', { duration: 5000 });
    }
  }

  resetForm(): void {
    this.coverSheetForm.reset();
    this.coverSheetData = null;
    this.previewMode = false;
  }
}
