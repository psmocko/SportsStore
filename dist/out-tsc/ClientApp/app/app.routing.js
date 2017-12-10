"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
//import { ProductTableComponent } from './structure/productTable.component';
//import { ProductDetailComponent } from './structure/productDetail.component';
var productSelection_component_1 = require("./store/productSelection.component");
var cartDetail_component_1 = require("./store/cartDetail.component");
var routes = [
    { path: 'cart', component: cartDetail_component_1.CartDetailComponent },
    { path: 'store', component: productSelection_component_1.ProductSelectionComponent },
    { path: '', component: productSelection_component_1.ProductSelectionComponent }
];
exports.RoutingConfig = router_1.RouterModule.forRoot(routes);
//# sourceMappingURL=app.routing.js.map