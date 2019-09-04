import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Response } from 'selenium-webdriver/http';
import { Http } from '@angular/http';
import { NotifierService } from 'angular-notifier';
import { Subscription } from 'rxjs';
import { HtttpService } from 'src/app/shared/httpService.service';
import { NgxSmartModalService } from 'ngx-smart-modal';

@Component({
  selector: 'app-new-flower',
  templateUrl: './new-flower.component.html',
  styleUrls: ['./new-flower.component.less']
})

export class NewFlowerComponent implements OnInit, OnDestroy {
  flowerId: number;
  colors = [];
  colorTypes = [];
  formats = [];
  companies = [];
  customers = [];
  flowers = [];
  fileToUpload = new FormData();
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
    private modal: NgxSmartModalService) { }
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
    this.fileToUpload = formData;
  }

  sabtGol() {
    if (this.newGolForm.status === 'INVALID') {
      this.notifier.notify('error', 'لطفا فیلد های خالی را تمکیل نمایید');
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
      this.fileToUpload.append('flower-data', JSON.stringify(obj))
      this.httpService.post('/api/NewFlower', this.fileToUpload).subscribe((response: any) => {
        this.notifier.notify(response.type, response.message);
        this.newGolForm.reset();
      })
    }
  }

  copy(id: number) {
    this.flowerId = id;
    this.modal.getModal('myModal').open();
  }

  arrange(id: number) {
    console.log()
  }

  copyflower() {
    console.log(this.flowerId)
  }
}