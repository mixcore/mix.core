import { SignalEventType } from './signal-event-type';
export interface SignalEvent<T> {
    type: SignalEventType;
    data: T;
}
