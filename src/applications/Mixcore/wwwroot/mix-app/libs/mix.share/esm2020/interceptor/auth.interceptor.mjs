import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthApiService } from '../services';
import * as i0 from "@angular/core";
import * as i1 from "../services";
import * as i2 from "@angular/router";
export class AuthInterceptor {
    constructor(authService, route) {
        this.authService = authService;
        this.route = route;
    }
    intercept(req, next) {
        const clonedReq = req.clone({
            setHeaders: { Authorization: `${this.authService.getTokenType} ${this.authService.getAccessToken}` }
        });
        return next.handle(clonedReq).pipe(catchError(requestError => {
            if (requestError instanceof HttpErrorResponse && requestError.status === 401) {
                this.route.navigateByUrl('/auth/login');
            }
            return throwError(() => new Error(requestError));
        }));
    }
}
AuthInterceptor.ɵfac = function AuthInterceptor_Factory(t) { return new (t || AuthInterceptor)(i0.ɵɵinject(i1.AuthApiService), i0.ɵɵinject(i2.Router)); };
AuthInterceptor.ɵprov = /*@__PURE__*/ i0.ɵɵdefineInjectable({ token: AuthInterceptor, factory: AuthInterceptor.ɵfac, providedIn: 'root' });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(AuthInterceptor, [{
        type: Injectable,
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: i1.AuthApiService }, { type: i2.Router }]; }, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC5pbnRlcmNlcHRvci5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvaW50ZXJjZXB0b3IvYXV0aC5pbnRlcmNlcHRvci50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsaUJBQWlCLEVBQXdELE1BQU0sc0JBQXNCLENBQUM7QUFDL0csT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDekMsT0FBTyxFQUFFLFVBQVUsRUFBYyxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFFMUQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGFBQWEsQ0FBQzs7OztBQUs3QyxNQUFNLE9BQU8sZUFBZTtJQUMxQixZQUE2QixXQUEyQixFQUFVLEtBQWE7UUFBbEQsZ0JBQVcsR0FBWCxXQUFXLENBQWdCO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBUTtJQUFHLENBQUM7SUFFNUUsU0FBUyxDQUFDLEdBQXlCLEVBQUUsSUFBaUI7UUFDM0QsTUFBTSxTQUFTLEdBQXlCLEdBQUcsQ0FBQyxLQUFLLENBQUM7WUFDaEQsVUFBVSxFQUFFLEVBQUUsYUFBYSxFQUFFLEdBQUcsSUFBSSxDQUFDLFdBQVcsQ0FBQyxZQUFZLElBQUksSUFBSSxDQUFDLFdBQVcsQ0FBQyxjQUFjLEVBQUUsRUFBRTtTQUNyRyxDQUFDLENBQUM7UUFFSCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUMsU0FBUyxDQUFDLENBQUMsSUFBSSxDQUNoQyxVQUFVLENBQUMsWUFBWSxDQUFDLEVBQUU7WUFDeEIsSUFBSSxZQUFZLFlBQVksaUJBQWlCLElBQUksWUFBWSxDQUFDLE1BQU0sS0FBSyxHQUFHLEVBQUU7Z0JBQzVFLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUFDLGFBQWEsQ0FBQyxDQUFDO2FBQ3pDO1lBRUQsT0FBTyxVQUFVLENBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxLQUFLLENBQUMsWUFBWSxDQUFDLENBQUMsQ0FBQztRQUNuRCxDQUFDLENBQUMsQ0FDSCxDQUFDO0lBQ0osQ0FBQzs7OEVBakJVLGVBQWU7cUVBQWYsZUFBZSxXQUFmLGVBQWUsbUJBRmQsTUFBTTt1RkFFUCxlQUFlO2NBSDNCLFVBQVU7ZUFBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEh0dHBFcnJvclJlc3BvbnNlLCBIdHRwRXZlbnQsIEh0dHBIYW5kbGVyLCBIdHRwSW50ZXJjZXB0b3IsIEh0dHBSZXF1ZXN0IH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xyXG5pbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IGNhdGNoRXJyb3IsIE9ic2VydmFibGUsIHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcclxuXHJcbmltcG9ydCB7IEF1dGhBcGlTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290J1xyXG59KVxyXG5leHBvcnQgY2xhc3MgQXV0aEludGVyY2VwdG9yIGltcGxlbWVudHMgSHR0cEludGVyY2VwdG9yIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlYWRvbmx5IGF1dGhTZXJ2aWNlOiBBdXRoQXBpU2VydmljZSwgcHJpdmF0ZSByb3V0ZTogUm91dGVyKSB7fVxyXG5cclxuICBwdWJsaWMgaW50ZXJjZXB0KHJlcTogSHR0cFJlcXVlc3Q8dW5rbm93bj4sIG5leHQ6IEh0dHBIYW5kbGVyKTogT2JzZXJ2YWJsZTxIdHRwRXZlbnQ8dW5rbm93bj4+IHtcclxuICAgIGNvbnN0IGNsb25lZFJlcTogSHR0cFJlcXVlc3Q8dW5rbm93bj4gPSByZXEuY2xvbmUoe1xyXG4gICAgICBzZXRIZWFkZXJzOiB7IEF1dGhvcml6YXRpb246IGAke3RoaXMuYXV0aFNlcnZpY2UuZ2V0VG9rZW5UeXBlfSAke3RoaXMuYXV0aFNlcnZpY2UuZ2V0QWNjZXNzVG9rZW59YCB9XHJcbiAgICB9KTtcclxuXHJcbiAgICByZXR1cm4gbmV4dC5oYW5kbGUoY2xvbmVkUmVxKS5waXBlKFxyXG4gICAgICBjYXRjaEVycm9yKHJlcXVlc3RFcnJvciA9PiB7XHJcbiAgICAgICAgaWYgKHJlcXVlc3RFcnJvciBpbnN0YW5jZW9mIEh0dHBFcnJvclJlc3BvbnNlICYmIHJlcXVlc3RFcnJvci5zdGF0dXMgPT09IDQwMSkge1xyXG4gICAgICAgICAgdGhpcy5yb3V0ZS5uYXZpZ2F0ZUJ5VXJsKCcvYXV0aC9sb2dpbicpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoKCkgPT4gbmV3IEVycm9yKHJlcXVlc3RFcnJvcikpO1xyXG4gICAgICB9KVxyXG4gICAgKTtcclxuICB9XHJcbn1cclxuIl19