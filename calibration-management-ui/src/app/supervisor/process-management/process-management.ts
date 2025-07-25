import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';

interface NumberSequence {
  id: string;
  sequenceName: string;
  description: string;
  currentValue: number;
  prefix: string;
  suffix: string;
  minLength: number;
  isActive: boolean;
  lastModified: Date;
}

interface DatabaseTable {
  name: string;
  description: string;
  recordCount: number;
  size: string;
  lastPacked: Date;
  needsPacking: boolean;
  status: 'Healthy' | 'Needs Attention' | 'Critical';
}

interface OrderCleanup {
  id: string;
  orderNumber: string;
  companyName: string;
  orderDate: Date;
  status: string;
  totalAmount: number;
  canDelete: boolean;
  reason: string;
}

interface DuplicateRecord {
  id: string;
  tableName: string;
  recordType: string;
  duplicateCount: number;
  primaryKey: string;
  description: string;
  lastFound: Date;
  severity: 'Low' | 'Medium' | 'High';
}

@Component({
  selector: 'app-process-management',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatTabsModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatTooltipModule,
    MatCheckboxModule,
    MatProgressBarModule,
    MatSnackBarModule,
    CommonModule
  ],
  templateUrl: './process-management.html',
  styleUrl: './process-management.scss'
})
export class ProcessManagement implements OnInit {
  selectedTabIndex = 0;
  numberForm: FormGroup;
  
  numberSequences: NumberSequence[] = [];
  databaseTables: DatabaseTable[] = [];
  orderCleanups: OrderCleanup[] = [];
  duplicateRecords: DuplicateRecord[] = [];

  numberColumns: string[] = ['sequenceName', 'currentValue', 'prefix', 'suffix', 'isActive', 'actions'];
  tableColumns: string[] = ['name', 'recordCount', 'size', 'lastPacked', 'status', 'actions'];
  orderColumns: string[] = ['orderNumber', 'companyName', 'orderDate', 'status', 'totalAmount', 'canDelete', 'actions'];
  duplicateColumns: string[] = ['tableName', 'recordType', 'duplicateCount', 'description', 'severity', 'actions'];

