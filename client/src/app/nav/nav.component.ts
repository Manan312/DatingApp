import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-nav',
  imports: [FormsModule,BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model: any={};
  //private
  accountservice= inject(AccountService);

  //loggedIn=false;

  login()
  {
    this.accountservice.login(this.model).subscribe({
      next:response=>{
        console.log(response);
        //this.loggedIn=true;
      },
      error:error=> console.log(error)
    })
  }
  loggedout(){
    this.accountservice.logout();
  }
}
