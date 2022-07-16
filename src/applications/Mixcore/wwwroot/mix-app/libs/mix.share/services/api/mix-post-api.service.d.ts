import { MixPostPortalModel, PaginationRequestModel, PaginationResultModel } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases/base-api.service';
import * as i0 from "@angular/core";
export declare class MixPostApiService extends BaseApiService {
    getDefaultPostTemplate(): Observable<MixPostPortalModel>;
    savePost(data: MixPostPortalModel): Observable<void>;
    getPosts(request: PaginationRequestModel): Observable<PaginationResultModel<MixPostPortalModel>>;
    deletePosts(id: number): Observable<void>;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixPostApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<MixPostApiService>;
}
