import { Injectable } from '@angular/core';
import { MixApiDict } from '@mix-spa/mix.lib';
import { map } from 'rxjs/operators';
import { BaseApiService } from '../../bases/base-api.service';
import * as i0 from "@angular/core";
export class ShareApiService extends BaseApiService {
    getCultures() {
        return this.get(MixApiDict.ShareApi.getCulturesEndpoint).pipe(map(res => res.items));
    }
    getGlobalSetting() {
        return this.get(MixApiDict.ShareApi.getGlobalSettingsEndpoint);
    }
}
ShareApiService.ɵfac = /*@__PURE__*/ function () { let ɵShareApiService_BaseFactory; return function ShareApiService_Factory(t) { return (ɵShareApiService_BaseFactory || (ɵShareApiService_BaseFactory = i0.ɵɵgetInheritedFactory(ShareApiService)))(t || ShareApiService); }; }();
ShareApiService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: ShareApiService, factory: ShareApiService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(ShareApiService, [{
        type: Injectable,
        args: [{ providedIn: 'root' }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2hhcmUtYXBpLnNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL3NlcnZpY2VzL2FwaS9zaGFyZS1hcGkuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBaUQsVUFBVSxFQUFFLE1BQU0sa0JBQWtCLENBQUM7QUFFN0YsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRXJDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSw4QkFBOEIsQ0FBQzs7QUFHOUQsTUFBTSxPQUFPLGVBQWdCLFNBQVEsY0FBYztJQUMxQyxXQUFXO1FBQ2hCLE9BQU8sSUFBSSxDQUFDLEdBQUcsQ0FBdUIsVUFBVSxDQUFDLFFBQVEsQ0FBQyxtQkFBbUIsQ0FBQyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQztJQUM3RyxDQUFDO0lBRU0sZ0JBQWdCO1FBQ3JCLE9BQU8sSUFBSSxDQUFDLEdBQUcsQ0FBaUIsVUFBVSxDQUFDLFFBQVEsQ0FBQyx5QkFBeUIsQ0FBQyxDQUFDO0lBQ2pGLENBQUM7O21PQVBVLGVBQWUsU0FBZixlQUFlO3FFQUFmLGVBQWUsV0FBZixlQUFlLG1CQURGLE1BQU07dUZBQ25CLGVBQWU7Y0FEM0IsVUFBVTtlQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQ3VsdHVyZSwgR2xvYmFsU2V0dGluZ3MsIElHZXRBbGxDdWx0dXJlUmVzdWx0LCBNaXhBcGlEaWN0IH0gZnJvbSAnQG1peC1zcGEvbWl4LmxpYic7XHJcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgbWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5cclxuaW1wb3J0IHsgQmFzZUFwaVNlcnZpY2UgfSBmcm9tICcuLi8uLi9iYXNlcy9iYXNlLWFwaS5zZXJ2aWNlJztcclxuXHJcbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXHJcbmV4cG9ydCBjbGFzcyBTaGFyZUFwaVNlcnZpY2UgZXh0ZW5kcyBCYXNlQXBpU2VydmljZSB7XHJcbiAgcHVibGljIGdldEN1bHR1cmVzKCk6IE9ic2VydmFibGU8Q3VsdHVyZVtdPiB7XHJcbiAgICByZXR1cm4gdGhpcy5nZXQ8SUdldEFsbEN1bHR1cmVSZXN1bHQ+KE1peEFwaURpY3QuU2hhcmVBcGkuZ2V0Q3VsdHVyZXNFbmRwb2ludCkucGlwZShtYXAocmVzID0+IHJlcy5pdGVtcykpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGdldEdsb2JhbFNldHRpbmcoKTogT2JzZXJ2YWJsZTxHbG9iYWxTZXR0aW5ncz4ge1xyXG4gICAgcmV0dXJuIHRoaXMuZ2V0PEdsb2JhbFNldHRpbmdzPihNaXhBcGlEaWN0LlNoYXJlQXBpLmdldEdsb2JhbFNldHRpbmdzRW5kcG9pbnQpO1xyXG4gIH1cclxufVxyXG4iXX0=