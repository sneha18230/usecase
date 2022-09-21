export class Company{
    name: String;
    ceo: String;
    brief: String;

    constructor(){}
    addCompany(name:String, ceo:String, brief:String){
        this.name=name;
        this.ceo=ceo;
        this.brief=brief;
    }
}