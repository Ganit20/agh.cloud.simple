import {
  bootstrapApplication,
  platformBrowser,
} from '@angular/platform-browser';
import { MainModuleModule } from './app/main-module/main-module.module';
platformBrowser()
  .bootstrapModule(MainModuleModule)
  .catch((err) => console.error(err));
