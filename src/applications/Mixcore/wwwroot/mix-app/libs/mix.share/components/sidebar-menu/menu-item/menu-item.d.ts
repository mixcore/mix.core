import { OnDestroy } from '@angular/core';
import { TuiTreeController, TuiTreeItemContentComponent, TuiTreeItemContext } from '@taiga-ui/kit';
import { SidebarMenuService } from '../sidebar-menu.service';
import * as i0 from "@angular/core";
export declare class MenuItemComponent extends TuiTreeItemContentComponent implements OnDestroy {
    sbS: SidebarMenuService;
    constructor(context: TuiTreeItemContext, controller: TuiTreeController, sbS: SidebarMenuService);
    get icon(): string;
    ngOnDestroy(): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MenuItemComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MenuItemComponent, "mix-menu-item", never, {}, {}, never, never, false>;
}
