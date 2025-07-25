import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';

interface StandardReport {
  id: string;
  title: string;
  description: string;
  icon: string;
  iconColor: string;
  lastGenerated: Date;
  recordCount: number;
}

interface ScheduledReport {
  id: string;
  name: string;
  frequency: string;
  nextRun: Date;
  recipients: string[];
  status: 'Active' | 'Paused' | 'Error';
}

interface Company {
  coId: string;
  coName: string;
}

interface ReportColumn {
  key: string;
  label: string;
  selected: boolean;
}

@Component({
  selector: 'app-reports-dashboard',
  imports: [
    ReactiveFormsModule,
    FormsModule,
    MatCardModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatTooltipModule,
    MatChipsModule,
    MatCheckboxModule,
    CommonModule
  ],
  templateUrl: './reports-dashboard.html',
  styleUrl: './reports-dashboard.scss'
})
export class ReportsDashboard implements OnInit {
  customReportForm: FormGroup;
  standardReports: StandardReport[] = [];
  scheduledReports: ScheduledReport[] = [];
  availableColumns: ReportColumn[] = [];

  scheduledColumns: string[] = ['name', 'frequency', 'nextRun', 'recipients', 'status', 'actions'];

  companies: Company[] = [
    { coId: 'ACME', coName: 'Acme Corporation' },
    { coId: 'TECH', coName: 'TechCorp Industries' },
    { coId: 'PREC', coName: 'Precision Instruments LLC' },
    { coId: 'METRO', coName: 'Metro Calibration Services' },
    { coId: 'QUAL', coName: 'Quality Assurance Labs' }
  ];

  constructor(private formBuilder: FormBuilder) {
    this.customReportForm = this.formBuilder.group({
      reportName: [''],
      reportType: ['calibration'],
      dateFrom: [''],
      dateTo: [''],
      companyFilter: [[]],
      calTypeFilter: [[]]
    });
  }

  ngOnInit(): void {
    this.initializeStandardReports();
    this.initializeScheduledReports();
    this.initializeAvailableColumns();
  }

  private initializeStandardReports(): void {
    this.standardReports = [
      {
        id: 'calibration-summary',
        title: 'Calibration Summary Report',
        description: 'Comprehensive overview of all calibrations performed',
        icon: 'assessment',
        iconColor: 'primary',
        lastGenerated: new Date('2024-07-20'),
        recordCount: 1247
      },
      {
        id: 'technician-performance',
        title: 'Technician Performance Report',
        description: 'Individual technician productivity and accuracy metrics',
        icon: 'person',
        iconColor: 'accent',
        lastGenerated: new Date('2024-07-18'),
        recordCount: 5
      },
      {
        id: 'company-activity',
        title: 'Company Activity Report',
        description: 'Calibration activity breakdown by company',
        icon: 'business',
        iconColor: 'primary',
        lastGenerated: new Date('2024-07-15'),
        recordCount: 45
      },
      {
        id: 'standards-usage',
        title: 'Standards Usage Report',
        description: 'Utilization and maintenance schedule for calibration standards',
        icon: 'verified',
        iconColor: 'accent',
        lastGenerated: new Date('2024-07-12'),
        recordCount: 23
      },
      {
        id: 'tolerance-analysis',
        title: 'Tolerance Analysis Report',
        description: 'Statistical analysis of measurement deviations and tolerances',
        icon: 'analytics',
        iconColor: 'primary',
        lastGenerated: new Date('2024-07-10'),
        recordCount: 892
      },
      {
        id: 'certificate-batch',
        title: 'Certificate Batch Report',
        description: 'Batch generation of calibration certificates',
        icon: 'description',
        iconColor: 'accent',
        lastGenerated: new Date('2024-07-08'),
        recordCount: 156
      }
    ];
  }

  private initializeScheduledReports(): void {
    this.scheduledReports = [
      {
        id: 'weekly-summary',
        name: 'Weekly Calibration Summary',
        frequency: 'Weekly',
        nextRun: new Date('2024-07-29'),
        recipients: ['manager@company.com', 'qa@company.com'],
        status: 'Active'
      },
      {
        id: 'monthly-performance',
        name: 'Monthly Technician Performance',
        frequency: 'Monthly',
        nextRun: new Date('2024-08-01'),
        recipients: ['hr@company.com', 'supervisor@company.com'],
        status: 'Active'
      },
      {
        id: 'quarterly-standards',
        name: 'Quarterly Standards Review',
        frequency: 'Quarterly',
        nextRun: new Date('2024-10-01'),
        recipients: ['metrology@company.com'],
        status: 'Paused'
      }
    ];
  }

  private initializeAvailableColumns(): void {
    this.availableColumns = [
      { key: 'calId', label: 'Calibration ID', selected: true },
      { key: 'serialNo', label: 'Serial Number', selected: true },
      { key: 'orderNo', label: 'Order Number', selected: true },
      { key: 'companyName', label: 'Company', selected: true },
      { key: 'calType', label: 'Calibration Type', selected: true },
      { key: 'calDate', label: 'Calibration Date', selected: true },
      { key: 'technicianName', label: 'Technician', selected: true },
      { key: 'status', label: 'Status', selected: true },
      { key: 'modelNo', label: 'Model Number', selected: false },
      { key: 'manufacturer', label: 'Manufacturer', selected: false },
      { key: 'dueDate', label: 'Due Date', selected: false },
      { key: 'standardsUsed', label: 'Standards Used', selected: false },
      { key: 'comments', label: 'Comments', selected: false }
    ];
  }

  generateReport(report: StandardReport): void {
    console.log('Generating standard report:', report.title);
  }

  viewReport(report: StandardReport): void {
    console.log('Viewing report:', report.title);
  }

  downloadReport(report: StandardReport): void {
    console.log('Downloading report:', report.title);
  }

  generateCustomReport(): void {
    if (this.customReportForm.valid) {
      const reportConfig = {
        ...this.customReportForm.value,
        selectedColumns: this.availableColumns.filter(col => col.selected)
      };
      console.log('Generating custom report:', reportConfig);
    }
  }

  previewCustomReport(): void {
    const reportConfig = {
      ...this.customReportForm.value,
      selectedColumns: this.availableColumns.filter(col => col.selected)
    };
    console.log('Previewing custom report:', reportConfig);
  }

  resetCustomReport(): void {
    this.customReportForm.reset({
      reportType: 'calibration'
    });
    this.availableColumns.forEach(col => {
      col.selected = ['calId', 'serialNo', 'orderNo', 'companyName', 'calType', 'calDate', 'technicianName', 'status'].includes(col.key);
    });
  }

  addScheduledReport(): void {
    console.log('Adding new scheduled report');
  }

  editScheduledReport(report: ScheduledReport): void {
    console.log('Editing scheduled report:', report.name);
  }

  runScheduledReport(report: ScheduledReport): void {
    console.log('Running scheduled report:', report.name);
  }

  deleteScheduledReport(report: ScheduledReport): void {
    console.log('Deleting scheduled report:', report.name);
  }

  getScheduleStatusColor(status: string): string {
    switch (status) {
      case 'Active': return 'primary';
      case 'Paused': return 'accent';
      case 'Error': return 'warn';
      default: return '';
    }
  }
}
