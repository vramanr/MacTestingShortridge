import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';

interface Company {
  coId: string;
  coName: string;
  address1: string;
  city: string;
  state: string;
  zip: string;
  phone: string;
  fax?: string;
  email?: string;
}

interface Contact {
  contactId: string;
  coId: string;
  contactName: string;
  title: string;
  phone: string;
  email: string;
}

interface ModelNumber {
  modelId: string;
  coId: string;
  modelNo: string;
  description: string;
  calType: string;
}

@Component({
  selector: 'app-company-management',
  imports: [
    FormsModule,
    MatCardModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatTooltipModule,
    CommonModule
  ],
  templateUrl: './company-management.html',
  styleUrl: './company-management.scss'
})
export class CompanyManagement implements OnInit {
  companies: Company[] = [];
  filteredCompanies: Company[] = [];
  selectedCompany: Company | null = null;
  companyContacts: Contact[] = [];
  companyModelNumbers: ModelNumber[] = [];
  companySearchTerm = '';

  companyColumns: string[] = ['coId', 'coName', 'address', 'city', 'phone', 'actions'];
  contactColumns: string[] = ['contactName', 'title', 'phone', 'email', 'actions'];
  modelColumns: string[] = ['modelNo', 'description', 'calType', 'actions'];

  private mockCompanies: Company[] = [
    {
      coId: 'ACME',
      coName: 'Acme Corporation',
      address1: '123 Main Street',
      city: 'New York',
      state: 'NY',
      zip: '10001',
      phone: '555-0123',
      email: 'info@acme.com'
    },
    {
      coId: 'TECH',
      coName: 'TechCorp Industries',
      address1: '456 Tech Avenue',
      city: 'San Francisco',
      state: 'CA',
      zip: '94102',
      phone: '555-0456',
      email: 'contact@techcorp.com'
    },
    {
      coId: 'PREC',
      coName: 'Precision Instruments LLC',
      address1: '789 Precision Way',
      city: 'Boston',
      state: 'MA',
      zip: '02101',
      phone: '555-0789',
      email: 'sales@precision.com'
    }
  ];

  private mockContacts: Contact[] = [
    {
      contactId: 'C001',
      coId: 'ACME',
      contactName: 'John Smith',
      title: 'Quality Manager',
      phone: '555-0123',
      email: 'john.smith@acme.com'
    },
    {
      contactId: 'C002',
      coId: 'TECH',
      contactName: 'Jane Doe',
      title: 'Lab Director',
      phone: '555-0456',
      email: 'jane.doe@techcorp.com'
    }
  ];

  private mockModelNumbers: ModelNumber[] = [
    {
      modelId: 'M001',
      coId: 'ACME',
      modelNo: 'TH-2000',
      description: 'Temperature/Humidity Sensor',
      calType: 'ADM'
    },
    {
      modelId: 'M002',
      coId: 'TECH',
      modelNo: 'PG-500',
      description: 'Pressure Gauge',
      calType: 'Pressure'
    }
  ];

  ngOnInit(): void {
    this.companies = [...this.mockCompanies];
    this.filteredCompanies = [...this.companies];
  }

  filterCompanies(): void {
    if (!this.companySearchTerm) {
      this.filteredCompanies = [...this.companies];
    } else {
      this.filteredCompanies = this.companies.filter(company =>
        company.coName.toLowerCase().includes(this.companySearchTerm.toLowerCase()) ||
        company.coId.toLowerCase().includes(this.companySearchTerm.toLowerCase())
      );
    }
  }

  selectCompany(company: Company): void {
    this.selectedCompany = company;
    this.loadCompanyContacts(company.coId);
    this.loadCompanyModelNumbers(company.coId);
  }

  loadCompanyContacts(coId: string): void {
    this.companyContacts = this.mockContacts.filter(contact => contact.coId === coId);
  }

  loadCompanyModelNumbers(coId: string): void {
    this.companyModelNumbers = this.mockModelNumbers.filter(model => model.coId === coId);
  }

  addCompany(): void {
    console.log('Adding new company');
  }

  editCompany(company: Company): void {
    console.log('Editing company:', company);
  }

  deleteCompany(company: Company): void {
    console.log('Deleting company:', company);
  }

  viewContacts(company: Company): void {
    this.selectCompany(company);
  }

  viewModelNumbers(company: Company): void {
    this.selectCompany(company);
  }

  addContact(): void {
    console.log('Adding new contact for company:', this.selectedCompany?.coId);
  }

  editContact(contact: Contact): void {
    console.log('Editing contact:', contact);
  }

  deleteContact(contact: Contact): void {
    console.log('Deleting contact:', contact);
  }

  addModelNumber(): void {
    console.log('Adding new model number for company:', this.selectedCompany?.coId);
  }

  editModelNumber(model: ModelNumber): void {
    console.log('Editing model number:', model);
  }

  deleteModelNumber(model: ModelNumber): void {
    console.log('Deleting model number:', model);
  }
}
