import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FlowerInfoComponent } from './main/flower-info/flower-info.component';
import { CuttedComponent } from './main/cutted/cutted.component';


const routes: Routes = [
  { path :'flowerinfo/:fid' , component: FlowerInfoComponent },
  { path :'cutted/:fid' , component: CuttedComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
