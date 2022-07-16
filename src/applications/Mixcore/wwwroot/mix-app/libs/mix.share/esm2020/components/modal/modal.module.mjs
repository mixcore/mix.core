import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TuiButtonModule } from '@taiga-ui/core';
import { PolymorpheusModule } from '@tinkoff/ng-polymorpheus';
import { ModalComponent } from './modal.component';
import { MODAL_PROVIDER } from './modal.service';
import * as i0 from "@angular/core";
export class MixModalModule {
}
MixModalModule.ɵfac = function MixModalModule_Factory(t) { return new (t || MixModalModule)(); };
MixModalModule.ɵmod = /*@__PURE__*/ i0.ɵɵdefineNgModule({ type: MixModalModule });
MixModalModule.ɵinj = /*@__PURE__*/ i0.ɵɵdefineInjector({ providers: [MODAL_PROVIDER], imports: [TuiButtonModule, PolymorpheusModule, CommonModule] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixModalModule, [{
        type: NgModule,
        args: [{
                imports: [TuiButtonModule, PolymorpheusModule, CommonModule],
                providers: [MODAL_PROVIDER],
                declarations: [ModalComponent],
                exports: [ModalComponent],
                entryComponents: [ModalComponent]
            }]
    }], null, null); })();
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && i0.ɵɵsetNgModuleScope(MixModalModule, { declarations: [ModalComponent], imports: [TuiButtonModule, PolymorpheusModule, CommonModule], exports: [ModalComponent] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwubW9kdWxlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21vZGFsL21vZGFsLm1vZHVsZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDakQsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFFOUQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG1CQUFtQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQzs7QUFTakQsTUFBTSxPQUFPLGNBQWM7OzRFQUFkLGNBQWM7Z0VBQWQsY0FBYztxRUFMZCxDQUFDLGNBQWMsQ0FBQyxZQURqQixlQUFlLEVBQUUsa0JBQWtCLEVBQUUsWUFBWTt1RkFNaEQsY0FBYztjQVAxQixRQUFRO2VBQUM7Z0JBQ1IsT0FBTyxFQUFFLENBQUMsZUFBZSxFQUFFLGtCQUFrQixFQUFFLFlBQVksQ0FBQztnQkFDNUQsU0FBUyxFQUFFLENBQUMsY0FBYyxDQUFDO2dCQUMzQixZQUFZLEVBQUUsQ0FBQyxjQUFjLENBQUM7Z0JBQzlCLE9BQU8sRUFBRSxDQUFDLGNBQWMsQ0FBQztnQkFDekIsZUFBZSxFQUFFLENBQUMsY0FBYyxDQUFDO2FBQ2xDOzt3RkFDWSxjQUFjLG1CQUpWLGNBQWMsYUFGbkIsZUFBZSxFQUFFLGtCQUFrQixFQUFFLFlBQVksYUFHakQsY0FBYyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbW1vbk1vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbic7XHJcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFR1aUJ1dHRvbk1vZHVsZSB9IGZyb20gJ0B0YWlnYS11aS9jb3JlJztcclxuaW1wb3J0IHsgUG9seW1vcnBoZXVzTW9kdWxlIH0gZnJvbSAnQHRpbmtvZmYvbmctcG9seW1vcnBoZXVzJztcclxuXHJcbmltcG9ydCB7IE1vZGFsQ29tcG9uZW50IH0gZnJvbSAnLi9tb2RhbC5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBNT0RBTF9QUk9WSURFUiB9IGZyb20gJy4vbW9kYWwuc2VydmljZSc7XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGltcG9ydHM6IFtUdWlCdXR0b25Nb2R1bGUsIFBvbHltb3JwaGV1c01vZHVsZSwgQ29tbW9uTW9kdWxlXSxcclxuICBwcm92aWRlcnM6IFtNT0RBTF9QUk9WSURFUl0sXHJcbiAgZGVjbGFyYXRpb25zOiBbTW9kYWxDb21wb25lbnRdLFxyXG4gIGV4cG9ydHM6IFtNb2RhbENvbXBvbmVudF0sXHJcbiAgZW50cnlDb21wb25lbnRzOiBbTW9kYWxDb21wb25lbnRdXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBNaXhNb2RhbE1vZHVsZSB7fVxyXG4iXX0=