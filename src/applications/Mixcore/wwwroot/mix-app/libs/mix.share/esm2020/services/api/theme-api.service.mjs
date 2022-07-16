import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { MixApiDict } from '@mix-spa/mix.lib';
import { BaseApiService } from '../../bases/base-api.service';
import { BASE_URL, GET_THEME_URL } from '../../token';
import { AppEventService } from '../helper/app-event.service';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common/http";
import * as i2 from "../helper/app-event.service";
export class ThemeApiService extends BaseApiService {
    constructor(http, baseUrl, getThemeStoreUrl, appEvent) {
        super(http, baseUrl, appEvent);
        this.http = http;
        this.baseUrl = baseUrl;
        this.getThemeStoreUrl = getThemeStoreUrl;
        this.appEvent = appEvent;
    }
    getThemeStore() {
        return this.http.get(this.getThemeStoreUrl);
    }
    getThemes(request) {
        return this.get(MixApiDict.ThemeApi.getThemeEndpoint, request);
    }
}
ThemeApiService.ɵfac = function ThemeApiService_Factory(t) { return new (t || ThemeApiService)(i0.ɵɵinject(i1.HttpClient), i0.ɵɵinject(BASE_URL), i0.ɵɵinject(GET_THEME_URL), i0.ɵɵinject(i2.AppEventService)); };
ThemeApiService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: ThemeApiService, factory: ThemeApiService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(ThemeApiService, [{
        type: Injectable,
        args: [{ providedIn: 'root' }]
    }], function () { return [{ type: i1.HttpClient }, { type: undefined, decorators: [{
                type: Inject,
                args: [BASE_URL]
            }] }, { type: undefined, decorators: [{
                type: Inject,
                args: [GET_THEME_URL]
            }] }, { type: i2.AppEventService }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYXBpLnNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL3NlcnZpY2VzL2FwaS90aGVtZS1hcGkuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDbEQsT0FBTyxFQUFFLE1BQU0sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUVMLFVBQVUsRUFJWCxNQUFNLGtCQUFrQixDQUFDO0FBRzFCLE9BQU8sRUFBRSxjQUFjLEVBQW9CLE1BQU0sOEJBQThCLENBQUM7QUFDaEYsT0FBTyxFQUFFLFFBQVEsRUFBRSxhQUFhLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDdEQsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLDZCQUE2QixDQUFDOzs7O0FBRzlELE1BQU0sT0FBTyxlQUFnQixTQUFRLGNBQWM7SUFDakQsWUFDOEIsSUFBZ0IsRUFDVixPQUFlLEVBQ25CLGdCQUF3QixFQUN0QyxRQUF5QjtRQUV6QyxLQUFLLENBQUMsSUFBSSxFQUFFLE9BQU8sRUFBRSxRQUFRLENBQUMsQ0FBQztRQUxILFNBQUksR0FBSixJQUFJLENBQVk7UUFDVixZQUFPLEdBQVAsT0FBTyxDQUFRO1FBQ25CLHFCQUFnQixHQUFoQixnQkFBZ0IsQ0FBUTtRQUN0QyxhQUFRLEdBQVIsUUFBUSxDQUFpQjtJQUczQyxDQUFDO0lBRU0sYUFBYTtRQUNsQixPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFnQyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsQ0FBQztJQUM3RSxDQUFDO0lBRU0sU0FBUyxDQUNkLE9BQStCO1FBRS9CLE9BQU8sSUFBSSxDQUFDLEdBQUcsQ0FDYixVQUFVLENBQUMsUUFBUSxDQUFDLGdCQUFnQixFQUNsQixPQUFPLENBQzFCLENBQUM7SUFDSixDQUFDOzs4RUFyQlUsZUFBZSwwQ0FHaEIsUUFBUSxlQUNSLGFBQWE7cUVBSlosZUFBZSxXQUFmLGVBQWUsbUJBREYsTUFBTTt1RkFDbkIsZUFBZTtjQUQzQixVQUFVO2VBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOztzQkFJN0IsTUFBTTt1QkFBQyxRQUFROztzQkFDZixNQUFNO3VCQUFDLGFBQWEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBIdHRwQ2xpZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xyXG5pbXBvcnQgeyBJbmplY3QsIEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHtcclxuICBJUGFnaW5hdGlvblJlc3VsdCxcclxuICBNaXhBcGlEaWN0LFxyXG4gIFBhZ2luYXRpb25SZXF1ZXN0TW9kZWwsXHJcbiAgUGFnaW5hdGlvblJlc3VsdE1vZGVsLFxyXG4gIFRoZW1lTW9kZWxcclxufSBmcm9tICdAbWl4LXNwYS9taXgubGliJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5cclxuaW1wb3J0IHsgQmFzZUFwaVNlcnZpY2UsIElIdHRwUGFyYW1PYmplY3QgfSBmcm9tICcuLi8uLi9iYXNlcy9iYXNlLWFwaS5zZXJ2aWNlJztcclxuaW1wb3J0IHsgQkFTRV9VUkwsIEdFVF9USEVNRV9VUkwgfSBmcm9tICcuLi8uLi90b2tlbic7XHJcbmltcG9ydCB7IEFwcEV2ZW50U2VydmljZSB9IGZyb20gJy4uL2hlbHBlci9hcHAtZXZlbnQuc2VydmljZSc7XHJcblxyXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxyXG5leHBvcnQgY2xhc3MgVGhlbWVBcGlTZXJ2aWNlIGV4dGVuZHMgQmFzZUFwaVNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHJvdGVjdGVkIG92ZXJyaWRlIHJlYWRvbmx5IGh0dHA6IEh0dHBDbGllbnQsXHJcbiAgICBASW5qZWN0KEJBU0VfVVJMKSBwdWJsaWMgb3ZlcnJpZGUgYmFzZVVybDogc3RyaW5nLFxyXG4gICAgQEluamVjdChHRVRfVEhFTUVfVVJMKSBwdWJsaWMgZ2V0VGhlbWVTdG9yZVVybDogc3RyaW5nLFxyXG4gICAgcHVibGljIG92ZXJyaWRlIGFwcEV2ZW50OiBBcHBFdmVudFNlcnZpY2VcclxuICApIHtcclxuICAgIHN1cGVyKGh0dHAsIGJhc2VVcmwsIGFwcEV2ZW50KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBnZXRUaGVtZVN0b3JlKCk6IE9ic2VydmFibGU8SVBhZ2luYXRpb25SZXN1bHQ8VGhlbWVNb2RlbD4+IHtcclxuICAgIHJldHVybiB0aGlzLmh0dHAuZ2V0PElQYWdpbmF0aW9uUmVzdWx0PFRoZW1lTW9kZWw+Pih0aGlzLmdldFRoZW1lU3RvcmVVcmwpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGdldFRoZW1lcyhcclxuICAgIHJlcXVlc3Q6IFBhZ2luYXRpb25SZXF1ZXN0TW9kZWxcclxuICApOiBPYnNlcnZhYmxlPFBhZ2luYXRpb25SZXN1bHRNb2RlbDxUaGVtZU1vZGVsPj4ge1xyXG4gICAgcmV0dXJuIHRoaXMuZ2V0PFBhZ2luYXRpb25SZXN1bHRNb2RlbDxUaGVtZU1vZGVsPj4oXHJcbiAgICAgIE1peEFwaURpY3QuVGhlbWVBcGkuZ2V0VGhlbWVFbmRwb2ludCxcclxuICAgICAgPElIdHRwUGFyYW1PYmplY3Q+cmVxdWVzdFxyXG4gICAgKTtcclxuICB9XHJcbn1cclxuIl19