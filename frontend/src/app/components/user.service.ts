import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public user: any = {
    name: 'Angular',
    email: 'angular@gmail.com',
    isValid: false,
    userType: 'unknown',
    message: ''
  }
  constructor() { }
}
