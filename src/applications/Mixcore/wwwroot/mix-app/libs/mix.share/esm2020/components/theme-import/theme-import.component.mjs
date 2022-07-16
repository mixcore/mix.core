import { ChangeDetectionStrategy, Component, EventEmitter, Inject, Output } from '@angular/core';
import { ThemeApiService } from '../../services';
import { ShareModule } from '../../share.module';
import { DOMAIN_URL } from '../../token/base-url.token';
import { ModalService } from '../modal/modal.service';
import * as i0 from "@angular/core";
import * as i1 from "../../services";
import * as i2 from "@angular/common";
import * as i3 from "@taiga-ui/core";
import * as i4 from "@taiga-ui/kit";
import * as i5 from "../modal/modal.service";
function ThemeImportComponent_div_6_ng_container_1_ng_container_1_div_2_Template(rf, ctx) { if (rf & 1) {
    const _r10 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "div", 13)(1, "div", 14)(2, "div", 15);
    i0.ɵɵelement(3, "img", 16);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(4, "div", 17);
    i0.ɵɵtext(5);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(6, "div", 18);
    i0.ɵɵtext(7);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(8, "div", 19);
    i0.ɵɵtext(9);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(10, "button", 20);
    i0.ɵɵtext(11, "View");
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(12, "button", 21);
    i0.ɵɵlistener("click", function ThemeImportComponent_div_6_ng_container_1_ng_container_1_div_2_Template_button_click_12_listener() { const restoredCtx = i0.ɵɵrestoreView(_r10); const item_r8 = restoredCtx.$implicit; const ctx_r9 = i0.ɵɵnextContext(4); return i0.ɵɵresetView(ctx_r9.selectTheme(item_r8)); });
    i0.ɵɵtext(13, "Select");
    i0.ɵɵelementEnd()()();
} if (rf & 2) {
    const item_r8 = ctx.$implicit;
    const ctx_r7 = i0.ɵɵnextContext(4);
    i0.ɵɵadvance(3);
    i0.ɵɵpropertyInterpolate("src", item_r8.imageUrl, i0.ɵɵsanitizeUrl);
    i0.ɵɵadvance(2);
    i0.ɵɵtextInterpolate1(" ", item_r8.createdBy || "Administrator", " ");
    i0.ɵɵadvance(2);
    i0.ɵɵtextInterpolate1(" ", item_r8.title, " ");
    i0.ɵɵadvance(2);
    i0.ɵɵtextInterpolate1(" ", item_r8.excerpt, "");
    i0.ɵɵadvance(3);
    i0.ɵɵproperty("disabled", !!ctx_r7.currentSelectedTheme && ctx_r7.currentSelectedTheme.id === item_r8.id);
} }
function ThemeImportComponent_div_6_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "div", 11);
    i0.ɵɵtemplate(2, ThemeImportComponent_div_6_ng_container_1_ng_container_1_div_2_Template, 14, 5, "div", 12);
    i0.ɵɵelementEnd();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const result_r1 = i0.ɵɵnextContext(2).ngIf;
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("ngForOf", result_r1.items);
} }
function ThemeImportComponent_div_6_ng_container_1_ng_template_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵtext(0, " There is no theme in MixCore Store now.");
} }
function ThemeImportComponent_div_6_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, ThemeImportComponent_div_6_ng_container_1_ng_container_1_Template, 3, 1, "ng-container", 9);
    i0.ɵɵtemplate(2, ThemeImportComponent_div_6_ng_container_1_ng_template_2_Template, 1, 0, "ng-template", null, 10, i0.ɵɵtemplateRefExtractor);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const _r5 = i0.ɵɵreference(3);
    const result_r1 = i0.ɵɵnextContext().ngIf;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", result_r1.items.length)("ngIfElse", _r5);
} }
function ThemeImportComponent_div_6_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1, " Upload ");
    i0.ɵɵelementContainerEnd();
} }
function ThemeImportComponent_div_6_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 7);
    i0.ɵɵtemplate(1, ThemeImportComponent_div_6_ng_container_1_Template, 4, 2, "ng-container", 8);
    i0.ɵɵtemplate(2, ThemeImportComponent_div_6_ng_container_2_Template, 2, 0, "ng-container", 8);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const ctx_r0 = i0.ɵɵnextContext();
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", ctx_r0.activeItemIndex === 0);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", ctx_r0.activeItemIndex === 1);
} }
export class ThemeImportComponent {
    constructor(themeApiService, modalService, domain) {
        this.themeApiService = themeApiService;
        this.modalService = modalService;
        this.domain = domain;
        this.activeItemIndex = 0;
        this.themeListVm$ = this.themeApiService.getThemeStore();
        this.currentSelectedTheme = null;
        this.cancel = new EventEmitter();
        this.themeSelect = new EventEmitter();
    }
    selectTheme(value) {
        this.currentSelectedTheme = value;
    }
    onCancelClick() {
        if (this.currentSelectedTheme) {
            this.modalService.confirm('Do you want to cancel?').subscribe(ok => {
                if (ok)
                    this.cancel.emit();
            });
        }
        else {
            this.cancel.emit();
        }
    }
    onUseThemeClick() {
        if (!this.currentSelectedTheme)
            return;
        this.themeSelect.emit(this.currentSelectedTheme);
        this.cancel.emit();
    }
}
ThemeImportComponent.ɵfac = function ThemeImportComponent_Factory(t) { return new (t || ThemeImportComponent)(i0.ɵɵdirectiveInject(i1.ThemeApiService), i0.ɵɵdirectiveInject(ModalService), i0.ɵɵdirectiveInject(DOMAIN_URL)); };
ThemeImportComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: ThemeImportComponent, selectors: [["mix-theme-import"]], outputs: { cancel: "cancel", themeSelect: "themeSelect" }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 13, vars: 6, consts: [[1, "theme-import"], [3, "activeItemIndex", "activeItemIndexChange"], ["tuiTab", "", 3, "click"], ["class", "theme-import__container", 4, "ngIf"], [1, "theme-import__footer"], ["tuiButton", "", "appearance", "secondary", 3, "click"], ["tuiButton", "", 3, "disabled", "click"], [1, "theme-import__container"], [4, "ngIf"], [4, "ngIf", "ngIfElse"], ["noTheme", ""], [1, "row"], ["class", "col-12 col-sm-6 col-md-4", 4, "ngFor", "ngForOf"], [1, "col-12", "col-sm-6", "col-md-4"], [1, "theme-card"], [1, "theme-card__thumbnail"], [3, "src"], [1, "theme-card__author"], [1, "theme-card__title"], [1, "theme-card__description"], ["tuiButton", "", "size", "s", 1, "theme-card__show-detail-btn"], ["tuiButton", "", "size", "s", 1, "theme-card__choose-btn", 3, "disabled", "click"]], template: function ThemeImportComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "tui-tabs", 1);
        i0.ɵɵlistener("activeItemIndexChange", function ThemeImportComponent_Template_tui_tabs_activeItemIndexChange_1_listener($event) { return ctx.activeItemIndex = $event; });
        i0.ɵɵelementStart(2, "button", 2);
        i0.ɵɵlistener("click", function ThemeImportComponent_Template_button_click_2_listener() { return ctx.activeItemIndex = 0; });
        i0.ɵɵtext(3, " From Mix Store ");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(4, "button", 2);
        i0.ɵɵlistener("click", function ThemeImportComponent_Template_button_click_4_listener() { return ctx.activeItemIndex = 1; });
        i0.ɵɵtext(5, " Upload ");
        i0.ɵɵelementEnd()();
        i0.ɵɵtemplate(6, ThemeImportComponent_div_6_Template, 3, 2, "div", 3);
        i0.ɵɵpipe(7, "async");
        i0.ɵɵelementStart(8, "div", 4)(9, "button", 5);
        i0.ɵɵlistener("click", function ThemeImportComponent_Template_button_click_9_listener() { return ctx.onCancelClick(); });
        i0.ɵɵtext(10, "Cancel");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(11, "button", 6);
        i0.ɵɵlistener("click", function ThemeImportComponent_Template_button_click_11_listener() { return ctx.onUseThemeClick(); });
        i0.ɵɵtext(12);
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("activeItemIndex", ctx.activeItemIndex);
        i0.ɵɵadvance(5);
        i0.ɵɵproperty("ngIf", i0.ɵɵpipeBind1(7, 4, ctx.themeListVm$));
        i0.ɵɵadvance(5);
        i0.ɵɵproperty("disabled", !ctx.currentSelectedTheme);
        i0.ɵɵadvance(1);
        i0.ɵɵtextInterpolate("Use " + (ctx.currentSelectedTheme == null ? null : ctx.currentSelectedTheme.title));
    } }, dependencies: [ShareModule, i2.NgForOf, i2.NgIf, i3.TuiButtonComponent, i4.TuiTabsComponent, i4.TuiTabComponent, i2.AsyncPipe], styles: [".theme-import[_ngcontent-%COMP%]{width:70vw;height:70vh;display:flex;flex-direction:column}.theme-import[_ngcontent-%COMP%]   *[_ngcontent-%COMP%]{box-sizing:border-box}.theme-import__container[_ngcontent-%COMP%]{margin-top:15px}.theme-import__footer[_ngcontent-%COMP%]{display:flex;justify-content:flex-end;align-items:center;margin-top:auto}.theme-import__footer[_ngcontent-%COMP%] > button[_ngcontent-%COMP%]{margin-left:10px}.theme-card[_ngcontent-%COMP%]{width:100%;border-radius:20px;border:1px solid var(--tui-base-04);height:350px;position:relative}.theme-card__thumbnail[_ngcontent-%COMP%]{border-top-left-radius:20px;border-top-right-radius:20px;overflow:hidden;width:100%;height:150px;border-bottom:1px solid var(--tui-base-04)}.theme-card__thumbnail[_ngcontent-%COMP%] > img[_ngcontent-%COMP%]{width:100%;height:100%;object-fit:cover}.theme-card__author[_ngcontent-%COMP%]{margin:10px 10px 0;font-size:12px;color:var(--tui-base-05)}.theme-card__title[_ngcontent-%COMP%]{margin:0 10px;font-weight:600}.theme-card__description[_ngcontent-%COMP%]{font-size:12px;margin:0 10px;text-overflow:ellipsis;-webkit-line-clamp:4;-webkit-box-orient:vertical;overflow:hidden;display:-webkit-box}.theme-card__show-detail-btn[_ngcontent-%COMP%]{position:absolute;bottom:-15px;right:80px}.theme-card__choose-btn[_ngcontent-%COMP%]{position:absolute;bottom:-15px;right:10px}"], changeDetection: 0 });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(ThemeImportComponent, [{
        type: Component,
        args: [{ selector: 'mix-theme-import', changeDetection: ChangeDetectionStrategy.OnPush, standalone: true, imports: [ShareModule], template: "<div class=\"theme-import\">\r\n  <tui-tabs [(activeItemIndex)]=\"activeItemIndex\">\r\n    <button (click)=\"activeItemIndex = 0\"\r\n            tuiTab>\r\n      From Mix Store\r\n    </button>\r\n    <button (click)=\"activeItemIndex = 1\"\r\n            tuiTab>\r\n      Upload\r\n    </button>\r\n  </tui-tabs>\r\n\r\n  <div *ngIf=\"themeListVm$ | async as result\"\r\n       class=\"theme-import__container\">\r\n    <ng-container *ngIf=\"activeItemIndex === 0\">\r\n      <ng-container *ngIf=\"result.items.length; else noTheme\">\r\n        <div class=\"row\">\r\n          <div *ngFor=\"let item of result.items\"\r\n               class=\"col-12 col-sm-6 col-md-4\">\r\n            <div class=\"theme-card\">\r\n              <div class=\"theme-card__thumbnail\">\r\n                <img src=\"{{item.imageUrl}}\">\r\n              </div>\r\n              <div class=\"theme-card__author\"> {{ item.createdBy || 'Administrator'}} </div>\r\n              <div class=\"theme-card__title\"> {{ item.title}} </div>\r\n              <div class=\"theme-card__description\"> {{ item.excerpt }}</div>\r\n              <button class=\"theme-card__show-detail-btn\"\r\n                      tuiButton\r\n                      size=\"s\">View</button>\r\n              <button class=\"theme-card__choose-btn\"\r\n                      [disabled]=\"!!currentSelectedTheme && currentSelectedTheme.id === item.id\"\r\n                      (click)=\"selectTheme(item)\"\r\n                      tuiButton\r\n                      size=\"s\">Select</button>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </ng-container>\r\n\r\n      <ng-template #noTheme> There is no theme in MixCore Store now.</ng-template>\r\n    </ng-container>\r\n\r\n    <ng-container *ngIf=\"activeItemIndex === 1\">\r\n      Upload\r\n    </ng-container>\r\n  </div>\r\n\r\n  <div class=\"theme-import__footer\">\r\n    <button (click)=\"onCancelClick()\"\r\n            tuiButton\r\n            appearance=\"secondary\">Cancel</button>\r\n\r\n    <button [disabled]=\"!currentSelectedTheme\"\r\n            (click)=\"onUseThemeClick()\"\r\n            tuiButton>{{ 'Use '+ currentSelectedTheme?.title }}</button>\r\n  </div>\r\n</div>\r\n", styles: [".theme-import{width:70vw;height:70vh;display:flex;flex-direction:column}.theme-import *{box-sizing:border-box}.theme-import__container{margin-top:15px}.theme-import__footer{display:flex;justify-content:flex-end;align-items:center;margin-top:auto}.theme-import__footer>button{margin-left:10px}.theme-card{width:100%;border-radius:20px;border:1px solid var(--tui-base-04);height:350px;position:relative}.theme-card__thumbnail{border-top-left-radius:20px;border-top-right-radius:20px;overflow:hidden;width:100%;height:150px;border-bottom:1px solid var(--tui-base-04)}.theme-card__thumbnail>img{width:100%;height:100%;object-fit:cover}.theme-card__author{margin:10px 10px 0;font-size:12px;color:var(--tui-base-05)}.theme-card__title{margin:0 10px;font-weight:600}.theme-card__description{font-size:12px;margin:0 10px;text-overflow:ellipsis;-webkit-line-clamp:4;-webkit-box-orient:vertical;overflow:hidden;display:-webkit-box}.theme-card__show-detail-btn{position:absolute;bottom:-15px;right:80px}.theme-card__choose-btn{position:absolute;bottom:-15px;right:10px}\n"] }]
    }], function () { return [{ type: i1.ThemeApiService }, { type: i5.ModalService, decorators: [{
                type: Inject,
                args: [ModalService]
            }] }, { type: undefined, decorators: [{
                type: Inject,
                args: [DOMAIN_URL]
            }] }]; }, { cancel: [{
            type: Output
        }], themeSelect: [{
            type: Output
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtaW1wb3J0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy90aGVtZS1pbXBvcnQvdGhlbWUtaW1wb3J0LmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy90aGVtZS1pbXBvcnQvdGhlbWUtaW1wb3J0LmNvbXBvbmVudC5odG1sIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFDTCx1QkFBdUIsRUFDdkIsU0FBUyxFQUNULFlBQVksRUFDWixNQUFNLEVBQ04sTUFBTSxFQUNQLE1BQU0sZUFBZSxDQUFDO0FBR3ZCLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNqRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDakQsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQ3hELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQzs7Ozs7Ozs7O0lDSzVDLCtCQUNzQyxjQUFBLGNBQUE7SUFHaEMsMEJBQTZCO0lBQy9CLGlCQUFNO0lBQ04sK0JBQWdDO0lBQUMsWUFBdUM7SUFBQSxpQkFBTTtJQUM5RSwrQkFBK0I7SUFBQyxZQUFnQjtJQUFBLGlCQUFNO0lBQ3RELCtCQUFxQztJQUFDLFlBQWtCO0lBQUEsaUJBQU07SUFDOUQsbUNBRWlCO0lBQUEscUJBQUk7SUFBQSxpQkFBUztJQUM5QixtQ0FJaUI7SUFGVCxtUUFBUyxlQUFBLDJCQUFpQixDQUFBLElBQUM7SUFFbEIsdUJBQU07SUFBQSxpQkFBUyxFQUFBLEVBQUE7Ozs7SUFaekIsZUFBdUI7SUFBdkIsbUVB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QUNuQyxJQUFJLFdBQ1AsQ0FBQyxXQUFXLENBQUM7O3NCQVluQixNQUFNO3VCQUFDLFlBQVk7O3NCQUNuQixNQUFNO3VCQUFDLFVBQVU7d0JBTkgsTUFBTTtrQkFBdEIsTUFBTTtZQUNVLFdBQVc7a0JBQTNCLE1BQU0iLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xyXG4gIENoYW5nZURldGVjdGlvblN0cmF0ZWd5LFxyXG4gIENvbXBvbmVudCxcclxuICBFdmVudEVtaXR0ZXIsXHJcbiAgSW5qZWN0LFxyXG4gIE91dHB1dFxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBUaGVtZU1vZGVsIH0gZnJvbSAnQG1peC1zcGEvbWl4LmxpYic7XHJcblxyXG5pbXBvcnQgeyBUaGVtZUFwaVNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcyc7XHJcbmltcG9ydCB7IFNoYXJlTW9kdWxlIH0gZnJvbSAnLi4vLi4vc2hhcmUubW9kdWxlJztcclxuaW1wb3J0IHsgRE9NQUlOX1VSTCB9IGZyb20gJy4uLy4uL3Rva2VuL2Jhc2UtdXJsLnRva2VuJztcclxuaW1wb3J0IHsgTW9kYWxTZXJ2aWNlIH0gZnJvbSAnLi4vbW9kYWwvbW9kYWwuc2VydmljZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC10aGVtZS1pbXBvcnQnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi90aGVtZS1pbXBvcnQuY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL3RoZW1lLWltcG9ydC5jb21wb25lbnQuc2NzcyddLFxyXG4gIGNoYW5nZURldGVjdGlvbjogQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3kuT25QdXNoLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW1NoYXJlTW9kdWxlXVxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGhlbWVJbXBvcnRDb21wb25lbnQge1xyXG4gIHB1YmxpYyBhY3RpdmVJdGVtSW5kZXggPSAwO1xyXG4gIHB1YmxpYyB0aGVtZUxpc3RWbSQgPSB0aGlzLnRoZW1lQXBpU2VydmljZS5nZXRUaGVtZVN0b3JlKCk7XHJcbiAgcHVibGljIGN1cnJlbnRTZWxlY3RlZFRoZW1lOiBUaGVtZU1vZGVsIHwgbnVsbCA9IG51bGw7XHJcblxyXG4gIEBPdXRwdXQoKSBwdWJsaWMgY2FuY2VsOiBFdmVudEVtaXR0ZXI8dm9pZD4gPSBuZXcgRXZlbnRFbWl0dGVyKCk7XHJcbiAgQE91dHB1dCgpIHB1YmxpYyB0aGVtZVNlbGVjdDogRXZlbnRFbWl0dGVyPFRoZW1lTW9kZWw+ID0gbmV3IEV2ZW50RW1pdHRlcigpO1xyXG5cclxuICBjb25zdHJ1Y3RvcihcclxuICAgIHB1YmxpYyB0aGVtZUFwaVNlcnZpY2U6IFRoZW1lQXBpU2VydmljZSxcclxuICAgIEBJbmplY3QoTW9kYWxTZXJ2aWNlKSBwcml2YXRlIHJlYWRvbmx5IG1vZGFsU2VydmljZTogTW9kYWxTZXJ2aWNlLFxyXG4gICAgQEluamVjdChET01BSU5fVVJMKSBwdWJsaWMgZG9tYWluOiBzdHJpbmdcclxuICApIHt9XHJcblxyXG4gIHB1YmxpYyBzZWxlY3RUaGVtZSh2YWx1ZTogVGhlbWVNb2RlbCk6IHZvaWQge1xyXG4gICAgdGhpcy5jdXJyZW50U2VsZWN0ZWRUaGVtZSA9IHZhbHVlO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIG9uQ2FuY2VsQ2xpY2soKTogdm9pZCB7XHJcbiAgICBpZiAodGhpcy5jdXJyZW50U2VsZWN0ZWRUaGVtZSkge1xyXG4gICAgICB0aGlzLm1vZGFsU2VydmljZS5jb25maXJtKCdEbyB5b3Ugd2FudCB0byBjYW5jZWw/Jykuc3Vic2NyaWJlKG9rID0+IHtcclxuICAgICAgICBpZiAob2spIHRoaXMuY2FuY2VsLmVtaXQoKTtcclxuICAgICAgfSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICB0aGlzLmNhbmNlbC5lbWl0KCk7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgb25Vc2VUaGVtZUNsaWNrKCk6IHZvaWQge1xyXG4gICAgaWYgKCF0aGlzLmN1cnJlbnRTZWxlY3RlZFRoZW1lKSByZXR1cm47XHJcblxyXG4gICAgdGhpcy50aGVtZVNlbGVjdC5lbWl0KHRoaXMuY3VycmVudFNlbGVjdGVkVGhlbWUpO1xyXG4gICAgdGhpcy5jYW5jZWwuZW1pdCgpO1xyXG4gIH1cclxufVxyXG4iLCI8ZGl2IGNsYXNzPVwidGhlbWUtaW1wb3J0XCI+XHJcbiAgPHR1aS10YWJzIFsoYWN0aXZlSXRlbUluZGV4KV09XCJhY3RpdmVJdGVtSW5kZXhcIj5cclxuICAgIDxidXR0b24gKGNsaWNrKT1cImFjdGl2ZUl0ZW1JbmRleCA9IDBcIlxyXG4gICAgICAgICAgICB0dWlUYWI+XHJcbiAgICAgIEZyb20gTWl4IFN0b3JlXHJcbiAgICA8L2J1dHRvbj5cclxuICAgIDxidXR0b24gKGNsaWNrKT1cImFjdGl2ZUl0ZW1JbmRleCA9IDFcIlxyXG4gICAgICAgICAgICB0dWlUYWI+XHJcbiAgICAgIFVwbG9hZFxyXG4gICAgPC9idXR0b24+XHJcbiAgPC90dWktdGFicz5cclxuXHJcbiAgPGRpdiAqbmdJZj1cInRoZW1lTGlzdFZtJCB8IGFzeW5jIGFzIHJlc3VsdFwiXHJcbiAgICAgICBjbGFzcz1cInRoZW1lLWltcG9ydF9fY29udGFpbmVyXCI+XHJcbiAgICA8bmctY29udGFpbmVyICpuZ0lmPVwiYWN0aXZlSXRlbUluZGV4ID09PSAwXCI+XHJcbiAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCJyZXN1bHQuaXRlbXMubGVuZ3RoOyBlbHNlIG5vVGhlbWVcIj5cclxuICAgICAgICA8ZGl2IGNsYXNzPVwicm93XCI+XHJcbiAgICAgICAgICA8ZGl2ICpuZ0Zvcj1cImxldCBpdGVtIG9mIHJlc3VsdC5pdGVtc1wiXHJcbiAgICAgICAgICAgICAgIGNsYXNzPVwiY29sLTEyIGNvbC1zbS02IGNvbC1tZC00XCI+XHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJ0aGVtZS1jYXJkXCI+XHJcbiAgICAgICAgICAgICAgPGRpdiBjbGFzcz1cInRoZW1lLWNhcmRfX3RodW1ibmFpbFwiPlxyXG4gICAgICAgICAgICAgICAgPGltZyBzcmM9XCJ7e2l0ZW0uaW1hZ2VVcmx9fVwiPlxyXG4gICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJ0aGVtZS1jYXJkX19hdXRob3JcIj4ge3sgaXRlbS5jcmVhdGVkQnkgfHwgJ0FkbWluaXN0cmF0b3InfX0gPC9kaXY+XHJcbiAgICAgICAgICAgICAgPGRpdiBjbGFzcz1cInRoZW1lLWNhcmRfX3RpdGxlXCI+IHt7IGl0ZW0udGl0bGV9fSA8L2Rpdj5cclxuICAgICAgICAgICAgICA8ZGl2IGNsYXNzPVwidGhlbWUtY2FyZF9fZGVzY3JpcHRpb25cIj4ge3sgaXRlbS5leGNlcnB0IH19PC9kaXY+XHJcbiAgICAgICAgICAgICAgPGJ1dHRvbiBjbGFzcz1cInRoZW1lLWNhcmRfX3Nob3ctZGV0YWlsLWJ0blwiXHJcbiAgICAgICAgICAgICAgICAgICAgICB0dWlCdXR0b25cclxuICAgICAgICAgICAgICAgICAgICAgIHNpemU9XCJzXCI+VmlldzwvYnV0dG9uPlxyXG4gICAgICAgICAgICAgIDxidXR0b24gY2xhc3M9XCJ0aGVtZS1jYXJkX19jaG9vc2UtYnRuXCJcclxuICAgICAgICAgICAgICAgICAgICAgIFtkaXNhYmxlZF09XCIhIWN1cnJlbnRTZWxlY3RlZFRoZW1lICYmIGN1cnJlbnRTZWxlY3RlZFRoZW1lLmlkID09PSBpdGVtLmlkXCJcclxuICAgICAgICAgICAgICAgICAgICAgIChjbGljayk9XCJzZWxlY3RUaGVtZShpdGVtKVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICB0dWlCdXR0b25cclxuICAgICAgICAgICAgICAgICAgICAgIHNpemU9XCJzXCI+U2VsZWN0PC9idXR0b24+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgPC9kaXY+XHJcbiAgICAgIDwvbmctY29udGFpbmVyPlxyXG5cclxuICAgICAgPG5nLXRlbXBsYXRlICNub1RoZW1lPiBUaGVyZSBpcyBubyB0aGVtZSBpbiBNaXhDb3JlIFN0b3JlIG5vdy48L25nLXRlbXBsYXRlPlxyXG4gICAgPC9uZy1jb250YWluZXI+XHJcblxyXG4gICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cImFjdGl2ZUl0ZW1JbmRleCA9PT0gMVwiPlxyXG4gICAgICBVcGxvYWRcclxuICAgIDwvbmctY29udGFpbmVyPlxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2IGNsYXNzPVwidGhlbWUtaW1wb3J0X19mb290ZXJcIj5cclxuICAgIDxidXR0b24gKGNsaWNrKT1cIm9uQ2FuY2VsQ2xpY2soKVwiXHJcbiAgICAgICAgICAgIHR1aUJ1dHRvblxyXG4gICAgICAgICAgICBhcHBlYXJhbmNlPVwic2Vjb25kYXJ5XCI+Q2FuY2VsPC9idXR0b24+XHJcblxyXG4gICAgPGJ1dHRvbiBbZGlzYWJsZWRdPVwiIWN1cnJlbnRTZWxlY3RlZFRoZW1lXCJcclxuICAgICAgICAgICAgKGNsaWNrKT1cIm9uVXNlVGhlbWVDbGljaygpXCJcclxuICAgICAgICAgICAgdHVpQnV0dG9uPnt7ICdVc2UgJysgY3VycmVudFNlbGVjdGVkVGhlbWU/LnRpdGxlIH19PC9idXR0b24+XHJcbiAgPC9kaXY+XHJcbjwvZGl2PlxyXG4iXX0=