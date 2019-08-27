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
import { FlowerMainInfoComponent } from './shared/flower-main-info/flower-main-info.component';
import { NotifierModule } from 'angular-notifier';

@
  NgModule({
    declarations: [
      AppComponent,
      MainComponent,
      SearchComponent,
      FlowerInfoComponent,
      CuttedComponent,
      FlowerMainInfoComponent
    ],
    imports: [
      BrowserModule,
      AppRoutingModule,
      FormsModule,
      HttpModule,
      NotifierModule.withConfig({
        behaviour: {
          autoHide: 2000,
          showDismissButton: false
        },
        position: {
          horizontal: {
            position: 'left'
          },
          vertical: {
            position: 'top'
          }
        }
      })
    ],
    providers: [],
    bootstrap: [AppComponent]
  })
export class AppModule { }
