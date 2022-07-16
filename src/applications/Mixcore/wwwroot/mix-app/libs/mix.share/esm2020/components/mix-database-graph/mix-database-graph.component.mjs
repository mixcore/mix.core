import { DragDropModule } from '@angular/cdk/drag-drop';
import { ChangeDetectorRef, Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import panzoom from 'panzoom';
import { DatabaseApiService } from '../../services';
import { ShareModule } from '../../share.module';
import { MixDatabaseCardComponent } from '../mix-database-card/mix-database-card.component';
import * as i0 from "@angular/core";
import * as i1 from "../../services";
import * as i2 from "@angular/common";
const _c0 = ["canvas"];
function MixDatabaseGraphComponent_ng_container_3_Template(rf, ctx) { if (rf & 1) {
    const _r5 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "mix-database-card", 5);
    i0.ɵɵlistener("dragEnd", function MixDatabaseGraphComponent_ng_container_3_Template_mix_database_card_dragEnd_1_listener() { i0.ɵɵrestoreView(_r5); const ctx_r4 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r4.resumeZoom()); })("dragStart", function MixDatabaseGraphComponent_ng_container_3_Template_mix_database_card_dragStart_1_listener() { i0.ɵɵrestoreView(_r5); const ctx_r6 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r6.pauseZoom()); });
    i0.ɵɵelementEnd();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const item_r2 = ctx.$implicit;
    const i_r3 = ctx.index;
    const ctx_r1 = i0.ɵɵnextContext();
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("database", item_r2)("index", i_r3)("zoomScale", ctx_r1.zoomScale);
} }
export class MixDatabaseGraphComponent {
    constructor(databaseApi, cdr) {
        this.databaseApi = databaseApi;
        this.cdr = cdr;
        this.zoomScale = 1;
        this.dragPosition = { x: 20, y: 20 };
        this.databases = [];
    }
    ngOnInit() {
        this.databaseApi.getDatabase({}).subscribe(result => {
            this.databases = result.items;
            this.cdr.detectChanges();
        });
    }
    ngAfterViewInit() {
        this.graphViewCanvas = panzoom(this.canvasElement.nativeElement, {
            maxZoom: 1,
            minZoom: 0.1,
            smoothScroll: true
        });
        this.graphViewCanvas.on('transform', e => {
            const result = this.graphViewCanvas.getTransform();
            this.zoomScale = result.scale;
        });
        this.graphViewCanvas.smoothZoom(0, 0, 0.4);
    }
    pauseZoom() {
        this.graphViewCanvas.pause();
    }
    resumeZoom() {
        this.graphViewCanvas.resume();
    }
    onDragMove(value) {
        console.log(value);
    }
}
MixDatabaseGraphComponent.ɵfac = function MixDatabaseGraphComponent_Factory(t) { return new (t || MixDatabaseGraphComponent)(i0.ɵɵdirectiveInject(i1.DatabaseApiService), i0.ɵɵdirectiveInject(i0.ChangeDetectorRef)); };
MixDatabaseGraphComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixDatabaseGraphComponent, selectors: [["mix-database-graph"]], viewQuery: function MixDatabaseGraphComponent_Query(rf, ctx) { if (rf & 1) {
        i0.ɵɵviewQuery(_c0, 5);
        i0.ɵɵviewQuery(MixDatabaseCardComponent, 5);
    } if (rf & 2) {
        let _t;
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.canvasElement = _t.first);
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.databaseCard = _t);
    } }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 6, vars: 2, consts: [[1, "mix-database-graph"], [1, "canvas"], ["canvas", ""], [4, "ngFor", "ngForOf"], [1, "mix-database-graph__zoom-info"], [3, "database", "index", "zoomScale", "dragEnd", "dragStart"]], template: function MixDatabaseGraphComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1, 2);
        i0.ɵɵtemplate(3, MixDatabaseGraphComponent_ng_container_3_Template, 2, 3, "ng-container", 3);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(4, "div", 4);
        i0.ɵɵtext(5);
        i0.ɵɵelementEnd()();
    } if (rf & 2) {
        i0.ɵɵadvance(3);
        i0.ɵɵproperty("ngForOf", ctx.databases);
        i0.ɵɵadvance(2);
        i0.ɵɵtextInterpolate1(" Zoom: ", (ctx.zoomScale * 100).toFixed(2), "% | Use Mouse Wheel to zoom in/out ");
    } }, dependencies: [ShareModule, i2.NgForOf, DragDropModule, MixDatabaseCardComponent], styles: [".mix-database-graph[_ngcontent-%COMP%]{width:100%;height:100%;display:flex;overflow:hidden;position:relative;background-color:#fff;background-size:100% 1.2em;background-image:linear-gradient(0deg,transparent 79px,#abced4 79px,#abced4 81px,transparent 81px),linear-gradient(#eee .05em,transparent .05em);-pie-background:linear-gradient(0deg,transparent 79px,#abced4 79px,#abced4 81px,transparent 81px) 0 0/100% 1.2em,linear-gradient(#eee .05em,transparent .05em) 0 0/100% 1.2em #fff}.mix-database-graph[_ngcontent-%COMP%]   .canvas[_ngcontent-%COMP%]{display:flex}.mix-database-graph__item[_ngcontent-%COMP%]{width:-moz-fit-content;width:fit-content;height:-moz-fit-content;height:fit-content;position:absolute}.mix-database-graph__zoom-info[_ngcontent-%COMP%]{position:absolute;padding:5px;left:5px;bottom:5px;background-color:var(--tui-base-01);border-radius:5px;border:1px solid var(--tui-base-04);box-shadow:#0000003d 0 3px 8px}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixDatabaseGraphComponent, [{
        type: Component,
        args: [{ selector: 'mix-database-graph', standalone: true, imports: [ShareModule, DragDropModule, MixDatabaseCardComponent], template: "<div class=\"mix-database-graph\">\r\n  <div #canvas\r\n       class=\"canvas\">\r\n    <ng-container *ngFor=\"let item of databases; let i = index\">\r\n      <mix-database-card [database]=\"item\"\r\n                         [index]=\"i\"\r\n                         [zoomScale]=\"zoomScale\"\r\n                         (dragEnd)=\"resumeZoom()\"\r\n                         (dragStart)=\"pauseZoom()\"></mix-database-card>\r\n    </ng-container>\r\n  </div>\r\n\r\n  <div class=\"mix-database-graph__zoom-info\">\r\n    Zoom: {{ (zoomScale * 100).toFixed(2) }}% | Use Mouse Wheel to zoom in/out\r\n  </div>\r\n</div>\r\n", styles: [".mix-database-graph{width:100%;height:100%;display:flex;overflow:hidden;position:relative;background-color:#fff;background-size:100% 1.2em;background-image:linear-gradient(0deg,transparent 79px,#abced4 79px,#abced4 81px,transparent 81px),linear-gradient(#eee .05em,transparent .05em);-pie-background:linear-gradient(0deg,transparent 79px,#abced4 79px,#abced4 81px,transparent 81px) 0 0/100% 1.2em,linear-gradient(#eee .05em,transparent .05em) 0 0/100% 1.2em #fff}.mix-database-graph .canvas{display:flex}.mix-database-graph__item{width:-moz-fit-content;width:fit-content;height:-moz-fit-content;height:fit-content;position:absolute}.mix-database-graph__zoom-info{position:absolute;padding:5px;left:5px;bottom:5px;background-color:var(--tui-base-01);border-radius:5px;border:1px solid var(--tui-base-04);box-shadow:#0000003d 0 3px 8px}\n"] }]
    }], function () { return [{ type: i1.DatabaseApiService }, { type: i0.ChangeDetectorRef }]; }, { canvasElement: [{
            type: ViewChild,
            args: ['canvas']
        }], databaseCard: [{
            type: ViewChildren,
            args: [MixDatabaseCardComponent]
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LWRhdGFiYXNlLWdyYXBoLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtZGF0YWJhc2UtZ3JhcGgvbWl4LWRhdGFiYXNlLWdyYXBoLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtZGF0YWJhc2UtZ3JhcGgvbWl4LWRhdGFiYXNlLWdyYXBoLmNvbXBvbmVudC5odG1sIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBZSxjQUFjLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUNyRSxPQUFPLEVBRUwsaUJBQWlCLEVBQ2pCLFNBQVMsRUFDVCxVQUFVLEVBRVYsU0FBUyxFQUNULFNBQVMsRUFDVCxZQUFZLEVBQ2IsTUFBTSxlQUFlLENBQUM7QUFFdkIsT0FBTyxPQUErQixNQUFNLFNBQVMsQ0FBQztBQUV0RCxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNwRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDakQsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sa0RBQWtELENBQUM7Ozs7Ozs7SUNieEYsNkJBQTREO0lBQzFELDRDQUk2QztJQUQxQiw4TEFBVyxlQUFBLG1CQUFZLENBQUEsSUFBQyxxTEFDWCxlQUFBLGtCQUFXLENBQUEsSUFEQTtJQUNFLGlCQUFvQjtJQUNuRSwwQkFBZTs7Ozs7SUFMTSxlQUFpQjtJQUFqQixrQ0FBaUIsZUFBQSwrQkFBQTs7QURxQjFDLE1BQU0sT0FBTyx5QkFBeUI7SUFVcEMsWUFDVSxXQUErQixFQUMvQixHQUFzQjtRQUR0QixnQkFBVyxHQUFYLFdBQVcsQ0FBb0I7UUFDL0IsUUFBRyxHQUFILEdBQUcsQ0FBbUI7UUFQekIsY0FBUyxHQUFHLENBQUMsQ0FBQztRQUVkLGlCQUFZLEdBQUcsRUFBRSxDQUFDLEVBQUUsRUFBRSxFQUFFLENBQUMsRUFBRSxFQUFFLEVBQUUsQ0FBQztRQUNoQyxjQUFTLEdBQXVCLEVBQUUsQ0FBQztJQUt2QyxDQUFDO0lBRUcsUUFBUTtRQUNiLElBQUksQ0FBQyxXQUFXLENBQUMsV0FBVyxDQUFDLEVBQUUsQ0FBQyxDQUFDLFNBQVMsQ0FBQyxNQUFNLENBQUMsRUFBRTtZQUNsRCxJQUFJLENBQUMsU0FBUyxHQUFHLE1BQU0sQ0FBQyxLQUFLLENBQUM7WUFDOUIsSUFBSSxDQUFDLEdBQUcsQ0FBQyxhQUFhLEVBQUUsQ0FBQztRQUMzQixDQUFDLENBQUMsQ0FBQztJQUNMLENBQUM7SUFFTSxlQUFlO1FBQ3BCLElBQUksQ0FBQyxlQUFlLEdBQUcsT0FBTyxDQUFDLElBQUksQ0FBQyxhQUFhLENBQUMsYUFBYSxFQUFFO1lBQy9ELE9BQU8sRUFBRSxDQUFDO1lBQ1YsT0FBTyxFQUFFLEdBQUc7WUFDWixZQUFZLEVBQUUsSUFBSTtTQUNuQixDQUFDLENBQUM7UUFFSCxJQUFJLENBQUMsZUFBZSxDQUFDLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQyxDQUFDLEVBQUU7WUFDdkMsTUFBTSxNQUFNLEdBQWMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxZQUFZLEVBQUUsQ0FBQztZQUM5RCxJQUFJLENBQUMsU0FBUyxHQUFHLE1BQU0sQ0FBQyxLQUFLLENBQUM7UUFDaEMsQ0FBQyxDQUFDLENBQUM7UUFFSCxJQUFJLENBQUMsZUFBZSxDQUFDLFVBQVUsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEdBQUcsQ0FBQyxDQUFDO0lBQzdDLENBQUM7SUFFTSxTQUFTO1FBQ2QsSUFBSSxDQUFDLGVBQWUsQ0FBQyxLQUFLLEVBQUUsQ0FBQztJQUMvQixDQUFDO0lBRU0sVUFBVTtRQUNmLElBQUksQ0FBQyxlQUFlLENBQUMsTUFBTSxFQUFFLENBQUM7SUFDaEMsQ0FBQztJQUVNLFVBQVUsQ0FBQyxLQUFrQjtRQUNsQyxPQUFPLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ3JCLENBQUM7O2tHQS9DVSx5QkFBeUI7NEVBQXpCLHlCQUF5Qjs7dUJBRXRCLHdCQUF3Qjs7Ozs7O1FDM0J4Qyw4QkFBZ0MsZ0JBQUE7UUFHNUIsNEZBTWU7UUFDakIsaUJBQU07UUFFTiw4QkFBMkM7UUFDekMsWUFDRjtRQUFBLGlCQUFNLEVBQUE7O1FBWDJCLGVBQWM7UUFBZCx1Q0FBYztRQVU3QyxlQUNGO1FBREUseUdBQ0Y7d0JEU1UsV0FBVyxjQUFFLGNBQWMsRUFBRSx3QkFBd0I7dUZBRXBELHlCQUF5QjtjQVByQyxTQUFTOzJCQUNFLG9CQUFvQixjQUdsQixJQUFJLFdBQ1AsQ0FBQyxXQUFXLEVBQUUsY0FBYyxFQUFFLHdCQUF3QixDQUFDO3FHQUczQyxhQUFhO2tCQUFqQyxTQUFTO21CQUFDLFFBQVE7WUFFbkIsWUFBWTtrQkFEWCxZQUFZO21CQUFDLHdCQUF3QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENka0RyYWdNb3ZlLCBEcmFnRHJvcE1vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2Nkay9kcmFnLWRyb3AnO1xyXG5pbXBvcnQge1xyXG4gIEFmdGVyVmlld0luaXQsXHJcbiAgQ2hhbmdlRGV0ZWN0b3JSZWYsXHJcbiAgQ29tcG9uZW50LFxyXG4gIEVsZW1lbnRSZWYsXHJcbiAgT25Jbml0LFxyXG4gIFF1ZXJ5TGlzdCxcclxuICBWaWV3Q2hpbGQsXHJcbiAgVmlld0NoaWxkcmVuXHJcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE1peERhdGFiYXNlTW9kZWwgfSBmcm9tICdAbWl4LXNwYS9taXgubGliJztcclxuaW1wb3J0IHBhbnpvb20sIHsgUGFuWm9vbSwgVHJhbnNmb3JtIH0gZnJvbSAncGFuem9vbSc7XHJcblxyXG5pbXBvcnQgeyBEYXRhYmFzZUFwaVNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcyc7XHJcbmltcG9ydCB7IFNoYXJlTW9kdWxlIH0gZnJvbSAnLi4vLi4vc2hhcmUubW9kdWxlJztcclxuaW1wb3J0IHsgTWl4RGF0YWJhc2VDYXJkQ29tcG9uZW50IH0gZnJvbSAnLi4vbWl4LWRhdGFiYXNlLWNhcmQvbWl4LWRhdGFiYXNlLWNhcmQuY29tcG9uZW50JztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnbWl4LWRhdGFiYXNlLWdyYXBoJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vbWl4LWRhdGFiYXNlLWdyYXBoLmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi9taXgtZGF0YWJhc2UtZ3JhcGguY29tcG9uZW50LnNjc3MnXSxcclxuICBzdGFuZGFsb25lOiB0cnVlLFxyXG4gIGltcG9ydHM6IFtTaGFyZU1vZHVsZSwgRHJhZ0Ryb3BNb2R1bGUsIE1peERhdGFiYXNlQ2FyZENvbXBvbmVudF1cclxufSlcclxuZXhwb3J0IGNsYXNzIE1peERhdGFiYXNlR3JhcGhDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQsIEFmdGVyVmlld0luaXQge1xyXG4gIEBWaWV3Q2hpbGQoJ2NhbnZhcycpIGNhbnZhc0VsZW1lbnQhOiBFbGVtZW50UmVmO1xyXG4gIEBWaWV3Q2hpbGRyZW4oTWl4RGF0YWJhc2VDYXJkQ29tcG9uZW50KVxyXG4gIGRhdGFiYXNlQ2FyZCE6IFF1ZXJ5TGlzdDxNaXhEYXRhYmFzZUNhcmRDb21wb25lbnQ+O1xyXG5cclxuICBwdWJsaWMgem9vbVNjYWxlID0gMTtcclxuICBwdWJsaWMgZ3JhcGhWaWV3Q2FudmFzITogUGFuWm9vbTtcclxuICBwdWJsaWMgZHJhZ1Bvc2l0aW9uID0geyB4OiAyMCwgeTogMjAgfTtcclxuICBwdWJsaWMgZGF0YWJhc2VzOiBNaXhEYXRhYmFzZU1vZGVsW10gPSBbXTtcclxuXHJcbiAgY29uc3RydWN0b3IoXHJcbiAgICBwcml2YXRlIGRhdGFiYXNlQXBpOiBEYXRhYmFzZUFwaVNlcnZpY2UsXHJcbiAgICBwcml2YXRlIGNkcjogQ2hhbmdlRGV0ZWN0b3JSZWZcclxuICApIHt9XHJcblxyXG4gIHB1YmxpYyBuZ09uSW5pdCgpOiB2b2lkIHtcclxuICAgIHRoaXMuZGF0YWJhc2VBcGkuZ2V0RGF0YWJhc2Uoe30pLnN1YnNjcmliZShyZXN1bHQgPT4ge1xyXG4gICAgICB0aGlzLmRhdGFiYXNlcyA9IHJlc3VsdC5pdGVtcztcclxuICAgICAgdGhpcy5jZHIuZGV0ZWN0Q2hhbmdlcygpO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgbmdBZnRlclZpZXdJbml0KCkge1xyXG4gICAgdGhpcy5ncmFwaFZpZXdDYW52YXMgPSBwYW56b29tKHRoaXMuY2FudmFzRWxlbWVudC5uYXRpdmVFbGVtZW50LCB7XHJcbiAgICAgIG1heFpvb206IDEsXHJcbiAgICAgIG1pblpvb206IDAuMSxcclxuICAgICAgc21vb3RoU2Nyb2xsOiB0cnVlXHJcbiAgICB9KTtcclxuXHJcbiAgICB0aGlzLmdyYXBoVmlld0NhbnZhcy5vbigndHJhbnNmb3JtJywgZSA9PiB7XHJcbiAgICAgIGNvbnN0IHJlc3VsdDogVHJhbnNmb3JtID0gdGhpcy5ncmFwaFZpZXdDYW52YXMuZ2V0VHJhbnNmb3JtKCk7XHJcbiAgICAgIHRoaXMuem9vbVNjYWxlID0gcmVzdWx0LnNjYWxlO1xyXG4gICAgfSk7XHJcblxyXG4gICAgdGhpcy5ncmFwaFZpZXdDYW52YXMuc21vb3RoWm9vbSgwLCAwLCAwLjQpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIHBhdXNlWm9vbSgpIHtcclxuICAgIHRoaXMuZ3JhcGhWaWV3Q2FudmFzLnBhdXNlKCk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgcmVzdW1lWm9vbSgpIHtcclxuICAgIHRoaXMuZ3JhcGhWaWV3Q2FudmFzLnJlc3VtZSgpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIG9uRHJhZ01vdmUodmFsdWU6IENka0RyYWdNb3ZlKTogdm9pZCB7XHJcbiAgICBjb25zb2xlLmxvZyh2YWx1ZSk7XHJcbiAgfVxyXG59XHJcbiIsIjxkaXYgY2xhc3M9XCJtaXgtZGF0YWJhc2UtZ3JhcGhcIj5cclxuICA8ZGl2ICNjYW52YXNcclxuICAgICAgIGNsYXNzPVwiY2FudmFzXCI+XHJcbiAgICA8bmctY29udGFpbmVyICpuZ0Zvcj1cImxldCBpdGVtIG9mIGRhdGFiYXNlczsgbGV0IGkgPSBpbmRleFwiPlxyXG4gICAgICA8bWl4LWRhdGFiYXNlLWNhcmQgW2RhdGFiYXNlXT1cIml0ZW1cIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgW2luZGV4XT1cImlcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgW3pvb21TY2FsZV09XCJ6b29tU2NhbGVcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgKGRyYWdFbmQpPVwicmVzdW1lWm9vbSgpXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgIChkcmFnU3RhcnQpPVwicGF1c2Vab29tKClcIj48L21peC1kYXRhYmFzZS1jYXJkPlxyXG4gICAgPC9uZy1jb250YWluZXI+XHJcbiAgPC9kaXY+XHJcblxyXG4gIDxkaXYgY2xhc3M9XCJtaXgtZGF0YWJhc2UtZ3JhcGhfX3pvb20taW5mb1wiPlxyXG4gICAgWm9vbToge3sgKHpvb21TY2FsZSAqIDEwMCkudG9GaXhlZCgyKSB9fSUgfCBVc2UgTW91c2UgV2hlZWwgdG8gem9vbSBpbi9vdXRcclxuICA8L2Rpdj5cclxuPC9kaXY+XHJcbiJdfQ==