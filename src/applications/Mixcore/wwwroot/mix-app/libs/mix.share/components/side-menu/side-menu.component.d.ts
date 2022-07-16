import { OnInit } from '@angular/core';
import { VerticalDisplayPosition } from '@mix-spa/mix.lib';
import * as i0 from "@angular/core";
export interface MixToolbarMenu {
    id: number;
    title: string;
    icon: string;
    hideDetail?: boolean;
    action?: () => void;
    detail: MenuItem[];
    position: VerticalDisplayPosition;
}
export interface MenuItem {
    icon: string;
    title: string;
    route?: string | string[];
    action?: () => void;
}
export declare class SideMenuComponent implements OnInit {
    showMenuLevel2: boolean;
    menuItems: MixToolbarMenu[];
    currentSelectedItem: MixToolbarMenu | undefined;
    readonly VerticalDisplayPosition: typeof VerticalDisplayPosition;
    ngOnInit(): void;
    itemSelect(item: MixToolbarMenu): void;
    itemClick(item: MenuItem): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<SideMenuComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<SideMenuComponent, "mix-side-menu", never, { "showMenuLevel2": "showMenuLevel2"; "menuItems": "menuItems"; }, {}, never, never, true>;
}
