import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CompanyService } from '../company.service';
import * as XLSX from 'xlsx';
type AOA = any[][];
// import { FormBuilder } from '@angular/forms';
// import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-upload-excel',
  templateUrl: './upload-excel.component.html',
  styleUrls: ['./upload-excel.component.scss']
})
export class UploadExcelComponent implements OnInit {

  data: AOA = [];
  emps = [];
  message = null;
  fetch;
 public constructor(private http:HttpClient, private companyService:CompanyService){
  // const target: string = <string>(evt.target);
  this.message = null;
  this.fetch = this.http.get("http://dummy.restapiexample.com/api/v1/employees") // can be the link for our github repo with data
   .subscribe((res:any)=>{
     console.log(res);

     if(res.status=="success"){
       this.emps = res.data;
       this.message = 1;
     } else {
       this.message = 0;
     }
    })
 }
  
    onFileChange(evt: any) {
      /* wire up file reader */
      const target: DataTransfer = <DataTransfer>(evt.target);
      if (target.files.length !== 1) throw new Error('Cannot use multiple files');
      const reader: FileReader = new FileReader();
      reader.onload = (e: any) => {
        /* read workbook */
        const bstr: string = e.target.result;
        const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });
  
        /* grab first sheet */
        const wsname: string = wb.SheetNames[0];
        const ws: XLSX.WorkSheet = wb.Sheets[wsname];
  
        /* save data */
        this.data = <AOA>(XLSX.utils.sheet_to_json(ws, { header: 1 }));
        
        if(this.data[0].length==5){
          
          if(this.data[0][0].trim()=='Company Code' && this.data[0][1].trim()=='Stock Exchange' && this.data[0][2].trim()=='Price Per Share(in Rs)'
          && this.data[0][3].trim()=='Date' && this.data[0][4].trim()=='Time'){
            console.log('data in proper format');
            this.companyService.company = this.data;
            // for(let i of this.companyService.company){
            //   console.log(i.length);
            // }
            console.log(this.companyService.company.length);
          }
          else console.log('data not in proper format');
        }
        else{
          console.log('data not in proper format');
        }
      };
      reader.readAsBinaryString(target.files[0]);
    }
 ngOnDestroy(){
  this.fetch.unsubscribe();
 }
  ngOnInit(): void {
  }

}
