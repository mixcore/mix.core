import { ContentChild, Directive, Input } from '@angular/core';
import { TableCellDirective } from './cell.directive';
import { TableHeaderDirective } from './header.directive';
import * as i0 from "@angular/core";
export class TableColumnDirective {
    constructor() {
        this.header = '';
        this.key = '';
        this.sortable = true;
        this.columnType = 'DATA';
        this.showHeader = true;
        this.showInSubTable = true;
    }
}
TableColumnDirective.ɵfac = function TableColumnDirective_Factory(t) { return new (t || TableColumnDirective)(); };
TableColumnDirective.ɵdir = /*@__PURE__*/ i0.ɵɵdefineDirective({ type: TableColumnDirective, selectors: [["", "mixTableColumn", ""]], contentQueries: function TableColumnDirective_ContentQueries(rf, ctx, dirIndex) { if (rf & 1) {
        i0.ɵɵcontentQuery(dirIndex, TableCellDirective, 7);
        i0.ɵɵcontentQuery(dirIndex, TableHeaderDirective, 7);
    } if (rf & 2) {
        let _t;
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.tplCell = _t.first);
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.tplHeader = _t.first);
    } }, inputs: { header: "header", key: "key", sortable: "sortable", columnType: "columnType", showHeader: "showHeader", width: "width", showInSubTable: "showInSubTable" } });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(TableColumnDirective, [{
        type: Directive,
        args: [{
                selector: '[mixTableColumn]'
            }]
    }], null, { header: [{
            type: Input
        }], key: [{
            type: Input
        }], sortable: [{
            type: Input
        }], columnType: [{
            type: Input
        }], showHeader: [{
            type: Input
        }], width: [{
            type: Input
        }], showInSubTable: [{
            type: Input
        }], tplCell: [{
            type: ContentChild,
            args: [TableCellDirective, { static: true }]
        }], tplHeader: [{
            type: ContentChild,
            args: [TableHeaderDirective, { static: true }]
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29sdW1uLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9kYXRhLXRhYmxlL2RpcmVjdGl2ZXMvY29sdW1uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsWUFBWSxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFHL0QsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sa0JBQWtCLENBQUM7QUFDdEQsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7O0FBSzFELE1BQU0sT0FBTyxvQkFBb0I7SUFIakM7UUFJa0IsV0FBTSxHQUFHLEVBQUUsQ0FBQztRQUNaLFFBQUcsR0FBRyxFQUFFLENBQUM7UUFDVCxhQUFRLEdBQUcsSUFBSSxDQUFDO1FBQ2hCLGVBQVUsR0FBZSxNQUFNLENBQUM7UUFDaEMsZUFBVSxHQUFHLElBQUksQ0FBQztRQUVsQixtQkFBYyxHQUFHLElBQUksQ0FBQztLQUl2Qzs7d0ZBWFksb0JBQW9CO3VFQUFwQixvQkFBb0I7b0NBU2pCLGtCQUFrQjtvQ0FDbEIsb0JBQW9COzs7Ozs7dUZBVnZCLG9CQUFvQjtjQUhoQyxTQUFTO2VBQUM7Z0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjthQUM3QjtnQkFFaUIsTUFBTTtrQkFBckIsS0FBSztZQUNVLEdBQUc7a0JBQWxCLEtBQUs7WUFDVSxRQUFRO2tCQUF2QixLQUFLO1lBQ1UsVUFBVTtrQkFBekIsS0FBSztZQUNVLFVBQVU7a0JBQXpCLEtBQUs7WUFDVSxLQUFLO2tCQUFwQixLQUFLO1lBQ1UsY0FBYztrQkFBN0IsS0FBSztZQUVxRCxPQUFPO2tCQUFqRSxZQUFZO21CQUFDLGtCQUFrQixFQUFFLEVBQUUsTUFBTSxFQUFFLElBQUksRUFBRTtZQUNXLFNBQVM7a0JBQXJFLFlBQVk7bUJBQUMsb0JBQW9CLEVBQUUsRUFBRSxNQUFNLEVBQUUsSUFBSSxFQUFFIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29udGVudENoaWxkLCBEaXJlY3RpdmUsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5pbXBvcnQgeyBDb2x1bW5UeXBlIH0gZnJvbSAnLi4vZGF0YS10YWJsZS5tb2RlbCc7XHJcbmltcG9ydCB7IFRhYmxlQ2VsbERpcmVjdGl2ZSB9IGZyb20gJy4vY2VsbC5kaXJlY3RpdmUnO1xyXG5pbXBvcnQgeyBUYWJsZUhlYWRlckRpcmVjdGl2ZSB9IGZyb20gJy4vaGVhZGVyLmRpcmVjdGl2ZSc7XHJcblxyXG5ARGlyZWN0aXZlKHtcclxuICBzZWxlY3RvcjogJ1ttaXhUYWJsZUNvbHVtbl0nXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUYWJsZUNvbHVtbkRpcmVjdGl2ZSB7XHJcbiAgQElucHV0KCkgcHVibGljIGhlYWRlciA9ICcnO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBrZXkgPSAnJztcclxuICBASW5wdXQoKSBwdWJsaWMgc29ydGFibGUgPSB0cnVlO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBjb2x1bW5UeXBlOiBDb2x1bW5UeXBlID0gJ0RBVEEnO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBzaG93SGVhZGVyID0gdHJ1ZTtcclxuICBASW5wdXQoKSBwdWJsaWMgd2lkdGg6IHN0cmluZyB8IHVuZGVmaW5lZDtcclxuICBASW5wdXQoKSBwdWJsaWMgc2hvd0luU3ViVGFibGUgPSB0cnVlO1xyXG5cclxuICBAQ29udGVudENoaWxkKFRhYmxlQ2VsbERpcmVjdGl2ZSwgeyBzdGF0aWM6IHRydWUgfSkgcHVibGljIHRwbENlbGw/OiBUYWJsZUNlbGxEaXJlY3RpdmU7XHJcbiAgQENvbnRlbnRDaGlsZChUYWJsZUhlYWRlckRpcmVjdGl2ZSwgeyBzdGF0aWM6IHRydWUgfSkgcHVibGljIHRwbEhlYWRlcj86IFRhYmxlSGVhZGVyRGlyZWN0aXZlO1xyXG59XHJcbiJdfQ==