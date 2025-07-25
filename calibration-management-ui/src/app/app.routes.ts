import { Routes } from '@angular/router';
import { Dashboard } from './dashboard/dashboard';
import { CalibrationEntry } from './calibration/calibration-entry/calibration-entry';
import { CalibrationSearch } from './calibration/calibration-search/calibration-search';
import { CalibrationEdit } from './calibration/calibration-edit/calibration-edit';
import { CalibrationTypeSelection } from './calibration/calibration-type-selection/calibration-type-selection';
import { OrderSearch } from './order/order-search/order-search';
import { CompanyManagement } from './company/company-management/company-management';
import { ToleranceManagement } from './tolerance/tolerance-management/tolerance-management';
import { TechnicianManagement } from './technician/technician-management/technician-management';
import { ReportsDashboard } from './reports/reports-dashboard/reports-dashboard';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Dashboard },
  { path: 'calibration/entry', component: CalibrationEntry },
  { path: 'calibration/entry/:mode', component: CalibrationEntry },
  { path: 'calibration/entry/:mode/:calType', component: CalibrationEntry },
  { path: 'calibration/search', component: CalibrationSearch },
  { path: 'calibration/edit/:id', component: CalibrationEdit },
  { path: 'calibration/type-selection', component: CalibrationTypeSelection },
  { path: 'order/search', component: OrderSearch },
  { path: 'company/management', component: CompanyManagement },
  { path: 'tolerance/management', component: ToleranceManagement },
  { path: 'technician/management', component: TechnicianManagement },
  { path: 'reports', component: ReportsDashboard },
  { path: '**', redirectTo: '/dashboard' }
];
