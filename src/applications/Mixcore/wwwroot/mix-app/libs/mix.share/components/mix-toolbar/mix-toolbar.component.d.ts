import { EventEmitter } from '@angular/core';
import * as i0 from "@angular/core";
export declare class MixToolbarComponent<T> {
    selectedItem: T[];
    delete: EventEmitter<void>;
    onDelete(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixToolbarComponent<any>, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixToolbarComponent<any>, "mix-toolbar", never, { "selectedItem": "selectedItem"; }, { "delete": "delete"; }, never, never, true>;
}
