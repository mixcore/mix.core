import { DropListRef } from '@angular/cdk/drag-drop';
import { ChangeDetectionStrategy, Component, ContentChildren, ElementRef, EventEmitter, Input, Output, QueryList, ViewChild, ViewEncapsulation } from '@angular/core';
import { TUI_ARROW } from '@taiga-ui/kit';
import { BehaviorSubject, catchError, combineLatest, debounceTime, Observable, of, startWith, Subject, switchMap, tap } from 'rxjs';
import { TableColumnDirective } from './directives/column.directive';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common";
import * as i2 from "@angular/forms";
import * as i3 from "@taiga-ui/core";
import * as i4 from "@taiga-ui/addon-table";
import * as i5 from "@taiga-ui/kit";
import * as i6 from "@taiga-ui/cdk";
import * as i7 from "@angular/cdk/drag-drop";
import * as i8 from "angular-tabler-icons";
import * as i9 from "../../pipes/relative-timepsan.pipe";
const _c0 = ["subDropList"];
const _c1 = ["dropList"];
function MixDataTableComponent_p_1_tui_input_1_Template(rf, ctx) { if (rf & 1) {
    const _r10 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tui-input", 14);
    i0.ɵɵlistener("ngModelChange", function MixDataTableComponent_p_1_tui_input_1_Template_tui_input_ngModelChange_0_listener($event) { i0.ɵɵrestoreView(_r10); const ctx_r9 = i0.ɵɵnextContext(2); return i0.ɵɵresetView(ctx_r9.searchText$.next($event)); });
    i0.ɵɵtext(1);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const ctx_r7 = i0.ɵɵnextContext(2);
    i0.ɵɵproperty("ngModel", ctx_r7.search)("tuiTextfieldCleaner", true);
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", ctx_r7.searchPlaceholder, " ");
} }
function MixDataTableComponent_p_1_tui_hosted_dropdown_2_ng_template_3_Template(rf, ctx) { if (rf & 1) {
    const _r14 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tui-reorder", 18);
    i0.ɵɵlistener("itemsChange", function MixDataTableComponent_p_1_tui_hosted_dropdown_2_ng_template_3_Template_tui_reorder_itemsChange_0_listener($event) { i0.ɵɵrestoreView(_r14); const ctx_r13 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r13.tableSortFields = $event); })("enabledChange", function MixDataTableComponent_p_1_tui_hosted_dropdown_2_ng_template_3_Template_tui_reorder_enabledChange_0_listener($event) { i0.ɵɵrestoreView(_r14); const ctx_r15 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r15.onEnabled($event)); });
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const ctx_r12 = i0.ɵɵnextContext(3);
    i0.ɵɵproperty("items", ctx_r12.tableSortFields)("enabled", ctx_r12.tableSortFields);
} }
function MixDataTableComponent_p_1_tui_hosted_dropdown_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "tui-hosted-dropdown", 15)(1, "button", 16);
    i0.ɵɵtext(2, "Columns");
    i0.ɵɵelementEnd();
    i0.ɵɵtemplate(3, MixDataTableComponent_p_1_tui_hosted_dropdown_2_ng_template_3_Template, 1, 2, "ng-template", null, 17, i0.ɵɵtemplateRefExtractor);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const _r11 = i0.ɵɵreference(4);
    const ctx_r8 = i0.ɵɵnextContext(2);
    i0.ɵɵproperty("content", _r11);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("iconRight", ctx_r8.arrow);
} }
function MixDataTableComponent_p_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "p", 11);
    i0.ɵɵtemplate(1, MixDataTableComponent_p_1_tui_input_1_Template, 2, 3, "tui-input", 12);
    i0.ɵɵtemplate(2, MixDataTableComponent_p_1_tui_hosted_dropdown_2_Template, 5, 2, "tui-hosted-dropdown", 13);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const ctx_r0 = i0.ɵɵnextContext();
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", ctx_r0.searchable);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", ctx_r0.reOrderable);
} }
function MixDataTableComponent_div_4_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 19);
    i0.ɵɵtext(1, " Current Page ");
    i0.ɵɵelementEnd();
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainer(0);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_ng_container_1_ng_container_1_Template, 1, 0, "ng-container", 27);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext(3).$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngTemplateOutlet", col_r20.tplHeader.template);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext(3).$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", col_r20.header, " ");
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "th", 26);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_ng_container_1_Template, 2, 1, "ng-container", 24);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_ng_container_2_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext(2).$implicit;
    i0.ɵɵproperty("resizable", true)("sorter", null);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r20.tplHeader);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", !col_r20.tplHeader && col_r20.showHeader);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_1_th_1_Template, 3, 4, "th", 25);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHead", col_r20.key);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainer(0);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_ng_container_1_ng_container_1_Template, 1, 0, "ng-container", 27);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext(3).$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngTemplateOutlet", col_r20.tplHeader.template);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext(3).$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", col_r20.header, " ");
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "th", 29);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_ng_container_1_Template, 2, 1, "ng-container", 24);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_ng_container_2_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext(2).$implicit;
    i0.ɵɵproperty("resizable", true);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r20.tplHeader);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", !col_r20.tplHeader && col_r20.showHeader);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_2_th_1_Template, 3, 3, "th", 28);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHead", col_r20.key);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_3_th_1_Template(rf, ctx) { if (rf & 1) {
    const _r42 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "th", 31)(1, "div", 32)(2, "tui-checkbox", 33);
    i0.ɵɵlistener("ngModelChange", function MixDataTableComponent_table_7_ng_container_3_ng_container_3_th_1_Template_tui_checkbox_ngModelChange_2_listener($event) { i0.ɵɵrestoreView(_r42); const ctx_r41 = i0.ɵɵnextContext(4); return i0.ɵɵresetView(ctx_r41.markAllChecked($event)); });
    i0.ɵɵelementEnd()()();
} if (rf & 2) {
    const ctx_r40 = i0.ɵɵnextContext(4);
    i0.ɵɵproperty("sorter", null);
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("ngModel", ctx_r40.isAllSelected);
} }
function MixDataTableComponent_table_7_ng_container_3_ng_container_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_3_th_1_Template, 3, 2, "th", 30);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHead", col_r20.key);
} }
function MixDataTableComponent_table_7_ng_container_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_ng_container_3_ng_container_1_Template, 2, 1, "ng-container", 24);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_7_ng_container_3_ng_container_2_Template, 2, 1, "ng-container", 24);
    i0.ɵɵtemplate(3, MixDataTableComponent_table_7_ng_container_3_ng_container_3_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r20 = ctx.$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r20.sortable === false && col_r20.columnType !== "CHECKBOX");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r20.sortable === true && col_r20.columnType !== "CHECKBOX");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r20.columnType === "CHECKBOX");
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainer(0);
} }
const _c2 = function (a0) { return { $implicit: a0 }; };
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_ng_container_1_Template, 1, 0, "ng-container", 40);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r48 = i0.ɵɵnextContext(3).$implicit;
    const item_r46 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngTemplateOutlet", col_r48.tplCell.template)("ngTemplateOutletContext", i0.ɵɵpureFunction1(2, _c2, item_r46));
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1);
    i0.ɵɵpipe(2, "relativeTimeSpan");
    i0.ɵɵpipe(3, "date");
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r48 = i0.ɵɵnextContext(3).$implicit;
    const item_r46 = i0.ɵɵnextContext().$implicit;
    let tmp_0_0;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", i0.ɵɵpipeBind1(2, 1, i0.ɵɵpipeBind2(3, 3, (tmp_0_0 = item_r46[col_r48.key]) !== null && tmp_0_0 !== undefined ? tmp_0_0 : "N/A", "short")), " ");
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r48 = i0.ɵɵnextContext(3).$implicit;
    const item_r46 = i0.ɵɵnextContext().$implicit;
    let tmp_0_0;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", (tmp_0_0 = item_r46[col_r48.key]) !== null && tmp_0_0 !== undefined ? tmp_0_0 : "N/A", " ");
} }
const _c3 = function (a0, a1, a2) { return { "--action": a0, "--date": a1, "--hide-sub-table": a2 }; };
const _c4 = function (a0) { return { "width": a0 }; };
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "td", 39);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_Template, 2, 4, "ng-container", 24);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_2_Template, 4, 6, "ng-container", 24);
    i0.ɵɵtemplate(3, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_3_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const col_r48 = i0.ɵɵnextContext(2).$implicit;
    i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction3(5, _c3, col_r48.columnType === "ACTION", col_r48.columnType === "DATE", col_r48.showInSubTable === false))("ngStyle", i0.ɵɵpureFunction1(9, _c4, col_r48.width));
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r48.tplCell);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", !col_r48.tplCell && col_r48.columnType === "DATE");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", !col_r48.tplCell && col_r48.columnType !== "DATE");
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_Template, 4, 11, "td", 38);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r48 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiCell", col_r48.key);
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_2_td_1_Template(rf, ctx) { if (rf & 1) {
    const _r67 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "td", 42)(1, "tui-checkbox", 33);
    i0.ɵɵlistener("ngModelChange", function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_2_td_1_Template_tui_checkbox_ngModelChange_1_listener($event) { i0.ɵɵrestoreView(_r67); const item_r46 = i0.ɵɵnextContext(3).$implicit; const ctx_r65 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r65.onItemSelected($event, item_r46)); });
    i0.ɵɵelementEnd()();
} if (rf & 2) {
    const item_r46 = i0.ɵɵnextContext(3).$implicit;
    const ctx_r64 = i0.ɵɵnextContext(3);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngModel", ctx_r64.isItemSelected(item_r46));
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_2_td_1_Template, 2, 1, "td", 41);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r48 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiCell", col_r48.key);
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_1_Template, 2, 1, "ng-container", 24);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_ng_container_2_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r48 = ctx.$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r48.columnType !== "CHECKBOX");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r48.columnType === "CHECKBOX");
} }
function MixDataTableComponent_table_7_tbody_4_tr_2_Template(rf, ctx) { if (rf & 1) {
    const _r71 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tr", 37);
    i0.ɵɵlistener("cdkDragMoved", function MixDataTableComponent_table_7_tbody_4_tr_2_Template_tr_cdkDragMoved_0_listener($event) { i0.ɵɵrestoreView(_r71); const ctx_r70 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r70.onDragItem($event)); })("cdkDragReleased", function MixDataTableComponent_table_7_tbody_4_tr_2_Template_tr_cdkDragReleased_0_listener() { i0.ɵɵrestoreView(_r71); const ctx_r72 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r72.onReleaseDragItem()); });
    i0.ɵɵtemplate(1, MixDataTableComponent_table_7_tbody_4_tr_2_ng_container_1_Template, 3, 2, "ng-container", 22);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const item_r46 = ctx.$implicit;
    const ctx_r45 = i0.ɵɵnextContext(3);
    i0.ɵɵproperty("cdkDragData", item_r46);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngForOf", ctx_r45.columns);
} }
function MixDataTableComponent_table_7_tbody_4_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "tbody", 34, 35);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_7_tbody_4_tr_2_Template, 2, 2, "tr", 36);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const data_r16 = i0.ɵɵnextContext().tuiLet;
    i0.ɵɵproperty("data", data_r16.items);
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("tuiRowOf", data_r16.items);
} }
function MixDataTableComponent_table_7_tfoot_5_Template(rf, ctx) { if (rf & 1) {
    const _r75 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tfoot")(1, "tr", 43)(2, "td", 44)(3, "tui-pagination", 45);
    i0.ɵɵlistener("indexChange", function MixDataTableComponent_table_7_tfoot_5_Template_tui_pagination_indexChange_3_listener($event) { i0.ɵɵrestoreView(_r75); const ctx_r74 = i0.ɵɵnextContext(2); return i0.ɵɵresetView(ctx_r74.onPageChange($event)); });
    i0.ɵɵelementEnd()()()();
} if (rf & 2) {
    const data_r16 = i0.ɵɵnextContext().tuiLet;
    const ctx_r19 = i0.ɵɵnextContext();
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("colSpan", ctx_r19.columns.length);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("length", data_r16.pagingData.totalPage || 0);
} }
function MixDataTableComponent_table_7_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "table", 20)(1, "thead")(2, "tr", 21);
    i0.ɵɵtemplate(3, MixDataTableComponent_table_7_ng_container_3_Template, 4, 3, "ng-container", 22);
    i0.ɵɵelementEnd()();
    i0.ɵɵtemplate(4, MixDataTableComponent_table_7_tbody_4_Template, 3, 2, "tbody", 23);
    i0.ɵɵtemplate(5, MixDataTableComponent_table_7_tfoot_5_Template, 4, 2, "tfoot", 24);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const data_r16 = ctx.tuiLet;
    const ctx_r2 = i0.ɵɵnextContext();
    i0.ɵɵproperty("columns", ctx_r2.tableColumns);
    i0.ɵɵadvance(3);
    i0.ɵɵproperty("ngForOf", ctx_r2.columns);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", data_r16);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", data_r16 && !ctx_r2.showSubTable);
} }
function MixDataTableComponent_div_9_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 46);
    i0.ɵɵelement(1, "i-tabler", 47);
    i0.ɵɵelementEnd();
} }
function MixDataTableComponent_div_11_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 19);
    i0.ɵɵtext(1, " Next Page ");
    i0.ɵɵelementEnd();
} }
function MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainer(0);
} }
function MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_ng_container_1_ng_container_1_Template, 1, 0, "ng-container", 27);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r80 = i0.ɵɵnextContext(3).$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngTemplateOutlet", col_r80.tplHeader.template);
} }
function MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r80 = i0.ɵɵnextContext(3).$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", col_r80.header, " ");
} }
function MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "th", 26);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_ng_container_1_Template, 2, 1, "ng-container", 24);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_ng_container_2_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const col_r80 = i0.ɵɵnextContext(2).$implicit;
    i0.ɵɵproperty("resizable", true)("sorter", null);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r80.tplHeader);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", !col_r80.tplHeader && col_r80.showHeader);
} }
function MixDataTableComponent_table_12_ng_container_3_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_ng_container_3_ng_container_1_th_1_Template, 3, 4, "th", 25);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r80 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHead", col_r80.key);
} }
function MixDataTableComponent_table_12_ng_container_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_ng_container_3_ng_container_1_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r80 = ctx.$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r80.columnType !== "CHECKBOX");
} }
function MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainer(0);
} }
function MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_ng_container_1_Template, 1, 0, "ng-container", 40);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r94 = i0.ɵɵnextContext(3).$implicit;
    const item_r92 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngTemplateOutlet", col_r94.tplCell.template)("ngTemplateOutletContext", i0.ɵɵpureFunction1(2, _c2, item_r92));
} }
function MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1);
    i0.ɵɵpipe(2, "relativeTimeSpan");
    i0.ɵɵpipe(3, "date");
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r94 = i0.ɵɵnextContext(3).$implicit;
    const item_r92 = i0.ɵɵnextContext().$implicit;
    let tmp_0_0;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", i0.ɵɵpipeBind1(2, 1, i0.ɵɵpipeBind2(3, 3, (tmp_0_0 = item_r92[col_r94.key]) !== null && tmp_0_0 !== undefined ? tmp_0_0 : "N/A", "short")), " ");
} }
function MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtext(1);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r94 = i0.ɵɵnextContext(3).$implicit;
    const item_r92 = i0.ɵɵnextContext().$implicit;
    let tmp_0_0;
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", (tmp_0_0 = item_r92[col_r94.key]) !== null && tmp_0_0 !== undefined ? tmp_0_0 : "N/A", " ");
} }
const _c5 = function (a0, a1) { return { "--action": a0, "--date": a1 }; };
function MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "td", 39);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_1_Template, 2, 4, "ng-container", 24);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_2_Template, 4, 6, "ng-container", 24);
    i0.ɵɵtemplate(3, MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_ng_container_3_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const col_r94 = i0.ɵɵnextContext(2).$implicit;
    i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction2(5, _c5, col_r94.columnType === "ACTION", col_r94.columnType === "DATE"))("ngStyle", i0.ɵɵpureFunction1(8, _c4, col_r94.width));
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r94.tplCell);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", !col_r94.tplCell && col_r94.columnType === "DATE");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", !col_r94.tplCell && col_r94.columnType !== "DATE");
} }
function MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_td_1_Template, 4, 10, "td", 38);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r94 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiCell", col_r94.key);
} }
function MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_ng_container_1_Template, 2, 1, "ng-container", 24);
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const col_r94 = ctx.$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", col_r94.columnType !== "CHECKBOX");
} }
function MixDataTableComponent_table_12_tbody_4_tr_2_Template(rf, ctx) { if (rf & 1) {
    const _r110 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tr", 37);
    i0.ɵɵlistener("cdkDragMoved", function MixDataTableComponent_table_12_tbody_4_tr_2_Template_tr_cdkDragMoved_0_listener($event) { i0.ɵɵrestoreView(_r110); const ctx_r109 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r109.onDragItem($event)); })("cdkDragReleased", function MixDataTableComponent_table_12_tbody_4_tr_2_Template_tr_cdkDragReleased_0_listener() { i0.ɵɵrestoreView(_r110); const ctx_r111 = i0.ɵɵnextContext(3); return i0.ɵɵresetView(ctx_r111.onReleaseDragItem()); });
    i0.ɵɵtemplate(1, MixDataTableComponent_table_12_tbody_4_tr_2_ng_container_1_Template, 2, 1, "ng-container", 22);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const item_r92 = ctx.$implicit;
    const ctx_r91 = i0.ɵɵnextContext(3);
    i0.ɵɵproperty("cdkDragData", item_r92);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngForOf", ctx_r91.columns);
} }
function MixDataTableComponent_table_12_tbody_4_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "tbody", 34, 48);
    i0.ɵɵtemplate(2, MixDataTableComponent_table_12_tbody_4_tr_2_Template, 2, 2, "tr", 36);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const data_r77 = i0.ɵɵnextContext().tuiLet;
    i0.ɵɵproperty("data", data_r77.items);
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("tuiRowOf", data_r77.items);
} }
function MixDataTableComponent_table_12_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "table", 20)(1, "thead")(2, "tr", 21);
    i0.ɵɵtemplate(3, MixDataTableComponent_table_12_ng_container_3_Template, 2, 1, "ng-container", 22);
    i0.ɵɵelementEnd()();
    i0.ɵɵtemplate(4, MixDataTableComponent_table_12_tbody_4_Template, 3, 2, "tbody", 23);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const data_r77 = ctx.tuiLet;
    const ctx_r5 = i0.ɵɵnextContext();
    i0.ɵɵproperty("columns", ctx_r5.subTableColumns);
    i0.ɵɵadvance(3);
    i0.ɵɵproperty("ngForOf", ctx_r5.columns);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("ngIf", data_r77);
} }
const _c6 = function (a0) { return { "--show": a0 }; };
export class MixDataTableComponent {
    constructor(elementRef) {
        this.elementRef = elementRef;
        this.currentSelectedItem = [];
        this.cacheItems = [];
        this.currentPage = 0;
        this.isAllSelected = false;
        this.selfControl = true;
        this.loading$ = new BehaviorSubject(true);
        this.search = '';
        this.searchPlaceholder = 'Search';
        this.totalRows = 0;
        this.searchable = true;
        this.reOrderable = true;
        this.dataIndexKey = 'id';
        this.searchColumns = 'title';
        this.pageChange = new EventEmitter();
        this.pageSizeChange = new EventEmitter();
        this.tableQueryChange = new EventEmitter();
        this.itemsSelectedChange = new EventEmitter();
        this.tableInitialColumns = [];
        this.tableColumns = [];
        this.subTableColumns = [];
        this.tableEnabledColumns = [];
        this.tableSortFields = [];
        this.columnDic = {};
        this.showSubTable = false;
        this.showDragLeft = new BehaviorSubject(false);
        this.showDragRight = new BehaviorSubject(false);
        this.arrow = TUI_ARROW;
        this.searchText$ = new BehaviorSubject('');
        this.size$ = new Subject();
        this.page$ = new Subject();
        this.dragChange = new Subject();
        this.direction$ = new BehaviorSubject(1);
        this.reload$ = new BehaviorSubject(false);
        this.emptyData = {
            items: [],
            pagingData: {
                pageIndex: 0,
                pageSize: 25,
                total: 0
            }
        };
        this.request$ = combineLatest([
            this.searchText$.pipe(debounceTime(300)),
            this.direction$,
            this.page$.pipe(startWith(0)),
            this.size$.pipe(startWith(10)),
            this.reload$
        ]);
    }
    ngOnInit() {
        if (this.selfControl) {
            this._setupSelfControl();
        }
        else {
            this.request$.subscribe((query) => {
                this.tableQueryChange.emit({
                    keyword: query[0],
                    pageIndex: query[3],
                    pageSize: query[2]
                });
            });
        }
    }
    handleDragAndDrop() {
        // this.dragChange.pipe(debounceTime(100)).subscribe((pointerX: number) => {
        //   const currentListOffsetLeft = this.elementRef.nativeElement.offsetLeft;
        //   const currentListOffsetRight = this.elementRef.nativeElement.offsetWidth + currentListOffsetLeft;
        //   if (pointerX - currentListOffsetLeft <= 100) {
        //     this.showDragRight.next(false);
        //     this.showDragLeft.next(true);
        //   } else if (currentListOffsetRight - pointerX <= 100) {
        //     this.showDragRight.next(true);
        //     this.showDragLeft.next(false);
        //   } else {
        //     this.showDragRight.next(false);
        //     this.showDragLeft.next(false);
        //   }
        // });
        // this.showDragLeft.pipe(debounceTime(500)).subscribe(v => {
        //   if (!v) return;
        //   this.showSubTable = true;
        // });
        // this.showDragRight.pipe(debounceTime(500)).subscribe(v => {
        //   if (!v) return;
        //   this.showSubTable = true;
        // });
    }
    isItemSelected(item) {
        return !!this.currentSelectedItem.find((v) => JSON.stringify(v) === JSON.stringify(item));
    }
    ngAfterContentInit() {
        const columns = this.columns.toArray();
        this.columnDic = this._buildColumnDictionary(columns);
        this.tableInitialColumns = columns.map((c) => c.key);
        this.tableColumns = columns.map((c) => c.key);
        this.subTableColumns = columns
            .filter(c => c.columnType !== 'CHECKBOX' && c.showInSubTable === true)
            .map((c) => c.key);
        this.tableSortFields = columns.map((c) => c.header);
    }
    onEnabled(enabled) {
        this.tableColumns = this.tableSortFields
            .filter((key) => enabled.includes(key))
            .map((v) => this.columnDic[v]);
    }
    onPageChange(page) {
        this.pageChange.emit(page);
        this.page$.next(page);
    }
    onSizeChange(size) {
        this.pageSizeChange.emit(size);
        this.size$.next(size);
    }
    onItemSelected(value, item) {
        if (value) {
            this.currentSelectedItem.push(item);
        }
        else {
            this.currentSelectedItem = this.currentSelectedItem.filter((v) => JSON.stringify(item) !== JSON.stringify(v));
        }
        this.isAllSelected =
            this.currentSelectedItem.length === this.cacheItems.length;
        this.itemsSelectedChange.emit(this.currentSelectedItem);
    }
    markAllChecked(value) {
        if (value) {
            this.currentSelectedItem = this.cacheItems;
        }
        else {
            this.currentSelectedItem = [];
        }
        this.isAllSelected = value;
        this.itemsSelectedChange.emit(this.currentSelectedItem);
    }
    getNavigationLength(totalCount, pageSize) {
        return Math.floor(totalCount / pageSize) || 1;
    }
    reloadData() {
        this.reload$.next(!this.reload$.getValue());
    }
    onDragItem(event) {
        this.dragChange.next(event.pointerPosition.x);
    }
    onReleaseDragItem() {
        // this.showDragLeft.next(false);
        // this.showDragRight.next(false);
        // this.showSubTable = false;
    }
    _processSelfFetchData(searchText, page, pageSize) {
        return this.fetchDataFn({
            keyword: searchText,
            pageIndex: page,
            pageSize: pageSize,
            searchColumns: this.searchColumns,
            searchMethod: 'Like'
        });
    }
    _showLoading() {
        this.loading$.next(true);
    }
    _hideLoading() {
        this.loading$.next(false);
    }
    _buildColumnDictionary(columns) {
        return columns.reduce((acc, item) => ({
            ...acc,
            [item.header]: item.key
        }), {});
    }
    _setupSelfControl() {
        this.data$ = this.request$.pipe(tap(() => this._showLoading()), switchMap((query) => this._processSelfFetchData(query[0], query[2], query[3])), tap((res) => {
            this._hideLoading();
            this.cacheItems = res.items;
            this.currentPage = res.pagingData.pageIndex;
        }), startWith(this.emptyData), catchError(() => {
            this._hideLoading();
            return of(this.emptyData);
        }));
    }
}
MixDataTableComponent.ɵfac = function MixDataTableComponent_Factory(t) { return new (t || MixDataTableComponent)(i0.ɵɵdirectiveInject(i0.ElementRef)); };
MixDataTableComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixDataTableComponent, selectors: [["mix-data-table"]], contentQueries: function MixDataTableComponent_ContentQueries(rf, ctx, dirIndex) { if (rf & 1) {
        i0.ɵɵcontentQuery(dirIndex, TableColumnDirective, 4);
    } if (rf & 2) {
        let _t;
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.columns = _t);
    } }, viewQuery: function MixDataTableComponent_Query(rf, ctx) { if (rf & 1) {
        i0.ɵɵviewQuery(_c0, 5);
        i0.ɵɵviewQuery(_c1, 5);
    } if (rf & 2) {
        let _t;
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.subDropList = _t.first);
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.dropList = _t.first);
    } }, inputs: { selfControl: "selfControl", fetchDataFn: "fetchDataFn", data$: "data$", loading$: "loading$", search: "search", searchPlaceholder: "searchPlaceholder", totalRows: "totalRows", searchable: "searchable", reOrderable: "reOrderable", dataIndexKey: "dataIndexKey", searchColumns: "searchColumns" }, outputs: { pageChange: "pageChange", pageSizeChange: "pageSizeChange", tableQueryChange: "tableQueryChange", itemsSelectedChange: "itemsSelectedChange" }, decls: 16, vars: 20, consts: [[1, "mix-data-table"], ["class", "mix-data-table__filters", "tuiTextfieldSize", "m", 4, "ngIf"], ["cdkDropListGroup", "", 1, "mix-data-table__main-container"], [1, "mix-data-table__main-table", 3, "ngClass"], ["class", "mix-data-table__table-header", 4, "ngIf"], [3, "overlay", "showLoader"], ["tuiTable", "", 3, "columns", 4, "tuiLet"], ["class", "mix-data-table__transfer-icon", 4, "ngIf"], [1, "mix-data-table__sub-table", 3, "ngClass"], ["cdkDropList", ""], ["temporaryDrag", ""], ["tuiTextfieldSize", "m", 1, "mix-data-table__filters"], ["class", "input", "icon", "tuiIconSearchLarge", 3, "ngModel", "tuiTextfieldCleaner", "ngModelChange", 4, "ngIf"], [3, "content", 4, "ngIf"], ["icon", "tuiIconSearchLarge", 1, "input", 3, "ngModel", "tuiTextfieldCleaner", "ngModelChange"], [3, "content"], ["tuiButton", "", "size", "m", 3, "iconRight"], ["dropdown", ""], [1, "columns", 3, "items", "enabled", "itemsChange", "enabledChange"], [1, "mix-data-table__table-header"], ["tuiTable", "", 3, "columns"], ["tuiThGroup", ""], [4, "ngFor", "ngForOf"], ["cdkDropList", "", "tuiTbody", "", 3, "data", 4, "ngIf"], [4, "ngIf"], ["tuiTh", "", 3, "resizable", "sorter", 4, "tuiHead"], ["tuiTh", "", 3, "resizable", "sorter"], [4, "ngTemplateOutlet"], ["tuiTh", "", 3, "resizable", 4, "tuiHead"], ["tuiTh", "", 3, "resizable"], ["class", "mix-data-table__checkbox", "tuiTh", "", 3, "sorter", 4, "tuiHead"], ["tuiTh", "", 1, "mix-data-table__checkbox", 3, "sorter"], [1, "wrapper"], [3, "ngModel", "ngModelChange"], ["cdkDropList", "", "tuiTbody", "", 3, "data"], ["dropList", ""], ["cdkDrag", "", "tuiTr", "", 3, "cdkDragData", "cdkDragMoved", "cdkDragReleased", 4, "tuiRow", "tuiRowOf"], ["cdkDrag", "", "tuiTr", "", 3, "cdkDragData", "cdkDragMoved", "cdkDragReleased"], ["class", "mix-data-table__td", "tuiTd", "", 3, "ngClass", "ngStyle", 4, "tuiCell"], ["tuiTd", "", 1, "mix-data-table__td", 3, "ngClass", "ngStyle"], [4, "ngTemplateOutlet", "ngTemplateOutletContext"], ["class", "mix-data-table__td mix-data-table__checkbox", "tuiTd", "", 4, "tuiCell"], ["tuiTd", "", 1, "mix-data-table__td", "mix-data-table__checkbox"], [1, "mix-data-table__paging"], [3, "colSpan"], [1, "tui-space_top-2", 3, "length", "indexChange"], [1, "mix-data-table__transfer-icon"], ["name", "square-toggle"], ["subDropList", ""]], template: function MixDataTableComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0);
        i0.ɵɵtemplate(1, MixDataTableComponent_p_1_Template, 3, 2, "p", 1);
        i0.ɵɵelementStart(2, "div", 2)(3, "div", 3);
        i0.ɵɵtemplate(4, MixDataTableComponent_div_4_Template, 2, 0, "div", 4);
        i0.ɵɵelementStart(5, "tui-loader", 5);
        i0.ɵɵpipe(6, "async");
        i0.ɵɵtemplate(7, MixDataTableComponent_table_7_Template, 6, 4, "table", 6);
        i0.ɵɵpipe(8, "async");
        i0.ɵɵelementEnd()();
        i0.ɵɵtemplate(9, MixDataTableComponent_div_9_Template, 2, 0, "div", 7);
        i0.ɵɵelementStart(10, "div", 8);
        i0.ɵɵtemplate(11, MixDataTableComponent_div_11_Template, 2, 0, "div", 4);
        i0.ɵɵtemplate(12, MixDataTableComponent_table_12_Template, 5, 3, "table", 6);
        i0.ɵɵpipe(13, "async");
        i0.ɵɵelementEnd()();
        i0.ɵɵelement(14, "div", 9, 10);
        i0.ɵɵelementEnd();
    } if (rf & 2) {
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.reOrderable || ctx.searchable);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(16, _c6, ctx.showSubTable));
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.showSubTable);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("overlay", true)("showLoader", !!i0.ɵɵpipeBind1(6, 10, ctx.loading$));
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("tuiLet", i0.ɵɵpipeBind1(8, 12, ctx.data$));
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("ngIf", ctx.showSubTable);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(18, _c6, ctx.showSubTable));
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.showSubTable);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("tuiLet", i0.ɵɵpipeBind1(13, 14, ctx.data$));
    } }, dependencies: [i1.NgClass, i1.NgForOf, i1.NgIf, i1.NgTemplateOutlet, i1.NgStyle, i2.NgControlStatus, i2.NgModel, i3.TuiButtonComponent, i4.TuiTableDirective, i4.TuiTbodyComponent, i4.TuiThGroupComponent, i4.TuiThComponent, i4.TuiTdComponent, i4.TuiTrComponent, i4.TuiCellDirective, i4.TuiHeadDirective, i4.TuiRowDirective, i5.TuiInputComponent, i5.TuiInputDirective, i3.TuiHostedDropdownComponent, i4.TuiReorderComponent, i3.TuiTextfieldCleanerDirective, i3.TuiTextfieldSizeDirective, i6.TuiLetDirective, i3.TuiLoaderComponent, i5.TuiCheckboxComponent, i5.TuiPaginationComponent, i7.CdkDropList, i7.CdkDropListGroup, i7.CdkDrag, i8.TablerIconComponent, i1.AsyncPipe, i1.DatePipe, i9.RelativeTimeSpanPipe], styles: [".mix-data-table{width:100%;padding:0 15px}.mix-data-table__filters{width:100%;display:flex}.mix-data-table__filters .input{flex:1;margin-right:15px}.mix-data-table__additional-toolbar{margin-bottom:10px;display:flex;justify-content:flex-end;align-items:center}.mix-data-table__additional-toolbar button{margin-left:10px}.mix-data-table__main-container{width:100%;position:relative;overflow:hidden;display:flex}.mix-data-table__main-table{width:100%}.mix-data-table__main-table.--show{width:60%}.mix-data-table__main-table table{width:100%}.mix-data-table__transfer-icon{margin:0 15px;display:flex;justify-content:center}.mix-data-table__transfer-icon i-tabler{margin-top:100px;width:30px;height:30px;opacity:.3}.mix-data-table__sub-table{width:0px}.mix-data-table__sub-table.--show{width:40%}.mix-data-table__sub-table table{width:100%}.mix-data-table__sub-table .cdk-drag-placeholder .mix-data-table__checkbox,.mix-data-table__sub-table .cdk-drag-placeholder .mix-data-table__td.--hide-sub-table{display:none}.mix-data-table__table-header{width:100%;margin-bottom:10px;padding:15px 10px;background-color:var(--tui-base-01);border-radius:var(--mix-border-radius-01);border:1px solid var(--tui-base-03)}.mix-data-table th{border:none;height:60px;border-bottom:1px solid var(--tui-base-03)}.mix-data-table__td{border:none!important;border-bottom:1px solid var(--tui-base-03)!important;cursor:pointer}.mix-data-table__td.--action{width:10%}.mix-data-table tbody tr:last-child{height:60px}.mix-data-table tbody tr:last-child td{border-bottom:2px solid var(--tui-base-04)!important}.mix-data-table tbody tr:last-child td:first-child{border-bottom-left-radius:var(--mix-border-radius-01)}.mix-data-table tbody tr:last-child td:last-child{border-bottom-right-radius:var(--mix-border-radius-01)}.mix-data-table__checkbox{width:50px;position:relative}.mix-data-table__checkbox>.wrapper{display:flex;justify-content:center}.mix-data-table__checkbox tui-checkbox{position:absolute;top:50%;left:50%;transform:translate(-50%,-50%)}.mix-data-table__paging{height:65px}.mix-data-table tr:nth-child(2n+1)>.weme-data-table__td{background-color:var(--tui-base-02)}.mix-data-table tr:nth-child(2n+1)>.weme-data-table__td:first-of-type{border-top-left-radius:10px;border-bottom-left-radius:10px}.mix-data-table tr:nth-child(2n+1)>.weme-data-table__td:last-of-type{border-top-right-radius:10px;border-bottom-right-radius:10px}@keyframes ripple{to{transform:scale(1.1)}}\n"], encapsulation: 2, changeDetection: 0 });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixDataTableComponent, [{
        type: Component,
        args: [{ selector: 'mix-data-table', changeDetection: ChangeDetectionStrategy.OnPush, encapsulation: ViewEncapsulation.None, template: "<div class=\"mix-data-table\">\r\n  <p *ngIf=\"reOrderable || searchable\"\r\n     class=\"mix-data-table__filters\"\r\n     tuiTextfieldSize=\"m\">\r\n    <tui-input *ngIf=\"searchable\"\r\n               class=\"input\"\r\n               [ngModel]=\"search\"\r\n               [tuiTextfieldCleaner]=\"true\"\r\n               (ngModelChange)=\"searchText$.next($event)\"\r\n               icon=\"tuiIconSearchLarge\">\r\n      {{ searchPlaceholder }}\r\n    </tui-input>\r\n\r\n    <tui-hosted-dropdown *ngIf=\"reOrderable\"\r\n                         [content]=\"dropdown\">\r\n      <button [iconRight]=\"arrow\"\r\n              tuiButton\r\n              size=\"m\">Columns</button>\r\n      <ng-template #dropdown>\r\n        <tui-reorder class=\"columns\"\r\n                     [(items)]=\"tableSortFields\"\r\n                     [enabled]=\"tableSortFields\"\r\n                     (enabledChange)=\"onEnabled($event)\"></tui-reorder>\r\n      </ng-template>\r\n    </tui-hosted-dropdown>\r\n  </p>\r\n\r\n  <!-- TODO: Comment until feature stable -->\r\n  <!-- <div class=\"mix-data-table__additional-toolbar\">\r\n    <button *ngIf=\"showSubTable\"\r\n            [appearance]=\"'accent'\"\r\n            [size]=\"'xs'\"\r\n            (click)=\"showSubTable = false\"\r\n            tuiButton>Exit Drag & Drop Mode</button>\r\n\r\n    <button [size]=\"'xs'\"\r\n            (click)=\"showSubTable = true\"\r\n            tuiButton>Show Next Page</button>\r\n  </div> -->\r\n\r\n  <div class=\"mix-data-table__main-container\"\r\n       cdkDropListGroup>\r\n    <div class=\"mix-data-table__main-table\"\r\n         [ngClass]=\"{'--show': showSubTable}\">\r\n      <div *ngIf=\"showSubTable\"\r\n           class=\"mix-data-table__table-header\">\r\n        Current Page\r\n      </div>\r\n      <tui-loader [overlay]=\"true\"\r\n                  [showLoader]=\"!!(loading$ | async)\">\r\n        <table *tuiLet=\"data$ | async as data\"\r\n               [columns]=\"tableColumns\"\r\n               tuiTable>\r\n          <thead>\r\n            <tr tuiThGroup>\r\n              <ng-container *ngFor=\"let col of columns\">\r\n                <ng-container *ngIf=\"col.sortable === false && col.columnType !== 'CHECKBOX'\">\r\n                  <th *tuiHead=\"col.key\"\r\n                      [resizable]=\"true\"\r\n                      [sorter]=\"null\"\r\n                      tuiTh>\r\n                    <ng-container *ngIf=\"col.tplHeader\">\r\n                      <ng-container *ngTemplateOutlet=\"col.tplHeader.template\"></ng-container>\r\n                    </ng-container>\r\n\r\n                    <ng-container *ngIf=\"!col.tplHeader && col.showHeader\">\r\n                      {{ col.header }}\r\n                    </ng-container>\r\n                  </th>\r\n                </ng-container>\r\n\r\n                <ng-container *ngIf=\"col.sortable === true && col.columnType !== 'CHECKBOX'\">\r\n                  <th *tuiHead=\"col.key\"\r\n                      [resizable]=\"true\"\r\n                      tuiTh>\r\n                    <ng-container *ngIf=\"col.tplHeader\">\r\n                      <ng-container *ngTemplateOutlet=\"col.tplHeader.template\"></ng-container>\r\n                    </ng-container>\r\n\r\n                    <ng-container *ngIf=\"!col.tplHeader && col.showHeader\">\r\n                      {{ col.header }}\r\n                    </ng-container>\r\n                  </th>\r\n                </ng-container>\r\n\r\n                <ng-container *ngIf=\"col.columnType === 'CHECKBOX'\">\r\n                  <th *tuiHead=\"col.key\"\r\n                      class=\"mix-data-table__checkbox\"\r\n                      [sorter]=\"null\"\r\n                      tuiTh>\r\n                    <div class=\"wrapper\">\r\n                      <tui-checkbox [ngModel]=\"isAllSelected\"\r\n                                    (ngModelChange)=\"markAllChecked($event)\"></tui-checkbox>\r\n                    </div>\r\n                  </th>\r\n                </ng-container>\r\n              </ng-container>\r\n            </tr>\r\n          </thead>\r\n\r\n          <tbody #dropList\r\n                 *ngIf=\"data\"\r\n                 [data]=\"data.items\"\r\n                 cdkDropList\r\n                 tuiTbody>\r\n            <tr *tuiRow=\"let item of data.items\"\r\n                [cdkDragData]=\"item\"\r\n                (cdkDragMoved)=\"onDragItem($event)\"\r\n                (cdkDragReleased)=\"onReleaseDragItem()\"\r\n                cdkDrag\r\n                tuiTr>\r\n              <ng-container *ngFor=\"let col of columns\">\r\n                <ng-container *ngIf=\"col.columnType !== 'CHECKBOX'\">\r\n                  <td *tuiCell=\"col.key\"\r\n                      class=\"mix-data-table__td\"\r\n                      [ngClass]=\"{'--action': col.columnType === 'ACTION',\r\n                      '--date': col.columnType === 'DATE',\r\n                      '--hide-sub-table': col.showInSubTable === false}\"\r\n                      [ngStyle]=\"{'width': col.width}\"\r\n                      tuiTd>\r\n                    <ng-container *ngIf=\"col.tplCell\">\r\n                      <ng-container *ngTemplateOutlet=\"col.tplCell.template; context: { $implicit: item }\"></ng-container>\r\n                    </ng-container>\r\n                    <ng-container *ngIf=\"!col.tplCell && col.columnType === 'DATE'\">\r\n                      {{ $any(item)[col.key] ?? 'N/A' | date:'short' | relativeTimeSpan }}\r\n                    </ng-container>\r\n\r\n                    <ng-container *ngIf=\"!col.tplCell && col.columnType !== 'DATE'\">\r\n                      {{ $any(item)[col.key] ?? 'N/A' }}\r\n                    </ng-container>\r\n                  </td>\r\n                </ng-container>\r\n\r\n                <ng-container *ngIf=\"col.columnType === 'CHECKBOX'\">\r\n                  <td *tuiCell=\"col.key\"\r\n                      class=\"mix-data-table__td mix-data-table__checkbox\"\r\n                      tuiTd>\r\n                    <tui-checkbox [ngModel]=\"isItemSelected(item)\"\r\n                                  (ngModelChange)=\"onItemSelected($event, item)\">\r\n                    </tui-checkbox>\r\n                  </td>\r\n                </ng-container>\r\n              </ng-container>\r\n            </tr>\r\n          </tbody>\r\n\r\n          <tfoot *ngIf=\"data && !showSubTable\">\r\n            <tr class=\"mix-data-table__paging\">\r\n              <td [colSpan]=\"columns.length\">\r\n                <tui-pagination class=\"tui-space_top-2\"\r\n                                [length]=\"data.pagingData.totalPage || 0\"\r\n                                (indexChange)=\"onPageChange($event)\">\r\n                </tui-pagination>\r\n              </td>\r\n            </tr>\r\n          </tfoot>\r\n        </table>\r\n      </tui-loader>\r\n    </div>\r\n\r\n    <div *ngIf=\"showSubTable\"\r\n         class=\"mix-data-table__transfer-icon\">\r\n      <i-tabler name=\"square-toggle\"></i-tabler>\r\n    </div>\r\n\r\n    <div class=\"mix-data-table__sub-table\"\r\n         [ngClass]=\"{'--show': showSubTable}\">\r\n      <div *ngIf=\"showSubTable\"\r\n           class=\"mix-data-table__table-header\">\r\n        Next Page\r\n      </div>\r\n      <table *tuiLet=\"data$ | async as data\"\r\n             [columns]=\"subTableColumns\"\r\n             tuiTable>\r\n        <thead>\r\n          <tr tuiThGroup>\r\n            <ng-container *ngFor=\"let col of columns\">\r\n              <ng-container *ngIf=\"col.columnType !== 'CHECKBOX'\">\r\n                <th *tuiHead=\"col.key\"\r\n                    [resizable]=\"true\"\r\n                    [sorter]=\"null\"\r\n                    tuiTh>\r\n                  <ng-container *ngIf=\"col.tplHeader\">\r\n                    <ng-container *ngTemplateOutlet=\"col.tplHeader.template\"></ng-container>\r\n                  </ng-container>\r\n\r\n                  <ng-container *ngIf=\"!col.tplHeader && col.showHeader\">\r\n                    {{ col.header }}\r\n                  </ng-container>\r\n                </th>\r\n              </ng-container>\r\n            </ng-container>\r\n          </tr>\r\n        </thead>\r\n\r\n        <tbody #subDropList\r\n               *ngIf=\"data\"\r\n               [data]=\"data.items\"\r\n               cdkDropList\r\n               tuiTbody>\r\n          <tr *tuiRow=\"let item of data.items\"\r\n              [cdkDragData]=\"item\"\r\n              (cdkDragMoved)=\"onDragItem($event)\"\r\n              (cdkDragReleased)=\"onReleaseDragItem()\"\r\n              cdkDrag\r\n              tuiTr>\r\n            <ng-container *ngFor=\"let col of columns\">\r\n              <ng-container *ngIf=\"col.columnType !== 'CHECKBOX'\">\r\n                <td *tuiCell=\"col.key\"\r\n                    class=\"mix-data-table__td\"\r\n                    [ngClass]=\"{'--action': col.columnType === 'ACTION',\r\n                                   '--date': col.columnType === 'DATE'}\"\r\n                    [ngStyle]=\"{'width': col.width}\"\r\n                    tuiTd>\r\n                  <ng-container *ngIf=\"col.tplCell\">\r\n                    <ng-container *ngTemplateOutlet=\"col.tplCell.template; context: { $implicit: item }\"></ng-container>\r\n                  </ng-container>\r\n                  <ng-container *ngIf=\"!col.tplCell && col.columnType === 'DATE'\">\r\n                    {{ $any(item)[col.key] ?? 'N/A' | date:'short' | relativeTimeSpan }}\r\n                  </ng-container>\r\n\r\n                  <ng-container *ngIf=\"!col.tplCell && col.columnType !== 'DATE'\">\r\n                    {{ $any(item)[col.key] ?? 'N/A' }}\r\n                  </ng-container>\r\n                </td>\r\n              </ng-container>\r\n            </ng-container>\r\n          </tr>\r\n        </tbody>\r\n      </table>\r\n    </div>\r\n  </div>\r\n\r\n  <div #temporaryDrag\r\n       cdkDropList>\r\n  </div>\r\n</div>\r\n", styles: [".mix-data-table{width:100%;padding:0 15px}.mix-data-table__filters{width:100%;display:flex}.mix-data-table__filters .input{flex:1;margin-right:15px}.mix-data-table__additional-toolbar{margin-bottom:10px;display:flex;justify-content:flex-end;align-items:center}.mix-data-table__additional-toolbar button{margin-left:10px}.mix-data-table__main-container{width:100%;position:relative;overflow:hidden;display:flex}.mix-data-table__main-table{width:100%}.mix-data-table__main-table.--show{width:60%}.mix-data-table__main-table table{width:100%}.mix-data-table__transfer-icon{margin:0 15px;display:flex;justify-content:center}.mix-data-table__transfer-icon i-tabler{margin-top:100px;width:30px;height:30px;opacity:.3}.mix-data-table__sub-table{width:0px}.mix-data-table__sub-table.--show{width:40%}.mix-data-table__sub-table table{width:100%}.mix-data-table__sub-table .cdk-drag-placeholder .mix-data-table__checkbox,.mix-data-table__sub-table .cdk-drag-placeholder .mix-data-table__td.--hide-sub-table{display:none}.mix-data-table__table-header{width:100%;margin-bottom:10px;padding:15px 10px;background-color:var(--tui-base-01);border-radius:var(--mix-border-radius-01);border:1px solid var(--tui-base-03)}.mix-data-table th{border:none;height:60px;border-bottom:1px solid var(--tui-base-03)}.mix-data-table__td{border:none!important;border-bottom:1px solid var(--tui-base-03)!important;cursor:pointer}.mix-data-table__td.--action{width:10%}.mix-data-table tbody tr:last-child{height:60px}.mix-data-table tbody tr:last-child td{border-bottom:2px solid var(--tui-base-04)!important}.mix-data-table tbody tr:last-child td:first-child{border-bottom-left-radius:var(--mix-border-radius-01)}.mix-data-table tbody tr:last-child td:last-child{border-bottom-right-radius:var(--mix-border-radius-01)}.mix-data-table__checkbox{width:50px;position:relative}.mix-data-table__checkbox>.wrapper{display:flex;justify-content:center}.mix-data-table__checkbox tui-checkbox{position:absolute;top:50%;left:50%;transform:translate(-50%,-50%)}.mix-data-table__paging{height:65px}.mix-data-table tr:nth-child(2n+1)>.weme-data-table__td{background-color:var(--tui-base-02)}.mix-data-table tr:nth-child(2n+1)>.weme-data-table__td:first-of-type{border-top-left-radius:10px;border-bottom-left-radius:10px}.mix-data-table tr:nth-child(2n+1)>.weme-data-table__td:last-of-type{border-top-right-radius:10px;border-bottom-right-radius:10px}@keyframes ripple{to{transform:scale(1.1)}}\n"] }]
    }], function () { return [{ type: i0.ElementRef }]; }, { selfControl: [{
            type: Input
        }], fetchDataFn: [{
            type: Input
        }], data$: [{
            type: Input
        }], loading$: [{
            type: Input
        }], search: [{
            type: Input
        }], searchPlaceholder: [{
            type: Input
        }], totalRows: [{
            type: Input
        }], searchable: [{
            type: Input
        }], reOrderable: [{
            type: Input
        }], dataIndexKey: [{
            type: Input
        }], searchColumns: [{
            type: Input
        }], pageChange: [{
            type: Output
        }], pageSizeChange: [{
            type: Output
        }], tableQueryChange: [{
            type: Output
        }], itemsSelectedChange: [{
            type: Output
        }], subDropList: [{
            type: ViewChild,
            args: ['subDropList', { static: false }]
        }], dropList: [{
            type: ViewChild,
            args: ['dropList', { static: false }]
        }], columns: [{
            type: ContentChildren,
            args: [TableColumnDirective]
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGF0YS10YWJsZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL2NvbXBvbmVudHMvZGF0YS10YWJsZS9kYXRhLXRhYmxlLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9kYXRhLXRhYmxlL2RhdGEtdGFibGUuY29tcG9uZW50Lmh0bWwiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFlLFdBQVcsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBQ2xFLE9BQU8sRUFFTCx1QkFBdUIsRUFDdkIsU0FBUyxFQUNULGVBQWUsRUFDZixVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFFTCxNQUFNLEVBQ04sU0FBUyxFQUNULFNBQVMsRUFDVCxpQkFBaUIsRUFDbEIsTUFBTSxlQUFlLENBQUM7QUFLdkIsT0FBTyxFQUFFLFNBQVMsRUFBcUIsTUFBTSxlQUFlLENBQUM7QUFFN0QsT0FBTyxFQUNMLGVBQWUsRUFDZixVQUFVLEVBQ1YsYUFBYSxFQUNiLFlBQVksRUFDWixVQUFVLEVBQ1YsRUFBRSxFQUNGLFNBQVMsRUFDVCxPQUFPLEVBQ1AsU0FBUyxFQUNULEdBQUcsRUFDSixNQUFNLE1BQU0sQ0FBQztBQUVkLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLCtCQUErQixDQUFDOzs7Ozs7Ozs7Ozs7Ozs7SUM5QmpFLHFDQUtxQztJQUQxQix1TUFBaUIsZUFBQSwrQkFBd0IsQ0FBQSxJQUFDO0lBRW5ELFlBQ0Y7SUFBQSxpQkFBWTs7O0lBTEQsdUNBQWtCLDZCQUFBO0lBSTNCLGVBQ0Y7SUFERSx5REFDRjs7OztJQVFJLHVDQUdpRDtJQUZwQyxrUkFBMkIscU5BRVYsZUFBQSx5QkFBaUIsQ0FBQSxJQUZQO0lBRVMsaUJBQWM7OztJQUZsRCwrQ0FBMkIsb0NBQUE7OztJQVA1QywrQ0FDMEMsaUJBQUE7SUFHdkIsdUJBQU87SUFBQSxpQkFBUztJQUNqQyxrSkFLYztJQUNoQixpQkFBc0I7Ozs7SUFWRCw4QkFBb0I7SUFDL0IsZUFBbUI7SUFBbkIsd0NBQW1COzs7SUFkL0IsNkJBRXdCO0lBQ3RCLHVGQU9ZO0lBRVosMkdBV3NCO0lBQ3hCLGlCQUFJOzs7SUFyQlUsZUFBZ0I7SUFBaEIsd0NBQWdCO0lBU04sZUFBaUI7SUFBakIseUNBQWlCOzs7SUErQnJDLCtCQUMwQztJQUN4Qyw4QkFDRjtJQUFBLGlCQUFNOzs7SUFlVSx3QkFBd0U7OztJQUQxRSw2QkFBb0M7SUFDbEMsbUpBQXdFO0lBQzFFLDBCQUFlOzs7SUFERSxlQUF3QztJQUF4Qyw2REFBd0M7OztJQUd6RCw2QkFBdUQ7SUFDckQsWUFDRjtJQUFBLDBCQUFlOzs7SUFEYixlQUNGO0lBREUsK0NBQ0Y7OztJQVZGLDhCQUdVO0lBQ1Isb0lBRWU7SUFFZixvSUFFZTtJQUNqQixpQkFBSzs7O0lBVkQsZ0NBQWtCLGdCQUFBO0lBR0wsZUFBbUI7SUFBbkIsd0NBQW1CO0lBSW5CLGVBQXNDO0lBQXRDLCtEQUFzQzs7O0lBVHpELDZCQUE4RTtJQUM1RSwyR0FXSztJQUNQLDBCQUFlOzs7SUFaUixlQUFnQjtJQUFoQixxQ0FBZ0I7OztJQW1CakIsd0JBQXdFOzs7SUFEMUUsNkJBQW9DO0lBQ2xDLG1KQUF3RTtJQUMxRSwwQkFBZTs7O0lBREUsZUFBd0M7SUFBeEMsNkRBQXdDOzs7SUFHekQsNkJBQXVEO0lBQ3JELFlBQ0Y7SUFBQSwwQkFBZTs7O0lBRGIsZUFDRjtJQURFLCtDQUNGOzs7SUFURiw4QkFFVTtJQUNSLG9JQUVlO0lBRWYsb0lBRWU7SUFDakIsaUJBQUs7OztJQVRELGdDQUFrQjtJQUVMLGVBQW1CO0lBQW5CLHdDQUFtQjtJQUluQixlQUFzQztJQUF0QywrREFBc0M7OztJQVJ6RCw2QkFBNkU7SUFDM0UsMkdBVUs7SUFDUCwwQkFBZTs7O0lBWFIsZUFBZ0I7SUFBaEIscUNBQWdCOzs7O0lBY3JCLDhCQUdVLGNBQUEsdUJBQUE7SUFHUSxzT0FBaUIsZUFBQSw4QkFBc0IsQ0FBQSxJQUFDO0lBQUMsaUJBQWUsRUFBQSxFQUFBOzs7SUFKdEUsNkJBQWU7SUFHRCxlQUF5QjtJQUF6QiwrQ0FBeUI7OztJQU43Qyw2QkFBb0Q7SUFDbEQsMkdBUUs7SUFDUCwwQkFBZTs7O0lBVFIsZUFBZ0I7SUFBaEIscUNBQWdCOzs7SUEvQnpCLDZCQUEwQztJQUN4QyxnSEFhZTtJQUVmLGdIQVllO0lBRWYsZ0hBVWU7SUFDakIsMEJBQWU7OztJQXhDRSxlQUE2RDtJQUE3RCxzRkFBNkQ7SUFlN0QsZUFBNEQ7SUFBNUQscUZBQTREO0lBYzVELGVBQW1DO0lBQW5DLHdEQUFtQzs7O0lBb0M1Qyx3QkFBb0c7Ozs7SUFEdEcsNkJBQWtDO0lBQ2hDLGdLQUFvRztJQUN0RywwQkFBZTs7OztJQURFLGVBQXdDO0lBQXhDLDJEQUF3QyxpRUFBQTs7O0lBRXpELDZCQUFnRTtJQUM5RCxZQUNGOzs7SUFBQSwwQkFBZTs7Ozs7SUFEYixlQUNGO0lBREUsMktBQ0Y7OztJQUVBLDZCQUFnRTtJQUM5RCxZQUNGO0lBQUEsMEJBQWU7Ozs7O0lBRGIsZUFDRjtJQURFLHNIQUNGOzs7OztJQWhCRiw4QkFNVTtJQUNSLGlKQUVlO0lBQ2YsaUpBRWU7SUFFZixpSkFFZTtJQUNqQixpQkFBSzs7O0lBZkQsc0pBRWtELHNEQUFBO0lBR3JDLGVBQWlCO0lBQWpCLHNDQUFpQjtJQUdqQixlQUErQztJQUEvQyx3RUFBK0M7SUFJL0MsZUFBK0M7SUFBL0Msd0VBQStDOzs7SUFmbEUsNkJBQW9EO0lBQ2xELHlIQWlCSztJQUNQLDBCQUFlOzs7SUFsQlIsZUFBZ0I7SUFBaEIscUNBQWdCOzs7O0lBcUJyQiw4QkFFVSx1QkFBQTtJQUVNLG1TQUFpQixlQUFBLHdDQUE0QixDQUFBLElBQUM7SUFDNUQsaUJBQWUsRUFBQTs7OztJQUZELGVBQWdDO0lBQWhDLDBEQUFnQzs7O0lBSmxELDZCQUFvRDtJQUNsRCx3SEFNSztJQUNQLDBCQUFlOzs7SUFQUixlQUFnQjtJQUFoQixxQ0FBZ0I7OztJQXZCekIsNkJBQTBDO0lBQ3hDLDZIQW1CZTtJQUVmLDZIQVFlO0lBQ2pCLDBCQUFlOzs7SUE5QkUsZUFBbUM7SUFBbkMsd0RBQW1DO0lBcUJuQyxlQUFtQztJQUFuQyx3REFBbUM7Ozs7SUE1QnRELDhCQUtVO0lBSE4sb01BQWdCLGVBQUEsMEJBQWtCLENBQUEsSUFBQyx1TEFDaEIsZUFBQSwyQkFBbUIsQ0FBQSxJQURIO0lBSXJDLDhHQStCZTtJQUNqQixpQkFBSzs7OztJQXJDRCxzQ0FBb0I7SUFLUSxlQUFVO0lBQVYseUNBQVU7OztJQVg1QyxxQ0FJZ0I7SUFDZCxxRkFzQ0s7SUFDUCxpQkFBUTs7O0lBMUNELHFDQUFtQjtJQUdGLGVBQWE7SUFBYix5Q0FBYTs7OztJQXlDckMsNkJBQXFDLGFBQUEsYUFBQSx5QkFBQTtJQUtmLHlNQUFlLGVBQUEsNEJBQW9CLENBQUEsSUFBQztJQUNwRCxpQkFBaUIsRUFBQSxFQUFBLEVBQUE7Ozs7SUFKZixlQUEwQjtJQUExQixnREFBMEI7SUFFWixlQUF5QztJQUF6QywyREFBeUM7OztJQXBHakUsaUNBRWdCLFlBQUEsYUFBQTtJQUdWLGlHQXlDZTtJQUNqQixpQkFBSyxFQUFBO0lBR1AsbUZBNENRO0lBRVIsbUZBU1E7SUFDVixpQkFBUTs7OztJQXpHRCw2Q0FBd0I7SUFJSyxlQUFVO0lBQVYsd0NBQVU7SUE4Q3BDLGVBQVU7SUFBViwrQkFBVTtJQTZDVixlQUEyQjtJQUEzQix1REFBMkI7OztJQWN6QywrQkFDMkM7SUFDekMsK0JBQTBDO0lBQzVDLGlCQUFNOzs7SUFJSiwrQkFDMEM7SUFDeEMsMkJBQ0Y7SUFBQSxpQkFBTTs7O0lBYVEsd0JBQXdFOzs7SUFEMUUsNkJBQW9DO0lBQ2xDLG9KQUF3RTtJQUMxRSwwQkFBZTs7O0lBREUsZUFBd0M7SUFBeEMsNkRBQXdDOzs7SUFHekQsNkJBQXVEO0lBQ3JELFlBQ0Y7SUFBQSwwQkFBZTs7O0lBRGIsZUFDRjtJQURFLCtDQUNGOzs7SUFWRiw4QkFHVTtJQUNSLHFJQUVlO0lBRWYscUlBRWU7SUFDakIsaUJBQUs7OztJQVZELGdDQUFrQixnQkFBQTtJQUdMLGVBQW1CO0lBQW5CLHdDQUFtQjtJQUluQixlQUFzQztJQUF0QywrREFBc0M7OztJQVR6RCw2QkFBb0Q7SUFDbEQsNEdBV0s7SUFDUCwwQkFBZTs7O0lBWlIsZUFBZ0I7SUFBaEIscUNBQWdCOzs7SUFGekIsNkJBQTBDO0lBQ3hDLGlIQWFlO0lBQ2pCLDBCQUFlOzs7SUFkRSxlQUFtQztJQUFuQyx3REFBbUM7OztJQXNDNUMsd0JBQW9HOzs7SUFEdEcsNkJBQWtDO0lBQ2hDLGlLQUFvRztJQUN0RywwQkFBZTs7OztJQURFLGVBQXdDO0lBQXhDLDJEQUF3QyxpRUFBQTs7O0lBRXpELDZCQUFnRTtJQUM5RCxZQUNGOzs7SUFBQSwwQkFBZTs7Ozs7SUFEYixlQUNGO0lBREUsMktBQ0Y7OztJQUVBLDZCQUFnRTtJQUM5RCxZQUNGO0lBQUEsMEJBQWU7Ozs7O0lBRGIsZUFDRjtJQURFLHNIQUNGOzs7O0lBZkYsOEJBS1U7SUFDUixrSkFFZTtJQUNmLGtKQUVlO0lBRWYsa0pBRWU7SUFDakIsaUJBQUs7OztJQWRELG9IQUNvRCxzREFBQTtJQUd2QyxlQUFpQjtJQUFqQixzQ0FBaUI7SUFHakIsZUFBK0M7SUFBL0Msd0VBQStDO0lBSS9DLGVBQStDO0lBQS9DLHdFQUErQzs7O0lBZGxFLDZCQUFvRDtJQUNsRCwwSEFnQks7SUFDUCwwQkFBZTs7O0lBakJSLGVBQWdCO0lBQWhCLHFDQUFnQjs7O0lBRnpCLDZCQUEwQztJQUN4Qyw4SEFrQmU7SUFDakIsMEJBQWU7OztJQW5CRSxlQUFtQztJQUFuQyx3REFBbUM7Ozs7SUFQdEQsOEJBS1U7SUFITix1TUFBZ0IsZUFBQSwyQkFBa0IsQ0FBQSxJQUFDLDBMQUNoQixlQUFBLDRCQUFtQixDQUFBLElBREg7SUFJckMsK0dBb0JlO0lBQ2pCLGlCQUFLOzs7O0lBMUJELHNDQUFvQjtJQUtRLGVBQVU7SUFBVix5Q0FBVTs7O0lBWDVDLHFDQUlnQjtJQUNkLHNGQTJCSztJQUNQLGlCQUFROzs7SUEvQkQscUNBQW1CO0lBR0YsZUFBYTtJQUFiLHlDQUFhOzs7SUE3QnZDLGlDQUVnQixZQUFBLGFBQUE7SUFHVixrR0FlZTtJQUNqQixpQkFBSyxFQUFBO0lBR1Asb0ZBaUNRO0lBQ1YsaUJBQVE7Ozs7SUF6REQsZ0RBQTJCO0lBSUUsZUFBVTtJQUFWLHdDQUFVO0lBb0JwQyxlQUFVO0lBQVYsK0JBQVU7OztBRHpKMUIsTUFBTSxPQUFPLHFCQUFxQjtJQTRFaEMsWUFBb0IsVUFBc0I7UUFBdEIsZUFBVSxHQUFWLFVBQVUsQ0FBWTtRQTNFbkMsd0JBQW1CLEdBQVEsRUFBRSxDQUFDO1FBQzlCLGVBQVUsR0FBUSxFQUFFLENBQUM7UUFDckIsZ0JBQVcsR0FBRyxDQUFDLENBQUM7UUFDaEIsa0JBQWEsR0FBRyxLQUFLLENBQUM7UUFFYixnQkFBVyxHQUFHLElBQUksQ0FBQztRQUtuQixhQUFRLEdBQ3RCLElBQUksZUFBZSxDQUFVLElBQUksQ0FBQyxDQUFDO1FBQ3JCLFdBQU0sR0FBRyxFQUFFLENBQUM7UUFDWixzQkFBaUIsR0FBRyxRQUFRLENBQUM7UUFDN0IsY0FBUyxHQUFHLENBQUMsQ0FBQztRQUNkLGVBQVUsR0FBRyxJQUFJLENBQUM7UUFDbEIsZ0JBQVcsR0FBRyxJQUFJLENBQUM7UUFDbkIsaUJBQVksR0FBRyxJQUFJLENBQUM7UUFDcEIsa0JBQWEsR0FBRyxPQUFPLENBQUM7UUFFdkIsZUFBVSxHQUF5QixJQUFJLFlBQVksRUFBRSxDQUFDO1FBQ3RELG1CQUFjLEdBQXlCLElBQUksWUFBWSxFQUFFLENBQUM7UUFDMUQscUJBQWdCLEdBQy9CLElBQUksWUFBWSxFQUFFLENBQUM7UUFDSix3QkFBbUIsR0FBc0IsSUFBSSxZQUFZLEVBQUUsQ0FBQztRQVF0RSx3QkFBbUIsR0FBYSxFQUFFLENBQUM7UUFDbkMsaUJBQVksR0FBYSxFQUFFLENBQUM7UUFDNUIsb0JBQWUsR0FBYSxFQUFFLENBQUM7UUFDL0Isd0JBQW1CLEdBQWEsRUFBRSxDQUFDO1FBQ25DLG9CQUFlLEdBQXNCLEVBQUUsQ0FBQztRQUN4QyxjQUFTLEdBQTJCLEVBQUUsQ0FBQztRQUN2QyxpQkFBWSxHQUFHLEtBQUssQ0FBQztRQUVaLGlCQUFZLEdBQzFCLElBQUksZUFBZSxDQUFVLEtBQUssQ0FBQyxDQUFDO1FBQ3RCLGtCQUFhLEdBQzNCLElBQUksZUFBZSxDQUFVLEtBQUssQ0FBQyxDQUFDO1FBQ3RCLFVBQUssR0FDbkIsU0FBUyxDQUFDO1FBQ0ksZ0JBQVcsR0FBNEIsSUFBSSxlQUFlLENBQ3hFLEVBQUUsQ0FDSCxDQUFDO1FBQ2MsVUFBSyxHQUFvQixJQUFJLE9BQU8sRUFBRSxDQUFDO1FBQ3ZDLFVBQUssR0FBb0IsSUFBSSxPQUFPLEVBQUUsQ0FBQztRQUN2QyxlQUFVLEdBQW9CLElBQUksT0FBTyxFQUFFLENBQUM7UUFDNUMsZUFBVSxHQUE0QixJQUFJLGVBQWUsQ0FFdkUsQ0FBQyxDQUFDLENBQUM7UUFDVyxZQUFPLEdBQ3JCLElBQUksZUFBZSxDQUFVLEtBQUssQ0FBQyxDQUFDO1FBQ3RCLGNBQVMsR0FBNkI7WUFDcEQsS0FBSyxFQUFFLEVBQUU7WUFDVCxVQUFVLEVBQUU7Z0JBQ1YsU0FBUyxFQUFFLENBQUM7Z0JBQ1osUUFBUSxFQUFFLEVBQUU7Z0JBQ1osS0FBSyxFQUFFLENBQUM7YUFDVDtTQUNGLENBQUM7UUFFSyxhQUFRLEdBQ2IsYUFBYSxDQUFDO1lBQ1osSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsWUFBWSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1lBQ3hDLElBQUksQ0FBQyxVQUFVO1lBQ2YsSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxDQUFDO1lBQzdCLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQztZQUM5QixJQUFJLENBQUMsT0FBTztTQUNiLENBQUMsQ0FBQztJQUV3QyxDQUFDO0lBRXZDLFFBQVE7UUFDYixJQUFJLElBQUksQ0FBQyxXQUFXLEVBQUU7WUFDcEIsSUFBSSxDQUFDLGlCQUFpQixFQUFFLENBQUM7U0FDMUI7YUFBTTtZQUNMLElBQUksQ0FBQyxRQUFRLENBQUMsU0FBUyxDQUNyQixDQUFDLEtBQWdELEVBQUUsRUFBRTtnQkFDbkQsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQztvQkFDekIsT0FBTyxFQUFFLEtBQUssQ0FBQyxDQUFDLENBQUM7b0JBQ2pCLFNBQVMsRUFBRSxLQUFLLENBQUMsQ0FBQyxDQUFDO29CQUNuQixRQUFRLEVBQUUsS0FBSyxDQUFDLENBQUMsQ0FBQztpQkFDbkIsQ0FBQyxDQUFDO1lBQ0wsQ0FBQyxDQUNGLENBQUM7U0FDSDtJQUNILENBQUM7SUFFTSxpQkFBaUI7UUFDdEIsNEVBQTRFO1FBQzVFLDRFQUE0RTtRQUM1RSxzR0FBc0c7UUFDdEcsbURBQW1EO1FBQ25ELHNDQUFzQztRQUN0QyxvQ0FBb0M7UUFDcEMsMkRBQTJEO1FBQzNELHFDQUFxQztRQUNyQyxxQ0FBcUM7UUFDckMsYUFBYTtRQUNiLHNDQUFzQztRQUN0QyxxQ0FBcUM7UUFDckMsTUFBTTtRQUNOLE1BQU07UUFDTiw2REFBNkQ7UUFDN0Qsb0JBQW9CO1FBQ3BCLDhCQUE4QjtRQUM5QixNQUFNO1FBQ04sOERBQThEO1FBQzlELG9CQUFvQjtRQUNwQiw4QkFBOEI7UUFDOUIsTUFBTTtJQUNSLENBQUM7SUFFTSxjQUFjLENBQUMsSUFBTztRQUMzQixPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsbUJBQW1CLENBQUMsSUFBSSxDQUNwQyxDQUFDLENBQUksRUFBRSxFQUFFLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsS0FBSyxJQUFJLENBQUMsU0FBUyxDQUFDLElBQUksQ0FBQyxDQUNyRCxDQUFDO0lBQ0osQ0FBQztJQUVNLGtCQUFrQjtRQUN2QixNQUFNLE9BQU8sR0FBMkIsSUFBSSxDQUFDLE9BQU8sQ0FBQyxPQUFPLEVBQUUsQ0FBQztRQUMvRCxJQUFJLENBQUMsU0FBUyxHQUFHLElBQUksQ0FBQyxzQkFBc0IsQ0FBQyxPQUFPLENBQUMsQ0FBQztRQUN0RCxJQUFJLENBQUMsbUJBQW1CLEdBQUcsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQXVCLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUMzRSxJQUFJLENBQUMsWUFBWSxHQUFHLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUF1QixFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDcEUsSUFBSSxDQUFDLGVBQWUsR0FBRyxPQUFPO2FBQzNCLE1BQU0sQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxVQUFVLEtBQUssVUFBVSxJQUFJLENBQUMsQ0FBQyxjQUFjLEtBQUssSUFBSSxDQUFDO2FBQ3JFLEdBQUcsQ0FBQyxDQUFDLENBQXVCLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUMzQyxJQUFJLENBQUMsZUFBZSxHQUFHLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUF1QixFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUM7SUFDNUUsQ0FBQztJQUVNLFNBQVMsQ0FBQyxPQUEwQjtRQUN6QyxJQUFJLENBQUMsWUFBWSxHQUFHLElBQUksQ0FBQyxlQUFlO2FBQ3JDLE1BQU0sQ0FBQyxDQUFDLEdBQVcsRUFBRSxFQUFFLENBQUMsT0FBTyxDQUFDLFFBQVEsQ0FBQyxHQUFHLENBQUMsQ0FBQzthQUM5QyxHQUFHLENBQUMsQ0FBQyxDQUFTLEVBQUUsRUFBRSxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUMzQyxDQUFDO0lBRU0sWUFBWSxDQUFDLElBQVk7UUFDOUIsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7UUFDM0IsSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7SUFDeEIsQ0FBQztJQUVNLFlBQVksQ0FBQyxJQUFZO1FBQzlCLElBQUksQ0FBQyxjQUFjLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDO1FBQy9CLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDO0lBQ3hCLENBQUM7SUFFTSxjQUFjLENBQUMsS0FBYyxFQUFFLElBQU87UUFDM0MsSUFBSSxLQUFLLEVBQUU7WUFDVCxJQUFJLENBQUMsbUJBQW1CLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDO1NBQ3JDO2FBQU07WUFDTCxJQUFJLENBQUMsbUJBQW1CLEdBQUcsSUFBSSxDQUFDLG1CQUFtQixDQUFDLE1BQU0sQ0FDeEQsQ0FBQyxDQUFJLEVBQUUsRUFBRSxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsSUFBSSxDQUFDLEtBQUssSUFBSSxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FDckQsQ0FBQztTQUNIO1FBRUQsSUFBSSxDQUFDLGFBQWE7WUFDaEIsSUFBSSxDQUFDLG1CQUFtQixDQUFDLE1BQU0sS0FBSyxJQUFJLENBQUMsVUFBVSxDQUFDLE1BQU0sQ0FBQztRQUM3RCxJQUFJLENBQUMsbUJBQW1CLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxtQkFBbUIsQ0FBQyxDQUFDO0lBQzFELENBQUM7SUFFTSxjQUFjLENBQUMsS0FBYztRQUNsQyxJQUFJLEtBQUssRUFBRTtZQUNULElBQUksQ0FBQyxtQkFBbUIsR0FBRyxJQUFJLENBQUMsVUFBVSxDQUFDO1NBQzVDO2FBQU07WUFDTCxJQUFJLENBQUMsbUJBQW1CLEdBQUcsRUFBRSxDQUFDO1NBQy9CO1FBRUQsSUFBSSxDQUFDLGFBQWEsR0FBRyxLQUFLLENBQUM7UUFDM0IsSUFBSSxDQUFDLG1CQUFtQixDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsbUJBQW1CLENBQUMsQ0FBQztJQUMxRCxDQUFDO0lBRU0sbUJBQW1CLENBQUMsVUFBa0IsRUFBRSxRQUFnQjtRQUM3RCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsVUFBVSxHQUFHLFFBQVEsQ0FBQyxJQUFJLENBQUMsQ0FBQztJQUNoRCxDQUFDO0lBRU0sVUFBVTtRQUNmLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsQ0FBQyxDQUFDO0lBQzlDLENBQUM7SUFFTSxVQUFVLENBQUMsS0FBa0I7UUFDbEMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGVBQWUsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUNoRCxDQUFDO0lBRU0saUJBQWlCO1FBQ3RCLGlDQUFpQztRQUNqQyxrQ0FBa0M7UUFDbEMsNkJBQTZCO0lBQy9CLENBQUM7SUFFTyxxQkFBcUIsQ0FDM0IsVUFBa0IsRUFDbEIsSUFBWSxFQUNaLFFBQWdCO1FBRWhCLE9BQU8sSUFBSSxDQUFDLFdBQVcsQ0FBQztZQUN0QixPQUFPLEVBQUUsVUFBVTtZQUNuQixTQUFTLEVBQUUsSUFBSTtZQUNmLFFBQVEsRUFBRSxRQUFRO1lBQ2xCLGFBQWEsRUFBRSxJQUFJLENBQUMsYUFBYTtZQUNqQyxZQUFZLEVBQUUsTUFBTTtTQUNyQixDQUFDLENBQUM7SUFDTCxDQUFDO0lBRU8sWUFBWTtRQUNsQixJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztJQUMzQixDQUFDO0lBRU8sWUFBWTtRQUNsQixJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUM1QixDQUFDO0lBRU8sc0JBQXNCLENBQzVCLE9BQStCO1FBRS9CLE9BQU8sT0FBTyxDQUFDLE1BQU0sQ0FDbkIsQ0FBQyxHQUFXLEVBQUUsSUFBMEIsRUFBRSxFQUFFLENBQUMsQ0FBQztZQUM1QyxHQUFHLEdBQUc7WUFDTixDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsRUFBRSxJQUFJLENBQUMsR0FBRztTQUN4QixDQUFDLEVBQ0YsRUFBRSxDQUNILENBQUM7SUFDSixDQUFDO0lBRU8saUJBQWlCO1FBQ3ZCLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQzdCLEdBQUcsQ0FBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQUMsWUFBWSxFQUFFLENBQUMsRUFDOUIsU0FBUyxDQUFDLENBQUMsS0FBZ0QsRUFBRSxFQUFFLENBQzdELElBQUksQ0FBQyxxQkFBcUIsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEVBQUUsS0FBSyxDQUFDLENBQUMsQ0FBQyxFQUFFLEtBQUssQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUN6RCxFQUNELEdBQUcsQ0FBQyxDQUFDLEdBQTZCLEVBQUUsRUFBRTtZQUNwQyxJQUFJLENBQUMsWUFBWSxFQUFFLENBQUM7WUFDcEIsSUFBSSxDQUFDLFVBQVUsR0FBRyxHQUFHLENBQUMsS0FBSyxDQUFDO1lBQzVCLElBQUksQ0FBQyxXQUFXLEdBQUcsR0FBRyxDQUFDLFVBQVUsQ0FBQyxTQUFTLENBQUM7UUFDOUMsQ0FBQyxDQUFDLEVBQ0YsU0FBUyxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsRUFDekIsVUFBVSxDQUFDLEdBQUcsRUFBRTtZQUNkLElBQUksQ0FBQyxZQUFZLEVBQUUsQ0FBQztZQUNwQixPQUFPLEVBQUUsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7UUFDNUIsQ0FBQyxDQUFDLENBQ0gsQ0FBQztJQUNKLENBQUM7OzBGQXRQVSxxQkFBcUI7d0VBQXJCLHFCQUFxQjtvQ0E4QmYsb0JBQW9COzs7Ozs7Ozs7Ozs7UUN6RXZDLDhCQUE0QjtRQUMxQixrRUF3Qkk7UUFlSiw4QkFDc0IsYUFBQTtRQUdsQixzRUFHTTtRQUNOLHFDQUNnRDs7UUFDOUMsMEVBMEdROztRQUNWLGlCQUFhLEVBQUE7UUFHZixzRUFHTTtRQUVOLCtCQUMwQztRQUN4Qyx3RUFHTTtRQUNOLDRFQTBEUTs7UUFDVixpQkFBTSxFQUFBO1FBR1IsOEJBRU07UUFDUixpQkFBTTs7UUEzT0EsZUFBK0I7UUFBL0Isd0RBQStCO1FBMEM1QixlQUFvQztRQUFwQyx1RUFBb0M7UUFDakMsZUFBa0I7UUFBbEIsdUNBQWtCO1FBSVosZUFBZ0I7UUFBaEIsOEJBQWdCLHFEQUFBO1FBRWxCLGVBQXNCO1FBQXRCLHlEQUFzQjtRQThHNUIsZUFBa0I7UUFBbEIsdUNBQWtCO1FBTW5CLGVBQW9DO1FBQXBDLHVFQUFvQztRQUNqQyxlQUFrQjtRQUFsQix1Q0FBa0I7UUFJaEIsZUFBc0I7UUFBdEIsMERBQXNCOzt1RkRoSXZCLHFCQUFxQjtjQVBqQyxTQUFTOzJCQUNFLGdCQUFnQixtQkFHVCx1QkFBdUIsQ0FBQyxNQUFNLGlCQUNoQyxpQkFBaUIsQ0FBQyxJQUFJOzZEQVFyQixXQUFXO2tCQUExQixLQUFLO1lBQ1UsV0FBVztrQkFBMUIsS0FBSztZQUdVLEtBQUs7a0JBQXBCLEtBQUs7WUFDVSxRQUFRO2tCQUF2QixLQUFLO1lBRVUsTUFBTTtrQkFBckIsS0FBSztZQUNVLGlCQUFpQjtrQkFBaEMsS0FBSztZQUNVLFNBQVM7a0JBQXhCLEtBQUs7WUFDVSxVQUFVO2tCQUF6QixLQUFLO1lBQ1UsV0FBVztrQkFBMUIsS0FBSztZQUNVLFlBQVk7a0JBQTNCLEtBQUs7WUFDVSxhQUFhO2tCQUE1QixLQUFLO1lBRVcsVUFBVTtrQkFBMUIsTUFBTTtZQUNVLGNBQWM7a0JBQTlCLE1BQU07WUFDVSxnQkFBZ0I7a0JBQWhDLE1BQU07WUFFVSxtQkFBbUI7a0JBQW5DLE1BQU07WUFFNkMsV0FBVztrQkFBOUQsU0FBUzttQkFBQyxhQUFhLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFO1lBQ00sUUFBUTtrQkFBeEQsU0FBUzttQkFBQyxVQUFVLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFO1lBR2pDLE9BQU87a0JBRGIsZUFBZTttQkFBQyxvQkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDZGtEcmFnTW92ZSwgRHJvcExpc3RSZWYgfSBmcm9tICdAYW5ndWxhci9jZGsvZHJhZy1kcm9wJztcclxuaW1wb3J0IHtcclxuICBBZnRlckNvbnRlbnRJbml0LFxyXG4gIENoYW5nZURldGVjdGlvblN0cmF0ZWd5LFxyXG4gIENvbXBvbmVudCxcclxuICBDb250ZW50Q2hpbGRyZW4sXHJcbiAgRWxlbWVudFJlZixcclxuICBFdmVudEVtaXR0ZXIsXHJcbiAgSW5wdXQsXHJcbiAgT25Jbml0LFxyXG4gIE91dHB1dCxcclxuICBRdWVyeUxpc3QsXHJcbiAgVmlld0NoaWxkLFxyXG4gIFZpZXdFbmNhcHN1bGF0aW9uXHJcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7XHJcbiAgUGFnaW5hdGlvblJlcXVlc3RNb2RlbCxcclxuICBQYWdpbmF0aW9uUmVzdWx0TW9kZWxcclxufSBmcm9tICdAbWl4LXNwYS9taXgubGliJztcclxuaW1wb3J0IHsgVFVJX0FSUk9XLCBUdWlBcnJvd0NvbXBvbmVudCB9IGZyb20gJ0B0YWlnYS11aS9raXQnO1xyXG5pbXBvcnQgeyBQb2x5bW9ycGhldXNDb21wb25lbnQgfSBmcm9tICdAdGlua29mZi9uZy1wb2x5bW9ycGhldXMnO1xyXG5pbXBvcnQge1xyXG4gIEJlaGF2aW9yU3ViamVjdCxcclxuICBjYXRjaEVycm9yLFxyXG4gIGNvbWJpbmVMYXRlc3QsXHJcbiAgZGVib3VuY2VUaW1lLFxyXG4gIE9ic2VydmFibGUsXHJcbiAgb2YsXHJcbiAgc3RhcnRXaXRoLFxyXG4gIFN1YmplY3QsXHJcbiAgc3dpdGNoTWFwLFxyXG4gIHRhcFxyXG59IGZyb20gJ3J4anMnO1xyXG5cclxuaW1wb3J0IHsgVGFibGVDb2x1bW5EaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvY29sdW1uLmRpcmVjdGl2ZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC1kYXRhLXRhYmxlJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vZGF0YS10YWJsZS5jb21wb25lbnQuaHRtbCcsXHJcbiAgc3R5bGVVcmxzOiBbJy4vZGF0YS10YWJsZS5jb21wb25lbnQuc2NzcyddLFxyXG4gIGNoYW5nZURldGVjdGlvbjogQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3kuT25QdXNoLFxyXG4gIGVuY2Fwc3VsYXRpb246IFZpZXdFbmNhcHN1bGF0aW9uLk5vbmVcclxufSlcclxuZXhwb3J0IGNsYXNzIE1peERhdGFUYWJsZUNvbXBvbmVudDxUPiBpbXBsZW1lbnRzIEFmdGVyQ29udGVudEluaXQsIE9uSW5pdCB7XHJcbiAgcHVibGljIGN1cnJlbnRTZWxlY3RlZEl0ZW06IFRbXSA9IFtdO1xyXG4gIHB1YmxpYyBjYWNoZUl0ZW1zOiBUW10gPSBbXTtcclxuICBwdWJsaWMgY3VycmVudFBhZ2UgPSAwO1xyXG4gIHB1YmxpYyBpc0FsbFNlbGVjdGVkID0gZmFsc2U7XHJcblxyXG4gIEBJbnB1dCgpIHB1YmxpYyBzZWxmQ29udHJvbCA9IHRydWU7XHJcbiAgQElucHV0KCkgcHVibGljIGZldGNoRGF0YUZuITogKFxyXG4gICAgZmlsdGVyOiBQYWdpbmF0aW9uUmVxdWVzdE1vZGVsXHJcbiAgKSA9PiBPYnNlcnZhYmxlPFBhZ2luYXRpb25SZXN1bHRNb2RlbDxUPj47XHJcbiAgQElucHV0KCkgcHVibGljIGRhdGEkITogT2JzZXJ2YWJsZTxQYWdpbmF0aW9uUmVzdWx0TW9kZWw8VD4+O1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBsb2FkaW5nJDogQmVoYXZpb3JTdWJqZWN0PGJvb2xlYW4+ID1cclxuICAgIG5ldyBCZWhhdmlvclN1YmplY3Q8Ym9vbGVhbj4odHJ1ZSk7XHJcbiAgQElucHV0KCkgcHVibGljIHNlYXJjaCA9ICcnO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBzZWFyY2hQbGFjZWhvbGRlciA9ICdTZWFyY2gnO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyB0b3RhbFJvd3MgPSAwO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBzZWFyY2hhYmxlID0gdHJ1ZTtcclxuICBASW5wdXQoKSBwdWJsaWMgcmVPcmRlcmFibGUgPSB0cnVlO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBkYXRhSW5kZXhLZXkgPSAnaWQnO1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBzZWFyY2hDb2x1bW5zID0gJ3RpdGxlJztcclxuXHJcbiAgQE91dHB1dCgpIHB1YmxpYyBwYWdlQ2hhbmdlOiBFdmVudEVtaXR0ZXI8bnVtYmVyPiA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcclxuICBAT3V0cHV0KCkgcHVibGljIHBhZ2VTaXplQ2hhbmdlOiBFdmVudEVtaXR0ZXI8bnVtYmVyPiA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcclxuICBAT3V0cHV0KCkgcHVibGljIHRhYmxlUXVlcnlDaGFuZ2U6IEV2ZW50RW1pdHRlcjxQYWdpbmF0aW9uUmVxdWVzdE1vZGVsPiA9XHJcbiAgICBuZXcgRXZlbnRFbWl0dGVyKCk7XHJcbiAgQE91dHB1dCgpIHB1YmxpYyBpdGVtc1NlbGVjdGVkQ2hhbmdlOiBFdmVudEVtaXR0ZXI8VFtdPiA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcclxuXHJcbiAgQFZpZXdDaGlsZCgnc3ViRHJvcExpc3QnLCB7IHN0YXRpYzogZmFsc2UgfSkgcHVibGljIHN1YkRyb3BMaXN0ITogRHJvcExpc3RSZWY7XHJcbiAgQFZpZXdDaGlsZCgnZHJvcExpc3QnLCB7IHN0YXRpYzogZmFsc2UgfSkgcHVibGljIGRyb3BMaXN0ITogRHJvcExpc3RSZWY7XHJcblxyXG4gIEBDb250ZW50Q2hpbGRyZW4oVGFibGVDb2x1bW5EaXJlY3RpdmUpXHJcbiAgcHVibGljIGNvbHVtbnMhOiBRdWVyeUxpc3Q8VGFibGVDb2x1bW5EaXJlY3RpdmU+O1xyXG5cclxuICBwdWJsaWMgdGFibGVJbml0aWFsQ29sdW1uczogc3RyaW5nW10gPSBbXTtcclxuICBwdWJsaWMgdGFibGVDb2x1bW5zOiBzdHJpbmdbXSA9IFtdO1xyXG4gIHB1YmxpYyBzdWJUYWJsZUNvbHVtbnM6IHN0cmluZ1tdID0gW107XHJcbiAgcHVibGljIHRhYmxlRW5hYmxlZENvbHVtbnM6IHN0cmluZ1tdID0gW107XHJcbiAgcHVibGljIHRhYmxlU29ydEZpZWxkczogcmVhZG9ubHkgc3RyaW5nW10gPSBbXTtcclxuICBwdWJsaWMgY29sdW1uRGljOiBSZWNvcmQ8c3RyaW5nLCBzdHJpbmc+ID0ge307XHJcbiAgcHVibGljIHNob3dTdWJUYWJsZSA9IGZhbHNlO1xyXG5cclxuICBwdWJsaWMgcmVhZG9ubHkgc2hvd0RyYWdMZWZ0OiBCZWhhdmlvclN1YmplY3Q8Ym9vbGVhbj4gPVxyXG4gICAgbmV3IEJlaGF2aW9yU3ViamVjdDxib29sZWFuPihmYWxzZSk7XHJcbiAgcHVibGljIHJlYWRvbmx5IHNob3dEcmFnUmlnaHQ6IEJlaGF2aW9yU3ViamVjdDxib29sZWFuPiA9XHJcbiAgICBuZXcgQmVoYXZpb3JTdWJqZWN0PGJvb2xlYW4+KGZhbHNlKTtcclxuICBwdWJsaWMgcmVhZG9ubHkgYXJyb3c6IFBvbHltb3JwaGV1c0NvbXBvbmVudDxUdWlBcnJvd0NvbXBvbmVudCwgb2JqZWN0PiA9XHJcbiAgICBUVUlfQVJST1c7XHJcbiAgcHVibGljIHJlYWRvbmx5IHNlYXJjaFRleHQkOiBCZWhhdmlvclN1YmplY3Q8c3RyaW5nPiA9IG5ldyBCZWhhdmlvclN1YmplY3QoXHJcbiAgICAnJ1xyXG4gICk7XHJcbiAgcHVibGljIHJlYWRvbmx5IHNpemUkOiBTdWJqZWN0PG51bWJlcj4gPSBuZXcgU3ViamVjdCgpO1xyXG4gIHB1YmxpYyByZWFkb25seSBwYWdlJDogU3ViamVjdDxudW1iZXI+ID0gbmV3IFN1YmplY3QoKTtcclxuICBwdWJsaWMgcmVhZG9ubHkgZHJhZ0NoYW5nZTogU3ViamVjdDxudW1iZXI+ID0gbmV3IFN1YmplY3QoKTtcclxuICBwdWJsaWMgcmVhZG9ubHkgZGlyZWN0aW9uJDogQmVoYXZpb3JTdWJqZWN0PDEgfCAtMT4gPSBuZXcgQmVoYXZpb3JTdWJqZWN0PFxyXG4gICAgLTEgfCAxXHJcbiAgPigxKTtcclxuICBwdWJsaWMgcmVhZG9ubHkgcmVsb2FkJDogQmVoYXZpb3JTdWJqZWN0PGJvb2xlYW4+ID1cclxuICAgIG5ldyBCZWhhdmlvclN1YmplY3Q8Ym9vbGVhbj4oZmFsc2UpO1xyXG4gIHB1YmxpYyByZWFkb25seSBlbXB0eURhdGE6IFBhZ2luYXRpb25SZXN1bHRNb2RlbDxUPiA9IHtcclxuICAgIGl0ZW1zOiBbXSxcclxuICAgIHBhZ2luZ0RhdGE6IHtcclxuICAgICAgcGFnZUluZGV4OiAwLFxyXG4gICAgICBwYWdlU2l6ZTogMjUsXHJcbiAgICAgIHRvdGFsOiAwXHJcbiAgICB9XHJcbiAgfTtcclxuXHJcbiAgcHVibGljIHJlcXVlc3QkOiBPYnNlcnZhYmxlPFtzdHJpbmcsIDEgfCAtMSwgbnVtYmVyLCBudW1iZXIsIGJvb2xlYW5dPiA9XHJcbiAgICBjb21iaW5lTGF0ZXN0KFtcclxuICAgICAgdGhpcy5zZWFyY2hUZXh0JC5waXBlKGRlYm91bmNlVGltZSgzMDApKSxcclxuICAgICAgdGhpcy5kaXJlY3Rpb24kLFxyXG4gICAgICB0aGlzLnBhZ2UkLnBpcGUoc3RhcnRXaXRoKDApKSxcclxuICAgICAgdGhpcy5zaXplJC5waXBlKHN0YXJ0V2l0aCgxMCkpLFxyXG4gICAgICB0aGlzLnJlbG9hZCRcclxuICAgIF0pO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGVsZW1lbnRSZWY6IEVsZW1lbnRSZWYpIHt9XHJcblxyXG4gIHB1YmxpYyBuZ09uSW5pdCgpOiB2b2lkIHtcclxuICAgIGlmICh0aGlzLnNlbGZDb250cm9sKSB7XHJcbiAgICAgIHRoaXMuX3NldHVwU2VsZkNvbnRyb2woKTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIHRoaXMucmVxdWVzdCQuc3Vic2NyaWJlKFxyXG4gICAgICAgIChxdWVyeTogW3N0cmluZywgMSB8IC0xLCBudW1iZXIsIG51bWJlciwgYm9vbGVhbl0pID0+IHtcclxuICAgICAgICAgIHRoaXMudGFibGVRdWVyeUNoYW5nZS5lbWl0KHtcclxuICAgICAgICAgICAga2V5d29yZDogcXVlcnlbMF0sXHJcbiAgICAgICAgICAgIHBhZ2VJbmRleDogcXVlcnlbM10sXHJcbiAgICAgICAgICAgIHBhZ2VTaXplOiBxdWVyeVsyXVxyXG4gICAgICAgICAgfSk7XHJcbiAgICAgICAgfVxyXG4gICAgICApO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgcHVibGljIGhhbmRsZURyYWdBbmREcm9wKCk6IHZvaWQge1xyXG4gICAgLy8gdGhpcy5kcmFnQ2hhbmdlLnBpcGUoZGVib3VuY2VUaW1lKDEwMCkpLnN1YnNjcmliZSgocG9pbnRlclg6IG51bWJlcikgPT4ge1xyXG4gICAgLy8gICBjb25zdCBjdXJyZW50TGlzdE9mZnNldExlZnQgPSB0aGlzLmVsZW1lbnRSZWYubmF0aXZlRWxlbWVudC5vZmZzZXRMZWZ0O1xyXG4gICAgLy8gICBjb25zdCBjdXJyZW50TGlzdE9mZnNldFJpZ2h0ID0gdGhpcy5lbGVtZW50UmVmLm5hdGl2ZUVsZW1lbnQub2Zmc2V0V2lkdGggKyBjdXJyZW50TGlzdE9mZnNldExlZnQ7XHJcbiAgICAvLyAgIGlmIChwb2ludGVyWCAtIGN1cnJlbnRMaXN0T2Zmc2V0TGVmdCA8PSAxMDApIHtcclxuICAgIC8vICAgICB0aGlzLnNob3dEcmFnUmlnaHQubmV4dChmYWxzZSk7XHJcbiAgICAvLyAgICAgdGhpcy5zaG93RHJhZ0xlZnQubmV4dCh0cnVlKTtcclxuICAgIC8vICAgfSBlbHNlIGlmIChjdXJyZW50TGlzdE9mZnNldFJpZ2h0IC0gcG9pbnRlclggPD0gMTAwKSB7XHJcbiAgICAvLyAgICAgdGhpcy5zaG93RHJhZ1JpZ2h0Lm5leHQodHJ1ZSk7XHJcbiAgICAvLyAgICAgdGhpcy5zaG93RHJhZ0xlZnQubmV4dChmYWxzZSk7XHJcbiAgICAvLyAgIH0gZWxzZSB7XHJcbiAgICAvLyAgICAgdGhpcy5zaG93RHJhZ1JpZ2h0Lm5leHQoZmFsc2UpO1xyXG4gICAgLy8gICAgIHRoaXMuc2hvd0RyYWdMZWZ0Lm5leHQoZmFsc2UpO1xyXG4gICAgLy8gICB9XHJcbiAgICAvLyB9KTtcclxuICAgIC8vIHRoaXMuc2hvd0RyYWdMZWZ0LnBpcGUoZGVib3VuY2VUaW1lKDUwMCkpLnN1YnNjcmliZSh2ID0+IHtcclxuICAgIC8vICAgaWYgKCF2KSByZXR1cm47XHJcbiAgICAvLyAgIHRoaXMuc2hvd1N1YlRhYmxlID0gdHJ1ZTtcclxuICAgIC8vIH0pO1xyXG4gICAgLy8gdGhpcy5zaG93RHJhZ1JpZ2h0LnBpcGUoZGVib3VuY2VUaW1lKDUwMCkpLnN1YnNjcmliZSh2ID0+IHtcclxuICAgIC8vICAgaWYgKCF2KSByZXR1cm47XHJcbiAgICAvLyAgIHRoaXMuc2hvd1N1YlRhYmxlID0gdHJ1ZTtcclxuICAgIC8vIH0pO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGlzSXRlbVNlbGVjdGVkKGl0ZW06IFQpOiBib29sZWFuIHtcclxuICAgIHJldHVybiAhIXRoaXMuY3VycmVudFNlbGVjdGVkSXRlbS5maW5kKFxyXG4gICAgICAodjogVCkgPT4gSlNPTi5zdHJpbmdpZnkodikgPT09IEpTT04uc3RyaW5naWZ5KGl0ZW0pXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIG5nQWZ0ZXJDb250ZW50SW5pdCgpOiB2b2lkIHtcclxuICAgIGNvbnN0IGNvbHVtbnM6IFRhYmxlQ29sdW1uRGlyZWN0aXZlW10gPSB0aGlzLmNvbHVtbnMudG9BcnJheSgpO1xyXG4gICAgdGhpcy5jb2x1bW5EaWMgPSB0aGlzLl9idWlsZENvbHVtbkRpY3Rpb25hcnkoY29sdW1ucyk7XHJcbiAgICB0aGlzLnRhYmxlSW5pdGlhbENvbHVtbnMgPSBjb2x1bW5zLm1hcCgoYzogVGFibGVDb2x1bW5EaXJlY3RpdmUpID0+IGMua2V5KTtcclxuICAgIHRoaXMudGFibGVDb2x1bW5zID0gY29sdW1ucy5tYXAoKGM6IFRhYmxlQ29sdW1uRGlyZWN0aXZlKSA9PiBjLmtleSk7XHJcbiAgICB0aGlzLnN1YlRhYmxlQ29sdW1ucyA9IGNvbHVtbnNcclxuICAgICAgLmZpbHRlcihjID0+IGMuY29sdW1uVHlwZSAhPT0gJ0NIRUNLQk9YJyAmJiBjLnNob3dJblN1YlRhYmxlID09PSB0cnVlKVxyXG4gICAgICAubWFwKChjOiBUYWJsZUNvbHVtbkRpcmVjdGl2ZSkgPT4gYy5rZXkpO1xyXG4gICAgdGhpcy50YWJsZVNvcnRGaWVsZHMgPSBjb2x1bW5zLm1hcCgoYzogVGFibGVDb2x1bW5EaXJlY3RpdmUpID0+IGMuaGVhZGVyKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBvbkVuYWJsZWQoZW5hYmxlZDogcmVhZG9ubHkgc3RyaW5nW10pIHtcclxuICAgIHRoaXMudGFibGVDb2x1bW5zID0gdGhpcy50YWJsZVNvcnRGaWVsZHNcclxuICAgICAgLmZpbHRlcigoa2V5OiBzdHJpbmcpID0+IGVuYWJsZWQuaW5jbHVkZXMoa2V5KSlcclxuICAgICAgLm1hcCgodjogc3RyaW5nKSA9PiB0aGlzLmNvbHVtbkRpY1t2XSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgb25QYWdlQ2hhbmdlKHBhZ2U6IG51bWJlcik6IHZvaWQge1xyXG4gICAgdGhpcy5wYWdlQ2hhbmdlLmVtaXQocGFnZSk7XHJcbiAgICB0aGlzLnBhZ2UkLm5leHQocGFnZSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgb25TaXplQ2hhbmdlKHNpemU6IG51bWJlcik6IHZvaWQge1xyXG4gICAgdGhpcy5wYWdlU2l6ZUNoYW5nZS5lbWl0KHNpemUpO1xyXG4gICAgdGhpcy5zaXplJC5uZXh0KHNpemUpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIG9uSXRlbVNlbGVjdGVkKHZhbHVlOiBib29sZWFuLCBpdGVtOiBUKTogdm9pZCB7XHJcbiAgICBpZiAodmFsdWUpIHtcclxuICAgICAgdGhpcy5jdXJyZW50U2VsZWN0ZWRJdGVtLnB1c2goaXRlbSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICB0aGlzLmN1cnJlbnRTZWxlY3RlZEl0ZW0gPSB0aGlzLmN1cnJlbnRTZWxlY3RlZEl0ZW0uZmlsdGVyKFxyXG4gICAgICAgICh2OiBUKSA9PiBKU09OLnN0cmluZ2lmeShpdGVtKSAhPT0gSlNPTi5zdHJpbmdpZnkodilcclxuICAgICAgKTtcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmlzQWxsU2VsZWN0ZWQgPVxyXG4gICAgICB0aGlzLmN1cnJlbnRTZWxlY3RlZEl0ZW0ubGVuZ3RoID09PSB0aGlzLmNhY2hlSXRlbXMubGVuZ3RoO1xyXG4gICAgdGhpcy5pdGVtc1NlbGVjdGVkQ2hhbmdlLmVtaXQodGhpcy5jdXJyZW50U2VsZWN0ZWRJdGVtKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBtYXJrQWxsQ2hlY2tlZCh2YWx1ZTogYm9vbGVhbik6IHZvaWQge1xyXG4gICAgaWYgKHZhbHVlKSB7XHJcbiAgICAgIHRoaXMuY3VycmVudFNlbGVjdGVkSXRlbSA9IHRoaXMuY2FjaGVJdGVtcztcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIHRoaXMuY3VycmVudFNlbGVjdGVkSXRlbSA9IFtdO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuaXNBbGxTZWxlY3RlZCA9IHZhbHVlO1xyXG4gICAgdGhpcy5pdGVtc1NlbGVjdGVkQ2hhbmdlLmVtaXQodGhpcy5jdXJyZW50U2VsZWN0ZWRJdGVtKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBnZXROYXZpZ2F0aW9uTGVuZ3RoKHRvdGFsQ291bnQ6IG51bWJlciwgcGFnZVNpemU6IG51bWJlcik6IG51bWJlciB7XHJcbiAgICByZXR1cm4gTWF0aC5mbG9vcih0b3RhbENvdW50IC8gcGFnZVNpemUpIHx8IDE7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgcmVsb2FkRGF0YSgpOiB2b2lkIHtcclxuICAgIHRoaXMucmVsb2FkJC5uZXh0KCF0aGlzLnJlbG9hZCQuZ2V0VmFsdWUoKSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgb25EcmFnSXRlbShldmVudDogQ2RrRHJhZ01vdmUpOiB2b2lkIHtcclxuICAgIHRoaXMuZHJhZ0NoYW5nZS5uZXh0KGV2ZW50LnBvaW50ZXJQb3NpdGlvbi54KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBvblJlbGVhc2VEcmFnSXRlbSgpOiB2b2lkIHtcclxuICAgIC8vIHRoaXMuc2hvd0RyYWdMZWZ0Lm5leHQoZmFsc2UpO1xyXG4gICAgLy8gdGhpcy5zaG93RHJhZ1JpZ2h0Lm5leHQoZmFsc2UpO1xyXG4gICAgLy8gdGhpcy5zaG93U3ViVGFibGUgPSBmYWxzZTtcclxuICB9XHJcblxyXG4gIHByaXZhdGUgX3Byb2Nlc3NTZWxmRmV0Y2hEYXRhKFxyXG4gICAgc2VhcmNoVGV4dDogc3RyaW5nLFxyXG4gICAgcGFnZTogbnVtYmVyLFxyXG4gICAgcGFnZVNpemU6IG51bWJlclxyXG4gICk6IE9ic2VydmFibGU8UGFnaW5hdGlvblJlc3VsdE1vZGVsPFQ+PiB7XHJcbiAgICByZXR1cm4gdGhpcy5mZXRjaERhdGFGbih7XHJcbiAgICAgIGtleXdvcmQ6IHNlYXJjaFRleHQsXHJcbiAgICAgIHBhZ2VJbmRleDogcGFnZSxcclxuICAgICAgcGFnZVNpemU6IHBhZ2VTaXplLFxyXG4gICAgICBzZWFyY2hDb2x1bW5zOiB0aGlzLnNlYXJjaENvbHVtbnMsXHJcbiAgICAgIHNlYXJjaE1ldGhvZDogJ0xpa2UnXHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHByaXZhdGUgX3Nob3dMb2FkaW5nKCk6IHZvaWQge1xyXG4gICAgdGhpcy5sb2FkaW5nJC5uZXh0KHRydWUpO1xyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBfaGlkZUxvYWRpbmcoKTogdm9pZCB7XHJcbiAgICB0aGlzLmxvYWRpbmckLm5leHQoZmFsc2UpO1xyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBfYnVpbGRDb2x1bW5EaWN0aW9uYXJ5KFxyXG4gICAgY29sdW1uczogVGFibGVDb2x1bW5EaXJlY3RpdmVbXVxyXG4gICk6IFJlY29yZDxzdHJpbmcsIHN0cmluZz4ge1xyXG4gICAgcmV0dXJuIGNvbHVtbnMucmVkdWNlKFxyXG4gICAgICAoYWNjOiBvYmplY3QsIGl0ZW06IFRhYmxlQ29sdW1uRGlyZWN0aXZlKSA9PiAoe1xyXG4gICAgICAgIC4uLmFjYyxcclxuICAgICAgICBbaXRlbS5oZWFkZXJdOiBpdGVtLmtleVxyXG4gICAgICB9KSxcclxuICAgICAge31cclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBwcml2YXRlIF9zZXR1cFNlbGZDb250cm9sKCk6IHZvaWQge1xyXG4gICAgdGhpcy5kYXRhJCA9IHRoaXMucmVxdWVzdCQucGlwZShcclxuICAgICAgdGFwKCgpID0+IHRoaXMuX3Nob3dMb2FkaW5nKCkpLFxyXG4gICAgICBzd2l0Y2hNYXAoKHF1ZXJ5OiBbc3RyaW5nLCAxIHwgLTEsIG51bWJlciwgbnVtYmVyLCBib29sZWFuXSkgPT5cclxuICAgICAgICB0aGlzLl9wcm9jZXNzU2VsZkZldGNoRGF0YShxdWVyeVswXSwgcXVlcnlbMl0sIHF1ZXJ5WzNdKVxyXG4gICAgICApLFxyXG4gICAgICB0YXAoKHJlczogUGFnaW5hdGlvblJlc3VsdE1vZGVsPFQ+KSA9PiB7XHJcbiAgICAgICAgdGhpcy5faGlkZUxvYWRpbmcoKTtcclxuICAgICAgICB0aGlzLmNhY2hlSXRlbXMgPSByZXMuaXRlbXM7XHJcbiAgICAgICAgdGhpcy5jdXJyZW50UGFnZSA9IHJlcy5wYWdpbmdEYXRhLnBhZ2VJbmRleDtcclxuICAgICAgfSksXHJcbiAgICAgIHN0YXJ0V2l0aCh0aGlzLmVtcHR5RGF0YSksXHJcbiAgICAgIGNhdGNoRXJyb3IoKCkgPT4ge1xyXG4gICAgICAgIHRoaXMuX2hpZGVMb2FkaW5nKCk7XHJcbiAgICAgICAgcmV0dXJuIG9mKHRoaXMuZW1wdHlEYXRhKTtcclxuICAgICAgfSlcclxuICAgICk7XHJcbiAgfVxyXG59XHJcbiIsIjxkaXYgY2xhc3M9XCJtaXgtZGF0YS10YWJsZVwiPlxyXG4gIDxwICpuZ0lmPVwicmVPcmRlcmFibGUgfHwgc2VhcmNoYWJsZVwiXHJcbiAgICAgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fZmlsdGVyc1wiXHJcbiAgICAgdHVpVGV4dGZpZWxkU2l6ZT1cIm1cIj5cclxuICAgIDx0dWktaW5wdXQgKm5nSWY9XCJzZWFyY2hhYmxlXCJcclxuICAgICAgICAgICAgICAgY2xhc3M9XCJpbnB1dFwiXHJcbiAgICAgICAgICAgICAgIFtuZ01vZGVsXT1cInNlYXJjaFwiXHJcbiAgICAgICAgICAgICAgIFt0dWlUZXh0ZmllbGRDbGVhbmVyXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAobmdNb2RlbENoYW5nZSk9XCJzZWFyY2hUZXh0JC5uZXh0KCRldmVudClcIlxyXG4gICAgICAgICAgICAgICBpY29uPVwidHVpSWNvblNlYXJjaExhcmdlXCI+XHJcbiAgICAgIHt7IHNlYXJjaFBsYWNlaG9sZGVyIH19XHJcbiAgICA8L3R1aS1pbnB1dD5cclxuXHJcbiAgICA8dHVpLWhvc3RlZC1kcm9wZG93biAqbmdJZj1cInJlT3JkZXJhYmxlXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgIFtjb250ZW50XT1cImRyb3Bkb3duXCI+XHJcbiAgICAgIDxidXR0b24gW2ljb25SaWdodF09XCJhcnJvd1wiXHJcbiAgICAgICAgICAgICAgdHVpQnV0dG9uXHJcbiAgICAgICAgICAgICAgc2l6ZT1cIm1cIj5Db2x1bW5zPC9idXR0b24+XHJcbiAgICAgIDxuZy10ZW1wbGF0ZSAjZHJvcGRvd24+XHJcbiAgICAgICAgPHR1aS1yZW9yZGVyIGNsYXNzPVwiY29sdW1uc1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIFsoaXRlbXMpXT1cInRhYmxlU29ydEZpZWxkc1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIFtlbmFibGVkXT1cInRhYmxlU29ydEZpZWxkc1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIChlbmFibGVkQ2hhbmdlKT1cIm9uRW5hYmxlZCgkZXZlbnQpXCI+PC90dWktcmVvcmRlcj5cclxuICAgICAgPC9uZy10ZW1wbGF0ZT5cclxuICAgIDwvdHVpLWhvc3RlZC1kcm9wZG93bj5cclxuICA8L3A+XHJcblxyXG4gIDwhLS0gVE9ETzogQ29tbWVudCB1bnRpbCBmZWF0dXJlIHN0YWJsZSAtLT5cclxuICA8IS0tIDxkaXYgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fYWRkaXRpb25hbC10b29sYmFyXCI+XHJcbiAgICA8YnV0dG9uICpuZ0lmPVwic2hvd1N1YlRhYmxlXCJcclxuICAgICAgICAgICAgW2FwcGVhcmFuY2VdPVwiJ2FjY2VudCdcIlxyXG4gICAgICAgICAgICBbc2l6ZV09XCIneHMnXCJcclxuICAgICAgICAgICAgKGNsaWNrKT1cInNob3dTdWJUYWJsZSA9IGZhbHNlXCJcclxuICAgICAgICAgICAgdHVpQnV0dG9uPkV4aXQgRHJhZyAmIERyb3AgTW9kZTwvYnV0dG9uPlxyXG5cclxuICAgIDxidXR0b24gW3NpemVdPVwiJ3hzJ1wiXHJcbiAgICAgICAgICAgIChjbGljayk9XCJzaG93U3ViVGFibGUgPSB0cnVlXCJcclxuICAgICAgICAgICAgdHVpQnV0dG9uPlNob3cgTmV4dCBQYWdlPC9idXR0b24+XHJcbiAgPC9kaXY+IC0tPlxyXG5cclxuICA8ZGl2IGNsYXNzPVwibWl4LWRhdGEtdGFibGVfX21haW4tY29udGFpbmVyXCJcclxuICAgICAgIGNka0Ryb3BMaXN0R3JvdXA+XHJcbiAgICA8ZGl2IGNsYXNzPVwibWl4LWRhdGEtdGFibGVfX21haW4tdGFibGVcIlxyXG4gICAgICAgICBbbmdDbGFzc109XCJ7Jy0tc2hvdyc6IHNob3dTdWJUYWJsZX1cIj5cclxuICAgICAgPGRpdiAqbmdJZj1cInNob3dTdWJUYWJsZVwiXHJcbiAgICAgICAgICAgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fdGFibGUtaGVhZGVyXCI+XHJcbiAgICAgICAgQ3VycmVudCBQYWdlXHJcbiAgICAgIDwvZGl2PlxyXG4gICAgICA8dHVpLWxvYWRlciBbb3ZlcmxheV09XCJ0cnVlXCJcclxuICAgICAgICAgICAgICAgICAgW3Nob3dMb2FkZXJdPVwiISEobG9hZGluZyQgfCBhc3luYylcIj5cclxuICAgICAgICA8dGFibGUgKnR1aUxldD1cImRhdGEkIHwgYXN5bmMgYXMgZGF0YVwiXHJcbiAgICAgICAgICAgICAgIFtjb2x1bW5zXT1cInRhYmxlQ29sdW1uc1wiXHJcbiAgICAgICAgICAgICAgIHR1aVRhYmxlPlxyXG4gICAgICAgICAgPHRoZWFkPlxyXG4gICAgICAgICAgICA8dHIgdHVpVGhHcm91cD5cclxuICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0Zvcj1cImxldCBjb2wgb2YgY29sdW1uc1wiPlxyXG4gICAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cImNvbC5zb3J0YWJsZSA9PT0gZmFsc2UgJiYgY29sLmNvbHVtblR5cGUgIT09ICdDSEVDS0JPWCdcIj5cclxuICAgICAgICAgICAgICAgICAgPHRoICp0dWlIZWFkPVwiY29sLmtleVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICBbcmVzaXphYmxlXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgW3NvcnRlcl09XCJudWxsXCJcclxuICAgICAgICAgICAgICAgICAgICAgIHR1aVRoPlxyXG4gICAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCJjb2wudHBsSGVhZGVyXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ1RlbXBsYXRlT3V0bGV0PVwiY29sLnRwbEhlYWRlci50ZW1wbGF0ZVwiPjwvbmctY29udGFpbmVyPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvbmctY29udGFpbmVyPlxyXG5cclxuICAgICAgICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0lmPVwiIWNvbC50cGxIZWFkZXIgJiYgY29sLnNob3dIZWFkZXJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgIHt7IGNvbC5oZWFkZXIgfX1cclxuICAgICAgICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgICAgICAgPC90aD5cclxuICAgICAgICAgICAgICAgIDwvbmctY29udGFpbmVyPlxyXG5cclxuICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCJjb2wuc29ydGFibGUgPT09IHRydWUgJiYgY29sLmNvbHVtblR5cGUgIT09ICdDSEVDS0JPWCdcIj5cclxuICAgICAgICAgICAgICAgICAgPHRoICp0dWlIZWFkPVwiY29sLmtleVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICBbcmVzaXphYmxlXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgdHVpVGg+XHJcbiAgICAgICAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cImNvbC50cGxIZWFkZXJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nVGVtcGxhdGVPdXRsZXQ9XCJjb2wudHBsSGVhZGVyLnRlbXBsYXRlXCI+PC9uZy1jb250YWluZXI+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9uZy1jb250YWluZXI+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCIhY29sLnRwbEhlYWRlciAmJiBjb2wuc2hvd0hlYWRlclwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAge3sgY29sLmhlYWRlciB9fVxyXG4gICAgICAgICAgICAgICAgICAgIDwvbmctY29udGFpbmVyPlxyXG4gICAgICAgICAgICAgICAgICA8L3RoPlxyXG4gICAgICAgICAgICAgICAgPC9uZy1jb250YWluZXI+XHJcblxyXG4gICAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cImNvbC5jb2x1bW5UeXBlID09PSAnQ0hFQ0tCT1gnXCI+XHJcbiAgICAgICAgICAgICAgICAgIDx0aCAqdHVpSGVhZD1cImNvbC5rZXlcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fY2hlY2tib3hcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgW3NvcnRlcl09XCJudWxsXCJcclxuICAgICAgICAgICAgICAgICAgICAgIHR1aVRoPlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJ3cmFwcGVyXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICA8dHVpLWNoZWNrYm94IFtuZ01vZGVsXT1cImlzQWxsU2VsZWN0ZWRcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAobmdNb2RlbENoYW5nZSk9XCJtYXJrQWxsQ2hlY2tlZCgkZXZlbnQpXCI+PC90dWktY2hlY2tib3g+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgIDwvdGg+XHJcbiAgICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgPC90cj5cclxuICAgICAgICAgIDwvdGhlYWQ+XHJcblxyXG4gICAgICAgICAgPHRib2R5ICNkcm9wTGlzdFxyXG4gICAgICAgICAgICAgICAgICpuZ0lmPVwiZGF0YVwiXHJcbiAgICAgICAgICAgICAgICAgW2RhdGFdPVwiZGF0YS5pdGVtc1wiXHJcbiAgICAgICAgICAgICAgICAgY2RrRHJvcExpc3RcclxuICAgICAgICAgICAgICAgICB0dWlUYm9keT5cclxuICAgICAgICAgICAgPHRyICp0dWlSb3c9XCJsZXQgaXRlbSBvZiBkYXRhLml0ZW1zXCJcclxuICAgICAgICAgICAgICAgIFtjZGtEcmFnRGF0YV09XCJpdGVtXCJcclxuICAgICAgICAgICAgICAgIChjZGtEcmFnTW92ZWQpPVwib25EcmFnSXRlbSgkZXZlbnQpXCJcclxuICAgICAgICAgICAgICAgIChjZGtEcmFnUmVsZWFzZWQpPVwib25SZWxlYXNlRHJhZ0l0ZW0oKVwiXHJcbiAgICAgICAgICAgICAgICBjZGtEcmFnXHJcbiAgICAgICAgICAgICAgICB0dWlUcj5cclxuICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0Zvcj1cImxldCBjb2wgb2YgY29sdW1uc1wiPlxyXG4gICAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cImNvbC5jb2x1bW5UeXBlICE9PSAnQ0hFQ0tCT1gnXCI+XHJcbiAgICAgICAgICAgICAgICAgIDx0ZCAqdHVpQ2VsbD1cImNvbC5rZXlcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fdGRcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgW25nQ2xhc3NdPVwieyctLWFjdGlvbic6IGNvbC5jb2x1bW5UeXBlID09PSAnQUNUSU9OJyxcclxuICAgICAgICAgICAgICAgICAgICAgICctLWRhdGUnOiBjb2wuY29sdW1uVHlwZSA9PT0gJ0RBVEUnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgJy0taGlkZS1zdWItdGFibGUnOiBjb2wuc2hvd0luU3ViVGFibGUgPT09IGZhbHNlfVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICBbbmdTdHlsZV09XCJ7J3dpZHRoJzogY29sLndpZHRofVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICB0dWlUZD5cclxuICAgICAgICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0lmPVwiY29sLnRwbENlbGxcIj5cclxuICAgICAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nVGVtcGxhdGVPdXRsZXQ9XCJjb2wudHBsQ2VsbC50ZW1wbGF0ZTsgY29udGV4dDogeyAkaW1wbGljaXQ6IGl0ZW0gfVwiPjwvbmctY29udGFpbmVyPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvbmctY29udGFpbmVyPlxyXG4gICAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCIhY29sLnRwbENlbGwgJiYgY29sLmNvbHVtblR5cGUgPT09ICdEQVRFJ1wiPlxyXG4gICAgICAgICAgICAgICAgICAgICAge3sgJGFueShpdGVtKVtjb2wua2V5XSA/PyAnTi9BJyB8IGRhdGU6J3Nob3J0JyB8IHJlbGF0aXZlVGltZVNwYW4gfX1cclxuICAgICAgICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cIiFjb2wudHBsQ2VsbCAmJiBjb2wuY29sdW1uVHlwZSAhPT0gJ0RBVEUnXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICB7eyAkYW55KGl0ZW0pW2NvbC5rZXldID8/ICdOL0EnIH19XHJcbiAgICAgICAgICAgICAgICAgICAgPC9uZy1jb250YWluZXI+XHJcbiAgICAgICAgICAgICAgICAgIDwvdGQ+XHJcbiAgICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuXHJcbiAgICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0lmPVwiY29sLmNvbHVtblR5cGUgPT09ICdDSEVDS0JPWCdcIj5cclxuICAgICAgICAgICAgICAgICAgPHRkICp0dWlDZWxsPVwiY29sLmtleVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICBjbGFzcz1cIm1peC1kYXRhLXRhYmxlX190ZCBtaXgtZGF0YS10YWJsZV9fY2hlY2tib3hcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgdHVpVGQ+XHJcbiAgICAgICAgICAgICAgICAgICAgPHR1aS1jaGVja2JveCBbbmdNb2RlbF09XCJpc0l0ZW1TZWxlY3RlZChpdGVtKVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAobmdNb2RlbENoYW5nZSk9XCJvbkl0ZW1TZWxlY3RlZCgkZXZlbnQsIGl0ZW0pXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgPC90dWktY2hlY2tib3g+XHJcbiAgICAgICAgICAgICAgICAgIDwvdGQ+XHJcbiAgICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgPC90cj5cclxuICAgICAgICAgIDwvdGJvZHk+XHJcblxyXG4gICAgICAgICAgPHRmb290ICpuZ0lmPVwiZGF0YSAmJiAhc2hvd1N1YlRhYmxlXCI+XHJcbiAgICAgICAgICAgIDx0ciBjbGFzcz1cIm1peC1kYXRhLXRhYmxlX19wYWdpbmdcIj5cclxuICAgICAgICAgICAgICA8dGQgW2NvbFNwYW5dPVwiY29sdW1ucy5sZW5ndGhcIj5cclxuICAgICAgICAgICAgICAgIDx0dWktcGFnaW5hdGlvbiBjbGFzcz1cInR1aS1zcGFjZV90b3AtMlwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgW2xlbmd0aF09XCJkYXRhLnBhZ2luZ0RhdGEudG90YWxQYWdlIHx8IDBcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIChpbmRleENoYW5nZSk9XCJvblBhZ2VDaGFuZ2UoJGV2ZW50KVwiPlxyXG4gICAgICAgICAgICAgICAgPC90dWktcGFnaW5hdGlvbj5cclxuICAgICAgICAgICAgICA8L3RkPlxyXG4gICAgICAgICAgICA8L3RyPlxyXG4gICAgICAgICAgPC90Zm9vdD5cclxuICAgICAgICA8L3RhYmxlPlxyXG4gICAgICA8L3R1aS1sb2FkZXI+XHJcbiAgICA8L2Rpdj5cclxuXHJcbiAgICA8ZGl2ICpuZ0lmPVwic2hvd1N1YlRhYmxlXCJcclxuICAgICAgICAgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fdHJhbnNmZXItaWNvblwiPlxyXG4gICAgICA8aS10YWJsZXIgbmFtZT1cInNxdWFyZS10b2dnbGVcIj48L2ktdGFibGVyPlxyXG4gICAgPC9kaXY+XHJcblxyXG4gICAgPGRpdiBjbGFzcz1cIm1peC1kYXRhLXRhYmxlX19zdWItdGFibGVcIlxyXG4gICAgICAgICBbbmdDbGFzc109XCJ7Jy0tc2hvdyc6IHNob3dTdWJUYWJsZX1cIj5cclxuICAgICAgPGRpdiAqbmdJZj1cInNob3dTdWJUYWJsZVwiXHJcbiAgICAgICAgICAgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fdGFibGUtaGVhZGVyXCI+XHJcbiAgICAgICAgTmV4dCBQYWdlXHJcbiAgICAgIDwvZGl2PlxyXG4gICAgICA8dGFibGUgKnR1aUxldD1cImRhdGEkIHwgYXN5bmMgYXMgZGF0YVwiXHJcbiAgICAgICAgICAgICBbY29sdW1uc109XCJzdWJUYWJsZUNvbHVtbnNcIlxyXG4gICAgICAgICAgICAgdHVpVGFibGU+XHJcbiAgICAgICAgPHRoZWFkPlxyXG4gICAgICAgICAgPHRyIHR1aVRoR3JvdXA+XHJcbiAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nRm9yPVwibGV0IGNvbCBvZiBjb2x1bW5zXCI+XHJcbiAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cImNvbC5jb2x1bW5UeXBlICE9PSAnQ0hFQ0tCT1gnXCI+XHJcbiAgICAgICAgICAgICAgICA8dGggKnR1aUhlYWQ9XCJjb2wua2V5XCJcclxuICAgICAgICAgICAgICAgICAgICBbcmVzaXphYmxlXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgIFtzb3J0ZXJdPVwibnVsbFwiXHJcbiAgICAgICAgICAgICAgICAgICAgdHVpVGg+XHJcbiAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCJjb2wudHBsSGVhZGVyXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdUZW1wbGF0ZU91dGxldD1cImNvbC50cGxIZWFkZXIudGVtcGxhdGVcIj48L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgICAgICAgPC9uZy1jb250YWluZXI+XHJcblxyXG4gICAgICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0lmPVwiIWNvbC50cGxIZWFkZXIgJiYgY29sLnNob3dIZWFkZXJcIj5cclxuICAgICAgICAgICAgICAgICAgICB7eyBjb2wuaGVhZGVyIH19XHJcbiAgICAgICAgICAgICAgICAgIDwvbmctY29udGFpbmVyPlxyXG4gICAgICAgICAgICAgICAgPC90aD5cclxuICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgPC9uZy1jb250YWluZXI+XHJcbiAgICAgICAgICA8L3RyPlxyXG4gICAgICAgIDwvdGhlYWQ+XHJcblxyXG4gICAgICAgIDx0Ym9keSAjc3ViRHJvcExpc3RcclxuICAgICAgICAgICAgICAgKm5nSWY9XCJkYXRhXCJcclxuICAgICAgICAgICAgICAgW2RhdGFdPVwiZGF0YS5pdGVtc1wiXHJcbiAgICAgICAgICAgICAgIGNka0Ryb3BMaXN0XHJcbiAgICAgICAgICAgICAgIHR1aVRib2R5PlxyXG4gICAgICAgICAgPHRyICp0dWlSb3c9XCJsZXQgaXRlbSBvZiBkYXRhLml0ZW1zXCJcclxuICAgICAgICAgICAgICBbY2RrRHJhZ0RhdGFdPVwiaXRlbVwiXHJcbiAgICAgICAgICAgICAgKGNka0RyYWdNb3ZlZCk9XCJvbkRyYWdJdGVtKCRldmVudClcIlxyXG4gICAgICAgICAgICAgIChjZGtEcmFnUmVsZWFzZWQpPVwib25SZWxlYXNlRHJhZ0l0ZW0oKVwiXHJcbiAgICAgICAgICAgICAgY2RrRHJhZ1xyXG4gICAgICAgICAgICAgIHR1aVRyPlxyXG4gICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0Zvcj1cImxldCBjb2wgb2YgY29sdW1uc1wiPlxyXG4gICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCJjb2wuY29sdW1uVHlwZSAhPT0gJ0NIRUNLQk9YJ1wiPlxyXG4gICAgICAgICAgICAgICAgPHRkICp0dWlDZWxsPVwiY29sLmtleVwiXHJcbiAgICAgICAgICAgICAgICAgICAgY2xhc3M9XCJtaXgtZGF0YS10YWJsZV9fdGRcIlxyXG4gICAgICAgICAgICAgICAgICAgIFtuZ0NsYXNzXT1cInsnLS1hY3Rpb24nOiBjb2wuY29sdW1uVHlwZSA9PT0gJ0FDVElPTicsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgJy0tZGF0ZSc6IGNvbC5jb2x1bW5UeXBlID09PSAnREFURSd9XCJcclxuICAgICAgICAgICAgICAgICAgICBbbmdTdHlsZV09XCJ7J3dpZHRoJzogY29sLndpZHRofVwiXHJcbiAgICAgICAgICAgICAgICAgICAgdHVpVGQ+XHJcbiAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCJjb2wudHBsQ2VsbFwiPlxyXG4gICAgICAgICAgICAgICAgICAgIDxuZy1jb250YWluZXIgKm5nVGVtcGxhdGVPdXRsZXQ9XCJjb2wudHBsQ2VsbC50ZW1wbGF0ZTsgY29udGV4dDogeyAkaW1wbGljaXQ6IGl0ZW0gfVwiPjwvbmctY29udGFpbmVyPlxyXG4gICAgICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cIiFjb2wudHBsQ2VsbCAmJiBjb2wuY29sdW1uVHlwZSA9PT0gJ0RBVEUnXCI+XHJcbiAgICAgICAgICAgICAgICAgICAge3sgJGFueShpdGVtKVtjb2wua2V5XSA/PyAnTi9BJyB8IGRhdGU6J3Nob3J0JyB8IHJlbGF0aXZlVGltZVNwYW4gfX1cclxuICAgICAgICAgICAgICAgICAgPC9uZy1jb250YWluZXI+XHJcblxyXG4gICAgICAgICAgICAgICAgICA8bmctY29udGFpbmVyICpuZ0lmPVwiIWNvbC50cGxDZWxsICYmIGNvbC5jb2x1bW5UeXBlICE9PSAnREFURSdcIj5cclxuICAgICAgICAgICAgICAgICAgICB7eyAkYW55KGl0ZW0pW2NvbC5rZXldID8/ICdOL0EnIH19XHJcbiAgICAgICAgICAgICAgICAgIDwvbmctY29udGFpbmVyPlxyXG4gICAgICAgICAgICAgICAgPC90ZD5cclxuICAgICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICAgICAgPC9uZy1jb250YWluZXI+XHJcbiAgICAgICAgICA8L3RyPlxyXG4gICAgICAgIDwvdGJvZHk+XHJcbiAgICAgIDwvdGFibGU+XHJcbiAgICA8L2Rpdj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiAjdGVtcG9yYXJ5RHJhZ1xyXG4gICAgICAgY2RrRHJvcExpc3Q+XHJcbiAgPC9kaXY+XHJcbjwvZGl2PlxyXG4iXX0=