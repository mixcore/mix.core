import { Component, Input } from '@angular/core';
import { slideAnimation } from '../../animations/slide';
import { PortalSidebarControlService } from '../../services';
import { MixModuleApiService } from '../../services/api/mix-module-api.service';
import { ShareModule } from '../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "../../services";
import * as i2 from "../../services/api/mix-module-api.service";
import * as i3 from "@angular/common";
import * as i4 from "@taiga-ui/core";
import * as i5 from "angular-tabler-icons";
function MixModuleDetailComponent_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r2 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "div", 7)(1, "div", 8)(2, "button", 9);
    i0.ɵɵlistener("click", function MixModuleDetailComponent_div_1_Template_button_click_2_listener() { i0.ɵɵrestoreView(_r2); const ctx_r1 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r1.closeSidebar()); });
    i0.ɵɵelement(3, "i-tabler", 10);
    i0.ɵɵelementEnd()()();
} if (rf & 2) {
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("appearance", "icon")("size", "xs");
} }
const _c0 = function (a0) { return { "--quick-mode": a0 }; };
export class MixModuleDetailComponent {
    constructor(sidebarControl, moduleApi) {
        this.sidebarControl = sidebarControl;
        this.moduleApi = moduleApi;
        this.mode = 'FullPage';
        this.moduleId = 0;
    }
    closeSidebar() {
        this.sidebarControl.hide();
    }
    ngOnInit() {
        this.moduleApi.getModuleById(this.moduleId).subscribe(module => {
            console.log(module);
        });
    }
}
MixModuleDetailComponent.ɵfac = function MixModuleDetailComponent_Factory(t) { return new (t || MixModuleDetailComponent)(i0.ɵɵdirectiveInject(i1.PortalSidebarControlService), i0.ɵɵdirectiveInject(i2.MixModuleApiService)); };
MixModuleDetailComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixModuleDetailComponent, selectors: [["mix-module-detail"]], inputs: { mode: "mode", moduleId: "moduleId" }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 9, vars: 6, consts: [[1, "module-detail", 3, "ngClass"], ["class", "module-detail__header", 4, "ngIf"], [1, "module-detail__content"], [1, "module-detail__content-left-side"], [1, "toolbar"], [1, "workspace"], [1, "module-detail__content-right-side"], [1, "module-detail__header"], [1, "action"], ["tuiButton", "", 3, "appearance", "size", "click"], ["name", "square-x"]], template: function MixModuleDetailComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0);
        i0.ɵɵtemplate(1, MixModuleDetailComponent_div_1_Template, 4, 2, "div", 1);
        i0.ɵɵelementStart(2, "div", 2)(3, "div", 3)(4, "div", 4);
        i0.ɵɵtext(5, " Toolbar ");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(6, "div", 5);
        i0.ɵɵtext(7);
        i0.ɵɵelementEnd()();
        i0.ɵɵelement(8, "div", 6);
        i0.ɵɵelementEnd()();
    } if (rf & 2) {
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(4, _c0, ctx.mode === "Quickly"))("@enterAnimation", undefined);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.mode === "Quickly");
        i0.ɵɵadvance(6);
        i0.ɵɵtextInterpolate1(" ", ctx.moduleId, " ");
    } }, dependencies: [ShareModule, i3.NgClass, i3.NgIf, i4.TuiButtonComponent, i5.TablerIconComponent], styles: [".module-detail[_ngcontent-%COMP%]{width:100%;height:100%}.module-detail.--quick-mode[_ngcontent-%COMP%]{width:60vw;display:flex;position:relative;flex-direction:column;border-top-left-radius:10px;background-color:var(--tui-base-01);border-left:1px sold var(--tui-base-04);box-shadow:#3c40434d 0 1px 2px,#3c404326 0 2px 6px 2px}.module-detail__header[_ngcontent-%COMP%]{display:flex;align-items:center;justify-content:flex-start;width:100%;padding:10px 10px 10px 20px;border-bottom:1px solid var(--tui-base-04)}.module-detail__header[_ngcontent-%COMP%]   .action[_ngcontent-%COMP%]{margin-left:auto}.module-detail__content[_ngcontent-%COMP%]{width:100%;height:100%;display:flex}.module-detail__content-left-side[_ngcontent-%COMP%]{width:100%}.module-detail__content-left-side[_ngcontent-%COMP%]   .toolbar[_ngcontent-%COMP%]{height:var(--mix-sub-header-height);background-color:var(--tui-base-03);display:flex;align-items:center;justify-content:flex-end;padding:0 20px;margin-bottom:5px}.module-detail__content-left-side[_ngcontent-%COMP%]   .workspace[_ngcontent-%COMP%]{padding:0 20px}.module-detail__content-right-side[_ngcontent-%COMP%]{width:30%;height:100%;border-left:1px solid var(--tui-base-04)}"], data: { animation: [slideAnimation] } });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixModuleDetailComponent, [{
        type: Component,
        args: [{ selector: 'mix-module-detail', standalone: true, imports: [ShareModule], animations: [slideAnimation], template: "<div class=\"module-detail\"\r\n     [ngClass]=\"{'--quick-mode': mode === 'Quickly'}\"\r\n     [@enterAnimation]>\r\n  <div *ngIf=\"mode === 'Quickly'\"\r\n       class=\"module-detail__header\">\r\n    <div class=\"action\">\r\n      <button [appearance]=\"'icon'\"\r\n              [size]=\"'xs'\"\r\n              (click)=\"closeSidebar()\"\r\n              tuiButton>\r\n        <i-tabler name=\"square-x\"></i-tabler>\r\n      </button>\r\n    </div>\r\n  </div>\r\n\r\n  <div class=\"module-detail__content\">\r\n    <div class=\"module-detail__content-left-side\">\r\n      <div class=\"toolbar\">\r\n        Toolbar\r\n      </div>\r\n\r\n      <div class=\"workspace\">\r\n        {{ moduleId }}\r\n      </div>\r\n    </div>\r\n\r\n    <div class=\"module-detail__content-right-side\">\r\n      <!-- ABC -->\r\n    </div>\r\n  </div>\r\n</div>\r\n", styles: [".module-detail{width:100%;height:100%}.module-detail.--quick-mode{width:60vw;display:flex;position:relative;flex-direction:column;border-top-left-radius:10px;background-color:var(--tui-base-01);border-left:1px sold var(--tui-base-04);box-shadow:#3c40434d 0 1px 2px,#3c404326 0 2px 6px 2px}.module-detail__header{display:flex;align-items:center;justify-content:flex-start;width:100%;padding:10px 10px 10px 20px;border-bottom:1px solid var(--tui-base-04)}.module-detail__header .action{margin-left:auto}.module-detail__content{width:100%;height:100%;display:flex}.module-detail__content-left-side{width:100%}.module-detail__content-left-side .toolbar{height:var(--mix-sub-header-height);background-color:var(--tui-base-03);display:flex;align-items:center;justify-content:flex-end;padding:0 20px;margin-bottom:5px}.module-detail__content-left-side .workspace{padding:0 20px}.module-detail__content-right-side{width:30%;height:100%;border-left:1px solid var(--tui-base-04)}\n"] }]
    }], function () { return [{ type: i1.PortalSidebarControlService }, { type: i2.MixModuleApiService }]; }, { mode: [{
            type: Input
        }], moduleId: [{
            type: Input
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LW1vZHVsZS1kZXRhaWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21peC1tb2R1bGUtZGV0YWlsL21peC1tb2R1bGUtZGV0YWlsLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtbW9kdWxlLWRldGFpbC9taXgtbW9kdWxlLWRldGFpbC5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUV6RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDeEQsT0FBTyxFQUFFLDJCQUEyQixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDN0QsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sMkNBQTJDLENBQUM7QUFDaEYsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLG9CQUFvQixDQUFDOzs7Ozs7Ozs7SUNGL0MsOEJBQ21DLGFBQUEsZ0JBQUE7SUFJdkIscUtBQVMsZUFBQSxxQkFBYyxDQUFBLElBQUM7SUFFOUIsK0JBQXFDO0lBQ3ZDLGlCQUFTLEVBQUEsRUFBQTs7SUFMRCxlQUFxQjtJQUFyQixtQ0FBcUIsY0FBQTs7O0FEU25DLE1BQU0sT0FBTyx3QkFBd0I7SUFJbkMsWUFDVSxjQUEyQyxFQUMzQyxTQUE4QjtRQUQ5QixtQkFBYyxHQUFkLGNBQWMsQ0FBNkI7UUFDM0MsY0FBUyxHQUFULFNBQVMsQ0FBcUI7UUFMeEIsU0FBSSxHQUEyQixVQUFVLENBQUM7UUFDMUMsYUFBUSxHQUFHLENBQUMsQ0FBQztJQUsxQixDQUFDO0lBRUcsWUFBWTtRQUNqQixJQUFJLENBQUMsY0FBYyxDQUFDLElBQUksRUFBRSxDQUFDO0lBQzdCLENBQUM7SUFFTSxRQUFRO1FBQ2IsSUFBSSxDQUFDLFNBQVMsQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDLFNBQVMsQ0FBQyxNQUFNLENBQUMsRUFBRTtZQUM3RCxPQUFPLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO1FBQ3RCLENBQUMsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7Z0dBakJVLHdCQUF3QjsyRUFBeEIsd0JBQXdCO1FDZnJDLDhCQUV1QjtRQUNyQix5RUFVTTtRQUVOLDhCQUFvQyxhQUFBLGFBQUE7UUFHOUIseUJBQ0Y7UUFBQSxpQkFBTTtRQUVOLDhCQUF1QjtRQUNyQixZQUNGO1FBQUEsaUJBQU0sRUFBQTtRQUdSLHlCQUVNO1FBQ1IsaUJBQU0sRUFBQTs7UUE1QkgsNEVBQWdELDhCQUFBO1FBRTdDLGVBQXdCO1FBQXhCLDZDQUF3QjtRQW1CeEIsZUFDRjtRQURFLDZDQUNGO3dCRFhNLFdBQVcsMnhDQUNULENBQUMsY0FBYyxDQUFDO3VGQUVqQix3QkFBd0I7Y0FScEMsU0FBUzsyQkFDRSxtQkFBbUIsY0FHakIsSUFBSSxXQUNQLENBQUMsV0FBVyxDQUFDLGNBQ1YsQ0FBQyxjQUFjLENBQUM7Z0hBR1osSUFBSTtrQkFBbkIsS0FBSztZQUNVLFFBQVE7a0JBQXZCLEtBQUsiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIElucHV0LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbmltcG9ydCB7IHNsaWRlQW5pbWF0aW9uIH0gZnJvbSAnLi4vLi4vYW5pbWF0aW9ucy9zbGlkZSc7XHJcbmltcG9ydCB7IFBvcnRhbFNpZGViYXJDb250cm9sU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzJztcclxuaW1wb3J0IHsgTWl4TW9kdWxlQXBpU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2FwaS9taXgtbW9kdWxlLWFwaS5zZXJ2aWNlJztcclxuaW1wb3J0IHsgU2hhcmVNb2R1bGUgfSBmcm9tICcuLi8uLi9zaGFyZS5tb2R1bGUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtbW9kdWxlLWRldGFpbCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL21peC1tb2R1bGUtZGV0YWlsLmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi9taXgtbW9kdWxlLWRldGFpbC5jb21wb25lbnQuc2NzcyddLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW1NoYXJlTW9kdWxlXSxcclxuICBhbmltYXRpb25zOiBbc2xpZGVBbmltYXRpb25dXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBNaXhNb2R1bGVEZXRhaWxDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBtb2RlOiAnUXVpY2tseScgfCAnRnVsbFBhZ2UnID0gJ0Z1bGxQYWdlJztcclxuICBASW5wdXQoKSBwdWJsaWMgbW9kdWxlSWQgPSAwO1xyXG5cclxuICBjb25zdHJ1Y3RvcihcclxuICAgIHByaXZhdGUgc2lkZWJhckNvbnRyb2w6IFBvcnRhbFNpZGViYXJDb250cm9sU2VydmljZSxcclxuICAgIHByaXZhdGUgbW9kdWxlQXBpOiBNaXhNb2R1bGVBcGlTZXJ2aWNlXHJcbiAgKSB7fVxyXG5cclxuICBwdWJsaWMgY2xvc2VTaWRlYmFyKCk6IHZvaWQge1xyXG4gICAgdGhpcy5zaWRlYmFyQ29udHJvbC5oaWRlKCk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgbmdPbkluaXQoKTogdm9pZCB7XHJcbiAgICB0aGlzLm1vZHVsZUFwaS5nZXRNb2R1bGVCeUlkKHRoaXMubW9kdWxlSWQpLnN1YnNjcmliZShtb2R1bGUgPT4ge1xyXG4gICAgICBjb25zb2xlLmxvZyhtb2R1bGUpO1xyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiIsIjxkaXYgY2xhc3M9XCJtb2R1bGUtZGV0YWlsXCJcclxuICAgICBbbmdDbGFzc109XCJ7Jy0tcXVpY2stbW9kZSc6IG1vZGUgPT09ICdRdWlja2x5J31cIlxyXG4gICAgIFtAZW50ZXJBbmltYXRpb25dPlxyXG4gIDxkaXYgKm5nSWY9XCJtb2RlID09PSAnUXVpY2tseSdcIlxyXG4gICAgICAgY2xhc3M9XCJtb2R1bGUtZGV0YWlsX19oZWFkZXJcIj5cclxuICAgIDxkaXYgY2xhc3M9XCJhY3Rpb25cIj5cclxuICAgICAgPGJ1dHRvbiBbYXBwZWFyYW5jZV09XCInaWNvbidcIlxyXG4gICAgICAgICAgICAgIFtzaXplXT1cIid4cydcIlxyXG4gICAgICAgICAgICAgIChjbGljayk9XCJjbG9zZVNpZGViYXIoKVwiXHJcbiAgICAgICAgICAgICAgdHVpQnV0dG9uPlxyXG4gICAgICAgIDxpLXRhYmxlciBuYW1lPVwic3F1YXJlLXhcIj48L2ktdGFibGVyPlxyXG4gICAgICA8L2J1dHRvbj5cclxuICAgIDwvZGl2PlxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2IGNsYXNzPVwibW9kdWxlLWRldGFpbF9fY29udGVudFwiPlxyXG4gICAgPGRpdiBjbGFzcz1cIm1vZHVsZS1kZXRhaWxfX2NvbnRlbnQtbGVmdC1zaWRlXCI+XHJcbiAgICAgIDxkaXYgY2xhc3M9XCJ0b29sYmFyXCI+XHJcbiAgICAgICAgVG9vbGJhclxyXG4gICAgICA8L2Rpdj5cclxuXHJcbiAgICAgIDxkaXYgY2xhc3M9XCJ3b3Jrc3BhY2VcIj5cclxuICAgICAgICB7eyBtb2R1bGVJZCB9fVxyXG4gICAgICA8L2Rpdj5cclxuICAgIDwvZGl2PlxyXG5cclxuICAgIDxkaXYgY2xhc3M9XCJtb2R1bGUtZGV0YWlsX19jb250ZW50LXJpZ2h0LXNpZGVcIj5cclxuICAgICAgPCEtLSBBQkMgLS0+XHJcbiAgICA8L2Rpdj5cclxuICA8L2Rpdj5cclxuPC9kaXY+XHJcbiJdfQ==