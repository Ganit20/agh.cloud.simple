import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FilePreview } from '../Model/file-preview';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  url = 'http://localhost:53547/api';
  constructor(private http: HttpClient) {}
  uploadFile(
    name: string,
    type: string,
    path: string,
    file: string | ArrayBuffer | null
  ) {
    return this.http.post(`${this.url}/file`, {
      name: name,
      file: file,
      type: type,
      path: path,
    });
  }

  getFiles() {
    return this.http.get<FilePreview[]>('http://localhost:53547/api/file');
  }
}
