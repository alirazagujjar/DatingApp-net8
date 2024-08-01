import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/Member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  private http = inject(HttpClient);
  baseUrl = environment.appUrl;
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'user');
  }
  getMemberByname(username: string) {
    console.log(this.baseUrl + 'user/GetUserByName/' + username);
    return this.http.get<Member>(this.baseUrl + `user/GetUserByName?name=${username}`);
  }
  
}
