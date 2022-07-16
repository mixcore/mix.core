import { OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { TabControlService } from '../../../services/helper/tab-control.service';
import * as i0 from "@angular/core";
export declare class TabControlDialogComponent implements OnDestroy {
    tabControl: TabControlService;
    router: Router;
    item$: import("rxjs").BehaviorSubject<import("../../../services/helper/tab-control.service").TabControl[]>;
    index$: import("rxjs").BehaviorSubject<number>;
    constructor(tabControl: TabControlService, router: Router);
    ngOnDestroy(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<TabControlDialogComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<TabControlDialogComponent, "mix-tab-control-dialog", never, {}, {}, never, never, true>;
}
