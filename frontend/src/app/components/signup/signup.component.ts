import { Component, OnInit } from '@angular/core';
import {NgForm} from '@angular/forms';
import { User } from '../login/User';
import { CanActivate, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

const headers = {'Content-Type': 'application/json'}

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

  constructor(private http:HttpClient, private router:Router) { }
  user= new User();
  ngOnInit(): void {
  }
  onSubmit(form: NgForm){
    
    this.user.addUser(form.value.username, form.value.password);
    this.http.post<Boolean>('http://localhost:8085/users/register', this.user ,{ headers }).subscribe(data=>{
      if(data){
        console.log("User created");
      }
      else{
        console.log("User already exists");
      }
    }
    )
    this.router.navigate(['', 'login'])
  }
}
