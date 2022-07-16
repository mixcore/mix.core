import { CdkDragMove } from '@angular/cdk/drag-drop';
import { AfterViewInit, ChangeDetectorRef, ElementRef, OnInit, QueryList } from '@angular/core';
import { MixDatabaseModel } from '@mix-spa/mix.lib';
import { PanZoom } from 'panzoom';
import { DatabaseApiService } from '../../services';
import { MixDatabaseCardComponent } from '../mix-database-card/mix-database-card.component';
import * as i0 from "@angular/core";
export declare class MixDatabaseGraphComponent implements OnInit, AfterViewInit {
    private databaseApi;
    private cdr;
    canvasElement: ElementRef;
    databaseCard: QueryList<MixDatabaseCardComponent>;
    zoomScale: number;
    graphViewCanvas: PanZoom;
    dragPosition: {
        x: number;
        y: number;
    };
    databases: MixDatabaseModel[];
    constructor(databaseApi: DatabaseApiService, cdr: ChangeDetectorRef);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    pauseZoom(): void;
    resumeZoom(): void;
    onDragMove(value: CdkDragMove): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixDatabaseGraphComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixDatabaseGraphComponent, "mix-database-graph", never, {}, {}, never, never, true>;
}
