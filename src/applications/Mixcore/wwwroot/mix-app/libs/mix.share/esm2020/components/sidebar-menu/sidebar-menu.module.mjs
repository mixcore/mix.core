import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TuiSvgModule } from '@taiga-ui/core';
import { TuiTreeModule } from '@taiga-ui/kit';
import { MenuItemComponent } from './menu-item/menu-item';
import { SidebarMenuComponent } from './sidebar-menu.component';
import * as i0 from "@angular/core";
export class SidebarMenuModule {
}
SidebarMenuModule.ɵfac = function SidebarMenuModule_Factory(t) { return new (t || SidebarMenuModule)(); };
SidebarMenuModule.ɵmod = /*@__PURE__*/ i0.ɵɵdefineNgModule({ type: SidebarMenuModule });
SidebarMenuModule.ɵinj = /*@__PURE__*/ i0.ɵɵdefineInjector({ imports: [CommonModule, TuiTreeModule, TuiSvgModule] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(SidebarMenuModule, [{
        type: NgModule,
        args: [{
                declarations: [SidebarMenuComponent, MenuItemComponent],
                imports: [CommonModule, TuiTreeModule, TuiSvgModule],
                exports: [SidebarMenuComponent]
            }]
    }], null, null); })();
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && i0.ɵɵsetNgModuleScope(SidebarMenuModule, { declarations: [SidebarMenuComponent, MenuItemComponent], imports: [CommonModule, TuiTreeModule, TuiSvgModule], exports: [SidebarMenuComponent] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2lkZWJhci1tZW51Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9zaWRlYmFyLW1lbnUvc2lkZWJhci1tZW51Lm1vZHVsZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDOUMsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUU5QyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSx1QkFBdUIsQ0FBQztBQUMxRCxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQzs7QUFPaEUsTUFBTSxPQUFPLGlCQUFpQjs7a0ZBQWpCLGlCQUFpQjttRUFBakIsaUJBQWlCO3VFQUhsQixZQUFZLEVBQUUsYUFBYSxFQUFFLFlBQVk7dUZBR3hDLGlCQUFpQjtjQUw3QixRQUFRO2VBQUM7Z0JBQ1IsWUFBWSxFQUFFLENBQUMsb0JBQW9CLEVBQUUsaUJBQWlCLENBQUM7Z0JBQ3ZELE9BQU8sRUFBRSxDQUFDLFlBQVksRUFBRSxhQUFhLEVBQUUsWUFBWSxDQUFDO2dCQUNwRCxPQUFPLEVBQUUsQ0FBQyxvQkFBb0IsQ0FBQzthQUNoQzs7d0ZBQ1ksaUJBQWlCLG1CQUpiLG9CQUFvQixFQUFFLGlCQUFpQixhQUM1QyxZQUFZLEVBQUUsYUFBYSxFQUFFLFlBQVksYUFDekMsb0JBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tbW9uTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uJztcclxuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgVHVpU3ZnTW9kdWxlIH0gZnJvbSAnQHRhaWdhLXVpL2NvcmUnO1xyXG5pbXBvcnQgeyBUdWlUcmVlTW9kdWxlIH0gZnJvbSAnQHRhaWdhLXVpL2tpdCc7XHJcblxyXG5pbXBvcnQgeyBNZW51SXRlbUNvbXBvbmVudCB9IGZyb20gJy4vbWVudS1pdGVtL21lbnUtaXRlbSc7XHJcbmltcG9ydCB7IFNpZGViYXJNZW51Q29tcG9uZW50IH0gZnJvbSAnLi9zaWRlYmFyLW1lbnUuY29tcG9uZW50JztcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgZGVjbGFyYXRpb25zOiBbU2lkZWJhck1lbnVDb21wb25lbnQsIE1lbnVJdGVtQ29tcG9uZW50XSxcclxuICBpbXBvcnRzOiBbQ29tbW9uTW9kdWxlLCBUdWlUcmVlTW9kdWxlLCBUdWlTdmdNb2R1bGVdLFxyXG4gIGV4cG9ydHM6IFtTaWRlYmFyTWVudUNvbXBvbmVudF1cclxufSlcclxuZXhwb3J0IGNsYXNzIFNpZGViYXJNZW51TW9kdWxlIHt9XHJcbiJdfQ==