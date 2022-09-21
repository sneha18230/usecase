import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../user.service';

@Injectable({
    providedIn: 'root'
})
export class SignupGuard implements CanActivate{

    constructor(private userService:UserService, private router:Router){}
    canActivate():boolean{

        if(this.userService.user.isValid==true){
            this.userService.user.message = 'Already logged in';
            this.router.navigate(['', 'home', this.userService.user.name])
        } else{
            console.log("Not logged in");
        }

        return !this.userService.user.isValid;
    }

}