import { Culture, GlobalSettings } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases/base-api.service';
import * as i0 from "@angular/core";
export declare class ShareApiService extends BaseApiService {
    getCultures(): Observable<Culture[]>;
    getGlobalSetting(): Observable<GlobalSettings>;
    static ɵfac: i0.ɵɵFactoryDeclaration<ShareApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<ShareApiService>;
}
