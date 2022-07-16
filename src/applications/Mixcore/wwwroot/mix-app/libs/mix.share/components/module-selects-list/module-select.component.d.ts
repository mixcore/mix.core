import { PaginationRequestModel } from '@mix-spa/mix.lib';
import { BehaviorSubject } from 'rxjs';
import { MixModuleApiService } from '../../services/api/mix-module-api.service';
import * as i0 from "@angular/core";
export declare class MixModuleSelectComponent {
    private moduleApi;
    filter$: BehaviorSubject<PaginationRequestModel>;
    data$: import("rxjs").Observable<import("@mix-spa/mix.lib").PaginationResultModel<import("@mix-spa/mix.lib").MixModulePortalModel>>;
    constructor(moduleApi: MixModuleApiService);
    static ɵfac: i0.ɵɵFactoryDeclaration<MixModuleSelectComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixModuleSelectComponent, "mix-module-select", never, {}, {}, never, never, true>;
}
