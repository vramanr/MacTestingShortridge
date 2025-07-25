import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, firstValueFrom } from 'rxjs';
const API_URL = 'https://localhost:7001';

export interface CalibrationReportData {
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

export interface StandardsReportData {
  name: string;
  sensor: string;
  model: string;
  serialNumber: string;
  calibrationDate: string;
  dueDate: string;
  range: string;
  units: string;
  accuracy: string;
  uncertainty: string;
  manufacturer: string;
  calibratedBy: string;
  calibrationInterval: string;
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = `${API_URL}/api/reports`;

  constructor(private http: HttpClient) {}

  async getCalibrationReportData(calNo: number): Promise<CalibrationReportData> {
    const response = this.http.get<CalibrationReportData>(`${this.apiUrl}/calibration-data/${calNo}`);
    return firstValueFrom(response);
  }

  async generateCalibrationCertificatePdf(calNo: number): Promise<Blob> {
    const response = this.http.get(`${this.apiUrl}/calibration-certificate/${calNo}`, {
      responseType: 'blob'
    });
    return firstValueFrom(response);
  }

  async getStandardsReportData(): Promise<StandardsReportData[]> {
    const response = this.http.get<StandardsReportData[]>(`${this.apiUrl}/standards-data`);
    return firstValueFrom(response);
  }

  async generateStandardsReportPdf(): Promise<Blob> {
    const response = this.http.get(`${this.apiUrl}/standards-report`, {
      responseType: 'blob'
    });
    return firstValueFrom(response);
  }

  async generateCoverSheetPdf(calNo: number): Promise<Blob> {
    const response = this.http.get(`${this.apiUrl}/cover-sheet/${calNo}`, {
      responseType: 'blob'
    });
    return firstValueFrom(response);
  }

  getMockCalibrationData(calNo: number): CalibrationReportData {
    return {
      calNo: calNo.toString(),
      companyName: 'ABC Manufacturing Corp',
      city: 'Phoenix',
      state: 'AZ',
      modelNumber: 'ADM-860C',
      serialNumber: 'SN123456',
      calibrationDate: new Date().toLocaleDateString(),
      testType: 'Standard Calibration Procedure',
      testBy: 'John Smith',
      reportTitle: 'AIRDATA MULTIMETER/FLOWMETER CERTIFICATE OF CALIBRATION',
      calibrationData: [
        {
          setPoint: '0.0000',
          standardReading: '0.0001',
          testReading: '0.0002',
          allowedDeviation: '0.0050',
          actualDeviation: '0.0001',
          tolerance: '±0.5%'
        },
        {
          setPoint: '25.0000',
          standardReading: '25.0012',
          testReading: '25.0015',
          allowedDeviation: '0.0250',
          actualDeviation: '0.0003',
          tolerance: '±0.1%'
        },
        {
          setPoint: '50.0000',
          standardReading: '50.0025',
          testReading: '50.0028',
          allowedDeviation: '0.0500',
          actualDeviation: '0.0003',
          tolerance: '±0.1%'
        },
        {
          setPoint: '75.0000',
          standardReading: '75.0038',
          testReading: '75.0041',
          allowedDeviation: '0.0750',
          actualDeviation: '0.0003',
          tolerance: '±0.1%'
        },
        {
          setPoint: '100.0000',
          standardReading: '100.0051',
          testReading: '100.0054',
          allowedDeviation: '0.1000',
          actualDeviation: '0.0003',
          tolerance: '±0.1%'
        }
      ]
    };
  }

  getMockStandardsData(): StandardsReportData[] {
    return [
      {
        name: 'Precision Voltage Standard',
        sensor: 'Voltage',
        model: '5720A',
        serialNumber: 'FL123456',
        calibrationDate: '01/15/2024',
        dueDate: '01/15/2025',
        range: '0-1000V',
        units: 'Volts',
        accuracy: '±0.001%',
        uncertainty: '±0.0005%',
        manufacturer: 'Fluke',
        calibratedBy: 'NIST',
        calibrationInterval: '12'
      },
      {
        name: 'Pressure Standard',
        sensor: 'Pressure',
        model: 'DPI611',
        serialNumber: 'DH789012',
        calibrationDate: '02/20/2024',
        dueDate: '02/20/2025',
        range: '0-300 PSI',
        units: 'PSI',
        accuracy: '±0.025%',
        uncertainty: '±0.01%',
        manufacturer: 'Druck',
        calibratedBy: 'Shortridge Cal Lab',
        calibrationInterval: '12'
      }
    ];
  }
}
