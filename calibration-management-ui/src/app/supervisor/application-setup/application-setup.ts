import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';

interface SystemSetting {
  key: string;
  label: string;
  value: string;
  type: 'text' | 'number' | 'boolean' | 'select';
  options?: string[];
  description: string;
  category: string;
}

interface DatabaseTable {
  name: string;
  description: string;
  recordCount: number;
  lastModified: Date;
  size: string;
  status: 'Active' | 'Inactive' | 'Maintenance';
}

@Component({
  selector: 'app-application-setup',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatCheckboxModule,
    MatSlideToggleModule,
    MatTooltipModule,
    CommonModule
  ],
  templateUrl: './application-setup.html',
  styleUrl: './application-setup.scss'
})
export class ApplicationSetup implements OnInit {
  setupForm: FormGroup;
  selectedTabIndex = 0;
  systemSettings: SystemSetting[] = [];
  databaseTables: DatabaseTable[] = [];
  
  tableColumns: string[] = ['name', 'description', 'recordCount', 'lastModified', 'size', 'status', 'actions'];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute
  ) {
    this.setupForm = this.formBuilder.group({
      companyName: ['Shortridge Calibration Services', Validators.required],
      companyAddress: ['123 Calibration Way', Validators.required],
      companyCity: ['Precision City', Validators.required],
      companyState: ['CA', Validators.required],
      companyZip: ['90210', Validators.required],
      companyPhone: ['(555) 123-4567', Validators.required],
      companyEmail: ['info@shortridge.com', [Validators.required, Validators.email]],
      defaultCalibrationInterval: [12, [Validators.required, Validators.min(1)]],
      defaultTolerancePercent: [2.0, [Validators.required, Validators.min(0)]],
      autoBackupEnabled: [true],
      backupRetentionDays: [30, [Validators.required, Validators.min(1)]],
      certificateTemplate: ['Standard Template', Validators.required],
      reportFormat: ['PDF', Validators.required]
    });
  }

  ngOnInit(): void {
    this.initializeSystemSettings();
    this.initializeDatabaseTables();
    this.checkTabFromRoute();
  }

  private checkTabFromRoute(): void {
    this.route.queryParams.subscribe(params => {
      if (params['tab'] === 'tables') {
        this.selectedTabIndex = 1;
      }
    });
  }

  private initializeSystemSettings(): void {
    this.systemSettings = [
      {
        key: 'cal_interval',
        label: 'Default Calibration Interval (months)',
        value: '12',
        type: 'number',
        description: 'Default interval for calibration scheduling',
        category: 'Calibration'
      },
      {
        key: 'tolerance_percent',
        label: 'Default Tolerance Percentage',
        value: '2.0',
        type: 'number',
        description: 'Default tolerance percentage for measurements',
        category: 'Calibration'
      },
      {
        key: 'auto_backup',
        label: 'Automatic Backup',
        value: 'true',
        type: 'boolean',
        description: 'Enable automatic database backups',
        category: 'System'
      },
      {
        key: 'backup_retention',
        label: 'Backup Retention (days)',
        value: '30',
        type: 'number',
        description: 'Number of days to retain backup files',
        category: 'System'
      },
      {
        key: 'cert_template',
        label: 'Certificate Template',
        value: 'Standard Template',
        type: 'select',
        options: ['Standard Template', 'ISO Template', 'Custom Template'],
        description: 'Default template for calibration certificates',
        category: 'Reports'
      },
      {
        key: 'report_format',
        label: 'Report Format',
        value: 'PDF',
        type: 'select',
        options: ['PDF', 'Excel', 'Word'],
        description: 'Default format for generated reports',
        category: 'Reports'
      }
    ];
  }

  private initializeDatabaseTables(): void {
    this.databaseTables = [
      {
        name: 'cal_info',
        description: 'Calibration information records',
        recordCount: 15247,
        lastModified: new Date('2024-07-20'),
        size: '2.3 MB',
        status: 'Active'
      },
      {
        name: 'cal_data',
        description: 'Calibration measurement data',
        recordCount: 89156,
        lastModified: new Date('2024-07-20'),
        size: '12.7 MB',
        status: 'Active'
      },
      {
        name: 'cal_standards',
        description: 'Calibration standards inventory',
        recordCount: 234,
        lastModified: new Date('2024-07-18'),
        size: '156 KB',
        status: 'Active'
      },
      {
        name: 'company',
        description: 'Customer company information',
        recordCount: 1847,
        lastModified: new Date('2024-07-19'),
        size: '892 KB',
        status: 'Active'
      },
      {
        name: 'tolerances',
        description: 'Tolerance specifications',
        recordCount: 567,
        lastModified: new Date('2024-07-15'),
        size: '234 KB',
        status: 'Active'
      },
      {
        name: 'cal_techs',
        description: 'Technician information',
        recordCount: 12,
        lastModified: new Date('2024-07-10'),
        size: '45 KB',
        status: 'Active'
      }
    ];
  }

  saveSettings(): void {
    if (this.setupForm.valid) {
      console.log('Saving application settings:', this.setupForm.value);
    }
  }

  resetSettings(): void {
    this.setupForm.reset();
    this.initializeSystemSettings();
  }

  browseTable(table: DatabaseTable): void {
    console.log('Browsing table:', table.name);
  }

  optimizeTable(table: DatabaseTable): void {
    console.log('Optimizing table:', table.name);
  }

  repairTable(table: DatabaseTable): void {
    console.log('Repairing table:', table.name);
  }

  backupTable(table: DatabaseTable): void {
    console.log('Backing up table:', table.name);
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Active': return 'primary';
      case 'Inactive': return 'accent';
      case 'Maintenance': return 'warn';
      default: return '';
    }
  }

  getSettingsByCategory(category: string): SystemSetting[] {
    return this.systemSettings.filter(setting => setting.category === category);
  }

  getCategories(): string[] {
    return [...new Set(this.systemSettings.map(setting => setting.category))];
  }
}
