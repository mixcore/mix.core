import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject, filter } from 'rxjs';
import * as i0 from "@angular/core";
import * as i1 from "@angular/router";
export class TabControlService {
    constructor(router, activatedRoute) {
        this.router = router;
        this.activatedRoute = activatedRoute;
        this.navControl = [];
        this.navControl$ = new BehaviorSubject([]);
        this.index$ = new BehaviorSubject(0);
        this.whiteLists = ['/auth/login'];
    }
    init() {
        this.router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(() => {
            if (this.whiteLists.includes(this.router.url))
                return;
            this.navControl = this.navControl.filter(c => c.path !== this.router.url);
            this.navControl.unshift({
                title: this.activatedRoute.snapshot.data['title'],
                path: this.router.url
            });
            this.navControl$.next(this.navControl);
        });
    }
    nextTab() {
        let currentIndex = this.index$.getValue();
        if (currentIndex >= this.navControl.length - 1) {
            currentIndex = 0;
        }
        else {
            currentIndex = currentIndex + 1;
        }
        this.index$.next(currentIndex);
    }
    unTab() {
        this.index$.next(0);
    }
}
TabControlService.ɵfac = function TabControlService_Factory(t) { return new (t || TabControlService)(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i1.ActivatedRoute)); };
TabControlService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: TabControlService, factory: TabControlService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(TabControlService, [{
        type: Injectable,
        args: [{ providedIn: 'root' }]
    }], function () { return [{ type: i1.Router }, { type: i1.ActivatedRoute }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFiLWNvbnRyb2wuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvc2VydmljZXMvaGVscGVyL3RhYi1jb250cm9sLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsY0FBYyxFQUFFLGFBQWEsRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN4RSxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sRUFBRSxNQUFNLE1BQU0sQ0FBQzs7O0FBUS9DLE1BQU0sT0FBTyxpQkFBaUI7SUFNNUIsWUFBb0IsTUFBYyxFQUFVLGNBQThCO1FBQXRELFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFMbkUsZUFBVSxHQUFpQixFQUFFLENBQUM7UUFDOUIsZ0JBQVcsR0FBa0MsSUFBSSxlQUFlLENBQWUsRUFBRSxDQUFDLENBQUM7UUFDbkYsV0FBTSxHQUE0QixJQUFJLGVBQWUsQ0FBUyxDQUFDLENBQUMsQ0FBQztRQUNqRSxlQUFVLEdBQWEsQ0FBQyxhQUFhLENBQUMsQ0FBQztJQUUrQixDQUFDO0lBRXZFLElBQUk7UUFDVCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxZQUFZLGFBQWEsQ0FBQyxDQUFDLENBQUMsU0FBUyxDQUFDLEdBQUcsRUFBRTtZQUM5RSxJQUFJLElBQUksQ0FBQyxVQUFVLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDO2dCQUFFLE9BQU87WUFFdEQsSUFBSSxDQUFDLFVBQVUsR0FBRyxJQUFJLENBQUMsVUFBVSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFJLEtBQUssSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQztZQUMxRSxJQUFJLENBQUMsVUFBVSxDQUFDLE9BQU8sQ0FBQztnQkFDdEIsS0FBSyxFQUFFLElBQUksQ0FBQyxjQUFjLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUM7Z0JBQ2pELElBQUksRUFBRSxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUc7YUFDdEIsQ0FBQyxDQUFDO1lBQ0gsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFDO1FBQ3pDLENBQUMsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUVNLE9BQU87UUFDWixJQUFJLFlBQVksR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDLFFBQVEsRUFBRSxDQUFDO1FBQzFDLElBQUksWUFBWSxJQUFJLElBQUksQ0FBQyxVQUFVLENBQUMsTUFBTSxHQUFHLENBQUMsRUFBRTtZQUM5QyxZQUFZLEdBQUcsQ0FBQyxDQUFDO1NBQ2xCO2FBQU07WUFDTCxZQUFZLEdBQUcsWUFBWSxHQUFHLENBQUMsQ0FBQztTQUNqQztRQUVELElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDLFlBQVksQ0FBQyxDQUFDO0lBQ2pDLENBQUM7SUFFTSxLQUFLO1FBQ1YsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDdEIsQ0FBQzs7a0ZBbENVLGlCQUFpQjt1RUFBakIsaUJBQWlCLFdBQWpCLGlCQUFpQixtQkFESixNQUFNO3VGQUNuQixpQkFBaUI7Y0FEN0IsVUFBVTtlQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQWN0aXZhdGVkUm91dGUsIE5hdmlnYXRpb25FbmQsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IEJlaGF2aW9yU3ViamVjdCwgZmlsdGVyIH0gZnJvbSAncnhqcyc7XHJcblxyXG5leHBvcnQgaW50ZXJmYWNlIFRhYkNvbnRyb2wge1xyXG4gIHBhdGg6IHN0cmluZztcclxuICB0aXRsZTogc3RyaW5nO1xyXG59XHJcblxyXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxyXG5leHBvcnQgY2xhc3MgVGFiQ29udHJvbFNlcnZpY2Uge1xyXG4gIHB1YmxpYyBuYXZDb250cm9sOiBUYWJDb250cm9sW10gPSBbXTtcclxuICBwdWJsaWMgbmF2Q29udHJvbCQ6IEJlaGF2aW9yU3ViamVjdDxUYWJDb250cm9sW10+ID0gbmV3IEJlaGF2aW9yU3ViamVjdDxUYWJDb250cm9sW10+KFtdKTtcclxuICBwdWJsaWMgaW5kZXgkOiBCZWhhdmlvclN1YmplY3Q8bnVtYmVyPiA9IG5ldyBCZWhhdmlvclN1YmplY3Q8bnVtYmVyPigwKTtcclxuICBwdWJsaWMgd2hpdGVMaXN0czogc3RyaW5nW10gPSBbJy9hdXRoL2xvZ2luJ107XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgYWN0aXZhdGVkUm91dGU6IEFjdGl2YXRlZFJvdXRlKSB7fVxyXG5cclxuICBwdWJsaWMgaW5pdCgpOiB2b2lkIHtcclxuICAgIHRoaXMucm91dGVyLmV2ZW50cy5waXBlKGZpbHRlcihlID0+IGUgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRW5kKSkuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgaWYgKHRoaXMud2hpdGVMaXN0cy5pbmNsdWRlcyh0aGlzLnJvdXRlci51cmwpKSByZXR1cm47XHJcblxyXG4gICAgICB0aGlzLm5hdkNvbnRyb2wgPSB0aGlzLm5hdkNvbnRyb2wuZmlsdGVyKGMgPT4gYy5wYXRoICE9PSB0aGlzLnJvdXRlci51cmwpO1xyXG4gICAgICB0aGlzLm5hdkNvbnRyb2wudW5zaGlmdCh7XHJcbiAgICAgICAgdGl0bGU6IHRoaXMuYWN0aXZhdGVkUm91dGUuc25hcHNob3QuZGF0YVsndGl0bGUnXSxcclxuICAgICAgICBwYXRoOiB0aGlzLnJvdXRlci51cmxcclxuICAgICAgfSk7XHJcbiAgICAgIHRoaXMubmF2Q29udHJvbCQubmV4dCh0aGlzLm5hdkNvbnRyb2wpO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgbmV4dFRhYigpOiB2b2lkIHtcclxuICAgIGxldCBjdXJyZW50SW5kZXggPSB0aGlzLmluZGV4JC5nZXRWYWx1ZSgpO1xyXG4gICAgaWYgKGN1cnJlbnRJbmRleCA+PSB0aGlzLm5hdkNvbnRyb2wubGVuZ3RoIC0gMSkge1xyXG4gICAgICBjdXJyZW50SW5kZXggPSAwO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgY3VycmVudEluZGV4ID0gY3VycmVudEluZGV4ICsgMTtcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmluZGV4JC5uZXh0KGN1cnJlbnRJbmRleCk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgdW5UYWIoKTogdm9pZCB7XHJcbiAgICB0aGlzLmluZGV4JC5uZXh0KDApO1xyXG4gIH1cclxufVxyXG4iXX0=