import { MixModulePortalModel, PaginationRequestModel, PaginationResultModel } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases';
import * as i0 from "@angular/core";
export declare class MixModuleApiService extends BaseApiService {
    getDefaultModuleTemplate(): Observable<MixModulePortalModel>;
    getModules(request: PaginationRequestModel): Observable<PaginationResultModel<MixModulePortalModel>>;
    getModuleById(id: number): Observable<MixModulePortalModel>;
    saveModule(data: MixModulePortalModel): Observable<void>;
    deleteModules(id: number): Observable<void>;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixModuleApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<MixModuleApiService>;
}
