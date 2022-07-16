import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MixUser } from '@mix-spa/mix.lib';
import { tuiButtonOptionsProvider } from '@taiga-ui/core';
import { ShareModule } from '../../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "@taiga-ui/core";
import * as i2 from "@taiga-ui/kit";
const _c0 = ["user", ""];
export class MixMessengerCardComponent {
    constructor() {
        this.closeChat = new EventEmitter();
        this.minimizeChat = new EventEmitter();
    }
    onCloseChat() {
        this.closeChat.emit(this.user);
    }
    onMinimizeChat() {
        this.minimizeChat.emit(this.user);
    }
}
MixMessengerCardComponent.ɵfac = function MixMessengerCardComponent_Factory(t) { return new (t || MixMessengerCardComponent)(); };
MixMessengerCardComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixMessengerCardComponent, selectors: [["mix-messenger-card", "user", ""]], inputs: { user: "user" }, outputs: { closeChat: "closeChat", minimizeChat: "minimizeChat" }, standalone: true, features: [i0.ɵɵProvidersFeature([
            tuiButtonOptionsProvider({
                shape: 'rounded',
                appearance: "outline" /* Outline */,
                size: 's'
            })
        ]), i0.ɵɵStandaloneFeature], attrs: _c0, decls: 11, vars: 1, consts: [[1, "messenger-card"], [1, "messenger-card__header"], [1, "title"], [1, "toolbar"], ["type", "button", "tuiIconButton", "", "icon", "tuiIconSubitem", 3, "click"], ["type", "button", "tuiIconButton", "", "icon", "tuiIconCloseLarge", 3, "click"], [1, "messenger-card__chat-area"], [1, "messenger-card__footer"], [1, "text-box", 3, "expandable"]], template: function MixMessengerCardComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1)(2, "div", 2);
        i0.ɵɵtext(3, " Phong Cao ");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(4, "div", 3)(5, "button", 4);
        i0.ɵɵlistener("click", function MixMessengerCardComponent_Template_button_click_5_listener() { return ctx.onMinimizeChat(); });
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(6, "button", 5);
        i0.ɵɵlistener("click", function MixMessengerCardComponent_Template_button_click_6_listener() { return ctx.onCloseChat(); });
        i0.ɵɵelementEnd()()();
        i0.ɵɵelement(7, "div", 6);
        i0.ɵɵelementStart(8, "div", 7)(9, "tui-text-area", 8);
        i0.ɵɵtext(10, " Type your message ");
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        i0.ɵɵadvance(9);
        i0.ɵɵproperty("expandable", false);
    } }, dependencies: [ShareModule, i1.TuiButtonComponent, i2.TuiTextAreaComponent, i2.TuiTextAreaDirective], styles: [".messenger-card[_ngcontent-%COMP%]{background-color:var(--tui-base-01);width:21rem;height:29rem;box-shadow:#0000003d 0 3px 8px;border-radius:var(--mix-border-radius-01);border:1px solid var(--tui-base-04);display:flex;flex-direction:column}.messenger-card__header[_ngcontent-%COMP%]{height:3.5rem;padding:0 10px;border-bottom:1px solid var(--tui-base-04);display:flex;align-items:center;justify-content:space-between}.messenger-card__header[_ngcontent-%COMP%]   .toolbar[_ngcontent-%COMP%]{display:flex}.messenger-card__header[_ngcontent-%COMP%]   .toolbar[_ngcontent-%COMP%]   button[_ngcontent-%COMP%]{margin-left:5px}.messenger-card__footer[_ngcontent-%COMP%]{border-top:1px solid var(--tui-base-04);margin-top:auto;padding:8px}.messenger-card__footer[_ngcontent-%COMP%]   .text-box[_ngcontent-%COMP%]{min-height:5rem}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixMessengerCardComponent, [{
        type: Component,
        args: [{ selector: 'mix-messenger-card [user]', standalone: true, imports: [ShareModule], providers: [
                    tuiButtonOptionsProvider({
                        shape: 'rounded',
                        appearance: "outline" /* Outline */,
                        size: 's'
                    })
                ], template: "<div class=\"messenger-card\">\r\n  <div class=\"messenger-card__header\">\r\n    <div class=\"title\">\r\n      Phong Cao\r\n    </div>\r\n\r\n    <div class=\"toolbar\">\r\n      <button type=\"button\"\r\n              (click)=\"onMinimizeChat()\"\r\n              tuiIconButton\r\n              icon=\"tuiIconSubitem\"></button>\r\n      <button type=\"button\"\r\n              (click)=\"onCloseChat()\"\r\n              tuiIconButton\r\n              icon=\"tuiIconCloseLarge\"></button>\r\n    </div>\r\n  </div>\r\n\r\n  <div class=\"messenger-card__chat-area\">\r\n\r\n  </div>\r\n\r\n  <div class=\"messenger-card__footer\">\r\n    <tui-text-area class=\"text-box\"\r\n                   [expandable]=\"false\">\r\n      Type your message\r\n    </tui-text-area>\r\n  </div>\r\n</div>\r\n", styles: [".messenger-card{background-color:var(--tui-base-01);width:21rem;height:29rem;box-shadow:#0000003d 0 3px 8px;border-radius:var(--mix-border-radius-01);border:1px solid var(--tui-base-04);display:flex;flex-direction:column}.messenger-card__header{height:3.5rem;padding:0 10px;border-bottom:1px solid var(--tui-base-04);display:flex;align-items:center;justify-content:space-between}.messenger-card__header .toolbar{display:flex}.messenger-card__header .toolbar button{margin-left:5px}.messenger-card__footer{border-top:1px solid var(--tui-base-04);margin-top:auto;padding:8px}.messenger-card__footer .text-box{min-height:5rem}\n"] }]
    }], null, { user: [{
            type: Input
        }], closeChat: [{
            type: Output
        }], minimizeChat: [{
            type: Output
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWVzc2VuZ2VyLWNhcmQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21peC1jaGF0L21lc3Nlbmdlci1jYXJkL21lc3Nlbmdlci1jYXJkLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtY2hhdC9tZXNzZW5nZXItY2FyZC9tZXNzZW5nZXItY2FyZC5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3ZFLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxrQkFBa0IsQ0FBQztBQUMzQyxPQUFPLEVBQWlCLHdCQUF3QixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFFekUsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHVCQUF1QixDQUFDOzs7OztBQWdCcEQsTUFBTSxPQUFPLHlCQUF5QjtJQWR0QztRQWdCbUIsY0FBUyxHQUEwQixJQUFJLFlBQVksRUFBRSxDQUFDO1FBQ3RELGlCQUFZLEdBQTBCLElBQUksWUFBWSxFQUFFLENBQUM7S0FTM0U7SUFQUSxXQUFXO1FBQ2hCLElBQUksQ0FBQyxTQUFTLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztJQUNqQyxDQUFDO0lBRU0sY0FBYztRQUNuQixJQUFJLENBQUMsWUFBWSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7SUFDcEMsQ0FBQzs7a0dBWFUseUJBQXlCOzRFQUF6Qix5QkFBeUIsbU1BUnpCO1lBQ1Qsd0JBQXdCLENBQUM7Z0JBQ3ZCLEtBQUssRUFBRSxTQUFTO2dCQUNoQixVQUFVLHlCQUF1QjtnQkFDakMsSUFBSSxFQUFFLEdBQUc7YUFDVixDQUFDO1NBQ0g7UUNsQkgsOEJBQTRCLGFBQUEsYUFBQTtRQUd0QiwyQkFDRjtRQUFBLGlCQUFNO1FBRU4sOEJBQXFCLGdCQUFBO1FBRVgsc0dBQVMsb0JBQWdCLElBQUM7UUFFSixpQkFBUztRQUN2QyxpQ0FHaUM7UUFGekIsc0dBQVMsaUJBQWEsSUFBQztRQUVFLGlCQUFTLEVBQUEsRUFBQTtRQUk5Qyx5QkFFTTtRQUVOLDhCQUFvQyx1QkFBQTtRQUdoQyxvQ0FDRjtRQUFBLGlCQUFnQixFQUFBLEVBQUE7O1FBRkQsZUFBb0I7UUFBcEIsa0NBQW9CO3dCRGIzQixXQUFXO3VGQVNWLHlCQUF5QjtjQWRyQyxTQUFTOzJCQUNFLDJCQUEyQixjQUd6QixJQUFJLFdBQ1AsQ0FBQyxXQUFXLENBQUMsYUFDWDtvQkFDVCx3QkFBd0IsQ0FBQzt3QkFDdkIsS0FBSyxFQUFFLFNBQVM7d0JBQ2hCLFVBQVUseUJBQXVCO3dCQUNqQyxJQUFJLEVBQUUsR0FBRztxQkFDVixDQUFDO2lCQUNIO2dCQUdlLElBQUk7a0JBQW5CLEtBQUs7WUFDVyxTQUFTO2tCQUF6QixNQUFNO1lBQ1UsWUFBWTtrQkFBNUIsTUFBTSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE1peFVzZXIgfSBmcm9tICdAbWl4LXNwYS9taXgubGliJztcclxuaW1wb3J0IHsgVHVpQXBwZWFyYW5jZSwgdHVpQnV0dG9uT3B0aW9uc1Byb3ZpZGVyIH0gZnJvbSAnQHRhaWdhLXVpL2NvcmUnO1xyXG5cclxuaW1wb3J0IHsgU2hhcmVNb2R1bGUgfSBmcm9tICcuLi8uLi8uLi9zaGFyZS5tb2R1bGUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtbWVzc2VuZ2VyLWNhcmQgW3VzZXJdJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vbWVzc2VuZ2VyLWNhcmQuY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL21lc3Nlbmdlci1jYXJkLmNvbXBvbmVudC5zY3NzJ10sXHJcbiAgc3RhbmRhbG9uZTogdHJ1ZSxcclxuICBpbXBvcnRzOiBbU2hhcmVNb2R1bGVdLFxyXG4gIHByb3ZpZGVyczogW1xyXG4gICAgdHVpQnV0dG9uT3B0aW9uc1Byb3ZpZGVyKHtcclxuICAgICAgc2hhcGU6ICdyb3VuZGVkJyxcclxuICAgICAgYXBwZWFyYW5jZTogVHVpQXBwZWFyYW5jZS5PdXRsaW5lLFxyXG4gICAgICBzaXplOiAncydcclxuICAgIH0pXHJcbiAgXVxyXG59KVxyXG5leHBvcnQgY2xhc3MgTWl4TWVzc2VuZ2VyQ2FyZENvbXBvbmVudCB7XHJcbiAgQElucHV0KCkgcHVibGljIHVzZXIhOiBNaXhVc2VyO1xyXG4gIEBPdXRwdXQoKSBwdWJsaWMgY2xvc2VDaGF0OiBFdmVudEVtaXR0ZXI8TWl4VXNlcj4gPSBuZXcgRXZlbnRFbWl0dGVyKCk7XHJcbiAgQE91dHB1dCgpIHB1YmxpYyBtaW5pbWl6ZUNoYXQ6IEV2ZW50RW1pdHRlcjxNaXhVc2VyPiA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcclxuXHJcbiAgcHVibGljIG9uQ2xvc2VDaGF0KCk6IHZvaWQge1xyXG4gICAgdGhpcy5jbG9zZUNoYXQuZW1pdCh0aGlzLnVzZXIpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIG9uTWluaW1pemVDaGF0KCk6IHZvaWQge1xyXG4gICAgdGhpcy5taW5pbWl6ZUNoYXQuZW1pdCh0aGlzLnVzZXIpO1xyXG4gIH1cclxufVxyXG4iLCI8ZGl2IGNsYXNzPVwibWVzc2VuZ2VyLWNhcmRcIj5cclxuICA8ZGl2IGNsYXNzPVwibWVzc2VuZ2VyLWNhcmRfX2hlYWRlclwiPlxyXG4gICAgPGRpdiBjbGFzcz1cInRpdGxlXCI+XHJcbiAgICAgIFBob25nIENhb1xyXG4gICAgPC9kaXY+XHJcblxyXG4gICAgPGRpdiBjbGFzcz1cInRvb2xiYXJcIj5cclxuICAgICAgPGJ1dHRvbiB0eXBlPVwiYnV0dG9uXCJcclxuICAgICAgICAgICAgICAoY2xpY2spPVwib25NaW5pbWl6ZUNoYXQoKVwiXHJcbiAgICAgICAgICAgICAgdHVpSWNvbkJ1dHRvblxyXG4gICAgICAgICAgICAgIGljb249XCJ0dWlJY29uU3ViaXRlbVwiPjwvYnV0dG9uPlxyXG4gICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIlxyXG4gICAgICAgICAgICAgIChjbGljayk9XCJvbkNsb3NlQ2hhdCgpXCJcclxuICAgICAgICAgICAgICB0dWlJY29uQnV0dG9uXHJcbiAgICAgICAgICAgICAgaWNvbj1cInR1aUljb25DbG9zZUxhcmdlXCI+PC9idXR0b24+XHJcbiAgICA8L2Rpdj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cIm1lc3Nlbmdlci1jYXJkX19jaGF0LWFyZWFcIj5cclxuXHJcbiAgPC9kaXY+XHJcblxyXG4gIDxkaXYgY2xhc3M9XCJtZXNzZW5nZXItY2FyZF9fZm9vdGVyXCI+XHJcbiAgICA8dHVpLXRleHQtYXJlYSBjbGFzcz1cInRleHQtYm94XCJcclxuICAgICAgICAgICAgICAgICAgIFtleHBhbmRhYmxlXT1cImZhbHNlXCI+XHJcbiAgICAgIFR5cGUgeW91ciBtZXNzYWdlXHJcbiAgICA8L3R1aS10ZXh0LWFyZWE+XHJcbiAgPC9kaXY+XHJcbjwvZGl2PlxyXG4iXX0=