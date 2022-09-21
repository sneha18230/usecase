import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompareChartsComponent } from './components/compare-charts/compare-charts.component';
import { NewCompanyComponent } from './components/new-company/new-company.component';
import { UploadExcelComponent } from './components/upload-excel/upload-excel.component';
import { ManageCompaniesComponent } from './components/manage-companies/manage-companies.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { LoginGuard } from './components/guards/login.guard';
import { LogGuard } from './components/guards/log.guard';
import { SignupGuard } from './components/guards/signup.guard';

const routes: Routes = [
  {path: 'home', component: CompareChartsComponent},
  {path: 'new-company', component: NewCompanyComponent, canActivate: [LogGuard]},
  {path: 'upload-excel', component: UploadExcelComponent, canActivate: [LoginGuard]},
  {path: 'manage-companies', component:ManageCompaniesComponent, canActivate: [LoginGuard]},
  {path: 'login', component:LoginComponent, canActivate:[SignupGuard]},
  {path: 'signup', component:SignupComponent, canActivate:[SignupGuard]},
  {path: "**", pathMatch: 'full', redirectTo: 'home'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
