import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { ModelModule } from './models/model.module';
import { AdminModule } from './admin/admin.module';
import { RoutingConfig } from './app.routing';
import { StoreModule } from './store/store.module';
import { ProductSelectionComponent } from './store/productSelection.component';
import { ErrorHandlerService } from './error-handler.service';

import { AppComponent } from './app.component';

const eHandler = new ErrorHandlerService();

export function handler() {
  return eHandler;
}

@NgModule({
  declarations: [
    AppComponent,  
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    ModelModule,
    RoutingConfig,
    StoreModule,
    AdminModule
  ],
  providers: [
    { provide: ErrorHandlerService, useFactory: handler },
    { provide: ErrorHandler, useFactory: handler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
