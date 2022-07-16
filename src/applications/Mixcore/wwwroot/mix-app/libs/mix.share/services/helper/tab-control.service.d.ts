import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import * as i0 from "@angular/core";
export interface TabControl {
    path: string;
    title: string;
}
export declare class TabControlService {
    private router;
    private activatedRoute;
    navControl: TabControl[];
    navControl$: BehaviorSubject<TabControl[]>;
    index$: BehaviorSubject<number>;
    whiteLists: string[];
    constructor(router: Router, activatedRoute: ActivatedRoute);
    init(): void;
    nextTab(): void;
    unTab(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<TabControlService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<TabControlService>;
}
