import { Inject, Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { filter, Subject } from 'rxjs';
import { DOMAIN_URL } from '../token';
import * as i0 from "@angular/core";
export class BaseSignalService {
    constructor(domain) {
        this.domain = domain;
        this._signalEvent = new Subject();
        this._openConnection = false;
        this._isInitializing = false;
    }
    get _hubName() {
        return '';
    }
    get _roomName() {
        return '';
    }
    getMessage(...filterValues) {
        this._ensureConnection();
        return this._signalEvent.asObservable().pipe(filter(event => filterValues.some(f => f === event.type)));
    }
    async _initializeSignalR() {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl(this.domain + 'hub/' + this._hubName)
            .withAutomaticReconnect()
            .build();
        try {
            await this._hubConnection.start();
            if (this._roomName)
                await this._hubConnection.invoke('JoinRoom', this._roomName);
            this._openConnection = true;
            this._isInitializing = false;
            this._setupSignalREvents();
        }
        catch (err) {
            this._hubConnection.stop().then(_ => {
                this._openConnection = false;
            });
        }
    }
    async _ensureConnection() {
        if (this._openConnection || this._isInitializing)
            return;
        await this._initializeSignalR();
    }
    _pushMessage(payload) {
        this._signalEvent.next(payload);
    }
}
BaseSignalService.ɵfac = function BaseSignalService_Factory(t) { return new (t || BaseSignalService)(i0.ɵɵinject(DOMAIN_URL)); };
BaseSignalService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: BaseSignalService, factory: BaseSignalService.ɵfac });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(BaseSignalService, [{
        type: Injectable
    }], function () { return [{ type: undefined, decorators: [{
                type: Inject,
                args: [DOMAIN_URL]
            }] }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYmFzZS1odWIuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvYmFzZXMvYmFzZS1odWIuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQWlCLG9CQUFvQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDekUsT0FBTyxFQUFFLE1BQU0sRUFBYyxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFJbkQsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLFVBQVUsQ0FBQzs7QUFHdEMsTUFBTSxPQUFnQixpQkFBaUI7SUFjckMsWUFBdUMsTUFBYztRQUFkLFdBQU0sR0FBTixNQUFNLENBQVE7UUFiOUMsaUJBQVksR0FBOEIsSUFBSSxPQUFPLEVBQU8sQ0FBQztRQUM3RCxvQkFBZSxHQUFHLEtBQUssQ0FBQztRQUN4QixvQkFBZSxHQUFHLEtBQUssQ0FBQztJQVd5QixDQUFDO0lBVHpELElBQWMsUUFBUTtRQUNwQixPQUFPLEVBQUUsQ0FBQztJQUNaLENBQUM7SUFDRCxJQUFjLFNBQVM7UUFDckIsT0FBTyxFQUFFLENBQUM7SUFDWixDQUFDO0lBTU0sVUFBVSxDQUFJLEdBQUcsWUFBK0I7UUFDckQsSUFBSSxDQUFDLGlCQUFpQixFQUFFLENBQUM7UUFDekIsT0FBTyxJQUFJLENBQUMsWUFBWSxDQUFDLFlBQVksRUFBRSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxZQUFZLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxLQUFLLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDMUcsQ0FBQztJQUVTLEtBQUssQ0FBQyxrQkFBa0I7UUFDaEMsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLG9CQUFvQixFQUFFO2FBQzdDLE9BQU8sQ0FBQyxJQUFJLENBQUMsTUFBTSxHQUFHLE1BQU0sR0FBRyxJQUFJLENBQUMsUUFBUSxDQUFDO2FBQzdDLHNCQUFzQixFQUFFO2FBQ3hCLEtBQUssRUFBRSxDQUFDO1FBRVgsSUFBSTtZQUNGLE1BQU0sSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLEVBQUUsQ0FBQztZQUNsQyxJQUFJLElBQUksQ0FBQyxTQUFTO2dCQUFFLE1BQU0sSUFBSSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsVUFBVSxFQUFFLElBQUksQ0FBQyxTQUFTLENBQUMsQ0FBQztZQUVqRixJQUFJLENBQUMsZUFBZSxHQUFHLElBQUksQ0FBQztZQUM1QixJQUFJLENBQUMsZUFBZSxHQUFHLEtBQUssQ0FBQztZQUM3QixJQUFJLENBQUMsbUJBQW1CLEVBQUUsQ0FBQztTQUM1QjtRQUFDLE9BQU8sR0FBRyxFQUFFO1lBQ1osSUFBSSxDQUFDLGNBQWMsQ0FBQyxJQUFJLEVBQUUsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQUU7Z0JBQ2xDLElBQUksQ0FBQyxlQUFlLEdBQUcsS0FBSyxDQUFDO1lBQy9CLENBQUMsQ0FBQyxDQUFDO1NBQ0o7SUFDSCxDQUFDO0lBRVMsS0FBSyxDQUFDLGlCQUFpQjtRQUMvQixJQUFJLElBQUksQ0FBQyxlQUFlLElBQUksSUFBSSxDQUFDLGVBQWU7WUFBRSxPQUFPO1FBQ3pELE1BQU0sSUFBSSxDQUFDLGtCQUFrQixFQUFFLENBQUM7SUFDbEMsQ0FBQztJQUVTLFlBQVksQ0FBSSxPQUF1QjtRQUMvQyxJQUFJLENBQUMsWUFBWSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNsQyxDQUFDOztrRkFoRG1CLGlCQUFpQixjQWNqQixVQUFVO3VFQWRWLGlCQUFpQixXQUFqQixpQkFBaUI7dUZBQWpCLGlCQUFpQjtjQUR0QyxVQUFVOztzQkFlSSxNQUFNO3VCQUFDLFVBQVUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3QsIEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgSHViQ29ubmVjdGlvbiwgSHViQ29ubmVjdGlvbkJ1aWxkZXIgfSBmcm9tICdAbWljcm9zb2Z0L3NpZ25hbHInO1xyXG5pbXBvcnQgeyBmaWx0ZXIsIE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuXHJcbmltcG9ydCB7IFNpZ25hbEV2ZW50IH0gZnJvbSAnLi4vaW50ZXJmYWNlcy9zaWduYWwtZXZlbnQnO1xyXG5pbXBvcnQgeyBTaWduYWxFdmVudFR5cGUgfSBmcm9tICcuLi9pbnRlcmZhY2VzL3NpZ25hbC1ldmVudC10eXBlJztcclxuaW1wb3J0IHsgRE9NQUlOX1VSTCB9IGZyb20gJy4uL3Rva2VuJztcclxuXHJcbkBJbmplY3RhYmxlKClcclxuZXhwb3J0IGFic3RyYWN0IGNsYXNzIEJhc2VTaWduYWxTZXJ2aWNlIHtcclxuICBwdWJsaWMgX3NpZ25hbEV2ZW50OiBTdWJqZWN0PFNpZ25hbEV2ZW50PGFueT4+ID0gbmV3IFN1YmplY3Q8YW55PigpO1xyXG4gIHB1YmxpYyBfb3BlbkNvbm5lY3Rpb24gPSBmYWxzZTtcclxuICBwdWJsaWMgX2lzSW5pdGlhbGl6aW5nID0gZmFsc2U7XHJcbiAgcHVibGljIF9odWJDb25uZWN0aW9uITogSHViQ29ubmVjdGlvbjtcclxuICBwcm90ZWN0ZWQgZ2V0IF9odWJOYW1lKCkge1xyXG4gICAgcmV0dXJuICcnO1xyXG4gIH1cclxuICBwcm90ZWN0ZWQgZ2V0IF9yb29tTmFtZSgpIHtcclxuICAgIHJldHVybiAnJztcclxuICB9XHJcblxyXG4gIGFic3RyYWN0IF9zZXR1cFNpZ25hbFJFdmVudHMoKTogdm9pZDtcclxuXHJcbiAgY29uc3RydWN0b3IoQEluamVjdChET01BSU5fVVJMKSBwdWJsaWMgZG9tYWluOiBzdHJpbmcpIHt9XHJcblxyXG4gIHB1YmxpYyBnZXRNZXNzYWdlPFQ+KC4uLmZpbHRlclZhbHVlczogU2lnbmFsRXZlbnRUeXBlW10pOiBPYnNlcnZhYmxlPFNpZ25hbEV2ZW50PFQ+PiB7XHJcbiAgICB0aGlzLl9lbnN1cmVDb25uZWN0aW9uKCk7XHJcbiAgICByZXR1cm4gdGhpcy5fc2lnbmFsRXZlbnQuYXNPYnNlcnZhYmxlKCkucGlwZShmaWx0ZXIoZXZlbnQgPT4gZmlsdGVyVmFsdWVzLnNvbWUoZiA9PiBmID09PSBldmVudC50eXBlKSkpO1xyXG4gIH1cclxuXHJcbiAgcHJvdGVjdGVkIGFzeW5jIF9pbml0aWFsaXplU2lnbmFsUigpIHtcclxuICAgIHRoaXMuX2h1YkNvbm5lY3Rpb24gPSBuZXcgSHViQ29ubmVjdGlvbkJ1aWxkZXIoKVxyXG4gICAgICAud2l0aFVybCh0aGlzLmRvbWFpbiArICdodWIvJyArIHRoaXMuX2h1Yk5hbWUpXHJcbiAgICAgIC53aXRoQXV0b21hdGljUmVjb25uZWN0KClcclxuICAgICAgLmJ1aWxkKCk7XHJcblxyXG4gICAgdHJ5IHtcclxuICAgICAgYXdhaXQgdGhpcy5faHViQ29ubmVjdGlvbi5zdGFydCgpO1xyXG4gICAgICBpZiAodGhpcy5fcm9vbU5hbWUpIGF3YWl0IHRoaXMuX2h1YkNvbm5lY3Rpb24uaW52b2tlKCdKb2luUm9vbScsIHRoaXMuX3Jvb21OYW1lKTtcclxuXHJcbiAgICAgIHRoaXMuX29wZW5Db25uZWN0aW9uID0gdHJ1ZTtcclxuICAgICAgdGhpcy5faXNJbml0aWFsaXppbmcgPSBmYWxzZTtcclxuICAgICAgdGhpcy5fc2V0dXBTaWduYWxSRXZlbnRzKCk7XHJcbiAgICB9IGNhdGNoIChlcnIpIHtcclxuICAgICAgdGhpcy5faHViQ29ubmVjdGlvbi5zdG9wKCkudGhlbihfID0+IHtcclxuICAgICAgICB0aGlzLl9vcGVuQ29ubmVjdGlvbiA9IGZhbHNlO1xyXG4gICAgICB9KTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIHByb3RlY3RlZCBhc3luYyBfZW5zdXJlQ29ubmVjdGlvbigpIHtcclxuICAgIGlmICh0aGlzLl9vcGVuQ29ubmVjdGlvbiB8fCB0aGlzLl9pc0luaXRpYWxpemluZykgcmV0dXJuO1xyXG4gICAgYXdhaXQgdGhpcy5faW5pdGlhbGl6ZVNpZ25hbFIoKTtcclxuICB9XHJcblxyXG4gIHByb3RlY3RlZCBfcHVzaE1lc3NhZ2U8VD4ocGF5bG9hZDogU2lnbmFsRXZlbnQ8VD4pIHtcclxuICAgIHRoaXMuX3NpZ25hbEV2ZW50Lm5leHQocGF5bG9hZCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==