import { ColumnType } from '../data-table.model';
import { TableCellDirective } from './cell.directive';
import { TableHeaderDirective } from './header.directive';
import * as i0 from "@angular/core";
export declare class TableColumnDirective {
    header: string;
    key: string;
    sortable: boolean;
    columnType: ColumnType;
    showHeader: boolean;
    width: string | undefined;
    showInSubTable: boolean;
    tplCell?: TableCellDirective;
    tplHeader?: TableHeaderDirective;
    static ɵfac: i0.ɵɵFactoryDeclaration<TableColumnDirective, never>;
    static ɵdir: i0.ɵɵDirectiveDeclaration<TableColumnDirective, "[mixTableColumn]", never, { "header": "header"; "key": "key"; "sortable": "sortable"; "columnType": "columnType"; "showHeader": "showHeader"; "width": "width"; "showInSubTable": "showInSubTable"; }, {}, ["tplCell", "tplHeader"], never, false>;
}
