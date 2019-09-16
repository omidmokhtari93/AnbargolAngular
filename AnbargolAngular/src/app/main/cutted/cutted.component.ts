import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { LoadingBarService } from '@ngx-loading-bar/core';

@Component({
  selector: 'app-cutted',
  templateUrl: './cutted.component.html',
  styleUrls: ['./cutted.component.less']
})
export class CuttedComponent implements OnInit {
  flowerId: number;
  data = [];
  httpSubscription: Subscription
  routeSubscription: Subscription
  constructor(private http: Http,
    private route: ActivatedRoute,
    private notifier: NotifierService,
    private loading: LoadingBarService) { }

  ngOnInit() {
    this.loading.start();
    this.getCuttedData(true);
  }

  getCuttedData(update: boolean) {
    this.routeSubscription = this.route.params.subscribe((p: Params) => {
      this.flowerId = p['fid'];
      this.httpSubscription = this.http.get('/api/Cutted', { params: { flowerId: p['fid'], update: update } }).subscribe(e => {
        this.data = e.json();
        this.loading.complete();;
      })
    })
  }

  plus(change, id) {
    this.loading.start();
    this.httpSubscription = this.http.get('/api/UpdateCutted',
      {
        params: { change: change, id: id, plus: true }
      }).subscribe(e => {
        let message = e.json();
        this.notifier.notify(message.type, message.message);
        this.getCuttedData(false);
    })
  }

  minus(change, id) {
    this.loading.start();
    this.httpSubscription = this.http.get('/api/UpdateCutted',
      {
        params: { change: change, id: id, plus: false }
      }).subscribe(e => {
        let message = e.json();
        if (message.type == 'error') {
          this.notifier.notify(message.type, message.message);
        } else {
          this.getCuttedData(false);
          this.notifier.notify(message.type, message.message);
        }
      })
  }

  saveComment(id, comment) {
    this.loading.start();
    this.httpSubscription = this.http.get('/api/UpdateComment',
      {
        params: { id: id, comment: comment }
      }).subscribe(e => {
        this.notifier.notify('success', 'ذخیره شد');
        this.getCuttedData(false);
      })
  }

  deleteCutted(id) {
    this.loading.start();
    this.httpSubscription = this.http.get('/api/DeleteCutted',
      {
        params: { id: id }
      }).subscribe(e => {
        let message = e.json();
        this.notifier.notify(message.type, message.message);
        this.getCuttedData(false);
      })
  }
}
