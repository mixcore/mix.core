import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthApiService } from '../services';
import * as i0 from "@angular/core";
export declare class AuthInterceptor implements HttpInterceptor {
    private readonly authService;
    private route;
    constructor(authService: AuthApiService, route: Router);
    intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>>;
    static ɵfac: i0.ɵɵFactoryDeclaration<AuthInterceptor, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<AuthInterceptor>;
}
