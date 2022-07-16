import { ActivatedRoute, Params, Router } from '@angular/router';
import { AuthApiService } from '../../services';
import { ModalService } from '../modal/modal.service';
import { HeaderMenuService } from './header-menu.service';
import * as i0 from "@angular/core";
export interface BreadcrumbOption {
    caption: string;
    params: Params;
    routerLink: string;
}
export declare class HeaderMenuComponent {
    authService: AuthApiService;
    headerService: HeaderMenuService;
    private router;
    private readonly modalService;
    private activatedRoute;
    user$: import("rxjs").BehaviorSubject<import("../../../../../../../../mix.core/src/applications/Mixcore/wwwroot/mix-spa/libs/mix.lib/src").User | null>;
    breadcrumb: BreadcrumbOption[];
    constructor(authService: AuthApiService, headerService: HeaderMenuService, router: Router, modalService: ModalService, activatedRoute: ActivatedRoute);
    logout(): void;
    private _registerRouterChange;
    private _getBreadcrumbs;
    static ɵfac: i0.ɵɵFactoryDeclaration<HeaderMenuComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<HeaderMenuComponent, "mix-header-menu", never, {}, {}, never, never, true>;
}
