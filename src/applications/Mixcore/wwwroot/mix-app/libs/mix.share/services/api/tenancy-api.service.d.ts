import { IInitFullTenantRequest, InitStep, ThemeAdditionalData } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases/base-api.service';
import * as i0 from "@angular/core";
export declare class TenancyApiService extends BaseApiService {
    initFullTenant(request: IInitFullTenantRequest): Observable<void>;
    getInitStatus(): Observable<InitStep>;
    installTheme(themeModel: ThemeAdditionalData): Observable<boolean>;
    static ɵfac: i0.ɵɵFactoryDeclaration<TenancyApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<TenancyApiService>;
}
