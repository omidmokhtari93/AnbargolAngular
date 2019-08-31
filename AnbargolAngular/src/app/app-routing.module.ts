import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FlowerInfoComponent } from './main/flower-info/flower-info.component';
import { CuttedComponent } from './main/cutted/cutted.component';
import { NewFlowerComponent } from './main/new-flower/new-flower.component';


const routes: Routes = [
  { path :'flowerinfo/:fid' , component: FlowerInfoComponent },
  { path :'cutted/:fid' , component: CuttedComponent },
  { path: 'newflower', component: NewFlowerComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
