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

interface PartItem {
  id: string;
  partNumber: string;
  description: string;
  category: string;
  manufacturer?: string;
  model?: string;
  specifications?: string;
  unitPrice?: number;
  stockQuantity?: number;
  isActive: boolean;
  lastModified: Date;
}

interface PartSize {
  id: string;
  sizeCode: string;
  description: string;
  dimensions: string;
  weight?: number;
  volume?: number;
  isStandard: boolean;
  isActive: boolean;
  lastModified: Date;
}

interface BoxSize {
  id: string;
  boxCode: string;
  description: string;
  length: number;
  width: number;
  height: number;
  maxWeight: number;
  material: string;
  isActive: boolean;
  lastModified: Date;
}

@Component({
  selector: 'app-parts-management',
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
  templateUrl: './parts-management.html',
  styleUrl: './parts-management.scss'
})
export class PartsManagement implements OnInit {
  selectedTabIndex = 0;
  partForm: FormGroup;
  sizeForm: FormGroup;
  boxForm: FormGroup;
  
  partItems: PartItem[] = [];
  partSizes: PartSize[] = [];
  boxSizes: BoxSize[] = [];

  partColumns: string[] = ['partNumber', 'description', 'category', 'manufacturer', 'unitPrice', 'stockQuantity', 'isActive', 'actions'];
  sizeColumns: string[] = ['sizeCode', 'description', 'dimensions', 'weight', 'isStandard', 'isActive', 'actions'];
  boxColumns: string[] = ['boxCode', 'description', 'dimensions', 'maxWeight', 'material', 'isActive', 'actions'];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute
  ) {
    this.partForm = this.formBuilder.group({
      partNumber: ['', Validators.required],
      description: ['', Validators.required],
      category: ['', Validators.required],
      manufacturer: [''],
      model: [''],
      specifications: [''],
      unitPrice: [0, [Validators.min(0)]],
      stockQuantity: [0, [Validators.min(0)]],
      isActive: [true]
    });

    this.sizeForm = this.formBuilder.group({
      sizeCode: ['', Validators.required],
      description: ['', Validators.required],
      dimensions: ['', Validators.required],
      weight: [0, [Validators.min(0)]],
      volume: [0, [Validators.min(0)]],
      isStandard: [false],
      isActive: [true]
    });

    this.boxForm = this.formBuilder.group({
      boxCode: ['', Validators.required],
      description: ['', Validators.required],
      length: [0, [Validators.required, Validators.min(0)]],
      width: [0, [Validators.required, Validators.min(0)]],
      height: [0, [Validators.required, Validators.min(0)]],
      maxWeight: [0, [Validators.required, Validators.min(0)]],
      material: ['', Validators.required],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.initializePartsData();
    this.initializeSizesData();
    this.initializeBoxesData();
    this.checkTabFromRoute();
  }

  private checkTabFromRoute(): void {
    this.route.queryParams.subscribe(params => {
      const tabMap: { [key: string]: number } = {
        'list': 0,
        'size': 1,
        'box': 2
      };
      
      if (params['tab'] && tabMap[params['tab']] !== undefined) {
        this.selectedTabIndex = tabMap[params['tab']];
      }
    });
  }

  private initializePartsData(): void {
    this.partItems = [
      {
        id: '1',
        partNumber: 'CAL-STD-001',
        description: 'Precision Voltage Standard',
        category: 'Calibration Standards',
        manufacturer: 'Fluke',
        model: '5720A',
        specifications: '0-1000V, ±0.01%',
        unitPrice: 15000.00,
        stockQuantity: 2,
        isActive: true,
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        partNumber: 'CAL-STD-002',
        description: 'Current Standard',
        category: 'Calibration Standards',
        manufacturer: 'Keysight',
        model: '3458A',
        specifications: '0-20A, ±0.005%',
        unitPrice: 12000.00,
        stockQuantity: 1,
        isActive: true,
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        partNumber: 'CAL-ACC-001',
        description: 'Test Leads Set',
        category: 'Accessories',
        manufacturer: 'Pomona',
        model: 'TL-1000',
        specifications: '1000V CAT III',
        unitPrice: 150.00,
        stockQuantity: 25,
        isActive: true,
        lastModified: new Date('2024-07-15')
      },
      {
        id: '4',
        partNumber: 'CAL-CERT-001',
        description: 'Calibration Certificate Template',
        category: 'Documentation',
        manufacturer: 'Internal',
        specifications: 'ISO 17025 Compliant',
        unitPrice: 0.00,
        stockQuantity: 999,
        isActive: true,
        lastModified: new Date('2024-07-10')
      }
    ];
  }

  private initializeSizesData(): void {
    this.partSizes = [
      {
        id: '1',
        sizeCode: 'SM',
        description: 'Small Instrument',
        dimensions: '6" x 4" x 2"',
        weight: 2.5,
        volume: 48,
        isStandard: true,
        isActive: true,
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        sizeCode: 'MD',
        description: 'Medium Instrument',
        dimensions: '12" x 8" x 4"',
        weight: 8.0,
        volume: 384,
        isStandard: true,
        isActive: true,
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        sizeCode: 'LG',
        description: 'Large Instrument',
        dimensions: '18" x 12" x 6"',
        weight: 20.0,
        volume: 1296,
        isStandard: true,
        isActive: true,
        lastModified: new Date('2024-07-15')
      },
      {
        id: '4',
        sizeCode: 'XL',
        description: 'Extra Large Instrument',
        dimensions: '24" x 18" x 8"',
        weight: 45.0,
        volume: 3456,
        isStandard: false,
        isActive: true,
        lastModified: new Date('2024-07-12')
      }
    ];
  }

  private initializeBoxesData(): void {
    this.boxSizes = [
      {
        id: '1',
        boxCode: 'BOX-SM',
        description: 'Small Shipping Box',
        length: 8,
        width: 6,
        height: 4,
        maxWeight: 5,
        material: 'Corrugated Cardboard',
        isActive: true,
        lastModified: new Date('2024-07-20')
      },
      {
        id: '2',
        boxCode: 'BOX-MD',
        description: 'Medium Shipping Box',
        length: 14,
        width: 10,
        height: 6,
        maxWeight: 15,
        material: 'Corrugated Cardboard',
        isActive: true,
        lastModified: new Date('2024-07-18')
      },
      {
        id: '3',
        boxCode: 'BOX-LG',
        description: 'Large Shipping Box',
        length: 20,
        width: 14,
        height: 8,
        maxWeight: 30,
        material: 'Double Wall Cardboard',
        isActive: true,
        lastModified: new Date('2024-07-15')
      },
      {
        id: '4',
        boxCode: 'BOX-CRATE',
        description: 'Wooden Shipping Crate',
        length: 30,
        width: 24,
        height: 12,
        maxWeight: 100,
        material: 'Plywood',
        isActive: true,
        lastModified: new Date('2024-07-10')
      }
    ];
  }

  addPart(): void {
    if (this.partForm.valid) {
      const newPart: PartItem = {
        id: Date.now().toString(),
        partNumber: this.partForm.value.partNumber,
        description: this.partForm.value.description,
        category: this.partForm.value.category,
        manufacturer: this.partForm.value.manufacturer,
        model: this.partForm.value.model,
        specifications: this.partForm.value.specifications,
        unitPrice: this.partForm.value.unitPrice,
        stockQuantity: this.partForm.value.stockQuantity,
        isActive: this.partForm.value.isActive,
        lastModified: new Date()
      };

      this.partItems.push(newPart);
      this.partForm.reset({ isActive: true });
      console.log('Added new part:', newPart);
    }
  }

  addSize(): void {
    if (this.sizeForm.valid) {
      const newSize: PartSize = {
        id: Date.now().toString(),
        sizeCode: this.sizeForm.value.sizeCode,
        description: this.sizeForm.value.description,
        dimensions: this.sizeForm.value.dimensions,
        weight: this.sizeForm.value.weight,
        volume: this.sizeForm.value.volume,
        isStandard: this.sizeForm.value.isStandard,
        isActive: this.sizeForm.value.isActive,
        lastModified: new Date()
      };

      this.partSizes.push(newSize);
      this.sizeForm.reset({ isActive: true });
      console.log('Added new size:', newSize);
    }
  }

  addBox(): void {
    if (this.boxForm.valid) {
      const newBox: BoxSize = {
        id: Date.now().toString(),
        boxCode: this.boxForm.value.boxCode,
        description: this.boxForm.value.description,
        length: this.boxForm.value.length,
        width: this.boxForm.value.width,
        height: this.boxForm.value.height,
        maxWeight: this.boxForm.value.maxWeight,
        material: this.boxForm.value.material,
        isActive: this.boxForm.value.isActive,
        lastModified: new Date()
      };

      this.boxSizes.push(newBox);
      this.boxForm.reset({ isActive: true });
      console.log('Added new box:', newBox);
    }
  }

  editPart(part: PartItem): void {
    this.partForm.patchValue(part);
    console.log('Editing part:', part);
  }

  editSize(size: PartSize): void {
    this.sizeForm.patchValue(size);
    console.log('Editing size:', size);
  }

  editBox(box: BoxSize): void {
    this.boxForm.patchValue(box);
    console.log('Editing box:', box);
  }

  deletePart(part: PartItem): void {
    const index = this.partItems.findIndex(p => p.id === part.id);
    if (index > -1) {
      this.partItems.splice(index, 1);
      console.log('Deleted part:', part);
    }
  }

  deleteSize(size: PartSize): void {
    const index = this.partSizes.findIndex(s => s.id === size.id);
    if (index > -1) {
      this.partSizes.splice(index, 1);
      console.log('Deleted size:', size);
    }
  }

  deleteBox(box: BoxSize): void {
    const index = this.boxSizes.findIndex(b => b.id === box.id);
    if (index > -1) {
      this.boxSizes.splice(index, 1);
      console.log('Deleted box:', box);
    }
  }

  togglePartStatus(part: PartItem): void {
    part.isActive = !part.isActive;
    part.lastModified = new Date();
    console.log('Toggled part status:', part);
  }

  toggleSizeStatus(size: PartSize): void {
    size.isActive = !size.isActive;
    size.lastModified = new Date();
    console.log('Toggled size status:', size);
  }

  toggleBoxStatus(box: BoxSize): void {
    box.isActive = !box.isActive;
    box.lastModified = new Date();
    console.log('Toggled box status:', box);
  }

  getBoxDimensions(box: BoxSize): string {
    return `${box.length}" x ${box.width}" x ${box.height}"`;
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(value);
  }
}
