import { LoginModel, TokenInfo, User, UserInfo } from '@mix-spa/mix.lib';
import { BehaviorSubject, Observable } from 'rxjs';
import { BaseApiService } from '../../bases/base-api.service';
import * as i0 from "@angular/core";
export declare class AuthApiService extends BaseApiService {
    user$: BehaviorSubject<User | null>;
    logout(callback?: () => void): void;
    login(loginData: LoginModel, apiEncryptKey: string): Observable<TokenInfo>;
    fetchUserInfo(): Observable<UserInfo>;
    get getAccessToken(): string | null;
    get getTokenType(): string | null;
    static ɵfac: i0.ɵɵFactoryDeclaration<AuthApiService, never>;
    static ɵprov: i0.ɵɵInjectableDeclaration<AuthApiService>;
}
