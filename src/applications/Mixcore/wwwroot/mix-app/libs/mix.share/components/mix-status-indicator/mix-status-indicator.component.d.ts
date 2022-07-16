import { MixContentStatus } from '@mix-spa/mix.lib';
import * as i0 from "@angular/core";
export declare class MixStatusIndicatorComponent {
    status: MixContentStatus;
    readonly option: Record<MixContentStatus, {
        label: string;
        color: string;
    }>;
    constructor();
    static ɵfac: i0.ɵɵFactoryDeclaration<MixStatusIndicatorComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixStatusIndicatorComponent, "mix-status-indicator [status]", never, { "status": "status"; }, {}, never, never, true>;
}
