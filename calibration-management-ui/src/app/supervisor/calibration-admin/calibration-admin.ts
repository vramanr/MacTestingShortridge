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
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { CommonModule } from '@angular/common';

interface CalibrationSetup {
  id: string;
  settingName: string;
  description: string;
  value: string;
  dataType: 'text' | 'number' | 'boolean' | 'select';
  options?: string[];
  category: string;
  isRequired: boolean;
  lastModified: Date;
}

interface CalibrationType {
  id: string;
  typeCode: string;
  typeName: string;
  description: string;
  defaultTolerance: number;
  units: string;
  category: string;
  isActive: boolean;
  lastModified: Date;
}

interface CalibrationMode {
  id: string;
  modeCode: string;
  modeName: string;
  description: string;
  procedureSteps: string[];
  requiredStandards: string[];
  isActive: boolean;
  lastModified: Date;
}

interface CalibrationStandard {
  id: string;
  standardId: string;
  description: string;
  manufacturer: string;
  model: string;
  serialNumber: string;
  accuracy: string;
  calibrationDate: Date;
  dueDate: Date;
  status: 'Active' | 'Due' | 'Overdue' | 'Retired';
  location: string;
  lastModified: Date;
}

interface CalibrationTechnician {
  id: string;
  techId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  certifications: string[];
  specialties: string[];
  isActive: boolean;
  hireDate: Date;
  lastModified: Date;
}

@Component({
  selector: 'app-calibration-admin',
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
    MatSlideToggleModule,
    MatChipsModule,
    CommonModule
  ],
  templateUrl: './calibration-admin.html',
  styleUrl: './calibration-admin.scss'
})
export class CalibrationAdmin implements OnInit {
  selectedTabIndex = 0;
  setupForm: FormGroup;
  typeForm: FormGroup;
  modeForm: FormGroup;
  standardForm: FormGroup;
  technicianForm: FormGroup;
  
  calibrationSetups: CalibrationSetup[] = [];
  calibrationTypes: CalibrationType[] = [];
  calibrationModes: CalibrationMode[] = [];
  calibrationStandards: CalibrationStandard[] = [];
  calibrationTechnicians: CalibrationTechnician[] = [];

