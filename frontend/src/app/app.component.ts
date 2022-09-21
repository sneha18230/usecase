import { Component } from '@angular/core';
import { UserService } from './components/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})


export class AppComponent {
  title = 'ng-app';

  constructor(private userService:UserService, private router:Router){}
  getLogout(){
    console.log("getting logout")
    this.userService.user.isValid = false;
  }
}
