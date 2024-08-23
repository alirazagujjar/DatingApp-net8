import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardsComponent } from "../member-cards/member-cards.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { UserParams } from '../../_models/UserParams';
import { AccountService } from '../../_services/account.service';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardsComponent, PaginationModule,FormsModule,ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  memberserivice = inject(MembersService);
  private accountserivice = inject(AccountService);
  userParams = new UserParams(this.accountserivice.currentUser());
  genderList = [{value:'male', display:'Males'},{value:'female', display:'Females'}]
  ngOnInit(): void {
    if (!this.memberserivice.paginationResult()) {
      this.loadMembers();
    }

  }
  loadMembers() {
    this.memberserivice.getMembers(this.userParams);
  }
  pageChanged(event: any) {
    if (this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }

  }
  resetFilters(){
    this.userParams = new UserParams(this.accountserivice.currentUser());
    this.loadMembers();
  }
}
