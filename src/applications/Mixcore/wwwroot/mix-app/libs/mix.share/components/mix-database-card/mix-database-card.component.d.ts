import { CdkDragEnd, DragRef, Point } from '@angular/cdk/drag-drop';
import { EventEmitter, OnInit } from '@angular/core';
import { MixDatabaseModel } from '@mix-spa/mix.lib';
import * as i0 from "@angular/core";
export declare class MixDatabaseCardComponent implements OnInit {
    database: MixDatabaseModel;
    zoomScale: number;
    pos: {
        x: number;
        y: number;
    };
    initializePos: {
        x: number;
        y: number;
    };
    index: number;
    dragStart: EventEmitter<void>;
    dragEnd: EventEmitter<void>;
    ngOnInit(): void;
    dragConstrainPoint: (point: Point, dragRef: DragRef) => {
        x: number;
        y: number;
    };
    startDragging(): void;
    endDragging($event: CdkDragEnd): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<MixDatabaseCardComponent, never>;
    static ɵcmp: i0.ɵɵComponentDeclaration<MixDatabaseCardComponent, "mix-database-card", never, { "database": "database"; "zoomScale": "zoomScale"; "pos": "pos"; "initializePos": "initializePos"; "index": "index"; }, { "dragStart": "dragStart"; "dragEnd": "dragEnd"; }, never, never, true>;
}
