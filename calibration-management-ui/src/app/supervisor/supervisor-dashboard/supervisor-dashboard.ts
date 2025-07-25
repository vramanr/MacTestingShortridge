import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatBadgeModule } from '@angular/material/badge';
import { CommonModule } from '@angular/common';

interface AdminModule {
  id: string;
  title: string;
  description: string;
  icon: string;
  iconColor: string;
  route: string;
  subModules?: AdminSubModule[];
  badgeCount?: number;
}

interface AdminSubModule {
  id: string;
  title: string;
  description: string;
  route: string;
}

@Component({
  selector: 'app-supervisor-dashboard',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatGridListModule,
    MatTooltipModule,
    MatBadgeModule,
    CommonModule
  ],
  templateUrl: './supervisor-dashboard.html',
  styleUrl: './supervisor-dashboard.scss'
})
export class SupervisorDashboard implements OnInit {
  adminModules: AdminModule[] = [];

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.initializeAdminModules();
  }

  private initializeAdminModules(): void {
    this.adminModules = [
      {
        id: 'application',
        title: 'Application Management',
        description: 'System setup, configuration, and table management',
        icon: 'settings',
        iconColor: 'primary',
        route: '/supervisor/application-setup',
        subModules: [
          {
            id: 'setup',
            title: 'Application Setup',
            description: 'Configure system settings and parameters',
            route: '/supervisor/application-setup'
          },
          {
            id: 'tables',
            title: 'Table Management',
            description: 'Browse and manage database tables',
            route: '/supervisor/application-setup?tab=tables'
          }
        ]
      },
      {
        id: 'calibration',
        title: 'Calibration Administration',
        description: 'Manage calibration setup, types, modes, standards, and technicians',
        icon: 'precision_manufacturing',
        iconColor: 'accent',
        route: '/supervisor/calibration-admin',
        subModules: [
          {
            id: 'cal-setup',
            title: 'Calibration Setup',
            description: 'Configure calibration parameters and settings',
            route: '/supervisor/calibration-admin?tab=setup'
          },
          {
            id: 'cal-types',
            title: 'Calibration Types',
            description: 'Manage calibration type definitions',
            route: '/supervisor/calibration-admin?tab=types'
          },
          {
            id: 'modes',
            title: 'Calibration Modes',
            description: 'Configure calibration mode settings',
            route: '/supervisor/calibration-admin?tab=modes'
          },
          {
            id: 'standards',
            title: 'Standards Management',
            description: 'Manage calibration standards and equipment',
            route: '/supervisor/calibration-admin?tab=standards'
          },
          {
            id: 'technicians',
            title: 'Technician Administration',
            description: 'Manage technician profiles and certifications',
            route: '/supervisor/calibration-admin?tab=technicians'
          }
        ]
      },
      {
        id: 'codes',
        title: 'Codes Management',
        description: 'Manage system codes for accounts, discounts, shipping, tax, and terms',
        icon: 'code',
        iconColor: 'primary',
        route: '/supervisor/codes-management',
        subModules: [
          {
            id: 'account',
            title: 'Account Codes',
            description: 'Manage customer account codes',
            route: '/supervisor/codes-management?tab=account'
          },
          {
            id: 'subaccount',
            title: 'Sub Account Codes',
            description: 'Manage sub-account classifications',
            route: '/supervisor/codes-management?tab=subaccount'
          },
          {
            id: 'discount',
            title: 'Discount Codes',
            description: 'Configure discount structures',
            route: '/supervisor/codes-management?tab=discount'
          },
          {
            id: 'qa',
            title: 'QA Codes',
            description: 'Quality assurance code management',
            route: '/supervisor/codes-management?tab=qa'
          },
          {
            id: 'shipping',
            title: 'Ship Via Codes',
            description: 'Shipping method configurations',
            route: '/supervisor/codes-management?tab=shipping'
          },
          {
            id: 'tax',
            title: 'Tax Codes',
            description: 'Tax calculation configurations',
            route: '/supervisor/codes-management?tab=tax'
          },
          {
            id: 'terms',
            title: 'Terms Codes',
            description: 'Payment terms configurations',
            route: '/supervisor/codes-management?tab=terms'
          }
        ]
      },
      {
        id: 'parts',
        title: 'Parts Management',
        description: 'Manage part lists, sizes, and box configurations',
        icon: 'inventory',
        iconColor: 'accent',
        route: '/supervisor/parts-management',
        subModules: [
          {
            id: 'part-list',
            title: 'Part List',
            description: 'Manage inventory part listings',
            route: '/supervisor/parts-management?tab=list'
          },
          {
            id: 'part-size',
            title: 'Part Size',
            description: 'Configure part size specifications',
            route: '/supervisor/parts-management?tab=size'
          },
          {
            id: 'box-size',
            title: 'Box Size',
            description: 'Manage packaging box sizes',
            route: '/supervisor/parts-management?tab=box'
          }
        ]
      },
      {
        id: 'process',
        title: 'Process Management',
        description: 'System maintenance, data management, and cleanup operations',
        icon: 'build',
        iconColor: 'warn',
        route: '/supervisor/process-management',
        badgeCount: 3,
        subModules: [
          {
            id: 'change-numbers',
            title: 'Change Numbers',
            description: 'Modify system numbering sequences',
            route: '/supervisor/process-management?tab=numbers'
          },
          {
            id: 'pack-database',
            title: 'Pack Database/Tables',
            description: 'Optimize and compact database tables',
            route: '/supervisor/process-management?tab=pack'
          },
          {
            id: 'delete-orders',
            title: 'Delete Orders',
            description: 'Remove obsolete order records',
            route: '/supervisor/process-management?tab=delete'
          },
          {
            id: 'duplicates',
            title: 'Duplicate Management',
            description: 'Find and resolve duplicate records',
            route: '/supervisor/process-management?tab=duplicates'
          }
        ]
      },
      {
        id: 'reports',
        title: 'Administrative Reports',
        description: 'Generate system reports and data exports',
        icon: 'assessment',
        iconColor: 'primary',
        route: '/reports?tab=admin',
        subModules: [
          {
            id: 'zip-report',
            title: 'Zip Report',
            description: 'Generate compressed data reports',
            route: '/reports?tab=admin&report=zip'
          }
        ]
      }
    ];
  }

  navigateToModule(module: AdminModule): void {
    this.router.navigate([module.route]);
  }

  navigateToSubModule(subModule: AdminSubModule): void {
    this.router.navigate([subModule.route]);
  }

  getModuleIcon(module: AdminModule): string {
    return module.icon;
  }

  getModuleColor(module: AdminModule): string {
    return module.iconColor;
  }
}
