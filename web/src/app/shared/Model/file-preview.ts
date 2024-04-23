import { FileTypes } from './enums/file-types';

export interface FilePreview {
  name: string;
  type: FileTypes;
  size: number;
  thumbnail: string;
}
