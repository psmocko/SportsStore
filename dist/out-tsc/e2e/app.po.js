"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var SportsStorePage = /** @class */ (function () {
    function SportsStorePage() {
    }
    SportsStorePage.prototype.navigateTo = function () {
        return protractor_1.browser.get('/');
    };
    SportsStorePage.prototype.getParagraphText = function () {
        return protractor_1.element(protractor_1.by.css('app-root h1')).getText();
    };
    return SportsStorePage;
}());
exports.SportsStorePage = SportsStorePage;
//# sourceMappingURL=app.po.js.map