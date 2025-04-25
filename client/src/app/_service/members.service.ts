import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http=inject(HttpClient);
  baseUrl=environment.apiUrl;
  //to store the member for state management
  members=signal<Member[]>([]);

  getMembers(){
    return this.http.get<Member[]>(this.baseUrl+'user').subscribe({
      next:members=>this.members.set(members)
    })
  }

  getMember(username:string)
  {
    const member=this.members().find(x=>x.userName==username);
    if(member!=undefined)
    {
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl+'user/'+username);
  }

  updateMember(member:Member){
    //return this.http.put(this.baseUrl+"user/UpdateMemberData",member);
    return this.http.put(this.baseUrl+"user/UpdateMemberData",member).pipe(
      tap(()=>{
        this.members.update(members=>members.map(x=>
          x.userName==member.userName ? member:x))
      })
    );
  }
// getMember(username:string)
//   {
//     return this.http.get<Member>(this.baseUrl+'user/'+username,this.getHttpOptions());
//   }

  // getHttpOptions()
  // {
  //   return {
  //     headers:new HttpHeaders({
  //       Authorization:`Bearer ${this.accountservice.currentUser()?.token}` 
  //     })
  //   }
  // }
}
