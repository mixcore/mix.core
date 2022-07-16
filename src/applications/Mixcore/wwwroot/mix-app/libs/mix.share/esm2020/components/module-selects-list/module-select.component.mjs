import { DragDropModule } from '@angular/cdk/drag-drop';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { BehaviorSubject, switchMap } from 'rxjs';
import { MixModuleApiService } from '../../services/api/mix-module-api.service';
import { ShareModule } from '../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "../../services/api/mix-module-api.service";
import * as i2 from "@angular/common";
import * as i3 from "@taiga-ui/core";
import * as i4 from "@taiga-ui/kit";
import * as i5 from "@angular/cdk/drag-drop";
function MixModuleSelectComponent_div_0_div_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 4);
    i0.ɵɵelement(1, "tui-toggle", 5);
    i0.ɵɵelementStart(2, "div", 6);
    i0.ɵɵtext(3);
    i0.ɵɵelementEnd()();
} if (rf & 2) {
    const module_r3 = ctx.$implicit;
    i0.ɵɵadvance(3);
    i0.ɵɵtextInterpolate1(" ", module_r3.title, " ");
} }
function MixModuleSelectComponent_div_0_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 1)(1, "button", 2);
    i0.ɵɵtext(2, " Create new module ");
    i0.ɵɵelementEnd();
    i0.ɵɵtemplate(3, MixModuleSelectComponent_div_0_div_3_Template, 4, 1, "div", 3);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const result_r1 = ctx.ngIf;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("size", "s");
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("ngForOf", result_r1.items);
} }
export class MixModuleSelectComponent {
    constructor(moduleApi) {
        this.moduleApi = moduleApi;
        this.filter$ = new BehaviorSubject({
            pageSize: 10,
            pageIndex: 0,
            keyword: ''
        });
        this.data$ = this.filter$.pipe(switchMap(filter => this.moduleApi.getModules(filter)));
    }
}
MixModuleSelectComponent.ɵfac = function MixModuleSelectComponent_Factory(t) { return new (t || MixModuleSelectComponent)(i0.ɵɵdirectiveInject(i1.MixModuleApiService)); };
MixModuleSelectComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixModuleSelectComponent, selectors: [["mix-module-select"]], standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 2, vars: 3, consts: [["class", "mix-module-select", "cdkDropList", "", 4, "ngIf"], ["cdkDropList", "", 1, "mix-module-select"], ["tuiButton", "", 1, "mix-module-select__create-new", 3, "size"], ["class", "mix-module-select__item", "cdkDrag", "", 4, "ngFor", "ngForOf"], ["cdkDrag", "", 1, "mix-module-select__item"], ["size", "l", 1, "tui-space_left-1"], [1, "title"]], template: function MixModuleSelectComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵtemplate(0, MixModuleSelectComponent_div_0_Template, 4, 2, "div", 0);
        i0.ɵɵpipe(1, "async");
    } if (rf & 2) {
        i0.ɵɵproperty("ngIf", i0.ɵɵpipeBind1(1, 1, ctx.data$));
    } }, dependencies: [ShareModule, i2.NgForOf, i2.NgIf, i3.TuiButtonComponent, i4.TuiToggleComponent, i2.AsyncPipe, DragDropModule, i5.CdkDropList, i5.CdkDrag], styles: [".mix-module-select[_ngcontent-%COMP%]{width:100%}.mix-module-select__create-new[_ngcontent-%COMP%]{margin-bottom:10px}.mix-module-select__item[_ngcontent-%COMP%]{padding:10px;width:100%;border-radius:var(--mix-border-radius-01);border:1px solid var(--tui-base-04);display:flex;align-items:center}.mix-module-select__item[_ngcontent-%COMP%]   .title[_ngcontent-%COMP%]{margin-left:10px}"], changeDetection: 0 });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixModuleSelectComponent, [{
        type: Component,
        args: [{ selector: 'mix-module-select', changeDetection: ChangeDetectionStrategy.OnPush, imports: [ShareModule, DragDropModule], standalone: true, template: "<div *ngIf=\"data$ | async as result\"\r\n     class=\"mix-module-select\"\r\n     cdkDropList>\r\n  <button class=\"mix-module-select__create-new\"\r\n          [size]=\"'s'\"\r\n          tuiButton> Create new module </button>\r\n  <div *ngFor=\"let module of result.items\"\r\n       class=\"mix-module-select__item\"\r\n       cdkDrag>\r\n    <tui-toggle class=\"tui-space_left-1\"\r\n                size=\"l\"></tui-toggle>\r\n    <div class=\"title\">\r\n      {{ module.title }}\r\n    </div>\r\n  </div>\r\n</div>\r\n", styles: [".mix-module-select{width:100%}.mix-module-select__create-new{margin-bottom:10px}.mix-module-select__item{padding:10px;width:100%;border-radius:var(--mix-border-radius-01);border:1px solid var(--tui-base-04);display:flex;align-items:center}.mix-module-select__item .title{margin-left:10px}\n"] }]
    }], function () { return [{ type: i1.MixModuleApiService }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kdWxlLXNlbGVjdC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL2NvbXBvbmVudHMvbW9kdWxlLXNlbGVjdHMtbGlzdC9tb2R1bGUtc2VsZWN0LmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9tb2R1bGUtc2VsZWN0cy1saXN0L21vZHVsZS1zZWxlY3QuY29tcG9uZW50Lmh0bWwiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBQ3hELE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFbkUsT0FBTyxFQUFFLGVBQWUsRUFBRSxTQUFTLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFFbEQsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sMkNBQTJDLENBQUM7QUFDaEYsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLG9CQUFvQixDQUFDOzs7Ozs7OztJQ0EvQyw4QkFFYTtJQUNYLGdDQUNrQztJQUNsQyw4QkFBbUI7SUFDakIsWUFDRjtJQUFBLGlCQUFNLEVBQUE7OztJQURKLGVBQ0Y7SUFERSxnREFDRjs7O0lBYkosOEJBRWlCLGdCQUFBO0lBR0ksbUNBQWtCO0lBQUEsaUJBQVM7SUFDOUMsK0VBUU07SUFDUixpQkFBTTs7O0lBWEksZUFBWTtJQUFaLDBCQUFZO0lBRUksZUFBZTtJQUFmLHlDQUFlOztBRFV6QyxNQUFNLE9BQU8sd0JBQXdCO0lBWW5DLFlBQW9CLFNBQThCO1FBQTlCLGNBQVMsR0FBVCxTQUFTLENBQXFCO1FBWDNDLFlBQU8sR0FDWixJQUFJLGVBQWUsQ0FBeUI7WUFDMUMsUUFBUSxFQUFFLEVBQUU7WUFDWixTQUFTLEVBQUUsQ0FBQztZQUNaLE9BQU8sRUFBRSxFQUFFO1NBQ1osQ0FBQyxDQUFDO1FBRUUsVUFBSyxHQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUM5QixTQUFTLENBQUMsTUFBTSxDQUFDLEVBQUUsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLFVBQVUsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUN2RCxDQUFDO0lBRW1ELENBQUM7O2dHQVozQyx3QkFBd0I7MkVBQXhCLHdCQUF3QjtRQ2hCckMseUVBZU07OztRQWZBLHNEQUFvQjt3QkRhZCxXQUFXLG1GQUFFLGNBQWM7dUZBRzFCLHdCQUF3QjtjQVJwQyxTQUFTOzJCQUNFLG1CQUFtQixtQkFHWix1QkFBdUIsQ0FBQyxNQUFNLFdBQ3RDLENBQUMsV0FBVyxFQUFFLGNBQWMsQ0FBQyxjQUMxQixJQUFJIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRHJhZ0Ryb3BNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jZGsvZHJhZy1kcm9wJztcclxuaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3ksIENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBQYWdpbmF0aW9uUmVxdWVzdE1vZGVsIH0gZnJvbSAnQG1peC1zcGEvbWl4LmxpYic7XHJcbmltcG9ydCB7IEJlaGF2aW9yU3ViamVjdCwgc3dpdGNoTWFwIH0gZnJvbSAncnhqcyc7XHJcblxyXG5pbXBvcnQgeyBNaXhNb2R1bGVBcGlTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvYXBpL21peC1tb2R1bGUtYXBpLnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBTaGFyZU1vZHVsZSB9IGZyb20gJy4uLy4uL3NoYXJlLm1vZHVsZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC1tb2R1bGUtc2VsZWN0JyxcclxuICB0ZW1wbGF0ZVVybDogJy4vbW9kdWxlLXNlbGVjdC5jb21wb25lbnQuaHRtbCcsXHJcbiAgc3R5bGVVcmxzOiBbJy4vbW9kdWxlLXNlbGVjdC5jb21wb25lbnQuc2NzcyddLFxyXG4gIGNoYW5nZURldGVjdGlvbjogQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3kuT25QdXNoLFxyXG4gIGltcG9ydHM6IFtTaGFyZU1vZHVsZSwgRHJhZ0Ryb3BNb2R1bGVdLFxyXG4gIHN0YW5kYWxvbmU6IHRydWVcclxufSlcclxuZXhwb3J0IGNsYXNzIE1peE1vZHVsZVNlbGVjdENvbXBvbmVudCB7XHJcbiAgcHVibGljIGZpbHRlciQ6IEJlaGF2aW9yU3ViamVjdDxQYWdpbmF0aW9uUmVxdWVzdE1vZGVsPiA9XHJcbiAgICBuZXcgQmVoYXZpb3JTdWJqZWN0PFBhZ2luYXRpb25SZXF1ZXN0TW9kZWw+KHtcclxuICAgICAgcGFnZVNpemU6IDEwLFxyXG4gICAgICBwYWdlSW5kZXg6IDAsXHJcbiAgICAgIGtleXdvcmQ6ICcnXHJcbiAgICB9KTtcclxuXHJcbiAgcHVibGljIGRhdGEkID0gdGhpcy5maWx0ZXIkLnBpcGUoXHJcbiAgICBzd2l0Y2hNYXAoZmlsdGVyID0+IHRoaXMubW9kdWxlQXBpLmdldE1vZHVsZXMoZmlsdGVyKSlcclxuICApO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIG1vZHVsZUFwaTogTWl4TW9kdWxlQXBpU2VydmljZSkge31cclxufVxyXG4iLCI8ZGl2ICpuZ0lmPVwiZGF0YSQgfCBhc3luYyBhcyByZXN1bHRcIlxyXG4gICAgIGNsYXNzPVwibWl4LW1vZHVsZS1zZWxlY3RcIlxyXG4gICAgIGNka0Ryb3BMaXN0PlxyXG4gIDxidXR0b24gY2xhc3M9XCJtaXgtbW9kdWxlLXNlbGVjdF9fY3JlYXRlLW5ld1wiXHJcbiAgICAgICAgICBbc2l6ZV09XCIncydcIlxyXG4gICAgICAgICAgdHVpQnV0dG9uPiBDcmVhdGUgbmV3IG1vZHVsZSA8L2J1dHRvbj5cclxuICA8ZGl2ICpuZ0Zvcj1cImxldCBtb2R1bGUgb2YgcmVzdWx0Lml0ZW1zXCJcclxuICAgICAgIGNsYXNzPVwibWl4LW1vZHVsZS1zZWxlY3RfX2l0ZW1cIlxyXG4gICAgICAgY2RrRHJhZz5cclxuICAgIDx0dWktdG9nZ2xlIGNsYXNzPVwidHVpLXNwYWNlX2xlZnQtMVwiXHJcbiAgICAgICAgICAgICAgICBzaXplPVwibFwiPjwvdHVpLXRvZ2dsZT5cclxuICAgIDxkaXYgY2xhc3M9XCJ0aXRsZVwiPlxyXG4gICAgICB7eyBtb2R1bGUudGl0bGUgfX1cclxuICAgIDwvZGl2PlxyXG4gIDwvZGl2PlxyXG48L2Rpdj5cclxuIl19