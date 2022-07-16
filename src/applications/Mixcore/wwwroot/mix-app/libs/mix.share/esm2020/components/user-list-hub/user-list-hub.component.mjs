import { Component } from '@angular/core';
import { ShareModule } from '../../share.module';
import * as i0 from "@angular/core";
import * as i1 from "@taiga-ui/kit";
export class MixUserListHubComponent {
}
MixUserListHubComponent.ɵfac = function MixUserListHubComponent_Factory(t) { return new (t || MixUserListHubComponent)(); };
MixUserListHubComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: MixUserListHubComponent, selectors: [["mix-user-list-hub"]], standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 15, vars: 9, consts: [[1, "user-list-hub"], [1, "user-list-hub__title"], [1, "user-list-hub__avatar"], ["text", "Nhat Hoang", 3, "autoColor", "rounded", "size"]], template: function MixUserListHubComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1);
        i0.ɵɵtext(2, " Users online in hub ");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(3, "div", 2);
        i0.ɵɵelement(4, "tui-avatar", 3);
        i0.ɵɵelementStart(5, "div");
        i0.ɵɵtext(6, " Nguyen Nhat Hoang ");
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(7, "div", 2);
        i0.ɵɵelement(8, "tui-avatar", 3);
        i0.ɵɵelementStart(9, "div");
        i0.ɵɵtext(10, " Cao Thien Phong ");
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(11, "div", 2);
        i0.ɵɵelement(12, "tui-avatar", 3);
        i0.ɵɵelementStart(13, "div");
        i0.ɵɵtext(14, " Harry Potter ");
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("autoColor", true)("rounded", true)("size", "s");
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("autoColor", true)("rounded", true)("size", "s");
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("autoColor", true)("rounded", true)("size", "s");
    } }, dependencies: [ShareModule, i1.TuiAvatarComponent], styles: [".user-list-hub[_ngcontent-%COMP%]{width:100%;height:100%}.user-list-hub__title[_ngcontent-%COMP%]{padding-left:5px;margin-bottom:10px;font-weight:700}.user-list-hub__avatar[_ngcontent-%COMP%]{padding:10px;display:flex;justify-content:baseline;align-items:center;border-radius:var(--mix-border-radius-01);cursor:pointer;transition:all .2s ease}.user-list-hub__avatar[_ngcontent-%COMP%]   tui-avatar[_ngcontent-%COMP%]{margin-right:15px}.user-list-hub__avatar[_ngcontent-%COMP%]:hover{background-color:var(--tui-base-01);transform:translate(-.3rem,-.3ex);box-shadow:#64646f33 0 7px 29px}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixUserListHubComponent, [{
        type: Component,
        args: [{ selector: 'mix-user-list-hub', standalone: true, imports: [ShareModule], template: "<div class=\"user-list-hub\">\r\n  <div class=\"user-list-hub__title\"> Users online in hub </div>\r\n  <div class=\"user-list-hub__avatar\">\r\n    <tui-avatar [autoColor]=\"true\"\r\n                [rounded]=\"true\"\r\n                [size]=\"'s'\"\r\n                text=\"Nhat Hoang\"></tui-avatar>\r\n    <div> Nguyen Nhat Hoang </div>\r\n  </div>\r\n\r\n  <div class=\"user-list-hub__avatar\">\r\n    <tui-avatar [autoColor]=\"true\"\r\n                [rounded]=\"true\"\r\n                [size]=\"'s'\"\r\n                text=\"Nhat Hoang\"></tui-avatar>\r\n    <div> Cao Thien Phong </div>\r\n  </div>\r\n\r\n  <div class=\"user-list-hub__avatar\">\r\n    <tui-avatar [autoColor]=\"true\"\r\n                [rounded]=\"true\"\r\n                [size]=\"'s'\"\r\n                text=\"Nhat Hoang\"></tui-avatar>\r\n    <div> Harry Potter </div>\r\n  </div>\r\n</div>\r\n", styles: [".user-list-hub{width:100%;height:100%}.user-list-hub__title{padding-left:5px;margin-bottom:10px;font-weight:700}.user-list-hub__avatar{padding:10px;display:flex;justify-content:baseline;align-items:center;border-radius:var(--mix-border-radius-01);cursor:pointer;transition:all .2s ease}.user-list-hub__avatar tui-avatar{margin-right:15px}.user-list-hub__avatar:hover{background-color:var(--tui-base-01);transform:translate(-.3rem,-.3ex);box-shadow:#64646f33 0 7px 29px}\n"] }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidXNlci1saXN0LWh1Yi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL2NvbXBvbmVudHMvdXNlci1saXN0LWh1Yi91c2VyLWxpc3QtaHViLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy91c2VyLWxpc3QtaHViL3VzZXItbGlzdC1odWIuY29tcG9uZW50Lmh0bWwiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUxQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sb0JBQW9CLENBQUM7OztBQVNqRCxNQUFNLE9BQU8sdUJBQXVCOzs4RkFBdkIsdUJBQXVCOzBFQUF2Qix1QkFBdUI7UUNYcEMsOEJBQTJCLGFBQUE7UUFDVSxxQ0FBb0I7UUFBQSxpQkFBTTtRQUM3RCw4QkFBbUM7UUFDakMsZ0NBRzJDO1FBQzNDLDJCQUFLO1FBQUMsbUNBQWtCO1FBQUEsaUJBQU0sRUFBQTtRQUdoQyw4QkFBbUM7UUFDakMsZ0NBRzJDO1FBQzNDLDJCQUFLO1FBQUMsa0NBQWdCO1FBQUEsaUJBQU0sRUFBQTtRQUc5QiwrQkFBbUM7UUFDakMsaUNBRzJDO1FBQzNDLDRCQUFLO1FBQUMsK0JBQWE7UUFBQSxpQkFBTSxFQUFBLEVBQUE7O1FBcEJiLGVBQWtCO1FBQWxCLGdDQUFrQixpQkFBQSxhQUFBO1FBUWxCLGVBQWtCO1FBQWxCLGdDQUFrQixpQkFBQSxhQUFBO1FBUWxCLGVBQWtCO1FBQWxCLGdDQUFrQixpQkFBQSxhQUFBO3dCRFZ0QixXQUFXO3VGQUVWLHVCQUF1QjtjQVBuQyxTQUFTOzJCQUNFLG1CQUFtQixjQUdqQixJQUFJLFdBQ1AsQ0FBQyxXQUFXLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbmltcG9ydCB7IFNoYXJlTW9kdWxlIH0gZnJvbSAnLi4vLi4vc2hhcmUubW9kdWxlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnbWl4LXVzZXItbGlzdC1odWInLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi91c2VyLWxpc3QtaHViLmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi91c2VyLWxpc3QtaHViLmNvbXBvbmVudC5zY3NzJ10sXHJcbiAgc3RhbmRhbG9uZTogdHJ1ZSxcclxuICBpbXBvcnRzOiBbU2hhcmVNb2R1bGVdXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBNaXhVc2VyTGlzdEh1YkNvbXBvbmVudCB7XHJcbiAgLy9cclxufVxyXG4iLCI8ZGl2IGNsYXNzPVwidXNlci1saXN0LWh1YlwiPlxyXG4gIDxkaXYgY2xhc3M9XCJ1c2VyLWxpc3QtaHViX190aXRsZVwiPiBVc2VycyBvbmxpbmUgaW4gaHViIDwvZGl2PlxyXG4gIDxkaXYgY2xhc3M9XCJ1c2VyLWxpc3QtaHViX19hdmF0YXJcIj5cclxuICAgIDx0dWktYXZhdGFyIFthdXRvQ29sb3JdPVwidHJ1ZVwiXHJcbiAgICAgICAgICAgICAgICBbcm91bmRlZF09XCJ0cnVlXCJcclxuICAgICAgICAgICAgICAgIFtzaXplXT1cIidzJ1wiXHJcbiAgICAgICAgICAgICAgICB0ZXh0PVwiTmhhdCBIb2FuZ1wiPjwvdHVpLWF2YXRhcj5cclxuICAgIDxkaXY+IE5ndXllbiBOaGF0IEhvYW5nIDwvZGl2PlxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2IGNsYXNzPVwidXNlci1saXN0LWh1Yl9fYXZhdGFyXCI+XHJcbiAgICA8dHVpLWF2YXRhciBbYXV0b0NvbG9yXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgW3JvdW5kZWRdPVwidHJ1ZVwiXHJcbiAgICAgICAgICAgICAgICBbc2l6ZV09XCIncydcIlxyXG4gICAgICAgICAgICAgICAgdGV4dD1cIk5oYXQgSG9hbmdcIj48L3R1aS1hdmF0YXI+XHJcbiAgICA8ZGl2PiBDYW8gVGhpZW4gUGhvbmcgPC9kaXY+XHJcbiAgPC9kaXY+XHJcblxyXG4gIDxkaXYgY2xhc3M9XCJ1c2VyLWxpc3QtaHViX19hdmF0YXJcIj5cclxuICAgIDx0dWktYXZhdGFyIFthdXRvQ29sb3JdPVwidHJ1ZVwiXHJcbiAgICAgICAgICAgICAgICBbcm91bmRlZF09XCJ0cnVlXCJcclxuICAgICAgICAgICAgICAgIFtzaXplXT1cIidzJ1wiXHJcbiAgICAgICAgICAgICAgICB0ZXh0PVwiTmhhdCBIb2FuZ1wiPjwvdHVpLWF2YXRhcj5cclxuICAgIDxkaXY+IEhhcnJ5IFBvdHRlciA8L2Rpdj5cclxuICA8L2Rpdj5cclxuPC9kaXY+XHJcbiJdfQ==