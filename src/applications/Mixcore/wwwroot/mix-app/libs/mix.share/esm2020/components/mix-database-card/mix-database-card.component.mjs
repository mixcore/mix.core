import { DragDropModule } from '@angular/cdk/drag-drop';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ShareModule } from '../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common";
import * as i2 from "@taiga-ui/core";
import * as i3 from "angular-tabler-icons";
import * as i4 from "@angular/cdk/drag-drop";
function MixDatabaseCardComponent_div_10_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 8)(1, "div", 9);
    i0.ɵɵelement(2, "i-tabler", 10);
    i0.ɵɵelementEnd();
    i0.ɵɵtext(3);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const column_r1 = ctx.$implicit;
    i0.ɵɵadvance(3);
    i0.ɵɵtextInterpolate2(" ", column_r1.displayName, " (", column_r1.dataType, ") ");
} }
const _c0 = function (a0, a1) { return { left: a0, top: a1 }; };
export class MixDatabaseCardComponent {
    constructor() {
        this.zoomScale = 1;
        this.pos = { x: 20, y: 20 };
        this.initializePos = { x: 0, y: 0 };
        this.index = 0;
        this.dragStart = new EventEmitter();
        this.dragEnd = new EventEmitter();
        this.dragConstrainPoint = (point, dragRef) => {
            let zoomMoveXDifference = 0;
            let zoomMoveYDifference = 0;
            if (this.zoomScale != 1) {
                zoomMoveXDifference =
                    (1 - this.zoomScale) * dragRef.getFreeDragPosition().x;
                zoomMoveYDifference =
                    (1 - this.zoomScale) * dragRef.getFreeDragPosition().y;
            }
            return {
                x: point.x + 20 + zoomMoveXDifference,
                y: point.y + zoomMoveYDifference
            };
        };
    }
    ngOnInit() {
        this.pos = { x: (this.index + 1) * 10, y: 20 };
    }
    startDragging() {
        this.dragStart.emit();
    }
    endDragging($event) {
        const elementMoving = $event.source.getRootElement();
        const elementMovingRect = elementMoving.getBoundingClientRect();
        if (elementMoving.parentElement) {
            const elementMovingParentElementRect = elementMoving.parentElement.getBoundingClientRect();
            this.pos.x =
                (elementMovingRect.left - elementMovingParentElementRect.left) /
                    this.zoomScale;
            this.pos.y =
                (elementMovingRect.top - elementMovingParentElementRect.top) /
                    this.zoomScale;
            const cdkDrag = $event.source;
            cdkDrag.reset();
            this.dragEnd.emit();
        }
        console.log(this.pos);
    }
}
MixDatabaseCardComponent.ɵfac = function MixDatabaseCardComponent_Factory(t) { return new (t || MixDatabaseCardComponent)(); };
MixDatabaseCardComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixDatabaseCardComponent, selectors: [["mix-database-card"]], inputs: { database: "database", zoomScale: "zoomScale", pos: "pos", initializePos: "initializePos", index: "index" }, outputs: { dragStart: "dragStart", dragEnd: "dragEnd" }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 11, vars: 8, consts: [["cdkDrag", "", 1, "mix-database-card", 3, "ngStyle", "cdkDragConstrainPosition", "cdkDragFreeDragPosition", "cdkDragEnded", "cdkDragStarted"], [1, "mix-database-card__header"], [1, "mix-database-card__body"], [1, "mix-database-card__reference"], ["tuiLink", "", "icon", "tuiIconSettings", "iconAlign", "left", 1, "add-reference-btn"], [1, "mix-database-card__column"], ["tuiLink", "", "icon", "tuiIconSettings", "iconAlign", "left", 1, "add-column-btn"], ["class", "column-item", 4, "ngFor", "ngForOf"], [1, "column-item"], [1, "icon"], ["name", "columns"]], template: function MixDatabaseCardComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0);
        i0.ɵɵlistener("cdkDragEnded", function MixDatabaseCardComponent_Template_div_cdkDragEnded_0_listener($event) { return ctx.endDragging($event); })("cdkDragStarted", function MixDatabaseCardComponent_Template_div_cdkDragStarted_0_listener() { return ctx.startDragging(); });
        i0.ɵɵelementStart(1, "div", 1);
        i0.ɵɵtext(2);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(3, "div", 2)(4, "div", 3)(5, "a", 4);
        i0.ɵɵtext(6, " Add Reference Table ");
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(7, "div", 5)(8, "a", 6);
        i0.ɵɵtext(9, " Add column ");
        i0.ɵɵelementEnd();
        i0.ɵɵtemplate(10, MixDatabaseCardComponent_div_10_Template, 4, 2, "div", 7);
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        let tmp_3_0;
        i0.ɵɵproperty("ngStyle", i0.ɵɵpureFunction2(5, _c0, ctx.pos.x + "px", ctx.pos.y + "px"))("cdkDragConstrainPosition", ctx.dragConstrainPoint)("cdkDragFreeDragPosition", ctx.pos);
        i0.ɵɵadvance(2);
        i0.ɵɵtextInterpolate1(" ", (tmp_3_0 = ctx.database == null ? null : ctx.database.displayName) !== null && tmp_3_0 !== undefined ? tmp_3_0 : "", " ");
        i0.ɵɵadvance(8);
        i0.ɵɵproperty("ngForOf", ctx.database.columns);
    } }, dependencies: [ShareModule, i1.NgForOf, i1.NgStyle, i2.TuiLinkComponent, i3.TablerIconComponent, DragDropModule, i4.CdkDrag], styles: [".mix-database-card[_ngcontent-%COMP%]{width:300px;border:1px solid var(--tui-base-04);border-radius:var(--mix-border-radius-01);overflow:hidden;position:relative;box-shadow:#0000003d 0 3px 8px}.mix-database-card__header[_ngcontent-%COMP%]{padding:10px;border-bottom:1px solid var(--tui-base-04);background-color:var(--tui-base-03)}.mix-database-card__body[_ngcontent-%COMP%]{padding:10px;background-color:var(--tui-base-01)}.mix-database-card__column[_ngcontent-%COMP%], .mix-database-card__reference[_ngcontent-%COMP%]{padding:10px}.mix-database-card__reference[_ngcontent-%COMP%]{border-bottom:1px solid var(--tui-base-04)}.mix-database-card__reference[_ngcontent-%COMP%]   .add-reference-btn[_ngcontent-%COMP%]{color:var(--tui-link)}.mix-database-card__column[_ngcontent-%COMP%]   .add-column-btn[_ngcontent-%COMP%]{color:var(--tui-link);margin-bottom:10px;display:inline-block}.mix-database-card__column[_ngcontent-%COMP%]   .column-item[_ngcontent-%COMP%]{margin-top:10px;display:flex;cursor:pointer}.mix-database-card__column[_ngcontent-%COMP%]   .column-item[_ngcontent-%COMP%]:hover{color:var(--tui-info-fill)}.mix-database-card__column[_ngcontent-%COMP%]   .column-item[_ngcontent-%COMP%]   .icon[_ngcontent-%COMP%]{margin-right:5px}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixDatabaseCardComponent, [{
        type: Component,
        args: [{ selector: 'mix-database-card', standalone: true, imports: [ShareModule, DragDropModule], template: "<div class=\"mix-database-card\"\r\n     [ngStyle]=\"{ left: pos.x + 'px', top: pos.y + 'px' }\"\r\n     [cdkDragConstrainPosition]=\"dragConstrainPoint\"\r\n     [cdkDragFreeDragPosition]=\"pos\"\r\n     (cdkDragEnded)=\"endDragging($event)\"\r\n     (cdkDragStarted)=\"startDragging()\"\r\n     cdkDrag>\r\n\r\n  <div class=\"mix-database-card__header\">\r\n    {{ database?.displayName ?? '' }}\r\n  </div>\r\n\r\n  <div class=\"mix-database-card__body\">\r\n    <div class=\"mix-database-card__reference\">\r\n      <a class=\"add-reference-btn\"\r\n         tuiLink\r\n         icon=\"tuiIconSettings\"\r\n         iconAlign=\"left\">\r\n        Add Reference Table\r\n      </a>\r\n    </div>\r\n\r\n    <div class=\"mix-database-card__column\">\r\n      <a class=\"add-column-btn\"\r\n         tuiLink\r\n         icon=\"tuiIconSettings\"\r\n         iconAlign=\"left\">\r\n        Add column\r\n      </a>\r\n\r\n      <div *ngFor=\"let column of database.columns\"\r\n           class=\"column-item\">\r\n        <div class=\"icon\">\r\n          <i-tabler name=\"columns\"></i-tabler>\r\n        </div>\r\n        {{ column.displayName }} ({{ column.dataType }})\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n", styles: [".mix-database-card{width:300px;border:1px solid var(--tui-base-04);border-radius:var(--mix-border-radius-01);overflow:hidden;position:relative;box-shadow:#0000003d 0 3px 8px}.mix-database-card__header{padding:10px;border-bottom:1px solid var(--tui-base-04);background-color:var(--tui-base-03)}.mix-database-card__body{padding:10px;background-color:var(--tui-base-01)}.mix-database-card__column,.mix-database-card__reference{padding:10px}.mix-database-card__reference{border-bottom:1px solid var(--tui-base-04)}.mix-database-card__reference .add-reference-btn{color:var(--tui-link)}.mix-database-card__column .add-column-btn{color:var(--tui-link);margin-bottom:10px;display:inline-block}.mix-database-card__column .column-item{margin-top:10px;display:flex;cursor:pointer}.mix-database-card__column .column-item:hover{color:var(--tui-info-fill)}.mix-database-card__column .column-item .icon{margin-right:5px}\n"] }]
    }], null, { database: [{
            type: Input
        }], zoomScale: [{
            type: Input
        }], pos: [{
            type: Input
        }], initializePos: [{
            type: Input
        }], index: [{
            type: Input
        }], dragStart: [{
            type: Output
        }], dragEnd: [{
            type: Output
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LWRhdGFiYXNlLWNhcmQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21peC1kYXRhYmFzZS1jYXJkL21peC1kYXRhYmFzZS1jYXJkLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtZGF0YWJhc2UtY2FyZC9taXgtZGF0YWJhc2UtY2FyZC5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBR0wsY0FBYyxFQUdmLE1BQU0sd0JBQXdCLENBQUM7QUFDaEMsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFVLE1BQU0sRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUcvRSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sb0JBQW9CLENBQUM7Ozs7Ozs7SUNvQjNDLDhCQUN5QixhQUFBO0lBRXJCLCtCQUFvQztJQUN0QyxpQkFBTTtJQUNOLFlBQ0Y7SUFBQSxpQkFBTTs7O0lBREosZUFDRjtJQURFLGlGQUNGOzs7QURqQk4sTUFBTSxPQUFPLHdCQUF3QjtJQVByQztRQVNrQixjQUFTLEdBQUcsQ0FBQyxDQUFDO1FBQ2QsUUFBRyxHQUFHLEVBQUUsQ0FBQyxFQUFFLEVBQUUsRUFBRSxDQUFDLEVBQUUsRUFBRSxFQUFFLENBQUM7UUFDdkIsa0JBQWEsR0FBRyxFQUFFLENBQUMsRUFBRSxDQUFDLEVBQUUsQ0FBQyxFQUFFLENBQUMsRUFBRSxDQUFDO1FBQy9CLFVBQUssR0FBRyxDQUFDLENBQUM7UUFFVCxjQUFTLEdBQUcsSUFBSSxZQUFZLEVBQVEsQ0FBQztRQUNyQyxZQUFPLEdBQUcsSUFBSSxZQUFZLEVB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R0FBRztrQkFBbEIsS0FBSztZQUNVLGFBQWE7a0JBQTVCLEtBQUs7WUFDVSxLQUFLO2tCQUFwQixLQUFLO1lBRVcsU0FBUztrQkFBekIsTUFBTTtZQUNVLE9BQU87a0JBQXZCLE1BQU0iLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xyXG4gIENka0RyYWcsXHJcbiAgQ2RrRHJhZ0VuZCxcclxuICBEcmFnRHJvcE1vZHVsZSxcclxuICBEcmFnUmVmLFxyXG4gIFBvaW50XHJcbn0gZnJvbSAnQGFuZ3VsYXIvY2RrL2RyYWctZHJvcCc7XHJcbmltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT25Jbml0LCBPdXRwdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgTWl4RGF0YWJhc2VNb2RlbCB9IGZyb20gJ0BtaXgtc3BhL21peC5saWInO1xyXG5cclxuaW1wb3J0IHsgU2hhcmVNb2R1bGUgfSBmcm9tICcuLi8uLi9zaGFyZS5tb2R1bGUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtZGF0YWJhc2UtY2FyZCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL21peC1kYXRhYmFzZS1jYXJkLmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi9taXgtZGF0YWJhc2UtY2FyZC5jb21wb25lbnQuc2NzcyddLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW1NoYXJlTW9kdWxlLCBEcmFnRHJvcE1vZHVsZV1cclxufSlcclxuZXhwb3J0IGNsYXNzIE1peERhdGFiYXNlQ2FyZENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XHJcbiAgQElucHV0KCkgcHVibGljIGRhdGFiYXNlITogTWl4RGF0YWJhc2VNb2RlbDtcclxuICBASW5wdXQoKSBwdWJsaWMgem9vbVNjYWxlID0gMTtcclxuICBASW5wdXQoKSBwdWJsaWMgcG9zID0geyB4OiAyMCwgeTogMjAgfTtcclxuICBASW5wdXQoKSBwdWJsaWMgaW5pdGlhbGl6ZVBvcyA9IHsgeDogMCwgeTogMCB9O1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBpbmRleCA9IDA7XHJcblxyXG4gIEBPdXRwdXQoKSBwdWJsaWMgZHJhZ1N0YXJ0ID0gbmV3IEV2ZW50RW1pdHRlcjx2b2lkPigpO1xyXG4gIEBPdXRwdXQoKSBwdWJsaWMgZHJhZ0VuZCA9IG5ldyBFdmVudEVtaXR0ZXI8dm9pZD4oKTtcclxuXHJcbiAgcHVibGljIG5nT25Jbml0KCk6IHZvaWQge1xyXG4gICAgdGhpcy5wb3MgPSB7IHg6ICh0aGlzLmluZGV4ICsgMSkgKiAxMCwgeTogMjAgfTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBkcmFnQ29uc3RyYWluUG9pbnQgPSAocG9pbnQ6IFBvaW50LCBkcmFnUmVmOiBEcmFnUmVmKSA9PiB7XHJcbiAgICBsZXQgem9vbU1vdmVYRGlmZmVyZW5jZSA9IDA7XHJcbiAgICBsZXQgem9vbU1vdmVZRGlmZmVyZW5jZSA9IDA7XHJcblxyXG4gICAgaWYgKHRoaXMuem9vbVNjYWxlICE9IDEpIHtcclxuICAgICAgem9vbU1vdmVYRGlmZmVyZW5jZSA9XHJcbiAgICAgICAgKDEgLSB0aGlzLnpvb21TY2FsZSkgKiBkcmFnUmVmLmdldEZyZWVEcmFnUG9zaXRpb24oKS54O1xyXG4gICAgICB6b29tTW92ZVlEaWZmZXJlbmNlID1cclxuICAgICAgICAoMSAtIHRoaXMuem9vbVNjYWxlKSAqIGRyYWdSZWYuZ2V0RnJlZURyYWdQb3NpdGlvbigpLnk7XHJcbiAgICB9XHJcblxyXG4gICAgcmV0dXJuIHtcclxuICAgICAgeDogcG9pbnQueCArIDIwICsgem9vbU1vdmVYRGlmZmVyZW5jZSxcclxuICAgICAgeTogcG9pbnQueSArIHpvb21Nb3ZlWURpZmZlcmVuY2VcclxuICAgIH07XHJcbiAgfTtcclxuXHJcbiAgcHVibGljIHN0YXJ0RHJhZ2dpbmcoKSB7XHJcbiAgICB0aGlzLmRyYWdTdGFydC5lbWl0KCk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgZW5kRHJhZ2dpbmcoJGV2ZW50OiBDZGtEcmFnRW5kKSB7XHJcbiAgICBjb25zdCBlbGVtZW50TW92aW5nID0gJGV2ZW50LnNvdXJjZS5nZXRSb290RWxlbWVudCgpO1xyXG4gICAgY29uc3QgZWxlbWVudE1vdmluZ1JlY3Q6IENsaWVudFJlY3QgPSBlbGVtZW50TW92aW5nLmdldEJvdW5kaW5nQ2xpZW50UmVjdCgpO1xyXG4gICAgaWYgKGVsZW1lbnRNb3ZpbmcucGFyZW50RWxlbWVudCkge1xyXG4gICAgICBjb25zdCBlbGVtZW50TW92aW5nUGFyZW50RWxlbWVudFJlY3Q6IENsaWVudFJlY3QgPVxyXG4gICAgICAgIGVsZW1lbnRNb3ZpbmcucGFyZW50RWxlbWVudC5nZXRCb3VuZGluZ0NsaWVudFJlY3QoKTtcclxuXHJcbiAgICAgIHRoaXMucG9zLnggPVxyXG4gICAgICAgIChlbGVtZW50TW92aW5nUmVjdC5sZWZ0IC0gZWxlbWVudE1vdmluZ1BhcmVudEVsZW1lbnRSZWN0LmxlZnQpIC9cclxuICAgICAgICB0aGlzLnpvb21TY2FsZTtcclxuICAgICAgdGhpcy5wb3MueSA9XHJcbiAgICAgICAgKGVsZW1lbnRNb3ZpbmdSZWN0LnRvcCAtIGVsZW1lbnRNb3ZpbmdQYXJlbnRFbGVtZW50UmVjdC50b3ApIC9cclxuICAgICAgICB0aGlzLnpvb21TY2FsZTtcclxuXHJcbiAgICAgIGNvbnN0IGNka0RyYWcgPSAkZXZlbnQuc291cmNlIGFzIENka0RyYWc7XHJcbiAgICAgIGNka0RyYWcucmVzZXQoKTtcclxuXHJcbiAgICAgIHRoaXMuZHJhZ0VuZC5lbWl0KCk7XHJcbiAgICB9XHJcblxyXG4gICAgY29uc29sZS5sb2codGhpcy5wb3MpO1xyXG4gIH1cclxufVxyXG4iLCI8ZGl2IGNsYXNzPVwibWl4LWRhdGFiYXNlLWNhcmRcIlxyXG4gICAgIFtuZ1N0eWxlXT1cInsgbGVmdDogcG9zLnggKyAncHgnLCB0b3A6IHBvcy55ICsgJ3B4JyB9XCJcclxuICAgICBbY2RrRHJhZ0NvbnN0cmFpblBvc2l0aW9uXT1cImRyYWdDb25zdHJhaW5Qb2ludFwiXHJcbiAgICAgW2Nka0RyYWdGcmVlRHJhZ1Bvc2l0aW9uXT1cInBvc1wiXHJcbiAgICAgKGNka0RyYWdFbmRlZCk9XCJlbmREcmFnZ2luZygkZXZlbnQpXCJcclxuICAgICAoY2RrRHJhZ1N0YXJ0ZWQpPVwic3RhcnREcmFnZ2luZygpXCJcclxuICAgICBjZGtEcmFnPlxyXG5cclxuICA8ZGl2IGNsYXNzPVwibWl4LWRhdGFiYXNlLWNhcmRfX2hlYWRlclwiPlxyXG4gICAge3sgZGF0YWJhc2U/LmRpc3BsYXlOYW1lID8/ICcnIH19XHJcbiAgPC9kaXY+XHJcblxyXG4gIDxkaXYgY2xhc3M9XCJtaXgtZGF0YWJhc2UtY2FyZF9fYm9keVwiPlxyXG4gICAgPGRpdiBjbGFzcz1cIm1peC1kYXRhYmFzZS1jYXJkX19yZWZlcmVuY2VcIj5cclxuICAgICAgPGEgY2xhc3M9XCJhZGQtcmVmZXJlbmNlLWJ0blwiXHJcbiAgICAgICAgIHR1aUxpbmtcclxuICAgICAgICAgaWNvbj1cInR1aUljb25TZXR0aW5nc1wiXHJcbiAgICAgICAgIGljb25BbGlnbj1cImxlZnRcIj5cclxuICAgICAgICBBZGQgUmVmZXJlbmNlIFRhYmxlXHJcbiAgICAgIDwvYT5cclxuICAgIDwvZGl2PlxyXG5cclxuICAgIDxkaXYgY2xhc3M9XCJtaXgtZGF0YWJhc2UtY2FyZF9fY29sdW1uXCI+XHJcbiAgICAgIDxhIGNsYXNzPVwiYWRkLWNvbHVtbi1idG5cIlxyXG4gICAgICAgICB0dWlMaW5rXHJcbiAgICAgICAgIGljb249XCJ0dWlJY29uU2V0dGluZ3NcIlxyXG4gICAgICAgICBpY29uQWxpZ249XCJsZWZ0XCI+XHJcbiAgICAgICAgQWRkIGNvbHVtblxyXG4gICAgICA8L2E+XHJcblxyXG4gICAgICA8ZGl2ICpuZ0Zvcj1cImxldCBjb2x1bW4gb2YgZGF0YWJhc2UuY29sdW1uc1wiXHJcbiAgICAgICAgICAgY2xhc3M9XCJjb2x1bW4taXRlbVwiPlxyXG4gICAgICAgIDxkaXYgY2xhc3M9XCJpY29uXCI+XHJcbiAgICAgICAgICA8aS10YWJsZXIgbmFtZT1cImNvbHVtbnNcIj48L2ktdGFibGVyPlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICAgIHt7IGNvbHVtbi5kaXNwbGF5TmFtZSB9fSAoe3sgY29sdW1uLmRhdGFUeXBlIH19KVxyXG4gICAgICA8L2Rpdj5cclxuICAgIDwvZGl2PlxyXG4gIDwvZGl2PlxyXG48L2Rpdj5cclxuIl19