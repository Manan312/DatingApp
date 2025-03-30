import { Component, EventEmitter, inject, input, Input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountservice=inject(AccountService);
  //@Input() userFromHome:any={};
  //userFromHome=input.required<any>();

  //@Output() cancelRegister=new EventEmitter();
  cancelRegister=output<boolean>();
   model:any={};

   register()
   {
    this.accountservice.register(this.model).subscribe({
      next:response=>{
        console.log(response);
        this.cancel();
      },
      error: error=> console.log(error)
    })
   }

   cancel()
   {
    this.cancelRegister.emit(false);
   }
}
