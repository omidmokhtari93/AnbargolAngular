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
  pageSize = 10;
  flowerId: any;
  pages = [];
  orders: string[] = [];
  forms: string[] = [];
  gol: string[] = [];
  formItems: string[] = [];
  formNumbers: string[] = [];
  loading: boolean = false;
  paramSubscription: Subscription
  httpSubscription: Subscription
  constructor(private router: ActivatedRoute, private http: Http) { }

  ngOnInit() {
    this.loading = true;
    this.paramSubscription = this.router.params.subscribe((p: Params) => {
      this.httpSubscription = this.http.get('/api/GetFlowerData', {
        params: {
          flowerId: p['fid'],
          pageSize: this.pageSize
        }
      }).subscribe(e => {
        let data = e.json();
        this.forms = data['forms'];
        this.gol = data['gol'];
        this.formItems = data['formItems'];
        this.formNumbers = data['formNumbers']
        this.orders = data['orders']
        this.loading = false;
        this.flowerId = p['fid'];
        this.pageCountCalcualte(this.pageSize, p['fid']);
      });
    });
  }

  ngOnDestroy() {
    this.paramSubscription.unsubscribe();
    this.httpSubscription.unsubscribe();
  }

  changeFormAction(e) {
    this.loading = true;
    this.http.get('/api/GetFlowForm', { params: { formId: e.target.value } }).subscribe(e => {
      console.log(e.json());
      let data = e.json();
      this.formItems = data['formItems'];
      this.forms = data['form'];
      this.loading = false;
    })
  }

  pageCountCalcualte(pageSize, flowerId) {
    this.http.get('/api/OrderPagesCount',
      {
        params: { pageSize: pageSize, flowerId: flowerId }
      }).subscribe(e => {
        this.pages = [];
        for (var i = 0; i < e.json(); i++) {
          this.pages.push({ size: pageSize, index: i })
        }
      })
  }
  paginateOrderTable(index, size) {
    this.http.get('/api/Orders', {
      params: {
        flowerId: this.flowerId, take: index * size, skip: size * (index + 1)
      }
    }).subscribe(e => {
      this.orders = e.json();
    })
  }
  showOrderForms(id) {
    console.log(id);
  }
}
