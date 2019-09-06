import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Http } from '@angular/http';
import { setInterval } from 'timers';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.less']
})
export class ItemsComponent implements OnInit {

  constructor(private http: HttpClient, private oldHttp: Http) { }

  ngOnInit() {
    const observ = new Observable((observer) => {
      setTimeout(() => {
        observer.next("next next next");
        observer.next("next next");
        observer.next("next");
        observer.complete();  
      }, 2000);
      
    })

    observ.subscribe(e =>{
      console.log(e)
    });
  }

}
