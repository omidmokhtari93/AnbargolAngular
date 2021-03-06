import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Http } from '@angular/http';
import { NotifierService } from 'angular-notifier';
import { Subscription } from 'rxjs';
import { HtttpService } from 'src/app/shared/httpService.service';
import { NgxSmartModalService } from 'ngx-smart-modal';
import { Router } from '@angular/router';
import * as moment from 'jalali-moment';

@Component({
  selector: 'app-new-flower',
  templateUrl: './new-flower.component.html',
  styleUrls: ['./new-flower.component.less']
})

export class NewFlowerComponent implements OnInit, OnDestroy {
  datePickerConfig = {
    format: 'jYYYY/jMM/jDD'
  }
  flowerId: number;
  colors = [];
  colorTypes = [];
  formats = [];
  companies = [];
  customers = [];
  flowers = [];
  data = new FormData();
  httpSubscription: Subscription;
  newGolForm = new FormGroup({
    name: new FormControl('', Validators.required),
    code: new FormControl('', Validators.required),
    format: new FormControl('', Validators.required),
    color: new FormControl('', Validators.required),
    colorType: new FormControl('', Validators.required),
    customer: new FormControl('', Validators.required),
    company: new FormControl('', Validators.required),
    enterDate: new FormControl('', Validators.required),
    comment: new FormControl(''),
    file: new FormControl('')
  })
  constructor(private http: Http,
    private notifier: NotifierService,
    private httpService: HtttpService,
    private modal: NgxSmartModalService,
    private route: Router) { }
    
  ngOnInit() {
    this.fillControls();
    this.getLastFlowers();
  }

  fillControls() {
    this.httpSubscription = this.httpService.get('/api/GetControls', '').subscribe((e: any) => {
      this.colors = e.color;
      this.colorTypes = e.colorType;
      this.companies = e.company;
      this.formats = e.format;
      this.customers = e.customer;
    });
  }

  getLastFlowers() {
    this.httpSubscription = this.httpService.get('/api/GetAllFlowers', '').subscribe((response: any) => {
      this.flowers = response;
    });
  }

  ngOnDestroy() {
    this.httpSubscription.unsubscribe();
  }

  handleFileInput(file: FileList) {
    const formData = new FormData();
    formData.append('flower-image', file.item(0));
    this.data = formData;
  }

  sabtGol() {
    console.log(this.newGolForm);
    if (this.newGolForm.status === 'INVALID') {
      this.notifier.notify('error', 'لطفا خطاهای موجود را بررسی نمایید');
    } else {
      let obj = {
        Name: this.newGolForm.get('name').value,
        Code: this.newGolForm.get('code').value,
        Color: this.newGolForm.get('color').value,
        ColorType: this.newGolForm.get('colorType').value,
        Format: this.newGolForm.get('format').value,
        Customer: this.newGolForm.get('customer').value,
        Company: this.newGolForm.get('company').value,
        EnterDate: this.newGolForm.get('enterDate').value,
        Comment: this.newGolForm.get('comment').value,
      }
      this.data.append('flower-data', JSON.stringify(obj))
      this.httpService.post('/api/NewFlower', this.data).subscribe((response: any) => {
        this.notifier.notify(response.type, response.message);
        this.newGolForm.reset();
        this.getLastFlowers();
      })
    }
  }

  copy(id: number) {
    this.flowerId = id;
    this.modal.getModal('copyModal').open();
  }

  arrange(id: number) {
    this.route.navigate(["arranges", id]);
    console.log()
  }

  copyflower() {
    this.httpSubscription = this.httpService.get('/api/CopyFlower', { params: { flowerId: this.flowerId } }).subscribe((e: any) => {
      this.notifier.notify(e.type, e.message);
      this.modal.getModal('copyModal').close();
      this.getLastFlowers();
    })
  }
}