import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { UserService } from '../user.service';
import { CompanyService } from '../company.service';
import { normalizeGenFileSuffix } from '@angular/compiler/src/aot/util';
@Component({
  selector: 'app-compare-charts',
  templateUrl: './compare-charts.component.html',
  styleUrls: ['./compare-charts.component.scss']
})
export class CompareChartsComponent {
  message = null;
  dataSize=0;
  stock_prices=[];
  time=[];
  constructor(private userService:UserService, private companyService:CompanyService) {
    this.message = this.userService.user.message;
    this.dataSize = 0;
  }
  ngOnInit(): void {
    for (let i=1;i<this.companyService.company.length;i++) {
      if(this.companyService.company[i].length!=0){
        this.dataSize++;
        this.stock_prices.push(this.companyService.company[i][2]);
        this.time.push(this.companyService.company[i][4]);
      }
    }
  }
  // barChartOptions: ChartOptions = {
  //   responsive: true,
  // };
  // barChartLabels: Label[] = ['Apple', 'Banana', 'Kiwifruit', 'Blueberry', 'Orange', 'Grapes'];
  // barChartType: ChartType = 'bar';
  // barChartLegend = true;
  // barChartPlugins = [];

  // barChartData: ChartDataSets[] = [
  //   { data: [45, 37, 60, 70, 46, 33], label: 'Best Fruits' }
  // ];

  // lineChartData: ChartDataSets[] = [
  //   { data: [85, 72, 78, 75, 77, 75], label: 'Crude oil prices' },
  // ];

  // lineChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June'];
  
  lineChartData: ChartDataSets[] = [
    { data: this.stock_prices, label:'Price per Share(in Rs.) vs Time'}
  ]
  lineChartLabels: Label[] = this.time;

  lineChartOptions = {
    responsive: true,
  };

  lineChartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,255,0,0.28)',
    },
  ];
  

  lineChartLegend = true;
  lineChartPlugins = [];
  lineChartType = 'line';

}
