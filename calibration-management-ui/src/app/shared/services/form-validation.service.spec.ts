import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { FormValidationService, ValidationResult } from './form-validation.service';

describe('FormValidationService', () => {
  let service: FormValidationService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [FormValidationService]
    });
    service = TestBed.inject(FormValidationService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should validate search criteria with valid inputs', () => {
    const result = service.validateSearchCriteria('12345', 'SN001');
    
    expect(result.isValid).toBe(true);
    expect(Object.keys(result.errors).length).toBe(0);
  });

  it('should validate search criteria with empty inputs', () => {
    const result = service.validateSearchCriteria('', '');
    
    expect(result.isValid).toBe(false);
    expect(result.errors['searchCriteria']).toContain('Search Criteria must be entered! Enter Order # or Serial #.');
  });

  it('should validate order search with valid inputs', () => {
    const mockResponse = { isValid: true, errors: {} };
    
    service.validateOrderSearch('COMP01', '12345').subscribe(result => {
      expect(result.isValid).toBe(true);
      expect(Object.keys(result.errors).length).toBe(0);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/order-search`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should validate order search with empty company', () => {
    const mockResponse = { 
      isValid: false, 
      errors: { company: ['Company must be selected'] }
    };
    
    service.validateOrderSearch('', '12345').subscribe(result => {
      expect(result.isValid).toBe(false);
      expect(result.errors['company']).toContain('Company must be selected');
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/order-search`);
    req.flush(mockResponse);
  });

  it('should validate calibration type selection with valid type', () => {
    const mockResponse = { isValid: true, errors: {} };
    
    service.validateCalibrationTypeSelection('ADM').subscribe(result => {
      expect(result.isValid).toBe(true);
      expect(Object.keys(result.errors).length).toBe(0);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/calibration-type`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should validate calibration type selection with invalid type', () => {
    const mockResponse = { 
      isValid: false, 
      errors: { calType: ['Calibration type must be selected'] }
    };
    
    service.validateCalibrationTypeSelection('').subscribe(result => {
      expect(result.isValid).toBe(false);
      expect(result.errors['calType']).toContain('Calibration type must be selected');
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/calibration-type`);
    req.flush(mockResponse);
  });

  it('should validate mode selection with compatible modes', () => {
    const mockResponse = { isValid: true, errors: {} };
    
    service.validateModeSelection('ADM', ['Temperature', 'Humidity'], 'CAL001').subscribe(result => {
      expect(result.isValid).toBe(true);
      expect(Object.keys(result.errors).length).toBe(0);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/mode-selection`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should validate mode selection with incompatible modes (VFP business rule)', () => {
    const mockResponse = { 
      isValid: false, 
      errors: { modes: ['Flow Eqv and Velocity Eqv cannot be selected together'] }
    };
    
    service.validateModeSelection('ADM', ['Flow Eqv', 'Velocity Eqv'], 'CAL001').subscribe(result => {
      expect(result.isValid).toBe(false);
      expect(result.errors['modes']).toContain('Flow Eqv and Velocity Eqv cannot be selected together');
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/mode-selection`);
    req.flush(mockResponse);
  });

  it('should validate edit permissions for active calibrations', () => {
    const mockResponse = { isValid: true, errors: {} };
    
    service.validateEditPermissions('CAL001', 'EDIT').subscribe(result => {
      expect(result.isValid).toBe(true);
      expect(Object.keys(result.errors).length).toBe(0);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/edit-permissions`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should prevent editing completed calibrations (VFP business rule)', () => {
    const mockResponse = { 
      isValid: false, 
      errors: { edit: ['Cannot edit completed calibrations'] }
    };
    
    service.validateEditPermissions('CAL002', 'EDIT').subscribe(result => {
      expect(result.isValid).toBe(false);
      expect(result.errors['edit']).toContain('Cannot edit completed calibrations');
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/validation/edit-permissions`);
    req.flush(mockResponse);
  });

  it('should validate flow velocity conflict (VFP business rule)', () => {
    const result = service.validateFlowVelocityConflict(['Flow Eqv', 'Velocity Eqv']);
    
    expect(result.isValid).toBe(false);
    expect(result.errors['modeSelection']).toContain('Flow Eqv and Velocity Eqv modes cannot be selected simultaneously');
  });

  it('should allow compatible mode combinations', () => {
    const result = service.validateFlowVelocityConflict(['Temperature', 'Humidity']);
    
    expect(result.isValid).toBe(true);
    expect(Object.keys(result.errors).length).toBe(0);
  });
});
