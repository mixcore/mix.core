import { Component, Inject } from '@angular/core';
import { ActivatedRoute, NavigationEnd, PRIMARY_OUTLET, Router, RouterModule } from '@angular/router';
import { TuiLinkModule } from '@taiga-ui/core';
import { TuiBreadcrumbsModule } from '@taiga-ui/kit';
import { filter, startWith } from 'rxjs';
import { AuthApiService } from '../../services';
import { ShareModule } from '../../share.module';
import { ModalService } from '../modal/modal.service';
import { HeaderMenuService } from './header-menu.service';
import * as i0 from "@angular/core";
import * as i1 from "../../services";
import * as i2 from "./header-menu.service";
import * as i3 from "@angular/router";
import * as i4 from "@angular/common";
import * as i5 from "@taiga-ui/kit";
import * as i6 from "@taiga-ui/core";
import * as i7 from "../modal/modal.service";
function HeaderMenuComponent_ng_container_3_a_1_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "a", 9);
    i0.ɵɵtext(1);
    i0.ɵɵelementEnd();
} if (rf & 2) {
    const item_r3 = i0.ɵɵnextContext().$implicit;
    i0.ɵɵproperty("routerLink", item_r3.routerLink);
    i0.ɵɵadvance(1);
    i0.ɵɵtextInterpolate1(" ", item_r3.caption, " ");
} }
function HeaderMenuComponent_ng_container_3_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵtemplate(1, HeaderMenuComponent_ng_container_3_a_1_Template, 2, 2, "a", 8);
    i0.ɵɵelementContainerEnd();
} }
function HeaderMenuComponent_ng_template_7_div_0_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 14);
    i0.ɵɵelement(1, "tui-avatar", 15);
    i0.ɵɵelementStart(2, "div", 16)(3, "span");
    i0.ɵɵtext(4, "Hi");
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(5, "strong");
    i0.ɵɵtext(6);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(7, "button", 17);
    i0.ɵɵtext(8, "View your profile");
    i0.ɵɵelementEnd()()();
} if (rf & 2) {
    const user_r8 = ctx.ngIf;
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("avatarUrl", user_r8.avatar || null)("rounded", true)("text", user_r8.userName);
    i0.ɵɵadvance(5);
    i0.ɵɵtextInterpolate(user_r8.userName);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("pseudo", true);
} }
function HeaderMenuComponent_ng_template_7_Template(rf, ctx) { if (rf & 1) {
    const _r10 = i0.ɵɵgetCurrentView();
    i0.ɵɵtemplate(0, HeaderMenuComponent_ng_template_7_div_0_Template, 9, 5, "div", 10);
    i0.ɵɵpipe(1, "async");
    i0.ɵɵelement(2, "div", 11);
    i0.ɵɵelementStart(3, "p", 12)(4, "button", 13);
    i0.ɵɵlistener("click", function HeaderMenuComponent_ng_template_7_Template_button_click_4_listener() { i0.ɵɵrestoreView(_r10); const ctx_r9 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r9.logout()); });
    i0.ɵɵtext(5, "Logout");
    i0.ɵɵelementEnd()();
} if (rf & 2) {
    const ctx_r2 = i0.ɵɵnextContext();
    i0.ɵɵproperty("ngIf", i0.ɵɵpipeBind1(1, 1, ctx_r2.user$));
} }
export class HeaderMenuComponent {
    constructor(authService, headerService, router, modalService, activatedRoute) {
        this.authService = authService;
        this.headerService = headerService;
        this.router = router;
        this.modalService = modalService;
        this.activatedRoute = activatedRoute;
        this.user$ = this.authService.user$;
        this.breadcrumb = [];
        this._registerRouterChange();
    }
    logout() {
        this.modalService.confirm('Do you want to sign out ?').subscribe(ok => {
            if (ok)
                this.authService.logout(() => this.router.navigateByUrl('/auth/login'));
        });
    }
    _registerRouterChange() {
        try {
            this.router.events
                .pipe(filter(e => e instanceof NavigationEnd), startWith(true))
                .subscribe(() => {
                this.breadcrumb = this._getBreadcrumbs(this.activatedRoute.root);
            });
        }
        catch (e) {
            throw new Error(`Error when try to load breadcrumb.`);
        }
    }
    _getBreadcrumbs(route, url = '', breadcrumbs = []) {
        const children = route.children;
        if (children.length === 0)
            return breadcrumbs;
        for (const child of children) {
            if (child.outlet === PRIMARY_OUTLET && !!child.snapshot) {
                const routeUrl = child.snapshot.url
                    .map(segment => segment.path)
                    .filter(path => path)
                    .join('/');
                const nextUrl = routeUrl ? `${url}/${routeUrl}` : url;
                const breadcrumbLabel = child.snapshot.data['title'];
                if (routeUrl && breadcrumbLabel) {
                    const breadcrumb = {
                        caption: breadcrumbLabel,
                        params: child.snapshot.params,
                        routerLink: nextUrl
                    };
                    breadcrumbs.push(breadcrumb);
                }
                return this._getBreadcrumbs(child, nextUrl, breadcrumbs);
            }
        }
        return breadcrumbs;
    }
}
HeaderMenuComponent.ɵfac = function HeaderMenuComponent_Factory(t) { return new (t || HeaderMenuComponent)(i0.ɵɵdirectiveInject(i1.AuthApiService), i0.ɵɵdirectiveInject(i2.HeaderMenuService), i0.ɵɵdirectiveInject(i3.Router), i0.ɵɵdirectiveInject(ModalService), i0.ɵɵdirectiveInject(i3.ActivatedRoute)); };
HeaderMenuComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: HeaderMenuComponent, selectors: [["mix-header-menu"]], standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 9, vars: 4, consts: [[1, "header-menu"], [1, "header-menu__title"], [3, "size"], [4, "ngFor", "ngForOf"], [1, "header-menu__right-menu"], [1, "header-menu__avatar", 3, "content"], ["text", "Admin", "size", "s", 3, "rounded"], ["profileTemplate", ""], ["tuiLink", "", 3, "routerLink", 4, "tuiBreadcrumb"], ["tuiLink", "", 3, "routerLink"], ["class", "header-menu__user-info", 4, "ngIf"], [1, "header-menu__separator"], [1, "header-menu__item", "tui-space_vertical-4", "tui-space_horizontal-4"], ["tuiLink", "", "icon", "tuiIconLogoutLarge", "iconAlign", "right", 3, "click"], [1, "header-menu__user-info"], ["size", "l", 1, "header-menu__avatar", 3, "avatarUrl", "rounded", "text"], [1, "title"], ["tuiLink", "", 3, "pseudo"]], template: function HeaderMenuComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1)(2, "tui-breadcrumbs", 2);
        i0.ɵɵtemplate(3, HeaderMenuComponent_ng_container_3_Template, 2, 0, "ng-container", 3);
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(4, "div", 4)(5, "tui-hosted-dropdown", 5);
        i0.ɵɵelement(6, "tui-avatar", 6);
        i0.ɵɵtemplate(7, HeaderMenuComponent_ng_template_7_Template, 6, 3, "ng-template", null, 7, i0.ɵɵtemplateRefExtractor);
        i0.ɵɵelementEnd()()();
    } if (rf & 2) {
        const _r1 = i0.ɵɵreference(8);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("size", "l");
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngForOf", ctx.breadcrumb);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("content", _r1);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("rounded", true);
    } }, dependencies: [ShareModule, i4.NgForOf, i4.NgIf, i5.TuiAvatarComponent, i6.TuiHostedDropdownComponent, i6.TuiLinkComponent, i4.AsyncPipe, TuiBreadcrumbsModule, i5.TuiBreadcrumbsWrapperComponent, i5.TuiBreadcrumbDirective, RouterModule, i3.RouterLinkWithHref, TuiLinkModule], styles: [".header-menu[_ngcontent-%COMP%]{width:100%;box-sizing:border-box;display:flex;height:var(--mix-header-height);border-bottom:1px solid var(--tui-base-04);align-items:center;padding:0 15px;justify-content:space-between}.header-menu__right-menu[_ngcontent-%COMP%]{margin-left:auto;display:flex}.header-menu__search[_ngcontent-%COMP%]{width:250px}.header-menu__title[_ngcontent-%COMP%]{font-size:16px;font-weight:500}.header-menu__user-info[_ngcontent-%COMP%]{padding:1rem 1rem 0rem;display:flex;width:200px;align-items:center}.header-menu__user-info[_ngcontent-%COMP%] > tui-avatar[_ngcontent-%COMP%]{margin-right:15px}.header-menu__user-info[_ngcontent-%COMP%] > .title[_ngcontent-%COMP%] > span[_ngcontent-%COMP%]{margin-right:5px}.header-menu__user-info[_ngcontent-%COMP%] > .title[_ngcontent-%COMP%] > strong[_ngcontent-%COMP%]{display:inline-block}.header-menu__user-info[_ngcontent-%COMP%] > .title[_ngcontent-%COMP%] > strong[_ngcontent-%COMP%]:first-letter{text-transform:uppercase}.header-menu__avatar[_ngcontent-%COMP%]{margin-left:15px;cursor:pointer}.header-menu__separator[_ngcontent-%COMP%]{display:block;content:\"\";height:.5px;width:calc(100% - 20px);background-color:var(--tui-base-04);margin:1rem 10px}.header-menu__item[_ngcontent-%COMP%] > button[tuiLink][_ngcontent-%COMP%]{color:var(--tui-text-01);width:100%;display:flex;justify-content:space-between}.header-menu__item[_ngcontent-%COMP%] > button[tuiLink][_ngcontent-%COMP%] > tui-svg[_ngcontent-%COMP%]{margin-right:2rem!important}"] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(HeaderMenuComponent, [{
        type: Component,
        args: [{ selector: 'mix-header-menu', standalone: true, imports: [ShareModule, TuiBreadcrumbsModule, RouterModule, TuiLinkModule], template: "<div class=\"header-menu\">\r\n    <div class=\"header-menu__title\">\r\n        <tui-breadcrumbs [size]=\"'l'\">\r\n            <ng-container *ngFor=\"let item of breadcrumb\">\r\n                <a *tuiBreadcrumb [routerLink]=\"item.routerLink\" tuiLink>\r\n          {{ item.caption }}\r\n        </a>\r\n            </ng-container>\r\n        </tui-breadcrumbs>\r\n    </div>\r\n\r\n    <div class=\"header-menu__right-menu\">\r\n        <!-- <tui-input class=\"header-menu__search\"\r\n               tuiTextfieldSize=\"s\">\r\n      Search\r\n      <input type=\"text\"\r\n             tuiTextfield>\r\n    </tui-input> -->\r\n\r\n        <tui-hosted-dropdown class=\"header-menu__avatar\" [content]=\"profileTemplate\">\r\n            <tui-avatar [rounded]=\"true\" text=\"Admin\" size=\"s\"></tui-avatar>\r\n            <ng-template #profileTemplate let-activeZone>\r\n                <div *ngIf=\"user$ | async as user\" class=\"header-menu__user-info\">\r\n                    <tui-avatar class=\"header-menu__avatar\" [avatarUrl]=\"user.avatar || null\" [rounded]=\"true\" [text]=\"user.userName\" size=\"l\"></tui-avatar>\r\n                    <div class=\"title\">\r\n                        <span>Hi</span> <strong>{{ user.userName }}</strong>\r\n                        <button [pseudo]=\"true\" tuiLink>View your profile</button>\r\n                    </div>\r\n                </div>\r\n                <div class=\"header-menu__separator\"></div>\r\n                <p class=\"header-menu__item tui-space_vertical-4 tui-space_horizontal-4\">\r\n                    <button (click)=\"logout()\" tuiLink icon=\"tuiIconLogoutLarge\" iconAlign=\"right\">Logout</button>\r\n                </p>\r\n            </ng-template>\r\n        </tui-hosted-dropdown>\r\n    </div>\r\n</div>", styles: [".header-menu{width:100%;box-sizing:border-box;display:flex;height:var(--mix-header-height);border-bottom:1px solid var(--tui-base-04);align-items:center;padding:0 15px;justify-content:space-between}.header-menu__right-menu{margin-left:auto;display:flex}.header-menu__search{width:250px}.header-menu__title{font-size:16px;font-weight:500}.header-menu__user-info{padding:1rem 1rem 0rem;display:flex;width:200px;align-items:center}.header-menu__user-info>tui-avatar{margin-right:15px}.header-menu__user-info>.title>span{margin-right:5px}.header-menu__user-info>.title>strong{display:inline-block}.header-menu__user-info>.title>strong:first-letter{text-transform:uppercase}.header-menu__avatar{margin-left:15px;cursor:pointer}.header-menu__separator{display:block;content:\"\";height:.5px;width:calc(100% - 20px);background-color:var(--tui-base-04);margin:1rem 10px}.header-menu__item>button[tuiLink]{color:var(--tui-text-01);width:100%;display:flex;justify-content:space-between}.header-menu__item>button[tuiLink]>tui-svg{margin-right:2rem!important}\n"] }]
    }], function () { return [{ type: i1.AuthApiService }, { type: i2.HeaderMenuService }, { type: i3.Router }, { type: i7.ModalService, decorators: [{
                type: Inject,
                args: [ModalService]
            }] }, { type: i3.ActivatedRoute }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaGVhZGVyLW1lbnUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL2hlYWRlci1tZW51L2hlYWRlci1tZW51LmNvbXBvbmVudC50cyIsIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9oZWFkZXItbWVudS9oZWFkZXItbWVudS5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRCxPQUFPLEVBQUUsY0FBYyxFQUFFLGFBQWEsRUFBVSxjQUFjLEVBQUUsTUFBTSxFQUFFLFlBQVksRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQzlHLE9BQU8sRUFBRSxhQUFhLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUMvQyxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDckQsT0FBTyxFQUFFLE1BQU0sRUFBRSxTQUFTLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFFekMsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2hELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUNqRCxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDdEQsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sdUJBQXVCLENBQUM7Ozs7Ozs7Ozs7SUNMMUMsNEJBQXlEO0lBQy9ELFlBQ0Y7SUFBQSxpQkFBSTs7O0lBRnNCLCtDQUE4QjtJQUN0RCxlQUNGO0lBREUsZ0RBQ0Y7OztJQUhJLDZCQUE4QztJQUMxQywrRUFFSjtJQUNBLDBC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O1xyXG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZSwgTmF2aWdhdGlvbkVuZCwgUGFyYW1zLCBQUklNQVJZX09VVExFVCwgUm91dGVyLCBSb3V0ZXJNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyBUdWlMaW5rTW9kdWxlIH0gZnJvbSAnQHRhaWdhLXVpL2NvcmUnO1xyXG5pbXBvcnQgeyBUdWlCcmVhZGNydW1ic01vZHVsZSB9IGZyb20gJ0B0YWlnYS11aS9raXQnO1xyXG5pbXBvcnQgeyBmaWx0ZXIsIHN0YXJ0V2l0aCB9IGZyb20gJ3J4anMnO1xyXG5cclxuaW1wb3J0IHsgQXV0aEFwaVNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcyc7XHJcbmltcG9ydCB7IFNoYXJlTW9kdWxlIH0gZnJvbSAnLi4vLi4vc2hhcmUubW9kdWxlJztcclxuaW1wb3J0IHsgTW9kYWxTZXJ2aWNlIH0gZnJvbSAnLi4vbW9kYWwvbW9kYWwuc2VydmljZSc7XHJcbmltcG9ydCB7IEhlYWRlck1lbnVTZXJ2aWNlIH0gZnJvbSAnLi9oZWFkZXItbWVudS5zZXJ2aWNlJztcclxuXHJcbmV4cG9ydCBpbnRlcmZhY2UgQnJlYWRjcnVtYk9wdGlvbiB7XHJcbiAgY2FwdGlvbjogc3RyaW5nO1xyXG4gIHBhcmFtczogUGFyYW1zO1xyXG4gIHJvdXRlckxpbms6IHN0cmluZztcclxufVxyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdtaXgtaGVhZGVyLW1lbnUnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9oZWFkZXItbWVudS5jb21wb25lbnQuaHRtbCcsXHJcbiAgc3R5bGVVcmxzOiBbJy4vaGVhZGVyLW1lbnUuY29tcG9uZW50LnNjc3MnXSxcclxuICBzdGFuZGFsb25lOiB0cnVlLFxyXG4gIGltcG9ydHM6IFtTaGFyZU1vZHVsZSwgVHVpQnJlYWRjcnVtYnNNb2R1bGUsIFJvdXRlck1vZHVsZSwgVHVpTGlua01vZHVsZV1cclxufSlcclxuZXhwb3J0IGNsYXNzIEhlYWRlck1lbnVDb21wb25lbnQge1xyXG4gIHB1YmxpYyB1c2VyJCA9IHRoaXMuYXV0aFNlcnZpY2UudXNlciQ7XHJcbiAgcHVibGljIGJyZWFkY3J1bWI6IEJyZWFkY3J1bWJPcHRpb25bXSA9IFtdO1xyXG5cclxuICBjb25zdHJ1Y3RvcihcclxuICAgIHB1YmxpYyBhdXRoU2VydmljZTogQXV0aEFwaVNlcnZpY2UsXHJcbiAgICBwdWJsaWMgaGVhZGVyU2VydmljZTogSGVhZGVyTWVudVNlcnZpY2UsXHJcbiAgICBwcml2YXRlIHJvdXRlcjogUm91dGVyLFxyXG4gICAgQEluamVjdChNb2RhbFNlcnZpY2UpIHByaXZhdGUgcmVhZG9ubHkgbW9kYWxTZXJ2aWNlOiBNb2RhbFNlcnZpY2UsXHJcbiAgICBwcml2YXRlIGFjdGl2YXRlZFJvdXRlOiBBY3RpdmF0ZWRSb3V0ZVxyXG4gICkge1xyXG4gICAgdGhpcy5fcmVnaXN0ZXJSb3V0ZXJDaGFuZ2UoKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBsb2dvdXQoKTogdm9pZCB7XHJcbiAgICB0aGlzLm1vZGFsU2VydmljZS5jb25maXJtKCdEbyB5b3Ugd2FudCB0byBzaWduIG91dCA/Jykuc3Vic2NyaWJlKG9rID0+IHtcclxuICAgICAgaWYgKG9rKSB0aGlzLmF1dGhTZXJ2aWNlLmxvZ291dCgoKSA9PiB0aGlzLnJvdXRlci5uYXZpZ2F0ZUJ5VXJsKCcvYXV0aC9sb2dpbicpKTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBfcmVnaXN0ZXJSb3V0ZXJDaGFuZ2UoKTogdm9pZCB7XHJcbiAgICB0cnkge1xyXG4gICAgICB0aGlzLnJvdXRlci5ldmVudHNcclxuICAgICAgICAucGlwZShcclxuICAgICAgICAgIGZpbHRlcihlID0+IGUgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRW5kKSxcclxuICAgICAgICAgIHN0YXJ0V2l0aCh0cnVlKVxyXG4gICAgICAgIClcclxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgICAgIHRoaXMuYnJlYWRjcnVtYiA9IHRoaXMuX2dldEJyZWFkY3J1bWJzKHRoaXMuYWN0aXZhdGVkUm91dGUucm9vdCk7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9IGNhdGNoIChlKSB7XHJcbiAgICAgIHRocm93IG5ldyBFcnJvcihgRXJyb3Igd2hlbiB0cnkgdG8gbG9hZCBicmVhZGNydW1iLmApO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBfZ2V0QnJlYWRjcnVtYnMocm91dGU6IEFjdGl2YXRlZFJvdXRlLCB1cmw6IHN0cmluZyA9ICcnLCBicmVhZGNydW1iczogQnJlYWRjcnVtYk9wdGlvbltdID0gW10pOiBCcmVhZGNydW1iT3B0aW9uW10ge1xyXG4gICAgY29uc3QgY2hpbGRyZW46IEFjdGl2YXRlZFJvdXRlW10gPSByb3V0ZS5jaGlsZHJlbjtcclxuXHJcbiAgICBpZiAoY2hpbGRyZW4ubGVuZ3RoID09PSAwKSByZXR1cm4gYnJlYWRjcnVtYnM7XHJcblxyXG4gICAgZm9yIChjb25zdCBjaGlsZCBvZiBjaGlsZHJlbikge1xyXG4gICAgICBpZiAoY2hpbGQub3V0bGV0ID09PSBQUklNQVJZX09VVExFVCAmJiAhIWNoaWxkLnNuYXBzaG90KSB7XHJcbiAgICAgICAgY29uc3Qgcm91dGVVcmw6IHN0cmluZyA9IGNoaWxkLnNuYXBzaG90LnVybFxyXG4gICAgICAgICAgLm1hcChzZWdtZW50ID0+IHNlZ21lbnQucGF0aClcclxuICAgICAgICAgIC5maWx0ZXIocGF0aCA9PiBwYXRoKVxyXG4gICAgICAgICAgLmpvaW4oJy8nKTtcclxuXHJcbiAgICAgICAgY29uc3QgbmV4dFVybCA9IHJvdXRlVXJsID8gYCR7dXJsfS8ke3JvdXRlVXJsfWAgOiB1cmw7XHJcbiAgICAgICAgY29uc3QgYnJlYWRjcnVtYkxhYmVsID0gY2hpbGQuc25hcHNob3QuZGF0YVsndGl0bGUnXTtcclxuXHJcbiAgICAgICAgaWYgKHJvdXRlVXJsICYmIGJyZWFkY3J1bWJMYWJlbCkge1xyXG4gICAgICAgICAgY29uc3QgYnJlYWRjcnVtYjogQnJlYWRjcnVtYk9wdGlvbiA9IHtcclxuICAgICAgICAgICAgY2FwdGlvbjogYnJlYWRjcnVtYkxhYmVsLFxyXG4gICAgICAgICAgICBwYXJhbXM6IGNoaWxkLnNuYXBzaG90LnBhcmFtcyxcclxuICAgICAgICAgICAgcm91dGVyTGluazogbmV4dFVybFxyXG4gICAgICAgICAgfTtcclxuICAgICAgICAgIGJyZWFkY3J1bWJzLnB1c2goYnJlYWRjcnVtYik7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICByZXR1cm4gdGhpcy5fZ2V0QnJlYWRjcnVtYnMoY2hpbGQsIG5leHRVcmwsIGJyZWFkY3J1bWJzKTtcclxuICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBicmVhZGNydW1icztcclxuICB9XHJcbn1cclxuIiwiPGRpdiBjbGFzcz1cImhlYWRlci1tZW51XCI+XHJcbiAgICA8ZGl2IGNsYXNzPVwiaGVhZGVyLW1lbnVfX3RpdGxlXCI+XHJcbiAgICAgICAgPHR1aS1icmVhZGNydW1icyBbc2l6ZV09XCInbCdcIj5cclxuICAgICAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdGb3I9XCJsZXQgaXRlbSBvZiBicmVhZGNydW1iXCI+XHJcbiAgICAgICAgICAgICAgICA8YSAqdHVpQnJlYWRjcnVtYiBbcm91dGVyTGlua109XCJpdGVtLnJvdXRlckxpbmtcIiB0dWlMaW5rPlxyXG4gICAgICAgICAge3sgaXRlbS5jYXB0aW9uIH19XHJcbiAgICAgICAgPC9hPlxyXG4gICAgICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgICA8L3R1aS1icmVhZGNydW1icz5cclxuICAgIDwvZGl2PlxyXG5cclxuICAgIDxkaXYgY2xhc3M9XCJoZWFkZXItbWVudV9fcmlnaHQtbWVudVwiPlxyXG4gICAgICAgIDwhLS0gPHR1aS1pbnB1dCBjbGFzcz1cImhlYWRlci1tZW51X19zZWFyY2hcIlxyXG4gICAgICAgICAgICAgICB0dWlUZXh0ZmllbGRTaXplPVwic1wiPlxyXG4gICAgICBTZWFyY2hcclxuICAgICAgPGlucHV0IHR5cGU9XCJ0ZXh0XCJcclxuICAgICAgICAgICAgIHR1aVRleHRmaWVsZD5cclxuICAgIDwvdHVpLWlucHV0PiAtLT5cclxuXHJcbiAgICAgICAgPHR1aS1ob3N0ZWQtZHJvcGRvd24gY2xhc3M9XCJoZWFkZXItbWVudV9fYXZhdGFyXCIgW2NvbnRlbnRdPVwicHJvZmlsZVRlbXBsYXRlXCI+XHJcbiAgICAgICAgICAgIDx0dWktYXZhdGFyIFtyb3VuZGVkXT1cInRydWVcIiB0ZXh0PVwiQWRtaW5cIiBzaXplPVwic1wiPjwvdHVpLWF2YXRhcj5cclxuICAgICAgICAgICAgPG5nLXRlbXBsYXRlICNwcm9maWxlVGVtcGxhdGUgbGV0LWFjdGl2ZVpvbmU+XHJcbiAgICAgICAgICAgICAgICA8ZGl2ICpuZ0lmPVwidXNlciQgfCBhc3luYyBhcyB1c2VyXCIgY2xhc3M9XCJoZWFkZXItbWVudV9fdXNlci1pbmZvXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgPHR1aS1hdmF0YXIgY2xhc3M9XCJoZWFkZXItbWVudV9fYXZhdGFyXCIgW2F2YXRhclVybF09XCJ1c2VyLmF2YXRhciB8fCBudWxsXCIgW3JvdW5kZWRdPVwidHJ1ZVwiIFt0ZXh0XT1cInVzZXIudXNlck5hbWVcIiBzaXplPVwibFwiPjwvdHVpLWF2YXRhcj5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzPVwidGl0bGVcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPHNwYW4+SGk8L3NwYW4+IDxzdHJvbmc+e3sgdXNlci51c2VyTmFtZSB9fTwvc3Ryb25nPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIFtwc2V1ZG9dPVwidHJ1ZVwiIHR1aUxpbms+VmlldyB5b3VyIHByb2ZpbGU8L2J1dHRvbj5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzcz1cImhlYWRlci1tZW51X19zZXBhcmF0b3JcIj48L2Rpdj5cclxuICAgICAgICAgICAgICAgIDxwIGNsYXNzPVwiaGVhZGVyLW1lbnVfX2l0ZW0gdHVpLXNwYWNlX3ZlcnRpY2FsLTQgdHVpLXNwYWNlX2hvcml6b250YWwtNFwiPlxyXG4gICAgICAgICAgICAgICAgICAgIDxidXR0b24gKGNsaWNrKT1cImxvZ291dCgpXCIgdHVpTGluayBpY29uPVwidHVpSWNvbkxvZ291dExhcmdlXCIgaWNvbkFsaWduPVwicmlnaHRcIj5Mb2dvdXQ8L2J1dHRvbj5cclxuICAgICAgICAgICAgICAgIDwvcD5cclxuICAgICAgICAgICAgPC9uZy10ZW1wbGF0ZT5cclxuICAgICAgICA8L3R1aS1ob3N0ZWQtZHJvcGRvd24+XHJcbiAgICA8L2Rpdj5cclxuPC9kaXY+Il19