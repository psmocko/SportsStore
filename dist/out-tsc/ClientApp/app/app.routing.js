"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var productSelection_component_1 = require("./store/productSelection.component");
var cartDetail_component_1 = require("./store/cartDetail.component");
var checkoutDetails_component_1 = require("./store/checkout/checkoutDetails.component");
var checkoutPayment_component_1 = require("./store/checkout/checkoutPayment.component");
var checkoutSummary_component_1 = require("./store/checkout/checkoutSummary.component");
var orderConfirmation_component_1 = require("./store/checkout/orderConfirmation.component");
var routes = [
    { path: 'checkout/step1', component: checkoutDetails_component_1.CheckoutDetailsComponent },
    { path: 'checkout/step2', component: checkoutPayment_component_1.CheckoutPaymentComponent },
    { path: 'checkout/step3', component: checkoutSummary_component_1.CheckoutSummaryComponent },
    { path: 'checkout/confirmation', component: orderConfirmation_component_1.OrderConfirmationComponent },
    { path: 'checkout', component: checkoutDetails_component_1.CheckoutDetailsComponent },
    { path: 'cart', component: cartDetail_component_1.CartDetailComponent },
    { path: 'store', component: productSelection_component_1.ProductSelectionComponent },
    { path: '', component: productSelection_component_1.ProductSelectionComponent }
];
exports.RoutingConfig = router_1.RouterModule.forRoot(routes);
//# sourceMappingURL=app.routing.js.map