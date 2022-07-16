import { DashboardInformation } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases';
import * as i0 from "@angular/core";
export declare class DashboardApiService extends BaseApiService {
    getDashboardInfo(): Observable<DashboardInformation>;
    static ɵfac: i0.ɵɵFactoryDeclaration<DashboardApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<DashboardApiService>;
}
