import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-manage-companies',
  templateUrl: './manage-companies.component.html',
  styleUrls: ['./manage-companies.component.scss']
})
export class ManageCompaniesComponent implements OnInit {
  cmps = [];
  message = null;
  fetch;
  constructor(private http: HttpClient) {
    this.fetch = this.http.get("http://localhost:8082/company") // can be the link for our github repo with data
   .subscribe((res:any)=>{
     console.log(res);

    //  if(res.status=="success"){
       this.cmps = res;
       this.message = null;
    //  } else {
    //    this.message = "Data not found";
    //  }
    })
  }
  ngOnDestroy(){
    this.fetch.unsubscribe();
   }
  ngOnInit(): void {
  }

}
