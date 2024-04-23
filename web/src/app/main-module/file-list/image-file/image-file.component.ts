import { Component, Input, OnInit } from '@angular/core';
import { FilePreview } from '../../../shared/Model/file-preview';

@Component({
  selector: 'app-image-file',
  templateUrl: './image-file.component.html',
  styleUrls: ['./image-file.component.css'],
})
export class ImageFileComponent implements OnInit {
  @Input() file: FilePreview | null = null;
  constructor() {}

  ngOnInit() {}
}
