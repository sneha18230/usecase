export class User {
    username: String;
    password: String;
    user_type: String;

    constructor(){}
    addUser(username: String, password:String){
        this.username=username;
        this.password=password;
        if(username=='admin'){
            this.user_type='admin';
        }
        else{
            this.user_type='user';
        }
    }
}