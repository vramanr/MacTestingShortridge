import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule, FormArray } from '@angular/forms';
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
import { CustomValidators } from '../../shared/validators/custom-validators';
import { FormValidationService } from '../../shared/services/form-validation.service';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    private router: Router,
    private snackBar: MatSnackBar,
    private formValidationService: FormValidationService
  ) {
    this.calibrationForm = this.formBuilder.group({
      orderNo: [''],
      coId: [''],
      serialNo: ['', [
        Validators.required,
        FormValidationService.serialNumberValidator()
      ]],
      modelNo: ['', [
        Validators.required,
        FormValidationService.modelNumberValidator()
      ]],
      manufacturer: ['', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100)
      ]],
      calType: ['', Validators.required],
      calMode: ['Standard', Validators.required],
      techId: ['', Validators.required],
      calDate: [new Date(), Validators.required],
      dueDate: ['', FormValidationService.calibrationDueDateValidator()],
      standardsUsed: [[], Validators.required],
      comments: ['', Validators.maxLength(500)],
      measurementPoints: this.formBuilder.array([])
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
        this.calibrationForm.get('orderNo')?.setValidators([
          Validators.required,
          FormValidationService.orderNumberValidator()
        ]);
        this.calibrationForm.get('coId')?.setValidators([
          Validators.required,
          FormValidationService.companyCodeValidator()
        ]);
      } else {
        this.calibrationForm.get('coId')?.setValidators([
          Validators.required,
          FormValidationService.companyCodeValidator()
        ]);
      }
      
      this.calibrationForm.get('orderNo')?.updateValueAndValidity();
      this.calibrationForm.get('coId')?.updateValueAndValidity();
    });

    this.addMeasurementPoint();
    this.addMeasurementPoint();
    this.addMeasurementPoint();
  }

  get measurementPointsFormArray(): FormArray {
    return this.calibrationForm.get('measurementPoints') as FormArray;
  }

  addMeasurementPoint(): void {
    const measurementPointGroup = this.formBuilder.group({
      setPoint: [0, [
        Validators.required,
        FormValidationService.calibrationReadingValidator()
      ]],
      reading: [0, [
        Validators.required,
        FormValidationService.calibrationReadingValidator()
      ]],
      deviation: [{ value: 0, disabled: true }],
      tolerance: [0, [
        Validators.required,
        FormValidationService.toleranceValueValidator()
      ]]
    });

    this.measurementPointsFormArray.push(measurementPointGroup);
    
    this.measurementPoints.push({
      setPoint: 0,
      reading: 0,
      deviation: 0,
      tolerance: 0
    });
  }

  removeMeasurementPoint(index: number): void {
    if (this.measurementPoints.length > 1) {
      this.measurementPointsFormArray.removeAt(index);
      this.measurementPoints.splice(index, 1);
    }
  }

  calculateDeviation(point: MeasurementPoint, index: number): void {
    point.deviation = point.reading - point.setPoint;
    
    const measurementGroup = this.measurementPointsFormArray.at(index);
    measurementGroup.get('deviation')?.setValue(point.deviation);
    
    this.validateTolerance(point, index);
  }

  validateTolerance(point: MeasurementPoint, index: number): void {
    const measurementGroup = this.measurementPointsFormArray.at(index);
    const deviation = Math.abs(point.deviation);
    const tolerance = point.tolerance;
    
    if (deviation > tolerance) {
      measurementGroup.get('reading')?.setErrors({ 
        toleranceExceeded: { 
          deviation: deviation, 
          tolerance: tolerance,
          difference: deviation - tolerance
        } 
      });
    } else {
      const currentErrors = measurementGroup.get('reading')?.errors;
      if (currentErrors) {
        delete currentErrors['toleranceExceeded'];
        const hasOtherErrors = Object.keys(currentErrors).length > 0;
        measurementGroup.get('reading')?.setErrors(hasOtherErrors ? currentErrors : null);
      }
    }
  }

  getFieldError(fieldName: string): string | null {
    const field = this.calibrationForm.get(fieldName);
    if (field?.errors && field.touched) {
      const errors = field.errors;
      
      if (errors['required']) return `${fieldName} is required`;
      if (errors['minlength']) return `${fieldName} must be at least ${errors['minlength'].requiredLength} characters`;
      if (errors['maxlength']) return `${fieldName} must not exceed ${errors['maxlength'].requiredLength} characters`;
      if (errors['serialNumberLength']) return `Serial number must be between ${errors['serialNumberLength'].min} and ${errors['serialNumberLength'].max} characters`;
      if (errors['serialNumberFormat']) return 'Serial number can only contain letters, numbers, hyphens, and underscores';
      if (errors['orderNumberFormat']) return 'Order number must be 4-10 digits';
      if (errors['companyCodeLength']) return `Company code must be between ${errors['companyCodeLength'].min} and ${errors['companyCodeLength'].max} characters`;
      if (errors['companyCodeFormat']) return 'Company code can only contain letters and numbers';
      if (errors['technicianIdLength']) return `Technician ID must be between ${errors['technicianIdLength'].min} and ${errors['technicianIdLength'].max} characters`;
      if (errors['technicianIdFormat']) return 'Technician ID can only contain letters and numbers';
      if (errors['modelNumberLength']) return `Model number must be between ${errors['modelNumberLength'].min} and ${errors['modelNumberLength'].max} characters`;
      if (errors['modelNumberFormat']) return 'Model number contains invalid characters';
      if (errors['dueDatePast']) return 'Due date cannot be in the past';
      if (errors['dueDateTooFar']) return 'Due date cannot be more than one year from now';
    }
    
    return null;
  }

  getMeasurementPointError(index: number, fieldName: string): string | null {
    const measurementGroup = this.measurementPointsFormArray.at(index);
    const field = measurementGroup.get(fieldName);
    
    if (field?.errors && field.touched) {
      const errors = field.errors;
      
      if (errors['required']) return `${fieldName} is required`;
      if (errors['readingFormat']) return 'Invalid reading format (use decimal numbers)';
      if (errors['readingRange']) return `Reading must be between ${errors['readingRange'].min} and ${errors['readingRange'].max}`;
      if (errors['toleranceRange']) return `Tolerance must be between ${errors['toleranceRange'].min}% and ${errors['toleranceRange'].max}%`;
      if (errors['toleranceFormat']) return 'Tolerance must be a valid number';
      if (errors['toleranceExceeded']) return `Reading exceeds tolerance by ${errors['toleranceExceeded'].difference.toFixed(4)}`;
    }
    
    return null;
  }

  searchOrder(): void {
    this.router.navigate(['/order/search']);
  }

  onSubmit(): void {
    const calInfo = {
      serialNo: this.calibrationForm.get('serialNo')?.value,
      modelNo: this.calibrationForm.get('modelNo')?.value,
      orderNo: this.calibrationForm.get('orderNo')?.value,
      coId: this.calibrationForm.get('coId')?.value,
      calType: this.calibrationForm.get('calType')?.value
    };

    const calData = this.measurementPoints.map(mp => ({
      setPoint: mp.setPoint,
      actualReading: mp.reading,
      deviation: mp.deviation,
      tolerance: mp.tolerance,
      mode: this.calibrationForm.get('calMode')?.value
    }));

    this.formValidationService.validateModeSelection(calInfo.calType, [calInfo.calType]).subscribe({
      next: (validation) => {
        if (!validation.isValid) {
          const errorMessages = Object.values(validation.errors).flat();
          this.snackBar.open(errorMessages[0], 'Close', { duration: 5000 });
          return;
        }

        if (this.calibrationForm.valid) {
          const formData = {
            ...this.calibrationForm.value,
            measurementPoints: this.measurementPoints,
            isNoOrder: this.isNoOrderMode
          };
          
          console.log('Calibration data:', formData);
          this.snackBar.open('Calibration saved successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/dashboard']);
        } else {
          this.snackBar.open('Please correct the form errors before submitting', 'Close', { duration: 5000 });
        }
      },
      error: (error) => {
        console.error('Calibration validation error:', error);
        this.snackBar.open('Validation error occurred', 'Close', { duration: 3000 });
      }
    });
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
