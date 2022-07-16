import { MixDatabaseModel, PaginationRequestModel, PaginationResultModel } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases/base-api.service';
import * as i0 from "@angular/core";
export declare class DatabaseApiService extends BaseApiService {
    getDatabase(request: PaginationRequestModel): Observable<PaginationResultModel<MixDatabaseModel>>;
    static ɵfac: i0.ɵɵFactoryDeclaration<DatabaseApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<DatabaseApiService>;
}
