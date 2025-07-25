import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { FormValidationService } from '../../shared/services/form-validation.service';

interface CalibrationTypeDescription {
  title: string;
  description: string;
  features: string[];
}

@Component({
  selector: 'app-calibration-type-selection',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    CommonModule
  ],
  templateUrl: './calibration-type-selection.html',
  styleUrl: './calibration-type-selection.scss'
})
export class CalibrationTypeSelection implements OnInit {
  calibrationTypeForm: FormGroup;
  selectedTypeDescription: CalibrationTypeDescription | null = null;

  private typeDescriptions: { [key: string]: CalibrationTypeDescription } = {
    'ADM': {
      title: 'Atmospheric Data Measurement (ADM)',
      description: 'Calibration for atmospheric monitoring instruments including temperature, humidity, and pressure sensors.',
      features: [
        'Temperature range: -40°C to +85°C',
        'Humidity range: 0% to 100% RH',
        'Pressure range: 300 to 1100 hPa',
        'NIST traceable standards',
        'Automated data logging'
      ]
    },
    'HDM': {
      title: 'Humidity Data Measurement (HDM)',
      description: 'Specialized calibration for humidity measurement devices and hygrometers.',
      features: [
        'Humidity range: 5% to 95% RH',
        'Temperature compensation',
        'Dew point calculations',
        'Multi-point calibration',
        'Uncertainty analysis'
      ]
    },
    'MultiTemp': {
      title: 'Multi-Temperature Calibration',
      description: 'Comprehensive temperature calibration across multiple temperature points and ranges.',
      features: [
        'Wide temperature range coverage',
        'Multiple sensor types supported',
        'Automated temperature cycling',
        'Statistical analysis',
        'Calibration certificates'
      ]
    },
    'FH': {
      title: 'Fixed Humidity (FH)',
      description: 'Calibration at specific humidity set points for specialized applications.',
      features: [
        'Fixed humidity points',
        'Stable environmental conditions',
        'Long-term stability testing',
        'Precision humidity control',
        'Custom humidity levels'
      ]
    },
    'Pressure': {
      title: 'Pressure Gauge Calibration',
      description: 'Calibration of pressure measurement instruments and gauges.',
      features: [
        'Various pressure ranges',
        'Gauge and absolute pressure',
        'Digital and analog instruments',
        'Leak testing capabilities',
        'Pressure cycling tests'
      ]
    },
    'Temperature': {
      title: 'Temperature Probe Calibration',
      description: 'Calibration of temperature sensors, probes, and thermometers.',
      features: [
        'RTD, thermocouple, thermistor support',
        'Ice point and triple point references',
        'Wide temperature range',
        'Immersion depth optimization',
        'Thermal equilibrium verification'
      ]
    },
    'Flow': {
      title: 'Flow Meter Calibration',
      description: 'Calibration of flow measurement devices for liquids and gases.',
      features: [
        'Volumetric and mass flow',
        'Various flow ranges',
        'Liquid and gas calibration',
        'Flow profile analysis',
        'Repeatability testing'
      ]
    },
    'Electrical': {
      title: 'Electrical Instrument Calibration',
      description: 'Calibration of electrical measurement instruments and meters.',
      features: [
        'Voltage, current, resistance',
        'AC and DC measurements',
        'Frequency and power',
        'Digital multimeters',
        'Oscilloscope calibration'
      ]
    }
  };

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private formValidationService: FormValidationService
  ) {
    this.calibrationTypeForm = this.formBuilder.group({
      calType: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.calibrationTypeForm.get('calType')?.valueChanges.subscribe(value => {
      this.selectedTypeDescription = value ? this.typeDescriptions[value] : null;
    });
  }

  onSubmit(): void {
    const selectedType = this.calibrationTypeForm.get('calType')?.value;
    
    if (!selectedType || selectedType.trim() === '') {
      this.snackBar.open('Please select a calibration type.', 'Close', { duration: 5000 });
      return;
    }

    this.formValidationService.validateCalibrationTypeSelection(selectedType).subscribe({
      next: (validation: any) => {
        if (!validation.isValid) {
          const errorMessages = Object.values(validation.errors).flat() as string[];
          this.snackBar.open(errorMessages[0], 'Close', { duration: 5000 });
          return;
        }

        if (this.calibrationTypeForm.valid) {
          this.snackBar.open(`Selected ${selectedType} calibration type`, 'Close', { duration: 3000 });
          this.router.navigate(['/calibration/entry', 'no-order', selectedType]);
        } else {
          this.snackBar.open('Please correct the form errors before proceeding', 'Close', { duration: 5000 });
        }
      },
      error: (error: any) => {
        console.error('Calibration type validation error:', error);
        this.router.navigate(['/calibration/entry', 'no-order', selectedType]);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/dashboard']);
  }
}
