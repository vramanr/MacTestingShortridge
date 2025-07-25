import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReportService } from '../services/report.service';

export interface CalibrationCertificateData {
  calNo: string;
  companyName: string;
  city: string;
  state: string;
  modelNumber: string;
  serialNumber: string;
  calibrationDate: string;
  testType: string;
  testBy: string;
  reportTitle: string;
  calibrationData: CalibrationDataPoint[];
}

export interface CalibrationDataPoint {
  setPoint: string;
  standardReading: string;
  testReading: string;
  allowedDeviation: string;
  actualDeviation: string;
  tolerance: string;
}

@Component({
  selector: 'app-calibration-certificate',
  templateUrl: './calibration-certificate.html',
  styleUrls: ['./calibration-certificate.scss']
})
export class CalibrationCertificate implements OnInit {
  certificateForm: FormGroup;
  isGenerating = false;
  certificateData: CalibrationCertificateData | null = null;
  previewMode = false;

  constructor(
    private fb: FormBuilder,
    private reportService: ReportService,
    private snackBar: MatSnackBar
  ) {
    this.certificateForm = this.fb.group({
      calNo: ['', [Validators.required, Validators.pattern(/^\d+$/)]]
    });
  }

  ngOnInit(): void {}

  async generateCertificate(): Promise<void> {
    if (this.certificateForm.invalid) {
      this.snackBar.open('Please enter a valid calibration number', 'Close', { duration: 3000 });
      return;
    }

    this.isGenerating = true;
    const calNo = parseInt(this.certificateForm.get('calNo')?.value);

    try {
      this.certificateData = await this.reportService.getCalibrationReportData(calNo);
      this.previewMode = true;
      this.snackBar.open('Certificate data loaded successfully', 'Close', { duration: 3000 });
    } catch (error) {
      console.error('Error generating certificate:', error);
      this.snackBar.open('Error loading certificate data', 'Close', { duration: 5000 });
    } finally {
      this.isGenerating = false;
    }
  }

  async downloadPdf(): Promise<void> {
    if (!this.certificateData) return;

    this.isGenerating = true;
    const calNo = parseInt(this.certificateForm.get('calNo')?.value);

    try {
      const pdfBlob = await this.reportService.generateCalibrationCertificatePdf(calNo);
      const url = window.URL.createObjectURL(pdfBlob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `Calibration_Certificate_${calNo}.pdf`;
      link.click();
      window.URL.revokeObjectURL(url);
      
      this.snackBar.open('Certificate downloaded successfully', 'Close', { duration: 3000 });
    } catch (error) {
      console.error('Error downloading certificate:', error);
      this.snackBar.open('Error downloading certificate', 'Close', { duration: 5000 });
    } finally {
      this.isGenerating = false;
    }
  }

  async printCertificate(): Promise<void> {
    if (!this.certificateData) return;

    try {
      window.print();
    } catch (error) {
      console.error('Error printing certificate:', error);
      this.snackBar.open('Error printing certificate', 'Close', { duration: 5000 });
    }
  }

  resetForm(): void {
    this.certificateForm.reset();
    this.certificateData = null;
    this.previewMode = false;
  }

  getPassFailStatus(dataPoint: CalibrationDataPoint): string {
    const allowedDev = parseFloat(dataPoint.allowedDeviation);
    const actualDev = Math.abs(parseFloat(dataPoint.actualDeviation));
    
    if (isNaN(allowedDev) || isNaN(actualDev)) return 'Unknown';
    
    return actualDev <= allowedDev ? 'PASS' : 'FAIL';
  }

  getStatusClass(dataPoint: CalibrationDataPoint): string {
    const status = this.getPassFailStatus(dataPoint);
    return status === 'PASS' ? 'status-pass' : 'status-fail';
  }
}
