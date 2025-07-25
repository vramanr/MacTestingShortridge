import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormValidationService } from '../../shared/services/form-validation.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';

interface CalibrationRecord {
  calId: string;
  serialNo: string;
  orderNo?: string;
  companyName: string;
  calType: string;
  calDate: Date;
  technicianName: string;
  status: string;
  modelNo?: string;
  manufacturer?: string;
}

interface Company {
  coId: string;
  coName: string;
}

interface Technician {
  techId: string;
  techName: string;
}

@Component({
  selector: 'app-calibration-search',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatPaginatorModule,
    MatChipsModule,
    MatTooltipModule,
    MatSnackBarModule,
    CommonModule
  ],
  templateUrl: './calibration-search.html',
  styleUrl: './calibration-search.scss'
})
export class CalibrationSearch implements OnInit {
  searchForm: FormGroup;
  searchResults: CalibrationRecord[] = [];
  selectedRecord: CalibrationRecord | null = null;
  hasSearched = false;

  displayedColumns: string[] = [
    'serialNo', 'orderNo', 'company', 'calType', 
    'calDate', 'technician', 'status', 'actions'
  ];

  companies: Company[] = [
    { coId: 'ACME', coName: 'Acme Corporation' },
    { coId: 'TECH', coName: 'TechCorp Industries' },
    { coId: 'PREC', coName: 'Precision Instruments LLC' },
    { coId: 'METRO', coName: 'Metro Calibration Services' },
    { coId: 'QUAL', coName: 'Quality Assurance Labs' }
  ];

  technicians: Technician[] = [
    { techId: 'TECH001', techName: 'John Smith' },
    { techId: 'TECH002', techName: 'Jane Doe' },
    { techId: 'TECH003', techName: 'Mike Johnson' },
    { techId: 'TECH004', techName: 'Sarah Wilson' },
    { techId: 'TECH005', techName: 'David Brown' }
  ];

  private mockRecords: CalibrationRecord[] = [
    {
      calId: 'CAL001',
      serialNo: 'TH-12345',
      orderNo: 'ORD-001',
      companyName: 'Acme Corporation',
      calType: 'ADM',
      calDate: new Date('2024-01-15'),
      technicianName: 'John Smith',
      status: 'Complete'
    },
    {
      calId: 'CAL002',
      serialNo: 'HM-67890',
      orderNo: 'ORD-002',
      companyName: 'TechCorp Industries',
      calType: 'HDM',
      calDate: new Date('2024-01-20'),
      technicianName: 'Jane Doe',
      status: 'Complete'
    },
    {
      calId: 'CAL003',
      serialNo: 'PG-11111',
      companyName: 'Precision Instruments LLC',
      calType: 'Pressure',
      calDate: new Date('2024-01-25'),
      technicianName: 'Mike Johnson',
      status: 'In Progress'
    },
    {
      calId: 'CAL004',
      serialNo: 'MT-22222',
      orderNo: 'ORD-004',
      companyName: 'Metro Calibration Services',
      calType: 'MultiTemp',
      calDate: new Date('2024-02-01'),
      technicianName: 'Sarah Wilson',
      status: 'Draft'
    },
    {
      calId: 'CAL005',
      serialNo: 'EL-33333',
      orderNo: 'ORD-005',
      companyName: 'Quality Assurance Labs',
      calType: 'Electrical',
      calDate: new Date('2024-02-05'),
      technicianName: 'David Brown',
      status: 'Pending'
    }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private formValidationService: FormValidationService
  ) {
    this.searchForm = this.formBuilder.group({
      serialNo: ['', [FormValidationService.serialNumberValidator()]],
      orderNo: ['', [FormValidationService.orderNumberValidator()]],
      coId: [''],
      calType: [''],
      techId: [''],
      status: [''],
      dateFrom: [''],
      dateTo: ['']
    });
  }

  ngOnInit(): void {
    this.searchResults = [...this.mockRecords];
  }

