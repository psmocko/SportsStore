
import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Repository } from '../models/repository';
import { Product } from '../models/product.model';

@Component({
  selector: 'product-detail',
  templateUrl: 'productDetail.component.html'
})
export class ProductDetailComponent {
  constructor(private repo: Repository, private router: Router, private activeRoute: ActivatedRoute) {
    let id = Number.parseInt(activeRoute.snapshot.params['id']);
    if (id) {
      this.repo.getProduct(id);
    } else {
      router.navigateByUrl('/');

    }
  }

  get product(): Product {
    return this.repo.product;
  }
}
