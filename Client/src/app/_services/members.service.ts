import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/Member';
import { of,tap } from 'rxjs';
import { Photo } from '../_models/Photo';

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
  setMainPhoto(photo:Photo){
    return this.http.put(this.baseUrl + 'user/set-main-photo/'+photo.id,{}).pipe(
      tap(()=>{
        this.members.update(member=>member.map(m=>{
          if(m.photos.includes(photo)){
            m.photoUrl = photo.url;
          }
          return m;
        }))
      })
    )

  }
  deletePhoto(photo:Photo){
    return this.http.delete(this.baseUrl + 'user/delete-photo/'+photo.id).pipe(
      tap(()=>{
        this.members.update(m=>m.map(
          x=>{
            if(x.photos.includes(photo)){
              x.photos = x.photos.filter(a=>a.id !== photo.id);
            }
            return x;
          }
        ))
      })
    );
  }
}