  isProcessing = false;
  processingMessage = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute
  ) {
    this.numberForm = this.formBuilder.group({
      sequenceName: ['', Validators.required],
      description: ['', Validators.required],
      currentValue: [1, [Validators.required, Validators.min(1)]],
      prefix: [''],
      suffix: [''],
      minLength: [4, [Validators.required, Validators.min(1)]],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.initializeNumberSequences();
    this.initializeDatabaseTables();
    this.initializeOrderCleanups();
    this.initializeDuplicateRecords();
    this.checkTabFromRoute();
  }

  private checkTabFromRoute(): void {
    this.route.queryParams.subscribe(params => {
      const tabMap: { [key: string]: number } = {
        'numbers': 0,
        'pack': 1,
        'delete': 2,
        'duplicates': 3
      };
      
      if (params['tab'] && tabMap[params['tab']] !== undefined) {
        this.selectedTabIndex = tabMap[params['tab']];
      }
    });
  }

  private initializeNumberSequences(): void {
    this.numberSequences = [
      {
        id: '1',
        sequenceName: 'Calibration ID',
        description: 'Sequential numbering for calibration records',
        currentValue: 24567,
        prefix: 'CAL',
        suffix: '',
        minLength: 6,
        isActive: true,
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        sequenceName: 'Order Number',
        description: 'Sequential numbering for customer orders',
        currentValue: 15432,
        prefix: 'ORD',
        suffix: '',
        minLength: 5,
        isActive: true,
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        sequenceName: 'Certificate Number',
        description: 'Sequential numbering for calibration certificates',
        currentValue: 98765,
        prefix: 'CERT',
        suffix: '',
        minLength: 6,
        isActive: true,
        lastModified: new Date('2024-07-15')
      },
      {
        id: '4',
        sequenceName: 'Invoice Number',
        description: 'Sequential numbering for invoices',
        currentValue: 12345,
        prefix: 'INV',
        suffix: '',
        minLength: 5,
        isActive: true,
        lastModified: new Date('2024-07-12')
      }
    ];
  }

  private initializeDatabaseTables(): void {
    this.databaseTables = [
      {
        name: 'cal_info',
        description: 'Calibration information records',
        recordCount: 15247,
        size: '2.3 MB',
        lastPacked: new Date('2024-06-15'),
        needsPacking: true,
        status: 'Needs Attention'
      },
      {
        name: 'cal_data',
        description: 'Calibration measurement data',
        recordCount: 89156,
        size: '12.7 MB',
        lastPacked: new Date('2024-07-01'),
        needsPacking: false,
        status: 'Healthy'
      },
      {
        name: 'company',
        description: 'Customer company information',
        recordCount: 1847,
        size: '892 KB',
        lastPacked: new Date('2024-05-20'),
        needsPacking: true,
        status: 'Critical'
      },
      {
        name: 'tolerances',
        description: 'Tolerance specifications',
        recordCount: 567,
        size: '234 KB',
        lastPacked: new Date('2024-07-10'),
        needsPacking: false,
        status: 'Healthy'
      }
    ];
  }

  private initializeOrderCleanups(): void {
    this.orderCleanups = [
      {
        id: '1',
        orderNumber: 'ORD12345',
        companyName: 'Old Tech Corp',
        orderDate: new Date('2022-01-15'),
        status: 'Completed',
        totalAmount: 1250.00,
        canDelete: true,
        reason: 'Order older than 2 years'
      },
      {
        id: '2',
        orderNumber: 'ORD12346',
        companyName: 'Legacy Systems Inc',
        orderDate: new Date('2022-03-20'),
        status: 'Cancelled',
        totalAmount: 0.00,
        canDelete: true,
        reason: 'Cancelled order older than 2 years'
      },
      {
        id: '3',
        orderNumber: 'ORD12347',
        companyName: 'Test Company',
        orderDate: new Date('2023-12-01'),
        status: 'Pending',
        totalAmount: 500.00,
        canDelete: false,
        reason: 'Recent pending order'
      }
    ];
  }

  private initializeDuplicateRecords(): void {
    this.duplicateRecords = [
      {
        id: '1',
        tableName: 'company',
        recordType: 'Customer Record',
        duplicateCount: 3,
        primaryKey: 'ACME001',
        description: 'Acme Corporation - Multiple entries with same company ID',
        lastFound: new Date('2024-07-20'),
        severity: 'High'
      },
      {
        id: '2',
        tableName: 'cal_standards',
        recordType: 'Standard Equipment',
        duplicateCount: 2,
        primaryKey: 'STD-VOLT-001',
        description: 'Voltage Standard - Duplicate serial numbers',
        lastFound: new Date('2024-07-18'),
        severity: 'Medium'
      },
      {
        id: '3',
        tableName: 'tolerances',
        recordType: 'Tolerance Specification',
        duplicateCount: 2,
        primaryKey: 'TOL-TEMP-001',
        description: 'Temperature tolerance - Duplicate specifications',
        lastFound: new Date('2024-07-15'),
        severity: 'Low'
      }
    ];
  }

  addNumberSequence(): void {
    if (this.numberForm.valid) {
      const newSequence: NumberSequence = {
        id: Date.now().toString(),
        sequenceName: this.numberForm.value.sequenceName,
        description: this.numberForm.value.description,
        currentValue: this.numberForm.value.currentValue,
        prefix: this.numberForm.value.prefix,
        suffix: this.numberForm.value.suffix,
        minLength: this.numberForm.value.minLength,
        isActive: this.numberForm.value.isActive,
        lastModified: new Date()
      };

      this.numberSequences.push(newSequence);
      this.numberForm.reset({ isActive: true });
      console.log('Added new number sequence:', newSequence);
    }
  }

  editNumberSequence(sequence: NumberSequence): void {
    this.numberForm.patchValue(sequence);
    console.log('Editing number sequence:', sequence);
  }

  resetNumberSequence(sequence: NumberSequence): void {
    sequence.currentValue = 1;
    sequence.lastModified = new Date();
    console.log('Reset number sequence:', sequence);
  }

  toggleSequenceStatus(sequence: NumberSequence): void {
    sequence.isActive = !sequence.isActive;
    sequence.lastModified = new Date();
    console.log('Toggled sequence status:', sequence);
  }

  packTable(table: DatabaseTable): void {
    this.isProcessing = true;
    this.processingMessage = `Packing table: ${table.name}`;
    
    setTimeout(() => {
      table.lastPacked = new Date();
      table.needsPacking = false;
      table.status = 'Healthy';
      this.isProcessing = false;
      this.processingMessage = '';
      console.log('Packed table:', table.name);
    }, 3000);
  }

  packAllTables(): void {
    this.isProcessing = true;
    this.processingMessage = 'Packing all database tables...';
    
    setTimeout(() => {
      this.databaseTables.forEach(table => {
        table.lastPacked = new Date();
        table.needsPacking = false;
        table.status = 'Healthy';
      });
      this.isProcessing = false;
      this.processingMessage = '';
      console.log('Packed all tables');
    }, 8000);
  }

  deleteOrder(order: OrderCleanup): void {
    if (order.canDelete) {
      const index = this.orderCleanups.findIndex(o => o.id === order.id);
      if (index > -1) {
        this.orderCleanups.splice(index, 1);
        console.log('Deleted order:', order.orderNumber);
      }
    }
  }

  deleteSelectedOrders(): void {
    const deletableOrders = this.orderCleanups.filter(o => o.canDelete);
    this.isProcessing = true;
    this.processingMessage = `Deleting ${deletableOrders.length} orders...`;
    
    setTimeout(() => {
      this.orderCleanups = this.orderCleanups.filter(o => !o.canDelete);
      this.isProcessing = false;
      this.processingMessage = '';
      console.log('Deleted selected orders');
    }, 2000);
  }

  resolveDuplicate(duplicate: DuplicateRecord): void {
    const index = this.duplicateRecords.findIndex(d => d.id === duplicate.id);
    if (index > -1) {
      this.duplicateRecords.splice(index, 1);
      console.log('Resolved duplicate:', duplicate.description);
    }
  }

  scanForDuplicates(): void {
    this.isProcessing = true;
    this.processingMessage = 'Scanning for duplicate records...';
    
    setTimeout(() => {
      this.isProcessing = false;
      this.processingMessage = '';
      console.log('Completed duplicate scan');
    }, 5000);
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Healthy': return 'primary';
      case 'Needs Attention': return 'accent';
      case 'Critical': return 'warn';
      default: return '';
    }
  }

  getSeverityColor(severity: string): string {
    switch (severity) {
      case 'Low': return 'primary';
      case 'Medium': return 'accent';
      case 'High': return 'warn';
      default: return '';
    }
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(value);
  }

  getFormattedNumber(sequence: NumberSequence): string {
    const paddedNumber = sequence.currentValue.toString().padStart(sequence.minLength, '0');
    return `${sequence.prefix}${paddedNumber}${sequence.suffix}`;
  }
}
