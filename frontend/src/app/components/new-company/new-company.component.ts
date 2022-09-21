import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Company} from './Company';
import {NgForm} from '@angular/forms';

const headers = {'Content-Type': 'application/json'}
@Component({
  selector: 'app-new-company',
  templateUrl: './new-company.component.html',
  styleUrls: ['./new-company.component.scss']
})
export class NewCompanyComponent implements OnInit {

  constructor(private http:HttpClient) { }

  company = new Company();

  ngOnInit(): void {
  }
  onSubmit(form:NgForm){
    this.company.addCompany(form.value.name, form.value.ceo, form.value.brief);
    this.http.post<Boolean>('http://localhost:8082/company/add', this.company, {headers}).subscribe(data=>{
      if(data){
        console.log("Company created succesfuly");
      }
      else{
        console.log("company already exists");
      }
    })
  }
}
