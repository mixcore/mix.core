import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ShareModule } from '../../share.module';
import { MixDatabaseGraphComponent } from '../mix-database-graph/mix-database-graph.component';
import * as i0 from "@angular/core";
import * as i1 from "@angular/forms";
import * as i2 from "@taiga-ui/core";
import * as i3 from "@taiga-ui/kit";
export class MixDatabaseVisualizeComponent {
    constructor() {
        this.toolbarForm = new FormGroup({
            autoSave: new FormControl(true),
            viewMode: new FormControl('graph')
        });
        //
    }
}
MixDatabaseVisualizeComponent.ɵfac = function MixDatabaseVisualizeComponent_Factory(t) { return new (t || MixDatabaseVisualizeComponent)(); };
MixDatabaseVisualizeComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixDatabaseVisualizeComponent, selectors: [["mix-database-visualize"]], standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 19, vars: 6, consts: [[1, "mix-database-visualize"], [1, "mix-database-visualize__header", 3, "formGroup"], [1, "action"], [1, "label"], ["size", "s", "contentAlign", "right", "formControlName", "viewMode", "item", "graph"], ["size", "s", "contentAlign", "right", "formControlName", "viewMode", "item", "list", 3, "disabled"], ["formControlName", "autoSave", "size", "l"], ["tuiButton", "", 1, "action", 3, "appearance", "size"], ["tuiButton", "", 1, "action", 3, "icon", "size"], [1, "mix-database-visualize__workspace"]], template: function MixDatabaseVisualizeComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1)(2, "div", 2)(3, "div", 3);
        i0.ɵɵtext(4, " View mode: ");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(5, "tui-radio-block", 4);
        i0.ɵɵtext(6, " Graph ");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(7, "tui-radio-block", 5);
        i0.ɵɵtext(8, " List ");
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(9, "div", 2)(10, "div", 3);
        i0.ɵɵtext(11, " Auto save on edit: ");
        i0.ɵɵelementEnd();
        i0.ɵɵelement(12, "tui-toggle", 6);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(13, "button", 7);
        i0.ɵɵtext(14, "Cancel");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(15, "button", 8);
        i0.ɵɵtext(16, "Save");
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(17, "div", 9);
        i0.ɵɵelement(18, "mix-database-graph");
        i0.ɵɵelementEnd()();
    } if (rf & 2) {
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("formGroup", ctx.toolbarForm);
        i0.ɵɵadvance(6);
        i0.ɵɵproperty("disabled", true);
        i0.ɵɵadvance(6);
        i0.ɵɵproperty("appearance", "outline")("size", "s");
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("icon", "tuiIconCheckLarge")("size", "s");
    } }, dependencies: [ShareModule, i1.NgControlStatus, i1.NgControlStatusGroup, i1.FormGroupDirective, i1.FormControlName, i2.TuiButtonComponent, i3.TuiRadioBlockComponent, i3.TuiToggleComponent, MixDatabaseGraphComponent], styles: [".mix-database-visualize[_ngcontent-%COMP%]{height:100%}.mix-database-visualize__header[_ngcontent-%COMP%]{height:var(--mix-sub-header-height);border-bottom:1px solid var(--tui-base-04);display:flex;align-items:center;justify-content:flex-start;padding:10px;box-sizing:border-box;background-color:#f0f4f9}.mix-database-visualize__header[_ngcontent-%COMP%]   .action[_ngcontent-%COMP%]{margin-right:15px;display:flex;align-items:center}.mix-database-visualize__header[_ngcontent-%COMP%]   .action[_ngcontent-%COMP%]   .label[_ngcontent-%COMP%]{margin:0 5px;opacity:.7}.mix-database-visualize__workspace[_ngcontent-%COMP%]{width:100%;height:calc(100% - var(--mix-sub-header-height))}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixDatabaseVisualizeComponent, [{
        type: Component,
        args: [{ selector: 'mix-database-visualize', standalone: true, imports: [ShareModule, MixDatabaseGraphComponent], template: "<div class=\"mix-database-visualize\">\r\n  <div class=\"mix-database-visualize__header\"\r\n       [formGroup]=\"toolbarForm\">\r\n    <div class=\"action\">\r\n      <div class=\"label\">\r\n        View mode:\r\n      </div>\r\n      <tui-radio-block size=\"s\"\r\n                       contentAlign=\"right\"\r\n                       formControlName=\"viewMode\"\r\n                       item=\"graph\">\r\n        Graph\r\n      </tui-radio-block>\r\n      <tui-radio-block [disabled]=\"true\"\r\n                       size=\"s\"\r\n                       contentAlign=\"right\"\r\n                       formControlName=\"viewMode\"\r\n                       item=\"list\">\r\n        List\r\n      </tui-radio-block>\r\n    </div>\r\n\r\n    <div class=\"action\">\r\n      <div class=\"label\">\r\n        Auto save on edit:\r\n      </div>\r\n      <tui-toggle formControlName=\"autoSave\"\r\n                  size=\"l\"></tui-toggle>\r\n    </div>\r\n\r\n    <button class=\"action\"\r\n            [appearance]=\"'outline'\"\r\n            [size]=\"'s'\"\r\n            tuiButton>Cancel</button>\r\n\r\n    <button class=\"action\"\r\n            [icon]=\"'tuiIconCheckLarge'\"\r\n            [size]=\"'s'\"\r\n            tuiButton>Save</button>\r\n\r\n\r\n  </div>\r\n\r\n  <div class=\"mix-database-visualize__workspace\">\r\n    <mix-database-graph></mix-database-graph>\r\n  </div>\r\n</div>\r\n", styles: [".mix-database-visualize{height:100%}.mix-database-visualize__header{height:var(--mix-sub-header-height);border-bottom:1px solid var(--tui-base-04);display:flex;align-items:center;justify-content:flex-start;padding:10px;box-sizing:border-box;background-color:#f0f4f9}.mix-database-visualize__header .action{margin-right:15px;display:flex;align-items:center}.mix-database-visualize__header .action .label{margin:0 5px;opacity:.7}.mix-database-visualize__workspace{width:100%;height:calc(100% - var(--mix-sub-header-height))}\n"] }]
    }], function () { return []; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LWRhdGFiYXNlLXZpc3VhbGl6ZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL2NvbXBvbmVudHMvbWl4LWRhdGFiYXNlLXZpc3VhbGl6ZS9taXgtZGF0YWJhc2UtdmlzdWFsaXplLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtZGF0YWJhc2UtdmlzdWFsaXplL21peC1kYXRhYmFzZS12aXN1YWxpemUuY29tcG9uZW50Lmh0bWwiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMxQyxPQUFPLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRXhELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUNqRCxPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSxvREFBb0QsQ0FBQzs7Ozs7QUFTL0YsTUFBTSxPQUFPLDZCQUE2QjtJQU14QztRQUxPLGdCQUFXLEdBQUcsSUFBSSxTQUFTLENBQUM7WUFDakMsUUFBUSxFQUFFLElBQUksV0FBVyxDQUFDLElBQUksQ0FBQztZQUMvQixRQUFRLEVBQUUsSUFBSSxXQUFXLENBQUMsT0FBTyxDQUFDO1NBQ25DLENBQUMsQ0FBQztRQUdELEVBQUU7SUFDSixDQUFDOzswR0FSVSw2QkFBNkI7Z0ZBQTdCLDZCQUE2QjtRQ2IxQyw4QkFBb0MsYUFBQSxhQUFBLGFBQUE7UUFLNUIsNEJBQ0Y7UUFBQSxpQkFBTTtRQUNOLDBDQUc4QjtRQUM1Qix1QkFDRjtRQUFBLGlCQUFrQjtRQUNsQiwwQ0FJNkI7UUFDM0Isc0JBQ0Y7UUFBQSxpQkFBa0IsRUFBQTtRQUdwQiw4QkFBb0IsY0FBQTtRQUVoQixxQ0FDRjtRQUFBLGlCQUFNO1FBQ04saUNBQ2tDO1FBQ3BDLGlCQUFNO1FBRU4sa0NBR2tCO1FBQUEsdUJBQU07UUFBQSxpQkFBUztRQUVqQyxrQ0FHa0I7UUFBQSxxQkFBSTtRQUFBLGlCQUFTLEVBQUE7UUFLakMsK0JBQStDO1FBQzdDLHNDQUF5QztRQUMzQyxpQkFBTSxFQUFBOztRQTNDRCxlQUF5QjtRQUF6QiwyQ0FBeUI7UUFXVCxlQUFpQjtRQUFqQiwrQkFBaUI7UUFrQjVCLGVBQXdCO1FBQXhCLHNDQUF3QixhQUFBO1FBS3hCLGVBQTRCO1FBQTVCLDBDQUE0QixhQUFBO3dCRHpCNUIsV0FBVyxtS0FBRSx5QkFBeUI7dUZBRXJDLDZCQUE2QjtjQVB6QyxTQUFTOzJCQUNFLHdCQUF3QixjQUd0QixJQUFJLFdBQ1AsQ0FBQyxXQUFXLEVBQUUseUJBQXlCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgRm9ybUNvbnRyb2wsIEZvcm1Hcm91cCB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcclxuXHJcbmltcG9ydCB7IFNoYXJlTW9kdWxlIH0gZnJvbSAnLi4vLi4vc2hhcmUubW9kdWxlJztcclxuaW1wb3J0IHsgTWl4RGF0YWJhc2VHcmFwaENvbXBvbmVudCB9IGZyb20gJy4uL21peC1kYXRhYmFzZS1ncmFwaC9taXgtZGF0YWJhc2UtZ3JhcGguY29tcG9uZW50JztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnbWl4LWRhdGFiYXNlLXZpc3VhbGl6ZScsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL21peC1kYXRhYmFzZS12aXN1YWxpemUuY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL21peC1kYXRhYmFzZS12aXN1YWxpemUuY29tcG9uZW50LnNjc3MnXSxcclxuICBzdGFuZGFsb25lOiB0cnVlLFxyXG4gIGltcG9ydHM6IFtTaGFyZU1vZHVsZSwgTWl4RGF0YWJhc2VHcmFwaENvbXBvbmVudF1cclxufSlcclxuZXhwb3J0IGNsYXNzIE1peERhdGFiYXNlVmlzdWFsaXplQ29tcG9uZW50IHtcclxuICBwdWJsaWMgdG9vbGJhckZvcm0gPSBuZXcgRm9ybUdyb3VwKHtcclxuICAgIGF1dG9TYXZlOiBuZXcgRm9ybUNvbnRyb2wodHJ1ZSksXHJcbiAgICB2aWV3TW9kZTogbmV3IEZvcm1Db250cm9sKCdncmFwaCcpXHJcbiAgfSk7XHJcblxyXG4gIGNvbnN0cnVjdG9yKCkge1xyXG4gICAgLy9cclxuICB9XHJcbn1cclxuIiwiPGRpdiBjbGFzcz1cIm1peC1kYXRhYmFzZS12aXN1YWxpemVcIj5cclxuICA8ZGl2IGNsYXNzPVwibWl4LWRhdGFiYXNlLXZpc3VhbGl6ZV9faGVhZGVyXCJcclxuICAgICAgIFtmb3JtR3JvdXBdPVwidG9vbGJhckZvcm1cIj5cclxuICAgIDxkaXYgY2xhc3M9XCJhY3Rpb25cIj5cclxuICAgICAgPGRpdiBjbGFzcz1cImxhYmVsXCI+XHJcbiAgICAgICAgVmlldyBtb2RlOlxyXG4gICAgICA8L2Rpdj5cclxuICAgICAgPHR1aS1yYWRpby1ibG9jayBzaXplPVwic1wiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgY29udGVudEFsaWduPVwicmlnaHRcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgIGZvcm1Db250cm9sTmFtZT1cInZpZXdNb2RlXCJcclxuICAgICAgICAgICAgICAgICAgICAgICBpdGVtPVwiZ3JhcGhcIj5cclxuICAgICAgICBHcmFwaFxyXG4gICAgICA8L3R1aS1yYWRpby1ibG9jaz5cclxuICAgICAgPHR1aS1yYWRpby1ibG9jayBbZGlzYWJsZWRdPVwidHJ1ZVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgc2l6ZT1cInNcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgIGNvbnRlbnRBbGlnbj1cInJpZ2h0XCJcclxuICAgICAgICAgICAgICAgICAgICAgICBmb3JtQ29udHJvbE5hbWU9XCJ2aWV3TW9kZVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgaXRlbT1cImxpc3RcIj5cclxuICAgICAgICBMaXN0XHJcbiAgICAgIDwvdHVpLXJhZGlvLWJsb2NrPlxyXG4gICAgPC9kaXY+XHJcblxyXG4gICAgPGRpdiBjbGFzcz1cImFjdGlvblwiPlxyXG4gICAgICA8ZGl2IGNsYXNzPVwibGFiZWxcIj5cclxuICAgICAgICBBdXRvIHNhdmUgb24gZWRpdDpcclxuICAgICAgPC9kaXY+XHJcbiAgICAgIDx0dWktdG9nZ2xlIGZvcm1Db250cm9sTmFtZT1cImF1dG9TYXZlXCJcclxuICAgICAgICAgICAgICAgICAgc2l6ZT1cImxcIj48L3R1aS10b2dnbGU+XHJcbiAgICA8L2Rpdj5cclxuXHJcbiAgICA8YnV0dG9uIGNsYXNzPVwiYWN0aW9uXCJcclxuICAgICAgICAgICAgW2FwcGVhcmFuY2VdPVwiJ291dGxpbmUnXCJcclxuICAgICAgICAgICAgW3NpemVdPVwiJ3MnXCJcclxuICAgICAgICAgICAgdHVpQnV0dG9uPkNhbmNlbDwvYnV0dG9uPlxyXG5cclxuICAgIDxidXR0b24gY2xhc3M9XCJhY3Rpb25cIlxyXG4gICAgICAgICAgICBbaWNvbl09XCIndHVpSWNvbkNoZWNrTGFyZ2UnXCJcclxuICAgICAgICAgICAgW3NpemVdPVwiJ3MnXCJcclxuICAgICAgICAgICAgdHVpQnV0dG9uPlNhdmU8L2J1dHRvbj5cclxuXHJcblxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2IGNsYXNzPVwibWl4LWRhdGFiYXNlLXZpc3VhbGl6ZV9fd29ya3NwYWNlXCI+XHJcbiAgICA8bWl4LWRhdGFiYXNlLWdyYXBoPjwvbWl4LWRhdGFiYXNlLWdyYXBoPlxyXG4gIDwvZGl2PlxyXG48L2Rpdj5cclxuIl19