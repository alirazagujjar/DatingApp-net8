import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable,  model,  signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/Member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/Photo';
import { PaginationResult } from '../_models/Pagination';
import { UserParams } from '../_models/UserParams';
import { AccountService } from './account.service';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.appUrl;
  memberCache = new Map();
  paginationResult = signal<PaginationResult<Member[]> | null>(null);
  user = this.accountService.currentUser();
  getMembers(userParams: UserParams) {

    const response = this.memberCache.get(Object.values(userParams).join('-'));
    console.log(response)
    if(response) return this.getPaginatedResponse(response);

    let params = this.setPaginationHeader(userParams.pageNumber, userParams.pageSize);
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
    return this.http.get<Member[]>(this.baseUrl + 'user', { observe: 'response', params }).subscribe({
      next: response => {
        this.getPaginatedResponse(response);
        this.memberCache.set(Object.values(userParams).join('-'),response);
      }
    });
  }
  private setPaginationHeader(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    if (pageNumber && pageSize) {
      params = params.append("pageNumber", pageNumber);
      params = params.append("pageSize", pageSize);
    }
    return params;
  }

  private getPaginatedResponse(response:HttpResponse<Member[]>){
    this.paginationResult.set({
      items: response.body as Member[],
      pagination: JSON.parse(response.headers.get('Pagination')!)
    })
  }

  getMemberByname(username: string) {
    const member:Member = [...this.memberCache.values()]
    .reduce((arr,elem) => arr.concat(elem.body),[])
    .find((m:Member)=>m.userName === username);
    if(member) return of(member);
    let response = this.http.get<Member>(this.baseUrl + `user/GetUserByName?name=${username}`);
    return response;
  }
  updateMemberInfo(member: Member) {
    return this.http.put<Member>(this.baseUrl + `user`, member).pipe(
      //       tap(()=>{
      // this.members.update(members=>members.map(m=>m.userName===member.userName ? member : m))
      //       })
    );
  }
  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'user/set-main-photo/' + photo.id, {}).pipe(
      // tap(()=>{
      //   this.members.update(member=>member.map(m=>{
      //     if(m.photos.includes(photo)){
      //       m.photoUrl = photo.url;
      //     }
      //     return m;
      //   }))
      // })
    )

  }
  deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + 'user/delete-photo/' + photo.id).pipe(
      // tap(()=>{
      //   this.members.update(m=>m.map(
      //     x=>{
      //       if(x.photos.includes(photo)){
      //         x.photos = x.photos.filter(a=>a.id !== photo.id);
      //       }
      //       return x;
      //     }
      //   ))
      // })
    );
  }
}
