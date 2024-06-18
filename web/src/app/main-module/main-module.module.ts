import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileListComponent } from './file-list/file-list.component';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { FileUploadModule } from 'primeng/fileupload';
import { HttpClientModule } from '@angular/common/http';
import { ImageFileComponent } from './file-list/image-file/image-file.component';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { FileService } from '../shared/services/file.service';
import { ImageModule } from 'primeng/image';
import {
  BrowserAnimationsModule,
  NoopAnimationsModule,
} from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { ContextMenuModule } from 'primeng/contextmenu';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { PanelMenuModule } from 'primeng/panelmenu';
import { MenuItem } from 'primeng/api';
import { MainModuleRoutes } from './main-module.routing';
import { AppComponent } from '../app.component';
import { SidebarModule } from 'primeng/sidebar';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { ToolbarModule } from 'primeng/toolbar';
import { AvatarModule } from 'primeng/avatar';
import { PanelModule } from 'primeng/panel';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { DialogModule } from 'primeng/dialog';

@NgModule({
  imports: [
    ButtonModule,
    CardModule,
    ImageModule,
    ContextMenuModule,
    FileUploadModule,
    HttpClientModule,
    PanelMenuModule,
    SidebarModule,
    MainModuleRoutes,
    ToolbarModule,
    AvatarModule,
    PanelModule,
    BreadcrumbModule,
    DialogModule,
  ],
  declarations: [
    FileListComponent,
    MainLayoutComponent,
    ImageFileComponent,
    SideMenuComponent,
    NavBarComponent,
    AppComponent,
    FileUploadComponent,
  ],
  bootstrap: [AppComponent],

  providers: [FileService],
})
export class MainModuleModule {}
