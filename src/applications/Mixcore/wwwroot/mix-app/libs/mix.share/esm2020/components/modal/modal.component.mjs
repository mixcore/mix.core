import { ChangeDetectionStrategy, Component, Inject, ViewEncapsulation } from '@angular/core';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import * as i0 from "@angular/core";
import * as i1 from "@taiga-ui/core";
import * as i2 from "@tinkoff/ng-polymorpheus";
export class ModalComponent {
    constructor(context) {
        this.context = context;
    }
    onClick(response) {
        this.context.completeWith(response);
    }
}
ModalComponent.ɵfac = function ModalComponent_Factory(t) { return new (t || ModalComponent)(i0.ɵɵdirectiveInject(POLYMORPHEUS_CONTEXT)); };
ModalComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: ModalComponent, selectors: [["mix-modal"]], decls: 9, vars: 9, consts: [[1, "mix-modal"], [3, "id"], ["polymorpheus-outlet", "", 3, "content", "context"], [1, "mix-modal__buttons"], ["tuiButton", "", 1, "tui-space_right-3", 3, "click"], ["tuiButton", "", "appearance", "secondary", 3, "click"]], template: function ModalComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "h3", 1);
        i0.ɵɵtext(2);
        i0.ɵɵelementEnd();
        i0.ɵɵelement(3, "section", 2);
        i0.ɵɵelementStart(4, "p", 3)(5, "button", 4);
        i0.ɵɵlistener("click", function ModalComponent_Template_button_click_5_listener() { return ctx.onClick(true); });
        i0.ɵɵtext(6);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(7, "button", 5);
        i0.ɵɵlistener("click", function ModalComponent_Template_button_click_7_listener() { return ctx.onClick(false); });
        i0.ɵɵtext(8);
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        i0.ɵɵstyleMapInterpolate1("--modal-boxshadow: ", ctx.context.borderShadowColor, "");
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("id", ctx.context.id);
        i0.ɵɵadvance(1);
        i0.ɵɵtextInterpolate(ctx.context.heading);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("content", ctx.context.content)("context", ctx.context);
        i0.ɵɵadvance(3);
        i0.ɵɵtextInterpolate1(" ", ctx.context.buttons[0], " ");
        i0.ɵɵadvance(2);
        i0.ɵɵtextInterpolate1(" ", ctx.context.buttons[1], " ");
    } }, dependencies: [i1.TuiButtonComponent, i2.PolymorpheusOutletComponent], styles: ["tui-dialog-host{z-index:100}mix-modal{width:100%;height:100%;display:flex;justify-content:center;align-items:center}.mix-modal{display:block;padding:1.5rem;margin:auto;border-radius:0 0 .75rem .75rem;background:var(--tui-base-01);box-shadow:inset 0 4px var(--modal-boxshadow);animation:tuiReveal var(--tui-duration),tuiFadeIn var(--tui-duration)}.mix-modal__buttons{display:flex;justify-content:center;margin:1.5rem 0 0}\n"], encapsulation: 2, changeDetection: 0 });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(ModalComponent, [{
        type: Component,
        args: [{ selector: 'mix-modal', changeDetection: ChangeDetectionStrategy.OnPush, encapsulation: ViewEncapsulation.None, template: "\t<div class=\"mix-modal\"\r\n\t     style=\"--modal-boxshadow: {{context.borderShadowColor}}\">\r\n\t  <h3 [id]=\"context.id\">{{ context.heading }}</h3>\r\n\t  <section [content]=\"context.content\"\r\n\t           [context]=\"context\"\r\n\t           polymorpheus-outlet></section>\r\n\t  <p class=\"mix-modal__buttons\">\r\n\t    <button class=\"tui-space_right-3\"\r\n\t            (click)=\"onClick(true)\"\r\n\t            tuiButton>\r\n\t      {{ context.buttons[0] }}\r\n\t    </button>\r\n\r\n\t    <button (click)=\"onClick(false)\"\r\n\t            tuiButton\r\n\t            appearance=\"secondary\">\r\n\t      {{ context.buttons[1] }}\r\n\t    </button>\r\n\t  </p>\r\n\t</div>\r\n", styles: ["tui-dialog-host{z-index:100}mix-modal{width:100%;height:100%;display:flex;justify-content:center;align-items:center}.mix-modal{display:block;padding:1.5rem;margin:auto;border-radius:0 0 .75rem .75rem;background:var(--tui-base-01);box-shadow:inset 0 4px var(--modal-boxshadow);animation:tuiReveal var(--tui-duration),tuiFadeIn var(--tui-duration)}.mix-modal__buttons{display:flex;justify-content:center;margin:1.5rem 0 0}\n"] }]
    }], function () { return [{ type: undefined, decorators: [{
                type: Inject,
                args: [POLYMORPHEUS_CONTEXT]
            }] }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL21vZGFsL21vZGFsLmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUU5RixPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQzs7OztBQVdoRSxNQUFNLE9BQU8sY0FBYztJQUN6QixZQUEwRCxPQUF3QztRQUF4QyxZQUFPLEdBQVAsT0FBTyxDQUFpQztJQUFHLENBQUM7SUFFL0YsT0FBTyxDQUFDLFFBQWlCO1FBQzlCLElBQUksQ0FBQyxPQUFPLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxDQUFDO0lBQ3RDLENBQUM7OzRFQUxVLGNBQWMsdUJBQ0wsb0JBQW9CO2lFQUQ3QixjQUFjO1FDYjFCLDhCQUM4RCxZQUFBO1FBQ3RDLFlBQXFCO1FBQUEsaUJBQUs7UUFDaEQsNkJBRXVDO1FBQ3ZDLDRCQUE4QixnQkFBQTtRQUVwQiwyRkFBUyxZQUFRLElBQUksQ0FBQyxJQUFDO1FBRTdCLFlBQ0Y7UUFBQSxpQkFBUztRQUVULGlDQUUrQjtRQUZ2QiwyRkFBUyxZQUFRLEtBQUssQ0FBQyxJQUFDO1FBRzlCLFlBQ0Y7UUFBQSxpQkFBUyxFQUFBLEVBQUE7O1FBaEJSLG1GQUF3RDtRQUN2RCxlQUFpQjtRQUFqQixtQ0FBaUI7UUFBQyxlQUFxQjtRQUFyQix5Q0FBcUI7UUFDbEMsZUFBMkI7UUFBM0IsNkNBQTJCLHdCQUFBO1FBT2hDLGVBQ0Y7UUFERSx1REFDRjtRQUtFLGVBQ0Y7UUFERSx1REFDRjs7dUZESlEsY0FBYztjQVAxQixTQUFTOzJCQUNFLFdBQVcsbUJBR0osdUJBQXVCLENBQUMsTUFBTSxpQkFDaEMsaUJBQWlCLENBQUMsSUFBSTs7c0JBR3hCLE1BQU07dUJBQUMsb0JBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3ksIENvbXBvbmVudCwgSW5qZWN0LCBWaWV3RW5jYXBzdWxhdGlvbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBUdWlEaWFsb2cgfSBmcm9tICdAdGFpZ2EtdWkvY2RrJztcclxuaW1wb3J0IHsgUE9MWU1PUlBIRVVTX0NPTlRFWFQgfSBmcm9tICdAdGlua29mZi9uZy1wb2x5bW9ycGhldXMnO1xyXG5cclxuaW1wb3J0IHsgTW9kYWxPcHRpb24gfSBmcm9tICcuL21vZGFsLnNlcnZpY2UnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtbW9kYWwnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9tb2RhbC5jb21wb25lbnQuaHRtbCcsXHJcbiAgc3R5bGVVcmxzOiBbJy4vbW9kYWwuY29tcG9uZW50LnNjc3MnXSxcclxuICBjaGFuZ2VEZXRlY3Rpb246IENoYW5nZURldGVjdGlvblN0cmF0ZWd5Lk9uUHVzaCxcclxuICBlbmNhcHN1bGF0aW9uOiBWaWV3RW5jYXBzdWxhdGlvbi5Ob25lXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBNb2RhbENvbXBvbmVudCB7XHJcbiAgY29uc3RydWN0b3IoQEluamVjdChQT0xZTU9SUEhFVVNfQ09OVEVYVCkgcHVibGljIHJlYWRvbmx5IGNvbnRleHQ6IFR1aURpYWxvZzxNb2RhbE9wdGlvbiwgYm9vbGVhbj4pIHt9XHJcblxyXG4gIHB1YmxpYyBvbkNsaWNrKHJlc3BvbnNlOiBib29sZWFuKTogdm9pZCB7XHJcbiAgICB0aGlzLmNvbnRleHQuY29tcGxldGVXaXRoKHJlc3BvbnNlKTtcclxuICB9XHJcbn1cclxuIiwiXHQ8ZGl2IGNsYXNzPVwibWl4LW1vZGFsXCJcclxuXHQgICAgIHN0eWxlPVwiLS1tb2RhbC1ib3hzaGFkb3c6IHt7Y29udGV4dC5ib3JkZXJTaGFkb3dDb2xvcn19XCI+XHJcblx0ICA8aDMgW2lkXT1cImNvbnRleHQuaWRcIj57eyBjb250ZXh0LmhlYWRpbmcgfX08L2gzPlxyXG5cdCAgPHNlY3Rpb24gW2NvbnRlbnRdPVwiY29udGV4dC5jb250ZW50XCJcclxuXHQgICAgICAgICAgIFtjb250ZXh0XT1cImNvbnRleHRcIlxyXG5cdCAgICAgICAgICAgcG9seW1vcnBoZXVzLW91dGxldD48L3NlY3Rpb24+XHJcblx0ICA8cCBjbGFzcz1cIm1peC1tb2RhbF9fYnV0dG9uc1wiPlxyXG5cdCAgICA8YnV0dG9uIGNsYXNzPVwidHVpLXNwYWNlX3JpZ2h0LTNcIlxyXG5cdCAgICAgICAgICAgIChjbGljayk9XCJvbkNsaWNrKHRydWUpXCJcclxuXHQgICAgICAgICAgICB0dWlCdXR0b24+XHJcblx0ICAgICAge3sgY29udGV4dC5idXR0b25zWzBdIH19XHJcblx0ICAgIDwvYnV0dG9uPlxyXG5cclxuXHQgICAgPGJ1dHRvbiAoY2xpY2spPVwib25DbGljayhmYWxzZSlcIlxyXG5cdCAgICAgICAgICAgIHR1aUJ1dHRvblxyXG5cdCAgICAgICAgICAgIGFwcGVhcmFuY2U9XCJzZWNvbmRhcnlcIj5cclxuXHQgICAgICB7eyBjb250ZXh0LmJ1dHRvbnNbMV0gfX1cclxuXHQgICAgPC9idXR0b24+XHJcblx0ICA8L3A+XHJcblx0PC9kaXY+XHJcbiJdfQ==