import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-flower-info',
  templateUrl: './flower-info.component.html',
  styleUrls: ['./flower-info.component.less']
})
export class FlowerInfoComponent implements OnInit, OnDestroy {
  flowerId: number;
  paramSubscription: Subscription
  constructor(private router: ActivatedRoute) { }

  ngOnInit() {
    this.paramSubscription = this.router.params.subscribe((params: Params) => {
      this.flowerId = params['fid'];
    })
  }
  ngOnDestroy() {
    this.paramSubscription.unsubscribe();
  }

}
