import { Component, Inject } from '@angular/core';
import { TUI_TREE_CONTROLLER, TuiTreeItemContentComponent } from '@taiga-ui/kit';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { SidebarMenuService } from '../sidebar-menu.service';
import * as i0 from "@angular/core";
import * as i1 from "../sidebar-menu.service";
import * as i2 from "@angular/common";
import * as i3 from "@taiga-ui/core";
function MenuItemComponent_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainer(0);
} }
const _c0 = function (a0) { return { "--expanded": a0 }; };
function MenuItemComponent_tui_svg_2_Template(rf, ctx) { if (rf & 1) {
    const _r3 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tui-svg", 3);
    i0.ɵɵlistener("click", function MenuItemComponent_tui_svg_2_Template_tui_svg_click_0_listener() { i0.ɵɵrestoreView(_r3); const ctx_r2 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r2.onClick()); });
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const ctx_r1 = i0.ɵɵnextContext();
    i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(2, _c0, ctx_r1.isExpanded))("src", ctx_r1.icon);
} }
export class MenuItemComponent extends TuiTreeItemContentComponent {
    constructor(context, controller, sbS) {
        super(context, controller);
        this.sbS = sbS;
        this.sbS.isExpanded$.subscribe(isExpanded => {
            if (!isExpanded && this.isExpanded) {
                this.onClick();
            }
        });
    }
    get icon() {
        return this.isExpandable ? 'tuiIconChevronRight' : '';
    }
    ngOnDestroy() {
        this.sbS.isExpanded$.unsubscribe();
    }
}
MenuItemComponent.ɵfac = function MenuItemComponent_Factory(t) { return new (t || MenuItemComponent)(i0.ɵɵdirectiveInject(POLYMORPHEUS_CONTEXT), i0.ɵɵdirectiveInject(TUI_TREE_CONTROLLER), i0.ɵɵdirectiveInject(i1.SidebarMenuService)); };
MenuItemComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MenuItemComponent, selectors: [["mix-menu-item"]], features: [i0.ɵɵInheritDefinitionFeature], decls: 4, vars: 4, consts: [[1, "mix-menu-item"], [4, "ngTemplateOutlet"], ["class", "mix-menu-item__expand-icon", 3, "ngClass", "src", "click", 4, "ngIf"], [1, "mix-menu-item__expand-icon", 3, "ngClass", "src", "click"]], template: function MenuItemComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0);
        i0.ɵɵtemplate(1, MenuItemComponent_ng_container_1_Template, 1, 0, "ng-container", 1);
        i0.ɵɵtemplate(2, MenuItemComponent_tui_svg_2_Template, 1, 4, "tui-svg", 2);
        i0.ɵɵpipe(3, "async");
        i0.ɵɵelementEnd();
    } if (rf & 2) {
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngTemplateOutlet", ctx.context.template);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", i0.ɵɵpipeBind1(3, 2, ctx.sbS.isExpanded$));
    } }, dependencies: [i2.NgClass, i2.NgIf, i2.NgTemplateOutlet, i3.TuiSvgComponent, i2.AsyncPipe], styles: [".mix-menu-item[_ngcontent-%COMP%]{height:40px;display:flex;align-items:center;justify-content:space-between;padding:0 .5rem;border-radius:var(--tui-radius-xs)}.mix-menu-item[_ngcontent-%COMP%]:before, .mix-menu-item[_ngcontent-%COMP%]:after{content:\"\";position:absolute;left:-5px;z-index:-1}.mix-menu-item[_ngcontent-%COMP%]:before{width:7px;border-bottom:.5px dashed #ff0066}.mix-menu-item[_ngcontent-%COMP%]:after{top:-5px;bottom:20px;border-left:.5px dashed #ff0066}.mix-menu-item._expandable[_ngcontent-%COMP%]:hover{cursor:pointer}.mix-menu-item__expand-icon[_ngcontent-%COMP%]{cursor:pointer;position:relative;background:inherit;transition:all .2s ease-in}.mix-menu-item__expand-icon.--expanded[_ngcontent-%COMP%]{transform:rotate(-90deg)}tui-tree:last-child    > tui-tree-item    > [polymorpheus-outlet][_nghost-%COMP%], tui-tree:last-child    > tui-tree-item    > [polymorpheus-outlet]   [_nghost-%COMP%]{position:relative}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MenuItemComponent, [{
        type: Component,
        args: [{ selector: 'mix-menu-item', template: `
    <div class="mix-menu-item">
      <ng-container *ngTemplateOutlet="context.template"></ng-container>
      <tui-svg
        *ngIf="sbS.isExpanded$ | async"
        class="mix-menu-item__expand-icon"
        [ngClass]="{ '--expanded': isExpanded }"
        [src]="icon"
        (click)="onClick()"
      ></tui-svg>
    </div>
  `, styles: [".mix-menu-item{height:40px;display:flex;align-items:center;justify-content:space-between;padding:0 .5rem;border-radius:var(--tui-radius-xs)}.mix-menu-item:before,.mix-menu-item:after{content:\"\";position:absolute;left:-5px;z-index:-1}.mix-menu-item:before{width:7px;border-bottom:.5px dashed #ff0066}.mix-menu-item:after{top:-5px;bottom:20px;border-left:.5px dashed #ff0066}.mix-menu-item._expandable:hover{cursor:pointer}.mix-menu-item__expand-icon{cursor:pointer;position:relative;background:inherit;transition:all .2s ease-in}.mix-menu-item__expand-icon.--expanded{transform:rotate(-90deg)}:host-context(tui-tree:last-child > tui-tree-item > [polymorpheus-outlet]){position:relative}\n"] }]
    }], function () { return [{ type: undefined, decorators: [{
                type: Inject,
                args: [POLYMORPHEUS_CONTEXT]
            }] }, { type: undefined, decorators: [{
                type: Inject,
                args: [TUI_TREE_CONTROLLER]
            }] }, { type: i1.SidebarMenuService }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWVudS1pdGVtLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL3NpZGViYXItbWVudS9tZW51LWl0ZW0vbWVudS1pdGVtLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQzdELE9BQU8sRUFBRSxtQkFBbUIsRUFBcUIsMkJBQTJCLEVBQXNCLE1BQU0sZUFBZSxDQUFDO0FBQ3hILE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLDBCQUEwQixDQUFDO0FBRWhFLE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxNQUFNLHlCQUF5QixDQUFDOzs7Ozs7SUFNdkQsd0JBQWtFOzs7OztJQUNsRSxrQ0FNQztJQURDLG1LQUFTLGVBQUEsZ0JBQVMsQ0FBQSxJQUFDO0lBQ3BCLGlCQUFVOzs7SUFIVCx1RUFBd0Msb0JBQUE7O0FBUWhELE1BQU0sT0FBTyxpQkFBa0IsU0FBUSwyQkFBMkI7SUFDaEUsWUFDZ0MsT0FBMkIsRUFDNUIsVUFBNkIsRUFDbkQsR0FBdUI7UUFFOUIsS0FBSyxDQUFDLE9BQU8sRUFBRSxVQUFVLENBQUMsQ0FBQztRQUZwQixRQUFHLEdBQUgsR0FBRyxDQUFvQjtRQUc5QixJQUFJLENBQUMsR0FBRyxDQUFDLFdBQVcsQ0FBQyxTQUFTLENBQUMsVUFBVSxDQUFDLEVBQUU7WUFDMUMsSUFBSSxDQUFDLFVBQVUsSUFBSSxJQUFJLENBQUMsVUFBVSxFQUFFO2dCQUNsQyxJQUFJLENBQUMsT0FBTyxFQUFFLENBQUM7YUFDaEI7UUFDSCxDQUFDLENBQUMsQ0FBQztJQUNMLENBQUM7SUFFRCxJQUFJLElBQUk7UUFDTixPQUFPLElBQUksQ0FBQyxZQUFZLENBQUMsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUM7SUFDeEQsQ0FBQztJQUVELFdBQVc7UUFDVCxJQUFJLENBQUMsR0FBRyxDQUFDLFdBQVcsQ0FBQyxXQUFXLEVBQUUsQ0FBQztJQUNyQyxDQUFDOztrRkFwQlUsaUJBQWlCLHVCQUVsQixvQkFBb0Isd0JBQ3BCLG1CQUFtQjtvRUFIbEIsaUJBQWlCO1FBYjFCLDhCQUEyQjtRQUN6QixvRkFBa0U7UUFDbEUsMEVBTVc7O1FBQ2IsaUJBQU07O1FBUlcsZUFBa0M7UUFBbEMsdURBQWtDO1FBRTlDLGVBQTZCO1FBQTdCLGdFQUE2Qjs7dUZBVXpCLGlCQUFpQjtjQWhCN0IsU0FBUzsyQkFDRSxlQUFlLFlBQ2Y7Ozs7Ozs7Ozs7O0dBV1Q7O3NCQUtFLE1BQU07dUJBQUMsb0JBQW9COztzQkFDM0IsTUFBTTt1QkFBQyxtQkFBbUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIEluamVjdCwgT25EZXN0cm95IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFRVSV9UUkVFX0NPTlRST0xMRVIsIFR1aVRyZWVDb250cm9sbGVyLCBUdWlUcmVlSXRlbUNvbnRlbnRDb21wb25lbnQsIFR1aVRyZWVJdGVtQ29udGV4dCB9IGZyb20gJ0B0YWlnYS11aS9raXQnO1xyXG5pbXBvcnQgeyBQT0xZTU9SUEhFVVNfQ09OVEVYVCB9IGZyb20gJ0B0aW5rb2ZmL25nLXBvbHltb3JwaGV1cyc7XHJcblxyXG5pbXBvcnQgeyBTaWRlYmFyTWVudVNlcnZpY2UgfSBmcm9tICcuLi9zaWRlYmFyLW1lbnUuc2VydmljZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC1tZW51LWl0ZW0nLFxyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8ZGl2IGNsYXNzPVwibWl4LW1lbnUtaXRlbVwiPlxyXG4gICAgICA8bmctY29udGFpbmVyICpuZ1RlbXBsYXRlT3V0bGV0PVwiY29udGV4dC50ZW1wbGF0ZVwiPjwvbmctY29udGFpbmVyPlxyXG4gICAgICA8dHVpLXN2Z1xyXG4gICAgICAgICpuZ0lmPVwic2JTLmlzRXhwYW5kZWQkIHwgYXN5bmNcIlxyXG4gICAgICAgIGNsYXNzPVwibWl4LW1lbnUtaXRlbV9fZXhwYW5kLWljb25cIlxyXG4gICAgICAgIFtuZ0NsYXNzXT1cInsgJy0tZXhwYW5kZWQnOiBpc0V4cGFuZGVkIH1cIlxyXG4gICAgICAgIFtzcmNdPVwiaWNvblwiXHJcbiAgICAgICAgKGNsaWNrKT1cIm9uQ2xpY2soKVwiXHJcbiAgICAgID48L3R1aS1zdmc+XHJcbiAgICA8L2Rpdj5cclxuICBgLFxyXG4gIHN0eWxlVXJsczogWycuL21lbnUtaXRlbS5zY3NzJ11cclxufSlcclxuZXhwb3J0IGNsYXNzIE1lbnVJdGVtQ29tcG9uZW50IGV4dGVuZHMgVHVpVHJlZUl0ZW1Db250ZW50Q29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcclxuICBjb25zdHJ1Y3RvcihcclxuICAgIEBJbmplY3QoUE9MWU1PUlBIRVVTX0NPTlRFWFQpIGNvbnRleHQ6IFR1aVRyZWVJdGVtQ29udGV4dCxcclxuICAgIEBJbmplY3QoVFVJX1RSRUVfQ09OVFJPTExFUikgY29udHJvbGxlcjogVHVpVHJlZUNvbnRyb2xsZXIsXHJcbiAgICBwdWJsaWMgc2JTOiBTaWRlYmFyTWVudVNlcnZpY2VcclxuICApIHtcclxuICAgIHN1cGVyKGNvbnRleHQsIGNvbnRyb2xsZXIpO1xyXG4gICAgdGhpcy5zYlMuaXNFeHBhbmRlZCQuc3Vic2NyaWJlKGlzRXhwYW5kZWQgPT4ge1xyXG4gICAgICBpZiAoIWlzRXhwYW5kZWQgJiYgdGhpcy5pc0V4cGFuZGVkKSB7XHJcbiAgICAgICAgdGhpcy5vbkNsaWNrKCk7XHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLmlzRXhwYW5kYWJsZSA/ICd0dWlJY29uQ2hldnJvblJpZ2h0JyA6ICcnO1xyXG4gIH1cclxuXHJcbiAgbmdPbkRlc3Ryb3koKSB7XHJcbiAgICB0aGlzLnNiUy5pc0V4cGFuZGVkJC51bnN1YnNjcmliZSgpO1xyXG4gIH1cclxufVxyXG4iXX0=