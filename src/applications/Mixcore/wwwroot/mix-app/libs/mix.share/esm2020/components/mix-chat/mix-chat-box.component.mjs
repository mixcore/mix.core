import { Component } from '@angular/core';
import { ShareModule } from '../../share.module';
import { MixMessengerCardComponent } from './messenger-card/messenger-card.component';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common";
import * as i2 from "@taiga-ui/kit";
import * as i3 from "angular-tabler-icons";
function MixChatBoxComponent_ng_container_2_Template(rf, ctx) { if (rf & 1) {
    const _r4 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "mix-messenger-card", 6);
    i0.ɵɵlistener("closeChat", function MixChatBoxComponent_ng_container_2_Template_mix_messenger_card_closeChat_1_listener($event) { i0.ɵɵrestoreView(_r4); const ctx_r3 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r3.closeChat($event)); })("minimizeChat", function MixChatBoxComponent_ng_container_2_Template_mix_messenger_card_minimizeChat_1_listener($event) { i0.ɵɵrestoreView(_r4); const ctx_r5 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r5.addToMinimize($event)); });
    i0.ɵɵelementEnd();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const user_r2 = ctx.$implicit;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("user", user_r2);
} }
function MixChatBoxComponent_ng_container_4_Template(rf, ctx) { if (rf & 1) {
    const _r8 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "div", 7);
    i0.ɵɵlistener("click", function MixChatBoxComponent_ng_container_4_Template_div_click_1_listener() { const restoredCtx = i0.ɵɵrestoreView(_r8); const user_r6 = restoredCtx.$implicit; const ctx_r7 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r7.addChatting(user_r6)); });
    i0.ɵɵelement(2, "tui-avatar", 8);
    i0.ɵɵelementEnd();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const user_r6 = ctx.$implicit;
    let tmp_3_0;
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("autoColor", true)("rounded", true)("size", "m")("text", (tmp_3_0 = user_r6.userName) !== null && tmp_3_0 !== undefined ? tmp_3_0 : "");
} }
export class MixChatBoxComponent {
    constructor() {
        this.usersOnChatting = [];
        this.usersOnMinimize = [{ userName: 'Phong Cao' }, { userName: 'Huy' }, { userName: 'Nhat Hoang' }];
    }
    addChatting(user) {
        const userToAddIndex = this.usersOnMinimize.findIndex(u => u.userName === user.userName);
        if (userToAddIndex >= 0) {
            this.usersOnChatting.push(this.usersOnMinimize[userToAddIndex]);
            this.usersOnMinimize = this.usersOnMinimize.filter(u => u.userName !== user.userName);
        }
    }
    addToMinimize(user) {
        this.usersOnMinimize.push(user);
        this.closeChat(user);
    }
    closeChat(user) {
        this.usersOnChatting = this.usersOnChatting.filter(u => u.userName !== user.userName);
    }
}
MixChatBoxComponent.ɵfac = function MixChatBoxComponent_Factory(t) { return new (t || MixChatBoxComponent)(); };
MixChatBoxComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixChatBoxComponent, selectors: [["mix-chat-box"]], standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 7, vars: 2, consts: [[1, "chat-box"], [1, "chat-box__chat-zone"], [4, "ngFor", "ngForOf"], [1, "chat-box__users-zone"], [1, "avatar"], ["name", "message-circle"], [3, "user", "closeChat", "minimizeChat"], [1, "avatar", 3, "click"], [3, "autoColor", "rounded", "size", "text"]], template: function MixChatBoxComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1);
        i0.ɵɵtemplate(2, MixChatBoxComponent_ng_container_2_Template, 2, 1, "ng-container", 2);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(3, "div", 3);
        i0.ɵɵtemplate(4, MixChatBoxComponent_ng_container_4_Template, 3, 4, "ng-container", 2);
        i0.ɵɵelementStart(5, "div", 4);
        i0.ɵɵelement(6, "i-tabler", 5);
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("ngForOf", ctx.usersOnChatting);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("ngForOf", ctx.usersOnMinimize);
    } }, dependencies: [ShareModule, i1.NgForOf, i2.TuiAvatarComponent, i3.TablerIconComponent, MixMessengerCardComponent], styles: [".chat-box[_ngcontent-%COMP%]{display:flex;align-items:flex-end}.chat-box__chat-zone[_ngcontent-%COMP%]{margin-right:30px;display:flex}.chat-box__chat-zone[_ngcontent-%COMP%]   mix-messenger-card[_ngcontent-%COMP%]{margin-left:10px}.chat-box__users-zone[_ngcontent-%COMP%]{padding:5px}.chat-box__users-zone[_ngcontent-%COMP%]   .avatar[_ngcontent-%COMP%]{margin-bottom:5px;border-radius:50%;z-index:0;transition:all .2s ease;cursor:pointer}.chat-box__users-zone[_ngcontent-%COMP%]   .avatar[_ngcontent-%COMP%]:hover{transform:translate(-.3rem,-.3ex)}.chat-box__users-zone[_ngcontent-%COMP%]   .avatar[_ngcontent-%COMP%]:last-child{display:flex;justify-content:center;align-items:center;height:48px;width:48px;background-color:var(--tui-base-03);font-size:12px}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixChatBoxComponent, [{
        type: Component,
        args: [{ selector: 'mix-chat-box', standalone: true, imports: [ShareModule, MixMessengerCardComponent], template: "<div class=\"chat-box\">\r\n  <div class=\"chat-box__chat-zone\">\r\n    <ng-container *ngFor=\"let user of usersOnChatting\">\r\n      <mix-messenger-card [user]=\"user\"\r\n                          (closeChat)=\"closeChat($event)\"\r\n                          (minimizeChat)=\"addToMinimize($event)\"></mix-messenger-card>\r\n    </ng-container>\r\n  </div>\r\n\r\n  <div class=\"chat-box__users-zone\">\r\n    <ng-container *ngFor=\"let user of usersOnMinimize\">\r\n      <div class=\"avatar\"\r\n           (click)=\"addChatting(user)\">\r\n        <tui-avatar [autoColor]=\"true\"\r\n                    [rounded]=\"true\"\r\n                    [size]=\"'m'\"\r\n                    [text]=\"user.userName ?? ''\"></tui-avatar>\r\n      </div>\r\n    </ng-container>\r\n\r\n    <div class=\"avatar\">\r\n      <i-tabler name=\"message-circle\"></i-tabler>\r\n    </div>\r\n  </div>\r\n</div>\r\n", styles: [".chat-box{display:flex;align-items:flex-end}.chat-box__chat-zone{margin-right:30px;display:flex}.chat-box__chat-zone mix-messenger-card{margin-left:10px}.chat-box__users-zone{padding:5px}.chat-box__users-zone .avatar{margin-bottom:5px;border-radius:50%;z-index:0;transition:all .2s ease;cursor:pointer}.chat-box__users-zone .avatar:hover{transform:translate(-.3rem,-.3ex)}.chat-box__users-zone .avatar:last-child{display:flex;justify-content:center;align-items:center;height:48px;width:48px;background-color:var(--tui-base-03);font-size:12px}\n"] }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWl4LWNoYXQtYm94LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9taXgtY2hhdC9taXgtY2hhdC1ib3guY29tcG9uZW50LnRzIiwiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21peC1jaGF0L21peC1jaGF0LWJveC5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRzFDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUNqRCxPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSwyQ0FBMkMsQ0FBQzs7Ozs7OztJQ0ZsRiw2QkFBbUQ7SUFDakQsNkNBRTJEO0lBRHZDLG1NQUFhLGVBQUEsd0JBQWlCLENBQUEsSUFBQyw0TEFDZixlQUFBLDRCQUFxQixDQUFBLElBRE47SUFDUSxpQkFBcUI7SUFDbEYsMEJBQWU7OztJQUhPLGVBQWE7SUFBYiw4QkFBYTs7OztJQU9uQyw2QkFBbUQ7SUFDakQsOEJBQ2lDO0lBQTVCLGlPQUFTLGVBQUEsMkJBQWlCLENBQUEsSUFBQztJQUM5QixnQ0FHc0Q7SUFDeEQsaUJBQU07SUFDUiwwQkFBZTs7OztJQUxDLGVBQWtCO0lBQWxCLGdDQUFrQixpQkFBQSxhQUFBLHVGQUFBOztBREF0QyxNQUFNLE9BQU8sbUJBQW1CO0lBUGhDO1FBUVMsb0JBQWUsR0FBYyxFQUFFLENBQUM7UUFDaEMsb0JBQWUsR0FBYyxDQUFDLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxFQUFFLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsUUFBUSxFQUFFLFlBQVksRUFBRSxDQUFDLENBQUM7S0FrQmxIO0lBaEJRLFdBQVcsQ0FBQyxJQUFhO1FBQzlCLE1BQU0sY0FBYyxHQUFHLElBQUksQ0FBQyxlQUFlLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLFFBQVEsS0FBSyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7UUFDekYsSUFBSSxjQUFjLElBQUksQ0FBQyxFQUFFO1lBQ3ZCLElBQUksQ0FBQyxlQUFlLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxlQUFlLENBQUMsY0FBYyxDQUFDLENBQUMsQ0FBQztZQUNoRSxJQUFJLENBQUMsZUFBZSxHQUFHLElBQUksQ0FBQyxlQUFlLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLFFBQVEsS0FBSyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7U0FDdkY7SUFDSCxDQUFDO0lBRU0sYUFBYSxDQUFDLElBQWE7UUFDaEMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7UUFDaEMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxJQUFJLENBQUMsQ0FBQztJQUN2QixDQUFDO0lBRU0sU0FBUyxDQUFDLElBQWE7UUFDNUIsSUFBSSxDQUFDLGVBQWUsR0FBRyxJQUFJLENBQUMsZUFBZSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxRQUFRLEtBQUssSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO0lBQ3hGLENBQUM7O3NGQW5CVSxtQkFBbUI7c0VBQW5CLG1CQUFtQjtRQ2JoQyw4QkFBc0IsYUFBQTtRQUVsQixzRkFJZTtRQUNqQixpQkFBTTtRQUVOLDhCQUFrQztRQUNoQyxzRkFRZTtRQUVmLDhCQUFvQjtRQUNsQiw4QkFBMkM7UUFDN0MsaUJBQU0sRUFBQSxFQUFBOztRQXBCeUIsZUFBa0I7UUFBbEIsNkNBQWtCO1FBUWxCLGVBQWtCO1FBQWxCLDZDQUFrQjt3QkRDekMsV0FBVyw2REFBRSx5QkFBeUI7dUZBRXJDLG1CQUFtQjtjQVAvQixTQUFTOzJCQUNFLGNBQWMsY0FHWixJQUFJLFdBQ1AsQ0FBQyxXQUFXLEVBQUUseUJBQXlCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgTWl4VXNlciB9IGZyb20gJ0BtaXgtc3BhL21peC5saWInO1xyXG5cclxuaW1wb3J0IHsgU2hhcmVNb2R1bGUgfSBmcm9tICcuLi8uLi9zaGFyZS5tb2R1bGUnO1xyXG5pbXBvcnQgeyBNaXhNZXNzZW5nZXJDYXJkQ29tcG9uZW50IH0gZnJvbSAnLi9tZXNzZW5nZXItY2FyZC9tZXNzZW5nZXItY2FyZC5jb21wb25lbnQnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtY2hhdC1ib3gnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9taXgtY2hhdC1ib3guY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL21peC1jaGF0LWJveC5jb21wb25lbnQuc2NzcyddLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW1NoYXJlTW9kdWxlLCBNaXhNZXNzZW5nZXJDYXJkQ29tcG9uZW50XVxyXG59KVxyXG5leHBvcnQgY2xhc3MgTWl4Q2hhdEJveENvbXBvbmVudCB7XHJcbiAgcHVibGljIHVzZXJzT25DaGF0dGluZzogTWl4VXNlcltdID0gW107XHJcbiAgcHVibGljIHVzZXJzT25NaW5pbWl6ZTogTWl4VXNlcltdID0gW3sgdXNlck5hbWU6ICdQaG9uZyBDYW8nIH0sIHsgdXNlck5hbWU6ICdIdXknIH0sIHsgdXNlck5hbWU6ICdOaGF0IEhvYW5nJyB9XTtcclxuXHJcbiAgcHVibGljIGFkZENoYXR0aW5nKHVzZXI6IE1peFVzZXIpOiB2b2lkIHtcclxuICAgIGNvbnN0IHVzZXJUb0FkZEluZGV4ID0gdGhpcy51c2Vyc09uTWluaW1pemUuZmluZEluZGV4KHUgPT4gdS51c2VyTmFtZSA9PT0gdXNlci51c2VyTmFtZSk7XHJcbiAgICBpZiAodXNlclRvQWRkSW5kZXggPj0gMCkge1xyXG4gICAgICB0aGlzLnVzZXJzT25DaGF0dGluZy5wdXNoKHRoaXMudXNlcnNPbk1pbmltaXplW3VzZXJUb0FkZEluZGV4XSk7XHJcbiAgICAgIHRoaXMudXNlcnNPbk1pbmltaXplID0gdGhpcy51c2Vyc09uTWluaW1pemUuZmlsdGVyKHUgPT4gdS51c2VyTmFtZSAhPT0gdXNlci51c2VyTmFtZSk7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgYWRkVG9NaW5pbWl6ZSh1c2VyOiBNaXhVc2VyKTogdm9pZCB7XHJcbiAgICB0aGlzLnVzZXJzT25NaW5pbWl6ZS5wdXNoKHVzZXIpO1xyXG4gICAgdGhpcy5jbG9zZUNoYXQodXNlcik7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgY2xvc2VDaGF0KHVzZXI6IE1peFVzZXIpOiB2b2lkIHtcclxuICAgIHRoaXMudXNlcnNPbkNoYXR0aW5nID0gdGhpcy51c2Vyc09uQ2hhdHRpbmcuZmlsdGVyKHUgPT4gdS51c2VyTmFtZSAhPT0gdXNlci51c2VyTmFtZSk7XHJcbiAgfVxyXG59XHJcbiIsIjxkaXYgY2xhc3M9XCJjaGF0LWJveFwiPlxyXG4gIDxkaXYgY2xhc3M9XCJjaGF0LWJveF9fY2hhdC16b25lXCI+XHJcbiAgICA8bmctY29udGFpbmVyICpuZ0Zvcj1cImxldCB1c2VyIG9mIHVzZXJzT25DaGF0dGluZ1wiPlxyXG4gICAgICA8bWl4LW1lc3Nlbmdlci1jYXJkIFt1c2VyXT1cInVzZXJcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgIChjbG9zZUNoYXQpPVwiY2xvc2VDaGF0KCRldmVudClcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgIChtaW5pbWl6ZUNoYXQpPVwiYWRkVG9NaW5pbWl6ZSgkZXZlbnQpXCI+PC9taXgtbWVzc2VuZ2VyLWNhcmQ+XHJcbiAgICA8L25nLWNvbnRhaW5lcj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cImNoYXQtYm94X191c2Vycy16b25lXCI+XHJcbiAgICA8bmctY29udGFpbmVyICpuZ0Zvcj1cImxldCB1c2VyIG9mIHVzZXJzT25NaW5pbWl6ZVwiPlxyXG4gICAgICA8ZGl2IGNsYXNzPVwiYXZhdGFyXCJcclxuICAgICAgICAgICAoY2xpY2spPVwiYWRkQ2hhdHRpbmcodXNlcilcIj5cclxuICAgICAgICA8dHVpLWF2YXRhciBbYXV0b0NvbG9yXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgIFtyb3VuZGVkXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgIFtzaXplXT1cIidtJ1wiXHJcbiAgICAgICAgICAgICAgICAgICAgW3RleHRdPVwidXNlci51c2VyTmFtZSA/PyAnJ1wiPjwvdHVpLWF2YXRhcj5cclxuICAgICAgPC9kaXY+XHJcbiAgICA8L25nLWNvbnRhaW5lcj5cclxuXHJcbiAgICA8ZGl2IGNsYXNzPVwiYXZhdGFyXCI+XHJcbiAgICAgIDxpLXRhYmxlciBuYW1lPVwibWVzc2FnZS1jaXJjbGVcIj48L2ktdGFibGVyPlxyXG4gICAgPC9kaXY+XHJcbiAgPC9kaXY+XHJcbjwvZGl2PlxyXG4iXX0=