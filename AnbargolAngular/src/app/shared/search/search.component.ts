import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { HostListener } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.less']
})
export class SearchComponent implements OnInit {
  @Input() config: any;
  @Output() output = new EventEmitter<any>();
  @ViewChild('parentdiv', { static: true }) parentdiv: any;
  @ViewChild('resultWidth', { static: true }) resultWidth: any;
  @ViewChild('ul', { static: true }) ul: any;
  placeholder: string = 'جستجو ...';
  width: string = '100%';
  zindex: string = '500';
  searchField: string = '';
  liItems = [];
  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (this.parentdiv.nativeElement.contains(event.target)) {
      return;
    } else {
      this.liItems = [];
      this.searchField = '';
      this.ul.nativeElement.style.borderBottomWidth = "0"
    }
  }
  constructor(private http: Http) { }

  ngOnInit() {
    if (!this.config.length) {
      this.width = this.config.width;
      this.placeholder = this.config.placeholder;
      this.zindex = this.config.zindex;
    }
    this.ul.nativeElement.style.borderBottomWidth = "0"
  }

  ngAfterViewInit() {
    this.resultWidth.nativeElement.style.width = (this.parentdiv.nativeElement.clientWidth + 2) + 'px';
  }

  search() {
    let gol = '{"' + this.config.arg + '\" : \"' + this.searchField + '\"}';
    this.liItems = [];
    if (this.searchField.length > 2 && !this.isEmpty(this.searchField)) {
      this.http.get(this.config.api, { params: JSON.parse(gol) }).subscribe(e => {
        this.liItems = e.json();
        if (this.liItems.length) {
          this.ul.nativeElement.style.borderBottom = "1px solid #ced4da";
        }
      })
    } else {
      this.ul.nativeElement.style.borderBottomWidth = "0"
    }
  }

  isEmpty(input) {
    return !(input.replace(/\s/g, "").length > 0);
  }

  addItem(itemid, itemname) {
    this.liItems = [];
    this.output.emit({ id: itemid, text: itemname });
  }
}
