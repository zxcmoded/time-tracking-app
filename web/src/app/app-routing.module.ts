import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { FormComponent } from './timelog/form/form.component';
import { ListComponent } from './timelog/list/list.component';
import { pageGuard } from './guards/page.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/auth/login',
    pathMatch: 'full',
  },
  {
    path: 'auth',
    canActivate: [pageGuard],
    children: [
      {
        path: 'login',
        component: AuthComponent,
      },
    ],
  },
  {
    path: 'timelog',
    canActivate: [pageGuard],
    children: [
      {
        path: 'form',
        component: FormComponent,
      },
      {
        path: 'list',
        component: ListComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
