import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import * as i0 from "@angular/core";
export class HeaderMenuService {
    constructor() {
        this.title$ = new BehaviorSubject('');
    }
    setTitle(text) {
        this.title$.next(text);
    }
}
HeaderMenuService.ɵfac = function HeaderMenuService_Factory(t) { return new (t || HeaderMenuService)(); };
HeaderMenuService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: HeaderMenuService, factory: HeaderMenuService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(HeaderMenuService, [{
        type: Injectable,
        args: [{ providedIn: 'root' }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaGVhZGVyLW1lbnUuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9oZWFkZXItbWVudS9oZWFkZXItbWVudS5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLE1BQU0sQ0FBQzs7QUFHdkMsTUFBTSxPQUFPLGlCQUFpQjtJQUQ5QjtRQUVTLFdBQU0sR0FBNEIsSUFBSSxlQUFlLENBQUMsRUFBRSxDQUFDLENBQUM7S0FLbEU7SUFIUSxRQUFRLENBQUMsSUFBWTtRQUMxQixJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztJQUN6QixDQUFDOztrRkFMVSxpQkFBaUI7dUVBQWpCLGlCQUFpQixXQUFqQixpQkFBaUIsbUJBREosTUFBTTt1RkFDbkIsaUJBQWlCO2NBRDdCLFVBQVU7ZUFBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEJlaGF2aW9yU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIEhlYWRlck1lbnVTZXJ2aWNlIHtcclxuICBwdWJsaWMgdGl0bGUkOiBCZWhhdmlvclN1YmplY3Q8c3RyaW5nPiA9IG5ldyBCZWhhdmlvclN1YmplY3QoJycpO1xyXG5cclxuICBwdWJsaWMgc2V0VGl0bGUodGV4dDogc3RyaW5nKTogdm9pZCB7XHJcbiAgICB0aGlzLnRpdGxlJC5uZXh0KHRleHQpO1xyXG4gIH1cclxufVxyXG4iXX0=