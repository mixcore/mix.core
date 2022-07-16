import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { EMPTY_ARRAY } from '@taiga-ui/cdk';
import { TUI_TREE_CONTENT } from '@taiga-ui/kit';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { MenuItemComponent } from './menu-item/menu-item';
import { SidebarMenuService } from './sidebar-menu.service';
import * as i0 from "@angular/core";
import * as i1 from "./sidebar-menu.service";
import * as i2 from "@angular/router";
import * as i3 from "@angular/common";
import * as i4 from "@taiga-ui/kit";
import * as i5 from "@taiga-ui/core";
function SidebarMenuComponent_div_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 7);
    i0.ɵɵelement(1, "img", 8);
    i0.ɵɵelementEnd();
} }
function SidebarMenuComponent_tui_tree_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelement(0, "tui-tree", 9);
} if (rf & 2) {
    const item_r4 = ctx.$implicit;
    const ctx_r1 = i0.ɵɵnextContext();
    const _r2 = i0.ɵɵreference(7);
    i0.ɵɵproperty("childrenHandler", ctx_r1.handler)("content", _r2)("map", ctx_r1.treeMap)("tuiTreeController", false)("value", item_r4);
} }
const _c0 = function (a0) { return { "--collapsed": a0 }; };
function SidebarMenuComponent_ng_template_6_tui_svg_0_Template(rf, ctx) { if (rf & 1) {
    const _r10 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tui-svg", 12);
    i0.ɵɵlistener("click", function SidebarMenuComponent_ng_template_6_tui_svg_0_Template_tui_svg_click_0_listener() { i0.ɵɵrestoreView(_r10); const item_r5 = i0.ɵɵnextContext().$implicit; const ctx_r8 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r8.menuItemIconClick(item_r5)); });
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const item_r5 = i0.ɵɵnextContext().$implicit;
    const ctx_r6 = i0.ɵɵnextContext();
    i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(2, _c0, !ctx_r6.isExpanded))("src", item_r5.icon);
} }
function SidebarMenuComponent_ng_template_6_span_1_Template(rf, ctx) { if (rf & 1) {
    const _r14 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "span", 13);
    i0.ɵɵlistener("click", function SidebarMenuComponent_ng_template_6_span_1_Template_span_click_0_listener() { i0.ɵɵrestoreView(_r14); const item_r5 = i0.ɵɵnextContext().$implicit; return i0.ɵɵresetView(item_r5.action && item_r5.action()); });
    i0.ɵɵtext(1);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const item_r5 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", item_r5.text, " ");
} }
function SidebarMenuComponent_ng_template_6_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵtemplate(0, SidebarMenuComponent_ng_template_6_tui_svg_0_Template, 1, 4, "tui-svg", 10);
    i0.ɵɵtemplate(1, SidebarMenuComponent_ng_template_6_span_1_Template, 2, 1, "span", 11);
} if (rf & 2) {
    const item_r5 = ctx.$implicit;
    const ctx_r3 = i0.ɵɵnextContext();
    i0.ɵɵproperty("ngIf", item_r5.icon);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", ctx_r3.isExpanded);
} }
const _c1 = function (a0) { return { "--expanded": a0 }; };
export class SidebarMenuComponent {
    constructor(sidebarService, route) {
        this.sidebarService = sidebarService;
        this.route = route;
        this.isExpanded = true;
        this.treeMap = new Map();
        this.data = {
            text: 'Mix Menu Item',
            children: [
                {
                    text: 'DashBoard',
                    icon: 'tuiIconStructureLarge',
                    action: () => {
                        this.route.navigateByUrl('/portal/dashboard');
                    },
                    children: [
                        {
                            text: 'News',
                            children: [{ text: 'Next level 1' }, { text: 'Next level 2' }, { text: 'Next level 3' }]
                        }
                    ]
                },
                {
                    text: 'Posts',
                    icon: 'tuiIconAddRowLarge',
                    action: () => {
                        this.route.navigateByUrl('/portal/list-post');
                    }
                },
                {
                    text: 'Pages',
                    icon: 'tuiIconFileLarge',
                    children: [{ text: 'Create Page' }, { text: 'List Page' }],
                    action: () => {
                        //
                    }
                }
            ]
        };
        this.handler = item => item.children || EMPTY_ARRAY;
        this.sidebarService.isExpanded$.subscribe(ok => (this.isExpanded = ok));
    }
    get collapseIcon() {
        return 'tuiIconChevronLeftLarge';
    }
    toggleMenu() {
        const isExpanded = this.sidebarService.isExpanded$.getValue();
        this.sidebarService.isExpanded$.next(!isExpanded);
    }
    menuItemIconClick(node) {
        if (this.isExpanded) {
            return;
        }
        this.sidebarService.isExpanded$.next(true);
        this.treeMap.set(node, !this.treeMap.get(node));
    }
}
SidebarMenuComponent.ɵfac = function SidebarMenuComponent_Factory(t) { return new (t || SidebarMenuComponent)(i0.ɵɵdirectiveInject(i1.SidebarMenuService), i0.ɵɵdirectiveInject(i2.Router)); };
SidebarMenuComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: SidebarMenuComponent, selectors: [["mix-sidebar-menu"]], features: [i0.ɵɵProvidersFeature([
            SidebarMenuService,
            {
                provide: TUI_TREE_CONTENT,
                useValue: new PolymorpheusComponent(MenuItemComponent)
            }
        ])], decls: 8, vars: 9, consts: [[1, "sidebar-menu", 3, "ngClass"], ["class", "sidebar-menu__header", 4, "ngIf"], [1, "sidebar-menu__main-navigation"], [3, "childrenHandler", "content", "map", "tuiTreeController", "value", 4, "ngFor", "ngForOf"], [1, "sidebar-menu__collapse-icon", 3, "ngClass", "click"], [3, "src"], ["content", ""], [1, "sidebar-menu__header"], ["src", "assets/images/mixcore-logo-red-2.svg", 1, "sidebar-menu__logo"], [3, "childrenHandler", "content", "map", "tuiTreeController", "value"], ["class", "sidebar-menu__menu-item-icon", 3, "ngClass", "src", "click", 4, "ngIf"], ["class", "sidebar-menu__menu-item-title", 3, "click", 4, "ngIf"], [1, "sidebar-menu__menu-item-icon", 3, "ngClass", "src", "click"], [1, "sidebar-menu__menu-item-title", 3, "click"]], template: function SidebarMenuComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0);
        i0.ɵɵtemplate(1, SidebarMenuComponent_div_1_Template, 2, 0, "div", 1);
        i0.ɵɵelementStart(2, "div", 2);
        i0.ɵɵtemplate(3, SidebarMenuComponent_tui_tree_3_Template, 1, 5, "tui-tree", 3);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(4, "div", 4);
        i0.ɵɵlistener("click", function SidebarMenuComponent_Template_div_click_4_listener() { return ctx.toggleMenu(); });
        i0.ɵɵelement(5, "tui-svg", 5);
        i0.ɵɵelementEnd()();
        i0.ɵɵtemplate(6, SidebarMenuComponent_ng_template_6_Template, 2, 2, "ng-template", null, 6, i0.ɵɵtemplateRefExtractor);
    } if (rf & 2) {
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(5, _c1, ctx.isExpanded));
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.isExpanded);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("ngForOf", ctx.data.children);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(7, _c0, !ctx.isExpanded));
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("src", ctx.collapseIcon);
    } }, dependencies: [i3.NgClass, i3.NgForOf, i3.NgIf, i4.TuiTreeComponent, i4.TuiTreeChildrenDirective, i4.TuiTreeControllerDirective, i5.TuiSvgComponent], styles: [".sidebar-menu[_ngcontent-%COMP%]{height:100%;width:60px;background-color:#141a25;color:#f5f4f3;position:relative;padding:10px;box-sizing:border-box}.sidebar-menu.--expanded[_ngcontent-%COMP%]{width:250px}.sidebar-menu__header[_ngcontent-%COMP%]{padding:10px 5px;height:50px;margin-bottom:20px}.sidebar-menu__logo[_ngcontent-%COMP%]{width:70%;height:auto}.sidebar-menu__menu-item-icon[_ngcontent-%COMP%]{margin-right:10px;opacity:.7;transition:all .2s ease-in-out}.sidebar-menu__menu-item-icon.--collapsed[_ngcontent-%COMP%]{cursor:pointer;opacity:1}.sidebar-menu__menu-item-icon.--collapsed[_ngcontent-%COMP%]:hover{opacity:.7}.sidebar-menu__menu-item-title[_ngcontent-%COMP%]{transition:all .2s ease-in-out;cursor:pointer}.sidebar-menu__menu-item-title.--collapsed[_ngcontent-%COMP%]{opacity:0}.sidebar-menu__collapse-icon[_ngcontent-%COMP%]{cursor:pointer;opacity:.8;border-radius:50%;background-color:#141a25;border:.5px solid #fff;color:fff;width:30px;height:30px;display:flex;justify-content:center;align-items:center;position:absolute;right:-15px;top:calc(50% - 15px);transition:all .5s ease-in-out}.sidebar-menu__collapse-icon[_ngcontent-%COMP%]:hover{opacity:1}.sidebar-menu__collapse-icon.--collapsed[_ngcontent-%COMP%]{transform:rotate(-180deg)}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(SidebarMenuComponent, [{
        type: Component,
        args: [{ selector: 'mix-sidebar-menu', providers: [
                    SidebarMenuService,
                    {
                        provide: TUI_TREE_CONTENT,
                        useValue: new PolymorpheusComponent(MenuItemComponent)
                    }
                ], template: "<div class=\"sidebar-menu\"\r\n     [ngClass]=\"{'--expanded': isExpanded}\">\r\n  <div *ngIf=\"isExpanded\"\r\n       class=\"sidebar-menu__header\">\r\n    <img class=\"sidebar-menu__logo\"\r\n         src=\"assets/images/mixcore-logo-red-2.svg\">\r\n  </div>\r\n\r\n  <div class=\"sidebar-menu__main-navigation\">\r\n    <tui-tree *ngFor=\"let item of data.children\"\r\n              [childrenHandler]=\"handler\"\r\n              [content]=\"content\"\r\n              [map]=\"treeMap\"\r\n              [tuiTreeController]=\"false\"\r\n              [value]=\"item\"></tui-tree>\r\n  </div>\r\n\r\n  <div class=\"sidebar-menu__collapse-icon\"\r\n       [ngClass]=\"{'--collapsed': !isExpanded}\"\r\n       (click)=\"toggleMenu()\">\r\n    <tui-svg [src]=\"collapseIcon\"></tui-svg>\r\n  </div>\r\n</div>\r\n\r\n\r\n<ng-template #content\r\n             let-item>\r\n  <tui-svg *ngIf=\"item.icon\"\r\n           class=\"sidebar-menu__menu-item-icon\"\r\n           [ngClass]=\"{'--collapsed': !isExpanded}\"\r\n           [src]=\"item.icon\"\r\n           (click)=\"menuItemIconClick(item)\"></tui-svg>\r\n  <span *ngIf=\"isExpanded\"\r\n        class=\"sidebar-menu__menu-item-title\"\r\n        (click)=\"item.action && item.action()\"> {{item.text}} </span>\r\n</ng-template>\r\n", styles: [".sidebar-menu{height:100%;width:60px;background-color:#141a25;color:#f5f4f3;position:relative;padding:10px;box-sizing:border-box}.sidebar-menu.--expanded{width:250px}.sidebar-menu__header{padding:10px 5px;height:50px;margin-bottom:20px}.sidebar-menu__logo{width:70%;height:auto}.sidebar-menu__menu-item-icon{margin-right:10px;opacity:.7;transition:all .2s ease-in-out}.sidebar-menu__menu-item-icon.--collapsed{cursor:pointer;opacity:1}.sidebar-menu__menu-item-icon.--collapsed:hover{opacity:.7}.sidebar-menu__menu-item-title{transition:all .2s ease-in-out;cursor:pointer}.sidebar-menu__menu-item-title.--collapsed{opacity:0}.sidebar-menu__collapse-icon{cursor:pointer;opacity:.8;border-radius:50%;background-color:#141a25;border:.5px solid #fff;color:fff;width:30px;height:30px;display:flex;justify-content:center;align-items:center;position:absolute;right:-15px;top:calc(50% - 15px);transition:all .5s ease-in-out}.sidebar-menu__collapse-icon:hover{opacity:1}.sidebar-menu__collapse-icon.--collapsed{transform:rotate(-180deg)}\n"] }]
    }], function () { return [{ type: i1.SidebarMenuService }, { type: i2.Router }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2lkZWJhci1tZW51LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9zaWRlYmFyLW1lbnUvc2lkZWJhci1tZW51LmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9zaWRlYmFyLW1lbnUvc2lkZWJhci1tZW51LmNvbXBvbmVudC5odG1sIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDMUMsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pDLE9BQU8sRUFBRSxXQUFXLEVBQWMsTUFBTSxlQUFlLENBQUM7QUFDeEQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ2pELE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLDBCQUEwQixDQUFDO0FBRWpFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHVCQUF1QixDQUFDO0FBQzFELE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7Ozs7OztJQ0wxRCw4QkFDa0M7SUFDaEMseUJBQ2dEO0lBQ2xELGlCQUFNOzs7SUFHSiw4QkFLb0M7Ozs7O0lBSjFCLGdEQUEyQixnQkFBQSx1QkFBQSw0QkFBQSxrQkFBQTs7Ozs7SUFpQnZDLG1DQUkyQztJQUFsQyxtT0FBUyxlQUFBLGlDQUF1QixDQUFBLElBQUM7SUFBQyxpQkFBVTs7OztJQUY1Qyx3RUFBd0MscUJBQUE7Ozs7SUFHakQsZ0NBRTZDO0lBQXZDLDBMQUFTLGlDQUFlLGdCQUFhLENBQUEsSUFBQztJQUFFLFlBQWM7SUFBQSxp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O1FBQ3RDLGlCQUFNO1FBRU4sOEJBRTRCO1FBQXZCLDhGQUFTLGdCQUFZLElBQUM7UUFDekIsNkJBQXdDO1FBQzFDLGlCQUFNLEVBQUE7UUFJUixzSEFVYzs7UUFsQ1Qsb0VBQXNDO1FBQ25DLGVBQWdCO1FBQWhCLHFDQUFnQjtRQU9PLGVBQWdCO1FBQWhCLDJDQUFnQjtRQVN4QyxlQUF3QztRQUF4QyxxRUFBd0M7UUFFbEMsZUFBb0I7UUFBcEIsc0NBQW9COzt1RkRRcEIsb0JBQW9CO2NBWmhDLFNBQVM7MkJBQ0Usa0JBQWtCLGFBR2pCO29CQUNULGtCQUFrQjtvQkFDbEI7d0JBQ0UsT0FBTyxFQUFFLGdCQUFnQjt3QkFDekIsUUFBUSxFQUFFLElBQUkscUJBQXFCLENBQUMsaUJBQWlCLENBQUM7cUJBQ3ZEO2lCQUNGIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IEVNUFRZX0FSUkFZLCBUdWlIYW5kbGVyIH0gZnJvbSAnQHRhaWdhLXVpL2Nkayc7XHJcbmltcG9ydCB7IFRVSV9UUkVFX0NPTlRFTlQgfSBmcm9tICdAdGFpZ2EtdWkva2l0JztcclxuaW1wb3J0IHsgUG9seW1vcnBoZXVzQ29tcG9uZW50IH0gZnJvbSAnQHRpbmtvZmYvbmctcG9seW1vcnBoZXVzJztcclxuXHJcbmltcG9ydCB7IE1lbnVJdGVtQ29tcG9uZW50IH0gZnJvbSAnLi9tZW51LWl0ZW0vbWVudS1pdGVtJztcclxuaW1wb3J0IHsgU2lkZWJhck1lbnVTZXJ2aWNlIH0gZnJvbSAnLi9zaWRlYmFyLW1lbnUuc2VydmljZSc7XHJcblxyXG5pbnRlcmZhY2UgVHJlZU5vZGUge1xyXG4gIHJlYWRvbmx5IHRleHQ6IHN0cmluZztcclxuICByZWFkb25seSBpY29uPzogc3RyaW5nO1xyXG4gIHJlYWRvbmx5IGFjdGlvbj86ICgpID0+IHZvaWQ7XHJcbiAgcmVhZG9ubHkgY2hpbGRyZW4/OiByZWFkb25seSBUcmVlTm9kZVtdO1xyXG59XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC1zaWRlYmFyLW1lbnUnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9zaWRlYmFyLW1lbnUuY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL3NpZGViYXItbWVudS5jb21wb25lbnQuc2NzcyddLFxyXG4gIHByb3ZpZGVyczogW1xyXG4gICAgU2lkZWJhck1lbnVTZXJ2aWNlLFxyXG4gICAge1xyXG4gICAgICBwcm92aWRlOiBUVUlfVFJFRV9DT05URU5ULFxyXG4gICAgICB1c2VWYWx1ZTogbmV3IFBvbHltb3JwaGV1c0NvbXBvbmVudChNZW51SXRlbUNvbXBvbmVudClcclxuICAgIH1cclxuICBdXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTaWRlYmFyTWVudUNvbXBvbmVudCB7XHJcbiAgcHVibGljIGlzRXhwYW5kZWQgPSB0cnVlO1xyXG4gIHB1YmxpYyB0cmVlTWFwID0gbmV3IE1hcDxUcmVlTm9kZSwgYm9vbGVhbj4oKTtcclxuICBwdWJsaWMgZGF0YTogVHJlZU5vZGUgPSB7XHJcbiAgICB0ZXh0OiAnTWl4IE1lbnUgSXRlbScsXHJcbiAgICBjaGlsZHJlbjogW1xyXG4gICAgICB7XHJcbiAgICAgICAgdGV4dDogJ0Rhc2hCb2FyZCcsXHJcbiAgICAgICAgaWNvbjogJ3R1aUljb25TdHJ1Y3R1cmVMYXJnZScsXHJcbiAgICAgICAgYWN0aW9uOiAoKSA9PiB7XHJcbiAgICAgICAgICB0aGlzLnJvdXRlLm5hdmlnYXRlQnlVcmwoJy9wb3J0YWwvZGFzaGJvYXJkJyk7XHJcbiAgICAgICAgfSxcclxuICAgICAgICBjaGlsZHJlbjogW1xyXG4gICAgICAgICAge1xyXG4gICAgICAgICAgICB0ZXh0OiAnTmV3cycsXHJcbiAgICAgICAgICAgIGNoaWxkcmVuOiBbeyB0ZXh0OiAnTmV4dCBsZXZlbCAxJyB9LCB7IHRleHQ6ICdOZXh0IGxldmVsIDInIH0sIHsgdGV4dDogJ05leHQgbGV2ZWwgMycgfV1cclxuICAgICAgICAgIH1cclxuICAgICAgICBdXHJcbiAgICAgIH0sXHJcbiAgICAgIHtcclxuICAgICAgICB0ZXh0OiAnUG9zdHMnLFxyXG4gICAgICAgIGljb246ICd0dWlJY29uQWRkUm93TGFyZ2UnLFxyXG4gICAgICAgIGFjdGlvbjogKCkgPT4ge1xyXG4gICAgICAgICAgdGhpcy5yb3V0ZS5uYXZpZ2F0ZUJ5VXJsKCcvcG9ydGFsL2xpc3QtcG9zdCcpO1xyXG4gICAgICAgIH1cclxuICAgICAgfSxcclxuICAgICAge1xyXG4gICAgICAgIHRleHQ6ICdQYWdlcycsXHJcbiAgICAgICAgaWNvbjogJ3R1aUljb25GaWxlTGFyZ2UnLFxyXG4gICAgICAgIGNoaWxkcmVuOiBbeyB0ZXh0OiAnQ3JlYXRlIFBhZ2UnIH0sIHsgdGV4dDogJ0xpc3QgUGFnZScgfV0sXHJcbiAgICAgICAgYWN0aW9uOiAoKSA9PiB7XHJcbiAgICAgICAgICAvL1xyXG4gICAgICAgIH1cclxuICAgICAgfVxyXG4gICAgXVxyXG4gIH07XHJcblxyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBzaWRlYmFyU2VydmljZTogU2lkZWJhck1lbnVTZXJ2aWNlLCBwdWJsaWMgcm91dGU6IFJvdXRlcikge1xyXG4gICAgdGhpcy5zaWRlYmFyU2VydmljZS5pc0V4cGFuZGVkJC5zdWJzY3JpYmUob2sgPT4gKHRoaXMuaXNFeHBhbmRlZCA9IG9rKSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgZ2V0IGNvbGxhcHNlSWNvbigpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuICd0dWlJY29uQ2hldnJvbkxlZnRMYXJnZSc7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgaGFuZGxlcjogVHVpSGFuZGxlcjxUcmVlTm9kZSwgcmVhZG9ubHkgVHJlZU5vZGVbXT4gPSBpdGVtID0+IGl0ZW0uY2hpbGRyZW4gfHwgRU1QVFlfQVJSQVk7XHJcblxyXG4gIHB1YmxpYyB0b2dnbGVNZW51KCk6IHZvaWQge1xyXG4gICAgY29uc3QgaXNFeHBhbmRlZCA9IHRoaXMuc2lkZWJhclNlcnZpY2UuaXNFeHBhbmRlZCQuZ2V0VmFsdWUoKTtcclxuICAgIHRoaXMuc2lkZWJhclNlcnZpY2UuaXNFeHBhbmRlZCQubmV4dCghaXNFeHBhbmRlZCk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgbWVudUl0ZW1JY29uQ2xpY2sobm9kZTogVHJlZU5vZGUpOiB2b2lkIHtcclxuICAgIGlmICh0aGlzLmlzRXhwYW5kZWQpIHtcclxuICAgICAgcmV0dXJuO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuc2lkZWJhclNlcnZpY2UuaXNFeHBhbmRlZCQubmV4dCh0cnVlKTtcclxuICAgIHRoaXMudHJlZU1hcC5zZXQobm9kZSwgIXRoaXMudHJlZU1hcC5nZXQobm9kZSkpO1xyXG4gIH1cclxufVxyXG4iLCI8ZGl2IGNsYXNzPVwic2lkZWJhci1tZW51XCJcclxuICAgICBbbmdDbGFzc109XCJ7Jy0tZXhwYW5kZWQnOiBpc0V4cGFuZGVkfVwiPlxyXG4gIDxkaXYgKm5nSWY9XCJpc0V4cGFuZGVkXCJcclxuICAgICAgIGNsYXNzPVwic2lkZWJhci1tZW51X19oZWFkZXJcIj5cclxuICAgIDxpbWcgY2xhc3M9XCJzaWRlYmFyLW1lbnVfX2xvZ29cIlxyXG4gICAgICAgICBzcmM9XCJhc3NldHMvaW1hZ2VzL21peGNvcmUtbG9nby1yZWQtMi5zdmdcIj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cInNpZGViYXItbWVudV9fbWFpbi1uYXZpZ2F0aW9uXCI+XHJcbiAgICA8dHVpLXRyZWUgKm5nRm9yPVwibGV0IGl0ZW0gb2YgZGF0YS5jaGlsZHJlblwiXHJcbiAgICAgICAgICAgICAgW2NoaWxkcmVuSGFuZGxlcl09XCJoYW5kbGVyXCJcclxuICAgICAgICAgICAgICBbY29udGVudF09XCJjb250ZW50XCJcclxuICAgICAgICAgICAgICBbbWFwXT1cInRyZWVNYXBcIlxyXG4gICAgICAgICAgICAgIFt0dWlUcmVlQ29udHJvbGxlcl09XCJmYWxzZVwiXHJcbiAgICAgICAgICAgICAgW3ZhbHVlXT1cIml0ZW1cIj48L3R1aS10cmVlPlxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2IGNsYXNzPVwic2lkZWJhci1tZW51X19jb2xsYXBzZS1pY29uXCJcclxuICAgICAgIFtuZ0NsYXNzXT1cInsnLS1jb2xsYXBzZWQnOiAhaXNFeHBhbmRlZH1cIlxyXG4gICAgICAgKGNsaWNrKT1cInRvZ2dsZU1lbnUoKVwiPlxyXG4gICAgPHR1aS1zdmcgW3NyY109XCJjb2xsYXBzZUljb25cIj48L3R1aS1zdmc+XHJcbiAgPC9kaXY+XHJcbjwvZGl2PlxyXG5cclxuXHJcbjxuZy10ZW1wbGF0ZSAjY29udGVudFxyXG4gICAgICAgICAgICAgbGV0LWl0ZW0+XHJcbiAgPHR1aS1zdmcgKm5nSWY9XCJpdGVtLmljb25cIlxyXG4gICAgICAgICAgIGNsYXNzPVwic2lkZWJhci1tZW51X19tZW51LWl0ZW0taWNvblwiXHJcbiAgICAgICAgICAgW25nQ2xhc3NdPVwieyctLWNvbGxhcHNlZCc6ICFpc0V4cGFuZGVkfVwiXHJcbiAgICAgICAgICAgW3NyY109XCJpdGVtLmljb25cIlxyXG4gICAgICAgICAgIChjbGljayk9XCJtZW51SXRlbUljb25DbGljayhpdGVtKVwiPjwvdHVpLXN2Zz5cclxuICA8c3BhbiAqbmdJZj1cImlzRXhwYW5kZWRcIlxyXG4gICAgICAgIGNsYXNzPVwic2lkZWJhci1tZW51X19tZW51LWl0ZW0tdGl0bGVcIlxyXG4gICAgICAgIChjbGljayk9XCJpdGVtLmFjdGlvbiAmJiBpdGVtLmFjdGlvbigpXCI+IHt7aXRlbS50ZXh0fX0gPC9zcGFuPlxyXG48L25nLXRlbXBsYXRlPlxyXG4iXX0=