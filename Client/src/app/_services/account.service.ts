import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  //http = Inject(HttpClient);
  constructor(private http: HttpClient) { }
  baseUrl = environment.appUrl;
  currentUser = signal<User | null>(null);
  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    );
  }
  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
  register(user: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', user).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user;
      })
    );
  }
}
