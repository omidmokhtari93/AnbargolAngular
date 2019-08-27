import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-cutted',
  templateUrl: './cutted.component.html',
  styleUrls: ['./cutted.component.less']
})
export class CuttedComponent implements OnInit {
  flowerId: number;
  loading = false;
  data = [];
  httpSubscription: Subscription
  routeSubscription: Subscription
  constructor(private http: Http, private route: ActivatedRoute, private notifier: NotifierService) { }

  ngOnInit() {
    this.loading = true;
    this.routeSubscription = this.route.params.subscribe((p: Params) => {
      this.flowerId = p['fid'];
      this.httpSubscription = this.http.get('/api/Cutted', { params: { flowerId: p['fid'] } }).subscribe(e => {
        this.data = e.json();
        this.loading = false;
      })
    })
  }

  plus(change, id) {
    this.httpSubscription = this.http.get('/api/UpdateCutted',
      {
        params: { change: change, id: id, plus: true }
      }).subscribe(e => {
      let message = e.json();
      this.notifier.notify(message.type, message.message);
    })
  }

  minus(change, id) {
    console.log(id);
    this.httpSubscription = this.http.get('/api/UpdateCutted',
      {
        params: { change: change, id: id, plus: false }
      }).subscribe(e => {
        let message = e.json();
        this.notifier.notify(message.type, message.message);
      })
  }

  editCutted(change, id) {
   
  }

  saveComment(id, comment) {
    this.httpSubscription = this.http.get('/api/UpdateComment',
      {
        params: { id: id, comment: comment }
      }).subscribe(e => {
        this.notifier.notify('success', 'ذخیره شد');
      })
  }
}
