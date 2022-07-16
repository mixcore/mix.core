import { EventEmitter } from '@angular/core';
import { MixUser } from '@mix-spa/mix.lib';
import * as i0 from "@angular/core";
export declare class MixMessengerCardComponent {
    user: MixUser;
    closeChat: EventEmitter<MixUser>;
    minimizeChat: EventEmitter<MixUser>;
    onCloseChat(): void;
    onMinimizeChat(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixMessengerCardComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixMessengerCardComponent, "mix-messenger-card [user]", never, { "user": "user"; }, { "closeChat": "closeChat"; "minimizeChat": "minimizeChat"; }, never, never, true>;
}
