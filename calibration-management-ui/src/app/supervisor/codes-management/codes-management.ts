import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';

interface CodeItem {
  id: string;
  code: string;
  description: string;
  value?: string;
  percentage?: number;
  isActive: boolean;
  lastModified: Date;
  category: string;
}

@Component({
  selector: 'app-codes-management',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatTabsModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatTooltipModule,
    MatCheckboxModule,
    CommonModule
  ],
  templateUrl: './codes-management.html',
  styleUrl: './codes-management.scss'
})
export class CodesManagement implements OnInit {
  selectedTabIndex = 0;
  codeForm: FormGroup;
  
  accountCodes: CodeItem[] = [];
  subAccountCodes: CodeItem[] = [];
  discountCodes: CodeItem[] = [];
  qaCodes: CodeItem[] = [];
  shippingCodes: CodeItem[] = [];
  taxCodes: CodeItem[] = [];
  termsCodes: CodeItem[] = [];

  displayedColumns: string[] = ['code', 'description', 'value', 'isActive', 'lastModified', 'actions'];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute
  ) {
    this.codeForm = this.formBuilder.group({
      code: ['', Validators.required],
      description: ['', Validators.required],
      value: [''],
      percentage: [0],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.initializeCodeData();
    this.checkTabFromRoute();
  }

  private checkTabFromRoute(): void {
    this.route.queryParams.subscribe(params => {
      const tabMap: { [key: string]: number } = {
        'account': 0,
        'subaccount': 1,
        'discount': 2,
        'qa': 3,
        'shipping': 4,
        'tax': 5,
        'terms': 6
      };
      
      if (params['tab'] && tabMap[params['tab']] !== undefined) {
        this.selectedTabIndex = tabMap[params['tab']];
      }
    });
  }

  private initializeCodeData(): void {
    this.accountCodes = [
      {
        id: '1',
        code: 'ACCT001',
        description: 'Standard Account',
        value: 'STD',
        isActive: true,
        lastModified: new Date('2024-07-20'),
        category: 'account'
      },
      {
        id: '2',
        code: 'ACCT002',
        description: 'Premium Account',
        value: 'PREM',
        isActive: true,
        lastModified: new Date('2024-07-18'),
        category: 'account'
      },
      {
        id: '3',
        code: 'ACCT003',
        description: 'Government Account',
        value: 'GOV',
        isActive: true,
        lastModified: new Date('2024-07-15'),
        category: 'account'
      }
    ];

    this.subAccountCodes = [
      {
        id: '1',
        code: 'SUB001',
        description: 'Engineering Division',
        value: 'ENG',
        isActive: true,
        lastModified: new Date('2024-07-19'),
        category: 'subaccount'
      },
      {
        id: '2',
        code: 'SUB002',
        description: 'Quality Assurance',
        value: 'QA',
        isActive: true,
        lastModified: new Date('2024-07-17'),
        category: 'subaccount'
      }
    ];

    this.discountCodes = [
      {
        id: '1',
        code: 'DISC10',
        description: '10% Volume Discount',
        percentage: 10,
        isActive: true,
        lastModified: new Date('2024-07-20'),
        category: 'discount'
      },
      {
        id: '2',
        code: 'DISC15',
        description: '15% Preferred Customer',
        percentage: 15,
        isActive: true,
        lastModified: new Date('2024-07-18'),
        category: 'discount'
      },
      {
        id: '3',
        code: 'DISC25',
        description: '25% Government Discount',
        percentage: 25,
        isActive: true,
        lastModified: new Date('2024-07-16'),
        category: 'discount'
      }
    ];

    this.qaCodes = [
      {
        id: '1',
        code: 'QA001',
        description: 'ISO 9001 Compliant',
        value: 'ISO9001',
        isActive: true,
        lastModified: new Date('2024-07-19'),
        category: 'qa'
      },
      {
        id: '2',
        code: 'QA002',
        description: 'AS9100 Aerospace',
        value: 'AS9100',
        isActive: true,
        lastModified: new Date('2024-07-17'),
        category: 'qa'
      }
    ];

    this.shippingCodes = [
      {
        id: '1',
        code: 'SHIP001',
        description: 'UPS Ground',
        value: 'UPS_GND',
        isActive: true,
        lastModified: new Date('2024-07-20'),
        category: 'shipping'
      },
      {
        id: '2',
        code: 'SHIP002',
        description: 'FedEx Overnight',
        value: 'FEDEX_ON',
        isActive: true,
        lastModified: new Date('2024-07-19'),
        category: 'shipping'
      },
      {
        id: '3',
        code: 'SHIP003',
        description: 'Customer Pickup',
        value: 'PICKUP',
        isActive: true,
        lastModified: new Date('2024-07-18'),
        category: 'shipping'
      }
    ];

    this.taxCodes = [
      {
        id: '1',
        code: 'TAX001',
        description: 'California Sales Tax',
        percentage: 8.25,
        isActive: true,
        lastModified: new Date('2024-07-20'),
        category: 'tax'
      },
      {
        id: '2',
        code: 'TAX002',
        description: 'Tax Exempt',
        percentage: 0,
        isActive: true,
        lastModified: new Date('2024-07-19'),
        category: 'tax'
      }
    ];

    this.termsCodes = [
      {
        id: '1',
        code: 'NET30',
        description: 'Net 30 Days',
        value: '30',
        isActive: true,
        lastModified: new Date('2024-07-20'),
        category: 'terms'
      },
      {
        id: '2',
        code: 'NET15',
        description: 'Net 15 Days',
        value: '15',
        isActive: true,
        lastModified: new Date('2024-07-19'),
        category: 'terms'
      },
      {
        id: '3',
        code: 'COD',
        description: 'Cash on Delivery',
        value: '0',
        isActive: true,
        lastModified: new Date('2024-07-18'),
        category: 'terms'
      }
    ];
  }

  getCurrentCodes(): CodeItem[] {
    switch (this.selectedTabIndex) {
      case 0: return this.accountCodes;
      case 1: return this.subAccountCodes;
      case 2: return this.discountCodes;
      case 3: return this.qaCodes;
      case 4: return this.shippingCodes;
      case 5: return this.taxCodes;
      case 6: return this.termsCodes;
      default: return [];
    }
  }

  getCurrentCategory(): string {
    const categories = ['account', 'subaccount', 'discount', 'qa', 'shipping', 'tax', 'terms'];
    return categories[this.selectedTabIndex] || 'account';
  }

  addCode(): void {
    if (this.codeForm.valid) {
      const newCode: CodeItem = {
        id: Date.now().toString(),
        code: this.codeForm.value.code,
        description: this.codeForm.value.description,
        value: this.codeForm.value.value,
        percentage: this.codeForm.value.percentage,
        isActive: this.codeForm.value.isActive,
        lastModified: new Date(),
        category: this.getCurrentCategory()
      };

      this.getCurrentCodes().push(newCode);
      this.codeForm.reset({ isActive: true });
      console.log('Added new code:', newCode);
    }
  }

  editCode(code: CodeItem): void {
    this.codeForm.patchValue({
      code: code.code,
      description: code.description,
      value: code.value,
      percentage: code.percentage,
      isActive: code.isActive
    });
    console.log('Editing code:', code);
  }

  deleteCode(code: CodeItem): void {
    const codes = this.getCurrentCodes();
    const index = codes.findIndex(c => c.id === code.id);
    if (index > -1) {
      codes.splice(index, 1);
      console.log('Deleted code:', code);
    }
  }

  toggleCodeStatus(code: CodeItem): void {
    code.isActive = !code.isActive;
    code.lastModified = new Date();
    console.log('Toggled code status:', code);
  }

  getValueDisplay(code: CodeItem): string {
    if (code.percentage !== undefined) {
      return `${code.percentage}%`;
    }
    return code.value || '-';
  }

  isPercentageField(): boolean {
    return this.selectedTabIndex === 2 || this.selectedTabIndex === 5; // discount or tax
  }

  getTabTitle(index: number): string {
    const titles = [
      'Account Codes',
      'Sub Account Codes', 
      'Discount Codes',
      'QA Codes',
      'Ship Via Codes',
      'Tax Codes',
      'Terms Codes'
    ];
    return titles[index] || '';
  }
}
