import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { NotifierService } from 'angular-notifier';
import { Arranges, ApiAlert } from 'src/app/shared/sharedClass.service';

@Component({
  selector: 'app-arranges',
  templateUrl: './arranges.component.html',
  styleUrls: ['./arranges.component.less']
})
export class ArrangesComponent implements OnInit, OnDestroy {
  dimensions = [];
  arranges: Arranges[] = [];
  flowerId: number;
  newArrange: Arranges
  routeSubscription: Subscription
  httpSubscription: Subscription
  newGolArrange = new FormGroup({
    name: new FormControl('', Validators.required),
    arrangeType: new FormControl('', Validators.required),
    dimension: new FormControl('', Validators.required),
    sheetCount: new FormControl('', Validators.required),
    mark: new FormControl(''),
    comment: new FormControl(''),
    date: new FormControl('', Validators.required)
  })
  constructor(private route: ActivatedRoute,
    private http: HttpClient,
    private notifier: NotifierService) { }

  ngOnInit() {
    this.httpSubscription = this.http.get('/api/GetControls').subscribe((e: any) => {
      this.dimensions = e.dimension;
    })
    this.routeSubscription = this.route.params.subscribe((p: Params) => {
      this.flowerId = p['fid'];
      this.getAllArranges();
    });
  }

  getAllArranges() {
    this.httpSubscription = this.http.get('/api/GetArranges', {
      params: new HttpParams().set('flowerId', this.flowerId + "")
    }).subscribe((e: Arranges[]) => {
      this.arranges = e;
    })
  }

  ngOnDestroy() {
    this.routeSubscription.unsubscribe();
    this.httpSubscription.unsubscribe();
  }

  saveArrange() {
    if (this.newGolArrange.status === "INVALID") {
      this.notifier.notify('error', 'لطفا خطاهای موجود را بررسی نمایید');
    } else {
      this.newArrange = {
        name: this.newGolArrange.get('name').value,
        arrangeType: this.newGolArrange.get('arrangeType').value,
        dimension: this.newGolArrange.get('dimension').value,
        count: this.newGolArrange.get('sheetCount').value,
        comment: this.newGolArrange.get('comment').value,
        date: this.newGolArrange.get('date').value,
        mark: this.newGolArrange.get('mark').value
      }
      this.httpSubscription = this.http.post('/api/SaveArrange', this.newArrange).subscribe((resp: ApiAlert) => {
        this.notifier.notify(resp.type, resp.message);
      });
    }
  }

  editSheetCount(plus: number, change: number, arrangeId: number) {
    console.log(plus, change, arrangeId)
  }

  editArrange(arrageId: number) {
    console.log(arrageId)
  }

  deleteArrange(arrangeId: number) {
    console.log(arrangeId)
  }
}
