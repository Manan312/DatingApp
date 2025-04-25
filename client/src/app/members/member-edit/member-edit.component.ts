import {
  Component,
  HostListener,
  inject,
  OnInit,
  ViewChild,
  viewChild,
} from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_service/account.service';
import { MembersService } from '../../_service/members.service';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule, FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css',
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$.events']) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
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
      next: (member) => (this.member = member),
      error: (error) => this.toasterService.error(error.error),
    });
  }
  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toasterService.success('Profile Updated');
        this.editForm?.reset(this.member);
      },
      error: errror=> this.toasterService.error(errror.error)
    });
  }
}
