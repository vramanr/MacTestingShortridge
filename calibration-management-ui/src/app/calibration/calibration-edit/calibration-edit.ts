import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';

interface MeasurementPoint {
  setPoint: number;
  reading: number;
  deviation: number;
  tolerance: number;
}

interface Company {
  coId: string;
  coName: string;
}

interface Technician {
  techId: string;
  techName: string;
}

interface Standard {
  standardId: string;
  description: string;
  serialNo: string;
}

interface AuditEntry {
  action: string;
  timestamp: Date;
  user: string;
  description: string;
}

@Component({
  selector: 'app-calibration-edit',
  imports: [
    ReactiveFormsModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    CommonModule
  ],
  templateUrl: './calibration-edit.html',
  styleUrl: './calibration-edit.scss'
})
export class CalibrationEdit implements OnInit {
  editForm: FormGroup;
  loading = true;
  saving = false;
  error: string | null = null;
  calId: string | null = null;
  lastModified: Date = new Date();
  measurementPoints: MeasurementPoint[] = [];
  auditTrail: AuditEntry[] = [];

  companies: Company[] = [
    { coId: 'ACME', coName: 'Acme Corporation' },
    { coId: 'TECH', coName: 'TechCorp Industries' },
    { coId: 'PREC', coName: 'Precision Instruments LLC' }
  ];

  technicians: Technician[] = [
    { techId: 'TECH001', techName: 'John Smith' },
    { techId: 'TECH002', techName: 'Jane Doe' },
    { techId: 'TECH003', techName: 'Mike Johnson' }
  ];

  availableStandards: Standard[] = [
    { standardId: 'STD001', description: 'Temperature Standard', serialNo: 'TS-12345' },
    { standardId: 'STD002', description: 'Humidity Standard', serialNo: 'HS-67890' },
    { standardId: 'STD003', description: 'Pressure Standard', serialNo: 'PS-11111' }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.editForm = this.formBuilder.group({
      calId: ['', Validators.required],
      status: ['', Validators.required],
      orderNo: [''],
      coId: [''],
      serialNo: ['', Validators.required],
      modelNo: [''],
      manufacturer: [''],
      calType: ['', Validators.required],
      calMode: ['Standard'],
      techId: ['', Validators.required],
      calDate: [new Date(), Validators.required],
      dueDate: [''],
      standardsUsed: [[]],
      comments: ['']
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.calId = params['id'];
      if (this.calId) {
        this.loadCalibrationRecord(this.calId);
      } else {
        this.error = 'No calibration ID provided';
        this.loading = false;
      }
    });
  }

  loadCalibrationRecord(calId: string): void {
    setTimeout(() => {
      const mockRecord = {
        calId: calId,
        status: 'In Progress',
        orderNo: 'ORD-001',
        coId: 'ACME',
        serialNo: 'TH-12345',
        modelNo: 'TH-2000',
        manufacturer: 'Acme Instruments',
        calType: 'ADM',
        calMode: 'Standard',
        techId: 'TECH001',
        calDate: new Date('2024-01-15'),
        dueDate: new Date('2024-07-15'),
        standardsUsed: ['STD001', 'STD002'],
        comments: 'Initial calibration completed successfully'
      };

      this.editForm.patchValue(mockRecord);
      this.lastModified = new Date();
      
      this.measurementPoints = [
        { setPoint: 20.0, reading: 20.1, deviation: 0.1, tolerance: 0.5 },
        { setPoint: 50.0, reading: 49.9, deviation: -0.1, tolerance: 0.5 },
        { setPoint: 80.0, reading: 80.2, deviation: 0.2, tolerance: 0.5 }
      ];

      this.auditTrail = [
        {
          action: 'Created',
          timestamp: new Date('2024-01-15T09:00:00'),
          user: 'John Smith',
          description: 'Calibration record created'
        },
        {
          action: 'Updated',
          timestamp: new Date('2024-01-15T10:30:00'),
          user: 'John Smith',
          description: 'Measurement points added'
        },
        {
          action: 'Updated',
          timestamp: new Date('2024-01-15T11:15:00'),
          user: 'John Smith',
          description: 'Standards information updated'
        }
      ];

      this.loading = false;
    }, 1000);
  }

  addMeasurementPoint(): void {
    this.measurementPoints.push({
      setPoint: 0,
      reading: 0,
      deviation: 0,
      tolerance: 0
    });
  }

  removeMeasurementPoint(index: number): void {
    if (this.measurementPoints.length > 1) {
      this.measurementPoints.splice(index, 1);
    }
  }

  calculateDeviation(point: MeasurementPoint): void {
    point.deviation = point.reading - point.setPoint;
  }

  onSave(): void {
    if (this.editForm.valid) {
      this.saving = true;
      
      const formData = {
        ...this.editForm.value,
        measurementPoints: this.measurementPoints
      };
      
      console.log('Saving calibration data:', formData);
      
      setTimeout(() => {
        this.saving = false;
        this.router.navigate(['/calibration/search']);
      }, 2000);
    }
  }

  onSaveDraft(): void {
    this.saving = true;
    
    const formData = {
      ...this.editForm.value,
      measurementPoints: this.measurementPoints,
      status: 'Draft'
    };
    
    console.log('Saving draft:', formData);
    
    setTimeout(() => {
      this.saving = false;
      this.editForm.patchValue({ status: 'Draft' });
    }, 1000);
  }

  goBack(): void {
    this.router.navigate(['/calibration/search']);
  }
}
