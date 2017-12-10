import { Routes, RouterModule } from '@angular/router';
//import { ProductTableComponent } from './structure/productTable.component';
//import { ProductDetailComponent } from './structure/productDetail.component';
import { ProductSelectionComponent } from './store/productSelection.component';
import { CartDetailComponent } from './store/cartDetail.component';

const routes: Routes = [
  { path: 'cart', component: CartDetailComponent },
  { path: 'store', component: ProductSelectionComponent },
  { path: '', component: ProductSelectionComponent }
]

export const RoutingConfig = RouterModule.forRoot(routes);
