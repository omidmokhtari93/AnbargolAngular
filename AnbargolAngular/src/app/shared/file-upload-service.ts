import { OnInit } from '@angular/core';
import { Http } from '@angular/http';

export class FileUploadService implements OnInit {

  ngOnInit() {

  }
  constructor(private http: Http) { }
  postFile(fileToUpload: File) {
    const endpoint = '/api/UploadFile';
    const formData: FormData = new FormData();
    formData.append('flower-image', fileToUpload, fileToUpload.name);
    this.http.post(endpoint, formData).subscribe(e => {
      console.log(e.json());
    })
  }
}
