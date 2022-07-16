import { DragDropModule } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiReorderModule, TuiTableModule, TuiTablePaginationModule } from '@taiga-ui/addon-table';
import { TuiLetModule } from '@taiga-ui/cdk';
import { TuiButtonModule, TuiHostedDropdownModule, TuiLoaderModule, TuiTextfieldControllerModule } from '@taiga-ui/core';
import { TuiCheckboxModule, TuiInputCountModule, TuiInputModule, TuiPaginationModule } from '@taiga-ui/kit';
import { TablerIconsModule } from 'angular-tabler-icons';
import { RelativeTimeSpanPipe } from '../../pipes';
import { MixDataTableComponent } from './data-table.component';
import { TableCellDirective } from './directives/cell.directive';
import { TableColumnDirective } from './directives/column.directive';
import { TableHeaderDirective } from './directives/header.directive';
import * as i0 from "@angular/core";
export class MixDataTableModule {
}
MixDataTableModule.ɵfac = function MixDataTableModule_Factory(t) { return new (t || MixDataTableModule)(); };
MixDataTableModule.ɵmod = /*@__PURE__*/ i0.ɵɵdefineNgModule({ type: MixDataTableModule });
MixDataTableModule.ɵinj = /*@__PURE__*/ i0.ɵɵdefineInjector({ imports: [CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TuiButtonModule,
        TuiTableModule,
        TuiInputModule,
        TuiInputCountModule,
        TuiHostedDropdownModule,
        TuiReorderModule,
        TuiTextfieldControllerModule,
        TuiTablePaginationModule,
        TuiLetModule,
        TuiLoaderModule,
        TuiCheckboxModule,
        TuiPaginationModule,
        DragDropModule,
        TablerIconsModule] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(MixDataTableModule, [{
        type: NgModule,
        args: [{
                declarations: [MixDataTableComponent, TableHeaderDirective, TableCellDirective, TableColumnDirective],
                imports: [
                    CommonModule,
                    FormsModule,
                    ReactiveFormsModule,
                    TuiButtonModule,
                    TuiTableModule,
                    TuiInputModule,
                    TuiInputCountModule,
                    TuiHostedDropdownModule,
                    TuiReorderModule,
                    TuiTextfieldControllerModule,
                    TuiTablePaginationModule,
                    TuiLetModule,
                    TuiLoaderModule,
                    TuiCheckboxModule,
                    TuiPaginationModule,
                    RelativeTimeSpanPipe,
                    DragDropModule,
                    TablerIconsModule
                ],
                exports: [MixDataTableComponent, TableHeaderDirective, TableCellDirective, TableColumnDirective]
            }]
    }], null, null); })();
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && i0.ɵɵsetNgModuleScope(MixDataTableModule, { declarations: [MixDataTableComponent, TableHeaderDirective, TableCellDirective, TableColumnDirective], imports: [CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TuiButtonModule,
        TuiTableModule,
        TuiInputModule,
        TuiInputCountModule,
        TuiHostedDropdownModule,
        TuiReorderModule,
        TuiTextfieldControllerModule,
        TuiTablePaginationModule,
        TuiLetModule,
        TuiLoaderModule,
        TuiCheckboxModule,
        TuiPaginationModule,
        RelativeTimeSpanPipe,
        DragDropModule,
        TablerIconsModule], exports: [MixDataTableComponent, TableHeaderDirective, TableCellDirective, TableColumnDirective] }); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGF0YS10YWJsZS5tb2R1bGUuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi8uLi9taXgucG9ydGFsLmFuZ3VsYXIvbGlicy9taXguc2hhcmUvc3JjL2NvbXBvbmVudHMvZGF0YS10YWJsZS9kYXRhLXRhYmxlLm1vZHVsZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDeEQsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLFdBQVcsRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2xFLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxjQUFjLEVBQUUsd0JBQXdCLEVBQUUsTUFBTSx1QkFBdUIsQ0FBQztBQUNuRyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzdDLE9BQU8sRUFBRSxlQUFlLEVBQUUsdUJBQXVCLEVBQUUsZUFBZSxFQUFFLDRCQUE0QixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDekgsT0FBTyxFQUFFLGlCQUFpQixFQUFFLG1CQUFtQixFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1RyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUV6RCxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDbkQsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDL0QsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sNkJBQTZCLENBQUM7QUFDakUsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sK0JBQStCLENBQUM7QUFDckUsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sK0JBQStCLENBQUM7O0FBMEJyRSxNQUFNLE9BQU8sa0JBQWtCOztvRkFBbEIsa0JBQWtCO29FQUFsQixrQkFBa0I7d0VBckIzQixZQUFZO1FBQ1osV0FBVztRQUNYLG1CQUFtQjtRQUNuQixlQUFlO1FBQ2YsY0FBYztRQUNkLGNBQWM7UUFDZCxtQkFBbUI7UUFDbkIsdUJBQXVCO1FBQ3ZCLGdCQUFnQjtRQUNoQiw0QkFBNEI7UUFDNUIsd0JBQXdCO1FBQ3hCLFlBQVk7UUFDWixlQUFlO1FBQ2YsaUJBQWlCO1FBQ2pCLG1CQUFtQjtRQUVuQixjQUFjO1FBQ2QsaUJBQWlCO3VGQUlSLGtCQUFrQjtjQXhCOUIsUUFBUTtlQUFDO2dCQUNSLFlBQVksRUFBRSxDQUFDLHFCQUFxQixFQUFFLG9CQUFvQixFQUFFLGtCQUFrQixFQUFFLG9CQUFvQixDQUFDO2dCQUNyRyxPQUFPLEVBQUU7b0JBQ1AsWUFBWTtvQkFDWixXQUFXO29CQUNYLG1CQUFtQjtvQkFDbkIsZUFBZTtvQkFDZixjQUFjO29CQUNkLGNBQWM7b0JBQ2QsbUJBQW1CO29CQUNuQix1QkFBdUI7b0JBQ3ZCLGdCQUFnQjtvQkFDaEIsNEJBQTRCO29CQUM1Qix3QkFBd0I7b0JBQ3hCLFlBQVk7b0JBQ1osZUFBZTtvQkFDZixpQkFBaUI7b0JBQ2pCLG1CQUFtQjtvQkFDbkIsb0JBQW9CO29CQUNwQixjQUFjO29CQUNkLGlCQUFpQjtpQkFDbEI7Z0JBQ0QsT0FBTyxFQUFFLENBQUMscUJBQXFCLEVBQUUsb0JBQW9CLEVBQUUsa0JBQWtCLEVBQUUsb0JBQW9CLENBQUM7YUFDakc7O3dGQUNZLGtCQUFrQixtQkF2QmQscUJBQXFCLEVBQUUsb0JBQW9CLEVBQUUsa0JBQWtCLEVBQUUsb0JBQW9CLGFBRWxHLFlBQVk7UUFDWixXQUFXO1FBQ1gsbUJBQW1CO1FBQ25CLGVBQWU7UUFDZixjQUFjO1FBQ2QsY0FBYztRQUNkLG1CQUFtQjtRQUNuQix1QkFBdUI7UUFDdkIsZ0JBQWdCO1FBQ2hCLDRCQUE0QjtRQUM1Qix3QkFBd0I7UUFDeEIsWUFBWTtRQUNaLGVBQWU7UUFDZixpQkFBaUI7UUFDakIsbUJBQW1CO1FBQ25CLG9CQUFvQjtRQUNwQixjQUFjO1FBQ2QsaUJBQWlCLGFBRVQscUJBQXFCLEVBQUUsb0JBQW9CLEVBQUUsa0JBQWtCLEVBQUUsb0JBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRHJhZ0Ryb3BNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jZGsvZHJhZy1kcm9wJztcclxuaW1wb3J0IHsgQ29tbW9uTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uJztcclxuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgRm9ybXNNb2R1bGUsIFJlYWN0aXZlRm9ybXNNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XHJcbmltcG9ydCB7IFR1aVJlb3JkZXJNb2R1bGUsIFR1aVRhYmxlTW9kdWxlLCBUdWlUYWJsZVBhZ2luYXRpb25Nb2R1bGUgfSBmcm9tICdAdGFpZ2EtdWkvYWRkb24tdGFibGUnO1xyXG5pbXBvcnQgeyBUdWlMZXRNb2R1bGUgfSBmcm9tICdAdGFpZ2EtdWkvY2RrJztcclxuaW1wb3J0IHsgVHVpQnV0dG9uTW9kdWxlLCBUdWlIb3N0ZWREcm9wZG93bk1vZHVsZSwgVHVpTG9hZGVyTW9kdWxlLCBUdWlUZXh0ZmllbGRDb250cm9sbGVyTW9kdWxlIH0gZnJvbSAnQHRhaWdhLXVpL2NvcmUnO1xyXG5pbXBvcnQgeyBUdWlDaGVja2JveE1vZHVsZSwgVHVpSW5wdXRDb3VudE1vZHVsZSwgVHVpSW5wdXRNb2R1bGUsIFR1aVBhZ2luYXRpb25Nb2R1bGUgfSBmcm9tICdAdGFpZ2EtdWkva2l0JztcclxuaW1wb3J0IHsgVGFibGVySWNvbnNNb2R1bGUgfSBmcm9tICdhbmd1bGFyLXRhYmxlci1pY29ucyc7XHJcblxyXG5pbXBvcnQgeyBSZWxhdGl2ZVRpbWVTcGFuUGlwZSB9IGZyb20gJy4uLy4uL3BpcGVzJztcclxuaW1wb3J0IHsgTWl4RGF0YVRhYmxlQ29tcG9uZW50IH0gZnJvbSAnLi9kYXRhLXRhYmxlLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFRhYmxlQ2VsbERpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9jZWxsLmRpcmVjdGl2ZSc7XHJcbmltcG9ydCB7IFRhYmxlQ29sdW1uRGlyZWN0aXZlIH0gZnJvbSAnLi9kaXJlY3RpdmVzL2NvbHVtbi5kaXJlY3RpdmUnO1xyXG5pbXBvcnQgeyBUYWJsZUhlYWRlckRpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9oZWFkZXIuZGlyZWN0aXZlJztcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgZGVjbGFyYXRpb25zOiBbTWl4RGF0YVRhYmxlQ29tcG9uZW50LCBUYWJsZUhlYWRlckRpcmVjdGl2ZSwgVGFibGVDZWxsRGlyZWN0aXZlLCBUYWJsZUNvbHVtbkRpcmVjdGl2ZV0sXHJcbiAgaW1wb3J0czogW1xyXG4gICAgQ29tbW9uTW9kdWxlLFxyXG4gICAgRm9ybXNNb2R1bGUsXHJcbiAgICBSZWFjdGl2ZUZvcm1zTW9kdWxlLFxyXG4gICAgVHVpQnV0dG9uTW9kdWxlLFxyXG4gICAgVHVpVGFibGVNb2R1bGUsXHJcbiAgICBUdWlJbnB1dE1vZHVsZSxcclxuICAgIFR1aUlucHV0Q291bnRNb2R1bGUsXHJcbiAgICBUdWlIb3N0ZWREcm9wZG93bk1vZHVsZSxcclxuICAgIFR1aVJlb3JkZXJNb2R1bGUsXHJcbiAgICBUdWlUZXh0ZmllbGRDb250cm9sbGVyTW9kdWxlLFxyXG4gICAgVHVpVGFibGVQYWdpbmF0aW9uTW9kdWxlLFxyXG4gICAgVHVpTGV0TW9kdWxlLFxyXG4gICAgVHVpTG9hZGVyTW9kdWxlLFxyXG4gICAgVHVpQ2hlY2tib3hNb2R1bGUsXHJcbiAgICBUdWlQYWdpbmF0aW9uTW9kdWxlLFxyXG4gICAgUmVsYXRpdmVUaW1lU3BhblBpcGUsXHJcbiAgICBEcmFnRHJvcE1vZHVsZSxcclxuICAgIFRhYmxlckljb25zTW9kdWxlXHJcbiAgXSxcclxuICBleHBvcnRzOiBbTWl4RGF0YVRhYmxlQ29tcG9uZW50LCBUYWJsZUhlYWRlckRpcmVjdGl2ZSwgVGFibGVDZWxsRGlyZWN0aXZlLCBUYWJsZUNvbHVtbkRpcmVjdGl2ZV1cclxufSlcclxuZXhwb3J0IGNsYXNzIE1peERhdGFUYWJsZU1vZHVsZSB7fVxyXG4iXX0=