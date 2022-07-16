import { MixPagePortalModel, PaginationRequestModel, PaginationResultModel } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases/base-api.service';
import * as i0 from "@angular/core";
export declare class MixPageApiService extends BaseApiService {
    getDefaultPageTemplate(): Observable<MixPagePortalModel>;
    getPages(request: PaginationRequestModel): Observable<PaginationResultModel<MixPagePortalModel>>;
    deletePages(id: number): Observable<void>;
    savePage(data: MixPagePortalModel): Observable<void>;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixPageApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<MixPageApiService>;
}
