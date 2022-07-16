import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AbstractTuiPortalHostComponent, AbstractTuiPortalService } from '@taiga-ui/cdk';
import { PortalSidebarControlService } from '../../services';
import * as i0 from "@angular/core";
export class PortalSidebarHostComponent extends AbstractTuiPortalHostComponent {
}
PortalSidebarHostComponent.ɵfac = /*@__PURE__*/ function () { let ɵPortalSidebarHostComponent_BaseFactory; return function PortalSidebarHostComponent_Factory(t) { return (ɵPortalSidebarHostComponent_BaseFactory || (ɵPortalSidebarHostComponent_BaseFactory = i0.ɵɵgetInheritedFactory(PortalSidebarHostComponent)))(t || PortalSidebarHostComponent); }; }();
PortalSidebarHostComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: PortalSidebarHostComponent, selectors: [["mix-portal-sidebar-host"]], standalone: true, features: [i0.ɵɵProvidersFeature([
            {
                provide: AbstractTuiPortalService,
                useExisting: PortalSidebarControlService
            }
        ]), i0.ɵɵInheritDefinitionFeature, i0.ɵɵStandaloneFeature], decls: 3, vars: 0, consts: [[1, "portal-sidebar-host"], ["viewContainer", ""]], template: function PortalSidebarHostComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0);
        i0.ɵɵelementContainer(1, null, 1);
        i0.ɵɵelementEnd();
    } }, dependencies: [CommonModule], styles: ["[_nghost-%COMP%]{position:absolute;top:0;right:0;height:100%;z-index:2}.portal-sidebar-host[_ngcontent-%COMP%]{height:100%;width:100%}"], changeDetection: 0 });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(PortalSidebarHostComponent, [{
        type: Component,
        args: [{ selector: 'mix-portal-sidebar-host', standalone: true, imports: [CommonModule], changeDetection: ChangeDetectionStrategy.OnPush, providers: [
                    {
                        provide: AbstractTuiPortalService,
                        useExisting: PortalSidebarControlService
                    }
                ], template: "<div class=\"portal-sidebar-host\">\r\n  <ng-container #viewContainer></ng-container>\r\n</div>\r\n", styles: [":host{position:absolute;top:0;right:0;height:100%;z-index:2}.portal-sidebar-host{height:100%;width:100%}\n"] }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicG9ydGFsLXNpZGViYXItaG9zdC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL2NvbXBvbmVudHMvcG9ydGFsLXNpZGViYXItaG9zdC9wb3J0YWwtc2lkZWJhci1ob3N0LmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9wb3J0YWwtc2lkZWJhci1ob3N0L3BvcnRhbC1zaWRlYmFyLWhvc3QuY29tcG9uZW50Lmh0bWwiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQy9DLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUNMLDhCQUE4QixFQUM5Qix3QkFBd0IsRUFDekIsTUFBTSxlQUFlLENBQUM7QUFFdkIsT0FBTyxFQUFFLDJCQUEyQixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7O0FBZ0I3RCxNQUFNLE9BQU8sMEJBQTJCLFNBQVEsOEJBQThCOzswUkFBakUsMEJBQTBCLFNBQTFCLDBCQUEwQjs2RUFBMUIsMEJBQTBCLCtGQVAxQjtZQUNUO2dCQUNFLE9BQU8sRUFBRSx3QkFBd0I7Z0JBQ2pDLFdBQVcsRUFBRSwyQkFBMkI7YUFDekM7U0FDRjtRQ3JCSCw4QkFBaUM7UUFDL0IsaUNBQTRDO1FBQzlDLGlCQUFNO3dCRFlNLFlBQVk7dUZBU1gsMEJBQTBCO2NBZHRDLFNBQVM7MkJBQ0UseUJBQXlCLGNBR3ZCLElBQUksV0FDUCxDQUFDLFlBQVksQ0FBQyxtQkFDTix1QkFBdUIsQ0FBQyxNQUFNLGFBQ3BDO29CQUNUO3dCQUNFLE9BQU8sRUFBRSx3QkFBd0I7d0JBQ2pDLFdBQVcsRUFBRSwyQkFBMkI7cUJBQ3pDO2lCQUNGIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tbW9uTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uJztcclxuaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3ksIENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQge1xyXG4gIEFic3RyYWN0VHVpUG9ydGFsSG9zdENvbXBvbmVudCxcclxuICBBYnN0cmFjdFR1aVBvcnRhbFNlcnZpY2VcclxufSBmcm9tICdAdGFpZ2EtdWkvY2RrJztcclxuXHJcbmltcG9ydCB7IFBvcnRhbFNpZGViYXJDb250cm9sU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnbWl4LXBvcnRhbC1zaWRlYmFyLWhvc3QnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9wb3J0YWwtc2lkZWJhci1ob3N0LmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi9wb3J0YWwtc2lkZWJhci1ob3N0LmNvbXBvbmVudC5zY3NzJ10sXHJcbiAgc3RhbmRhbG9uZTogdHJ1ZSxcclxuICBpbXBvcnRzOiBbQ29tbW9uTW9kdWxlXSxcclxuICBjaGFuZ2VEZXRlY3Rpb246IENoYW5nZURldGVjdGlvblN0cmF0ZWd5Lk9uUHVzaCxcclxuICBwcm92aWRlcnM6IFtcclxuICAgIHtcclxuICAgICAgcHJvdmlkZTogQWJzdHJhY3RUdWlQb3J0YWxTZXJ2aWNlLFxyXG4gICAgICB1c2VFeGlzdGluZzogUG9ydGFsU2lkZWJhckNvbnRyb2xTZXJ2aWNlXHJcbiAgICB9XHJcbiAgXVxyXG59KVxyXG5leHBvcnQgY2xhc3MgUG9ydGFsU2lkZWJhckhvc3RDb21wb25lbnQgZXh0ZW5kcyBBYnN0cmFjdFR1aVBvcnRhbEhvc3RDb21wb25lbnQge31cclxuIiwiPGRpdiBjbGFzcz1cInBvcnRhbC1zaWRlYmFyLWhvc3RcIj5cclxuICA8bmctY29udGFpbmVyICN2aWV3Q29udGFpbmVyPjwvbmctY29udGFpbmVyPlxyXG48L2Rpdj5cclxuIl19