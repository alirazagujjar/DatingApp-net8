import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/Member';
import { of,tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  private http = inject(HttpClient);
  baseUrl = environment.appUrl;
  members = signal<Member[]>([]);
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'user').subscribe({
      next:response=>this.members.set(response)
    });
  }
  getMemberByname(username: string) {
    const member = this.members().find(x=>x.userName===username);
    if(member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + `user/GetUserByName?name=${username}`);
  }
  updateMemberInfo(member: Member) {
    return this.http.put<Member>(this.baseUrl + `user`,member).pipe(
      tap(()=>{
this.members.update(members=>members.map(m=>m.userName===member.userName ? member : m))
      })
    );
  }
}
