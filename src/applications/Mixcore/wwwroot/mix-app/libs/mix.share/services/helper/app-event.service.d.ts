import { Subject } from 'rxjs';
import * as i0 from "@angular/core";
export declare enum AppEvent {
    NewModuleAdded = "NewModuleAdded",
    NewPageAdded = "NewPageAdded",
    NewPostAdded = "NewPostAdded"
}
export declare class AppEventService {
    event$: Subject<AppEvent>;
    notify(event: AppEvent): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<AppEventService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<AppEventService>;
}
