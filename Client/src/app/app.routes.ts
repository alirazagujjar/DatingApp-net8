import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guards/auth.guard';

export const routes: Routes = [
    {path:'',component:HomeComponent},
    {
        path:'',
        runGuardsAndResolvers:'always',
        canActivate:[authGuard],
        children:[
            {path:'members',component:MemberListComponent , canActivate:[authGuard]},
            {path:'members/:id',component:MemberDetailsComponent,canActivate:[authGuard]},
            {path:'lists',component:ListsComponent,canActivate:[authGuard]},
            {path:'messages',component:MessagesComponent,canActivate:[authGuard]}
        ]

    },
    
    {path:'**',component:HomeComponent,pathMatch:'full'}
];
