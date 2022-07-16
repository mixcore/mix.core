import { TuiDialog } from '@taiga-ui/cdk';
import { ModalOption } from './modal.service';
import * as i0 from "@angular/core";
export declare class ModalComponent {
    readonly context: TuiDialog<ModalOption, boolean>;
    constructor(context: TuiDialog<ModalOption, boolean>);
    onClick(response: boolean): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<ModalComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<ModalComponent, "mix-modal", never, {}, {}, never, never, false>;
}
