import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { ModelModule } from './models/model.module';
import { AdminModule } from './admin/admin.module';
//import { ProductTableComponent } from './structure/productTable.component';
//import { CategoryFilterComponent } from './structure/categoryFilter.component';
//import { ProductDetailComponent } from './structure/productDetail.component';
import { RoutingConfig } from './app.routing';
import { StoreModule } from './store/store.module';
import { ProductSelectionComponent } from './store/productSelection.component';

import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent,
    //ProductTableComponent,
    //CategoryFilterComponent,
    //ProductDetailComponent
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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
