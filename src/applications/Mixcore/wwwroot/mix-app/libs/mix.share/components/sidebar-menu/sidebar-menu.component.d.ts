import { Router } from '@angular/router';
import { TuiHandler } from '@taiga-ui/cdk';
import { SidebarMenuService } from './sidebar-menu.service';
import * as i0 from "@angular/core";
interface TreeNode {
    readonly text: string;
    readonly icon?: string;
    readonly action?: () => void;
    readonly children?: readonly TreeNode[];
}
export declare class SidebarMenuComponent {
    sidebarService: SidebarMenuService;
    route: Router;
    isExpanded: boolean;
    treeMap: Map<TreeNode, boolean>;
    data: TreeNode;
    constructor(sidebarService: SidebarMenuService, route: Router);
    get collapseIcon(): string;
    handler: TuiHandler<TreeNode, readonly TreeNode[]>;
    toggleMenu(): void;
    menuItemIconClick(node: TreeNode): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<SidebarMenuComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<SidebarMenuComponent, "mix-sidebar-menu", never, {}, {}, never, never, false>;
}
export {};
