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
import { MatChipsModule } from '@angular/material/chips';
import { CommonModule } from '@angular/common';

interface Technician {
  techId: string;
  techName: string;
  email: string;
  phone: string;
  status: 'Active' | 'Inactive' | 'Training';
  department?: string;
  hireDate: Date;
  lastActive: Date;
  supervisor?: string;
  certifications: string[];
  detailedCertifications: Certification[];
  totalCalibrations: number;
  monthlyCalibrations: number;
  averageRating: number;
  accuracyRate: number;
}

interface Certification {
  name: string;
  issuingBody: string;
  issueDate: Date;
  expiryDate: Date;
}

@Component({
  selector: 'app-technician-management',
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
    MatChipsModule,
    CommonModule
  ],
  templateUrl: './technician-management.html',
  styleUrl: './technician-management.scss'
})
export class TechnicianManagement implements OnInit {
  technicians: Technician[] = [];
  filteredTechnicians: Technician[] = [];
  selectedTechnician: Technician | null = null;
  selectedStatus = '';
  searchTerm = '';
  today = new Date();

  displayedColumns: string[] = [
    'techId', 'techName', 'email', 'phone', 
    'certifications', 'status', 'lastActive', 'actions'
  ];

  private mockTechnicians: Technician[] = [
    {
      techId: 'TECH001',
      techName: 'John Smith',
      email: 'john.smith@company.com',
      phone: '555-0123',
      status: 'Active',
      department: 'Calibration Lab',
      hireDate: new Date('2020-03-15'),
      lastActive: new Date('2024-07-24'),
      supervisor: 'Jane Manager',
      certifications: ['NIST', 'ISO 17025', 'Temperature'],
      detailedCertifications: [
        {
          name: 'NIST Traceable Calibration',
          issuingBody: 'NIST',
          issueDate: new Date('2023-01-15'),
          expiryDate: new Date('2025-01-15')
        },
        {
          name: 'ISO 17025 Laboratory Quality',
          issuingBody: 'ISO',
          issueDate: new Date('2022-06-01'),
          expiryDate: new Date('2025-06-01')
        },
        {
          name: 'Temperature Calibration Specialist',
          issuingBody: 'NCSL International',
          issueDate: new Date('2021-09-10'),
          expiryDate: new Date('2024-09-10')
        }
      ],
      totalCalibrations: 1247,
      monthlyCalibrations: 45,
      averageRating: 4.8,
      accuracyRate: 99.2
    },
    {
      techId: 'TECH002',
      techName: 'Jane Doe',
      email: 'jane.doe@company.com',
      phone: '555-0456',
      status: 'Active',
      department: 'Calibration Lab',
      hireDate: new Date('2019-08-20'),
      lastActive: new Date('2024-07-24'),
      supervisor: 'Jane Manager',
      certifications: ['NIST', 'Pressure', 'Flow'],
      detailedCertifications: [
        {
          name: 'NIST Traceable Calibration',
          issuingBody: 'NIST',
          issueDate: new Date('2023-02-01'),
          expiryDate: new Date('2025-02-01')
        },
        {
          name: 'Pressure Calibration Certification',
          issuingBody: 'NCSL International',
          issueDate: new Date('2022-11-15'),
          expiryDate: new Date('2025-11-15')
        },
        {
          name: 'Flow Measurement Specialist',
          issuingBody: 'ASME',
          issueDate: new Date('2021-04-20'),
          expiryDate: new Date('2024-04-20')
        }
      ],
      totalCalibrations: 1589,
      monthlyCalibrations: 52,
      averageRating: 4.9,
      accuracyRate: 99.5
    },
    {
      techId: 'TECH003',
      techName: 'Mike Johnson',
      email: 'mike.johnson@company.com',
      phone: '555-0789',
      status: 'Training',
      department: 'Calibration Lab',
      hireDate: new Date('2024-01-10'),
      lastActive: new Date('2024-07-23'),
      supervisor: 'John Smith',
      certifications: ['Basic Calibration'],
      detailedCertifications: [
        {
          name: 'Basic Calibration Training',
          issuingBody: 'Company Training Center',
          issueDate: new Date('2024-01-15'),
          expiryDate: new Date('2025-01-15')
        }
      ],
      totalCalibrations: 89,
      monthlyCalibrations: 15,
      averageRating: 4.2,
      accuracyRate: 96.8
    },
    {
      techId: 'TECH004',
      techName: 'Sarah Wilson',
      email: 'sarah.wilson@company.com',
      phone: '555-0321',
      status: 'Active',
      department: 'Quality Assurance',
      hireDate: new Date('2018-05-12'),
      lastActive: new Date('2024-07-24'),
      supervisor: 'Jane Manager',
      certifications: ['NIST', 'ISO 17025', 'Electrical', 'Humidity'],
      detailedCertifications: [
        {
          name: 'NIST Traceable Calibration',
          issuingBody: 'NIST',
          issueDate: new Date('2023-03-01'),
          expiryDate: new Date('2025-03-01')
        },
        {
          name: 'ISO 17025 Laboratory Quality',
          issuingBody: 'ISO',
          issueDate: new Date('2022-07-15'),
          expiryDate: new Date('2025-07-15')
        },
        {
          name: 'Electrical Calibration Specialist',
          issuingBody: 'IEEE',
          issueDate: new Date('2021-12-01'),
          expiryDate: new Date('2024-12-01')
        },
        {
          name: 'Humidity Measurement Expert',
          issuingBody: 'ASHRAE',
          issueDate: new Date('2020-10-15'),
          expiryDate: new Date('2023-10-15')
        }
      ],
      totalCalibrations: 2156,
      monthlyCalibrations: 38,
      averageRating: 4.7,
      accuracyRate: 98.9
    },
    {
      techId: 'TECH005',
      techName: 'David Brown',
      email: 'david.brown@company.com',
      phone: '555-0654',
      status: 'Inactive',
      department: 'Calibration Lab',
      hireDate: new Date('2017-11-08'),
      lastActive: new Date('2024-06-15'),
      supervisor: 'Jane Manager',
      certifications: ['NIST', 'Temperature', 'Pressure'],
      detailedCertifications: [
        {
          name: 'NIST Traceable Calibration',
          issuingBody: 'NIST',
          issueDate: new Date('2022-12-01'),
          expiryDate: new Date('2024-12-01')
        },
        {
          name: 'Temperature Calibration Specialist',
          issuingBody: 'NCSL International',
          issueDate: new Date('2021-08-20'),
          expiryDate: new Date('2024-08-20')
        }
      ],
      totalCalibrations: 1834,
      monthlyCalibrations: 0,
      averageRating: 4.6,
      accuracyRate: 98.7
    }
  ];

