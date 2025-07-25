import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormValidationService } from '../../shared/services/form-validation.service';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';

interface OrderRecord {
  orderId: string;
  orderNo: string;
  companyName: string;
  orderDate: Date;
  dueDate: Date;
  status: string;
  itemCount: number;
  subtotal: number;
  tax: number;
  total: number;
  contactName?: string;
  phone?: string;
  email?: string;
  items?: OrderItem[];
}

interface OrderItem {
  description: string;
  serialNo: string;
  calType: string;
  price: number;
  status: string;
}

interface Company {
  coId: string;
  coName: string;
}

@Component({
  selector: 'app-order-search',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatPaginatorModule,
    MatChipsModule,
    MatTooltipModule,
    CommonModule
  ],
  templateUrl: './order-search.html',
  styleUrl: './order-search.scss'
})
export class OrderSearch implements OnInit {
  searchForm: FormGroup;
  searchResults: OrderRecord[] = [];
  selectedOrder: OrderRecord | null = null;
  hasSearched = false;

  displayedColumns: string[] = [
    'orderNo', 'company', 'orderDate', 'dueDate', 
    'status', 'itemCount', 'total', 'actions'
  ];

  itemColumns: string[] = [
    'description', 'serialNo', 'calType', 'price', 'status'
  ];

  companies: Company[] = [
    { coId: 'ACME', coName: 'Acme Corporation' },
    { coId: 'TECH', coName: 'TechCorp Industries' },
    { coId: 'PREC', coName: 'Precision Instruments LLC' },
    { coId: 'METRO', coName: 'Metro Calibration Services' },
    { coId: 'QUAL', coName: 'Quality Assurance Labs' }
  ];

  private mockOrders: OrderRecord[] = [
    {
      orderId: 'ORD001',
      orderNo: 'ORD-2024-001',
      companyName: 'Acme Corporation',
      orderDate: new Date('2024-01-10'),
      dueDate: new Date('2024-01-25'),
      status: 'In Progress',
      itemCount: 3,
      subtotal: 750.00,
      tax: 60.00,
      total: 810.00,
      contactName: 'John Smith',
      phone: '555-0123',
      email: 'john.smith@acme.com',
      items: [
        { description: 'Temperature Probe Calibration', serialNo: 'TP-001', calType: 'Temperature', price: 250.00, status: 'Complete' },
        { description: 'Pressure Gauge Calibration', serialNo: 'PG-002', calType: 'Pressure', price: 300.00, status: 'In Progress' },
        { description: 'Humidity Sensor Calibration', serialNo: 'HS-003', calType: 'HDM', price: 200.00, status: 'Pending' }
      ]
    },
    {
      orderId: 'ORD002',
      orderNo: 'ORD-2024-002',
      companyName: 'TechCorp Industries',
      orderDate: new Date('2024-01-15'),
      dueDate: new Date('2024-02-01'),
      status: 'Open',
      itemCount: 2,
      subtotal: 500.00,
      tax: 40.00,
      total: 540.00,
      contactName: 'Jane Doe',
      phone: '555-0456',
      email: 'jane.doe@techcorp.com',
      items: [
        { description: 'ADM Calibration', serialNo: 'ADM-004', calType: 'ADM', price: 300.00, status: 'Pending' },
        { description: 'Flow Meter Calibration', serialNo: 'FM-005', calType: 'Flow', price: 200.00, status: 'Pending' }
      ]
    },
    {
      orderId: 'ORD003',
      orderNo: 'ORD-2024-003',
      companyName: 'Precision Instruments LLC',
      orderDate: new Date('2024-01-20'),
      dueDate: new Date('2024-02-10'),
      status: 'Complete',
      itemCount: 1,
      subtotal: 400.00,
      tax: 32.00,
      total: 432.00,
      contactName: 'Mike Johnson',
      phone: '555-0789',
      email: 'mike.johnson@precision.com',
      items: [
        { description: 'Electrical Meter Calibration', serialNo: 'EM-006', calType: 'Electrical', price: 400.00, status: 'Complete' }
      ]
    }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private formValidationService: FormValidationService
  ) {
    this.searchForm = this.formBuilder.group({
      orderNo: ['', [FormValidationService.orderNumberValidator()]],
      coId: ['', [FormValidationService.companyCodeValidator()]],
      status: [''],
      dateFrom: [''],
      dateTo: ['']
    });
  }

