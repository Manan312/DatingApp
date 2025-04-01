import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_service/account.service';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const accountservice=inject(AccountService);
  const toastr=inject(ToastrService);

  if(accountservice.currentUser())
    return true;
  else{
    toastr.error("You shall not pass");
    return false;
  }
  return true;
};
