import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-cutted',
  templateUrl: './cutted.component.html',
  styleUrls: ['./cutted.component.less']
})
export class CuttedComponent implements OnInit {
  loading = false;
  data = [];
  httpSubscription: Subscription
  routeSubscription: Subscription
  constructor(private http: Http, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loading = true;
    this.routeSubscription = this.route.params.subscribe((p: Params) => {
      this.httpSubscription = this.http.get('/api/Cutted', { params: { flowerId: p['fid'] } }).subscribe(e => {
        this.data = e.json();
        this.loading = false;
      })
    })
  }

}
