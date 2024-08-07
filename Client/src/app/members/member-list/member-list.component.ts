import { Component, inject, OnInit } from '@angular/core';
import { Member } from '../../_models/Member';
import { MembersService } from '../../_services/members.service';
import { MemberCardsComponent } from "../member-cards/member-cards.component";

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardsComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  memberserivice = inject(MembersService);

  ngOnInit(): void {
    if (this.memberserivice.members().length === 0) {
      this.loadMembers();
    }

  }
  loadMembers() {
    this.memberserivice.getMembers();
  }
}
