import { OnInit } from '@angular/core';
import { PortalSidebarControlService } from '../../services';
import { MixModuleApiService } from '../../services/api/mix-module-api.service';
import * as i0 from "@angular/core";
export declare class MixModuleDetailComponent implements OnInit {
    private sidebarControl;
    private moduleApi;
    mode: 'Quickly' | 'FullPage';
    moduleId: number;
    constructor(sidebarControl: PortalSidebarControlService, moduleApi: MixModuleApiService);
    closeSidebar(): void;
    ngOnInit(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixModuleDetailComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixModuleDetailComponent, "mix-module-detail", never, { "mode": "mode"; "moduleId": "moduleId"; }, {}, never, never, true>;
}
