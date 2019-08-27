import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';
import { SearchComponent } from './shared/search/search.component';
import { HttpModule } from '@angular/http';
import { FlowerInfoComponent } from './main/flower-info/flower-info.component';
import { CuttedComponent } from './main/cutted/cutted.component';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    SearchComponent,
    FlowerInfoComponent,
    CuttedComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
