import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';
import { SearchComponent } from './shared/search/search.component';
import { HttpModule } from '@angular/http';
import { FlowerInfoComponent } from './main/flower-info/flower-info.component';
import { CuttedComponent } from './main/cutted/cutted.component';
import { FlowerMainInfoComponent } from './shared/flower-main-info/flower-main-info.component';
import { NotifierModule } from 'angular-notifier';
import { NewFlowerComponent } from './main/new-flower/new-flower.component';
import { HttpClientModule } from '@angular/common/http'
import { HtttpService } from './shared/httpService.service';
import { NgxSmartModalModule } from 'ngx-smart-modal';
import { ArrangesComponent } from './main/arranges/arranges.component';
import { ItemsComponent } from './main/items/items.component';
import { DpDatePickerModule } from 'ng2-jalali-date-picker'

@
  NgModule({
    declarations: [
      AppComponent,
      MainComponent,
      SearchComponent,
      FlowerInfoComponent,
      CuttedComponent,
      FlowerMainInfoComponent,
      NewFlowerComponent,
      ArrangesComponent,
      ItemsComponent
    ],
    imports: [
      ReactiveFormsModule,
      BrowserModule,
      AppRoutingModule,
      FormsModule,
      HttpModule,
      HttpClientModule,
      DpDatePickerModule,
      NgxSmartModalModule.forRoot(),
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
    providers: [HtttpService],
    bootstrap: [AppComponent]
  })
export class AppModule { }
