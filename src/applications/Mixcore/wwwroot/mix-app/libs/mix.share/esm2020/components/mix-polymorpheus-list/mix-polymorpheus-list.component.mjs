import { ChangeDetectionStrategy, Component, Inject, Input, TemplateRef, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MixContentStatus, MixContentType } from '@mix-spa/mix.lib';
import { BehaviorSubject, combineLatest, filter, tap } from 'rxjs';
import { AppEvent, AppEventService, PortalSidebarControlService } from '../../services';
import { MixModuleApiService } from '../../services/api/mix-module-api.service';
import { MixPageApiService } from '../../services/api/mix-page-api.service';
import { MixPostApiService } from '../../services/api/mix-post-api.service';
import { ShareModule } from '../../share.module';
import { MixDataTableComponent } from '../data-table';
import { MixDataTableModule } from '../data-table/data-table.module';
import { MixChatBoxComponent } from '../mix-chat';
import { MixModuleDetailComponent } from '../mix-module-detail/mix-module-detail.component';
import { MixStatusIndicatorComponent } from '../mix-status-indicator';
import { MixToolbarComponent } from '../mix-toolbar/mix-toolbar.component';
import { ModalService } from '../modal/modal.service';
import { MixUserListHubComponent } from '../user-list-hub/user-list-hub.component';
import * as i0 from "@angular/core";
import * as i1 from "../../services/api/mix-page-api.service";
import * as i2 from "../../services/api/mix-post-api.service";
import * as i3 from "../../services/api/mix-module-api.service";
import * as i4 from "../../services";
import * as i5 from "@angular/common";
import * as i6 from "@angular/forms";
import * as i7 from "@taiga-ui/kit";
import * as i8 from "@taiga-ui/core";
import * as i9 from "angular-tabler-icons";
import * as i10 from "../data-table/data-table.component";
import * as i11 from "../data-table/directives/cell.directive";
import * as i12 from "../data-table/directives/column.directive";
import * as i13 from "../modal/modal.service";
const _c0 = ["moduleDetail"];
function MixPolymorphousListComponent_ng_template_15_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelement(0, "mix-status-indicator", 18);
} if (rf & 2) {
    const data_r7 = ctx.$implicit;
    i0.ɵɵproperty("status", data_r7.status);
} }
function MixPolymorphousListComponent_ng_template_17_Template(rf, ctx) { if (rf & 1) {
    const _r10 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tui-hosted-dropdown", 19)(1, "button", 20);
    i0.ɵɵlistener("click", function MixPolymorphousListComponent_ng_template_17_Template_button_click_1_listener() { const restoredCtx = i0.ɵɵrestoreView(_r10); const data_r8 = restoredCtx.$implicit; const ctx_r9 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r9.currentActionItem = data_r8); });
    i0.ɵɵelementEnd()();
} if (rf & 2) {
    i0.ɵɵnextContext();
    const _r3 = i0.ɵɵreference(20);
    i0.ɵɵproperty("content", _r3);
} }
function MixPolymorphousListComponent_div_18_tui_data_list_wrapper_9_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelement(0, "tui-data-list-wrapper", 30);
} if (rf & 2) {
    const ctx_r11 = i0.ɵɵnextContext(2);
    i0.ɵɵproperty("items", ctx_r11.statusOption);
} }
function MixPolymorphousListComponent_div_18_tui_data_list_wrapper_13_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelement(0, "tui-data-list-wrapper", 31);
} if (rf & 2) {
    const ctx_r12 = i0.ɵɵnextContext(2);
    i0.ɵɵproperty("items", ctx_r12.sortOption);
} }
function MixPolymorphousListComponent_div_18_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 21)(1, "div", 22)(2, "label", 23);
    i0.ɵɵtext(3, " From Date - To Date (Updated Date) ");
    i0.ɵɵelementStart(4, "tui-input-date-range", 24);
    i0.ɵɵtext(5, " Pick a date ");
    i0.ɵɵelementEnd()();
    i0.ɵɵelementStart(6, "label", 23);
    i0.ɵɵtext(7, " Status ");
    i0.ɵɵelementStart(8, "tui-multi-select", 25);
    i0.ɵɵtemplate(9, MixPolymorphousListComponent_div_18_tui_data_list_wrapper_9_Template, 1, 1, "tui-data-list-wrapper", 26);
    i0.ɵɵelementEnd()();
    i0.ɵɵelementStart(10, "label", 23);
    i0.ɵɵtext(11, " Sort by: ");
    i0.ɵɵelementStart(12, "tui-select", 27);
    i0.ɵɵtemplate(13, MixPolymorphousListComponent_div_18_tui_data_list_wrapper_13_Template, 1, 1, "tui-data-list-wrapper", 28);
    i0.ɵɵelementEnd()()();
    i0.ɵɵelementStart(14, "div", 29);
    i0.ɵɵelement(15, "mix-user-list-hub");
    i0.ɵɵelementEnd()();
} if (rf & 2) {
    const ctx_r2 = i0.ɵɵnextContext();
    i0.ɵɵadvance(4);
    i0.ɵɵproperty("tuiTextfieldSize", "m");
    i0.ɵɵadvance(4);
    i0.ɵɵproperty("editable", false)("expandable", false)("formControl", ctx_r2.statusControl)("tuiTextfieldLabelOutside", true)("tuiTextfieldSize", "m");
    i0.ɵɵadvance(4);
    i0.ɵɵproperty("formControl", ctx_r2.sortOptionControl)("tuiTextfieldLabelOutside", true)("tuiTextfieldSize", "m");
} }
function MixPolymorphousListComponent_ng_template_19_Template(rf, ctx) { if (rf & 1) {
    const _r14 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "tui-data-list")(1, "button", 32);
    i0.ɵɵlistener("click", function MixPolymorphousListComponent_ng_template_19_Template_button_click_1_listener() { i0.ɵɵrestoreView(_r14); const ctx_r13 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r13.editItem()); });
    i0.ɵɵtext(2, " Edit ");
    i0.ɵɵelementEnd()();
} if (rf & 2) {
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("size", "s");
} }
function MixPolymorphousListComponent_ng_template_21_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelement(0, "mix-module-detail", 33);
} if (rf & 2) {
    const ctx_r6 = i0.ɵɵnextContext();
    i0.ɵɵproperty("mode", "Quickly")("moduleId", ctx_r6.moduleId);
} }
export class MixPolymorphousListComponent {
    constructor(modalService, pageApi, postApi, moduleApi, appEvent, sidebarControl) {
        this.modalService = modalService;
        this.pageApi = pageApi;
        this.postApi = postApi;
        this.moduleApi = moduleApi;
        this.appEvent = appEvent;
        this.sidebarControl = sidebarControl;
        this.listType = MixContentType.Page;
        this.contentConfig = {
            Page: {
                header: 'Pages Available',
                searchPlaceholder: 'Type your Page name...'
            },
            Post: {
                header: 'Posts Available',
                searchPlaceholder: 'Type your Post name...'
            },
            Module: {
                header: 'Module sAvailable',
                searchPlaceholder: 'Type your Module name...'
            },
            MixDatabase: {
                header: 'MixDatabases Available',
                searchPlaceholder: 'Type your MixDatabase name...'
            },
            Scheduler: {
                header: 'Schedulers Available',
                searchPlaceholder: 'Type your Scheduler name...'
            },
            Tenant: {
                header: 'Tenants Available',
                searchPlaceholder: 'Type your Tenant name...'
            },
            Domain: {
                header: 'Domains Available',
                searchPlaceholder: 'Type your Domain name...'
            },
            Media: {
                header: 'Medias Available',
                searchPlaceholder: 'Type your Media name...'
            },
            Theme: {
                header: 'Themes Available',
                searchPlaceholder: 'Type your Theme name...'
            },
            Language: {
                header: 'Languages Available',
                searchPlaceholder: 'Type your Language name...'
            },
            Localization: {
                header: 'Localizations Available',
                searchPlaceholder: 'Type your Localization name...'
            }
        };
        // Filter By Status
        this.statusOption = [
            MixContentStatus.Draft,
            MixContentStatus.Published,
            MixContentStatus.Deleted,
            MixContentStatus.Preview
        ];
        this.statusControl = new FormControl(this.statusOption);
        // Sort Option
        this.sortOption = ['Last Updated', 'Priority'];
        this.sortOptionControl = new FormControl('Last Updated');
        this.showLeftSide = true;
        this.loading$ = new BehaviorSubject(true);
        this.itemCount = 0;
        this.currentSelectedItems = [];
        this.currentActionItem = undefined;
        this.moduleId = 0;
        this.fetchDataFn = (query) => {
            return this.request(query).pipe(tap(result => {
                this.itemCount = result.pagingData.total || 0;
            }));
        };
    }
    ngOnInit() {
        switch (this.listType) {
            case MixContentType.Page:
                this.request = (query) => this.pageApi.getPages(query);
                this.deleteRequest = (id) => this.pageApi.deletePages(id);
                break;
            case MixContentType.Post:
                this.request = (query) => this.postApi.getPosts(query);
                this.deleteRequest = (id) => this.postApi.deletePosts(id);
                break;
            default:
                this.request = (query) => this.moduleApi.getModules(query);
                this.deleteRequest = (id) => this.moduleApi.deleteModules(id);
                break;
        }
        this.appEvent.event$
            .pipe(filter(event => [
            AppEvent.NewModuleAdded,
            AppEvent.NewPageAdded,
            AppEvent.NewPostAdded
        ].includes(event)))
            .subscribe(() => {
            this.dataTable.reloadData();
        });
    }
    itemSelectedChange(items) {
        this.currentSelectedItems = items;
    }
    onDelete() {
        const message = 'Are you sure to delete this items ? Your data may not be revert';
        this.modalService.confirm(message).subscribe((ok) => {
            if (ok)
                this.deleteItem();
        });
    }
    deleteItem() {
        this.loading$.next(true);
        combineLatest(this.currentSelectedItems.map(v => this.deleteRequest(v.id))).subscribe(() => {
            this.modalService.success('Successfully delete data').subscribe();
            this.dataTable.reloadData();
            this.currentSelectedItems = [];
        });
    }
    toggleFilter() {
        this.showLeftSide = !this.showLeftSide;
    }
    editItem() {
        if (!this.currentActionItem)
            return;
        switch (this.listType) {
            case MixContentType.Module:
                this.moduleId = this.currentActionItem.id;
                this.sidebarControl.show(this.moduleDetail);
                break;
            default:
                break;
        }
    }
}
MixPolymorphousListComponent.ɵfac = function MixPolymorphousListComponent_Factory(t) { return new (t || MixPolymorphousListComponent)(i0.ɵɵdirectiveInject(ModalService), i0.ɵɵdirectiveInject(i1.MixPageApiService), i0.ɵɵdirectiveInject(i2.MixPostApiService), i0.ɵɵdirectiveInject(i3.MixModuleApiService), i0.ɵɵdirectiveInject(i4.AppEventService), i0.ɵɵdirectiveInject(i4.PortalSidebarControlService)); };
MixPolymorphousListComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixPolymorphousListComponent, selectors: [["mix-polymorphous-list"]], viewQuery: function MixPolymorphousListComponent_Query(rf, ctx) { if (rf & 1) {
        i0.ɵɵviewQuery(MixDataTableComponent, 5);
        i0.ɵɵviewQuery(_c0, 5);
    } if (rf & 2) {
        let _t;
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.dataTable = _t.first);
        i0.ɵɵqueryRefresh(_t = i0.ɵɵloadQuery()) && (ctx.moduleDetail = _t.first);
    } }, inputs: { listType: "listType", request: "request", deleteRequest: "deleteRequest" }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 23, vars: 20, consts: [["header", "", 1, "polymorphous-list__header"], [1, "separator"], [3, "selectedItem", "delete"], ["tuiIconButton", "", "size", "s", 1, "toggle-left-side", 3, "click"], ["name", "filter"], [1, "polymorphous-list__container"], [1, "polymorphous-list__main-workspace"], [3, "fetchDataFn", "searchPlaceholder", "loading$", "itemsSelectedChange"], ["mixTableColumn", "", 3, "columnType", "showInSubTable"], ["mixTableColumn", "", "key", "id", 3, "header", "width"], ["mixTableColumn", "", "key", "title", 3, "header"], ["key", "createdDateTime", "mixTableColumn", "", 3, "columnType", "header", "showInSubTable"], ["mixTableColumn", "", "key", "status", 3, "header", "showInSubTable"], ["mixColumnCell", ""], ["header", "Action", "mixTableColumn", "", "key", "Action", 3, "columnType", "showHeader", "sortable"], ["class", "polymorphous-list__left-side", 4, "ngIf"], ["action", ""], ["moduleDetail", ""], [3, "status"], ["tuiDropdownAlign", "left", 3, "content"], ["type", "button", "tuiIconButton", "", "size", "m", "appearance", "flat", "icon", "tuiIconMoreVer", 3, "click"], [1, "polymorphous-list__left-side"], [1, "filter"], ["tuiLabel", ""], [1, "b-form", 3, "tuiTextfieldSize"], ["tuiTextfieldExampleText", "Ignored text", 1, "b-form", 3, "editable", "expandable", "formControl", "tuiTextfieldLabelOutside", "tuiTextfieldSize"], ["tuiMultiSelectGroup", "", 3, "items", 4, "tuiDataList"], ["tuiTextfieldExampleText", "Ignored text", 1, "b-form", 3, "formControl", "tuiTextfieldLabelOutside", "tuiTextfieldSize"], [3, "items", 4, "tuiDataList"], [1, "chatbox"], ["tuiMultiSelectGroup", "", 3, "items"], [3, "items"], ["type", "button", "tuiOption", "", "tuiHintMode", "onDark", "tuiHintDirection", "right", 1, "tui-space_right-3", 3, "size", "click"], [3, "mode", "moduleId"]], template: function MixPolymorphousListComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div");
        i0.ɵɵtext(2);
        i0.ɵɵelementEnd();
        i0.ɵɵelement(3, "div", 1);
        i0.ɵɵelementStart(4, "mix-toolbar", 2);
        i0.ɵɵlistener("delete", function MixPolymorphousListComponent_Template_mix_toolbar_delete_4_listener() { return ctx.onDelete(); });
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(5, "button", 3);
        i0.ɵɵlistener("click", function MixPolymorphousListComponent_Template_button_click_5_listener() { return ctx.toggleFilter(); });
        i0.ɵɵelement(6, "i-tabler", 4);
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(7, "div", 5)(8, "div", 6)(9, "mix-data-table", 7);
        i0.ɵɵlistener("itemsSelectedChange", function MixPolymorphousListComponent_Template_mix_data_table_itemsSelectedChange_9_listener($event) { return ctx.itemSelectedChange($event); });
        i0.ɵɵelement(10, "div", 8)(11, "div", 9)(12, "div", 10)(13, "div", 11);
        i0.ɵɵelementStart(14, "div", 12);
        i0.ɵɵtemplate(15, MixPolymorphousListComponent_ng_template_15_Template, 1, 1, "ng-template", 13);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(16, "div", 14);
        i0.ɵɵtemplate(17, MixPolymorphousListComponent_ng_template_17_Template, 2, 1, "ng-template", 13);
        i0.ɵɵelementEnd()()();
        i0.ɵɵtemplate(18, MixPolymorphousListComponent_div_18_Template, 16, 9, "div", 15);
        i0.ɵɵelementEnd();
        i0.ɵɵtemplate(19, MixPolymorphousListComponent_ng_template_19_Template, 3, 1, "ng-template", null, 16, i0.ɵɵtemplateRefExtractor);
        i0.ɵɵtemplate(21, MixPolymorphousListComponent_ng_template_21_Template, 1, 2, "ng-template", null, 17, i0.ɵɵtemplateRefExtractor);
    } if (rf & 2) {
        i0.ɵɵadvance(2);
        i0.ɵɵtextInterpolate2("", ctx.itemCount, " ", ctx.contentConfig[ctx.listType].header, "");
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("selectedItem", ctx.currentSelectedItems);
        i0.ɵɵadvance(5);
        i0.ɵɵproperty("fetchDataFn", ctx.fetchDataFn)("searchPlaceholder", ctx.contentConfig[ctx.listType].searchPlaceholder)("loading$", ctx.loading$);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("columnType", "CHECKBOX")("showInSubTable", false);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("header", "#")("width", "70px");
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("header", "Title");
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("columnType", "DATE")("header", "Last Updated")("showInSubTable", false);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("header", "Status")("showInSubTable", false);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("columnType", "ACTION")("showHeader", false)("sortable", false);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("ngIf", ctx.showLeftSide);
    } }, dependencies: [ShareModule, i5.NgIf, i6.NgControlStatus, i6.FormControlDirective, i7.TuiSelectComponent, i7.TuiSelectDirective, i7.TuiMultiSelectComponent, i7.TuiMultiSelectGroupDirective, i8.TuiButtonComponent, i8.TuiHostedDropdownComponent, i8.TuiDataListComponent, i8.TuiOptionComponent, i8.TuiDataListDirective, i7.TuiDataListWrapperComponent, i8.TuiLabelComponent, i8.TuiTextfieldExampleTextDirective, i8.TuiTextfieldLabelOutsideDirective, i8.TuiTextfieldSizeDirective, i7.TuiInputDateRangeComponent, i7.TuiInputDateRangeDirective, i9.TablerIconComponent, MixDataTableModule, i10.MixDataTableComponent, i11.TableCellDirective, i12.TableColumnDirective, MixToolbarComponent,
        MixStatusIndicatorComponent,
        MixUserListHubComponent,
        MixModuleDetailComponent], styles: ["mix-data-table[_ngcontent-%COMP%]{width:100%}.polymorphous-list[_ngcontent-%COMP%]{width:100%;height:100%}.polymorphous-list__header[_ngcontent-%COMP%]{background-color:#f0f4f9;display:flex;align-items:center;padding:5px 15px;border-bottom:1px solid var(--tui-base-04)}.polymorphous-list__header[_ngcontent-%COMP%]   .separator[_ngcontent-%COMP%]{content:\"\";display:block;height:60%;width:1px;background-color:var(--tui-base-04);margin:0 15px 0 25px}.polymorphous-list__header[_ngcontent-%COMP%]   .toggle-left-side[_ngcontent-%COMP%]{margin-left:auto}.polymorphous-list__container[_ngcontent-%COMP%]{display:flex;width:100%;height:calc(100vh - 100px)}.polymorphous-list__main-workspace[_ngcontent-%COMP%]{width:100%;height:100%}.polymorphous-list__left-side[_ngcontent-%COMP%]{width:350px;border-left:1px solid var(--tui-base-04);padding:10px;display:flex;flex-direction:column}.polymorphous-list__left-side[_ngcontent-%COMP%]   .filter[_ngcontent-%COMP%]{background-color:#fff;border-radius:10px;padding:10px}.polymorphous-list__left-side[_ngcontent-%COMP%]   .filter[_ngcontent-%COMP%]   [tuiLabel][_ngcontent-%COMP%]{margin-bottom:15px}.polymorphous-list__left-side[_ngcontent-%COMP%]   .chatbox[_ngcontent-%COMP%]{margin-top:20px}"], changeDetection: 0 });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixPolymorphousListComponent, [{
        type: Component,
        args: [{ selector: 'mix-polymorphous-list', standalone: true, imports: [
                    ShareModule,
                    MixDataTableModule,
                    MixToolbarComponent,
                    MixStatusIndicatorComponent,
                    MixChatBoxComponent,
                    MixUserListHubComponent,
                    MixModuleDetailComponent
                ], changeDetection: ChangeDetectionStrategy.OnPush, template: "  <div class=\"polymorphous-list__header\"\r\n       header>\r\n    <div>{{ itemCount }} {{ contentConfig[listType].header }}</div>\r\n    <div class=\"separator\"></div>\r\n\r\n    <mix-toolbar [selectedItem]=\"currentSelectedItems\"\r\n                 (delete)=\"onDelete()\"></mix-toolbar>\r\n\r\n    <button class=\"toggle-left-side\"\r\n            (click)=\"toggleFilter()\"\r\n            tuiIconButton\r\n            size=\"s\">\r\n      <i-tabler name=\"filter\"></i-tabler>\r\n    </button>\r\n  </div>\r\n\r\n  <div class=\"polymorphous-list__container\">\r\n    <div class=\"polymorphous-list__main-workspace\">\r\n      <mix-data-table [fetchDataFn]=\"fetchDataFn\"\r\n                      [searchPlaceholder]=\"contentConfig[listType].searchPlaceholder\"\r\n                      (itemsSelectedChange)=\"itemSelectedChange($event)\"\r\n                      [loading$]=\"loading$\">\r\n        <div [columnType]=\"'CHECKBOX'\"\r\n             [showInSubTable]=\"false\"\r\n             mixTableColumn></div>\r\n        <div [header]=\"'#'\"\r\n             [width]=\"'70px'\"\r\n             mixTableColumn\r\n             key=\"id\"></div>\r\n        <div [header]=\"'Title'\"\r\n             mixTableColumn\r\n             key=\"title\"></div>\r\n        <div [columnType]=\"'DATE'\"\r\n             [header]=\"'Last Updated'\"\r\n             [showInSubTable]=\"false\"\r\n             key=\"createdDateTime\"\r\n             mixTableColumn>\r\n        </div>\r\n        <div [header]=\"'Status'\"\r\n             [showInSubTable]=\"false\"\r\n             mixTableColumn\r\n             key=\"status\">\r\n          <ng-template let-data\r\n                       mixColumnCell>\r\n            <mix-status-indicator [status]=\"data.status\"></mix-status-indicator>\r\n          </ng-template>\r\n        </div>\r\n        <div [columnType]=\"'ACTION'\"\r\n             [showHeader]=\"false\"\r\n             [sortable]=\"false\"\r\n             header=\"Action\"\r\n             mixTableColumn\r\n             key=\"Action\">\r\n          <ng-template let-data\r\n                       mixColumnCell>\r\n            <tui-hosted-dropdown [content]=\"action\"\r\n                                 tuiDropdownAlign=\"left\">\r\n              <button type=\"button\"\r\n                      (click)=\"currentActionItem = data\"\r\n                      tuiIconButton\r\n                      size=\"m\"\r\n                      appearance=\"flat\"\r\n                      icon=\"tuiIconMoreVer\">\r\n              </button>\r\n            </tui-hosted-dropdown>\r\n          </ng-template>\r\n        </div>\r\n      </mix-data-table>\r\n    </div>\r\n\r\n    <div *ngIf=\"showLeftSide\"\r\n         class=\"polymorphous-list__left-side\">\r\n      <div class=\"filter\">\r\n        <label tuiLabel>\r\n          From Date - To Date (Updated Date)\r\n          <tui-input-date-range class=\"b-form\"\r\n                                [tuiTextfieldSize]=\"'m'\">\r\n            Pick a date\r\n          </tui-input-date-range>\r\n        </label>\r\n\r\n        <label tuiLabel>\r\n          Status\r\n          <tui-multi-select class=\"b-form\"\r\n                            [editable]=\"false\"\r\n                            [expandable]=\"false\"\r\n                            [formControl]=\"statusControl\"\r\n                            [tuiTextfieldLabelOutside]=\"true\"\r\n                            [tuiTextfieldSize]=\"'m'\"\r\n                            tuiTextfieldExampleText=\"Ignored text\">\r\n            <tui-data-list-wrapper *tuiDataList\r\n                                   [items]=\"statusOption\"\r\n                                   tuiMultiSelectGroup></tui-data-list-wrapper>\r\n          </tui-multi-select>\r\n        </label>\r\n\r\n        <label tuiLabel>\r\n          Sort by:\r\n          <tui-select class=\"b-form\"\r\n                      [formControl]=\"sortOptionControl\"\r\n                      [tuiTextfieldLabelOutside]=\"true\"\r\n                      [tuiTextfieldSize]=\"'m'\"\r\n                      tuiTextfieldExampleText=\"Ignored text\">\r\n            <tui-data-list-wrapper *tuiDataList\r\n                                   [items]=\"sortOption\"></tui-data-list-wrapper>\r\n          </tui-select>\r\n        </label>\r\n      </div>\r\n\r\n      <div class=\"chatbox\">\r\n        <mix-user-list-hub></mix-user-list-hub>\r\n      </div>\r\n    </div>\r\n  </div>\r\n\r\n  <ng-template #action>\r\n    <tui-data-list>\r\n      <button class=\"tui-space_right-3\"\r\n              type=\"button\"\r\n              [size]=\"'s'\"\r\n              (click)=\"editItem()\"\r\n              tuiOption\r\n              tuiHintMode=\"onDark\"\r\n              tuiHintDirection=\"right\">\r\n        Edit\r\n      </button>\r\n    </tui-data-list>\r\n  </ng-template>\r\n\r\n  <ng-template #moduleDetail>\r\n    <mix-module-detail [mode]=\"'Quickly'\"\r\n                       [moduleId]=\"moduleId\"></mix-module-detail>\r\n  </ng-template>\r\n", styles: ["mix-data-table{width:100%}.polymorphous-list{width:100%;height:100%}.polymorphous-list__header{background-color:#f0f4f9;display:flex;align-items:center;padding:5px 15px;border-bottom:1px solid var(--tui-base-04)}.polymorphous-list__header .separator{content:\"\";display:block;height:60%;width:1px;background-color:var(--tui-base-04);margin:0 15px 0 25px}.polymorphous-list__header .toggle-left-side{margin-left:auto}.polymorphous-list__container{display:flex;width:100%;height:calc(100vh - 100px)}.polymorphous-list__main-workspace{width:100%;height:100%}.polymorphous-list__left-side{width:350px;border-left:1px solid var(--tui-base-04);padding:10px;display:flex;flex-direction:column}.polymorphous-list__left-side .filter{background-color:#fff;border-radius:10px;padding:10px}.polymorphous-list__left-side .filter [tuiLabel]{margin-bottom:15px}.polymorphous-list__left-side .chatbox{margin-top:20px}\n"] }]
    }], function () { return [{ type: i13.ModalService, decorators: [{
                type: Inject,
                args: [ModalService]
            }] }, { type: i1.MixPageApiService }, { type: i2.MixPostApiService }, { type: i3.MixModuleApiService }, { type: i4.AppEventService }, { type: i4.PortalSidebarControlService }]; }, { listType: [{
            type: Input
        }], request: [{
            type: Input
        }], deleteRequest: [{
            type: Input
        }], dataTable: [{
            type: ViewChild,
            args: [MixDataTableComponent]
        }], moduleDetail: [{
            type: ViewChild,
            args: ['moduleDetail']
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LXBvbHltb3JwaGV1cy1saXN0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtcG9seW1vcnBoZXVzLWxpc3QvbWl4LXBvbHltb3JwaGV1cy1saXN0LmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtcG9seW1vcnBoZXVzLWxpc3QvbWl4LXBvbHltb3JwaGV1cy1saXN0LmNvbXBvbmVudC5odG1sIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFDTCx1QkFBdUIsRUFDdkIsU0FBUyxFQUNULE1BQU0sRUFDTixLQUFLLEVBRUwsV0FBVyxFQUNYLFNBQVMsRUFDVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDN0MsT0FBTyxFQUNMLGdCQUFnQixFQUNoQixjQUFjLEVBTWYsTUFBTSxrQkFBa0IsQ0FBQztBQUMxQixPQUFPLEVBQUUsZUFBZSxFQUFFLGFBQWEsRUFBRSxNQUFNLEVBQWMsR0FBRyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBRS9FLE9BQU8sRUFDTCxRQUFRLEVBQ1IsZUFBZSxFQUNmLDJCQUEyQixFQUM1QixNQUFNLGdCQUFnQixDQUFDO0FBQ3hCLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLDJDQUEyQyxDQUFDO0FBQ2hGLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHlDQUF5QyxDQUFDO0FBQzVFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHlDQUF5QyxDQUFDO0FBQzVFLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztB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QkFBQTtRQUd6QixlQUFjO1FBQWQsNEJBQWMsaUJBQUE7UUFJZCxlQUFrQjtRQUFsQixnQ0FBa0I7UUFHbEIsZUFBcUI7UUFBckIsbUNBQXFCLDBCQUFBLHlCQUFBO1FBTXJCLGVBQW1CO1FBQW5CLGlDQUFtQix5QkFBQTtRQVNuQixlQUF1QjtRQUF2QixxQ0FBdUIscUJBQUEsbUJBQUE7UUF1QjFCLGVBQWtCO1FBQWxCLHVDQUFrQjt3QkRwQnhCLFdBQVcsdWhCQUNYLGtCQUFrQiwrRUFDbEIsbUJBQW1CO1FBQ25CLDJCQUEyQjtRQUUzQix1QkFBdUI7UUFDdkIsd0JBQXdCO3VGQUlmLDRCQUE0QjtjQWhCeEMsU0FBUzsyQkFDRSx1QkFBdUIsY0FHckIsSUFBSSxXQUNQO29CQUNQLFdBQVc7b0JBQ1gsa0JBQWtCO29CQUNsQixtQkFBbUI7b0JBQ25CLDJCQUEyQjtvQkFDM0IsbUJBQW1CO29CQUNuQix1QkFBdUI7b0JBQ3ZCLHdCQUF3QjtpQkFDekIsbUJBQ2dCLHVCQUF1QixDQUFDLE1BQU07O3NCQXNGNUMsTUFBTTt1QkFBQyxZQUFZO2tNQW5GTixRQUFRO2tCQUF2QixLQUFLO1lBQ1UsT0FBTztrQkFBdEIsS0FBSztZQUdVLGFBQWE7a0JBQTVCLEtBQUs7WUFHTixTQUFTO2tCQURSLFNBQVM7bUJBQUMscUJBQXFCO1lBRUUsWUFBWTtrQkFBN0MsU0FBUzttQkFBQyxjQUFjIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBDaGFuZ2VEZXRlY3Rpb25TdHJhdGVneSxcclxuICBDb21wb25lbnQsXHJcbiAgSW5qZWN0LFxyXG4gIElucHV0LFxyXG4gIE9uSW5pdCxcclxuICBUZW1wbGF0ZVJlZixcclxuICBWaWV3Q2hpbGRcclxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgRm9ybUNvbnRyb2wgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XHJcbmltcG9ydCB7XHJcbiAgTWl4Q29udGVudFN0YXR1cyxcclxuICBNaXhDb250ZW50VHlwZSxcclxuICBNaXhNb2R1bGVQb3J0YWxNb2RlbCxcclxuICBNaXhQYWdlUG9ydGFsTW9kZWwsXHJcbiAgTWl4UG9zdFBvcnRhbE1vZGVsLFxyXG4gIFBhZ2luYXRpb25SZXF1ZXN0TW9kZWwsXHJcbiAgUGFnaW5hdGlvblJlc3VsdE1vZGVsXHJcbn0gZnJvbSAnQG1peC1zcGEvbWl4LmxpYic7XHJcbmltcG9ydCB7IEJlaGF2aW9yU3ViamVjdCwgY29tYmluZUxhdGVzdCwgZmlsdGVyLCBPYnNlcnZhYmxlLCB0YXAgfSBmcm9tICdyeGpzJztcclxuXHJcbmltcG9ydCB7XHJcbiAgQXBwRXZlbnQsXHJcbiAgQXBwRXZlbnRTZXJ2aWNlLFxyXG4gIFBvcnRhbFNpZGViYXJDb250cm9sU2VydmljZVxyXG59IGZyb20gJy4uLy4uL3NlcnZpY2VzJztcclxuaW1wb3J0IHsgTWl4TW9kdWxlQXBpU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2FwaS9taXgtbW9kdWxlLWFwaS5zZXJ2aWNlJztcclxuaW1wb3J0IHsgTWl4UGFnZUFwaVNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9hcGkvbWl4LXBhZ2UtYXBpLnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBNaXhQb3N0QXBpU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2FwaS9taXgtcG9zdC1hcGkuc2VydmljZSc7XHJcbmltcG9ydCB7IFNoYXJlTW9kdWxlIH0gZnJvbSAnLi4vLi4vc2hhcmUubW9kdWxlJztcclxuaW1wb3J0IHsgTWl4RGF0YVRhYmxlQ29tcG9uZW50IH0gZnJvbSAnLi4vZGF0YS10YWJsZSc7XHJcbmltcG9ydCB7IE1peERhdGFUYWJsZU1vZHVsZSB9IGZyb20gJy4uL2RhdGEtdGFibGUvZGF0YS10YWJsZS5tb2R1bGUnO1xyXG5pbXBvcnQgeyBNaXhDaGF0Qm94Q29tcG9uZW50IH0gZnJvbSAnLi4vbWl4LWNoYXQnO1xyXG5pbXBvcnQgeyBNaXhNb2R1bGVEZXRhaWxDb21wb25lbnQgfSBmcm9tICcuLi9taXgtbW9kdWxlLWRldGFpbC9taXgtbW9kdWxlLWRldGFpbC5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBNaXhTdGF0dXNJbmRpY2F0b3JDb21wb25lbnQgfSBmcm9tICcuLi9taXgtc3RhdHVzLWluZGljYXRvcic7XHJcbmltcG9ydCB7IE1peFRvb2xiYXJDb21wb25lbnQgfSBmcm9tICcuLi9taXgtdG9vbGJhci9taXgtdG9vbGJhci5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBNb2RhbFNlcnZpY2UgfSBmcm9tICcuLi9tb2RhbC9tb2RhbC5zZXJ2aWNlJztcclxuaW1wb3J0IHsgTWl4VXNlckxpc3RIdWJDb21wb25lbnQgfSBmcm9tICcuLi91c2VyLWxpc3QtaHViL3VzZXItbGlzdC1odWIuY29tcG9uZW50JztcclxuXHJcbmV4cG9ydCB0eXBlIFBvbHltb3JwaG91c0xpc3RSZXN1bHQgPVxyXG4gIHwgTWl4UGFnZVBvcnRhbE1vZGVsXHJcbiAgfCBNaXhQb3N0UG9ydGFsTW9kZWxcclxuICB8IE1peE1vZHVsZVBvcnRhbE1vZGVsO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtcG9seW1vcnBob3VzLWxpc3QnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9taXgtcG9seW1vcnBoZXVzLWxpc3QuY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL21peC1wb2x5bW9ycGhldXMtbGlzdC5jb21wb25lbnQuc2NzcyddLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW1xyXG4gICAgU2hhcmVNb2R1bGUsXHJcbiAgICBNaXhEYXRhVGFibGVNb2R1bGUsXHJcbiAgICBNaXhUb29sYmFyQ29tcG9uZW50LFxyXG4gICAgTWl4U3RhdHVzSW5kaWNhdG9yQ29tcG9uZW50LFxyXG4gICAgTWl4Q2hhdEJveENvbXBvbmVudCxcclxuICAgIE1peFVzZXJMaXN0SHViQ29tcG9uZW50LFxyXG4gICAgTWl4TW9kdWxlRGV0YWlsQ29tcG9uZW50XHJcbiAgXSxcclxuICBjaGFuZ2VEZXRlY3Rpb246IENoYW5nZURldGVjdGlvblN0cmF0ZWd5Lk9uUHVzaFxyXG59KVxyXG5leHBvcnQgY2xhc3MgTWl4UG9seW1vcnBob3VzTGlzdENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XHJcbiAgQElucHV0KCkgcHVibGljIGxpc3RUeXBlOiBNaXhDb250ZW50VHlwZSA9IE1peENvbnRlbnRUeXBlLlBhZ2U7XHJcbiAgQElucHV0KCkgcHVibGljIHJlcXVlc3QhOiAoXHJcbiAgICBxdWVyeTogUGFnaW5hdGlvblJlcXVlc3RNb2RlbFxyXG4gICkgPT4gT2JzZXJ2YWJsZTxQYWdpbmF0aW9uUmVzdWx0TW9kZWw8UG9seW1vcnBob3VzTGlzdFJlc3VsdD4+O1xyXG4gIEBJbnB1dCgpIHB1YmxpYyBkZWxldGVSZXF1ZXN0ITogKGlkOiBudW1iZXIpID0+IE9ic2VydmFibGU8dm9pZD47XHJcblxyXG4gIEBWaWV3Q2hpbGQoTWl4RGF0YVRhYmxlQ29tcG9uZW50KVxyXG4gIGRhdGFUYWJsZSE6IE1peERhdGFUYWJsZUNvbXBvbmVudDxQb2x5bW9ycGhvdXNMaXN0UmVzdWx0PjtcclxuICBAVmlld0NoaWxkKCdtb2R1bGVEZXRhaWwnKSBwdWJsaWMgbW9kdWxlRGV0YWlsITogVGVtcGxhdGVSZWY8dW5rbm93bj47XHJcblxyXG4gIHB1YmxpYyByZWFkb25seSBjb250ZW50Q29uZmlnOiBSZWNvcmQ8XHJcbiAgICBNaXhDb250ZW50VHlwZSxcclxuICAgIHsgaGVhZGVyOiBzdHJpbmc7IHNlYXJjaFBsYWNlaG9sZGVyOiBzdHJpbmcgfVxyXG4gID4gPSB7XHJcbiAgICBQYWdlOiB7XHJcbiAgICAgIGhlYWRlcjogJ1BhZ2VzIEF2YWlsYWJsZScsXHJcbiAgICAgIHNlYXJjaFBsYWNlaG9sZGVyOiAnVHlwZSB5b3VyIFBhZ2UgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBQb3N0OiB7XHJcbiAgICAgIGhlYWRlcjogJ1Bvc3RzIEF2YWlsYWJsZScsXHJcbiAgICAgIHNlYXJjaFBsYWNlaG9sZGVyOiAnVHlwZSB5b3VyIFBvc3QgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBNb2R1bGU6IHtcclxuICAgICAgaGVhZGVyOiAnTW9kdWxlIHNBdmFpbGFibGUnLFxyXG4gICAgICBzZWFyY2hQbGFjZWhvbGRlcjogJ1R5cGUgeW91ciBNb2R1bGUgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBNaXhEYXRhYmFzZToge1xyXG4gICAgICBoZWFkZXI6ICdNaXhEYXRhYmFzZXMgQXZhaWxhYmxlJyxcclxuICAgICAgc2VhcmNoUGxhY2Vob2xkZXI6ICdUeXBlIHlvdXIgTWl4RGF0YWJhc2UgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBTY2hlZHVsZXI6IHtcclxuICAgICAgaGVhZGVyOiAnU2NoZWR1bGVycyBBdmFpbGFibGUnLFxyXG4gICAgICBzZWFyY2hQbGFjZWhvbGRlcjogJ1R5cGUgeW91ciBTY2hlZHVsZXIgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBUZW5hbnQ6IHtcclxuICAgICAgaGVhZGVyOiAnVGVuYW50cyBBdmFpbGFibGUnLFxyXG4gICAgICBzZWFyY2hQbGFjZWhvbGRlcjogJ1R5cGUgeW91ciBUZW5hbnQgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBEb21haW46IHtcclxuICAgICAgaGVhZGVyOiAnRG9tYWlucyBBdmFpbGFibGUnLFxyXG4gICAgICBzZWFyY2hQbGFjZWhvbGRlcjogJ1R5cGUgeW91ciBEb21haW4gbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBNZWRpYToge1xyXG4gICAgICBoZWFkZXI6ICdNZWRpYXMgQXZhaWxhYmxlJyxcclxuICAgICAgc2VhcmNoUGxhY2Vob2xkZXI6ICdUeXBlIHlvdXIgTWVkaWEgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBUaGVtZToge1xyXG4gICAgICBoZWFkZXI6ICdUaGVtZXMgQXZhaWxhYmxlJyxcclxuICAgICAgc2VhcmNoUGxhY2Vob2xkZXI6ICdUeXBlIHlvdXIgVGhlbWUgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBMYW5ndWFnZToge1xyXG4gICAgICBoZWFkZXI6ICdMYW5ndWFnZXMgQXZhaWxhYmxlJyxcclxuICAgICAgc2VhcmNoUGxhY2Vob2xkZXI6ICdUeXBlIHlvdXIgTGFuZ3VhZ2UgbmFtZS4uLidcclxuICAgIH0sXHJcbiAgICBMb2NhbGl6YXRpb246IHtcclxuICAgICAgaGVhZGVyOiAnTG9jYWxpemF0aW9ucyBBdmFpbGFibGUnLFxyXG4gICAgICBzZWFyY2hQbGFjZWhvbGRlcjogJ1R5cGUgeW91ciBMb2NhbGl6YXRpb24gbmFtZS4uLidcclxuICAgIH1cclxuICB9O1xyXG5cclxuICAvLyBGaWx0ZXIgQnkgU3RhdHVzXHJcbiAgcHVibGljIHJlYWRvbmx5IHN0YXR1c09wdGlvbjogTWl4Q29udGVudFN0YXR1c1tdID0gW1xyXG4gICAgTWl4Q29udGVudFN0YXR1cy5EcmFmdCxcclxuICAgIE1peENvbnRlbnRTdGF0dXMuUHVibGlzaGVkLFxyXG4gICAgTWl4Q29udGVudFN0YXR1cy5EZWxldGVkLFxyXG4gICAgTWl4Q29udGVudFN0YXR1cy5QcmV2aWV3XHJcbiAgXTtcclxuICBwdWJsaWMgc3RhdHVzQ29udHJvbDogRm9ybUNvbnRyb2wgPSBuZXcgRm9ybUNvbnRyb2wodGhpcy5zdGF0dXNPcHRpb24pO1xyXG5cclxuICAvLyBTb3J0IE9wdGlvblxyXG4gIHB1YmxpYyByZWFkb25seSBzb3J0T3B0aW9uOiBzdHJpbmdbXSA9IFsnTGFzdCBVcGRhdGVkJywgJ1ByaW9yaXR5J107XHJcbiAgcHVibGljIHNvcnRPcHRpb25Db250cm9sOiBGb3JtQ29udHJvbCA9IG5ldyBGb3JtQ29udHJvbCgnTGFzdCBVcGRhdGVkJyk7XHJcblxyXG4gIHB1YmxpYyBzaG93TGVmdFNpZGUgPSB0cnVlO1xyXG4gIHB1YmxpYyBsb2FkaW5nJDogQmVoYXZpb3JTdWJqZWN0PGJvb2xlYW4+ID0gbmV3IEJlaGF2aW9yU3ViamVjdDxib29sZWFuPihcclxuICAgIHRydWVcclxuICApO1xyXG4gIHB1YmxpYyBpdGVtQ291bnQgPSAwO1xyXG4gIHB1YmxpYyBjdXJyZW50U2VsZWN0ZWRJdGVtczogUG9seW1vcnBob3VzTGlzdFJlc3VsdFtdID0gW107XHJcbiAgcHVibGljIGN1cnJlbnRBY3Rpb25JdGVtOiBQb2x5bW9ycGhvdXNMaXN0UmVzdWx0IHwgdW5kZWZpbmVkID0gdW5kZWZpbmVkO1xyXG4gIHB1YmxpYyBtb2R1bGVJZCA9IDA7XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgQEluamVjdChNb2RhbFNlcnZpY2UpIHByaXZhdGUgcmVhZG9ubHkgbW9kYWxTZXJ2aWNlOiBNb2RhbFNlcnZpY2UsXHJcbiAgICBwdWJsaWMgcGFnZUFwaTogTWl4UGFnZUFwaVNlcnZpY2UsXHJcbiAgICBwdWJsaWMgcG9zdEFwaTogTWl4UG9zdEFwaVNlcnZpY2UsXHJcbiAgICBwdWJsaWMgbW9kdWxlQXBpOiBNaXhNb2R1bGVBcGlTZXJ2aWNlLFxyXG4gICAgcHJpdmF0ZSBhcHBFdmVudDogQXBwRXZlbnRTZXJ2aWNlLFxyXG4gICAgcHJpdmF0ZSBzaWRlYmFyQ29udHJvbDogUG9ydGFsU2lkZWJhckNvbnRyb2xTZXJ2aWNlXHJcbiAgKSB7fVxyXG5cclxuICBwdWJsaWMgbmdPbkluaXQoKTogdm9pZCB7XHJcbiAgICBzd2l0Y2ggKHRoaXMubGlzdFR5cGUpIHtcclxuICAgICAgY2FzZSBNaXhDb250ZW50VHlwZS5QYWdlOlxyXG4gICAgICAgIHRoaXMucmVxdWVzdCA9IChxdWVyeTogUGFnaW5hdGlvblJlcXVlc3RNb2RlbCkgPT5cclxuICAgICAgICAgIHRoaXMucGFnZUFwaS5nZXRQYWdlcyhxdWVyeSk7XHJcbiAgICAgICAgdGhpcy5kZWxldGVSZXF1ZXN0ID0gKGlkOiBudW1iZXIpID0+IHRoaXMucGFnZUFwaS5kZWxldGVQYWdlcyhpZCk7XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGNhc2UgTWl4Q29udGVudFR5cGUuUG9zdDpcclxuICAgICAgICB0aGlzLnJlcXVlc3QgPSAocXVlcnk6IFBhZ2luYXRpb25SZXF1ZXN0TW9kZWwpID0+XHJcbiAgICAgICAgICB0aGlzLnBvc3RBcGkuZ2V0UG9zdHMocXVlcnkpO1xyXG4gICAgICAgIHRoaXMuZGVsZXRlUmVxdWVzdCA9IChpZDogbnVtYmVyKSA9PiB0aGlzLnBvc3RBcGkuZGVsZXRlUG9zdHMoaWQpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgICBkZWZhdWx0OlxyXG4gICAgICAgIHRoaXMucmVxdWVzdCA9IChxdWVyeTogUGFnaW5hdGlvblJlcXVlc3RNb2RlbCkgPT5cclxuICAgICAgICAgIHRoaXMubW9kdWxlQXBpLmdldE1vZHVsZXMocXVlcnkpO1xyXG4gICAgICAgIHRoaXMuZGVsZXRlUmVxdWVzdCA9IChpZDogbnVtYmVyKSA9PiB0aGlzLm1vZHVsZUFwaS5kZWxldGVNb2R1bGVzKGlkKTtcclxuICAgICAgICBicmVhaztcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmFwcEV2ZW50LmV2ZW50JFxyXG4gICAgICAucGlwZShcclxuICAgICAgICBmaWx0ZXIoZXZlbnQgPT5cclxuICAgICAgICAgIFtcclxuICAgICAgICAgICAgQXBwRXZlbnQuTmV3TW9kdWxlQWRkZWQsXHJcbiAgICAgICAgICAgIEFwcEV2ZW50Lk5ld1BhZ2VBZGRlZCxcclxuICAgICAgICAgICAgQXBwRXZlbnQuTmV3UG9zdEFkZGVkXHJcbiAgICAgICAgICBdLmluY2x1ZGVzKGV2ZW50KVxyXG4gICAgICAgIClcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgICB0aGlzLmRhdGFUYWJsZS5yZWxvYWREYXRhKCk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGZldGNoRGF0YUZuID0gKHF1ZXJ5OiBQYWdpbmF0aW9uUmVxdWVzdE1vZGVsKSA9PiB7XHJcbiAgICByZXR1cm4gdGhpcy5yZXF1ZXN0KHF1ZXJ5KS5waXBlKFxyXG4gICAgICB0YXAocmVzdWx0ID0+IHtcclxuICAgICAgICB0aGlzLml0ZW1Db3VudCA9IHJlc3VsdC5wYWdpbmdEYXRhLnRvdGFsIHx8IDA7XHJcbiAgICAgIH0pXHJcbiAgICApO1xyXG4gIH07XHJcblxyXG4gIHB1YmxpYyBpdGVtU2VsZWN0ZWRDaGFuZ2UoaXRlbXM6IFBvbHltb3JwaG91c0xpc3RSZXN1bHRbXSk6IHZvaWQge1xyXG4gICAgdGhpcy5jdXJyZW50U2VsZWN0ZWRJdGVtcyA9IGl0ZW1zO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIG9uRGVsZXRlKCk6IHZvaWQge1xyXG4gICAgY29uc3QgbWVzc2FnZSA9XHJcbiAgICAgICdBcmUgeW91IHN1cmUgdG8gZGVsZXRlIHRoaXMgaXRlbXMgPyBZb3VyIGRhdGEgbWF5IG5vdCBiZSByZXZlcnQnO1xyXG4gICAgdGhpcy5tb2RhbFNlcnZpY2UuY29uZmlybShtZXNzYWdlKS5zdWJzY3JpYmUoKG9rOiBib29sZWFuKSA9PiB7XHJcbiAgICAgIGlmIChvaykgdGhpcy5kZWxldGVJdGVtKCk7XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBkZWxldGVJdGVtKCk6IHZvaWQge1xyXG4gICAgdGhpcy5sb2FkaW5nJC5uZXh0KHRydWUpO1xyXG4gICAgY29tYmluZUxhdGVzdChcclxuICAgICAgdGhpcy5jdXJyZW50U2VsZWN0ZWRJdGVtcy5tYXAodiA9PiB0aGlzLmRlbGV0ZVJlcXVlc3Qodi5pZCkpXHJcbiAgICApLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgIHRoaXMubW9kYWxTZXJ2aWNlLnN1Y2Nlc3MoJ1N1Y2Nlc3NmdWxseSBkZWxldGUgZGF0YScpLnN1YnNjcmliZSgpO1xyXG4gICAgICB0aGlzLmRhdGFUYWJsZS5yZWxvYWREYXRhKCk7XHJcbiAgICAgIHRoaXMuY3VycmVudFNlbGVjdGVkSXRlbXMgPSBbXTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIHRvZ2dsZUZpbHRlcigpOiB2b2lkIHtcclxuICAgIHRoaXMuc2hvd0xlZnRTaWRlID0gIXRoaXMuc2hvd0xlZnRTaWRlO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGVkaXRJdGVtKCk6IHZvaWQge1xyXG4gICAgaWYgKCF0aGlzLmN1cnJlbnRBY3Rpb25JdGVtKSByZXR1cm47XHJcblxyXG4gICAgc3dpdGNoICh0aGlzLmxpc3RUeXBlKSB7XHJcbiAgICAgIGNhc2UgTWl4Q29udGVudFR5cGUuTW9kdWxlOlxyXG4gICAgICAgIHRoaXMubW9kdWxlSWQgPSB0aGlzLmN1cnJlbnRBY3Rpb25JdGVtLmlkO1xyXG4gICAgICAgIHRoaXMuc2lkZWJhckNvbnRyb2wuc2hvdyh0aGlzLm1vZHVsZURldGFpbCk7XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGRlZmF1bHQ6XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiIsIiAgPGRpdiBjbGFzcz1cInBvbHltb3JwaG91cy1saXN0X19oZWFkZXJcIlxyXG4gICAgICAgaGVhZGVyPlxyXG4gICAgPGRpdj57eyBpdGVtQ291bnQgfX0ge3sgY29udGVudENvbmZpZ1tsaXN0VHlwZV0uaGVhZGVyIH19PC9kaXY+XHJcbiAgICA8ZGl2IGNsYXNzPVwic2VwYXJhdG9yXCI+PC9kaXY+XHJcblxyXG4gICAgPG1peC10b29sYmFyIFtzZWxlY3RlZEl0ZW1dPVwiY3VycmVudFNlbGVjdGVkSXRlbXNcIlxyXG4gICAgICAgICAgICAgICAgIChkZWxldGUpPVwib25EZWxldGUoKVwiPjwvbWl4LXRvb2xiYXI+XHJcblxyXG4gICAgPGJ1dHRvbiBjbGFzcz1cInRvZ2dsZS1sZWZ0LXNpZGVcIlxyXG4gICAgICAgICAgICAoY2xpY2spPVwidG9nZ2xlRmlsdGVyKClcIlxyXG4gICAgICAgICAgICB0dWlJY29uQnV0dG9uXHJcbiAgICAgICAgICAgIHNpemU9XCJzXCI+XHJcbiAgICAgIDxpLXRhYmxlciBuYW1lPVwiZmlsdGVyXCI+PC9pLXRhYmxlcj5cclxuICAgIDwvYnV0dG9uPlxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2IGNsYXNzPVwicG9seW1vcnBob3VzLWxpc3RfX2NvbnRhaW5lclwiPlxyXG4gICAgPGRpdiBjbGFzcz1cInBvbHltb3JwaG91cy1saXN0X19tYWluLXdvcmtzcGFjZVwiPlxyXG4gICAgICA8bWl4LWRhdGEtdGFibGUgW2ZldGNoRGF0YUZuXT1cImZldGNoRGF0YUZuXCJcclxuICAgICAgICAgICAgICAgICAgICAgIFtzZWFyY2hQbGFjZWhvbGRlcl09XCJjb250ZW50Q29uZmlnW2xpc3RUeXBlXS5zZWFyY2hQbGFjZWhvbGRlclwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAoaXRlbXNTZWxlY3RlZENoYW5nZSk9XCJpdGVtU2VsZWN0ZWRDaGFuZ2UoJGV2ZW50KVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICBbbG9hZGluZyRdPVwibG9hZGluZyRcIj5cclxuICAgICAgICA8ZGl2IFtjb2x1bW5UeXBlXT1cIidDSEVDS0JPWCdcIlxyXG4gICAgICAgICAgICAgW3Nob3dJblN1YlRhYmxlXT1cImZhbHNlXCJcclxuICAgICAgICAgICAgIG1peFRhYmxlQ29sdW1uPjwvZGl2PlxyXG4gICAgICAgIDxkaXYgW2hlYWRlcl09XCInIydcIlxyXG4gICAgICAgICAgICAgW3dpZHRoXT1cIic3MHB4J1wiXHJcbiAgICAgICAgICAgICBtaXhUYWJsZUNvbHVtblxyXG4gICAgICAgICAgICAga2V5PVwiaWRcIj48L2Rpdj5cclxuICAgICAgICA8ZGl2IFtoZWFkZXJdPVwiJ1RpdGxlJ1wiXHJcbiAgICAgICAgICAgICBtaXhUYWJsZUNvbHVtblxyXG4gICAgICAgICAgICAga2V5PVwidGl0bGVcIj48L2Rpdj5cclxuICAgICAgICA8ZGl2IFtjb2x1bW5UeXBlXT1cIidEQVRFJ1wiXHJcbiAgICAgICAgICAgICBbaGVhZGVyXT1cIidMYXN0IFVwZGF0ZWQnXCJcclxuICAgICAgICAgICAgIFtzaG93SW5TdWJUYWJsZV09XCJmYWxzZVwiXHJcbiAgICAgICAgICAgICBrZXk9XCJjcmVhdGVkRGF0ZVRpbWVcIlxyXG4gICAgICAgICAgICAgbWl4VGFibGVDb2x1bW4+XHJcbiAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgPGRpdiBbaGVhZGVyXT1cIidTdGF0dXMnXCJcclxuICAgICAgICAgICAgIFtzaG93SW5TdWJUYWJsZV09XCJmYWxzZVwiXHJcbiAgICAgICAgICAgICBtaXhUYWJsZUNvbHVtblxyXG4gICAgICAgICAgICAga2V5PVwic3RhdHVzXCI+XHJcbiAgICAgICAgICA8bmctdGVtcGxhdGUgbGV0LWRhdGFcclxuICAgICAgICAgICAgICAgICAgICAgICBtaXhDb2x1bW5DZWxsPlxyXG4gICAgICAgICAgICA8bWl4LXN0YXR1cy1pbmRpY2F0b3IgW3N0YXR1c109XCJkYXRhLnN0YXR1c1wiPjwvbWl4LXN0YXR1cy1pbmRpY2F0b3I+XHJcbiAgICAgICAgICA8L25nLXRlbXBsYXRlPlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICAgIDxkaXYgW2NvbHVtblR5cGVdPVwiJ0FDVElPTidcIlxyXG4gICAgICAgICAgICAgW3Nob3dIZWFkZXJdPVwiZmFsc2VcIlxyXG4gICAgICAgICAgICAgW3NvcnRhYmxlXT1cImZhbHNlXCJcclxuICAgICAgICAgICAgIGhlYWRlcj1cIkFjdGlvblwiXHJcbiAgICAgICAgICAgICBtaXhUYWJsZUNvbHVtblxyXG4gICAgICAgICAgICAga2V5PVwiQWN0aW9uXCI+XHJcbiAgICAgICAgICA8bmctdGVtcGxhdGUgbGV0LWRhdGFcclxuICAgICAgICAgICAgICAgICAgICAgICBtaXhDb2x1bW5DZWxsPlxyXG4gICAgICAgICAgICA8dHVpLWhvc3RlZC1kcm9wZG93biBbY29udGVudF09XCJhY3Rpb25cIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB0dWlEcm9wZG93bkFsaWduPVwibGVmdFwiPlxyXG4gICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAoY2xpY2spPVwiY3VycmVudEFjdGlvbkl0ZW0gPSBkYXRhXCJcclxuICAgICAgICAgICAgICAgICAgICAgIHR1aUljb25CdXR0b25cclxuICAgICAgICAgICAgICAgICAgICAgIHNpemU9XCJtXCJcclxuICAgICAgICAgICAgICAgICAgICAgIGFwcGVhcmFuY2U9XCJmbGF0XCJcclxuICAgICAgICAgICAgICAgICAgICAgIGljb249XCJ0dWlJY29uTW9yZVZlclwiPlxyXG4gICAgICAgICAgICAgIDwvYnV0dG9uPlxyXG4gICAgICAgICAgICA8L3R1aS1ob3N0ZWQtZHJvcGRvd24+XHJcbiAgICAgICAgICA8L25nLXRlbXBsYXRlPlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICA8L21peC1kYXRhLXRhYmxlPlxyXG4gICAgPC9kaXY+XHJcblxyXG4gICAgPGRpdiAqbmdJZj1cInNob3dMZWZ0U2lkZVwiXHJcbiAgICAgICAgIGNsYXNzPVwicG9seW1vcnBob3VzLWxpc3RfX2xlZnQtc2lkZVwiPlxyXG4gICAgICA8ZGl2IGNsYXNzPVwiZmlsdGVyXCI+XHJcbiAgICAgICAgPGxhYmVsIHR1aUxhYmVsPlxyXG4gICAgICAgICAgRnJvbSBEYXRlIC0gVG8gRGF0ZSAoVXBkYXRlZCBEYXRlKVxyXG4gICAgICAgICAgPHR1aS1pbnB1dC1kYXRlLXJhbmdlIGNsYXNzPVwiYi1mb3JtXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBbdHVpVGV4dGZpZWxkU2l6ZV09XCInbSdcIj5cclxuICAgICAgICAgICAgUGljayBhIGRhdGVcclxuICAgICAgICAgIDwvdHVpLWlucHV0LWRhdGUtcmFuZ2U+XHJcbiAgICAgICAgPC9sYWJlbD5cclxuXHJcbiAgICAgICAgPGxhYmVsIHR1aUxhYmVsPlxyXG4gICAgICAgICAgU3RhdHVzXHJcbiAgICAgICAgICA8dHVpLW11bHRpLXNlbGVjdCBjbGFzcz1cImItZm9ybVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBbZWRpdGFibGVdPVwiZmFsc2VcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgW2V4cGFuZGFibGVdPVwiZmFsc2VcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgW2Zvcm1Db250cm9sXT1cInN0YXR1c0NvbnRyb2xcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgW3R1aVRleHRmaWVsZExhYmVsT3V0c2lkZV09XCJ0cnVlXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIFt0dWlUZXh0ZmllbGRTaXplXT1cIidtJ1wiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB0dWlUZXh0ZmllbGRFeGFtcGxlVGV4dD1cIklnbm9yZWQgdGV4dFwiPlxyXG4gICAgICAgICAgICA8dHVpLWRhdGEtbGlzdC13cmFwcGVyICp0dWlEYXRhTGlzdFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFtpdGVtc109XCJzdGF0dXNPcHRpb25cIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHR1aU11bHRpU2VsZWN0R3JvdXA+PC90dWktZGF0YS1saXN0LXdyYXBwZXI+XHJcbiAgICAgICAgICA8L3R1aS1tdWx0aS1zZWxlY3Q+XHJcbiAgICAgICAgPC9sYWJlbD5cclxuXHJcbiAgICAgICAgPGxhYmVsIHR1aUxhYmVsPlxyXG4gICAgICAgICAgU29ydCBieTpcclxuICAgICAgICAgIDx0dWktc2VsZWN0IGNsYXNzPVwiYi1mb3JtXCJcclxuICAgICAgICAgICAgICAgICAgICAgIFtmb3JtQ29udHJvbF09XCJzb3J0T3B0aW9uQ29udHJvbFwiXHJcbiAgICAgICAgICAgICAgICAgICAgICBbdHVpVGV4dGZpZWxkTGFiZWxPdXRzaWRlXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgW3R1aVRleHRmaWVsZFNpemVdPVwiJ20nXCJcclxuICAgICAgICAgICAgICAgICAgICAgIHR1aVRleHRmaWVsZEV4YW1wbGVUZXh0PVwiSWdub3JlZCB0ZXh0XCI+XHJcbiAgICAgICAgICAgIDx0dWktZGF0YS1saXN0LXdyYXBwZXIgKnR1aURhdGFMaXN0XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgW2l0ZW1zXT1cInNvcnRPcHRpb25cIj48L3R1aS1kYXRhLWxpc3Qtd3JhcHBlcj5cclxuICAgICAgICAgIDwvdHVpLXNlbGVjdD5cclxuICAgICAgICA8L2xhYmVsPlxyXG4gICAgICA8L2Rpdj5cclxuXHJcbiAgICAgIDxkaXYgY2xhc3M9XCJjaGF0Ym94XCI+XHJcbiAgICAgICAgPG1peC11c2VyLWxpc3QtaHViPjwvbWl4LXVzZXItbGlzdC1odWI+XHJcbiAgICAgIDwvZGl2PlxyXG4gICAgPC9kaXY+XHJcbiAgPC9kaXY+XHJcblxyXG4gIDxuZy10ZW1wbGF0ZSAjYWN0aW9uPlxyXG4gICAgPHR1aS1kYXRhLWxpc3Q+XHJcbiAgICAgIDxidXR0b24gY2xhc3M9XCJ0dWktc3BhY2VfcmlnaHQtM1wiXHJcbiAgICAgICAgICAgICAgdHlwZT1cImJ1dHRvblwiXHJcbiAgICAgICAgICAgICAgW3NpemVdPVwiJ3MnXCJcclxuICAgICAgICAgICAgICAoY2xpY2spPVwiZWRpdEl0ZW0oKVwiXHJcbiAgICAgICAgICAgICAgdHVpT3B0aW9uXHJcbiAgICAgICAgICAgICAgdHVpSGludE1vZGU9XCJvbkRhcmtcIlxyXG4gICAgICAgICAgICAgIHR1aUhpbnREaXJlY3Rpb249XCJyaWdodFwiPlxyXG4gICAgICAgIEVkaXRcclxuICAgICAgPC9idXR0b24+XHJcbiAgICA8L3R1aS1kYXRhLWxpc3Q+XHJcbiAgPC9uZy10ZW1wbGF0ZT5cclxuXHJcbiAgPG5nLXRlbXBsYXRlICNtb2R1bGVEZXRhaWw+XHJcbiAgICA8bWl4LW1vZHVsZS1kZXRhaWwgW21vZGVdPVwiJ1F1aWNrbHknXCJcclxuICAgICAgICAgICAgICAgICAgICAgICBbbW9kdWxlSWRdPVwibW9kdWxlSWRcIj48L21peC1tb2R1bGUtZGV0YWlsPlxyXG4gIDwvbmctdGVtcGxhdGU+XHJcbiJdfQ==