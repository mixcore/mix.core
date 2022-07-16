import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ShareModule } from '../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common";
import * as i2 from "@taiga-ui/core";
import * as i3 from "angular-tabler-icons";
function MixToolbarComponent_ng_template_7_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "span");
    i0.ɵɵtext(1, "Delete");
    i0.ɵɵelementEnd();
} }
function MixToolbarComponent_ng_template_11_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "span");
    i0.ɵɵtext(1, "Re-name");
    i0.ɵɵelementEnd();
} }
function MixToolbarComponent_ng_template_15_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "span");
    i0.ɵɵtext(1, "Duplicate");
    i0.ɵɵelementEnd();
} }
const _c0 = function (a0) { return { "--disabled": a0 }; };
export class MixToolbarComponent {
    constructor() {
        this.selectedItem = [];
        this.delete = new EventEmitter();
    }
    onDelete() {
        this.delete.emit();
    }
}
MixToolbarComponent.ɵfac = function MixToolbarComponent_Factory(t) { return new (t || MixToolbarComponent)(); };
MixToolbarComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixToolbarComponent, selectors: [["mix-toolbar"]], inputs: { selectedItem: "selectedItem" }, outputs: { delete: "delete" }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 17, vars: 21, consts: [[1, "mix-toolbar"], [1, "mix-toolbar__toolbar-item"], ["name", "circle-plus"], ["tuiHintDirection", "bottom-middle", 1, "mix-toolbar__toolbar-item", 3, "ngClass", "tuiHint", "tuiHintHideDelay", "tuiHintMode", "tuiHintShowDelay", "click"], ["name", "trash"], ["tooltip", ""], ["tuiHintDirection", "bottom-middle", 1, "mix-toolbar__toolbar-item", 3, "ngClass", "tuiHint", "tuiHintHideDelay", "tuiHintMode", "tuiHintShowDelay"], ["name", "writing"], ["tooltip1", ""], ["name", "copy"], ["tooltip2", ""]], template: function MixToolbarComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1);
        i0.ɵɵelement(2, "i-tabler", 2);
        i0.ɵɵelementStart(3, "span");
        i0.ɵɵtext(4, " New ");
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(5, "div", 3);
        i0.ɵɵlistener("click", function MixToolbarComponent_Template_div_click_5_listener() { return ctx.onDelete(); });
        i0.ɵɵelement(6, "i-tabler", 4);
        i0.ɵɵtemplate(7, MixToolbarComponent_ng_template_7_Template, 2, 0, "ng-template", null, 5, i0.ɵɵtemplateRefExtractor);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(9, "div", 6);
        i0.ɵɵelement(10, "i-tabler", 7);
        i0.ɵɵtemplate(11, MixToolbarComponent_ng_template_11_Template, 2, 0, "ng-template", null, 8, i0.ɵɵtemplateRefExtractor);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(13, "div", 6);
        i0.ɵɵelement(14, "i-tabler", 9);
        i0.ɵɵtemplate(15, MixToolbarComponent_ng_template_15_Template, 2, 0, "ng-template", null, 10, i0.ɵɵtemplateRefExtractor);
        i0.ɵɵelementEnd()();
    } if (rf & 2) {
        const _r0 = i0.ɵɵreference(8);
        const _r2 = i0.ɵɵreference(12);
        const _r4 = i0.ɵɵreference(16);
        i0.ɵɵadvance(5);
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(15, _c0, ctx.selectedItem.length <= 0))("tuiHint", _r0)("tuiHintHideDelay", 0)("tuiHintMode", "onDark")("tuiHintShowDelay", 50);
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(17, _c0, ctx.selectedItem.length !== 1))("tuiHint", _r2)("tuiHintHideDelay", 0)("tuiHintMode", "onDark")("tuiHintShowDelay", 50);
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(19, _c0, ctx.selectedItem.length !== 1))("tuiHint", _r4)("tuiHintHideDelay", 0)("tuiHintMode", "onDark")("tuiHintShowDelay", 50);
    } }, dependencies: [ShareModule, i1.NgClass, i2.TuiHintDirective, i3.TablerIconComponent], styles: [".mix-toolbar[_ngcontent-%COMP%]{height:100%;width:-moz-fit-content;width:fit-content;display:flex}.mix-toolbar__toolbar-item[_ngcontent-%COMP%]{display:flex;justify-content:center;align-items:center;cursor:pointer;opacity:1;transition:all .2s ease-in;border-radius:8px;padding:8px 13px}.mix-toolbar__toolbar-item[_ngcontent-%COMP%]:hover{background-color:#fff}.mix-toolbar__toolbar-item.--disabled[_ngcontent-%COMP%]{opacity:.3;pointer-events:none}.mix-toolbar__toolbar-item[_ngcontent-%COMP%]   i-tabler[_ngcontent-%COMP%]{margin-right:3px}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixToolbarComponent, [{
        type: Component,
        args: [{ selector: 'mix-toolbar', standalone: true, imports: [ShareModule], template: "<div class=\"mix-toolbar\">\r\n  <div class=\"mix-toolbar__toolbar-item\">\r\n    <i-tabler name=\"circle-plus\"></i-tabler> <span> New </span>\r\n  </div>\r\n\r\n  <div class=\"mix-toolbar__toolbar-item\"\r\n       [ngClass]=\"{'--disabled': selectedItem.length <= 0}\"\r\n       [tuiHint]=\"tooltip\"\r\n       [tuiHintHideDelay]=\"0\"\r\n       [tuiHintMode]=\"'onDark'\"\r\n       [tuiHintShowDelay]=\"50\"\r\n       (click)=\"onDelete()\"\r\n       tuiHintDirection=\"bottom-middle\">\r\n    <i-tabler name=\"trash\"></i-tabler>\r\n    <ng-template #tooltip>\r\n      <span>Delete</span>\r\n    </ng-template>\r\n  </div>\r\n\r\n  <div class=\"mix-toolbar__toolbar-item\"\r\n       [ngClass]=\"{'--disabled': selectedItem.length !== 1}\"\r\n       [tuiHint]=\"tooltip1\"\r\n       [tuiHintHideDelay]=\"0\"\r\n       [tuiHintMode]=\"'onDark'\"\r\n       [tuiHintShowDelay]=\"50\"\r\n       tuiHintDirection=\"bottom-middle\">\r\n    <i-tabler name=\"writing\"></i-tabler>\r\n    <ng-template #tooltip1>\r\n      <span>Re-name</span>\r\n    </ng-template>\r\n  </div>\r\n\r\n  <div class=\"mix-toolbar__toolbar-item\"\r\n       [ngClass]=\"{'--disabled': selectedItem.length !==  1}\"\r\n       [tuiHint]=\"tooltip2\"\r\n       [tuiHintHideDelay]=\"0\"\r\n       [tuiHintMode]=\"'onDark'\"\r\n       [tuiHintShowDelay]=\"50\"\r\n       tuiHintDirection=\"bottom-middle\">\r\n    <i-tabler name=\"copy\"></i-tabler>\r\n    <ng-template #tooltip2>\r\n      <span>Duplicate</span>\r\n    </ng-template>\r\n  </div>\r\n</div>\r\n", styles: [".mix-toolbar{height:100%;width:-moz-fit-content;width:fit-content;display:flex}.mix-toolbar__toolbar-item{display:flex;justify-content:center;align-items:center;cursor:pointer;opacity:1;transition:all .2s ease-in;border-radius:8px;padding:8px 13px}.mix-toolbar__toolbar-item:hover{background-color:#fff}.mix-toolbar__toolbar-item.--disabled{opacity:.3;pointer-events:none}.mix-toolbar__toolbar-item i-tabler{margin-right:3px}\n"] }]
    }], null, { selectedItem: [{
            type: Input
        }], delete: [{
            type: Output
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LXRvb2xiYXIuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21peC10b29sYmFyL21peC10b29sYmFyLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtdG9vbGJhci9taXgtdG9vbGJhci5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXZFLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQzs7Ozs7O0lDYTNDLDRCQUFNO0lBQUEsc0JBQU07SUFBQSxpQkFBTzs7O0lBYW5CLDRCQUFNO0lBQUEsdUJBQU87SUFBQSxpQkFBTzs7O0lBYXBCLDRCQUFNO0lBQUEseUJBQVM7SUFBQSxpQkFBTzs7O0FEOUI1QixNQUFNLE9BQU8sbUJBQW1CO0lBUGhDO1FBUWtCLGlCQUFZLEdBQVEsRUFBRSxDQUFDO1FBQzdCLFdBQU0sR0FBdUIsSUFBSSxZQUFZLEVBQVEsQ0FBQztLQUtqRTtJQUhRLFFBQVE7UUFDYixJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksRUFBRSxDQUFDO0lBQ3JCLENBQUM7O3NGQU5VLG1CQUFtQjtzRUFBbkIsbUJBQW1CO1FDWGhDLDhCQUF5QixhQUFBO1FBRXJCLDhCQUF3QztRQUFDLDRCQUFNO1FBQUMscUJBQUk7UUFBQSxpQkFBTyxFQUFBO1FBRzdELDhCQU9zQztRQURqQyw2RkFBUyxjQUFVLElBQUM7UUFFdkIsOEJBQWtDO1FBQ2xDLHFIQUVjO1FBQ2hCLGlCQUFNO1FBRU4sOEJBTXNDO1FBQ3BDLCtCQUFvQztRQUNwQyx1SEFFYztRQUNoQixpQkFBTTtRQUVOLCtCQU1zQztRQUNwQywrQkFBaUM7UUFDakMsd0hBRWM7UUFDaEIsaUJBQU0sRUFBQTs7Ozs7UUFyQ0QsZUFBb0Q7UUFBcEQsbUZBQW9ELGdCQUFBLHVCQUFBLHlCQUFBLHdCQUFBO1FBY3BELGVBQXFEO1FBQXJELG9GQUFxRCxnQkFBQSx1QkFBQSx5QkFBQSx3QkFBQTtRQWFyRCxlQUFzRDtRQUF0RCxvRkFBc0QsZ0JBQUEsdUJBQUEseUJBQUEsd0JBQUE7d0JEeEJqRCxXQUFXO3VGQUVWLG1CQUFtQjtjQVAvQixTQUFTOzJCQUNFLGFBQWEsY0FHWCxJQUFJLFdBQ1AsQ0FBQyxXQUFXLENBQUM7Z0JBR04sWUFBWTtrQkFBM0IsS0FBSztZQUNJLE1BQU07a0JBQWYsTUFBTSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5pbXBvcnQgeyBTaGFyZU1vZHVsZSB9IGZyb20gJy4uLy4uL3NoYXJlLm1vZHVsZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC10b29sYmFyJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vbWl4LXRvb2xiYXIuY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL21peC10b29sYmFyLmNvbXBvbmVudC5zY3NzJ10sXHJcbiAgc3RhbmRhbG9uZTogdHJ1ZSxcclxuICBpbXBvcnRzOiBbU2hhcmVNb2R1bGVdXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBNaXhUb29sYmFyQ29tcG9uZW50PFQ+IHtcclxuICBASW5wdXQoKSBwdWJsaWMgc2VsZWN0ZWRJdGVtOiBUW10gPSBbXTtcclxuICBAT3V0cHV0KCkgZGVsZXRlOiBFdmVudEVtaXR0ZXI8dm9pZD4gPSBuZXcgRXZlbnRFbWl0dGVyPHZvaWQ+KCk7XHJcblxyXG4gIHB1YmxpYyBvbkRlbGV0ZSgpOiB2b2lkIHtcclxuICAgIHRoaXMuZGVsZXRlLmVtaXQoKTtcclxuICB9XHJcbn1cclxuIiwiPGRpdiBjbGFzcz1cIm1peC10b29sYmFyXCI+XHJcbiAgPGRpdiBjbGFzcz1cIm1peC10b29sYmFyX190b29sYmFyLWl0ZW1cIj5cclxuICAgIDxpLXRhYmxlciBuYW1lPVwiY2lyY2xlLXBsdXNcIj48L2ktdGFibGVyPiA8c3Bhbj4gTmV3IDwvc3Bhbj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cIm1peC10b29sYmFyX190b29sYmFyLWl0ZW1cIlxyXG4gICAgICAgW25nQ2xhc3NdPVwieyctLWRpc2FibGVkJzogc2VsZWN0ZWRJdGVtLmxlbmd0aCA8PSAwfVwiXHJcbiAgICAgICBbdHVpSGludF09XCJ0b29sdGlwXCJcclxuICAgICAgIFt0dWlIaW50SGlkZURlbGF5XT1cIjBcIlxyXG4gICAgICAgW3R1aUhpbnRNb2RlXT1cIidvbkRhcmsnXCJcclxuICAgICAgIFt0dWlIaW50U2hvd0RlbGF5XT1cIjUwXCJcclxuICAgICAgIChjbGljayk9XCJvbkRlbGV0ZSgpXCJcclxuICAgICAgIHR1aUhpbnREaXJlY3Rpb249XCJib3R0b20tbWlkZGxlXCI+XHJcbiAgICA8aS10YWJsZXIgbmFtZT1cInRyYXNoXCI+PC9pLXRhYmxlcj5cclxuICAgIDxuZy10ZW1wbGF0ZSAjdG9vbHRpcD5cclxuICAgICAgPHNwYW4+RGVsZXRlPC9zcGFuPlxyXG4gICAgPC9uZy10ZW1wbGF0ZT5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cIm1peC10b29sYmFyX190b29sYmFyLWl0ZW1cIlxyXG4gICAgICAgW25nQ2xhc3NdPVwieyctLWRpc2FibGVkJzogc2VsZWN0ZWRJdGVtLmxlbmd0aCAhPT0gMX1cIlxyXG4gICAgICAgW3R1aUhpbnRdPVwidG9vbHRpcDFcIlxyXG4gICAgICAgW3R1aUhpbnRIaWRlRGVsYXldPVwiMFwiXHJcbiAgICAgICBbdHVpSGludE1vZGVdPVwiJ29uRGFyaydcIlxyXG4gICAgICAgW3R1aUhpbnRTaG93RGVsYXldPVwiNTBcIlxyXG4gICAgICAgdHVpSGludERpcmVjdGlvbj1cImJvdHRvbS1taWRkbGVcIj5cclxuICAgIDxpLXRhYmxlciBuYW1lPVwid3JpdGluZ1wiPjwvaS10YWJsZXI+XHJcbiAgICA8bmctdGVtcGxhdGUgI3Rvb2x0aXAxPlxyXG4gICAgICA8c3Bhbj5SZS1uYW1lPC9zcGFuPlxyXG4gICAgPC9uZy10ZW1wbGF0ZT5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cIm1peC10b29sYmFyX190b29sYmFyLWl0ZW1cIlxyXG4gICAgICAgW25nQ2xhc3NdPVwieyctLWRpc2FibGVkJzogc2VsZWN0ZWRJdGVtLmxlbmd0aCAhPT0gIDF9XCJcclxuICAgICAgIFt0dWlIaW50XT1cInRvb2x0aXAyXCJcclxuICAgICAgIFt0dWlIaW50SGlkZURlbGF5XT1cIjBcIlxyXG4gICAgICAgW3R1aUhpbnRNb2RlXT1cIidvbkRhcmsnXCJcclxuICAgICAgIFt0dWlIaW50U2hvd0RlbGF5XT1cIjUwXCJcclxuICAgICAgIHR1aUhpbnREaXJlY3Rpb249XCJib3R0b20tbWlkZGxlXCI+XHJcbiAgICA8aS10YWJsZXIgbmFtZT1cImNvcHlcIj48L2ktdGFibGVyPlxyXG4gICAgPG5nLXRlbXBsYXRlICN0b29sdGlwMj5cclxuICAgICAgPHNwYW4+RHVwbGljYXRlPC9zcGFuPlxyXG4gICAgPC9uZy10ZW1wbGF0ZT5cclxuICA8L2Rpdj5cclxuPC9kaXY+XHJcbiJdfQ==