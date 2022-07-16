import { Injectable } from '@angular/core';
import { AbstractTuiDialogService, TUI_DIALOGS } from '@taiga-ui/cdk';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { ModalComponent } from './modal.component';
import * as i0 from "@angular/core";
export class ModalService extends AbstractTuiDialogService {
    constructor() {
        super(...arguments);
        this.modalShadowColor = {
            success: '#4ac99b',
            warning: '#ff7043',
            error: '#f45725',
            confirm: '#ff7043',
            info: '#70b6f6' //--tui-info-fill
        };
        this.defaultOptions = {
            heading: 'Confirm?',
            buttons: ['Yes', 'No'],
            borderShadowColor: this.modalShadowColor.confirm
        };
        this.component = new PolymorpheusComponent(ModalComponent);
    }
    show(message, title) {
        const options = { ...this.defaultOptions, heading: title, borderShadowColor: this.modalShadowColor.info };
        return this.open(message, options);
    }
    confirm(message) {
        const options = { ...this.defaultOptions, heading: 'Confirmation', borderShadowColor: this.modalShadowColor.confirm };
        return this.open(message, options);
    }
    info(message) {
        const options = { ...this.defaultOptions, heading: 'Note', borderShadowColor: this.modalShadowColor.info };
        return this.open(message, options);
    }
    success(message) {
        const options = { ...this.defaultOptions, heading: 'Congratulation', borderShadowColor: this.modalShadowColor.success };
        return this.open(message, options);
    }
    error(message) {
        const options = { ...this.defaultOptions, heading: 'Error !', borderShadowColor: this.modalShadowColor.success };
        return this.open(message, options);
    }
    warning(message) {
        const options = { ...this.defaultOptions, heading: 'Warning', borderShadowColor: this.modalShadowColor.warning };
        return this.open(message, options);
    }
}
ModalService.ɵfac = /*@__PURE__*/ function () { let ɵModalService_BaseFactory; return function ModalService_Factory(t) { return (ɵModalService_BaseFactory || (ɵModalService_BaseFactory = i0.ɵɵgetInheritedFactory(ModalService)))(t || ModalService); }; }();
ModalService.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: ModalService, factory: ModalService.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(ModalService, [{
        type: Injectable,
        args: [{
                providedIn: 'root'
            }]
    }], null, null); })();
