import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../user.service';

@Injectable({
    providedIn: 'root'
})
export class LoginGuard implements CanActivate{

    constructor(private userService:UserService, private router:Router){}
    canActivate():boolean{

        if(this.userService.user.isValid==true){
            this.userService.user.message = 'Already logged in';
            return this.userService.user.isValid;
        } else{
            console.log("Not logged in");
            this.router.navigate(['', 'login'])
        }

    }

}