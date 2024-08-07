import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/Member';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [TabsModule,FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?:NgForm;
  @HostListener('window:beforeunload',['$event']) consumestate($event:any){
    if(this.editForm?.dirty){
      $event.returnValue = true;
    }
  }
  member?: Member;
  private accountServ = inject(AccountService);
  private memberServ = inject(MembersService);
  private toastr = inject(ToastrService);
  ngOnInit(): void {
    this.loadMember();
  }
  loadMember() {
    const user = this.accountServ.currentUser()?.userName;
    if (!user) return;
    this.memberServ.getMemberByname(user).subscribe({
      next: res =>{
        this.member = res;
        console.log(res);
      } ,
      error: e => console.log(e.messages)
    });
  }
  updateMember() {
    if(this.editForm?.value)
    {
      const updatecall = this.memberServ.updateMemberInfo(this.editForm?.value).subscribe({
        next: res=>{
          this.toastr.success("Congrats: Youe profile has updated");
          this.editForm?.reset(this.member);
        }

      });
    }
   else{
    this.toastr.error("Something bad happened during updation");
   }
    
    
  }
}
