import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TabControlService } from '../../../services/helper/tab-control.service';
import { ShareModule } from '../../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "../../../services/helper/tab-control.service";
import * as i2 from "@angular/router";
import * as i3 from "@angular/common";
import * as i4 from "@taiga-ui/cdk";
const _c0 = function (a0) { return { "--active": a0 }; };
function TabControlDialogComponent_div_0_ng_container_1_div_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 4);
    i0.ɵɵtext(1);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const tab_r5 = ctx.$implicit;
    const i_r6 = ctx.index;
    const index_r3 = i0.ɵɵnextContext().tuiLet;
    i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(2, _c0, index_r3 === i_r6));
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", tab_r5.path.replace("/", ""), " ");
} }
function TabControlDialogComponent_div_0_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, TabControlDialogComponent_div_0_ng_container_1_div_1_Template, 2, 4, "div", 3);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const items_r1 = i0.ɵɵnextContext().ngIf;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngForOf", items_r1);
} }
function TabControlDialogComponent_div_0_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 1);
    i0.ɵɵtemplate(1, TabControlDialogComponent_div_0_ng_container_1_Template, 2, 1, "ng-container", 2);
    i0.ɵɵpipe(2, "async");
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const ctx_r0 = i0.ɵɵnextContext();
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiLet", i0.ɵɵpipeBind1(2, 1, ctx_r0.index$));
} }
export class TabControlDialogComponent {
    constructor(tabControl, router) {
        this.tabControl = tabControl;
        this.router = router;
        this.item$ = this.tabControl.navControl$;
        this.index$ = this.tabControl.index$;
    }
    ngOnDestroy() {
        const tabControls = this.item$.getValue();
        const index = this.index$.getValue();
        if (index > tabControls.length)
            return;
        if (tabControls[index].path == this.router.url)
            return;
        this.router.navigateByUrl(tabControls[index].path);
        this.tabControl.unTab();
    }
}
TabControlDialogComponent.ɵfac = function TabControlDialogComponent_Factory(t) { return new (t || TabControlDialogComponent)(i0.ɵɵdirectiveInject(i1.TabControlService), i0.ɵɵdirectiveInject(i2.Router)); };
TabControlDialogComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: TabControlDialogComponent, selectors: [["mix-tab-control-dialog"]], standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 2, vars: 3, consts: [["class", "tab-control", 4, "ngIf"], [1, "tab-control"], [4, "tuiLet"], ["class", "tab-control__item", 3, "ngClass", 4, "ngFor", "ngForOf"], [1, "tab-control__item", 3, "ngClass"]], template: function TabControlDialogComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵtemplate(0, TabControlDialogComponent_div_0_Template, 3, 3, "div", 0);
        i0.ɵɵpipe(1, "async");
    } if (rf & 2) {
        i0.ɵɵproperty("ngIf", i0.ɵɵpipeBind1(1, 1, ctx.item$));
    } }, dependencies: [ShareModule, i3.NgClass, i3.NgForOf, i3.NgIf, i4.TuiLetDirective, i3.AsyncPipe], styles: [".tab-control[_ngcontent-%COMP%]{display:flex;flex-direction:column}.tab-control__item[_ngcontent-%COMP%]{display:flex;min-width:400px;margin:5px;padding:15px;border-radius:10px}.tab-control__item.--active[_ngcontent-%COMP%]{box-shadow:#0000003d 0 3px 8px}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(TabControlDialogComponent, [{
        type: Component,
        args: [{ selector: 'mix-tab-control-dialog', standalone: true, imports: [ShareModule], template: "<div *ngIf=\"item$ | async as items\"\r\n     class=\"tab-control\">\r\n  <ng-container *tuiLet=\"index$ | async as index\">\r\n    <div *ngFor=\"let tab of items; let i = index\"\r\n         class=\"tab-control__item\"\r\n         [ngClass]=\"{'--active': index === i}\">\r\n      {{ tab.path.replace('/', '') }}\r\n    </div>\r\n  </ng-container>\r\n</div>\r\n", styles: [".tab-control{display:flex;flex-direction:column}.tab-control__item{display:flex;min-width:400px;margin:5px;padding:15px;border-radius:10px}.tab-control__item.--active{box-shadow:#0000003d 0 3px 8px}\n"] }]
    }], function () { return [{ type: i1.TabControlService }, { type: i2.Router }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFiLWNvbnRyb2wuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL2RpYWxvZ3MvdGFiLWNvbnRyb2wvdGFiLWNvbnRyb2wuY29tcG9uZW50LnRzIiwiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL2RpYWxvZ3MvdGFiLWNvbnRyb2wvdGFiLWNvbnRyb2wuY29tcG9uZW50Lmh0bWwiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFNBQVMsRUFBYSxNQUFNLGVBQWUsQ0FBQztBQUNyRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFFekMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sOENBQThDLENBQUM7QUFDakYsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHVCQUF1QixDQUFDOzs7Ozs7OztJQ0RoRCw4QkFFMkM7SUFDekMsWUFDRjtJQUFBLGlCQUFNOzs7OztJQUZELHVFQUFxQztJQUN4QyxlQUNGO0lBREUsNkRBQ0Y7OztJQUxGLDZCQUFnRDtJQUM5QywrRkFJTTtJQUNSLDBCQUFlOzs7SUFMUSxlQUFVO0lBQVYsa0NBQVU7OztJQUhuQyw4QkFDeUI7SUFDdkIsa0dBTWU7O0lBQ2pCLGlCQUFNOzs7SUFQVyxlQUF1QjtJQUF2Qiw0REFBdUI7O0FEV3hDLE1BQU0sT0FBTyx5QkFBeUI7SUFJcEMsWUFBbUIsVUFBNkIsRUFBUyxNQUFjO1FBQXBELGVBQVUsR0FBVixVQUFVLENBQW1CO1FBQVMsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUhoRSxVQUFLLEdBQUcsSUFBSSxDQUFDLFVBQVUsQ0FBQyxXQUFXLENBQUM7UUFDcEMsV0FBTSxHQUFHLElBQUksQ0FBQyxVQUFVLENBQUMsTUFBTSxDQUFDO0lBRW1DLENBQUM7SUFFcEUsV0FBVztRQUNoQixNQUFNLFdBQVcsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxDQUFDO1FBQzFDLE1BQU0sS0FBSyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsUUFBUSxFQUFFLENBQUM7UUFFckMsSUFBSSxLQUFLLEdBQUcsV0FBVyxDQUFDLE1BQU07WUFBRSxPQUFPO1FBQ3ZDLElBQUksV0FBVyxDQUFDLEtBQUssQ0FBQyxDQUFDLElBQUksSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUc7WUFBRSxPQUFPO1FBRXZELElBQUksQ0FBQyxNQUFNLENBQUMsYUFBYSxDQUFDLFdBQVcsQ0FBQyxLQUFLLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQUNuRCxJQUFJLENBQUMsVUFBVSxDQUFDLEtBQUssRUFBRSxDQUFDO0lBQzFCLENBQUM7O2tHQWZVLHlCQUF5Qjs0RUFBekIseUJBQXlCO1FDYnRDLDBFQVNNOzs7UUFUQSxzREFBb0I7d0JEV2QsV0FBVzt1RkFFVix5QkFBeUI7Y0FQckMsU0FBUzsyQkFDRSx3QkFBd0IsY0FHdEIsSUFBSSxXQUNQLENBQUMsV0FBVyxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBPbkRlc3Ryb3kgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuXHJcbmltcG9ydCB7IFRhYkNvbnRyb2xTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vLi4vc2VydmljZXMvaGVscGVyL3RhYi1jb250cm9sLnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBTaGFyZU1vZHVsZSB9IGZyb20gJy4uLy4uLy4uL3NoYXJlLm1vZHVsZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC10YWItY29udHJvbC1kaWFsb2cnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi90YWItY29udHJvbC5jb21wb25lbnQuaHRtbCcsXHJcbiAgc3R5bGVVcmxzOiBbJy4vdGFiLWNvbnRyb2wuY29tcG9uZW50LnNjc3MnXSxcclxuICBzdGFuZGFsb25lOiB0cnVlLFxyXG4gIGltcG9ydHM6IFtTaGFyZU1vZHVsZV1cclxufSlcclxuZXhwb3J0IGNsYXNzIFRhYkNvbnRyb2xEaWFsb2dDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xyXG4gIHB1YmxpYyBpdGVtJCA9IHRoaXMudGFiQ29udHJvbC5uYXZDb250cm9sJDtcclxuICBwdWJsaWMgaW5kZXgkID0gdGhpcy50YWJDb250cm9sLmluZGV4JDtcclxuXHJcbiAgY29uc3RydWN0b3IocHVibGljIHRhYkNvbnRyb2w6IFRhYkNvbnRyb2xTZXJ2aWNlLCBwdWJsaWMgcm91dGVyOiBSb3V0ZXIpIHt9XHJcblxyXG4gIHB1YmxpYyBuZ09uRGVzdHJveSgpOiB2b2lkIHtcclxuICAgIGNvbnN0IHRhYkNvbnRyb2xzID0gdGhpcy5pdGVtJC5nZXRWYWx1ZSgpO1xyXG4gICAgY29uc3QgaW5kZXggPSB0aGlzLmluZGV4JC5nZXRWYWx1ZSgpO1xyXG5cclxuICAgIGlmIChpbmRleCA+IHRhYkNvbnRyb2xzLmxlbmd0aCkgcmV0dXJuO1xyXG4gICAgaWYgKHRhYkNvbnRyb2xzW2luZGV4XS5wYXRoID09IHRoaXMucm91dGVyLnVybCkgcmV0dXJuO1xyXG5cclxuICAgIHRoaXMucm91dGVyLm5hdmlnYXRlQnlVcmwodGFiQ29udHJvbHNbaW5kZXhdLnBhdGgpO1xyXG4gICAgdGhpcy50YWJDb250cm9sLnVuVGFiKCk7XHJcbiAgfVxyXG59XHJcbiIsIjxkaXYgKm5nSWY9XCJpdGVtJCB8IGFzeW5jIGFzIGl0ZW1zXCJcclxuICAgICBjbGFzcz1cInRhYi1jb250cm9sXCI+XHJcbiAgPG5nLWNvbnRhaW5lciAqdHVpTGV0PVwiaW5kZXgkIHwgYXN5bmMgYXMgaW5kZXhcIj5cclxuICAgIDxkaXYgKm5nRm9yPVwibGV0IHRhYiBvZiBpdGVtczsgbGV0IGkgPSBpbmRleFwiXHJcbiAgICAgICAgIGNsYXNzPVwidGFiLWNvbnRyb2xfX2l0ZW1cIlxyXG4gICAgICAgICBbbmdDbGFzc109XCJ7Jy0tYWN0aXZlJzogaW5kZXggPT09IGl9XCI+XHJcbiAgICAgIHt7IHRhYi5wYXRoLnJlcGxhY2UoJy8nLCAnJykgfX1cclxuICAgIDwvZGl2PlxyXG4gIDwvbmctY29udGFpbmVyPlxyXG48L2Rpdj5cclxuIl19