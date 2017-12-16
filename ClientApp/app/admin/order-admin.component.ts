import { Component } from '@angular/core';
import { Repository } from '../models/repository';
import { Order } from '../models/order.model';

@Component({    
    templateUrl: './order-admin.component.html'
})

export class OrderAdminComponent {
    
  constructor(private repo: Repository) { }

  get orders(): Order[] {
    return this.repo.orders;
  }

  markShipped(order: Order) {
    this.repo.shipOrder(order);
  }
}
