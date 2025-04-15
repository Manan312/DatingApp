import { Component, inject, OnInit } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_service/account.service';
import { MembersService } from '../../_service/members.service';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule,FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css',
})
export class MemberEditComponent implements OnInit {
  accountService = inject(AccountService);
  memberService = inject(MembersService);
  toasterService = inject(ToastrService);
  member?: Member;

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const user = this.accountService.currentUser();

    if (!user) {
      this.toasterService.error('User not found');
      return;
    }

    this.memberService.getMember(user.userName).subscribe({
      next:
      member=>this.member=member,
      error: error=>this.toasterService.error(error.error)
    });
    
  }
}
