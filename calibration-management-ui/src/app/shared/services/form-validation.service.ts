import { Injectable } from '@angular/core';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface ValidationResult {
  isValid: boolean;
  errors: { [key: string]: string[] };
}

@Injectable({
  providedIn: 'root'
})
export class FormValidationService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  /**
   * Port of VFP search criteria validation from cal_search.SCT lines 180-183
   */
  validateSearchCriteria(orderNo: string, serialNo: string): ValidationResult {
    const result: ValidationResult = { isValid: true, errors: {} };

    if (!orderNo?.trim() && !serialNo?.trim()) {
      result.isValid = false;
      result.errors['searchCriteria'] = ['Search Criteria must be entered! Enter Order # or Serial #.'];
      return result;
    }

    if (orderNo?.trim()) {
      if (orderNo.trim().length < 4 || orderNo.trim().length > 10) {
        result.isValid = false;
        result.errors['orderNo'] = ['Order number must be 4-10 characters'];
      }
    }

    if (serialNo?.trim()) {
      if (serialNo.trim().length < 3 || serialNo.trim().length > 50) {
        result.isValid = false;
        result.errors['serialNo'] = ['Serial number must be 3-50 characters'];
      }
    }

    return result;
  }

  /**
   * Port of VFP order search validation from order_search.SCT
   */
  validateOrderSearch(companyId: string, orderNo: string): Observable<ValidationResult> {
    const params = { companyId: companyId || '', orderNo: orderNo || '' };
    
    return this.http.post<ValidationResult>(`${this.apiUrl}/validation/order-search`, params)
      .pipe(
        catchError(error => {
          console.error('Order search validation error:', error);
          return of({
            isValid: false,
            errors: { general: ['Validation error occurred'] }
          });
        })
      );
  }

  /**
   * Port of VFP Flow Eqv vs Velocity Eqv validation from cal_form.SCT lines 162-167, 226-231
   */
  validateFlowVelocityConflict(selectedModes: string[]): ValidationResult {
    const result: ValidationResult = { isValid: true, errors: {} };

    const hasFlowEqv = selectedModes.includes('Flow Eqv');
    const hasVelocityEqv = selectedModes.includes('Velocity Eqv');

    if (hasFlowEqv && hasVelocityEqv) {
      result.isValid = false;
      result.errors['modeSelection'] = ['Flow Eqv and Velocity Eqv modes cannot be selected simultaneously'];
    }

    return result;
  }

  /**
   * Port of VFP calibration type selection validation from order_search.SCT lines 359-361
   */
  validateCalibrationTypeSelection(calType: string): Observable<ValidationResult> {
    const params = { calType };
    
    return this.http.post<ValidationResult>(`${this.apiUrl}/validation/calibration-type`, params)
      .pipe(
        catchError(error => {
          console.error('Calibration type validation error:', error);
          return of({
            isValid: false,
            errors: { general: ['Calibration type validation error occurred'] }
          });
        })
      );
  }

  /**
   * Validate mode selection compatibility
   */
  validateModeSelection(calType: string, selectedModes: string[], calNo?: string): Observable<ValidationResult> {
    const params = { calType, selectedModes, calNo: calNo || '' };
    
    return this.http.post<ValidationResult>(`${this.apiUrl}/validation/mode-selection`, params)
      .pipe(
        catchError(error => {
          console.error('Mode selection validation error:', error);
          return of({
            isValid: false,
            errors: { general: ['Mode validation error occurred'] }
          });
        })
      );
  }

  /**
   * Validate edit permissions
   */
  validateEditPermissions(calNo: string, editType: string): Observable<ValidationResult> {
    const params = { calNo, editType };
    
    return this.http.post<ValidationResult>(`${this.apiUrl}/validation/edit-permissions`, params)
      .pipe(
        catchError(error => {
          console.error('Edit permissions validation error:', error);
          return of({
            isValid: false,
            errors: { general: ['Edit permission validation error occurred'] }
          });
        })
      );
  }

  /**
   * Custom validator for serial number format
   */
  static serialNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const serialNo = control.value.toString().trim();
      
      if (serialNo.length < 3 || serialNo.length > 50) {
        return { serialNumber: { message: 'Serial number must be between 3 and 50 characters' } };
      }

      if (!/^[A-Z0-9\-_]+$/i.test(serialNo)) {
        return { serialNumber: { message: 'Serial number can only contain letters, numbers, hyphens, and underscores' } };
      }

      return null;
    };
  }

  /**
   * Custom validator for order number format
   */
  static orderNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const orderNo = control.value.toString().trim();
      
      if (!/^\d{4,10}$/.test(orderNo)) {
        return { orderNumber: { message: 'Order number must be 4-10 digits' } };
      }

      return null;
    };
  }

  /**
   * Custom validator for calibration reading
   */
  static calibrationReadingValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value && control.value !== 0) {
        return null;
      }

      const reading = parseFloat(control.value);
      
      if (isNaN(reading)) {
        return { calibrationReading: { message: 'Invalid reading format' } };
      }

      if (reading < -999999 || reading > 999999) {
        return { calibrationReading: { message: 'Reading must be between -999999 and 999999' } };
      }

      return null;
    };
  }

  /**
   * Custom validator for tolerance value
   */
  static toleranceValueValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value && control.value !== 0) {
        return null;
      }

      const tolerance = parseFloat(control.value);
      
      if (isNaN(tolerance)) {
        return { toleranceValue: { message: 'Tolerance must be a valid number' } };
      }

      if (tolerance < 0 || tolerance > 100) {
        return { toleranceValue: { message: 'Tolerance must be between 0% and 100%' } };
      }

      return null;
    };
  }

  /**
   * Custom validator for company code
   */
  static companyCodeValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const code = control.value.toString().trim().toUpperCase();
      
      if (code.length < 2 || code.length > 10) {
        return { companyCode: { message: 'Company code must be between 2 and 10 characters' } };
      }

      if (!/^[A-Z0-9]+$/.test(code)) {
        return { companyCode: { message: 'Company code can only contain letters and numbers' } };
      }

      return null;
    };
  }

  /**
   * Custom validator for model number
   */
  static modelNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const modelNo = control.value.toString().trim();
      
      if (modelNo.length < 2 || modelNo.length > 50) {
        return { modelNumber: { message: 'Model number must be between 2 and 50 characters' } };
      }

      if (!/^[A-Z0-9\-_\s]+$/i.test(modelNo)) {
        return { modelNumber: { message: 'Model number contains invalid characters' } };
      }

      return null;
    };
  }

  /**
   * Custom validator for calibration due date
   */
  static calibrationDueDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const dueDate = new Date(control.value);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      
      const oneYearFromNow = new Date();
      oneYearFromNow.setFullYear(oneYearFromNow.getFullYear() + 1);

      if (dueDate < today) {
        return { calibrationDueDate: { message: 'Due date cannot be in the past' } };
      }

      if (dueDate > oneYearFromNow) {
        return { calibrationDueDate: { message: 'Due date cannot be more than one year from now' } };
      }

      return null;
    };
  }
}
