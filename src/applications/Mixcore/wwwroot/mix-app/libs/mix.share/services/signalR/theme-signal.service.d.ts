import { BaseSignalService } from '../../bases/base-hub.service';
import * as i0 from "@angular/core";
export declare class ThemeSignalService extends BaseSignalService {
    get _hubName(): string;
    get _roomName(): string;
    _setupSignalREvents(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<ThemeSignalService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<ThemeSignalService>;
}
