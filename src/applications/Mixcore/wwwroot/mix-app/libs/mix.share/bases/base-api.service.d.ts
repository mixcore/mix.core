import { HttpClient, HttpContext, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppEventService } from '../services';
import * as i0 from "@angular/core";
export interface IHttpParamObject {
    [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>;
}
export interface IHttpHeadersObject {
    [header: string]: string | string[];
}
export interface IHttpOptions {
    headers?: HttpHeaders | IHttpHeadersObject;
    context?: HttpContext;
    observe?: 'body';
    params?: HttpParams | IHttpParamObject;
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
}
export interface IGetWithPaginationRequest {
    searchText?: string;
    searchId?: string;
    skipCount?: number;
    maxResultCount?: number;
    handleAuditedTrackId?: string;
    handleAuditedDate?: string;
    handleAuditedByUserId?: string;
}
export interface IGetWithPaginationResult<T> {
    items: T[];
    totalCount: number;
    pageSize: number;
}
export declare class BaseApiService {
    protected readonly http: HttpClient;
    baseUrl: string;
    appEvent: AppEventService;
    protected get url(): string;
    constructor(http: HttpClient, baseUrl: string, appEvent: AppEventService);
    get<TResult>(path: string, params?: IHttpParamObject): Observable<TResult>;
    post<TRequest, TResult>(path: string, request: TRequest, params?: HttpParams | IHttpParamObject): Observable<TResult>;
    put<TRequest, TResult>(path: string, request: TRequest, params?: HttpParams | IHttpParamObject): Observable<TResult>;
    delete<TResult>(path: string, params?: HttpParams | IHttpParamObject): Observable<TResult>;
    private getHttpOptions;
    private handleError;
    static ɵfac: i0.ɵɵFactoryDeclaration<BaseApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<BaseApiService>;
}
