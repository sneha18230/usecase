import { Component, OnInit } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import {NgForm} from '@angular/forms';
import { Router } from '@angular/router';
import { User } from './User';
import { UserService } from '../user.service';
// const httpOptions = {
//   headers: new HttpHeaders({
//     'Content-Type': 'application/json'
//   })
// };

const headers = {'Content-Type': 'application/json'}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private http:HttpClient, private userService:UserService,private router:Router) {}
  
  user= new User();
  login:boolean;
  ngOnInit(): void {
  }
  onSubmit(form: NgForm){
    // console.log('form value: ', form.value);
    this.user.addUser(form.value.username, form.value.password);
    this.http.post<boolean>('http://localhost:8085/users/login', this.user ,{ headers }).subscribe(data=>{
      this.login=data;
      this.userService.user.userType = this.user.user_type;
      this.getLogin();
    }
    )
  }
  getLogin(){
    console.log('trying to get login');
    if(this.login){
      this.userService.user.isValid = true;
      this.router.navigate(['','home'])
    }
  }
}
