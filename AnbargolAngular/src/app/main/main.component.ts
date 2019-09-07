import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.less']
})
export class MainComponent implements OnInit {
  subjectMessage: string;
  golSearchConfig = {
    placeholder: 'جستجوی سریع طرح ...',
    width: '300px',
    api: '/api/SearchGol',
    zindex: '600',
    arg: 'golName'
  }
  constructor(private route: Router) { }

  ngOnInit() {

  }

  createGolList(e) {
    this.route.navigate(['flowerinfo', e.id]);
  }
}
