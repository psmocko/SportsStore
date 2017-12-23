"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var platform_browser_1 = require("@angular/platform-browser");
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var model_module_1 = require("./models/model.module");
var admin_module_1 = require("./admin/admin.module");
var app_routing_1 = require("./app.routing");
var store_module_1 = require("./store/store.module");
var error_handler_service_1 = require("./error-handler.service");
var auth_module_1 = require("./auth/auth.module");
var authentication_service_1 = require("./auth/authentication.service");
var app_component_1 = require("./app.component");
var eHandler = new error_handler_service_1.ErrorHandlerService();
function handler() {
    return eHandler;
}
exports.handler = handler;
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            declarations: [
                app_component_1.AppComponent,
            ],
            imports: [
                platform_browser_1.BrowserModule,
                forms_1.FormsModule,
                http_1.HttpModule,
                model_module_1.ModelModule,
                app_routing_1.RoutingConfig,
                store_module_1.StoreModule,
                admin_module_1.AdminModule,
                auth_module_1.AuthModule
            ],
            providers: [
                { provide: error_handler_service_1.ErrorHandlerService, useFactory: handler },
                { provide: core_1.ErrorHandler, useFactory: handler },
                authentication_service_1.AuthenticationService
            ],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map