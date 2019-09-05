import { Component, OnInit } from '@angular/core';
import { Observable, Observer } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.less']
})
export class ItemsComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit() {
    const api = this.http.get('/api/Login');
    console.log(api)
  }

}
