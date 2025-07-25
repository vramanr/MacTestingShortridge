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

@Component({
  selector: 'app-calibration-entry',
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
    CommonModule
  ],
  templateUrl: './calibration-entry.html',
  styleUrl: './calibration-entry.scss'
})
export class CalibrationEntry implements OnInit {
  calibrationForm: FormGroup;
  isNoOrderMode = false;
  calibrationType: string | null = null;
  measurementPoints: MeasurementPoint[] = [];

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
    this.calibrationForm = this.formBuilder.group({
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
      const mode = params['mode'];
      const calType = params['calType'];
      
      this.isNoOrderMode = mode === 'no-order';
      this.calibrationType = calType;
      
      if (this.isNoOrderMode && calType) {
        this.calibrationForm.patchValue({ calType: calType });
      }
      
      if (!this.isNoOrderMode) {
        this.calibrationForm.get('orderNo')?.setValidators([Validators.required]);
        this.calibrationForm.get('coId')?.setValidators([Validators.required]);
      }
    });

    this.addMeasurementPoint();
    this.addMeasurementPoint();
    this.addMeasurementPoint();
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

  searchOrder(): void {
    this.router.navigate(['/order/search']);
  }

  onSubmit(): void {
    if (this.calibrationForm.valid) {
      const formData = {
        ...this.calibrationForm.value,
        measurementPoints: this.measurementPoints,
        isNoOrder: this.isNoOrderMode
      };
      
      console.log('Calibration data:', formData);
      
      this.router.navigate(['/dashboard']);
    }
  }

  onSaveDraft(): void {
    const formData = {
      ...this.calibrationForm.value,
      measurementPoints: this.measurementPoints,
      isNoOrder: this.isNoOrderMode,
      isDraft: true
    };
    
    console.log('Saving draft:', formData);
  }

  onCancel(): void {
    this.router.navigate(['/dashboard']);
  }
}
