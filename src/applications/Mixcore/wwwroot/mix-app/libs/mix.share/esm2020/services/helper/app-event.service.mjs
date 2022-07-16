import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import * as i0 from "@angular/core";
export var AppEvent;
(function (AppEvent) {
    AppEvent["NewModuleAdded"] = "NewModuleAdded";
    AppEvent["NewPageAdded"] = "NewPageAdded";
    AppEvent["NewPostAdded"] = "NewPostAdded";
})(AppEvent || (AppEvent = {}));
export class AppEventService {
    constructor() {
        this.event$ = new Subject();
    }
    notify(event) {
        this.event$.next(event);
    }
}
AppEventService.ɵfac = function AppEventService_Factory(t) { return new (t || AppEventService)(); };
AppEventService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: AppEventService, factory: AppEventService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(AppEventService, [{
        type: Injectable,
        args: [{ providedIn: 'root' }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwLWV2ZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL3NlcnZpY2VzL2hlbHBlci9hcHAtZXZlbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7O0FBRS9CLE1BQU0sQ0FBTixJQUFZLFFBSVg7QUFKRCxXQUFZLFFBQVE7SUFDbEIsNkNBQWlDLENBQUE7SUFDakMseUNBQTZCLENBQUE7SUFDN0IseUNBQTZCLENBQUE7QUFDL0IsQ0FBQyxFQUpXLFFBQVEsS0FBUixRQUFRLFFBSW5CO0FBR0QsTUFBTSxPQUFPLGVBQWU7SUFENUI7UUFFUyxXQUFNLEdBQXNCLElBQUksT0FBTyxFQUFFLENBQUM7S0FLbEQ7SUFIUSxNQUFNLENBQUMsS0FBZTtRQUMzQixJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUMxQixDQUFDOzs4RUFMVSxlQUFlO3FFQUFmLGVBQWUsV0FBZixlQUFlLG1CQURGLE1BQU07dUZBQ25CLGVBQWU7Y0FEM0IsVUFBVTtlQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG5cclxuZXhwb3J0IGVudW0gQXBwRXZlbnQge1xyXG4gIE5ld01vZHVsZUFkZGVkID0gJ05ld01vZHVsZUFkZGVkJyxcclxuICBOZXdQYWdlQWRkZWQgPSAnTmV3UGFnZUFkZGVkJyxcclxuICBOZXdQb3N0QWRkZWQgPSAnTmV3UG9zdEFkZGVkJ1xyXG59XHJcblxyXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxyXG5leHBvcnQgY2xhc3MgQXBwRXZlbnRTZXJ2aWNlIHtcclxuICBwdWJsaWMgZXZlbnQkOiBTdWJqZWN0PEFwcEV2ZW50PiA9IG5ldyBTdWJqZWN0KCk7XHJcblxyXG4gIHB1YmxpYyBub3RpZnkoZXZlbnQ6IEFwcEV2ZW50KTogdm9pZCB7XHJcbiAgICB0aGlzLmV2ZW50JC5uZXh0KGV2ZW50KTtcclxuICB9XHJcbn1cclxuIl19