import { HttpClient } from '@angular/common/http';
import { Component, OnInit, WritableSignal, signal } from '@angular/core';
import { FormBuilder, FormControl, UntypedFormGroup } from '@angular/forms';
import { FilePreview } from '../../shared/Model/file-preview';
import { FileTypes } from '../../shared/Model/enums/file-types';
import { FileService } from '../../shared/services/file.service';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.css'],
})
export class FileListComponent implements OnInit {
  constructor(private fb: FormBuilder, private fileService: FileService) {}

  path = '/';
  types = FileTypes;
  files: WritableSignal<FilePreview[]> = signal([]);
  home = { icon: 'pi pi-home' };
  breadcrumbItems: MenuItem[] = [];

  ngOnInit() {
    this.getFiles();
    this.breadcrumbItems.push({ label: 'Electronics' });
    this.breadcrumbItems.push({ label: 'Computer' });
    this.breadcrumbItems.push({ label: 'Notebook' });
    this.breadcrumbItems.push({ label: 'Accessories' });
    this.breadcrumbItems.push({ label: 'Backpacks' });
    this.breadcrumbItems.push({ label: 'Item' });
  }

  getFiles() {
    this.fileService.getFiles().subscribe((x) => {
      console.log(x);
      this.files.set(x);
    });
  }
}
