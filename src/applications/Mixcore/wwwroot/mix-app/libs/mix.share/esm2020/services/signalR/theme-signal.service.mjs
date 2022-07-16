import { Injectable } from '@angular/core';
import { BaseSignalService } from '../../bases/base-hub.service';
import { SignalEventType } from '../../interfaces/signal-event-type';
import * as i0 from "@angular/core";
export class ThemeSignalService extends BaseSignalService {
    get _hubName() {
        return 'mixThemeHub';
    }
    get _roomName() {
        return 'Theme';
    }
    _setupSignalREvents() {
        this._hubConnection.on('receive_message', result => {
            if (!result)
                return;
            const data = JSON.parse(result);
            if (data.action && data.action === 'Downloading') {
                this._pushMessage({ type: SignalEventType.THEME_DOWNLOAD, data: Math.round(data.message) });
            }
        });
        this._hubConnection.onclose(() => (this._openConnection = false));
    }
}
ThemeSignalService.ɵfac = /*@__PURE__*/ function () { let ɵThemeSignalService_BaseFactory; return function ThemeSignalService_Factory(t) { return (ɵThemeSignalService_BaseFactory || (ɵThemeSignalService_BaseFactory = i0.ɵɵgetInheritedFactory(ThemeSignalService)))(t || ThemeSignalService); }; }();
ThemeSignalService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: ThemeSignalService, factory: ThemeSignalService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(ThemeSignalService, [{
        type: Injectable,
        args: [{ providedIn: 'root' }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtc2lnbmFsLnNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL3NlcnZpY2VzL3NpZ25hbFIvdGhlbWUtc2lnbmFsLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw4QkFBOEIsQ0FBQztBQUNqRSxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sb0NBQW9DLENBQUM7O0FBR3JFLE1BQU0sT0FBTyxrQkFBbUIsU0FBUSxpQkFBaUI7SUFDdkQsSUFBb0IsUUFBUTtRQUMxQixPQUFPLGFBQWEsQ0FBQztJQUN2QixDQUFDO0lBQ0QsSUFBb0IsU0FBUztRQUMzQixPQUFPLE9BQU8sQ0FBQztJQUNqQixDQUFDO0lBRU0sbUJBQW1CO1FBQ3hCLElBQUksQ0FBQyxjQUFjLENBQUMsRUFBRSxDQUFDLGlCQUFpQixFQUFFLE1BQU0sQ0FBQyxFQUFFO1lBQ2pELElBQUksQ0FBQyxNQUFNO2dCQUFFLE9BQU87WUFFcEIsTUFBTSxJQUFJLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsQ0FBQztZQUNoQyxJQUFJLElBQUksQ0FBQyxNQUFNLElBQUksSUFBSSxDQUFDLE1BQU0sS0FBSyxhQUFhLEVBQUU7Z0JBQ2hELElBQUksQ0FBQyxZQUFZLENBQUMsRUFBRSxJQUFJLEVBQUUsZUFBZSxDQUFDLGNBQWMsRUFBRSxJQUFJLEVBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxDQUFDO2FBQzdGO1FBQ0gsQ0FBQyxDQUFDLENBQUM7UUFFSCxJQUFJLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxHQUFHLEVBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxlQUFlLEdBQUcsS0FBSyxDQUFDLENBQUMsQ0FBQztJQUNwRSxDQUFDOztrUEFuQlUsa0JBQWtCLFNBQWxCLGtCQUFrQjt3RUFBbEIsa0JBQWtCLFdBQWxCLGtCQUFrQixtQkFETCxNQUFNO3VGQUNuQixrQkFBa0I7Y0FEOUIsVUFBVTtlQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbmltcG9ydCB7IEJhc2VTaWduYWxTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vYmFzZXMvYmFzZS1odWIuc2VydmljZSc7XHJcbmltcG9ydCB7IFNpZ25hbEV2ZW50VHlwZSB9IGZyb20gJy4uLy4uL2ludGVyZmFjZXMvc2lnbmFsLWV2ZW50LXR5cGUnO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIFRoZW1lU2lnbmFsU2VydmljZSBleHRlbmRzIEJhc2VTaWduYWxTZXJ2aWNlIHtcclxuICBwdWJsaWMgb3ZlcnJpZGUgZ2V0IF9odWJOYW1lKCkge1xyXG4gICAgcmV0dXJuICdtaXhUaGVtZUh1Yic7XHJcbiAgfVxyXG4gIHB1YmxpYyBvdmVycmlkZSBnZXQgX3Jvb21OYW1lKCkge1xyXG4gICAgcmV0dXJuICdUaGVtZSc7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgX3NldHVwU2lnbmFsUkV2ZW50cygpIHtcclxuICAgIHRoaXMuX2h1YkNvbm5lY3Rpb24ub24oJ3JlY2VpdmVfbWVzc2FnZScsIHJlc3VsdCA9PiB7XHJcbiAgICAgIGlmICghcmVzdWx0KSByZXR1cm47XHJcblxyXG4gICAgICBjb25zdCBkYXRhID0gSlNPTi5wYXJzZShyZXN1bHQpO1xyXG4gICAgICBpZiAoZGF0YS5hY3Rpb24gJiYgZGF0YS5hY3Rpb24gPT09ICdEb3dubG9hZGluZycpIHtcclxuICAgICAgICB0aGlzLl9wdXNoTWVzc2FnZSh7IHR5cGU6IFNpZ25hbEV2ZW50VHlwZS5USEVNRV9ET1dOTE9BRCwgZGF0YTogTWF0aC5yb3VuZChkYXRhLm1lc3NhZ2UpIH0pO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuXHJcbiAgICB0aGlzLl9odWJDb25uZWN0aW9uLm9uY2xvc2UoKCkgPT4gKHRoaXMuX29wZW5Db25uZWN0aW9uID0gZmFsc2UpKTtcclxuICB9XHJcbn1cclxuIl19