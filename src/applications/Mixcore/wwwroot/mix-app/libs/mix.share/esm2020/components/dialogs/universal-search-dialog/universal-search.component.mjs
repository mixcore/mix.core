import { Component } from '@angular/core';
import { ShareModule } from '../../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "@angular/forms";
import * as i2 from "@taiga-ui/kit";
import * as i3 from "@taiga-ui/core";
function UniversalSearchComponent_button_7_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "button", 7);
    i0.ɵɵtext(1, " Pages ");
    i0.ɵɵelementEnd();
} }
function UniversalSearchComponent_button_8_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "button", 7);
    i0.ɵɵtext(1, " Modules ");
    i0.ɵɵelementEnd();
} }
function UniversalSearchComponent_button_9_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "button", 7);
    i0.ɵɵtext(1, " Posts ");
    i0.ɵɵelementEnd();
} }
function UniversalSearchComponent_button_10_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "button", 7);
    i0.ɵɵtext(1, " Google ");
    i0.ɵɵelementEnd();
} }
function UniversalSearchComponent_button_11_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "button", 7);
    i0.ɵɵtext(1, " Bing ");
    i0.ɵɵelementEnd();
} }
export class UniversalSearchComponent {
    constructor() {
        this.activeItemIndex = 0;
        this.searchText = '';
    }
}
UniversalSearchComponent.ɵfac = function UniversalSearchComponent_Factory(t) { return new (t || UniversalSearchComponent)(); };
UniversalSearchComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: UniversalSearchComponent, selectors: [["mix-universal-search"]], standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 12, vars: 3, consts: [[1, "universal-search"], [1, "universal-search__header"], ["tuiTextfieldSize", "m", 3, "ngModel", "ngModelChange"], ["type", "text", "tuiTextfield", ""], [1, "universal-search__content"], [3, "activeItemIndex", "itemsLimit"], ["tuiTab", "", 4, "tuiTab"], ["tuiTab", ""]], template: function UniversalSearchComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1)(2, "tui-input", 2);
        i0.ɵɵlistener("ngModelChange", function UniversalSearchComponent_Template_tui_input_ngModelChange_2_listener($event) { return ctx.searchText = $event; });
        i0.ɵɵtext(3, " Type anything that you want to search ");
        i0.ɵɵelement(4, "input", 3);
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(5, "div", 4)(6, "tui-tabs-with-more", 5);
        i0.ɵɵtemplate(7, UniversalSearchComponent_button_7_Template, 2, 0, "button", 6);
        i0.ɵɵtemplate(8, UniversalSearchComponent_button_8_Template, 2, 0, "button", 6);
        i0.ɵɵtemplate(9, UniversalSearchComponent_button_9_Template, 2, 0, "button", 6);
        i0.ɵɵtemplate(10, UniversalSearchComponent_button_10_Template, 2, 0, "button", 6);
        i0.ɵɵtemplate(11, UniversalSearchComponent_button_11_Template, 2, 0, "button", 6);
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("ngModel", ctx.searchText);
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("activeItemIndex", ctx.activeItemIndex)("itemsLimit", 3);
    } }, dependencies: [ShareModule, i1.NgControlStatus, i1.NgModel, i2.TuiInputComponent, i2.TuiInputDirective, i3.TuiTextfieldComponent, i3.TuiTextfieldSizeDirective, i2.TuiTabsWithMoreComponent, i2.TuiTabComponent, i2.TuiTabDirective], styles: [".universal-search[_ngcontent-%COMP%]{width:600px;height:600px}.universal-search__header[_ngcontent-%COMP%]{margin-bottom:10px}.universal-search__content[_ngcontent-%COMP%]{padding:0 10px}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(UniversalSearchComponent, [{
        type: Component,
        args: [{ selector: 'mix-universal-search', standalone: true, imports: [ShareModule], template: "<div class=\"universal-search\">\r\n  <div class=\"universal-search__header\">\r\n    <tui-input [(ngModel)]=\"searchText\"\r\n               tuiTextfieldSize=\"m\">\r\n      Type anything that you want to search\r\n      <input type=\"text\"\r\n             tuiTextfield>\r\n    </tui-input>\r\n  </div>\r\n\r\n  <div class=\"universal-search__content\">\r\n    <tui-tabs-with-more [activeItemIndex]=\"activeItemIndex\"\r\n                        [itemsLimit]=\"3\">\r\n      <button *tuiTab\r\n              tuiTab>\r\n        Pages\r\n      </button>\r\n      <button *tuiTab\r\n              tuiTab>\r\n        Modules\r\n      </button>\r\n      <button *tuiTab\r\n              tuiTab>\r\n\r\n        Posts\r\n      </button>\r\n      <button *tuiTab\r\n              tuiTab>\r\n        Google\r\n      </button>\r\n\r\n      <button *tuiTab\r\n              tuiTab>\r\n        Bing\r\n      </button>\r\n    </tui-tabs-with-more>\r\n  </div>\r\n</div>\r\n", styles: [".universal-search{width:600px;height:600px}.universal-search__header{margin-bottom:10px}.universal-search__content{padding:0 10px}\n"] }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidW5pdmVyc2FsLXNlYXJjaC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL2NvbXBvbmVudHMvZGlhbG9ncy91bml2ZXJzYWwtc2VhcmNoLWRpYWxvZy91bml2ZXJzYWwtc2VhcmNoLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9kaWFsb2dzL3VuaXZlcnNhbC1zZWFyY2gtZGlhbG9nL3VuaXZlcnNhbC1zZWFyY2guY29tcG9uZW50Lmh0bWwiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUxQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7Ozs7OztJQ1c5QyxpQ0FDZTtJQUNiLHVCQUNGO0lBQUEsaUJBQVM7OztJQUNULGlDQUNlO0lBQ2IseUJBQ0Y7SUFBQSxpQkFBUzs7O0lBQ1QsaUNBQ2U7SUFFYix1QkFDRjtJQUFBLGlCQUFTOzs7SUFDVCxpQ0FDZTtJQUNiLHdCQUNGO0lBQUEsaUJBQVM7OztJQUVULGlDQUNlO0lBQ2Isc0JBQ0Y7SUFBQSxpQkFBUzs7QUR2QmYsTUFBTSxPQUFPLHdCQUF3QjtJQVByQztRQVFTLG9CQUFlLEdBQUcsQ0FBQyxDQUFDO1FBQ3BCLGVBQVUsR0FBRyxFQUFFLENBQUM7S0FDeEI7O2dHQUhZLHdCQUF3QjsyRUFBeEIsd0JBQXdCO1FDWHJDLDhCQUE4QixhQUFBLG1CQUFBO1FBRWYseUpBQXdCO1FBRWpDLHVEQUNBO1FBQUEsMkJBQ29CO1FBQ3RCLGlCQUFZLEVBQUE7UUFHZCw4QkFBdUMsNEJBQUE7UUFHbkMsK0VBR1M7UUFDVCwrRUFHUztRQUNULCtFQUlTO1FBQ1QsaUZBR1M7UUFFVCxpRkFHUztRQUNYLGlCQUFxQixFQUFBLEVBQUE7O1FBakNWLGVBQXdCO1FBQXhCLHdDQUF3QjtRQVNmLGVBQW1DO1FBQW5DLHFEQUFtQyxpQkFBQTt3QkRGL0MsV0FBVzt1RkFFVix3QkFBd0I7Y0FQcEMsU0FBUzsyQkFDRSxzQkFBc0IsY0FHcEIsSUFBSSxXQUNQLENBQUMsV0FBVyxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5pbXBvcnQgeyBTaGFyZU1vZHVsZSB9IGZyb20gJy4uLy4uLy4uL3NoYXJlLm1vZHVsZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC11bml2ZXJzYWwtc2VhcmNoJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vdW5pdmVyc2FsLXNlYXJjaC5jb21wb25lbnQuaHRtbCcsXHJcbiAgc3R5bGVVcmxzOiBbJy4vdW5pdmVyc2FsLXNlYXJjaC5jb21wb25lbnQuc2NzcyddLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW1NoYXJlTW9kdWxlXVxyXG59KVxyXG5leHBvcnQgY2xhc3MgVW5pdmVyc2FsU2VhcmNoQ29tcG9uZW50IHtcclxuICBwdWJsaWMgYWN0aXZlSXRlbUluZGV4ID0gMDtcclxuICBwdWJsaWMgc2VhcmNoVGV4dCA9ICcnO1xyXG59XHJcbiIsIjxkaXYgY2xhc3M9XCJ1bml2ZXJzYWwtc2VhcmNoXCI+XHJcbiAgPGRpdiBjbGFzcz1cInVuaXZlcnNhbC1zZWFyY2hfX2hlYWRlclwiPlxyXG4gICAgPHR1aS1pbnB1dCBbKG5nTW9kZWwpXT1cInNlYXJjaFRleHRcIlxyXG4gICAgICAgICAgICAgICB0dWlUZXh0ZmllbGRTaXplPVwibVwiPlxyXG4gICAgICBUeXBlIGFueXRoaW5nIHRoYXQgeW91IHdhbnQgdG8gc2VhcmNoXHJcbiAgICAgIDxpbnB1dCB0eXBlPVwidGV4dFwiXHJcbiAgICAgICAgICAgICB0dWlUZXh0ZmllbGQ+XHJcbiAgICA8L3R1aS1pbnB1dD5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cInVuaXZlcnNhbC1zZWFyY2hfX2NvbnRlbnRcIj5cclxuICAgIDx0dWktdGFicy13aXRoLW1vcmUgW2FjdGl2ZUl0ZW1JbmRleF09XCJhY3RpdmVJdGVtSW5kZXhcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICBbaXRlbXNMaW1pdF09XCIzXCI+XHJcbiAgICAgIDxidXR0b24gKnR1aVRhYlxyXG4gICAgICAgICAgICAgIHR1aVRhYj5cclxuICAgICAgICBQYWdlc1xyXG4gICAgICA8L2J1dHRvbj5cclxuICAgICAgPGJ1dHRvbiAqdHVpVGFiXHJcbiAgICAgICAgICAgICAgdHVpVGFiPlxyXG4gICAgICAgIE1vZHVsZXNcclxuICAgICAgPC9idXR0b24+XHJcbiAgICAgIDxidXR0b24gKnR1aVRhYlxyXG4gICAgICAgICAgICAgIHR1aVRhYj5cclxuXHJcbiAgICAgICAgUG9zdHNcclxuICAgICAgPC9idXR0b24+XHJcbiAgICAgIDxidXR0b24gKnR1aVRhYlxyXG4gICAgICAgICAgICAgIHR1aVRhYj5cclxuICAgICAgICBHb29nbGVcclxuICAgICAgPC9idXR0b24+XHJcblxyXG4gICAgICA8YnV0dG9uICp0dWlUYWJcclxuICAgICAgICAgICAgICB0dWlUYWI+XHJcbiAgICAgICAgQmluZ1xyXG4gICAgICA8L2J1dHRvbj5cclxuICAgIDwvdHVpLXRhYnMtd2l0aC1tb3JlPlxyXG4gIDwvZGl2PlxyXG48L2Rpdj5cclxuIl19