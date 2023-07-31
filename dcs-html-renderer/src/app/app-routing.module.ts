//BEGIN LICENSE BLOCK 
//Interneuron Synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//END LICENSE BLOCK 
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DcsHtmlRendererComponent } from '../../projects/dcs-html-renderer/src/public-api';

//const routes: Routes = [
//  {
//    path: '',
//    children: [],

//  },
//  {
//    path: 'protected',
//    component: AppComponent,
//    canActivate: [AuthGuardService]
//  },

//  {
//    path: 'oidc-callback',
//    component: OidcCallbackComponent
//  }

//];

const routes: Routes = [
  {
    path: 'oidc-callback',
    component: DcsHtmlRendererComponent
  }
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
