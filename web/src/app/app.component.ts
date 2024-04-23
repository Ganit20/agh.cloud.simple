import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainModuleModule } from './main-module/main-module.module';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'cloud.web';
}
