import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { CommonModule } from '@angular/common';

interface RecentActivity {
  icon: string;
  title: string;
  description: string;
  timestamp: Date;
}

@Component({
  selector: 'app-dashboard',
  imports: [
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    CommonModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard {
  totalCalibrations = 156;
  pendingOrders = 23;
  activeCompanies = 45;
  dueSoon = 8;

  recentActivities: RecentActivity[] = [
    {
      icon: 'assignment_turned_in',
      title: 'ADM Calibration Completed',
      description: 'Order #12345 - Acme Corp thermometer calibration',
      timestamp: new Date(Date.now() - 2 * 60 * 60 * 1000)
    },
    {
      icon: 'add_circle',
      title: 'New Order Created',
      description: 'Order #12346 - TechCorp pressure gauge calibration',
      timestamp: new Date(Date.now() - 4 * 60 * 60 * 1000)
    },
    {
      icon: 'edit',
      title: 'Calibration Record Updated',
      description: 'HDM calibration for serial #ABC123',
      timestamp: new Date(Date.now() - 6 * 60 * 60 * 1000)
    },
    {
      icon: 'business',
      title: 'New Company Added',
      description: 'Precision Instruments LLC registered',
      timestamp: new Date(Date.now() - 8 * 60 * 60 * 1000)
    },
    {
      icon: 'schedule',
      title: 'Calibration Due Reminder',
      description: '5 instruments due for recalibration this week',
      timestamp: new Date(Date.now() - 12 * 60 * 60 * 1000)
    }
  ];
}
