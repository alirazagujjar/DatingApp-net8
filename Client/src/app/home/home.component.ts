import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  title = 'Dating App';
  ngOnInit(): void {
    this.getusers();
  }
  registerMode=false;
  users:any;
  http = inject(HttpClient);
  private accountService =inject(AccountService);
  registerToggle(){
    this.registerMode=!this.registerMode;
  }
  getusers(){
    this.http.get("https://localhost:5001/api/User").subscribe({
      next:response =>this.users=response,
      error :error=> console.error(error),
      complete: ()=>console.log("Request has complted")
      
    });
    
  }
  cancelRegisterMode(event: boolean) {
    this.registerMode=event;
    }
}
