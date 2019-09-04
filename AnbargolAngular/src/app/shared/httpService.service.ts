import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class HtttpService implements OnInit {
    constructor(private httpClient: HttpClient) { }
    ngOnInit() {

    }

    get(url: string, data: any) {
        return this.httpClient.get(url, data);
    }

    post(url: string, data: any) {
        return this.httpClient.post(url, data);
    }
}