  onSearch(): void {
    const orderNo = this.searchForm.get('orderNo')?.value || '';
    const serialNo = this.searchForm.get('serialNo')?.value || '';
    
    const validation = this.formValidationService.validateSearchCriteria(orderNo, serialNo);
    
    if (!validation.isValid) {
      const errorMessages = Object.values(validation.errors).flat();
      this.snackBar.open(errorMessages[0], 'Close', { duration: 5000 });
      return;
    }

    this.hasSearched = true;
    const criteria = this.searchForm.value;
    
    this.searchResults = this.mockRecords.filter(record => {
      return (!criteria.serialNo || record.serialNo.toLowerCase().includes(criteria.serialNo.toLowerCase())) &&
             (!criteria.orderNo || record.orderNo?.toLowerCase().includes(criteria.orderNo.toLowerCase())) &&
             (!criteria.coId || this.getCompanyName(criteria.coId) === record.companyName) &&
             (!criteria.calType || record.calType === criteria.calType) &&
             (!criteria.techId || this.getTechnicianName(criteria.techId) === record.technicianName) &&
             (!criteria.status || record.status === criteria.status) &&
             (!criteria.dateFrom || record.calDate >= criteria.dateFrom) &&
             (!criteria.dateTo || record.calDate <= criteria.dateTo);
    });

    if (this.searchResults.length === 0) {
      this.snackBar.open('Record not found! Try Again', 'Close', { duration: 3000 });
    } else if (this.searchResults.length === 1) {
      const record = this.searchResults[0];
      this.handleSingleRecord(record);
    } else {
      this.snackBar.open(`Found ${this.searchResults.length} records - [F2] to Select`, 'Close', { duration: 3000 });
    }

    console.log('Search criteria:', criteria);
    console.log('Search results:', this.searchResults);
  }

  onClear(): void {
    this.searchForm.reset();
    this.searchResults = [...this.mockRecords];
    this.selectedRecord = null;
    this.hasSearched = false;
  }

  selectRecord(record: CalibrationRecord): void {
    this.selectedRecord = record;
  }

  clearSelection(): void {
    this.selectedRecord = null;
  }

  editRecord(record: CalibrationRecord): void {
    this.formValidationService.validateEditPermissions(record.calId, 'EDIT').subscribe({
      next: (validation) => {
        if (validation.isValid) {
          this.router.navigate(['/calibration/edit', record.calId]);
        } else {
          const errorMessages = Object.values(validation.errors).flat();
          this.snackBar.open(errorMessages[0], 'Close', { duration: 5000 });
        }
      },
      error: (error) => {
        console.error('Edit permission validation error:', error);
        this.snackBar.open('Unable to validate edit permissions', 'Close', { duration: 3000 });
      }
    });
  }

  viewRecord(record: CalibrationRecord): void {
    console.log('Viewing record:', record);
  }

  duplicateRecord(record: CalibrationRecord): void {
    console.log('Duplicating record:', record);
  }

  deleteRecord(record: CalibrationRecord): void {
    console.log('Deleting record:', record);
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Complete': return 'primary';
      case 'In Progress': return 'accent';
      case 'Draft': return 'warn';
      case 'Pending': return '';
      default: return '';
    }
  }

  private getCompanyName(coId: string): string {
    const company = this.companies.find(c => c.coId === coId);
    return company ? company.coName : '';
  }

  private getTechnicianName(techId: string): string {
    const technician = this.technicians.find(t => t.techId === techId);
    return technician ? technician.techName : '';
  }

  private handleSingleRecord(record: CalibrationRecord): void {
    const allowEdit = record.status !== 'Complete'; // Mock logic
    
    if (!allowEdit) {
      this.router.navigate(['/calibration/edit', record.calId], { 
        queryParams: { mode: 'EDIT' } 
      });
    } else {
      this.router.navigate(['/calibration/edit', record.calId], { 
        queryParams: { mode: 'FULL EDIT' } 
      });
    }
  }

  getErrorMessage(fieldName: string): string {
    const field = this.searchForm.get(fieldName);
    if (field?.errors) {
      const firstError = Object.keys(field.errors)[0];
      return field.errors[firstError]?.message || `${fieldName} is invalid`;
    }
    return '';
  }
}
