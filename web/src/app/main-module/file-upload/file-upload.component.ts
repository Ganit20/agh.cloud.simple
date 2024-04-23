import { Component, OnInit } from '@angular/core';
import { FileService } from '../../shared/services/file.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
})
export class FileUploadComponent implements OnInit {
  constructor(private http: HttpClient, private fileService: FileService) {}

  ngOnInit() {}
  uploadFile(event: any) {
    for (let file of event.files) {
      console.log(file);
      this.http
        .get(file.objectURL.changingThisBreaksApplicationSecurity, {
          responseType: 'blob',
        })
        .subscribe((resp: any) => {
          var reader = new FileReader();
          reader.readAsDataURL(resp);

          reader.onloadend = () => {
            var base64data = reader.result;
            console.log(reader);
            this.fileService
              .uploadFile(file.name, file.type, '/', base64data)
              .subscribe((x) => {});
          };
        });
    }
  }
}
