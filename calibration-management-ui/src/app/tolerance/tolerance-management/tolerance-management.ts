import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatExpansionModule } from '@angular/material/expansion';
import { CommonModule } from '@angular/common';

interface Tolerance {
  toleranceId: string;
  calType: string;
  parameter: string;
  rangeMin: number;
  rangeMax: number;
  units: string;
  toleranceValue: number;
  toleranceType: 'absolute' | 'percentage';
  uncertainty: number;
  uncertaintyType: 'absolute' | 'percentage';
  standard: string;
  description?: string;
  lastUpdated: Date;
}

@Component({
  selector: 'app-tolerance-management',
  imports: [
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    MatTooltipModule,
    MatExpansionModule,
    CommonModule
  ],
  templateUrl: './tolerance-management.html',
  styleUrl: './tolerance-management.scss'
})
export class ToleranceManagement implements OnInit {
  tolerances: Tolerance[] = [];
  filteredTolerances: Tolerance[] = [];
  selectedTolerance: Tolerance | null = null;
  selectedCalType = '';
  searchTerm = '';

  displayedColumns: string[] = [
    'calType', 'parameter', 'rangeMin', 'rangeMax', 
    'tolerance', 'uncertainty', 'standard', 'actions'
  ];

  private mockTolerances: Tolerance[] = [
    {
      toleranceId: 'TOL001',
      calType: 'ADM',
      parameter: 'Temperature',
      rangeMin: -40,
      rangeMax: 85,
      units: '°C',
      toleranceValue: 0.5,
      toleranceType: 'absolute',
      uncertainty: 0.1,
      uncertaintyType: 'absolute',
      standard: 'NIST',
      description: 'Atmospheric temperature measurement tolerance',
      lastUpdated: new Date('2024-01-15')
    },
    {
      toleranceId: 'TOL002',
      calType: 'ADM',
      parameter: 'Humidity',
      rangeMin: 0,
      rangeMax: 100,
      units: '%RH',
      toleranceValue: 2,
      toleranceType: 'percentage',
      uncertainty: 0.5,
      uncertaintyType: 'percentage',
      standard: 'NIST',
      description: 'Atmospheric humidity measurement tolerance',
      lastUpdated: new Date('2024-01-15')
    },
    {
      toleranceId: 'TOL003',
      calType: 'HDM',
      parameter: 'Humidity',
      rangeMin: 5,
      rangeMax: 95,
      units: '%RH',
      toleranceValue: 1.5,
      toleranceType: 'percentage',
      uncertainty: 0.3,
      uncertaintyType: 'percentage',
      standard: 'NIST',
      description: 'High precision humidity measurement tolerance',
      lastUpdated: new Date('2024-01-20')
    },
    {
      toleranceId: 'TOL004',
      calType: 'Pressure',
      parameter: 'Pressure',
      rangeMin: 0,
      rangeMax: 1000,
      units: 'psi',
      toleranceValue: 0.25,
      toleranceType: 'percentage',
      uncertainty: 0.05,
      uncertaintyType: 'percentage',
      standard: 'NIST',
      description: 'Pressure gauge calibration tolerance',
      lastUpdated: new Date('2024-01-25')
    },
    {
      toleranceId: 'TOL005',
      calType: 'Temperature',
      parameter: 'Temperature',
      rangeMin: -200,
      rangeMax: 500,
      units: '°C',
      toleranceValue: 0.1,
      toleranceType: 'absolute',
      uncertainty: 0.02,
      uncertaintyType: 'absolute',
      standard: 'NIST',
      description: 'High precision temperature probe tolerance',
      lastUpdated: new Date('2024-02-01')
    },
    {
      toleranceId: 'TOL006',
      calType: 'Flow',
      parameter: 'Flow Rate',
      rangeMin: 0,
      rangeMax: 1000,
      units: 'L/min',
      toleranceValue: 1,
      toleranceType: 'percentage',
      uncertainty: 0.2,
      uncertaintyType: 'percentage',
      standard: 'NIST',
      description: 'Flow meter calibration tolerance',
      lastUpdated: new Date('2024-02-05')
    },
    {
      toleranceId: 'TOL007',
      calType: 'Electrical',
      parameter: 'Voltage',
      rangeMin: 0,
      rangeMax: 1000,
      units: 'V',
      toleranceValue: 0.1,
      toleranceType: 'percentage',
      uncertainty: 0.02,
      uncertaintyType: 'percentage',
      standard: 'NIST',
      description: 'Electrical voltage measurement tolerance',
      lastUpdated: new Date('2024-02-10')
    }
  ];

  ngOnInit(): void {
    this.tolerances = [...this.mockTolerances];
    this.filteredTolerances = [...this.tolerances];
  }

  filterTolerances(): void {
    let filtered = [...this.tolerances];

    if (this.selectedCalType) {
      filtered = filtered.filter(tolerance => tolerance.calType === this.selectedCalType);
    }

    if (this.searchTerm) {
      filtered = filtered.filter(tolerance =>
        tolerance.parameter.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        tolerance.standard.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        tolerance.description?.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }

    this.filteredTolerances = filtered;
  }

  selectTolerance(tolerance: Tolerance): void {
    this.selectedTolerance = tolerance;
  }

  addTolerance(): void {
    console.log('Adding new tolerance');
  }

  editTolerance(tolerance: Tolerance): void {
    console.log('Editing tolerance:', tolerance);
  }

  duplicateTolerance(tolerance: Tolerance): void {
    console.log('Duplicating tolerance:', tolerance);
  }

  deleteTolerance(tolerance: Tolerance): void {
    console.log('Deleting tolerance:', tolerance);
  }
}