  setupColumns: string[] = ['settingName', 'value', 'category', 'isRequired', 'actions'];
  typeColumns: string[] = ['typeCode', 'typeName', 'defaultTolerance', 'units', 'category', 'isActive', 'actions'];
  modeColumns: string[] = ['modeCode', 'modeName', 'description', 'isActive', 'actions'];
  standardColumns: string[] = ['standardId', 'description', 'manufacturer', 'calibrationDate', 'dueDate', 'status', 'actions'];
  technicianColumns: string[] = ['techId', 'name', 'email', 'certifications', 'specialties', 'isActive', 'actions'];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute
  ) {
    this.setupForm = this.formBuilder.group({
      settingName: ['', Validators.required],
      description: ['', Validators.required],
      value: ['', Validators.required],
      dataType: ['text', Validators.required],
      category: ['', Validators.required],
      isRequired: [false]
    });

    this.typeForm = this.formBuilder.group({
      typeCode: ['', Validators.required],
      typeName: ['', Validators.required],
      description: ['', Validators.required],
      defaultTolerance: [0, [Validators.required, Validators.min(0)]],
      units: ['', Validators.required],
      category: ['', Validators.required],
      isActive: [true]
    });

    this.modeForm = this.formBuilder.group({
      modeCode: ['', Validators.required],
      modeName: ['', Validators.required],
      description: ['', Validators.required],
      isActive: [true]
    });

    this.standardForm = this.formBuilder.group({
      standardId: ['', Validators.required],
      description: ['', Validators.required],
      manufacturer: ['', Validators.required],
      model: ['', Validators.required],
      serialNumber: ['', Validators.required],
      accuracy: ['', Validators.required],
      calibrationDate: ['', Validators.required],
      dueDate: ['', Validators.required],
      location: ['', Validators.required]
    });

    this.technicianForm = this.formBuilder.group({
      techId: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      hireDate: ['', Validators.required],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.initializeCalibrationSetups();
    this.initializeCalibrationTypes();
    this.initializeCalibrationModes();
    this.initializeCalibrationStandards();
    this.initializeCalibrationTechnicians();
    this.checkTabFromRoute();
  }

  private checkTabFromRoute(): void {
    this.route.queryParams.subscribe(params => {
      const tabMap: { [key: string]: number } = {
        'setup': 0,
        'types': 1,
        'modes': 2,
        'standards': 3,
        'technicians': 4
      };
      
      if (params['tab'] && tabMap[params['tab']] !== undefined) {
        this.selectedTabIndex = tabMap[params['tab']];
      }
    });
  }

  private initializeCalibrationSetups(): void {
    this.calibrationSetups = [
      {
        id: '1',
        settingName: 'Default Calibration Interval',
        description: 'Default interval in months for calibration scheduling',
        value: '12',
        dataType: 'number',
        category: 'Scheduling',
        isRequired: true,
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        settingName: 'Temperature Units',
        description: 'Default temperature units for calibrations',
        value: 'Celsius',
        dataType: 'select',
        options: ['Celsius', 'Fahrenheit', 'Kelvin'],
        category: 'Units',
        isRequired: true,
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        settingName: 'Auto Generate Certificates',
        description: 'Automatically generate certificates upon completion',
        value: 'true',
        dataType: 'boolean',
        category: 'Automation',
        isRequired: false,
        lastModified: new Date('2024-07-15')
      },
      {
        id: '4',
        settingName: 'Certificate Template',
        description: 'Default certificate template to use',
        value: 'ISO 17025 Standard',
        dataType: 'select',
        options: ['ISO 17025 Standard', 'ANSI/NCSL Z540', 'Custom Template'],
        category: 'Documentation',
        isRequired: true,
        lastModified: new Date('2024-07-12')
      }
    ];
  }

  private initializeCalibrationTypes(): void {
    this.calibrationTypes = [
      {
        id: '1',
        typeCode: 'VOLT',
        typeName: 'Voltage Calibration',
        description: 'DC and AC voltage measurement calibration',
        defaultTolerance: 0.01,
        units: 'Volts',
        category: 'Electrical',
        isActive: true,
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        typeCode: 'CURR',
        typeName: 'Current Calibration',
        description: 'DC and AC current measurement calibration',
        defaultTolerance: 0.005,
        units: 'Amperes',
        category: 'Electrical',
        isActive: true,
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        typeCode: 'TEMP',
        typeName: 'Temperature Calibration',
        description: 'Temperature measurement device calibration',
        defaultTolerance: 0.1,
        units: 'Degrees C',
        category: 'Environmental',
        isActive: true,
        lastModified: new Date('2024-07-15')
      },
      {
        id: '4',
        typeCode: 'PRES',
        typeName: 'Pressure Calibration',
        description: 'Pressure measurement device calibration',
        defaultTolerance: 0.02,
        units: 'PSI',
        category: 'Mechanical',
        isActive: true,
        lastModified: new Date('2024-07-12')
      }
    ];
  }

  private initializeCalibrationModes(): void {
    this.calibrationModes = [
      {
        id: '1',
        modeCode: 'STD',
        modeName: 'Standard Calibration',
        description: 'Standard calibration procedure with full documentation',
        procedureSteps: ['Setup equipment', 'Perform measurements', 'Record data', 'Generate certificate'],
        requiredStandards: ['Primary Standard', 'Reference Standard'],
        isActive: true,
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        modeCode: 'EXP',
        modeName: 'Expedited Calibration',
        description: 'Fast-track calibration with reduced documentation',
        procedureSteps: ['Quick setup', 'Essential measurements', 'Basic certificate'],
        requiredStandards: ['Reference Standard'],
        isActive: true,
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        modeCode: 'ADV',
        modeName: 'Advanced Calibration',
        description: 'Comprehensive calibration with extended testing',
        procedureSteps: ['Extended setup', 'Multiple measurements', 'Statistical analysis', 'Detailed certificate'],
        requiredStandards: ['Primary Standard', 'Reference Standard', 'Working Standard'],
        isActive: true,
        lastModified: new Date('2024-07-15')
      }
    ];
  }

  private initializeCalibrationStandards(): void {
    this.calibrationStandards = [
      {
        id: '1',
        standardId: 'STD-VOLT-001',
        description: 'Precision Voltage Standard',
        manufacturer: 'Fluke',
        model: '5720A',
        serialNumber: 'FL123456',
        accuracy: '±0.001%',
        calibrationDate: new Date('2024-01-15'),
        dueDate: new Date('2025-01-15'),
        status: 'Active',
        location: 'Lab A - Shelf 1',
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        standardId: 'STD-CURR-001',
        description: 'Current Reference Standard',
        manufacturer: 'Keysight',
        model: '3458A',
        serialNumber: 'KS789012',
        accuracy: '±0.0005%',
        calibrationDate: new Date('2024-03-20'),
        dueDate: new Date('2025-03-20'),
        status: 'Active',
        location: 'Lab A - Shelf 2',
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        standardId: 'STD-TEMP-001',
        description: 'Temperature Reference',
        manufacturer: 'Hart Scientific',
        model: '1560',
        serialNumber: 'HS345678',
        accuracy: '±0.01°C',
        calibrationDate: new Date('2023-12-10'),
        dueDate: new Date('2024-12-10'),
        status: 'Due',
        location: 'Lab B - Cabinet 1',
        lastModified: new Date('2024-07-15')
      }
    ];
  }

  private initializeCalibrationTechnicians(): void {
    this.calibrationTechnicians = [
      {
        id: '1',
        techId: 'TECH001',
        firstName: 'John',
        lastName: 'Smith',
        email: 'john.smith@company.com',
        phone: '(555) 123-4567',
        certifications: ['ISO 17025', 'ANSI/NCSL Z540'],
        specialties: ['Electrical', 'Temperature'],
        isActive: true,
        hireDate: new Date('2020-03-15'),
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        techId: 'TECH002',
        firstName: 'Sarah',
        lastName: 'Johnson',
        email: 'sarah.johnson@company.com',
        phone: '(555) 234-5678',
        certifications: ['ISO 17025', 'NIST Traceable'],
        specialties: ['Pressure', 'Flow'],
        isActive: true,
        hireDate: new Date('2019-08-22'),
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        techId: 'TECH003',
        firstName: 'Michael',
        lastName: 'Brown',
        email: 'michael.brown@company.com',
        phone: '(555) 345-6789',
        certifications: ['ISO 17025'],
        specialties: ['Dimensional', 'Mass'],
        isActive: true,
        hireDate: new Date('2021-11-10'),
        lastModified: new Date('2024-07-15')
      }
    ];
  }

  addSetup(): void {
    if (this.setupForm.valid) {
      const newSetup: CalibrationSetup = {
        id: Date.now().toString(),
        settingName: this.setupForm.value.settingName,
        description: this.setupForm.value.description,
        value: this.setupForm.value.value,
        dataType: this.setupForm.value.dataType,
        category: this.setupForm.value.category,
        isRequired: this.setupForm.value.isRequired,
        lastModified: new Date()
      };

      this.calibrationSetups.push(newSetup);
      this.setupForm.reset();
      console.log('Added new calibration setup:', newSetup);
    }
  }

  addType(): void {
    if (this.typeForm.valid) {
      const newType: CalibrationType = {
        id: Date.now().toString(),
        typeCode: this.typeForm.value.typeCode,
        typeName: this.typeForm.value.typeName,
        description: this.typeForm.value.description,
        defaultTolerance: this.typeForm.value.defaultTolerance,
        units: this.typeForm.value.units,
        category: this.typeForm.value.category,
        isActive: this.typeForm.value.isActive,
        lastModified: new Date()
      };

      this.calibrationTypes.push(newType);
      this.typeForm.reset({ isActive: true });
      console.log('Added new calibration type:', newType);
    }
  }

  addMode(): void {
    if (this.modeForm.valid) {
      const newMode: CalibrationMode = {
        id: Date.now().toString(),
        modeCode: this.modeForm.value.modeCode,
        modeName: this.modeForm.value.modeName,
        description: this.modeForm.value.description,
        procedureSteps: [],
        requiredStandards: [],
        isActive: this.modeForm.value.isActive,
        lastModified: new Date()
      };

      this.calibrationModes.push(newMode);
      this.modeForm.reset({ isActive: true });
      console.log('Added new calibration mode:', newMode);
    }
  }

  addStandard(): void {
    if (this.standardForm.valid) {
      const newStandard: CalibrationStandard = {
        id: Date.now().toString(),
        standardId: this.standardForm.value.standardId,
        description: this.standardForm.value.description,
        manufacturer: this.standardForm.value.manufacturer,
        model: this.standardForm.value.model,
        serialNumber: this.standardForm.value.serialNumber,
        accuracy: this.standardForm.value.accuracy,
        calibrationDate: new Date(this.standardForm.value.calibrationDate),
        dueDate: new Date(this.standardForm.value.dueDate),
        status: 'Active',
        location: this.standardForm.value.location,
        lastModified: new Date()
      };

      this.calibrationStandards.push(newStandard);
      this.standardForm.reset();
      console.log('Added new calibration standard:', newStandard);
    }
  }

  addTechnician(): void {
    if (this.technicianForm.valid) {
      const newTechnician: CalibrationTechnician = {
        id: Date.now().toString(),
        techId: this.technicianForm.value.techId,
        firstName: this.technicianForm.value.firstName,
        lastName: this.technicianForm.value.lastName,
        email: this.technicianForm.value.email,
        phone: this.technicianForm.value.phone,
        certifications: [],
        specialties: [],
        isActive: this.technicianForm.value.isActive,
        hireDate: new Date(this.technicianForm.value.hireDate),
        lastModified: new Date()
      };

      this.calibrationTechnicians.push(newTechnician);
      this.technicianForm.reset({ isActive: true });
      console.log('Added new calibration technician:', newTechnician);
    }
  }

  editSetup(setup: CalibrationSetup): void {
    this.setupForm.patchValue(setup);
    console.log('Editing calibration setup:', setup);
  }

  editType(type: CalibrationType): void {
    this.typeForm.patchValue(type);
    console.log('Editing calibration type:', type);
  }

  editMode(mode: CalibrationMode): void {
    this.modeForm.patchValue(mode);
    console.log('Editing calibration mode:', mode);
  }

  editStandard(standard: CalibrationStandard): void {
    this.standardForm.patchValue({
      ...standard,
      calibrationDate: standard.calibrationDate.toISOString().split('T')[0],
      dueDate: standard.dueDate.toISOString().split('T')[0]
    });
    console.log('Editing calibration standard:', standard);
  }

  editTechnician(technician: CalibrationTechnician): void {
    this.technicianForm.patchValue({
      ...technician,
      hireDate: technician.hireDate.toISOString().split('T')[0]
    });
    console.log('Editing calibration technician:', technician);
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Active': return 'primary';
      case 'Due': return 'accent';
      case 'Overdue': return 'warn';
      case 'Retired': return '';
      default: return '';
    }
  }

  getTechnicianName(technician: CalibrationTechnician): string {
    return `${technician.firstName} ${technician.lastName}`;
  }

  getDaysUntilDue(dueDate: Date): number {
    const today = new Date();
    const due = new Date(dueDate);
    const diffTime = due.getTime() - today.getTime();
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }
}
