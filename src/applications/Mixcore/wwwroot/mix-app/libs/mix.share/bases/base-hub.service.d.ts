import { HubConnection } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { SignalEvent } from '../interfaces/signal-event';
import { SignalEventType } from '../interfaces/signal-event-type';
import * as i0 from "@angular/core";
export declare abstract class BaseSignalService {
    domain: string;
    _signalEvent: Subject<SignalEvent<any>>;
    _openConnection: boolean;
    _isInitializing: boolean;
    _hubConnection: HubConnection;
    protected get _hubName(): string;
    protected get _roomName(): string;
    abstract _setupSignalREvents(): void;
    constructor(domain: string);
    getMessage<T>(...filterValues: SignalEventType[]): Observable<SignalEvent<T>>;
    protected _initializeSignalR(): Promise<void>;
    protected _ensureConnection(): Promise<void>;
    protected _pushMessage<T>(payload: SignalEvent<T>): void;
    static ɵfac: i0.ɵɵFactoryDeclaration<BaseSignalService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<BaseSignalService>;
}