  ngOnInit(): void {
    this.technicians = [...this.mockTechnicians];
    this.filteredTechnicians = [...this.technicians];
  }

  filterTechnicians(): void {
    let filtered = [...this.technicians];

    if (this.selectedStatus) {
      filtered = filtered.filter(tech => tech.status === this.selectedStatus);
    }

    if (this.searchTerm) {
      filtered = filtered.filter(tech =>
        tech.techName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        tech.techId.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        tech.email.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        tech.certifications.some(cert => cert.toLowerCase().includes(this.searchTerm.toLowerCase()))
      );
    }

    this.filteredTechnicians = filtered;
  }

  selectTechnician(technician: Technician): void {
    this.selectedTechnician = technician;
  }

  addTechnician(): void {
    console.log('Adding new technician');
  }

  editTechnician(technician: Technician): void {
    console.log('Editing technician:', technician);
  }

  viewCertifications(technician: Technician): void {
    this.selectTechnician(technician);
  }

  viewCalibrations(technician: Technician): void {
    console.log('Viewing calibrations for technician:', technician.techId);
  }

  toggleStatus(technician: Technician): void {
    const newStatus = technician.status === 'Active' ? 'Inactive' : 'Active';
    console.log(`Changing status of ${technician.techName} from ${technician.status} to ${newStatus}`);
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Active': return 'primary';
      case 'Training': return 'accent';
      case 'Inactive': return 'warn';
      default: return '';
    }
  }

  getCertificationStatus(cert: Certification): string {
    const daysUntilExpiry = Math.ceil((cert.expiryDate.getTime() - this.today.getTime()) / (1000 * 60 * 60 * 24));
    
    if (daysUntilExpiry < 0) {
      return 'Expired';
    } else if (daysUntilExpiry <= 30) {
      return 'Expiring Soon';
    } else {
      return 'Valid';
    }
  }

  getCertificationStatusColor(cert: Certification): string {
    const status = this.getCertificationStatus(cert);
    switch (status) {
      case 'Valid': return 'primary';
      case 'Expiring Soon': return 'accent';
      case 'Expired': return 'warn';
      default: return '';
    }
  }
}