export const MODAL_PROVIDER = {
    provide: TUI_DIALOGS,
    useExisting: ModalService,
    multi: true
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDckQsT0FBTyxFQUFFLHdCQUF3QixFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0RSxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUdqRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sbUJBQW1CLENBQUM7O0FBS25ELE1BQU0sT0FBTyxZQUFhLFNBQVEsd0JBQXFDO0lBSHZFOztRQUlTLHFCQUFnQixHQUF5RTtZQUM5RixPQUFPLEVBQUUsU0FBUztZQUNsQixPQUFPLEVBQUUsU0FBUztZQUNsQixLQUFLLEVBQUUsU0FBUztZQUNoQixPQUFPLEVBQUUsU0FBUztZQUNsQixJQUFJLEVBQUUsU0FBUyxDQUFDLGlCQUFpQjtTQUNsQyxDQUFDO1FBRWMsbUJBQWMsR0FBZ0I7WUFDNUMsT0FBTyxFQUFFLFVBQVU7WUFDbkIsT0FBTyxFQUFFLENBQUMsS0FBSyxFQUFFLElBQUksQ0FBQztZQUN0QixpQkFBaUIsRUFBRSxJQUFJLENBQUMsZ0JBQWdCLENBQUMsT0FBTztTQUN4QyxDQUFDO1FBRUssY0FBUyxHQUFrRCxJQUFJLHFCQUFxQixDQUFDLGNBQWMsQ0FBQyxDQUFDO0tBK0J0SDtJQTdCUSxJQUFJLENBQUMsT0FBZSxFQUFFLEtBQWE7UUFDeEMsTUFBTSxPQUFPLEdBQWdCLEVBQUUsR0FBRyxJQUFJLENBQUMsY0FBYyxFQUFFLE9BQU8sRUFBRSxLQUFLLEVBQUUsaUJBQWlCLEVBQUUsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksRUFBRSxDQUFDO1FBQ3ZILE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDckMsQ0FBQztJQUVNLE9BQU8sQ0FBQyxPQUFlO1FBQzVCLE1BQU0sT0FBTyxHQUFnQixFQUFFLEdBQUcsSUFBSSxDQUFDLGNBQWMsRUFBRSxPQUFPLEVBQUUsY0FBYyxFQUFFLGlCQUFpQixFQUFFLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLEVBQUUsQ0FBQztRQUNuSSxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3JDLENBQUM7SUFFTSxJQUFJLENBQUMsT0FBZTtRQUN6QixNQUFNLE9BQU8sR0FBZ0IsRUFBRSxHQUFHLElBQUksQ0FBQyxjQUFjLEVBQUUsT0FBTyxFQUFFLE1BQU0sRUFBRSxpQkFBaUIsRUFBRSxJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxFQUFFLENBQUM7UUFDeEgsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNyQyxDQUFDO0lBRU0sT0FBTyxDQUFDLE9BQWU7UUFDNUIsTUFBTSxPQUFPLEdBQWdCLEVBQUUsR0FBRyxJQUFJLENBQUMsY0FBYyxFQUFFLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxpQkFBaUIsRUFBRSxJQUFJLENBQUMsZ0JBQWdCLENBQUMsT0FBTyxFQUFFLENBQUM7UUFDckksT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNyQyxDQUFDO0lBRU0sS0FBSyxDQUFDLE9BQWU7UUFDMUIsTUFBTSxPQUFPLEdBQWdCLEVBQUUsR0FBRyxJQUFJLENBQUMsY0FBYyxFQUFFLE9BQU8sRUFBRSxTQUFTLEVBQUUsaUJBQWlCLEVBQUUsSUFBSSxDQUFDLGdCQUFnQixDQUFDLE9BQU8sRUFBRSxDQUFDO1FBQzlILE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDckMsQ0FBQztJQUVNLE9BQU8sQ0FBQyxPQUFlO1FBQzVCLE1BQU0sT0FBTyxHQUFnQixFQUFFLEdBQUcsSUFBSSxDQUFDLGNBQWMsRUFBRSxPQUFPLEVBQUUsU0FBUyxFQUFFLGlCQUFpQixFQUFFLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLEVBQUUsQ0FBQztRQUM5SCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3JDLENBQUM7O29OQTdDVSxZQUFZLFNBQVosWUFBWTtrRUFBWixZQUFZLFdBQVosWUFBWSxtQkFGWCxNQUFNO3VGQUVQLFlBQVk7Y0FIeEIsVUFBVTtlQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COztBQWlERCxNQUFNLENBQUMsTUFBTSxjQUFjLEdBQWE7SUFDdEMsT0FBTyxFQUFFLFdBQVc7SUFDcEIsV0FBVyxFQUFFLFlBQVk7SUFDekIsS0FBSyxFQUFFLElBQUk7Q0FDWixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgUHJvdmlkZXIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQWJzdHJhY3RUdWlEaWFsb2dTZXJ2aWNlLCBUVUlfRElBTE9HUyB9IGZyb20gJ0B0YWlnYS11aS9jZGsnO1xyXG5pbXBvcnQgeyBQb2x5bW9ycGhldXNDb21wb25lbnQgfSBmcm9tICdAdGlua29mZi9uZy1wb2x5bW9ycGhldXMnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcblxyXG5pbXBvcnQgeyBNb2RhbENvbXBvbmVudCB9IGZyb20gJy4vbW9kYWwuY29tcG9uZW50JztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCdcclxufSlcclxuZXhwb3J0IGNsYXNzIE1vZGFsU2VydmljZSBleHRlbmRzIEFic3RyYWN0VHVpRGlhbG9nU2VydmljZTxNb2RhbE9wdGlvbj4ge1xyXG4gIHB1YmxpYyBtb2RhbFNoYWRvd0NvbG9yOiBSZWNvcmQ8J3N1Y2Nlc3MnIHwgJ2Vycm9yJyB8ICd3YXJuaW5nJyB8ICdjb25maXJtJyB8ICdpbmZvJywgc3RyaW5nPiA9IHtcclxuICAgIHN1Y2Nlc3M6ICcjNGFjOTliJywgLy8tLXR1aS1zdWNjZXNzLWZpbGxcclxuICAgIHdhcm5pbmc6ICcjZmY3MDQzJywgLy8tLXR1aS1hY2NlbnQtZmlsbFxyXG4gICAgZXJyb3I6ICcjZjQ1NzI1JywgLy8tLXR1aS1lcnJvci1maWxsXHJcbiAgICBjb25maXJtOiAnI2ZmNzA0MycsIC8vLS10dWktYWNjZW50LWZpbGwsXHJcbiAgICBpbmZvOiAnIzcwYjZmNicgLy8tLXR1aS1pbmZvLWZpbGxcclxuICB9O1xyXG5cclxuICBwdWJsaWMgcmVhZG9ubHkgZGVmYXVsdE9wdGlvbnM6IE1vZGFsT3B0aW9uID0ge1xyXG4gICAgaGVhZGluZzogJ0NvbmZpcm0/JyxcclxuICAgIGJ1dHRvbnM6IFsnWWVzJywgJ05vJ10sXHJcbiAgICBib3JkZXJTaGFkb3dDb2xvcjogdGhpcy5tb2RhbFNoYWRvd0NvbG9yLmNvbmZpcm1cclxuICB9IGFzIGNvbnN0O1xyXG5cclxuICBwdWJsaWMgcmVhZG9ubHkgY29tcG9uZW50OiBQb2x5bW9ycGhldXNDb21wb25lbnQ8TW9kYWxDb21wb25lbnQsIG9iamVjdD4gPSBuZXcgUG9seW1vcnBoZXVzQ29tcG9uZW50KE1vZGFsQ29tcG9uZW50KTtcclxuXHJcbiAgcHVibGljIHNob3cobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nKTogT2JzZXJ2YWJsZTxib29sZWFuPiB7XHJcbiAgICBjb25zdCBvcHRpb25zOiBNb2RhbE9wdGlvbiA9IHsgLi4udGhpcy5kZWZhdWx0T3B0aW9ucywgaGVhZGluZzogdGl0bGUsIGJvcmRlclNoYWRvd0NvbG9yOiB0aGlzLm1vZGFsU2hhZG93Q29sb3IuaW5mbyB9O1xyXG4gICAgcmV0dXJuIHRoaXMub3BlbihtZXNzYWdlLCBvcHRpb25zKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBjb25maXJtKG1lc3NhZ2U6IHN0cmluZyk6IE9ic2VydmFibGU8Ym9vbGVhbj4ge1xyXG4gICAgY29uc3Qgb3B0aW9uczogTW9kYWxPcHRpb24gPSB7IC4uLnRoaXMuZGVmYXVsdE9wdGlvbnMsIGhlYWRpbmc6ICdDb25maXJtYXRpb24nLCBib3JkZXJTaGFkb3dDb2xvcjogdGhpcy5tb2RhbFNoYWRvd0NvbG9yLmNvbmZpcm0gfTtcclxuICAgIHJldHVybiB0aGlzLm9wZW4obWVzc2FnZSwgb3B0aW9ucyk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgaW5mbyhtZXNzYWdlOiBzdHJpbmcpOiBPYnNlcnZhYmxlPGJvb2xlYW4+IHtcclxuICAgIGNvbnN0IG9wdGlvbnM6IE1vZGFsT3B0aW9uID0geyAuLi50aGlzLmRlZmF1bHRPcHRpb25zLCBoZWFkaW5nOiAnTm90ZScsIGJvcmRlclNoYWRvd0NvbG9yOiB0aGlzLm1vZGFsU2hhZG93Q29sb3IuaW5mbyB9O1xyXG4gICAgcmV0dXJuIHRoaXMub3BlbihtZXNzYWdlLCBvcHRpb25zKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBzdWNjZXNzKG1lc3NhZ2U6IHN0cmluZyk6IE9ic2VydmFibGU8dm9pZD4ge1xyXG4gICAgY29uc3Qgb3B0aW9uczogTW9kYWxPcHRpb24gPSB7IC4uLnRoaXMuZGVmYXVsdE9wdGlvbnMsIGhlYWRpbmc6ICdDb25ncmF0dWxhdGlvbicsIGJvcmRlclNoYWRvd0NvbG9yOiB0aGlzLm1vZGFsU2hhZG93Q29sb3Iuc3VjY2VzcyB9O1xyXG4gICAgcmV0dXJuIHRoaXMub3BlbihtZXNzYWdlLCBvcHRpb25zKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBlcnJvcihtZXNzYWdlOiBzdHJpbmcpOiBPYnNlcnZhYmxlPHZvaWQ+IHtcclxuICAgIGNvbnN0IG9wdGlvbnM6IE1vZGFsT3B0aW9uID0geyAuLi50aGlzLmRlZmF1bHRPcHRpb25zLCBoZWFkaW5nOiAnRXJyb3IgIScsIGJvcmRlclNoYWRvd0NvbG9yOiB0aGlzLm1vZGFsU2hhZG93Q29sb3Iuc3VjY2VzcyB9O1xyXG4gICAgcmV0dXJuIHRoaXMub3BlbihtZXNzYWdlLCBvcHRpb25zKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyB3YXJuaW5nKG1lc3NhZ2U6IHN0cmluZyk6IE9ic2VydmFibGU8dm9pZD4ge1xyXG4gICAgY29uc3Qgb3B0aW9uczogTW9kYWxPcHRpb24gPSB7IC4uLnRoaXMuZGVmYXVsdE9wdGlvbnMsIGhlYWRpbmc6ICdXYXJuaW5nJywgYm9yZGVyU2hhZG93Q29sb3I6IHRoaXMubW9kYWxTaGFkb3dDb2xvci53YXJuaW5nIH07XHJcbiAgICByZXR1cm4gdGhpcy5vcGVuKG1lc3NhZ2UsIG9wdGlvbnMpO1xyXG4gIH1cclxufVxyXG5cclxuZXhwb3J0IGNvbnN0IE1PREFMX1BST1ZJREVSOiBQcm92aWRlciA9IHtcclxuICBwcm92aWRlOiBUVUlfRElBTE9HUyxcclxuICB1c2VFeGlzdGluZzogTW9kYWxTZXJ2aWNlLFxyXG4gIG11bHRpOiB0cnVlXHJcbn07XHJcblxyXG5leHBvcnQgaW50ZXJmYWNlIE1vZGFsT3B0aW9uIHtcclxuICByZWFkb25seSBoZWFkaW5nOiBzdHJpbmc7XHJcbiAgcmVhZG9ubHkgYnV0dG9uczogcmVhZG9ubHkgW3N0cmluZywgc3RyaW5nXTtcclxuICByZWFkb25seSBib3JkZXJTaGFkb3dDb2xvcj86IHN0cmluZztcclxufVxyXG4iXX0=