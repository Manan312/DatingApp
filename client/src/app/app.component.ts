import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgFor } from '@angular/common';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_service/account.service';
import { HomeComponent } from "./home/home.component";
import { NgxSpinnerComponent } from 'ngx-spinner';


@Component({
  selector: 'app-root',
  standalone:true,  
  imports: [ NavComponent,RouterOutlet,NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  // http=inject(HttpClient); 
  private accountservice=inject(AccountService);
  // users:any;
  ngOnInit(): void {
      // this.getUsers();
      this.setCurrentUser();
  }
  setCurrentUser()
  {
    const userString=localStorage.getItem('user');
    if(!userString) return;
    const user=JSON.parse(userString);
    this.accountservice.currentUser.set(user);
  }
  // getUsers(){
  //   this.http.get("https://localhost:5001/api/User/").subscribe(
  //     {
  //       next: response=>this.users=response,
  //       error:error=>console.log(error),
  //       complete:()=>console.log("Yay!")
  //     }
  //   )
  // }

}
