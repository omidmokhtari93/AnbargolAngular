import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-new-flower',
  templateUrl: './new-flower.component.html',
  styleUrls: ['./new-flower.component.less']
})
export class NewFlowerComponent implements OnInit {
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
  constructor() { }

  ngOnInit() {
    //console.log(this.newGolForm);
  }

  sabtGol() {
    console.log(this.newGolForm);
  }
}
