import { MixUser } from '@mix-spa/mix.lib';
import * as i0 from "@angular/core";
export declare class MixChatBoxComponent {
    usersOnChatting: MixUser[];
    usersOnMinimize: MixUser[];
    addChatting(user: MixUser): void;
    addToMinimize(user: MixUser): void;
    closeChat(user: MixUser): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixChatBoxComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixChatBoxComponent, "mix-chat-box", never, {}, {}, never, never, true>;
}
