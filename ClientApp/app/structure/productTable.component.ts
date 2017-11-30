
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Repository } from '../models/repository';
import { Product } from '../models/product.model';


@Component({
  selector: 'product-table',
  templateUrl: './productTable.component.html'
})
export class ProductTableComponent {
  constructor(private repo: Repository, private router: Router) {}

  get products(): Product[] {
    return this.repo.products;
  }

  selectProduct(id: number): void {
    this.repo.getProduct(id);
    this.router.navigateByUrl('/detail');
  }
}
