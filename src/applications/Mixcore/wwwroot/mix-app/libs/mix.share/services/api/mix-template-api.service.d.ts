import { IGetTemplatesRequest, MixTemplateModel, PaginationResultModel } from '@mix-spa/mix.lib';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../bases';
import * as i0 from "@angular/core";
export declare class MixTemplateApiService extends BaseApiService {
    getTemplates(request: IGetTemplatesRequest): Observable<PaginationResultModel<MixTemplateModel>>;
    getTemplateById(id: string): Observable<MixTemplateModel>;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixTemplateApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<MixTemplateApiService>;
}
