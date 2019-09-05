import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-arranges',
  templateUrl: './arranges.component.html',
  styleUrls: ['./arranges.component.less']
})
export class ArrangesComponent implements OnInit, OnDestroy {
  flowerId: number;
  routeSubscription: Subscription
  newGolArrange = new FormGroup({
    name: new FormControl('', Validators.required),
    arrangeType: new FormControl('', Validators.required),
    dimension: new FormControl('', Validators.required),
    sheetCount: new FormControl('', Validators.required),
    mark: new FormControl(''),
    comment: new FormControl(''),
    date: new FormControl('', Validators.required)
  })
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe((p: Params) => {
      this.flowerId = p['fid'];
    });
  }

  ngOnDestroy() {
    this.routeSubscription.unsubscribe();
  }

}
