import { Component, OnInit, Input } from '@angular/core';
import { FlowerInformation } from '../sharedClass.service';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-flower-main-info',
  templateUrl: './flower-main-info.component.html',
  styleUrls: ['./flower-main-info.component.less']
})
export class FlowerMainInfoComponent implements OnInit {
  @Input('flowerid') id: number = 0;
  gol: FlowerInformation;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get('/api/GetFlowerMainInfo', {
      params: new HttpParams().set("flowerId", this.id + "")
    }).subscribe((flowerInfo: FlowerInformation) => {
      this.gol = flowerInfo;
    })
  }
} 