  ngOnInit(): void {
    this.searchResults = [...this.mockOrders];
  }

  onSearch(): void {
    const orderNo = this.searchForm.get('orderNo')?.value || '';
    const coId = this.searchForm.get('coId')?.value || '';
    
    if (!orderNo.trim() && !coId.trim()) {
      this.snackBar.open('Search Criteria must be entered!', 'Close', { duration: 5000 });
      return;
    }

    this.formValidationService.validateOrderSearch(coId, orderNo).subscribe({
      next: (validation) => {
        if (!validation.isValid) {
          const errorMessages = Object.values(validation.errors).flat();
          this.snackBar.open(errorMessages[0], 'Close', { duration: 5000 });
          return;
        }

        this.performSearch();
      },
      error: (error) => {
        console.error('Order search validation error:', error);
        this.performSearch(); // Continue with search even if validation fails
      }
    });
  }

  private performSearch(): void {
    this.hasSearched = true;
    const criteria = this.searchForm.value;
    
    this.searchResults = this.mockOrders.filter(order => {
      return (!criteria.orderNo || order.orderNo.toLowerCase().includes(criteria.orderNo.toLowerCase())) &&
             (!criteria.coId || this.getCompanyName(criteria.coId) === order.companyName) &&
             (!criteria.status || order.status === criteria.status) &&
             (!criteria.dateFrom || order.orderDate >= criteria.dateFrom) &&
             (!criteria.dateTo || order.orderDate <= criteria.dateTo);
    });

    if (this.searchResults.length === 0) {
      this.snackBar.open('Company or Order not found.', 'Close', { duration: 3000 });
    } else if (this.searchResults.length === 1) {
      const order = this.searchResults[0];
      this.handleSingleOrderFound(order);
    } else {
      this.snackBar.open(`Found ${this.searchResults.length} orders`, 'Close', { duration: 3000 });
    }

    console.log('Search criteria:', criteria);
    console.log('Search results:', this.searchResults);
  }

  onClear(): void {
    this.searchForm.reset();
    this.searchResults = [...this.mockOrders];
    this.selectedOrder = null;
    this.hasSearched = false;
  }

  selectOrder(order: OrderRecord): void {
    this.selectedOrder = order;
  }

  clearSelection(): void {
    this.selectedOrder = null;
  }

  viewOrder(order: OrderRecord): void {
    this.selectedOrder = order;
  }

  editOrder(order: OrderRecord): void {
    console.log('Editing order:', order);
  }

  addCalibration(order: OrderRecord): void {
    this.router.navigate(['/calibration/entry'], { 
      queryParams: { orderNo: order.orderNo, coId: this.getCompanyIdByName(order.companyName) }
    });
  }

  printOrder(order: OrderRecord): void {
    console.log('Printing order:', order);
  }

  createNewOrder(): void {
    console.log('Creating new order');
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Complete': return 'primary';
      case 'In Progress': return 'accent';
      case 'Open': return 'warn';
      case 'Shipped': return 'primary';
      case 'Cancelled': return '';
      default: return '';
    }
  }

  private getCompanyName(coId: string): string {
    const company = this.companies.find(c => c.coId === coId);
    return company ? company.coName : '';
  }

  private  getCompanyIdByName(companyName: string): string {
    const company = this.companies.find(c => c.coName === companyName);
    return company ? company.coId : '';
  }

  private handleSingleOrderFound(order: any): void {
    const orderData = {
      orderNo: order.orderNo,
      coId: this.getCompanyIdByName(order.companyName),
      companyName: order.companyName,
    };

    this.router.navigate(['/calibration/type-selection'], { 
      queryParams: { 
        orderNo: order.orderNo,
        coId: this.getCompanyIdByName(order.companyName)
      } 
    });
  }

  getErrorMessage(fieldName: string): string {
    const field = this.searchForm.get(fieldName);
    if (field?.errors) {
      const firstError = Object.keys(field.errors)[0];
      return field.errors[firstError]?.message || `${fieldName} is invalid`;
    }
    return '';
  }
}
