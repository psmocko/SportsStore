"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Cart = (function () {
    function Cart() {
        this.selections = [];
        this.itemCount = 0;
        this.totalPrice = 0;
    }
    Cart.prototype.addProduct = function (product) {
        var selection = this.selections.find(function (ps) { return ps.productId == product.productId; });
        if (selection) {
            selection.quantity++;
        }
        else {
            this.selections.push(new ProductSelection(this, product.productId, product.name, product.price, 1));
        }
        this.update();
    };
    Cart.prototype.updateQuantity = function (productId, quantity) {
        if (quantity > 0) {
            var selection = this.selections.find(function (ps) { return ps.productId == productId; });
            if (selection) {
                selection.quantity = quantity;
            }
            else {
                var index = this.selections.findIndex(function (ps) { return ps.productId == productId; });
                if (index != -1) {
                    this.selections.splice(index, 1);
                }
                this.update();
            }
        }
    };
    Cart.prototype.clear = function () {
        this.selections = [];
        this.update();
    };
    Cart.prototype.update = function () {
        this.itemCount = this.selections.map(function (ps) { return ps.quantity; }).reduce(function (prev, curr) { return prev + curr; }, 0);
        this.totalPrice = this.selections.map(function (ps) { return ps.price * ps.quantity; }).reduce(function (prev, curr) { return prev + curr; }, 0);
    };
    return Cart;
}());
Cart = __decorate([
    core_1.Injectable()
], Cart);
exports.Cart = Cart;
var ProductSelection = (function () {
    function ProductSelection(cart, productId, name, price, quantityValue) {
        this.cart = cart;
        this.productId = productId;
        this.name = name;
        this.price = price;
        this.quantityValue = quantityValue;
    }
    Object.defineProperty(ProductSelection.prototype, "quantity", {
        get: function () {
            return this.quantityValue;
        },
        set: function (newQuantity) {
            this.quantityValue = newQuantity;
            this.cart.update();
        },
        enumerable: true,
        configurable: true
    });
    return ProductSelection;
}());
exports.ProductSelection = ProductSelection;
//# sourceMappingURL=cart.model.js.map