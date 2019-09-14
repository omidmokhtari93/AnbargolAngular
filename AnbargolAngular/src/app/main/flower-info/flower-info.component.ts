import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { Http } from '@angular/http';
import { FlowerInformation, Arranges } from '../../shared/sharedClass.service';

@Component({
  selector: 'app-flower-info',
  templateUrl: './flower-info.component.html',
  styleUrls: ['./flower-info.component.less']
})
export class FlowerInfoComponent implements OnInit, OnDestroy {
  pageinationButton: any;
  orderId = 0;
  pageSize = 10;
  flowerId: any;
  pages = [];
  orders = [];
  ordreForms = [];
  forms: Arranges;
  gol: FlowerInformation;
  formItems = [];
  formNumbers = [];
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
        this.ordreForms = [];
        this.pageCountCalcualte(this.pageSize, p['fid']);
        this.orderId = 0;
      });
    });
  }

  ngOnDestroy() {
    this.paramSubscription.unsubscribe();
    this.httpSubscription.unsubscribe();
  }

  changeFormAction(e) {
    this.loading = true;
    this.httpSubscription = this.http.get('/api/GetFlowForm', { params: { formId: e.target.value } }).subscribe(e => {
      let data = e.json();
      this.formItems = data['formItems'];
      this.forms = data['form'];
      this.loading = false;
    })
  }

  pageCountCalcualte(pageSize, flowerId) {
    this.httpSubscription = this.http.get('/api/OrderPagesCount',
      {
        params: { pageSize: pageSize, flowerId: flowerId }
      }).subscribe(e => {
        this.pages = [];
        for (var i = 0; i < e.json(); i++) {
          this.pages.push({ size: pageSize, index: i })
        }
      })
  }

  paginateOrderTable(index, size, button) {
    if (this.pageinationButton == undefined) {
      this.pageinationButton = button;
      button.style.color = 'red';
    } else {
      this.pageinationButton.style.color = '';
      this.pageinationButton = button;
      button.style.color = 'red';
    }
    this.httpSubscription = this.http.get('/api/Orders', {
      params: {
        flowerId: this.flowerId, take: index * size, skip: size * (index + 1)
      }
    }).subscribe(e => {
      this.orders = e.json();
    })
  }

  showOrderForms(id) {
    this.httpSubscription = this.http.get('/api/GetOrderForms',
      {
        params: { orderId: id }
      }).subscribe(e => {
        this.orderId = id;
        this.ordreForms = e.json();
      })
  }
}
