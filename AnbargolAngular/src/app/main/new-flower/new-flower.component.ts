import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient } from 'selenium-webdriver/http';
import { Http } from '@angular/http';
import { NotifierService } from 'angular-notifier';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-new-flower',
  templateUrl: './new-flower.component.html',
  styleUrls: ['./new-flower.component.less']
})

export class NewFlowerComponent implements OnInit, OnDestroy {
  colors = [];
  colorTypes = [];
  formats = [];
  companies = [];
  customers = [];
  fileToUpload: File = null;
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
  })
  constructor(private http: Http, private notifier: NotifierService) { }

  ngOnInit() {
    this.httpSubscription = this.http.get('/api/GetControls').subscribe(e => {
      let data = e.json();
      this.colors = data.color
      this.colorTypes = data.colorType
      this.companies = data.company
      this.formats = data.format
      this.customers = data.customer
    })
  }

  ngOnDestroy() {
    this.httpSubscription.unsubscribe();
  }

  sabtGol() {
    if (this.newGolForm.status == 'INVALID') {
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
      this.httpSubscription = this.http.post('/api/NewFlower', obj).subscribe(e => {
        let m = e.json();
        this.notifier.notify(m.type, m.message);
        this.newGolForm.reset();
      })
    }
  }
}
