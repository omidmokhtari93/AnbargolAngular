import { Component, OnInit, Input } from '@angular/core';
import { Http } from '@angular/http';

@Component({
  selector: 'app-flower-main-info',
  templateUrl: './flower-main-info.component.html',
  styleUrls: ['./flower-main-info.component.less']
})
export class FlowerMainInfoComponent implements OnInit {
  @Input('flowerid') id: number = 0;
  gol = [];
  constructor(private http: Http) { }

  ngOnInit() {
    this.http.get('/api/GetFlowerMainInfo', { params: { flowerId: this.id } }).subscribe(e => {
      this.gol = e.json();
    })
  }
}
