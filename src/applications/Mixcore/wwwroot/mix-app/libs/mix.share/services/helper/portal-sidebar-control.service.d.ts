import { EmbeddedViewRef, TemplateRef } from '@angular/core';
import { AbstractTuiPortalService } from '@taiga-ui/cdk';
import * as i0 from "@angular/core";
export declare class PortalSidebarControlService extends AbstractTuiPortalService {
    currentTemplate: EmbeddedViewRef<unknown> | null;
    show(templateRef: TemplateRef<unknown>): void;
    hide(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<PortalSidebarControlService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<PortalSidebarControlService>;
}
