"use strict";(self.webpackChunkmix_portal=self.webpackChunkmix_portal||[]).push([[40],{6040:(P,m,l)=>{l.r(m),l.d(m,{PortalLayoutComponent:()=>f});var p=l(6696),n=l(573),u=l(1163),d=l(1135),e=l(5e3),_=l(9808);const h=["creationTemplate"];function g(i,s){1&i&&e.\u0275\u0275element(0,"mix-universal-search")}function v(i,s){1&i&&e.\u0275\u0275element(0,"mix-tab-control-dialog")}function C(i,s){if(1&i&&(e.\u0275\u0275elementStart(0,"mix-sidebar-container"),e.\u0275\u0275element(1,"mix-creation-dialog",9),e.\u0275\u0275elementEnd()),2&i){const t=e.\u0275\u0275nextContext();e.\u0275\u0275advance(1),e.\u0275\u0275property("type",t.createMode)}}let f=(()=>{class i{constructor(t,a,r,o,y){this.dialogService=t,this.router=a,this.tabControl=r,this.sidebarControl=o,this.appEvent=y,this.isShowUniversalSearch=!1,this.isShowTab=!1,this.createMode="Page",this.expand$=new d.X(!0),this.appEvent.event$.subscribe(c=>{c.type==n.qp.CreatePage?this.createNew("Page"):c.type==n.qp.CreatePost?this.createNew("Post"):c.type==n.qp.CreateModule?this.createNew("Module"):c.type==n.qp.UniversalSearch&&this.toggleUniversalSearch()})}showSearch(){this.toggleUniversalSearch()}new(){this.createNew("Post")}tab(t){t.preventDefault(),this.toggleTabControl(!0)}tabAlt(t){t.preventDefault(),this.toggleTabControl(!1)}createNew(t){this.createMode=t,this.sidebarControl.show(this.createTemplate)}navigate(t){this.router.navigateByUrl(t)}toggleUniversalSearch(){this.isShowUniversalSearch=!this.isShowUniversalSearch}toggleTabControl(t){this.isShowTab&&t&&this.tabControl.nextTab(),this.isShowTab=t}}return i.\u0275fac=function(t){return new(t||i)(e.\u0275\u0275directiveInject(u.RO),e.\u0275\u0275directiveInject(p.F0),e.\u0275\u0275directiveInject(n.aU),e.\u0275\u0275directiveInject(n.jY),e.\u0275\u0275directiveInject(n.Ts))},i.\u0275cmp=e.\u0275\u0275defineComponent({type:i,selectors:[["mix-portal-layout"]],viewQuery:function(t,a){if(1&t&&e.\u0275\u0275viewQuery(h,5),2&t){let r;e.\u0275\u0275queryRefresh(r=e.\u0275\u0275loadQuery())&&(a.createTemplate=r.first)}},hostBindings:function(t,a){1&t&&e.\u0275\u0275listener("keydown.f2",function(o){return a.showSearch(o)},!1,e.\u0275\u0275resolveWindow)("keydown.f3",function(o){return a.new(o)},!1,e.\u0275\u0275resolveWindow)("keydown.alt.z",function(o){return a.tab(o)},!1,e.\u0275\u0275resolveWindow)("keyup.alt",function(o){return a.tabAlt(o)},!1,e.\u0275\u0275resolveWindow)},standalone:!0,features:[e.\u0275\u0275StandaloneFeature],decls:14,vars:5,consts:[[1,"cms-portal-container"],[1,"cms-portal-container__header"],[3,"showLogo"],[1,"cms-portal-container__body"],[3,"expandChange"],[1,"cms-portal-container__main-workspace"],["ngProjectAs","tuiOverContent",5,["tuiOverContent"]],[3,"tuiDialog","tuiDialogChange"],["creationTemplate",""],[3,"type"]],template:function(t,a){1&t&&(e.\u0275\u0275elementStart(0,"div",0)(1,"div",1),e.\u0275\u0275element(2,"mix-header-menu",2),e.\u0275\u0275pipe(3,"async"),e.\u0275\u0275elementEnd(),e.\u0275\u0275elementStart(4,"div",3)(5,"mix-side-menu",4),e.\u0275\u0275listener("expandChange",function(o){return a.expand$.next(o)}),e.\u0275\u0275elementEnd(),e.\u0275\u0275elementStart(6,"div",5),e.\u0275\u0275element(7,"router-outlet"),e.\u0275\u0275elementContainerStart(8,6),e.\u0275\u0275element(9,"mix-portal-sidebar-host"),e.\u0275\u0275elementContainerEnd(),e.\u0275\u0275elementEnd()()(),e.\u0275\u0275template(10,g,1,0,"ng-template",7),e.\u0275\u0275listener("tuiDialogChange",function(o){return a.isShowUniversalSearch=o}),e.\u0275\u0275template(11,v,1,0,"ng-template",7),e.\u0275\u0275listener("tuiDialogChange",function(o){return a.isShowTab=o}),e.\u0275\u0275template(12,C,2,1,"ng-template",null,8,e.\u0275\u0275templateRefExtractor)),2&t&&(e.\u0275\u0275advance(2),e.\u0275\u0275property("showLogo",!e.\u0275\u0275pipeBind1(3,3,a.expand$)),e.\u0275\u0275advance(8),e.\u0275\u0275property("tuiDialog",a.isShowUniversalSearch),e.\u0275\u0275advance(1),e.\u0275\u0275property("tuiDialog",a.isShowTab))},dependencies:[n.Xb,p.Bz,p.lC,n.Gn,u.iu,_.Ov,n.V3,n.jI,n.R2,n.Ci,n.Ld,n.LA],styles:[".cms-portal-container[_ngcontent-%COMP%]{width:100vw;height:100vh;position:relative;background-color:var(--mix-body-background)}.cms-portal-container__body[_ngcontent-%COMP%]{display:flex;height:calc(100vh - var(--mix-header-height))}.cms-portal-container__main-workspace[_ngcontent-%COMP%]{border-radius:var(--mix-border-radius-02);margin-left:var(--mix-space-1);margin-right:var(--mix-space-2);width:100%;height:calc(100% - var(--mix-space-2));box-sizing:border-box;background-color:#fff;position:relative}.cms-portal-container__chat-bubble[_ngcontent-%COMP%]{position:absolute;bottom:0px;right:var(--mix-space-1);z-index:1}"],changeDetection:0}),i})()}}]);