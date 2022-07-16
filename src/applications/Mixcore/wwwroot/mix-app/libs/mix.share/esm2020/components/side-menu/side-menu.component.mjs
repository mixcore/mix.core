import { animate, style, transition, trigger } from '@angular/animations';
import { Component, Input } from '@angular/core';
import { VerticalDisplayPosition } from '@mix-spa/mix.lib';
import { ShareModule } from '../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common";
import * as i2 from "@taiga-ui/core";
import * as i3 from "angular-tabler-icons";
function SideMenuComponent_div_0_ng_container_4_ng_template_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div");
    i0.ɵɵtext(1);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const item_r4 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", item_r4.title, " ");
} }
const _c0 = function (a0, a1) { return { "--active": a0, "mt-auto": a1 }; };
function SideMenuComponent_div_0_ng_container_4_Template(rf, ctx) { if (rf & 1) {
    const _r9 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "div", 8);
    i0.ɵɵlistener("click", function SideMenuComponent_div_0_ng_container_4_Template_div_click_1_listener() { const restoredCtx = i0.ɵɵrestoreView(_r9); const item_r4 = restoredCtx.$implicit; const ctx_r8 = i0.ɵɵnextContext(2); return i0.ɵɵresetView(ctx_r8.itemSelect(item_r4)); });
    i0.ɵɵelement(2, "i-tabler", 9);
    i0.ɵɵtemplate(3, SideMenuComponent_div_0_ng_container_4_ng_template_3_Template, 2, 1, "ng-template", null, 10, i0.ɵɵtemplateRefExtractor);
    i0.ɵɵelementEnd();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const item_r4 = ctx.$implicit;
    const _r5 = i0.ɵɵreference(4);
    const ctx_r1 = i0.ɵɵnextContext(2);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction2(6, _c0, (ctx_r1.currentSelectedItem == null ? null : ctx_r1.currentSelectedItem.id) === (item_r4 == null ? null : item_r4.id), item_r4.position === ctx_r1.VerticalDisplayPosition.Bottom))("tuiHint", _r5)("tuiHintHideDelay", 0)("tuiHintMode", "onDark")("tuiHintShowDelay", 200);
    i0.ɵɵadvance(1);
    i0.ɵɵpropertyInterpolate("name", item_r4.icon);
} }
function SideMenuComponent_div_0_div_5_ng_container_4_Template(rf, ctx) { if (rf & 1) {
    const _r13 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "div", 14);
    i0.ɵɵlistener("click", function SideMenuComponent_div_0_div_5_ng_container_4_Template_div_click_1_listener() { const restoredCtx = i0.ɵɵrestoreView(_r13); const menu_r11 = restoredCtx.$implicit; const ctx_r12 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r12.itemClick(menu_r11)); });
    i0.ɵɵelement(2, "i-tabler", 9);
    i0.ɵɵelementStart(3, "div");
    i0.ɵɵtext(4);
    i0.ɵɵelementEnd()();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const menu_r11 = ctx.$implicit;
    i0.ɵɵadvance(2);
    i0.ɵɵpropertyInterpolate("name", menu_r11.icon);
    i0.ɵɵadvance(2);
    i0.ɵɵtextInterpolate1(" ", menu_r11.title, " ");
} }
function SideMenuComponent_div_0_div_5_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 11)(1, "div", 12);
    i0.ɵɵtext(2);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(3, "div", 13);
    i0.ɵɵtemplate(4, SideMenuComponent_div_0_div_5_ng_container_4_Template, 5, 2, "ng-container", 5);
    i0.ɵɵelementEnd()();
} if (rf & 2) {
    const ctx_r2 = i0.ɵɵnextContext(2);
    i0.ɵɵproperty("@enterAnimation", undefined);
    i0.ɵɵadvance(2);
    i0.ɵɵtextInterpolate1(" ", ctx_r2.currentSelectedItem.title, " ");
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("ngForOf", ctx_r2.currentSelectedItem.detail);
} }
function SideMenuComponent_div_0_div_6_Template(rf, ctx) { if (rf & 1) {
    const _r15 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "div", 15);
    i0.ɵɵlistener("click", function SideMenuComponent_div_0_div_6_Template_div_click_0_listener() { i0.ɵɵrestoreView(_r15); const ctx_r14 = i0.ɵɵnextContext(2); return i0.ɵɵresetView(ctx_r14.currentSelectedItem = undefined); });
    i0.ɵɵelement(1, "i-tabler", 16);
    i0.ɵɵelementEnd();
} }
function SideMenuComponent_div_0_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 1)(1, "div", 2)(2, "div", 3);
    i0.ɵɵelement(3, "div", 4);
    i0.ɵɵelementEnd();
    i0.ɵɵtemplate(4, SideMenuComponent_div_0_ng_container_4_Template, 5, 9, "ng-container", 5);
    i0.ɵɵelementEnd();
    i0.ɵɵtemplate(5, SideMenuComponent_div_0_div_5_Template, 5, 3, "div", 6);
    i0.ɵɵtemplate(6, SideMenuComponent_div_0_div_6_Template, 2, 0, "div", 7);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const ctx_r0 = i0.ɵɵnextContext();
    i0.ɵɵadvance(4);
    i0.ɵɵproperty("ngForOf", ctx_r0.menuItems);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", ctx_r0.currentSelectedItem);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", ctx_r0.currentSelectedItem);
} }
export class SideMenuComponent {
    constructor() {
        this.showMenuLevel2 = false;
        this.menuItems = [];
        this.VerticalDisplayPosition = VerticalDisplayPosition;
    }
    ngOnInit() {
        this.currentSelectedItem = this.menuItems[1];
    }
    itemSelect(item) {
        if (item.action)
            item.action();
        if (item.hideDetail)
            return;
        this.currentSelectedItem = item;
    }
    itemClick(item) {
        if (item.action) {
            return item.action();
        }
    }
}
SideMenuComponent.ɵfac = function SideMenuComponent_Factory(t) { return new (t || SideMenuComponent)(); };
SideMenuComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: SideMenuComponent, selectors: [["mix-side-menu"]], inputs: { showMenuLevel2: "showMenuLevel2", menuItems: "menuItems" }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 1, vars: 1, consts: [["class", "side-menu", 4, "ngIf"], [1, "side-menu"], [1, "side-menu__level-1"], [1, "side-menu__level-1-logo"], [1, "login-form-logo-square"], [4, "ngFor", "ngForOf"], ["class", "side-menu__level-2", 4, "ngIf"], ["class", "side-menu__collapse-icon", 3, "click", 4, "ngIf"], ["tuiHintDirection", "right", 1, "side-menu__level-1-icon", 3, "ngClass", "tuiHint", "tuiHintHideDelay", "tuiHintMode", "tuiHintShowDelay", "click"], [3, "name"], ["tooltip", ""], [1, "side-menu__level-2"], [1, "side-menu__level-2-title"], [1, "side-menu__level-2-content"], [1, "side-menu__detail-item", 3, "click"], [1, "side-menu__collapse-icon", 3, "click"], ["name", "minus"]], template: function SideMenuComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵtemplate(0, SideMenuComponent_div_0_Template, 7, 3, "div", 0);
    } if (rf & 2) {
        i0.ɵɵproperty("ngIf", ctx.menuItems);
    } }, dependencies: [ShareModule, i1.NgClass, i1.NgForOf, i1.NgIf, i2.TuiHintDirective, i3.TablerIconComponent], styles: [".side-menu[_ngcontent-%COMP%]{height:100%;box-sizing:border-box;display:flex;position:relative}.side-menu__level-1[_ngcontent-%COMP%]{height:100%;padding:5px;border-right:1px solid var(--tui-base-04);display:flex;align-items:center;flex-direction:column}.side-menu__level-1-logo[_ngcontent-%COMP%]{margin-bottom:15px}.side-menu__level-1-logo[_ngcontent-%COMP%]   .login-form-logo-square[_ngcontent-%COMP%]{width:40px;height:35px;background-color:var(--tui-primary);-webkit-mask:url(/assets/images/mixcore-logo-red-square.svg) no-repeat 50% 50%;mask:url(/assets/images/mixcore-logo-red-square.svg) no-repeat 50% 50%}.side-menu__level-1-icon[_ngcontent-%COMP%]{margin-bottom:5px;width:40px;height:40px;display:flex;justify-content:center;align-items:center;cursor:pointer;border-radius:var(--tui-radius-l)}.side-menu__level-1-icon[_ngcontent-%COMP%]:hover{background-color:var(--tui-primary-hover)}.side-menu__level-1-icon.--active[_ngcontent-%COMP%]{background-color:var(--tui-primary-active);color:var(--tui-text-01-night)}.side-menu__level-2[_ngcontent-%COMP%]{height:100%;width:255px}.side-menu__level-2-title[_ngcontent-%COMP%]{height:var(--mix-header-height);border-bottom:1px solid var(--tui-base-04);font-size:16px;font-weight:500;padding:0 0 0 32px;display:flex;align-items:center}.side-menu__level-2-content[_ngcontent-%COMP%]{padding:24px 24px 5px}.side-menu__detail-item[_ngcontent-%COMP%]{cursor:pointer;border-radius:8px;padding:8px;width:100%;display:flex;align-items:center;font-size:14px;transition:background .8s}.side-menu__detail-item[_ngcontent-%COMP%] > i-tabler[_ngcontent-%COMP%]{margin-right:15px;width:18px;height:18px}.side-menu__detail-item[_ngcontent-%COMP%]:hover{background-color:var(--tui-primary-hover)}.side-menu__detail-item[_ngcontent-%COMP%]:active{background-color:#e8f0f7;background-size:100%;transition:background 0s}.side-menu__collapse-icon[_ngcontent-%COMP%]{position:absolute;width:40px;height:40px;top:3px;right:8px;display:flex;justify-content:center;align-items:center;cursor:pointer;border-radius:12px}.side-menu[_ngcontent-%COMP%]   .separator[_ngcontent-%COMP%]{content:\"\";display:block;height:1px;width:100%;background:var(--tui-base-04);margin:15px 0}"], data: { animation: [
            trigger('enterAnimation', [
                transition(':enter', [style({ width: 0, opacity: 0 }), animate('110ms', style({ width: '200px', opacity: 1 }))]),
                transition(':leave', [style({ width: '200px', opacity: 1 }), animate('110ms', style({ width: 0, opacity: 0 }))])
            ])
        ] } });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(SideMenuComponent, [{
        type: Component,
        args: [{ selector: 'mix-side-menu', standalone: true, imports: [ShareModule], animations: [
                    trigger('enterAnimation', [
                        transition(':enter', [style({ width: 0, opacity: 0 }), animate('110ms', style({ width: '200px', opacity: 1 }))]),
                        transition(':leave', [style({ width: '200px', opacity: 1 }), animate('110ms', style({ width: 0, opacity: 0 }))])
                    ])
                ], template: "<div *ngIf=\"menuItems\" class=\"side-menu\">\r\n  <div class=\"side-menu__level-1\">\r\n    <div class=\"side-menu__level-1-logo\">\r\n      <div class=\"login-form-logo-square\"></div>\r\n    </div>\r\n\r\n    <ng-container *ngFor=\"let item of menuItems\">\r\n      <div class=\"side-menu__level-1-icon\"\r\n        [ngClass]=\"{'--active': currentSelectedItem?.id === item?.id, 'mt-auto': item.position === VerticalDisplayPosition.Bottom}\"\r\n        [tuiHint]=\"tooltip\" [tuiHintHideDelay]=\"0\" [tuiHintMode]=\"'onDark'\" [tuiHintShowDelay]=\"200\"\r\n        (click)=\"itemSelect(item)\" tuiHintDirection=\"right\">\r\n        <i-tabler name=\"{{item.icon}}\"></i-tabler>\r\n\r\n        <ng-template #tooltip>\r\n          <div>\r\n            {{ item.title }}\r\n          </div>\r\n        </ng-template>\r\n      </div>\r\n    </ng-container>\r\n  </div>\r\n\r\n  <div *ngIf=\"currentSelectedItem\" class=\"side-menu__level-2\" [@enterAnimation]>\r\n    <div class=\"side-menu__level-2-title\"> {{ currentSelectedItem.title }} </div>\r\n\r\n    <div class=\"side-menu__level-2-content\">\r\n      <ng-container *ngFor=\"let menu of currentSelectedItem.detail\">\r\n        <div class=\"side-menu__detail-item\" (click)=\"itemClick(menu)\">\r\n          <i-tabler name=\"{{menu.icon}}\"></i-tabler>\r\n          <div>\r\n            {{ menu.title }}\r\n          </div>\r\n        </div>\r\n      </ng-container>\r\n\r\n      <!-- <div class=\"separator\"></div> -->\r\n    </div>\r\n  </div>\r\n\r\n  <div *ngIf=\"currentSelectedItem\" class=\"side-menu__collapse-icon\" (click)=\"currentSelectedItem = undefined\">\r\n    <i-tabler name=\"minus\"></i-tabler>\r\n  </div>\r\n</div>\r\n", styles: [".side-menu{height:100%;box-sizing:border-box;display:flex;position:relative}.side-menu__level-1{height:100%;padding:5px;border-right:1px solid var(--tui-base-04);display:flex;align-items:center;flex-direction:column}.side-menu__level-1-logo{margin-bottom:15px}.side-menu__level-1-logo .login-form-logo-square{width:40px;height:35px;background-color:var(--tui-primary);-webkit-mask:url(/assets/images/mixcore-logo-red-square.svg) no-repeat 50% 50%;mask:url(/assets/images/mixcore-logo-red-square.svg) no-repeat 50% 50%}.side-menu__level-1-icon{margin-bottom:5px;width:40px;height:40px;display:flex;justify-content:center;align-items:center;cursor:pointer;border-radius:var(--tui-radius-l)}.side-menu__level-1-icon:hover{background-color:var(--tui-primary-hover)}.side-menu__level-1-icon.--active{background-color:var(--tui-primary-active);color:var(--tui-text-01-night)}.side-menu__level-2{height:100%;width:255px}.side-menu__level-2-title{height:var(--mix-header-height);border-bottom:1px solid var(--tui-base-04);font-size:16px;font-weight:500;padding:0 0 0 32px;display:flex;align-items:center}.side-menu__level-2-content{padding:24px 24px 5px}.side-menu__detail-item{cursor:pointer;border-radius:8px;padding:8px;width:100%;display:flex;align-items:center;font-size:14px;transition:background .8s}.side-menu__detail-item>i-tabler{margin-right:15px;width:18px;height:18px}.side-menu__detail-item:hover{background-color:var(--tui-primary-hover)}.side-menu__detail-item:active{background-color:#e8f0f7;background-size:100%;transition:background 0s}.side-menu__collapse-icon{position:absolute;width:40px;height:40px;top:3px;right:8px;display:flex;justify-content:center;align-items:center;cursor:pointer;border-radius:12px}.side-menu .separator{content:\"\";display:block;height:1px;width:100%;background:var(--tui-base-04);margin:15px 0}\n"] }]
    }], null, { showMenuLevel2: [{
            type: Input
        }], menuItems: [{
            type: Input
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2lkZS1tZW51LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9zaWRlLW1lbnUvc2lkZS1tZW51LmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9zaWRlLW1lbnUvc2lkZS1tZW51LmNvbXBvbmVudC5odG1sIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxPQUFPLEVBQUUsS0FBSyxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUMxRSxPQUFPLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUN6RCxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSxrQkFBa0IsQ0FBQztBQUUzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sb0JBQW9CLENBQUM7Ozs7OztJQ1V2QywyQkFBSztJQUNILFlBQ0Y7SUFBQSxpQkFBTTs7O0lBREosZUFDRjtJQURFLDhDQUNGOzs7OztJQVZOLDZCQUE2QztJQUMzQyw4QkFHc0Q7SUFBcEQsc09BQVMsZUFBQSwwQkFBZ0IsQ0FBQSxJQUFDO0lBQzFCLDhCQUEwQztJQUUxQyx5SUFJYztJQUNoQixpQkFBTTtJQUNSLDBCQUFlOzs7OztJQVhYLGVBQTJIO0lBQTNILHVPQUEySCxnQkFBQSx1QkFBQSx5QkFBQSx5QkFBQTtJQUdqSCxlQUFvQjtJQUFwQiw4Q0FBb0I7Ozs7SUFlaEMsNkJBQThEO0lBQzVELCtCQUE4RDtJQUExQiwrT0FBUyxlQUFBLDJCQUFlLENBQUEsSUFBQztJQUMzRCw4QkFBMEM7SUFDMUMsMkJBQUs7SUFDSCxZQUNGO0lBQUEsaUJBQU0s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QUMsRUFBRSxDQUFDLEVBQUUsT0FBTyxDQUFDLE9BQU8sRUFBRSxLQUFLLENBQUMsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLE9BQU8sRUFBRSxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQztxQkFDakgsQ0FBQztpQkFDSDtnQkFHZSxjQUFjO2tCQUE3QixLQUFLO1lBQ1UsU0FBUztrQkFBeEIsS0FBSyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGFuaW1hdGUsIHN0eWxlLCB0cmFuc2l0aW9uLCB0cmlnZ2VyIH0gZnJvbSAnQGFuZ3VsYXIvYW5pbWF0aW9ucyc7XHJcbmltcG9ydCB7IENvbXBvbmVudCwgSW5wdXQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBWZXJ0aWNhbERpc3BsYXlQb3NpdGlvbiB9IGZyb20gJ0BtaXgtc3BhL21peC5saWInO1xyXG5cclxuaW1wb3J0IHsgU2hhcmVNb2R1bGUgfSBmcm9tICcuLi8uLi9zaGFyZS5tb2R1bGUnO1xyXG5cclxuZXhwb3J0IGludGVyZmFjZSBNaXhUb29sYmFyTWVudSB7XHJcbiAgaWQ6IG51bWJlcjtcclxuICB0aXRsZTogc3RyaW5nO1xyXG4gIGljb246IHN0cmluZztcclxuICBoaWRlRGV0YWlsPzogYm9vbGVhbjtcclxuICBhY3Rpb24/OiAoKSA9PiB2b2lkO1xyXG4gIGRldGFpbDogTWVudUl0ZW1bXTtcclxuICBwb3NpdGlvbjogVmVydGljYWxEaXNwbGF5UG9zaXRpb247XHJcbn1cclxuXHJcbmV4cG9ydCBpbnRlcmZhY2UgTWVudUl0ZW0ge1xyXG4gIGljb246IHN0cmluZztcclxuICB0aXRsZTogc3RyaW5nO1xyXG4gIHJvdXRlPzogc3RyaW5nIHwgc3RyaW5nW107XHJcbiAgYWN0aW9uPzogKCkgPT4gdm9pZDtcclxufVxyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtc2lkZS1tZW51JyxcclxuICB0ZW1wbGF0ZVVybDogJy4vc2lkZS1tZW51LmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi9zaWRlLW1lbnUuY29tcG9uZW50LnNjc3MnXSxcclxuICBzdGFuZGFsb25lOiB0cnVlLFxyXG4gIGltcG9ydHM6IFtTaGFyZU1vZHVsZV0sXHJcbiAgYW5pbWF0aW9uczogW1xyXG4gICAgdHJpZ2dlcignZW50ZXJBbmltYXRpb24nLCBbXHJcbiAgICAgIHRyYW5zaXRpb24oJzplbnRlcicsIFtzdHlsZSh7IHdpZHRoOiAwLCBvcGFjaXR5OiAwIH0pLCBhbmltYXRlKCcxMTBtcycsIHN0eWxlKHsgd2lkdGg6ICcyMDBweCcsIG9wYWNpdHk6IDEgfSkpXSksXHJcbiAgICAgIHRyYW5zaXRpb24oJzpsZWF2ZScsIFtzdHlsZSh7IHdpZHRoOiAnMjAwcHgnLCBvcGFjaXR5OiAxIH0pLCBhbmltYXRlKCcxMTBtcycsIHN0eWxlKHsgd2lkdGg6IDAsIG9wYWNpdHk6IDAgfSkpXSlcclxuICAgIF0pXHJcbiAgXVxyXG59KVxyXG5leHBvcnQgY2xhc3MgU2lkZU1lbnVDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBzaG93TWVudUxldmVsMiA9IGZhbHNlO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBtZW51SXRlbXM6IE1peFRvb2xiYXJNZW51W10gPSBbXTtcclxuICBwdWJsaWMgY3VycmVudFNlbGVjdGVkSXRlbTogTWl4VG9vbGJhck1lbnUgfCB1bmRlZmluZWQ7XHJcbiAgcHVibGljIHJlYWRvbmx5IFZlcnRpY2FsRGlzcGxheVBvc2l0aW9uID0gVmVydGljYWxEaXNwbGF5UG9zaXRpb247XHJcblxyXG4gIHB1YmxpYyBuZ09uSW5pdCgpOiB2b2lkIHtcclxuICAgIHRoaXMuY3VycmVudFNlbGVjdGVkSXRlbSA9IHRoaXMubWVudUl0ZW1zWzFdO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGl0ZW1TZWxlY3QoaXRlbTogTWl4VG9vbGJhck1lbnUpOiB2b2lkIHtcclxuICAgIGlmIChpdGVtLmFjdGlvbikgaXRlbS5hY3Rpb24oKTtcclxuICAgIGlmIChpdGVtLmhpZGVEZXRhaWwpIHJldHVybjtcclxuXHJcbiAgICB0aGlzLmN1cnJlbnRTZWxlY3RlZEl0ZW0gPSBpdGVtO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGl0ZW1DbGljayhpdGVtOiBNZW51SXRlbSk6IHZvaWQge1xyXG4gICAgaWYgKGl0ZW0uYWN0aW9uKSB7XHJcbiAgICAgIHJldHVybiBpdGVtLmFjdGlvbigpO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iLCI8ZGl2ICpuZ0lmPVwibWVudUl0ZW1zXCIgY2xhc3M9XCJzaWRlLW1lbnVcIj5cclxuICA8ZGl2IGNsYXNzPVwic2lkZS1tZW51X19sZXZlbC0xXCI+XHJcbiAgICA8ZGl2IGNsYXNzPVwic2lkZS1tZW51X19sZXZlbC0xLWxvZ29cIj5cclxuICAgICAgPGRpdiBjbGFzcz1cImxvZ2luLWZvcm0tbG9nby1zcXVhcmVcIj48L2Rpdj5cclxuICAgIDwvZGl2PlxyXG5cclxuICAgIDxuZy1jb250YWluZXIgKm5nRm9yPVwibGV0IGl0ZW0gb2YgbWVudUl0ZW1zXCI+XHJcbiAgICAgIDxkaXYgY2xhc3M9XCJzaWRlLW1lbnVfX2xldmVsLTEtaWNvblwiXHJcbiAgICAgICAgW25nQ2xhc3NdPVwieyctLWFjdGl2ZSc6IGN1cnJlbnRTZWxlY3RlZEl0ZW0/LmlkID09PSBpdGVtPy5pZCwgJ210LWF1dG8nOiBpdGVtLnBvc2l0aW9uID09PSBWZXJ0aWNhbERpc3BsYXlQb3NpdGlvbi5Cb3R0b219XCJcclxuICAgICAgICBbdHVpSGludF09XCJ0b29sdGlwXCIgW3R1aUhpbnRIaWRlRGVsYXldPVwiMFwiIFt0dWlIaW50TW9kZV09XCInb25EYXJrJ1wiIFt0dWlIaW50U2hvd0RlbGF5XT1cIjIwMFwiXHJcbiAgICAgICAgKGNsaWNrKT1cIml0ZW1TZWxlY3QoaXRlbSlcIiB0dWlIaW50RGlyZWN0aW9uPVwicmlnaHRcIj5cclxuICAgICAgICA8aS10YWJsZXIgbmFtZT1cInt7aXRlbS5pY29ufX1cIj48L2ktdGFibGVyPlxyXG5cclxuICAgICAgICA8bmctdGVtcGxhdGUgI3Rvb2x0aXA+XHJcbiAgICAgICAgICA8ZGl2PlxyXG4gICAgICAgICAgICB7eyBpdGVtLnRpdGxlIH19XHJcbiAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICA8L25nLXRlbXBsYXRlPlxyXG4gICAgICA8L2Rpdj5cclxuICAgIDwvbmctY29udGFpbmVyPlxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2ICpuZ0lmPVwiY3VycmVudFNlbGVjdGVkSXRlbVwiIGNsYXNzPVwic2lkZS1tZW51X19sZXZlbC0yXCIgW0BlbnRlckFuaW1hdGlvbl0+XHJcbiAgICA8ZGl2IGNsYXNzPVwic2lkZS1tZW51X19sZXZlbC0yLXRpdGxlXCI+IHt7IGN1cnJlbnRTZWxlY3RlZEl0ZW0udGl0bGUgfX0gPC9kaXY+XHJcblxyXG4gICAgPGRpdiBjbGFzcz1cInNpZGUtbWVudV9fbGV2ZWwtMi1jb250ZW50XCI+XHJcbiAgICAgIDxuZy1jb250YWluZXIgKm5nRm9yPVwibGV0IG1lbnUgb2YgY3VycmVudFNlbGVjdGVkSXRlbS5kZXRhaWxcIj5cclxuICAgICAgICA8ZGl2IGNsYXNzPVwic2lkZS1tZW51X19kZXRhaWwtaXRlbVwiIChjbGljayk9XCJpdGVtQ2xpY2sobWVudSlcIj5cclxuICAgICAgICAgIDxpLXRhYmxlciBuYW1lPVwie3ttZW51Lmljb259fVwiPjwvaS10YWJsZXI+XHJcbiAgICAgICAgICA8ZGl2PlxyXG4gICAgICAgICAgICB7eyBtZW51LnRpdGxlIH19XHJcbiAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICA8L2Rpdj5cclxuICAgICAgPC9uZy1jb250YWluZXI+XHJcblxyXG4gICAgICA8IS0tIDxkaXYgY2xhc3M9XCJzZXBhcmF0b3JcIj48L2Rpdj4gLS0+XHJcbiAgICA8L2Rpdj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiAqbmdJZj1cImN1cnJlbnRTZWxlY3RlZEl0ZW1cIiBjbGFzcz1cInNpZGUtbWVudV9fY29sbGFwc2UtaWNvblwiIChjbGljayk9XCJjdXJyZW50U2VsZWN0ZWRJdGVtID0gdW5kZWZpbmVkXCI+XHJcbiAgICA8aS10YWJsZXIgbmFtZT1cIm1pbnVzXCI+PC9pLXRhYmxlcj5cclxuICA8L2Rpdj5cclxuPC9kaXY+XHJcbiJdfQ==