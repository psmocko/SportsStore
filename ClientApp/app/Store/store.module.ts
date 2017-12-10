import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CartSummaryComponent } from './cartSummary.component';
import { CategoryFilterComponent } from './categoryFilter.component';
import { PaginationComponent } from './pagination.component';
import { ProductListComponent } from './productList.component';
import { RatingsComponent } from './ratings.component';
import { ProductSelectionComponent } from './productSelection.component';
import { CartDetailComponent } from './cartDetail.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CartSummaryComponent,
    CategoryFilterComponent,
    PaginationComponent,
    ProductListComponent,
    RatingsComponent,
    ProductSelectionComponent,
    CartDetailComponent
  ],
  imports: [
    BrowserModule,
    RouterModule,
    FormsModule
  ],
  exports: [
    ProductSelectionComponent
  ]
})
export class StoreModule { }
