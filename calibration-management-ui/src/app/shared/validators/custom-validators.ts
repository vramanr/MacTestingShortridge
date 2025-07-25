import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class CustomValidators {
  static serialNumber(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const serialNo = control.value.toString().trim();
      if (serialNo.length < 3 || serialNo.length > 50) {
        return { serialNumberLength: { min: 3, max: 50, actual: serialNo.length } };
      }
      
      if (!/^[A-Z0-9\-_]+$/i.test(serialNo)) {
        return { serialNumberFormat: { value: serialNo } };
      }
      
      return null;
    };
  }

  static orderNumber(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const orderNo = control.value.toString().trim();
      if (!/^\d{4,10}$/.test(orderNo)) {
        return { orderNumberFormat: { value: orderNo } };
      }
      
      return null;
    };
  }

  static calibrationReading(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value && control.value !== 0) {
        return null;
      }
      
      const reading = control.value.toString();
      if (!/^-?\d+(\.\d{1,4})?$/.test(reading)) {
        return { readingFormat: { value: reading } };
      }
      
      const numericValue = parseFloat(reading);
      if (numericValue < -999999 || numericValue > 999999) {
        return { readingRange: { min: -999999, max: 999999, actual: numericValue } };
      }
      
      return null;
    };
  }

  static toleranceValue(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value && control.value !== 0) {
        return null;
      }
      
      const tolerance = parseFloat(control.value);
      if (isNaN(tolerance)) {
        return { toleranceFormat: { value: control.value } };
      }
      
      if (tolerance < 0 || tolerance > 100) {
        return { toleranceRange: { min: 0, max: 100, actual: tolerance } };
      }
      
      return null;
    };
  }

  static dateRange(startDateField: string, endDateField: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const startDate = control.get(startDateField)?.value;
      const endDate = control.get(endDateField)?.value;
      
      if (!startDate || !endDate) {
        return null;
      }
      
      if (new Date(startDate) > new Date(endDate)) {
        return { dateRange: { startDate, endDate } };
      }
      
      return null;
    };
  }

  static calibrationDueDate(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const dueDate = new Date(control.value);
      const today = new Date();
      const oneYearFromNow = new Date();
      oneYearFromNow.setFullYear(today.getFullYear() + 1);
      
      if (dueDate < today) {
        return { dueDatePast: { dueDate: control.value } };
      }
      
      if (dueDate > oneYearFromNow) {
        return { dueDateTooFar: { dueDate: control.value, maxDate: oneYearFromNow } };
      }
      
      return null;
    };
  }

  static companyCode(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const code = control.value.toString().trim().toUpperCase();
      if (code.length < 2 || code.length > 10) {
        return { companyCodeLength: { min: 2, max: 10, actual: code.length } };
      }
      
      if (!/^[A-Z0-9]+$/.test(code)) {
        return { companyCodeFormat: { value: code } };
      }
      
      return null;
    };
  }

  static technicianId(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const techId = control.value.toString().trim();
      if (techId.length < 3 || techId.length > 20) {
        return { technicianIdLength: { min: 3, max: 20, actual: techId.length } };
      }
      
      if (!/^[A-Z0-9]+$/i.test(techId)) {
        return { technicianIdFormat: { value: techId } };
      }
      
      return null;
    };
  }

  static modelNumber(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const modelNo = control.value.toString().trim();
      if (modelNo.length < 2 || modelNo.length > 50) {
        return { modelNumberLength: { min: 2, max: 50, actual: modelNo.length } };
      }
      
      if (!/^[A-Z0-9\-_\s]+$/i.test(modelNo)) {
        return { modelNumberFormat: { value: modelNo } };
      }
      
      return null;
    };
  }
}
