import { OnInit, TemplateRef } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MixContentStatus, MixContentType, MixModulePortalModel, MixPagePortalModel, MixPostPortalModel, PaginationRequestModel, PaginationResultModel } from '@mix-spa/mix.lib';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppEventService, PortalSidebarControlService } from '../../services';
import { MixModuleApiService } from '../../services/api/mix-module-api.service';
import { MixPageApiService } from '../../services/api/mix-page-api.service';
import { MixPostApiService } from '../../services/api/mix-post-api.service';
import { MixDataTableComponent } from '../data-table';
import { ModalService } from '../modal/modal.service';
import * as i0 from "@angular/core";
export declare type PolymorphousListResult = MixPagePortalModel | MixPostPortalModel | MixModulePortalModel;
export declare class MixPolymorphousListComponent implements OnInit {
    private readonly modalService;
    pageApi: MixPageApiService;
    postApi: MixPostApiService;
    moduleApi: MixModuleApiService;
    private appEvent;
    private sidebarControl;
    listType: MixContentType;
    request: (query: PaginationRequestModel) => Observable<PaginationResultModel<PolymorphousListResult>>;
    deleteRequest: (id: number) => Observable<void>;
    dataTable: MixDataTableComponent<PolymorphousListResult>;
    moduleDetail: TemplateRef<unknown>;
    readonly contentConfig: Record<MixContentType, {
        header: string;
        searchPlaceholder: string;
    }>;
    readonly statusOption: MixContentStatus[];
    statusControl: FormControl;
    readonly sortOption: string[];
    sortOptionControl: FormControl;
    showLeftSide: boolean;
    loading$: BehaviorSubject<boolean>;
    itemCount: number;
    currentSelectedItems: PolymorphousListResult[];
    currentActionItem: PolymorphousListResult | undefined;
    moduleId: number;
    constructor(modalService: ModalService, pageApi: MixPageApiService, postApi: MixPostApiService, moduleApi: MixModuleApiService, appEvent: AppEventService, sidebarControl: PortalSidebarControlService);
    ngOnInit(): void;
    fetchDataFn: (query: PaginationRequestModel) => Observable<PaginationResultModel<PolymorphousListResult>>;
    itemSelectedChange(items: PolymorphousListResult[]): void;
    onDelete(): void;
    deleteItem(): void;
    toggleFilter(): void;
    editItem(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixPolymorphousListComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixPolymorphousListComponent, "mix-polymorphous-list", never, { "listType": "listType"; "request": "request"; "deleteRequest": "deleteRequest"; }, {}, never, never, true>;
}