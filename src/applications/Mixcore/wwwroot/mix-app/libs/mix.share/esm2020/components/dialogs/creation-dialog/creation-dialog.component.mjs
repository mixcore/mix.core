import { Component, Inject, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TuiAlertService } from '@taiga-ui/core';
import { switchMap } from 'rxjs';
import { slideAnimation } from '../../../animations';
import { MixPageApiService, MixPostApiService, PortalSidebarControlService } from '../../../services';
import { MixModuleApiService } from '../../../services/api/mix-module-api.service';
import { ShareModule } from '../../../share.module';
import { FormUtils, StringUtils } from '../../../utils';
import { MixModuleSelectComponent } from '../../module-selects-list/module-select.component';
import * as i0 from "@angular/core";
import * as i1 from "../../../services";
import * as i2 from "../../../services/api/mix-module-api.service";
import * as i3 from "@angular/common";
import * as i4 from "@angular/forms";
import * as i5 from "@taiga-ui/kit";
import * as i6 from "@taiga-ui/core";
import * as i7 from "@taiga-ui/addon-editor";
import * as i8 from "angular-tabler-icons";
function CreationDialogComponent_tui_data_list_wrapper_6_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelement(0, "tui-data-list-wrapper", 22);
} if (rf & 2) {
    const ctx_r0 = i0.ɵɵnextContext();
    i0.ɵɵproperty("items", ctx_r0.items);
} }
function CreationDialogComponent_button_21_Template(rf, ctx) { if (rf & 1) {
    const _r10 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "button", 15);
    i0.ɵɵlistener("click", function CreationDialogComponent_button_21_Template_button_click_0_listener() { i0.ɵɵrestoreView(_r10); const ctx_r9 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r9.activeTabIndex = 1); });
    i0.ɵɵtext(1, " Seo ");
    i0.ɵɵelementEnd();
} }
function CreationDialogComponent_button_22_Template(rf, ctx) { if (rf & 1) {
    const _r12 = i0.ɵɵgetCurrentView();
    i0.ɵɵelementStart(0, "button", 15);
    i0.ɵɵlistener("click", function CreationDialogComponent_button_22_Template_button_click_0_listener() { i0.ɵɵrestoreView(_r12); const ctx_r11 = i0.ɵɵnextContext(); return i0.ɵɵresetView(ctx_r11.activeTabIndex = 2); });
    i0.ɵɵtext(1, " Related ");
    i0.ɵɵelementEnd();
} }
function CreationDialogComponent_ng_container_23_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "div", 23)(2, "div", 24)(3, "label", 25)(4, "tui-input", 26);
    i0.ɵɵelement(5, "input", 27);
    i0.ɵɵelementEnd();
    i0.ɵɵelement(6, "tui-field-error", 28);
    i0.ɵɵelementEnd()();
    i0.ɵɵelementStart(7, "div", 24)(8, "label", 25)(9, "tui-input", 29);
    i0.ɵɵelement(10, "input", 27);
    i0.ɵɵelementEnd();
    i0.ɵɵelement(11, "tui-field-error", 30);
    i0.ɵɵelementEnd()();
    i0.ɵɵelementStart(12, "div", 24)(13, "label", 25);
    i0.ɵɵelement(14, "tui-text-area", 31);
    i0.ɵɵelementEnd()();
    i0.ɵɵelementStart(15, "div", 24)(16, "label", 25);
    i0.ɵɵelement(17, "tui-editor", 32);
    i0.ɵɵelementEnd()()();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const ctx_r3 = i0.ɵɵnextContext();
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("formGroup", ctx_r3.form);
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("label", "Title:");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiTextfieldLabelOutside", true);
    i0.ɵɵadvance(4);
    i0.ɵɵproperty("label", "System name:");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiTextfieldLabelOutside", true);
    i0.ɵɵadvance(4);
    i0.ɵɵproperty("label", "Excerpt:");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiTextfieldLabelOutside", true);
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("label", "Description:");
} }
function CreationDialogComponent_ng_container_24_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelementStart(1, "div", 23)(2, "div", 24)(3, "label", 25)(4, "tui-input", 33);
    i0.ɵɵelement(5, "input", 27);
    i0.ɵɵelementEnd()()();
    i0.ɵɵelementStart(6, "div", 24)(7, "label", 25);
    i0.ɵɵelement(8, "tui-text-area", 34);
    i0.ɵɵelementEnd()();
    i0.ɵɵelementStart(9, "div", 24)(10, "label", 25)(11, "tui-input", 35);
    i0.ɵɵelement(12, "input", 27);
    i0.ɵɵelementEnd()()();
    i0.ɵɵelementStart(13, "div", 24)(14, "label", 25);
    i0.ɵɵelement(15, "tui-text-area", 36);
    i0.ɵɵelementEnd()()();
    i0.ɵɵelementContainerEnd();
} if (rf & 2) {
    const ctx_r4 = i0.ɵɵnextContext();
    const _r7 = i0.ɵɵreference(30);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("formGroup", ctx_r4.form);
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("label", "Friendly Title:");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHintContent", "Google displays the entire title on the search results, which has 63 characters.")("tuiTextfieldLabelOutside", true);
    i0.ɵɵadvance(3);
    i0.ɵɵproperty("label", "Meta Description:");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHintContent", "Do make sure your most important keywords for the webpage show up in the meta description. Often search engines will highlight in bold where it finds the searchers query in your snippet.")("tuiTextfieldLabelOutside", true);
    i0.ɵɵadvance(2);
    i0.ɵɵproperty("label", "Friendly URL:");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHintContent", _r7)("tuiTextfieldLabelOutside", true);
    i0.ɵɵadvance(3);
    i0.ɵɵproperty("label", "Meta Keywords:");
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("tuiHintContent", "Do make sure your most important keywords for the webpage show up in the meta description. Often search engines will highlight in bold where it finds the searchers query in your snippet.")("tuiTextfieldLabelOutside", true);
} }
function CreationDialogComponent_ng_container_25_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementContainerStart(0);
    i0.ɵɵelement(1, "mix-module-select");
    i0.ɵɵelementContainerEnd();
} }
function CreationDialogComponent_div_26_ng_template_4_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵtext(0, " Development kit consisting of the low level tools and abstractions used to develop Taiga UI Angular entities ");
} }
function CreationDialogComponent_div_26_ng_template_7_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", null, 41);
    i0.ɵɵtext(2, " Basic elements needed to develop components, directives and more using Taiga UI design system ");
    i0.ɵɵelementEnd();
} }
function CreationDialogComponent_div_26_ng_template_10_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵtext(0, " The main set of components used to build Taiga UI based Angular applications ");
} }
function CreationDialogComponent_div_26_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div", 37)(1, "tui-accordion", 38)(2, "tui-accordion-item", 39);
    i0.ɵɵtext(3, " Publishing ");
    i0.ɵɵtemplate(4, CreationDialogComponent_div_26_ng_template_4_Template, 1, 0, "ng-template", 40);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(5, "tui-accordion-item", 39);
    i0.ɵɵtext(6, " Language ");
    i0.ɵɵtemplate(7, CreationDialogComponent_div_26_ng_template_7_Template, 3, 0, "ng-template", 40);
    i0.ɵɵelementEnd();
    i0.ɵɵelementStart(8, "tui-accordion-item", 39);
    i0.ɵɵtext(9, " Images ");
    i0.ɵɵtemplate(10, CreationDialogComponent_div_26_ng_template_10_Template, 1, 0, "ng-template", 40);
    i0.ɵɵelementEnd()()();
} if (rf & 2) {
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("rounded", false);
    i0.ɵɵadvance(1);
    i0.ɵɵproperty("size", "s");
    i0.ɵɵadvance(3);
    i0.ɵɵproperty("size", "s");
    i0.ɵɵadvance(3);
    i0.ɵɵproperty("size", "s");
} }
function CreationDialogComponent_ng_template_29_Template(rf, ctx) { if (rf & 1) {
    i0.ɵɵelementStart(0, "div");
    i0.ɵɵtext(1, " Easy to read: Users and search engines should be able to understand what is on each page just by looking at the URL. ");
    i0.ɵɵelement(2, "br")(3, "br");
    i0.ɵɵtext(4, " Keyword-rich: Keywords still matter and your target queries should be within URLs. Just be wary of overkill; extending URLs just to include more keywords is a bad idea.");
    i0.ɵɵelement(5, "br")(6, "br");
    i0.ɵɵtext(7, " Consistent: There are multiple ways to create an SEO-friendly URL structure on any site. It\u2019s essential that, whatever logic you choose to follow, it is applied consistently across the site. ");
    i0.ɵɵelementEnd();
} }
const _c0 = function (a0) { return { "--large": a0 }; };
export class CreationDialogComponent {
    constructor(postApi, moduleApi, pageApi, alertService, sidebarControl) {
        this.postApi = postApi;
        this.moduleApi = moduleApi;
        this.pageApi = pageApi;
        this.alertService = alertService;
        this.sidebarControl = sidebarControl;
        this.type = 'Post';
        this.items = ['Post', 'Page', 'Module'];
        this.form = new FormGroup({
            title: new FormControl('', Validators.required),
            systemName: new FormControl('', Validators.required),
            excerpt: new FormControl(''),
            description: new FormControl(''),
            seoName: new FormControl(''),
            seoTitle: new FormControl(''),
            seoDescription: new FormControl(''),
            seoKeywords: new FormControl(''),
            seoSource: new FormControl('')
        });
        this.goToPostAfterCreate = true;
        this.closeDialogAfterCreate = true;
        this.loading = false;
        this.initialize = false;
        this.activeTabIndex = 0;
        this.showRightSideMenu = true;
        this.largeMode = false;
    }
    get canShowRelatedModule() {
        return this.type === 'Page' || this.type === 'Post';
    }
    get canShowSEOConfig() {
        return this.type === 'Page' || this.type === 'Post';
    }
    ngOnInit() {
        this.form.controls['title'].valueChanges.subscribe((title) => {
            this.form.controls['systemName'].patchValue(StringUtils.textToSystemName(title));
        });
    }
    submitForm() {
        if (!FormUtils.validateForm(this.form))
            return;
        switch (this.type) {
            case 'Post':
                this.createNewPost();
                break;
            case 'Module':
                this.createNewModule();
                break;
            case 'Page':
                this.createNewPage();
                break;
        }
    }
    createNewPost() {
        this.handleBeforeCreate();
        this.postApi
            .getDefaultPostTemplate()
            .pipe(switchMap(result => {
            const form = this.form.getRawValue();
            const post = {
                ...result,
                title: form['title'],
                excerpt: form['excerpt'],
                content: form['description']
            };
            return this.postApi.savePost(post);
        }))
            .subscribe(() => {
            this.alertService
                .open(`Create ${this.form.value.title} successfully`, {
                label: 'Success'
            })
                .subscribe();
            this.handleAfterCreate();
        });
    }
    createNewModule() {
        this.handleBeforeCreate();
        this.moduleApi
            .getDefaultModuleTemplate()
            .pipe(switchMap(result => {
            const form = this.form.getRawValue();
            const module = {
                ...result,
                title: form['title'],
                excerpt: form['excerpt'],
                content: form['description'],
                systemName: form['systemName']
            };
            return this.moduleApi.saveModule(module);
        }))
            .subscribe(() => {
            this.alertService
                .open(`Create ${this.form.value.title} successfully`, {
                label: 'Success'
            })
                .subscribe();
            this.handleAfterCreate();
        });
    }
    createNewPage() {
        this.handleBeforeCreate();
        this.pageApi
            .getDefaultPageTemplate()
            .pipe(switchMap(result => {
            const form = this.form.getRawValue();
            const page = {
                ...result,
                title: form['title'],
                excerpt: form['excerpt'],
                content: form['description'],
                systemName: form['systemName'],
                seoDescription: form['seoDescription'],
                seoKeywords: form['seoKeywords'],
                seoName: form['seoName'],
                seoTitle: form['seoTitle'],
                seoSource: form['seoSource']
            };
            return this.pageApi.savePage(page);
        }))
            .subscribe(() => {
            const message = `Create ${this.form.value.title} successfully`;
            this.alertService.open(message, { label: 'Success' }).subscribe();
            this.handleAfterCreate();
        });
    }
    handleBeforeCreate() {
        this.form.disable();
        this.loading = true;
    }
    handleAfterCreate() {
        this.loading = false;
        this.form.enable();
    }
    closeSidebar() {
        this.sidebarControl.hide();
    }
    toggleLargeMode() {
        this.largeMode = !this.largeMode;
    }
}
CreationDialogComponent.ɵfac = function CreationDialogComponent_Factory(t) { return new (t || CreationDialogComponent)(i0.ɵɵdirectiveInject(i1.MixPostApiService), i0.ɵɵdirectiveInject(i2.MixModuleApiService), i0.ɵɵdirectiveInject(i1.MixPageApiService), i0.ɵɵdirectiveInject(TuiAlertService), i0.ɵɵdirectiveInject(PortalSidebarControlService)); };
CreationDialogComponent.ɵcmp = /*@__PURE__*/ i0.ɵɵdefineComponent({ type: CreationDialogComponent, selectors: [["mix-creation-dialog"]], inputs: { type: "type" }, standalone: true, features: [i0.ɵɵStandaloneFeature], decls: 31, vars: 21, consts: [[1, "creation-dialog", 3, "ngClass"], [1, "creation-dialog__header"], ["tuiTextfieldSize", "s", 3, "ngModel", "tuiTextfieldLabelOutside", "ngModelChange"], ["tuiTextfield", "", "placeholder", "Choose your type"], [3, "items", 4, "tuiDataList"], [1, "action"], ["tuiButton", "", 3, "appearance", "size", "click"], ["name", "layout-sidebar-right"], ["name", "square-x"], [1, "creation-dialog__content"], [1, "creation-dialog__content-left-side"], [1, "toolbar"], ["tuiButton", "", 1, "action", 3, "icon", "showLoader", "size", "click"], [1, "workspace"], [3, "activeItemIndex", "activeItemIndexChange"], ["tuiTab", "", 3, "click"], ["tuiTab", "", 3, "click", 4, "ngIf"], [4, "ngIf"], ["class", "creation-dialog__content-right-side", 4, "ngIf"], [1, "creation-dialog__expand-btn", 3, "click"], [3, "src"], ["seoHint", ""], [3, "items"], [1, "mix-form", 3, "formGroup"], [1, "mix-form__row"], ["tuiLabel", "", 3, "label"], ["tuiTextfieldSize", "m", "formControlName", "title", 3, "tuiTextfieldLabelOutside"], ["type", "text", "tuiTextfield", ""], ["formControlName", "title"], ["tuiTextfieldSize", "m", "formControlName", "systemName", 3, "tuiTextfieldLabelOutside"], ["formControlName", "systemName"], ["tuiTextfieldSize", "m", "formControlName", "excerpt", 3, "tuiTextfieldLabelOutside"], ["new", "", "formControlName", "description", 1, "editor"], ["tuiTextfieldSize", "m", "formControlName", "seoTitle", 3, "tuiHintContent", "tuiTextfieldLabelOutside"], ["tuiTextfieldSize", "m", "formControlName", "seoDescription", 3, "tuiHintContent", "tuiTextfieldLabelOutside"], ["tuiTextfieldSize", "m", "formControlName", "seoName", 3, "tuiHintContent", "tuiTextfieldLabelOutside"], ["tuiTextfieldSize", "m", "formControlName", "seoKeywords", 3, "tuiHintContent", "tuiTextfieldLabelOutside"], [1, "creation-dialog__content-right-side"], [3, "rounded"], [3, "size"], ["tuiAccordionItemContent", ""], ["content", ""]], template: function CreationDialogComponent_Template(rf, ctx) { if (rf & 1) {
        i0.ɵɵelementStart(0, "div", 0)(1, "div", 1)(2, "div");
        i0.ɵɵtext(3, "Create new: ");
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(4, "tui-select", 2);
        i0.ɵɵlistener("ngModelChange", function CreationDialogComponent_Template_tui_select_ngModelChange_4_listener($event) { return ctx.type = $event; });
        i0.ɵɵelement(5, "input", 3);
        i0.ɵɵtemplate(6, CreationDialogComponent_tui_data_list_wrapper_6_Template, 1, 1, "tui-data-list-wrapper", 4);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(7, "div", 5)(8, "button", 6);
        i0.ɵɵlistener("click", function CreationDialogComponent_Template_button_click_8_listener() { return ctx.showRightSideMenu = !ctx.showRightSideMenu; });
        i0.ɵɵelement(9, "i-tabler", 7);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(10, "button", 6);
        i0.ɵɵlistener("click", function CreationDialogComponent_Template_button_click_10_listener() { return ctx.closeSidebar(); });
        i0.ɵɵelement(11, "i-tabler", 8);
        i0.ɵɵelementEnd()()();
        i0.ɵɵelementStart(12, "div", 9)(13, "div", 10)(14, "div", 11)(15, "button", 12);
        i0.ɵɵlistener("click", function CreationDialogComponent_Template_button_click_15_listener() { return ctx.submitForm(); });
        i0.ɵɵtext(16, "Create");
        i0.ɵɵelementEnd()();
        i0.ɵɵelementStart(17, "div", 13)(18, "tui-tabs", 14);
        i0.ɵɵlistener("activeItemIndexChange", function CreationDialogComponent_Template_tui_tabs_activeItemIndexChange_18_listener($event) { return ctx.activeTabIndex = $event; });
        i0.ɵɵelementStart(19, "button", 15);
        i0.ɵɵlistener("click", function CreationDialogComponent_Template_button_click_19_listener() { return ctx.activeTabIndex = 0; });
        i0.ɵɵtext(20, " Contents ");
        i0.ɵɵelementEnd();
        i0.ɵɵtemplate(21, CreationDialogComponent_button_21_Template, 2, 0, "button", 16);
        i0.ɵɵtemplate(22, CreationDialogComponent_button_22_Template, 2, 0, "button", 16);
        i0.ɵɵelementEnd();
        i0.ɵɵtemplate(23, CreationDialogComponent_ng_container_23_Template, 18, 8, "ng-container", 17);
        i0.ɵɵtemplate(24, CreationDialogComponent_ng_container_24_Template, 16, 13, "ng-container", 17);
        i0.ɵɵtemplate(25, CreationDialogComponent_ng_container_25_Template, 2, 0, "ng-container", 17);
        i0.ɵɵelementEnd()();
        i0.ɵɵtemplate(26, CreationDialogComponent_div_26_Template, 11, 4, "div", 18);
        i0.ɵɵelementEnd();
        i0.ɵɵelementStart(27, "div", 19);
        i0.ɵɵlistener("click", function CreationDialogComponent_Template_div_click_27_listener() { return ctx.toggleLargeMode(); });
        i0.ɵɵelement(28, "tui-svg", 20);
        i0.ɵɵelementEnd()();
        i0.ɵɵtemplate(29, CreationDialogComponent_ng_template_29_Template, 8, 0, "ng-template", null, 21, i0.ɵɵtemplateRefExtractor);
    } if (rf & 2) {
        i0.ɵɵproperty("ngClass", i0.ɵɵpureFunction1(19, _c0, ctx.largeMode))("@enterAnimation", undefined);
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("ngModel", ctx.type)("tuiTextfieldLabelOutside", true);
        i0.ɵɵadvance(4);
        i0.ɵɵproperty("appearance", "icon")("size", "xs");
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("appearance", "icon")("size", "xs");
        i0.ɵɵadvance(5);
        i0.ɵɵproperty("icon", "tuiIconCheckLarge")("showLoader", ctx.loading)("size", "s");
        i0.ɵɵadvance(3);
        i0.ɵɵproperty("activeItemIndex", ctx.activeTabIndex);
        i0.ɵɵadvance(3);
        i0.ɵɵproperty("ngIf", ctx.canShowSEOConfig);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.canShowRelatedModule);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.activeTabIndex === 0);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.activeTabIndex === 1);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.activeTabIndex === 2);
        i0.ɵɵadvance(1);
        i0.ɵɵproperty("ngIf", ctx.showRightSideMenu);
        i0.ɵɵadvance(2);
        i0.ɵɵproperty("src", ctx.largeMode ? "tuiIconArrowRight" : "tuiIconArrowLeft");
    } }, dependencies: [ShareModule, i3.NgClass, i3.NgIf, i4.NgControlStatus, i4.NgControlStatusGroup, i4.NgModel, i4.FormGroupDirective, i4.FormControlName, i5.TuiInputComponent, i5.TuiInputDirective, i6.TuiTextfieldComponent, i5.TuiSelectComponent, i5.TuiSelectDirective, i6.TuiSvgComponent, i6.TuiButtonComponent, i6.TuiDataListDirective, i5.TuiDataListWrapperComponent, i6.TuiLabelComponent, i5.TuiTextAreaComponent, i5.TuiTextAreaDirective, i6.TuiTextfieldLabelOutsideDirective, i6.TuiTextfieldSizeDirective, i5.TuiFieldErrorComponent, i5.TuiTabsComponent, i5.TuiTabComponent, i6.TuiHintControllerDirective, i7.TuiEditorNewComponent, i5.TuiAccordionComponent, i5.TuiAccordionItemComponent, i5.TuiAccordionItemContentDirective, i8.TablerIconComponent, MixModuleSelectComponent], styles: [".creation-dialog[_ngcontent-%COMP%]{width:55vw;min-width:800px;height:100%;display:flex;position:relative;flex-direction:column;border-top-left-radius:10px;background-color:var(--tui-base-01);border-left:1px sold var(--tui-base-04);transition:all .3s ease;box-shadow:#3c40434d 0 1px 2px,#3c404326 0 2px 6px 2px}.creation-dialog.--large[_ngcontent-%COMP%]{width:70vw}.creation-dialog__header[_ngcontent-%COMP%]{display:flex;align-items:center;justify-content:flex-start;width:100%;padding:10px 10px 10px 20px;border-bottom:1px solid var(--tui-base-04)}.creation-dialog__header[_ngcontent-%COMP%]   tui-select[_ngcontent-%COMP%]{margin-left:10px;width:200px}.creation-dialog__header[_ngcontent-%COMP%]   .action[_ngcontent-%COMP%]{margin-left:auto}.creation-dialog__content[_ngcontent-%COMP%]{width:100%;height:100%;display:flex}.creation-dialog__content-left-side[_ngcontent-%COMP%]{width:100%;height:100%}.creation-dialog__content-left-side[_ngcontent-%COMP%]   .toolbar[_ngcontent-%COMP%]{height:var(--mix-sub-header-height);background-color:var(--tui-base-03);display:flex;align-items:center;justify-content:flex-end;padding:0 20px;margin-bottom:5px}.creation-dialog__content-left-side[_ngcontent-%COMP%]   .workspace[_ngcontent-%COMP%]{padding:0 20px}.creation-dialog__content-left-side[_ngcontent-%COMP%]   tui-tabs[_ngcontent-%COMP%]{margin-bottom:10px}.creation-dialog__content-right-side[_ngcontent-%COMP%]{width:300px;height:100%;border-left:1px solid var(--tui-base-04)}.creation-dialog__content-right-side[_ngcontent-%COMP%]   tui-accordion[_ngcontent-%COMP%]{width:100%}.creation-dialog__expand-btn[_ngcontent-%COMP%]{position:absolute;left:-15px;border-radius:50%;padding:2px;display:flex;justify-content:center;align-items:center;border:1px solid var(--tui-base-04);background-color:var(--tui-base-01);box-shadow:#3c40434d 0 1px 2px,#3c404326 0 2px 6px 2px}.creation-dialog__expand-btn[_ngcontent-%COMP%]{bottom:10px}"], data: { animation: [slideAnimation] } });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(CreationDialogComponent, [{
        type: Component,
        args: [{ selector: 'mix-creation-dialog', standalone: true, imports: [ShareModule, MixModuleSelectComponent], animations: [slideAnimation], template: "<div class=\"creation-dialog\"\r\n     [ngClass]=\"{'--large': largeMode}\"\r\n     [@enterAnimation]>\r\n  <div class=\"creation-dialog__header\">\r\n    <div>Create new: </div>\r\n    <tui-select [(ngModel)]=\"type\"\r\n                [tuiTextfieldLabelOutside]=\"true\"\r\n                tuiTextfieldSize=\"s\">\r\n      <input tuiTextfield\r\n             placeholder=\"Choose your type\">\r\n      <tui-data-list-wrapper *tuiDataList\r\n                             [items]=\"items\"></tui-data-list-wrapper>\r\n    </tui-select>\r\n\r\n    <div class=\"action\">\r\n      <button [appearance]=\"'icon'\"\r\n              [size]=\"'xs'\"\r\n              (click)=\"showRightSideMenu = !showRightSideMenu\"\r\n              tuiButton>\r\n        <i-tabler name=\"layout-sidebar-right\"></i-tabler>\r\n      </button>\r\n\r\n      <button [appearance]=\"'icon'\"\r\n              [size]=\"'xs'\"\r\n              (click)=\"closeSidebar()\"\r\n              tuiButton>\r\n        <i-tabler name=\"square-x\"></i-tabler>\r\n      </button>\r\n    </div>\r\n  </div>\r\n\r\n  <div class=\"creation-dialog__content\">\r\n    <div class=\"creation-dialog__content-left-side\">\r\n      <div class=\"toolbar\">\r\n        <button class=\"action\"\r\n                [icon]=\"'tuiIconCheckLarge'\"\r\n                [showLoader]=\"loading\"\r\n                [size]=\"'s'\"\r\n                (click)=\"submitForm()\"\r\n                tuiButton>Create</button>\r\n      </div>\r\n\r\n      <div class=\"workspace\">\r\n        <tui-tabs [(activeItemIndex)]=\"activeTabIndex\">\r\n          <button (click)=\"activeTabIndex = 0\"\r\n                  tuiTab>\r\n            Contents\r\n          </button>\r\n          <button *ngIf=\"canShowSEOConfig\"\r\n                  (click)=\"activeTabIndex = 1\"\r\n                  tuiTab>\r\n            Seo\r\n          </button>\r\n          <button *ngIf=\"canShowRelatedModule\"\r\n                  (click)=\"activeTabIndex = 2\"\r\n                  tuiTab>\r\n            Related\r\n          </button>\r\n        </tui-tabs>\r\n\r\n        <!-- Main information -->\r\n        <ng-container *ngIf=\"activeTabIndex === 0\">\r\n          <div class=\"mix-form\"\r\n               [formGroup]=\"form\">\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'Title:'\"\r\n                     tuiLabel>\r\n                <tui-input [tuiTextfieldLabelOutside]=\"true\"\r\n                           tuiTextfieldSize=\"m\"\r\n                           formControlName=\"title\">\r\n                  <input type=\"text\"\r\n                         tuiTextfield>\r\n                </tui-input>\r\n                <tui-field-error formControlName=\"title\"></tui-field-error>\r\n              </label>\r\n            </div>\r\n\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'System name:'\"\r\n                     tuiLabel>\r\n                <tui-input [tuiTextfieldLabelOutside]=\"true\"\r\n                           tuiTextfieldSize=\"m\"\r\n                           formControlName=\"systemName\">\r\n                  <input type=\"text\"\r\n                         tuiTextfield>\r\n                </tui-input>\r\n                <tui-field-error formControlName=\"systemName\"></tui-field-error>\r\n              </label>\r\n            </div>\r\n\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'Excerpt:'\"\r\n                     tuiLabel>\r\n                <tui-text-area [tuiTextfieldLabelOutside]=\"true\"\r\n                               tuiTextfieldSize=\"m\"\r\n                               formControlName=\"excerpt\"></tui-text-area>\r\n              </label>\r\n            </div>\r\n\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'Description:'\"\r\n                     tuiLabel>\r\n                <tui-editor class=\"editor\"\r\n                            new\r\n                            formControlName=\"description\">\r\n                </tui-editor>\r\n              </label>\r\n            </div>\r\n          </div>\r\n        </ng-container>\r\n\r\n        <!-- SEO information -->\r\n        <ng-container *ngIf=\"activeTabIndex === 1\">\r\n          <div class=\"mix-form\"\r\n               [formGroup]=\"form\">\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'Friendly Title:'\"\r\n                     tuiLabel>\r\n                <tui-input [tuiHintContent]=\"'Google displays the entire title on the search results, which has 63 characters.'\"\r\n                           [tuiTextfieldLabelOutside]=\"true\"\r\n                           tuiTextfieldSize=\"m\"\r\n                           formControlName=\"seoTitle\">\r\n                  <input type=\"text\"\r\n                         tuiTextfield>\r\n                </tui-input>\r\n              </label>\r\n            </div>\r\n\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'Meta Description:'\"\r\n                     tuiLabel>\r\n                <tui-text-area [tuiHintContent]=\"'Do make sure your most important keywords for the webpage show up in the meta description. Often search engines will highlight in bold where it finds the searchers query in your snippet.'\"\r\n                               [tuiTextfieldLabelOutside]=\"true\"\r\n                               tuiTextfieldSize=\"m\"\r\n                               formControlName=\"seoDescription\"></tui-text-area>\r\n              </label>\r\n            </div>\r\n\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'Friendly URL:'\"\r\n                     tuiLabel>\r\n                <tui-input [tuiHintContent]=\"seoHint\"\r\n                           [tuiTextfieldLabelOutside]=\"true\"\r\n                           tuiTextfieldSize=\"m\"\r\n                           formControlName=\"seoName\">\r\n                  <input type=\"text\"\r\n                         tuiTextfield>\r\n                </tui-input>\r\n              </label>\r\n            </div>\r\n\r\n            <div class=\"mix-form__row\">\r\n              <label [label]=\"'Meta Keywords:'\"\r\n                     tuiLabel>\r\n                <tui-text-area [tuiHintContent]=\"'Do make sure your most important keywords for the webpage show up in the meta description. Often search engines will highlight in bold where it finds the searchers query in your snippet.'\"\r\n                               [tuiTextfieldLabelOutside]=\"true\"\r\n                               tuiTextfieldSize=\"m\"\r\n                               formControlName=\"seoKeywords\"></tui-text-area>\r\n              </label>\r\n            </div>\r\n          </div>\r\n        </ng-container>\r\n\r\n        <!-- Data information -->\r\n        <ng-container *ngIf=\"activeTabIndex === 2\">\r\n          <mix-module-select></mix-module-select>\r\n        </ng-container>\r\n      </div>\r\n    </div>\r\n\r\n    <div *ngIf=\"showRightSideMenu\"\r\n         class=\"creation-dialog__content-right-side\">\r\n      <tui-accordion [rounded]=\"false\">\r\n        <tui-accordion-item [size]=\"'s'\">\r\n          Publishing\r\n          <ng-template tuiAccordionItemContent>\r\n            Development kit consisting of the low level tools and abstractions\r\n            used to develop Taiga UI Angular entities\r\n          </ng-template>\r\n        </tui-accordion-item>\r\n        <tui-accordion-item [size]=\"'s'\">\r\n          Language\r\n          <ng-template tuiAccordionItemContent>\r\n            <div #content>\r\n              Basic elements needed to develop components, directives and more\r\n              using Taiga UI design system\r\n            </div>\r\n          </ng-template>\r\n        </tui-accordion-item>\r\n        <tui-accordion-item [size]=\"'s'\">\r\n          Images\r\n          <ng-template tuiAccordionItemContent>\r\n            The main set of components used to build Taiga UI based Angular\r\n            applications\r\n          </ng-template>\r\n        </tui-accordion-item>\r\n      </tui-accordion>\r\n    </div>\r\n  </div>\r\n\r\n  <div class=\"creation-dialog__expand-btn\"\r\n       (click)=\"toggleLargeMode()\">\r\n    <tui-svg [src]=\"largeMode ? 'tuiIconArrowRight' : 'tuiIconArrowLeft'\"></tui-svg>\r\n  </div>\r\n</div>\r\n\r\n\r\n<ng-template #seoHint>\r\n  <div>\r\n    Easy to read: Users and search engines should be able to understand what is on each page just by looking at the URL. <br> <br>\r\n\r\n    Keyword-rich: Keywords still matter and your target queries should be within URLs. Just be wary of overkill; extending URLs just to\r\n    include more keywords is a bad idea.<br> <br>\r\n    Consistent: There are multiple ways to create an SEO-friendly URL structure on any site. It\u2019s essential that, whatever logic you choose\r\n    to follow, it is applied consistently across the site.\r\n  </div>\r\n</ng-template>\r\n", styles: [".creation-dialog{width:55vw;min-width:800px;height:100%;display:flex;position:relative;flex-direction:column;border-top-left-radius:10px;background-color:var(--tui-base-01);border-left:1px sold var(--tui-base-04);transition:all .3s ease;box-shadow:#3c40434d 0 1px 2px,#3c404326 0 2px 6px 2px}.creation-dialog.--large{width:70vw}.creation-dialog__header{display:flex;align-items:center;justify-content:flex-start;width:100%;padding:10px 10px 10px 20px;border-bottom:1px solid var(--tui-base-04)}.creation-dialog__header tui-select{margin-left:10px;width:200px}.creation-dialog__header .action{margin-left:auto}.creation-dialog__content{width:100%;height:100%;display:flex}.creation-dialog__content-left-side{width:100%;height:100%}.creation-dialog__content-left-side .toolbar{height:var(--mix-sub-header-height);background-color:var(--tui-base-03);display:flex;align-items:center;justify-content:flex-end;padding:0 20px;margin-bottom:5px}.creation-dialog__content-left-side .workspace{padding:0 20px}.creation-dialog__content-left-side tui-tabs{margin-bottom:10px}.creation-dialog__content-right-side{width:300px;height:100%;border-left:1px solid var(--tui-base-04)}.creation-dialog__content-right-side tui-accordion{width:100%}.creation-dialog__expand-btn{position:absolute;left:-15px;border-radius:50%;padding:2px;display:flex;justify-content:center;align-items:center;border:1px solid var(--tui-base-04);background-color:var(--tui-base-01);box-shadow:#3c40434d 0 1px 2px,#3c404326 0 2px 6px 2px}.creation-dialog__expand-btn{bottom:10px}\n"] }]
    }], function () { return [{ type: i1.MixPostApiService }, { type: i2.MixModuleApiService }, { type: i1.MixPageApiService }, { type: i6.TuiAlertService, decorators: [{
                type: Inject,
                args: [TuiAlertService]
            }] }, { type: i1.PortalSidebarControlService, decorators: [{
                type: Inject,
                args: [PortalSidebarControlService]
            }] }]; }, { type: [{
            type: Input
        }] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY3JlYXRpb24tZGlhbG9nLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvY29tcG9uZW50cy9kaWFsb2dzL2NyZWF0aW9uLWRpYWxvZy9jcmVhdGlvbi1kaWFsb2cuY29tcG9uZW50LnRzIiwiLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vLi4vbWl4LnBvcnRhbC5hbmd1bGFyL2xpYnMvbWl4LnNoYXJlL3NyYy9jb21wb25lbnRzL2RpYWxvZ3MvY3JlYXRpb24tZGlhbG9nL2NyZWF0aW9uLWRpYWxvZy5jb21wb25lbnQuaHRtbCJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDakUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFNcEUsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2pELE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFFakMsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ3JELE9BQU8sRUFDTCxpQkFBaUIsRUFDakIsaUJBQWlCLEVBQ2pCLDJCQUEyQixFQUM1QixNQUFNLG1CQUFtQixDQUFDO0FBQzNCLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLDhDQUE4QyxDQUFDO0FBQ25GLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx1QkFBdUIsQ0FBQztBQUNwRCxPQUFPLEVBQUUsU0FBUyxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3hELE9BQU8sRUFBRSx3QkFBd0IsRUFBRSxNQUFNLG1EQUFtRCxDQUFDOzs7Ozs7Ozs7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QUlTO1FBQ1QsaUZBSVM7UUFDWCxpQkFBVztRQUdYLDhGQWdEZTtRQUdmLCtGQWlEZTtRQUdmLDZGQUVlO1FBQ2pCLGlCQUFNLEVBQUE7UUFHUiw0RUEyQk07UUFDUixpQkFBTTtRQUVOLGdDQUNpQztRQUE1QixrR0FBUyxxQkFBaUIsSUFBQztRQUM5QiwrQkFBZ0Y7UUFDbEYsaUJBQU0sRUFBQTtRQUlSLDRIQVNjOztRQXZOVCxvRUFBa0MsOEJBQUE7UUFJdkIsZUFBa0I7UUFBbEIsa0NBQWtCLGtDQUFBO1FBVXBCLGVBQXFCO1FBQXJCLG1DQUFxQixjQUFBO1FBT3JCLGVBQXFCO1FBQXJCLG1DQUFxQixjQUFBO1FBYW5CLGVBQTRCO1FBQTVCLDBDQUE0QiwyQkFBQSxhQUFBO1FBUTFCLGVBQW9DO1FBQXBDLG9EQUFvQztRQUtuQyxlQUFzQjtRQUF0QiwyQ0FBc0I7UUFLdEIsZUFBMEI7UUFBMUIsK0NBQTBCO1FBUXRCLGVBQTBCO1FBQTFCLCtDQUEwQjtRQW1EMUIsZUFBMEI7UUFBMUIsK0NBQTBCO1FBb0QxQixlQUEwQjtRQUExQiwrQ0FBMEI7UUFNdkMsZUFBdUI7UUFBdkIsNENBQXVCO1FBZ0NwQixlQUE0RDtRQUE1RCw4RUFBNEQ7d0JEOUs3RCxXQUFXLGl0QkFBRSx3QkFBd0IsODZEQUNuQyxDQUFDLGNBQWMsQ0FBQzt1RkFFakIsdUJBQXVCO2NBUm5DLFNBQVM7MkJBQ0UscUJBQXFCLGNBR25CLElBQUksV0FDUCxDQUFDLFdBQVcsRUFBRSx3QkFBd0IsQ0FBQyxjQUNwQyxDQUFDLGNBQWMsQ0FBQzs7c0JBb0N6QixNQUFNO3VCQUFDLGVBQWU7O3NCQUN0QixNQUFNO3VCQUFDLDJCQUEyQjt3QkFsQ3JCLElBQUk7a0JBQW5CLEtBQUsiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIEluamVjdCwgSW5wdXQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBGb3JtQ29udHJvbCwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQge1xyXG4gIE1peE1vZHVsZVBvcnRhbE1vZGVsLFxyXG4gIE1peFBhZ2VQb3J0YWxNb2RlbCxcclxuICBNaXhQb3N0UG9ydGFsTW9kZWxcclxufSBmcm9tICdAbWl4LXNwYS9taXgubGliJztcclxuaW1wb3J0IHsgVHVpQWxlcnRTZXJ2aWNlIH0gZnJvbSAnQHRhaWdhLXVpL2NvcmUnO1xyXG5pbXBvcnQgeyBzd2l0Y2hNYXAgfSBmcm9tICdyeGpzJztcclxuXHJcbmltcG9ydCB7IHNsaWRlQW5pbWF0aW9uIH0gZnJvbSAnLi4vLi4vLi4vYW5pbWF0aW9ucyc7XHJcbmltcG9ydCB7XHJcbiAgTWl4UGFnZUFwaVNlcnZpY2UsXHJcbiAgTWl4UG9zdEFwaVNlcnZpY2UsXHJcbiAgUG9ydGFsU2lkZWJhckNvbnRyb2xTZXJ2aWNlXHJcbn0gZnJvbSAnLi4vLi4vLi4vc2VydmljZXMnO1xyXG5pbXBvcnQgeyBNaXhNb2R1bGVBcGlTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vLi4vc2VydmljZXMvYXBpL21peC1tb2R1bGUtYXBpLnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBTaGFyZU1vZHVsZSB9IGZyb20gJy4uLy4uLy4uL3NoYXJlLm1vZHVsZSc7XHJcbmltcG9ydCB7IEZvcm1VdGlscywgU3RyaW5nVXRpbHMgfSBmcm9tICcuLi8uLi8uLi91dGlscyc7XHJcbmltcG9ydCB7IE1peE1vZHVsZVNlbGVjdENvbXBvbmVudCB9IGZyb20gJy4uLy4uL21vZHVsZS1zZWxlY3RzLWxpc3QvbW9kdWxlLXNlbGVjdC5jb21wb25lbnQnO1xyXG5cclxuZXhwb3J0IHR5cGUgTWl4Q3JlYXRpb25UeXBlID0gJ1Bvc3QnIHwgJ1BhZ2UnIHwgJ01vZHVsZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ21peC1jcmVhdGlvbi1kaWFsb2cnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9jcmVhdGlvbi1kaWFsb2cuY29tcG9uZW50Lmh0bWwnLFxyXG4gIHN0eWxlVXJsczogWycuL2NyZWF0aW9uLWRpYWxvZy5jb21wb25lbnQuc2NzcyddLFxyXG4gIHN0YW5kYWxvbmU6IHRydWUsXHJcbiAgaW1wb3J0czogW1NoYXJlTW9kdWxlLCBNaXhNb2R1bGVTZWxlY3RDb21wb25lbnRdLFxyXG4gIGFuaW1hdGlvbnM6IFtzbGlkZUFuaW1hdGlvbl1cclxufSlcclxuZXhwb3J0IGNsYXNzIENyZWF0aW9uRGlhbG9nQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcclxuICBASW5wdXQoKSBwdWJsaWMgdHlwZTogTWl4Q3JlYXRpb25UeXBlID0gJ1Bvc3QnO1xyXG5cclxuICBwdWJsaWMgaXRlbXM6IE1peENyZWF0aW9uVHlwZVtdID0gWydQb3N0JywgJ1BhZ2UnLCAnTW9kdWxlJ107XHJcbiAgcHVibGljIGZvcm06IEZvcm1Hcm91cCA9IG5ldyBGb3JtR3JvdXAoe1xyXG4gICAgdGl0bGU6IG5ldyBGb3JtQ29udHJvbCgnJywgVmFsaWRhdG9ycy5yZXF1aXJlZCksXHJcbiAgICBzeXN0ZW1OYW1lOiBuZXcgRm9ybUNvbnRyb2woJycsIFZhbGlkYXRvcnMucmVxdWlyZWQpLFxyXG4gICAgZXhjZXJwdDogbmV3IEZvcm1Db250cm9sKCcnKSxcclxuICAgIGRlc2NyaXB0aW9uOiBuZXcgRm9ybUNvbnRyb2woJycpLFxyXG4gICAgc2VvTmFtZTogbmV3IEZvcm1Db250cm9sKCcnKSxcclxuICAgIHNlb1RpdGxlOiBuZXcgRm9ybUNvbnRyb2woJycpLFxyXG4gICAgc2VvRGVzY3JpcHRpb246IG5ldyBGb3JtQ29udHJvbCgnJyksXHJcbiAgICBzZW9LZXl3b3JkczogbmV3IEZvcm1Db250cm9sKCcnKSxcclxuICAgIHNlb1NvdXJjZTogbmV3IEZvcm1Db250cm9sKCcnKVxyXG4gIH0pO1xyXG5cclxuICBwdWJsaWMgZ29Ub1Bvc3RBZnRlckNyZWF0ZSA9IHRydWU7XHJcbiAgcHVibGljIGNsb3NlRGlhbG9nQWZ0ZXJDcmVhdGUgPSB0cnVlO1xyXG4gIHB1YmxpYyBsb2FkaW5nID0gZmFsc2U7XHJcbiAgcHVibGljIGluaXRpYWxpemUgPSBmYWxzZTtcclxuICBwdWJsaWMgYWN0aXZlVGFiSW5kZXggPSAwO1xyXG4gIHB1YmxpYyBzaG93UmlnaHRTaWRlTWVudSA9IHRydWU7XHJcbiAgcHVibGljIGxhcmdlTW9kZSA9IGZhbHNlO1xyXG4gIHB1YmxpYyBnZXQgY2FuU2hvd1JlbGF0ZWRNb2R1bGUoKTogYm9vbGVhbiB7XHJcbiAgICByZXR1cm4gdGhpcy50eXBlID09PSAnUGFnZScgfHwgdGhpcy50eXBlID09PSAnUG9zdCc7XHJcbiAgfVxyXG4gIHB1YmxpYyBnZXQgY2FuU2hvd1NFT0NvbmZpZygpOiBib29sZWFuIHtcclxuICAgIHJldHVybiB0aGlzLnR5cGUgPT09ICdQYWdlJyB8fCB0aGlzLnR5cGUgPT09ICdQb3N0JztcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHVibGljIHBvc3RBcGk6IE1peFBvc3RBcGlTZXJ2aWNlLFxyXG4gICAgcHVibGljIG1vZHVsZUFwaTogTWl4TW9kdWxlQXBpU2VydmljZSxcclxuICAgIHB1YmxpYyBwYWdlQXBpOiBNaXhQYWdlQXBpU2VydmljZSxcclxuICAgIEBJbmplY3QoVHVpQWxlcnRTZXJ2aWNlKSBwcml2YXRlIHJlYWRvbmx5IGFsZXJ0U2VydmljZTogVHVpQWxlcnRTZXJ2aWNlLFxyXG4gICAgQEluamVjdChQb3J0YWxTaWRlYmFyQ29udHJvbFNlcnZpY2UpXHJcbiAgICBwcml2YXRlIHJlYWRvbmx5IHNpZGViYXJDb250cm9sOiBQb3J0YWxTaWRlYmFyQ29udHJvbFNlcnZpY2VcclxuICApIHt9XHJcblxyXG4gIHB1YmxpYyBuZ09uSW5pdCgpOiB2b2lkIHtcclxuICAgIHRoaXMuZm9ybS5jb250cm9sc1sndGl0bGUnXS52YWx1ZUNoYW5nZXMuc3Vic2NyaWJlKCh0aXRsZTogc3RyaW5nKSA9PiB7XHJcbiAgICAgIHRoaXMuZm9ybS5jb250cm9sc1snc3lzdGVtTmFtZSddLnBhdGNoVmFsdWUoXHJcbiAgICAgICAgU3RyaW5nVXRpbHMudGV4dFRvU3lzdGVtTmFtZSh0aXRsZSlcclxuICAgICAgKTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIHN1Ym1pdEZvcm0oKTogdm9pZCB7XHJcbiAgICBpZiAoIUZvcm1VdGlscy52YWxpZGF0ZUZvcm0odGhpcy5mb3JtKSkgcmV0dXJuO1xyXG5cclxuICAgIHN3aXRjaCAodGhpcy50eXBlKSB7XHJcbiAgICAgIGNhc2UgJ1Bvc3QnOlxyXG4gICAgICAgIHRoaXMuY3JlYXRlTmV3UG9zdCgpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgICBjYXNlICdNb2R1bGUnOlxyXG4gICAgICAgIHRoaXMuY3JlYXRlTmV3TW9kdWxlKCk7XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGNhc2UgJ1BhZ2UnOlxyXG4gICAgICAgIHRoaXMuY3JlYXRlTmV3UGFnZSgpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgcHVibGljIGNyZWF0ZU5ld1Bvc3QoKTogdm9pZCB7XHJcbiAgICB0aGlzLmhhbmRsZUJlZm9yZUNyZWF0ZSgpO1xyXG4gICAgdGhpcy5wb3N0QXBpXHJcbiAgICAgIC5nZXREZWZhdWx0UG9zdFRlbXBsYXRlKClcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgc3dpdGNoTWFwKHJlc3VsdCA9PiB7XHJcbiAgICAgICAgICBjb25zdCBmb3JtID0gdGhpcy5mb3JtLmdldFJhd1ZhbHVlKCk7XHJcbiAgICAgICAgICBjb25zdCBwb3N0ID0gPE1peFBvc3RQb3J0YWxNb2RlbD57XHJcbiAgICAgICAgICAgIC4uLnJlc3VsdCxcclxuICAgICAgICAgICAgdGl0bGU6IGZvcm1bJ3RpdGxlJ10sXHJcbiAgICAgICAgICAgIGV4Y2VycHQ6IGZvcm1bJ2V4Y2VycHQnXSxcclxuICAgICAgICAgICAgY29udGVudDogZm9ybVsnZGVzY3JpcHRpb24nXVxyXG4gICAgICAgICAgfTtcclxuXHJcbiAgICAgICAgICByZXR1cm4gdGhpcy5wb3N0QXBpLnNhdmVQb3N0KHBvc3QpO1xyXG4gICAgICAgIH0pXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgICAgdGhpcy5hbGVydFNlcnZpY2VcclxuICAgICAgICAgIC5vcGVuKGBDcmVhdGUgJHt0aGlzLmZvcm0udmFsdWUudGl0bGV9IHN1Y2Nlc3NmdWxseWAsIHtcclxuICAgICAgICAgICAgbGFiZWw6ICdTdWNjZXNzJ1xyXG4gICAgICAgICAgfSlcclxuICAgICAgICAgIC5zdWJzY3JpYmUoKTtcclxuICAgICAgICB0aGlzLmhhbmRsZUFmdGVyQ3JlYXRlKCk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGNyZWF0ZU5ld01vZHVsZSgpOiB2b2lkIHtcclxuICAgIHRoaXMuaGFuZGxlQmVmb3JlQ3JlYXRlKCk7XHJcbiAgICB0aGlzLm1vZHVsZUFwaVxyXG4gICAgICAuZ2V0RGVmYXVsdE1vZHVsZVRlbXBsYXRlKClcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgc3dpdGNoTWFwKHJlc3VsdCA9PiB7XHJcbiAgICAgICAgICBjb25zdCBmb3JtID0gdGhpcy5mb3JtLmdldFJhd1ZhbHVlKCk7XHJcbiAgICAgICAgICBjb25zdCBtb2R1bGUgPSA8TWl4TW9kdWxlUG9ydGFsTW9kZWw+e1xyXG4gICAgICAgICAgICAuLi5yZXN1bHQsXHJcbiAgICAgICAgICAgIHRpdGxlOiBmb3JtWyd0aXRsZSddLFxyXG4gICAgICAgICAgICBleGNlcnB0OiBmb3JtWydleGNlcnB0J10sXHJcbiAgICAgICAgICAgIGNvbnRlbnQ6IGZvcm1bJ2Rlc2NyaXB0aW9uJ10sXHJcbiAgICAgICAgICAgIHN5c3RlbU5hbWU6IGZvcm1bJ3N5c3RlbU5hbWUnXVxyXG4gICAgICAgICAgfTtcclxuXHJcbiAgICAgICAgICByZXR1cm4gdGhpcy5tb2R1bGVBcGkuc2F2ZU1vZHVsZShtb2R1bGUpO1xyXG4gICAgICAgIH0pXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgICAgdGhpcy5hbGVydFNlcnZpY2VcclxuICAgICAgICAgIC5vcGVuKGBDcmVhdGUgJHt0aGlzLmZvcm0udmFsdWUudGl0bGV9IHN1Y2Nlc3NmdWxseWAsIHtcclxuICAgICAgICAgICAgbGFiZWw6ICdTdWNjZXNzJ1xyXG4gICAgICAgICAgfSlcclxuICAgICAgICAgIC5zdWJzY3JpYmUoKTtcclxuXHJcbiAgICAgICAgdGhpcy5oYW5kbGVBZnRlckNyZWF0ZSgpO1xyXG4gICAgICB9KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBjcmVhdGVOZXdQYWdlKCk6IHZvaWQge1xyXG4gICAgdGhpcy5oYW5kbGVCZWZvcmVDcmVhdGUoKTtcclxuICAgIHRoaXMucGFnZUFwaVxyXG4gICAgICAuZ2V0RGVmYXVsdFBhZ2VUZW1wbGF0ZSgpXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIHN3aXRjaE1hcChyZXN1bHQgPT4ge1xyXG4gICAgICAgICAgY29uc3QgZm9ybSA9IHRoaXMuZm9ybS5nZXRSYXdWYWx1ZSgpO1xyXG4gICAgICAgICAgY29uc3QgcGFnZSA9IDxNaXhQYWdlUG9ydGFsTW9kZWw+e1xyXG4gICAgICAgICAgICAuLi5yZXN1bHQsXHJcbiAgICAgICAgICAgIHRpdGxlOiBmb3JtWyd0aXRsZSddLFxyXG4gICAgICAgICAgICBleGNlcnB0OiBmb3JtWydleGNlcnB0J10sXHJcbiAgICAgICAgICAgIGNvbnRlbnQ6IGZvcm1bJ2Rlc2NyaXB0aW9uJ10sXHJcbiAgICAgICAgICAgIHN5c3RlbU5hbWU6IGZvcm1bJ3N5c3RlbU5hbWUnXSxcclxuICAgICAgICAgICAgc2VvRGVzY3JpcHRpb246IGZvcm1bJ3Nlb0Rlc2NyaXB0aW9uJ10sXHJcbiAgICAgICAgICAgIHNlb0tleXdvcmRzOiBmb3JtWydzZW9LZXl3b3JkcyddLFxyXG4gICAgICAgICAgICBzZW9OYW1lOiBmb3JtWydzZW9OYW1lJ10sXHJcbiAgICAgICAgICAgIHNlb1RpdGxlOiBmb3JtWydzZW9UaXRsZSddLFxyXG4gICAgICAgICAgICBzZW9Tb3VyY2U6IGZvcm1bJ3Nlb1NvdXJjZSddXHJcbiAgICAgICAgICB9O1xyXG5cclxuICAgICAgICAgIHJldHVybiB0aGlzLnBhZ2VBcGkuc2F2ZVBhZ2UocGFnZSk7XHJcbiAgICAgICAgfSlcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgICBjb25zdCBtZXNzYWdlID0gYENyZWF0ZSAke3RoaXMuZm9ybS52YWx1ZS50aXRsZX0gc3VjY2Vzc2Z1bGx5YDtcclxuICAgICAgICB0aGlzLmFsZXJ0U2VydmljZS5vcGVuKG1lc3NhZ2UsIHsgbGFiZWw6ICdTdWNjZXNzJyB9KS5zdWJzY3JpYmUoKTtcclxuXHJcbiAgICAgICAgdGhpcy5oYW5kbGVBZnRlckNyZWF0ZSgpO1xyXG4gICAgICB9KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBoYW5kbGVCZWZvcmVDcmVhdGUoKTogdm9pZCB7XHJcbiAgICB0aGlzLmZvcm0uZGlzYWJsZSgpO1xyXG4gICAgdGhpcy5sb2FkaW5nID0gdHJ1ZTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBoYW5kbGVBZnRlckNyZWF0ZSgpOiB2b2lkIHtcclxuICAgIHRoaXMubG9hZGluZyA9IGZhbHNlO1xyXG4gICAgdGhpcy5mb3JtLmVuYWJsZSgpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIGNsb3NlU2lkZWJhcigpOiB2b2lkIHtcclxuICAgIHRoaXMuc2lkZWJhckNvbnRyb2wuaGlkZSgpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIHRvZ2dsZUxhcmdlTW9kZSgpOiB2b2lkIHtcclxuICAgIHRoaXMubGFyZ2VNb2RlID0gIXRoaXMubGFyZ2VNb2RlO1xyXG4gIH1cclxufVxyXG4iLCI8ZGl2IGNsYXNzPVwiY3JlYXRpb24tZGlhbG9nXCJcclxuICAgICBbbmdDbGFzc109XCJ7Jy0tbGFyZ2UnOiBsYXJnZU1vZGV9XCJcclxuICAgICBbQGVudGVyQW5pbWF0aW9uXT5cclxuICA8ZGl2IGNsYXNzPVwiY3JlYXRpb24tZGlhbG9nX19oZWFkZXJcIj5cclxuICAgIDxkaXY+Q3JlYXRlIG5ldzogPC9kaXY+XHJcbiAgICA8dHVpLXNlbGVjdCBbKG5nTW9kZWwpXT1cInR5cGVcIlxyXG4gICAgICAgICAgICAgICAgW3R1aVRleHRmaWVsZExhYmVsT3V0c2lkZV09XCJ0cnVlXCJcclxuICAgICAgICAgICAgICAgIHR1aVRleHRmaWVsZFNpemU9XCJzXCI+XHJcbiAgICAgIDxpbnB1dCB0dWlUZXh0ZmllbGRcclxuICAgICAgICAgICAgIHBsYWNlaG9sZGVyPVwiQ2hvb3NlIHlvdXIgdHlwZVwiPlxyXG4gICAgICA8dHVpLWRhdGEtbGlzdC13cmFwcGVyICp0dWlEYXRhTGlzdFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgIFtpdGVtc109XCJpdGVtc1wiPjwvdHVpLWRhdGEtbGlzdC13cmFwcGVyPlxyXG4gICAgPC90dWktc2VsZWN0PlxyXG5cclxuICAgIDxkaXYgY2xhc3M9XCJhY3Rpb25cIj5cclxuICAgICAgPGJ1dHRvbiBbYXBwZWFyYW5jZV09XCInaWNvbidcIlxyXG4gICAgICAgICAgICAgIFtzaXplXT1cIid4cydcIlxyXG4gICAgICAgICAgICAgIChjbGljayk9XCJzaG93UmlnaHRTaWRlTWVudSA9ICFzaG93UmlnaHRTaWRlTWVudVwiXHJcbiAgICAgICAgICAgICAgdHVpQnV0dG9uPlxyXG4gICAgICAgIDxpLXRhYmxlciBuYW1lPVwibGF5b3V0LXNpZGViYXItcmlnaHRcIj48L2ktdGFibGVyPlxyXG4gICAgICA8L2J1dHRvbj5cclxuXHJcbiAgICAgIDxidXR0b24gW2FwcGVhcmFuY2VdPVwiJ2ljb24nXCJcclxuICAgICAgICAgICAgICBbc2l6ZV09XCIneHMnXCJcclxuICAgICAgICAgICAgICAoY2xpY2spPVwiY2xvc2VTaWRlYmFyKClcIlxyXG4gICAgICAgICAgICAgIHR1aUJ1dHRvbj5cclxuICAgICAgICA8aS10YWJsZXIgbmFtZT1cInNxdWFyZS14XCI+PC9pLXRhYmxlcj5cclxuICAgICAgPC9idXR0b24+XHJcbiAgICA8L2Rpdj5cclxuICA8L2Rpdj5cclxuXHJcbiAgPGRpdiBjbGFzcz1cImNyZWF0aW9uLWRpYWxvZ19fY29udGVudFwiPlxyXG4gICAgPGRpdiBjbGFzcz1cImNyZWF0aW9uLWRpYWxvZ19fY29udGVudC1sZWZ0LXNpZGVcIj5cclxuICAgICAgPGRpdiBjbGFzcz1cInRvb2xiYXJcIj5cclxuICAgICAgICA8YnV0dG9uIGNsYXNzPVwiYWN0aW9uXCJcclxuICAgICAgICAgICAgICAgIFtpY29uXT1cIid0dWlJY29uQ2hlY2tMYXJnZSdcIlxyXG4gICAgICAgICAgICAgICAgW3Nob3dMb2FkZXJdPVwibG9hZGluZ1wiXHJcbiAgICAgICAgICAgICAgICBbc2l6ZV09XCIncydcIlxyXG4gICAgICAgICAgICAgICAgKGNsaWNrKT1cInN1Ym1pdEZvcm0oKVwiXHJcbiAgICAgICAgICAgICAgICB0dWlCdXR0b24+Q3JlYXRlPC9idXR0b24+XHJcbiAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgPGRpdiBjbGFzcz1cIndvcmtzcGFjZVwiPlxyXG4gICAgICAgIDx0dWktdGFicyBbKGFjdGl2ZUl0ZW1JbmRleCldPVwiYWN0aXZlVGFiSW5kZXhcIj5cclxuICAgICAgICAgIDxidXR0b24gKGNsaWNrKT1cImFjdGl2ZVRhYkluZGV4ID0gMFwiXHJcbiAgICAgICAgICAgICAgICAgIHR1aVRhYj5cclxuICAgICAgICAgICAgQ29udGVudHNcclxuICAgICAgICAgIDwvYnV0dG9uPlxyXG4gICAgICAgICAgPGJ1dHRvbiAqbmdJZj1cImNhblNob3dTRU9Db25maWdcIlxyXG4gICAgICAgICAgICAgICAgICAoY2xpY2spPVwiYWN0aXZlVGFiSW5kZXggPSAxXCJcclxuICAgICAgICAgICAgICAgICAgdHVpVGFiPlxyXG4gICAgICAgICAgICBTZW9cclxuICAgICAgICAgIDwvYnV0dG9uPlxyXG4gICAgICAgICAgPGJ1dHRvbiAqbmdJZj1cImNhblNob3dSZWxhdGVkTW9kdWxlXCJcclxuICAgICAgICAgICAgICAgICAgKGNsaWNrKT1cImFjdGl2ZVRhYkluZGV4ID0gMlwiXHJcbiAgICAgICAgICAgICAgICAgIHR1aVRhYj5cclxuICAgICAgICAgICAgUmVsYXRlZFxyXG4gICAgICAgICAgPC9idXR0b24+XHJcbiAgICAgICAgPC90dWktdGFicz5cclxuXHJcbiAgICAgICAgPCEtLSBNYWluIGluZm9ybWF0aW9uIC0tPlxyXG4gICAgICAgIDxuZy1jb250YWluZXIgKm5nSWY9XCJhY3RpdmVUYWJJbmRleCA9PT0gMFwiPlxyXG4gICAgICAgICAgPGRpdiBjbGFzcz1cIm1peC1mb3JtXCJcclxuICAgICAgICAgICAgICAgW2Zvcm1Hcm91cF09XCJmb3JtXCI+XHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJtaXgtZm9ybV9fcm93XCI+XHJcbiAgICAgICAgICAgICAgPGxhYmVsIFtsYWJlbF09XCInVGl0bGU6J1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIHR1aUxhYmVsPlxyXG4gICAgICAgICAgICAgICAgPHR1aS1pbnB1dCBbdHVpVGV4dGZpZWxkTGFiZWxPdXRzaWRlXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICB0dWlUZXh0ZmllbGRTaXplPVwibVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgIGZvcm1Db250cm9sTmFtZT1cInRpdGxlXCI+XHJcbiAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwidGV4dFwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICB0dWlUZXh0ZmllbGQ+XHJcbiAgICAgICAgICAgICAgICA8L3R1aS1pbnB1dD5cclxuICAgICAgICAgICAgICAgIDx0dWktZmllbGQtZXJyb3IgZm9ybUNvbnRyb2xOYW1lPVwidGl0bGVcIj48L3R1aS1maWVsZC1lcnJvcj5cclxuICAgICAgICAgICAgICA8L2xhYmVsPlxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJtaXgtZm9ybV9fcm93XCI+XHJcbiAgICAgICAgICAgICAgPGxhYmVsIFtsYWJlbF09XCInU3lzdGVtIG5hbWU6J1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIHR1aUxhYmVsPlxyXG4gICAgICAgICAgICAgICAgPHR1aS1pbnB1dCBbdHVpVGV4dGZpZWxkTGFiZWxPdXRzaWRlXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICB0dWlUZXh0ZmllbGRTaXplPVwibVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgIGZvcm1Db250cm9sTmFtZT1cInN5c3RlbU5hbWVcIj5cclxuICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJ0ZXh0XCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgIHR1aVRleHRmaWVsZD5cclxuICAgICAgICAgICAgICAgIDwvdHVpLWlucHV0PlxyXG4gICAgICAgICAgICAgICAgPHR1aS1maWVsZC1lcnJvciBmb3JtQ29udHJvbE5hbWU9XCJzeXN0ZW1OYW1lXCI+PC90dWktZmllbGQtZXJyb3I+XHJcbiAgICAgICAgICAgICAgPC9sYWJlbD5cclxuICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzPVwibWl4LWZvcm1fX3Jvd1wiPlxyXG4gICAgICAgICAgICAgIDxsYWJlbCBbbGFiZWxdPVwiJ0V4Y2VycHQ6J1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIHR1aUxhYmVsPlxyXG4gICAgICAgICAgICAgICAgPHR1aS10ZXh0LWFyZWEgW3R1aVRleHRmaWVsZExhYmVsT3V0c2lkZV09XCJ0cnVlXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHR1aVRleHRmaWVsZFNpemU9XCJtXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGZvcm1Db250cm9sTmFtZT1cImV4Y2VycHRcIj48L3R1aS10ZXh0LWFyZWE+XHJcbiAgICAgICAgICAgICAgPC9sYWJlbD5cclxuICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzPVwibWl4LWZvcm1fX3Jvd1wiPlxyXG4gICAgICAgICAgICAgIDxsYWJlbCBbbGFiZWxdPVwiJ0Rlc2NyaXB0aW9uOidcIlxyXG4gICAgICAgICAgICAgICAgICAgICB0dWlMYWJlbD5cclxuICAgICAgICAgICAgICAgIDx0dWktZWRpdG9yIGNsYXNzPVwiZWRpdG9yXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG5ld1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgZm9ybUNvbnRyb2xOYW1lPVwiZGVzY3JpcHRpb25cIj5cclxuICAgICAgICAgICAgICAgIDwvdHVpLWVkaXRvcj5cclxuICAgICAgICAgICAgICA8L2xhYmVsPlxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgIDwvbmctY29udGFpbmVyPlxyXG5cclxuICAgICAgICA8IS0tIFNFTyBpbmZvcm1hdGlvbiAtLT5cclxuICAgICAgICA8bmctY29udGFpbmVyICpuZ0lmPVwiYWN0aXZlVGFiSW5kZXggPT09IDFcIj5cclxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJtaXgtZm9ybVwiXHJcbiAgICAgICAgICAgICAgIFtmb3JtR3JvdXBdPVwiZm9ybVwiPlxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzPVwibWl4LWZvcm1fX3Jvd1wiPlxyXG4gICAgICAgICAgICAgIDxsYWJlbCBbbGFiZWxdPVwiJ0ZyaWVuZGx5IFRpdGxlOidcIlxyXG4gICAgICAgICAgICAgICAgICAgICB0dWlMYWJlbD5cclxuICAgICAgICAgICAgICAgIDx0dWktaW5wdXQgW3R1aUhpbnRDb250ZW50XT1cIidHb29nbGUgZGlzcGxheXMgdGhlIGVudGlyZSB0aXRsZSBvbiB0aGUgc2VhcmNoIHJlc3VsdHMsIHdoaWNoIGhhcyA2MyBjaGFyYWN0ZXJzLidcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICBbdHVpVGV4dGZpZWxkTGFiZWxPdXRzaWRlXT1cInRydWVcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICB0dWlUZXh0ZmllbGRTaXplPVwibVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgIGZvcm1Db250cm9sTmFtZT1cInNlb1RpdGxlXCI+XHJcbiAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwidGV4dFwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICB0dWlUZXh0ZmllbGQ+XHJcbiAgICAgICAgICAgICAgICA8L3R1aS1pbnB1dD5cclxuICAgICAgICAgICAgICA8L2xhYmVsPlxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJtaXgtZm9ybV9fcm93XCI+XHJcbiAgICAgICAgICAgICAgPGxhYmVsIFtsYWJlbF09XCInTWV0YSBEZXNjcmlwdGlvbjonXCJcclxuICAgICAgICAgICAgICAgICAgICAgdHVpTGFiZWw+XHJcbiAgICAgICAgICAgICAgICA8dHVpLXRleHQtYXJlYSBbdHVpSGludENvbnRlbnRdPVwiJ0RvIG1ha2Ugc3VyZSB5b3VyIG1vc3QgaW1wb3J0YW50IGtleXdvcmRzIGZvciB0aGUgd2VicGFnZSBzaG93IHVwIGluIHRoZSBtZXRhIGRlc2NyaXB0aW9uLiBPZnRlbiBzZWFyY2ggZW5naW5lcyB3aWxsIGhpZ2hsaWdodCBpbiBib2xkIHdoZXJlIGl0IGZpbmRzIHRoZSBzZWFyY2hlcnMgcXVlcnkgaW4geW91ciBzbmlwcGV0LidcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgW3R1aVRleHRmaWVsZExhYmVsT3V0c2lkZV09XCJ0cnVlXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHR1aVRleHRmaWVsZFNpemU9XCJtXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGZvcm1Db250cm9sTmFtZT1cInNlb0Rlc2NyaXB0aW9uXCI+PC90dWktdGV4dC1hcmVhPlxyXG4gICAgICAgICAgICAgIDwvbGFiZWw+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgPGRpdiBjbGFzcz1cIm1peC1mb3JtX19yb3dcIj5cclxuICAgICAgICAgICAgICA8bGFiZWwgW2xhYmVsXT1cIidGcmllbmRseSBVUkw6J1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIHR1aUxhYmVsPlxyXG4gICAgICAgICAgICAgICAgPHR1aS1pbnB1dCBbdHVpSGludENvbnRlbnRdPVwic2VvSGludFwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgIFt0dWlUZXh0ZmllbGRMYWJlbE91dHNpZGVdPVwidHJ1ZVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgIHR1aVRleHRmaWVsZFNpemU9XCJtXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgZm9ybUNvbnRyb2xOYW1lPVwic2VvTmFtZVwiPlxyXG4gICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cInRleHRcIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgdHVpVGV4dGZpZWxkPlxyXG4gICAgICAgICAgICAgICAgPC90dWktaW5wdXQ+XHJcbiAgICAgICAgICAgICAgPC9sYWJlbD5cclxuICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzPVwibWl4LWZvcm1fX3Jvd1wiPlxyXG4gICAgICAgICAgICAgIDxsYWJlbCBbbGFiZWxdPVwiJ01ldGEgS2V5d29yZHM6J1wiXHJcbiAgICAgICAgICAgICAgICAgICAgIHR1aUxhYmVsPlxyXG4gICAgICAgICAgICAgICAgPHR1aS10ZXh0LWFyZWEgW3R1aUhpbnRDb250ZW50XT1cIidEbyBtYWtlIHN1cmUgeW91ciBtb3N0IGltcG9ydGFudCBrZXl3b3JkcyBmb3IgdGhlIHdlYnBhZ2Ugc2hvdyB1cCBpbiB0aGUgbWV0YSBkZXNjcmlwdGlvbi4gT2Z0ZW4gc2VhcmNoIGVuZ2luZXMgd2lsbCBoaWdobGlnaHQgaW4gYm9sZCB3aGVyZSBpdCBmaW5kcyB0aGUgc2VhcmNoZXJzIHF1ZXJ5IGluIHlvdXIgc25pcHBldC4nXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFt0dWlUZXh0ZmllbGRMYWJlbE91dHNpZGVdPVwidHJ1ZVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB0dWlUZXh0ZmllbGRTaXplPVwibVwiXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBmb3JtQ29udHJvbE5hbWU9XCJzZW9LZXl3b3Jkc1wiPjwvdHVpLXRleHQtYXJlYT5cclxuICAgICAgICAgICAgICA8L2xhYmVsPlxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgIDwvbmctY29udGFpbmVyPlxyXG5cclxuICAgICAgICA8IS0tIERhdGEgaW5mb3JtYXRpb24gLS0+XHJcbiAgICAgICAgPG5nLWNvbnRhaW5lciAqbmdJZj1cImFjdGl2ZVRhYkluZGV4ID09PSAyXCI+XHJcbiAgICAgICAgICA8bWl4LW1vZHVsZS1zZWxlY3Q+PC9taXgtbW9kdWxlLXNlbGVjdD5cclxuICAgICAgICA8L25nLWNvbnRhaW5lcj5cclxuICAgICAgPC9kaXY+XHJcbiAgICA8L2Rpdj5cclxuXHJcbiAgICA8ZGl2ICpuZ0lmPVwic2hvd1JpZ2h0U2lkZU1lbnVcIlxyXG4gICAgICAgICBjbGFzcz1cImNyZWF0aW9uLWRpYWxvZ19fY29udGVudC1yaWdodC1zaWRlXCI+XHJcbiAgICAgIDx0dWktYWNjb3JkaW9uIFtyb3VuZGVkXT1cImZhbHNlXCI+XHJcbiAgICAgICAgPHR1aS1hY2NvcmRpb24taXRlbSBbc2l6ZV09XCIncydcIj5cclxuICAgICAgICAgIFB1Ymxpc2hpbmdcclxuICAgICAgICAgIDxuZy10ZW1wbGF0ZSB0dWlBY2NvcmRpb25JdGVtQ29udGVudD5cclxuICAgICAgICAgICAgRGV2ZWxvcG1lbnQga2l0IGNvbnNpc3Rpbmcgb2YgdGhlIGxvdyBsZXZlbCB0b29scyBhbmQgYWJzdHJhY3Rpb25zXHJcbiAgICAgICAgICAgIHVzZWQgdG8gZGV2ZWxvcCBUYWlnYSBVSSBBbmd1bGFyIGVudGl0aWVzXHJcbiAgICAgICAgICA8L25nLXRlbXBsYXRlPlxyXG4gICAgICAgIDwvdHVpLWFjY29yZGlvbi1pdGVtPlxyXG4gICAgICAgIDx0dWktYWNjb3JkaW9uLWl0ZW0gW3NpemVdPVwiJ3MnXCI+XHJcbiAgICAgICAgICBMYW5ndWFnZVxyXG4gICAgICAgICAgPG5nLXRlbXBsYXRlIHR1aUFjY29yZGlvbkl0ZW1Db250ZW50PlxyXG4gICAgICAgICAgICA8ZGl2ICNjb250ZW50PlxyXG4gICAgICAgICAgICAgIEJhc2ljIGVsZW1lbnRzIG5lZWRlZCB0byBkZXZlbG9wIGNvbXBvbmVudHMsIGRpcmVjdGl2ZXMgYW5kIG1vcmVcclxuICAgICAgICAgICAgICB1c2luZyBUYWlnYSBVSSBkZXNpZ24gc3lzdGVtXHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgPC9uZy10ZW1wbGF0ZT5cclxuICAgICAgICA8L3R1aS1hY2NvcmRpb24taXRlbT5cclxuICAgICAgICA8dHVpLWFjY29yZGlvbi1pdGVtIFtzaXplXT1cIidzJ1wiPlxyXG4gICAgICAgICAgSW1hZ2VzXHJcbiAgICAgICAgICA8bmctdGVtcGxhdGUgdHVpQWNjb3JkaW9uSXRlbUNvbnRlbnQ+XHJcbiAgICAgICAgICAgIFRoZSBtYWluIHNldCBvZiBjb21wb25lbnRzIHVzZWQgdG8gYnVpbGQgVGFpZ2EgVUkgYmFzZWQgQW5ndWxhclxyXG4gICAgICAgICAgICBhcHBsaWNhdGlvbnNcclxuICAgICAgICAgIDwvbmctdGVtcGxhdGU+XHJcbiAgICAgICAgPC90dWktYWNjb3JkaW9uLWl0ZW0+XHJcbiAgICAgIDwvdHVpLWFjY29yZGlvbj5cclxuICAgIDwvZGl2PlxyXG4gIDwvZGl2PlxyXG5cclxuICA8ZGl2IGNsYXNzPVwiY3JlYXRpb24tZGlhbG9nX19leHBhbmQtYnRuXCJcclxuICAgICAgIChjbGljayk9XCJ0b2dnbGVMYXJnZU1vZGUoKVwiPlxyXG4gICAgPHR1aS1zdmcgW3NyY109XCJsYXJnZU1vZGUgPyAndHVpSWNvbkFycm93UmlnaHQnIDogJ3R1aUljb25BcnJvd0xlZnQnXCI+PC90dWktc3ZnPlxyXG4gIDwvZGl2PlxyXG48L2Rpdj5cclxuXHJcblxyXG48bmctdGVtcGxhdGUgI3Nlb0hpbnQ+XHJcbiAgPGRpdj5cclxuICAgIEVhc3kgdG8gcmVhZDogVXNlcnMgYW5kIHNlYXJjaCBlbmdpbmVzIHNob3VsZCBiZSBhYmxlIHRvIHVuZGVyc3RhbmQgd2hhdCBpcyBvbiBlYWNoIHBhZ2UganVzdCBieSBsb29raW5nIGF0IHRoZSBVUkwuIDxicj4gPGJyPlxyXG5cclxuICAgIEtleXdvcmQtcmljaDogS2V5d29yZHMgc3RpbGwgbWF0dGVyIGFuZCB5b3VyIHRhcmdldCBxdWVyaWVzIHNob3VsZCBiZSB3aXRoaW4gVVJMcy4gSnVzdCBiZSB3YXJ5IG9mIG92ZXJraWxsOyBleHRlbmRpbmcgVVJMcyBqdXN0IHRvXHJcbiAgICBpbmNsdWRlIG1vcmUga2V5d29yZHMgaXMgYSBiYWQgaWRlYS48YnI+IDxicj5cclxuICAgIENvbnNpc3RlbnQ6IFRoZXJlIGFyZSBtdWx0aXBsZSB3YXlzIHRvIGNyZWF0ZSBhbiBTRU8tZnJpZW5kbHkgVVJMIHN0cnVjdHVyZSBvbiBhbnkgc2l0ZS4gSXTigJlzIGVzc2VudGlhbCB0aGF0LCB3aGF0ZXZlciBsb2dpYyB5b3UgY2hvb3NlXHJcbiAgICB0byBmb2xsb3csIGl0IGlzIGFwcGxpZWQgY29uc2lzdGVudGx5IGFjcm9zcyB0aGUgc2l0ZS5cclxuICA8L2Rpdj5cclxuPC9uZy10ZW1wbGF0ZT5cclxuIl19