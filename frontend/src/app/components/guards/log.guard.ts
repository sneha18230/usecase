import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../user.service';

@Injectable({
    providedIn: 'root'
})
export class LogGuard implements CanActivate{

    constructor(private userService:UserService, private router:Router){}
    canActivate():boolean{

        if(this.userService.user.isValid==true){
            if(this.userService.user.userType=='admin')
                return true;
            else{
                this.userService.user.message = 'Login using admin credentials';
                this.router.navigate(['', 'home'])
                return false;
            }
        } else{
            console.log("Not logged in");
            this.router.navigate(['', 'login'])
        }

    }

}