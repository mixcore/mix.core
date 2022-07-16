import { HttpClient } from '@angular/common/http';
import { IPaginationResult, PaginationRequestModel, PaginationResultModel, ThemeModel } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases/base-api.service';
import { AppEventService } from '../helper/app-event.service';
import * as i0 from "@angular/core";
export declare class ThemeApiService extends BaseApiService {
    protected readonly http: HttpClient;
    baseUrl: string;
    getThemeStoreUrl: string;
    appEvent: AppEventService;
    constructor(http: HttpClient, baseUrl: string, getThemeStoreUrl: string, appEvent: AppEventService);
    getThemeStore(): Observable<IPaginationResult<ThemeModel>>;
    getThemes(request: PaginationRequestModel): Observable<PaginationResultModel<ThemeModel>>;
    static ɵfac: i0.ɵɵFactoryDeclaration<ThemeApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<ThemeApiService>;
}
