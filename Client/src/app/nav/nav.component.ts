import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule,RouterLink,RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  private routes= inject(Router);
  private toastr = inject(ToastrService);
  isLogin :boolean=false;
  maodel: any = {};
  login() {
    this.accountService.login(this.maodel).subscribe({
      next: _ => {
       this.isLogin=true;
        this.routes.navigateByUrl('/members');
      },
      error: (error: any) => {
        console.log(error)
        this.toastr.error(error.error.messgae);
      }
      

    });
  }
  logout() {
    this.accountService.logout();
    this.isLogin=false;
    this.routes.navigateByUrl('/');
  }
}
