import { Provider } from '@angular/core';
import { AbstractTuiDialogService } from '@taiga-ui/cdk';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { Observable } from 'rxjs';
import { ModalComponent } from './modal.component';
import * as i0 from "@angular/core";
export declare class ModalService extends AbstractTuiDialogService<ModalOption> {
    modalShadowColor: Record<'success' | 'error' | 'warning' | 'confirm' | 'info', string>;
    readonly defaultOptions: ModalOption;
    readonly component: PolymorpheusComponent<ModalComponent, object>;
    show(message: string, title: string): Observable<boolean>;
    confirm(message: string): Observable<boolean>;
    info(message: string): Observable<boolean>;
    success(message: string): Observable<void>;
    error(message: string): Observable<void>;
    warning(message: string): Observable<void>;
    static ɵfac: i0.ɵɵFactoryDeclaration<ModalService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<ModalService>;
}
export declare const MODAL_PROVIDER: Provider;
export interface ModalOption {
    readonly heading: string;
    readonly buttons: readonly [string, string];
    readonly borderShadowColor?: string;
}
