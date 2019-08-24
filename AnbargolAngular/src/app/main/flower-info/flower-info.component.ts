import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { Http } from '@angular/http';

@Component({
  selector: 'app-flower-info',
  templateUrl: './flower-info.component.html',
  styleUrls: ['./flower-info.component.less']
})
export class FlowerInfoComponent implements OnInit, OnDestroy {
  fData: string[] = [];
  paramSubscription: Subscription
  httpSubscription: Subscription
  constructor(private router: ActivatedRoute, private http: Http) { }

  ngOnInit() {
    this.paramSubscription = this.router.params.subscribe((p: Params) => {
      this.httpSubscription = this.http.get('/api/GetFlowerData', { params: { flowerId: p['fid'] } }).subscribe(e => {
        this.fData = e.json();
        console.log(this.fData);
      });
    });
  }
  ngOnDestroy() {
    this.paramSubscription.unsubscribe();
    this.httpSubscription.unsubscribe();
  }
  changeFormAction(e) {
    console.log(e.target.value);
  }

}
