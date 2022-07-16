import { Injectable } from '@angular/core';
import { MixApiDict } from '@mix-spa/mix.lib';
import { BaseApiService } from '../../bases';
import * as i0 from "@angular/core";
export class DashboardApiService extends BaseApiService {
    getDashboardInfo() {
        return this.get(MixApiDict.ShareApi.getSharedDashboardInfoEndpoint);
    }
}
DashboardApiService.ɵfac = /*@__PURE__*/ function () { let ɵDashboardApiService_BaseFactory; return function DashboardApiService_Factory(t) { return (ɵDashboardApiService_BaseFactory || (ɵDashboardApiService_BaseFactory = i0.ɵɵgetInheritedFactory(DashboardApiService)))(t || DashboardApiService); }; }();
DashboardApiService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: DashboardApiService, factory: DashboardApiService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(DashboardApiService, [{
        type: Injectable,
        args: [{ providedIn: 'root' }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGFzaGJvYXJkLWFwaS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9zZXJ2aWNlcy9hcGkvZGFzaGJvYXJkLWFwaS5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUF3QixVQUFVLEVBQUUsTUFBTSxrQkFBa0IsQ0FBQztBQUdwRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sYUFBYSxDQUFDOztBQUc3QyxNQUFNLE9BQU8sbUJBQW9CLFNBQVEsY0FBYztJQUM5QyxnQkFBZ0I7UUFDckIsT0FBTyxJQUFJLENBQUMsR0FBRyxDQUF1QixVQUFVLENBQUMsUUFBUSxDQUFDLDhCQUE4QixDQUFDLENBQUM7SUFDNUYsQ0FBQzs7dVBBSFUsbUJBQW1CLFNBQW5CLG1CQUFtQjt5RUFBbkIsbUJBQW1CLFdBQW5CLG1CQUFtQixtQkFETixNQUFNO3VGQUNuQixtQkFBbUI7Y0FEL0IsVUFBVTtlQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgRGFzaGJvYXJkSW5mb3JtYXRpb24sIE1peEFwaURpY3QgfSBmcm9tICdAbWl4LXNwYS9taXgubGliJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5cclxuaW1wb3J0IHsgQmFzZUFwaVNlcnZpY2UgfSBmcm9tICcuLi8uLi9iYXNlcyc7XHJcblxyXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxyXG5leHBvcnQgY2xhc3MgRGFzaGJvYXJkQXBpU2VydmljZSBleHRlbmRzIEJhc2VBcGlTZXJ2aWNlIHtcclxuICBwdWJsaWMgZ2V0RGFzaGJvYXJkSW5mbygpOiBPYnNlcnZhYmxlPERhc2hib2FyZEluZm9ybWF0aW9uPiB7XHJcbiAgICByZXR1cm4gdGhpcy5nZXQ8RGFzaGJvYXJkSW5mb3JtYXRpb24+KE1peEFwaURpY3QuU2hhcmVBcGkuZ2V0U2hhcmVkRGFzaGJvYXJkSW5mb0VuZHBvaW50KTtcclxuICB9XHJcbn1cclxuIl19