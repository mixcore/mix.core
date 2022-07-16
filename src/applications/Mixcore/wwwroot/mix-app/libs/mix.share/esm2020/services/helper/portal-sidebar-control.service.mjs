import { Injectable } from '@angular/core';
import { AbstractTuiPortalService } from '@taiga-ui/cdk';
import * as i0 from "@angular/core";
export class PortalSidebarControlService extends AbstractTuiPortalService {
    constructor() {
        super(...arguments);
        this.currentTemplate = null;
    }
    show(templateRef) {
        if (this.currentTemplate) {
            this.removeTemplate(this.currentTemplate);
        }
        this.currentTemplate = this.addTemplate(templateRef);
    }
    hide() {
        if (this.currentTemplate)
            this.removeTemplate(this.currentTemplate);
        this.currentTemplate = null;
    }
}
PortalSidebarControlService.ɵfac = /*@__PURE__*/ function () { let ɵPortalSidebarControlService_BaseFactory; return function PortalSidebarControlService_Factory(t) { return (ɵPortalSidebarControlService_BaseFactory || (ɵPortalSidebarControlService_BaseFactory = i0.ɵɵgetInheritedFactory(PortalSidebarControlService)))(t || PortalSidebarControlService); }; }();
PortalSidebarControlService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: PortalSidebarControlService, factory: PortalSidebarControlService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(PortalSidebarControlService, [{
        type: Injectable,
        args: [{
                providedIn: 'root'
            }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicG9ydGFsLXNpZGViYXItY29udHJvbC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9zZXJ2aWNlcy9oZWxwZXIvcG9ydGFsLXNpZGViYXItY29udHJvbC5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBbUIsVUFBVSxFQUFlLE1BQU0sZUFBZSxDQUFDO0FBQ3pFLE9BQU8sRUFBRSx3QkFBd0IsRUFBRSxNQUFNLGVBQWUsQ0FBQzs7QUFLekQsTUFBTSxPQUFPLDJCQUE0QixTQUFRLHdCQUF3QjtJQUh6RTs7UUFJUyxvQkFBZSxHQUFvQyxJQUFJLENBQUM7S0FlaEU7SUFiUSxJQUFJLENBQUMsV0FBaUM7UUFDM0MsSUFBSSxJQUFJLENBQUMsZUFBZSxFQUFFO1lBQ3hCLElBQUksQ0FBQyxjQUFjLENBQUMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxDQUFDO1NBQzNDO1FBRUQsSUFBSSxDQUFDLGVBQWUsR0FBRyxJQUFJLENBQUMsV0FBVyxDQUFDLFdBQVcsQ0FBQyxDQUFDO0lBQ3ZELENBQUM7SUFFTSxJQUFJO1FBQ1QsSUFBSSxJQUFJLENBQUMsZUFBZTtZQUFFLElBQUksQ0FBQyxjQUFjLENBQUMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxDQUFDO1FBRXBFLElBQUksQ0FBQyxlQUFlLEdBQUcsSUFBSSxDQUFDO0lBQzlCLENBQUM7OytSQWZVLDJCQUEyQixTQUEzQiwyQkFBMkI7aUZBQTNCLDJCQUEyQixXQUEzQiwyQkFBMkIsbUJBRjFCLE1BQU07dUZBRVAsMkJBQTJCO2NBSHZDLFVBQVU7ZUFBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEVtYmVkZGVkVmlld1JlZiwgSW5qZWN0YWJsZSwgVGVtcGxhdGVSZWYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQWJzdHJhY3RUdWlQb3J0YWxTZXJ2aWNlIH0gZnJvbSAnQHRhaWdhLXVpL2Nkayc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBQb3J0YWxTaWRlYmFyQ29udHJvbFNlcnZpY2UgZXh0ZW5kcyBBYnN0cmFjdFR1aVBvcnRhbFNlcnZpY2Uge1xyXG4gIHB1YmxpYyBjdXJyZW50VGVtcGxhdGU6IEVtYmVkZGVkVmlld1JlZjx1bmtub3duPiB8IG51bGwgPSBudWxsO1xyXG5cclxuICBwdWJsaWMgc2hvdyh0ZW1wbGF0ZVJlZjogVGVtcGxhdGVSZWY8dW5rbm93bj4pOiB2b2lkIHtcclxuICAgIGlmICh0aGlzLmN1cnJlbnRUZW1wbGF0ZSkge1xyXG4gICAgICB0aGlzLnJlbW92ZVRlbXBsYXRlKHRoaXMuY3VycmVudFRlbXBsYXRlKTtcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmN1cnJlbnRUZW1wbGF0ZSA9IHRoaXMuYWRkVGVtcGxhdGUodGVtcGxhdGVSZWYpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGhpZGUoKTogdm9pZCB7XHJcbiAgICBpZiAodGhpcy5jdXJyZW50VGVtcGxhdGUpIHRoaXMucmVtb3ZlVGVtcGxhdGUodGhpcy5jdXJyZW50VGVtcGxhdGUpO1xyXG5cclxuICAgIHRoaXMuY3VycmVudFRlbXBsYXRlID0gbnVsbDtcclxuICB9XHJcbn1cclxuIl19