import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';
import { HomeComponent } from "./home/home.component";

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, NavComponent, HomeComponent]
})
export class AppComponent implements OnInit{
  
  title = 'Client';
  http = inject(HttpClient);
  private accountService =inject(AccountService);
  users:any;
  ngOnInit(): void {
   this.getuser();
   this.setCurrentUser();
  }
  setCurrentUser(){
    const userstring= localStorage.getItem('user');
    if(!userstring) return; 
    const user = JSON.parse(userstring);
    this.accountService.currentUser.set(user);
  }
  getuser(){
    this.accountService.login(this.users);
  }
}
