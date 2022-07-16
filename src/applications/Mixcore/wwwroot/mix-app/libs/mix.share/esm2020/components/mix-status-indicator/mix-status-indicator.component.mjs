import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MixContentStatus } from '@mix-spa/mix.lib';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common";
const _c0 = ["status", ""];
const _c1 = function (a0) { return { background: a0 }; };
export class MixStatusIndicatorComponent {
    constructor() {
        this.option = {
            Published: {
                label: 'Published',
                color: '#4BD28F'
            },
            Deleted: {
                label: 'Deleted',
                color: '#FF4D4D'
            },
            Draft: {
                label: '#D8E2E9',
                color: 'green'
            },
            Schedule: {
                label: 'Schedule',
                color: '#FFAA04'
            },
            Preview: {
                label: 'Preview',
                color: '#FFAA04'
            }
        };
        //
    }
}
MixStatusIndicatorComponent.ɵfac = function MixStatusIndicatorComponent_Factory(t) { return new (t || MixStatusIndicatorComponent)(); };
MixStatusIndicatorComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixStatusIndicatorComponent, selectors: [["mix-status-indicator", "status", ""]], inputs: { status: "status" }, standalone: true, features: [i0.ɵɵStandaloneFeature], attrs: _c0, decls: 4, vars: 4, consts: [[1, "status-indicator"], [1, "status-indicator__dot", 3, "ngStyle"], [1, "status-indicator__label"]], template: function MixStatusIndicatorComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0);
        i0.ɵɵelement(1, "div", 1);
        i0.ɵɵelementStart(2, "div", 2);
        i0.ɵɵtext(3);
        i0.ɵɵelementEnd()();
    } if (rf & 2) {
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngStyle", i0.ɵɵpureFunction1(2, _c1, ctx.option[ctx.status].color));
        i0.ɵɵadvance(2);
        i0.ɵɵtextInterpolate1(" ", ctx.option[ctx.status].label, " ");
    } }, dependencies: [CommonModule, i1.NgStyle], styles: [".status-indicator[_ngcontent-%COMP%]{display:flex;justify-content:center;align-items:center;width:-moz-fit-content;width:fit-content}.status-indicator__dot[_ngcontent-%COMP%]{display:inline-block;content:\"\";width:10px;height:10px;margin-right:10px;border-radius:50%}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixStatusIndicatorComponent, [{
        type: Component,
        args: [{ selector: 'mix-status-indicator [status]', standalone: true, imports: [CommonModule], template: "<div class=\"status-indicator\">\r\n  <div class=\"status-indicator__dot\"\r\n       [ngStyle]=\"{background: option[status].color}\">\r\n  </div>\r\n\r\n  <div class=\"status-indicator__label\">\r\n    {{ option[status].label }}\r\n  </div>\r\n</div>\r\n", styles: [".status-indicator{display:flex;justify-content:center;align-items:center;width:-moz-fit-content;width:fit-content}.status-indicator__dot{display:inline-block;content:\"\";width:10px;height:10px;margin-right:10px;border-radius:50%}\n"] }]
    }], function () { return []; }, { status: [{
            type: Input
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LXN0YXR1cy1pbmRpY2F0b3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21peC1zdGF0dXMtaW5kaWNhdG9yL21peC1zdGF0dXMtaW5kaWNhdG9yLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtc3RhdHVzLWluZGljYXRvci9taXgtc3RhdHVzLWluZGljYXRvci5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDakQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sa0JBQWtCLENBQUM7Ozs7O0FBU3BELE1BQU0sT0FBTywyQkFBMkI7SUEwQnRDO1FBdkJnQixXQUFNLEdBQStEO1lBQ25GLFNBQVMsRUFBRTtnQkFDVCxLQUFLLEVBQUUsV0FBVztnQkFDbEIsS0FBSyxFQUFFLFNBQVM7YUFDakI7WUFDRCxPQUFPLEVBQUU7Z0JBQ1AsS0FBSyxFQUFFLFNBQVM7Z0JBQ2hCLEtBQUssRUFBRSxTQUFTO2FBQ2pCO1lBQ0QsS0FBSyxFQUFFO2dCQUNMLEtBQUssRUFBRSxTQUFTO2dCQUNoQixLQUFLLEVBQUUsT0FBTzthQUNmO1lBQ0QsUUFBUSxFQUFFO2dCQUNSLEtBQUssRUFBRSxVQUFVO2dCQUNqQixLQUFLLEVBQUUsU0FBUzthQUNqQjtZQUNELE9BQU8sRUFBRTtnQkFDUCxLQUFLLEVBQUUsU0FBUztnQkFDaEIsS0FBSyxFQUFFLFNBQVM7YUFDakI7U0FDRixDQUFDO1FBR0EsRUFBRTtJQUNKLENBQUM7O3NHQTVCVSwyQkFBMkI7OEVBQTNCLDJCQUEyQjtRQ1h4Qyw4QkFBOEI7UUFDNUIseUJBRU07UUFFTiw4QkFBcUM7UUFDbkMsWUFDRjtRQUFBLGlCQUFNLEVBQUE7O1FBTEQsZUFBOEM7UUFBOUMsa0ZBQThDO1FBSWpELGVBQ0Y7UUFERSw2REFDRjt3QkRFVSxZQUFZO3VGQUVYLDJCQUEyQjtjQVB2QyxTQUFTOzJCQUNFLCtCQUErQixjQUc3QixJQUFJLFdBQ1AsQ0FBQyxZQUFZLENBQUM7c0NBR1AsTUFBTTtrQkFBckIsS0FBSyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbW1vbk1vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbic7XHJcbmltcG9ydCB7IENvbXBvbmVudCwgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgTWl4Q29udGVudFN0YXR1cyB9IGZyb20gJ0BtaXgtc3BhL21peC5saWInO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtc3RhdHVzLWluZGljYXRvciBbc3RhdHVzXScsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL21peC1zdGF0dXMtaW5kaWNhdG9yLmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi9taXgtc3RhdHVzLWluZGljYXRvci5jb21wb25lbnQuc2NzcyddLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW0NvbW1vbk1vZHVsZV1cclxufSlcclxuZXhwb3J0IGNsYXNzIE1peFN0YXR1c0luZGljYXRvckNvbXBvbmVudCB7XHJcbiAgQElucHV0KCkgcHVibGljIHN0YXR1cyE6IE1peENvbnRlbnRTdGF0dXM7XHJcblxyXG4gIHB1YmxpYyByZWFkb25seSBvcHRpb246IFJlY29yZDxNaXhDb250ZW50U3RhdHVzLCB7IGxhYmVsOiBzdHJpbmc7IGNvbG9yOiBzdHJpbmcgfT4gPSB7XHJcbiAgICBQdWJsaXNoZWQ6IHtcclxuICAgICAgbGFiZWw6ICdQdWJsaXNoZWQnLFxyXG4gICAgICBjb2xvcjogJyM0QkQyOEYnXHJcbiAgICB9LFxyXG4gICAgRGVsZXRlZDoge1xyXG4gICAgICBsYWJlbDogJ0RlbGV0ZWQnLFxyXG4gICAgICBjb2xvcjogJyNGRjRENEQnXHJcbiAgICB9LFxyXG4gICAgRHJhZnQ6IHtcclxuICAgICAgbGFiZWw6ICcjRDhFMkU5JyxcclxuICAgICAgY29sb3I6ICdncmVlbidcclxuICAgIH0sXHJcbiAgICBTY2hlZHVsZToge1xyXG4gICAgICBsYWJlbDogJ1NjaGVkdWxlJyxcclxuICAgICAgY29sb3I6ICcjRkZBQTA0J1xyXG4gICAgfSxcclxuICAgIFByZXZpZXc6IHtcclxuICAgICAgbGFiZWw6ICdQcmV2aWV3JyxcclxuICAgICAgY29sb3I6ICcjRkZBQTA0J1xyXG4gICAgfVxyXG4gIH07XHJcblxyXG4gIGNvbnN0cnVjdG9yKCkge1xyXG4gICAgLy9cclxuICB9XHJcbn1cclxuIiwiPGRpdiBjbGFzcz1cInN0YXR1cy1pbmRpY2F0b3JcIj5cclxuICA8ZGl2IGNsYXNzPVwic3RhdHVzLWluZGljYXRvcl9fZG90XCJcclxuICAgICAgIFtuZ1N0eWxlXT1cIntiYWNrZ3JvdW5kOiBvcHRpb25bc3RhdHVzXS5jb2xvcn1cIj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cInN0YXR1cy1pbmRpY2F0b3JfX2xhYmVsXCI+XHJcbiAgICB7eyBvcHRpb25bc3RhdHVzXS5sYWJlbCB9fVxyXG4gIDwvZGl2PlxyXG48L2Rpdj5cclxuIl19