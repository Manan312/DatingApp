import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_service/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';

@Component({
  selector: 'app-member-detail',
  imports: [],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit{
  private memberService=inject(MembersService);
  private route=inject(ActivatedRoute);

  member?:Member;
   
  ngOnInit(): void {
      this.loadMember()
  }

  loadMember()
  {
    const userName=this.route.snapshot.paramMap.get('username');

    if(!userName) return;

    this.memberService.getMember(userName).subscribe({
      next: member=> this.member=member,
      error:error=> console.log(error)  
    })
  }
}
