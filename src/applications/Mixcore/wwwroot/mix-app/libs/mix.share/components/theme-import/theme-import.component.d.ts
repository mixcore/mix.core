import { EventEmitter } from '@angular/core';
import { ThemeModel } from '@mix-spa/mix.lib';
import { ThemeApiService } from '../../services';
import { ModalService } from '../modal/modal.service';
import * as i0 from "@angular/core";
export declare class ThemeImportComponent {
    themeApiService: ThemeApiService;
    private readonly modalService;
    domain: string;
    activeItemIndex: number;
    themeListVm$: import("rxjs").Observable<import("@mix-spa/mix.lib").IPaginationResult<ThemeModel>>;
    currentSelectedTheme: ThemeModel | null;
    cancel: EventEmitter<void>;
    themeSelect: EventEmitter<ThemeModel>;
    constructor(themeApiService: ThemeApiService, modalService: ModalService, domain: string);
    selectTheme(value: ThemeModel): void;
    onCancelClick(): void;
    onUseThemeClick(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<ThemeImportComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<ThemeImportComponent, "mix-theme-import", never, {}, { "cancel": "cancel"; "themeSelect": "themeSelect"; }, never, never, true>;
}
