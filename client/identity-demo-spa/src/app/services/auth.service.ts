import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UserManager, UserManagerSettings, User } from 'oidc-client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authStatusSource = new BehaviorSubject<boolean>(false);
  private userNameStatusSource = new BehaviorSubject<string>('');
  private userManager = new UserManager(this.getUserManagerSettings());
  private user: User | null;

  authStatus$ = this.authStatusSource.asObservable();
  userNameStatus$ = this.userNameStatusSource.asObservable();

  constructor() {
    this.userManager.getUser().then(user => {
      this.user = user;
      this.authStatusSource.next(this.isAuthenticated());
      this.userNameStatusSource.next(this.user ? this.user.profile.name : '');
    });
  }

  async login() {
    await this.userManager.signinRedirect();
  }

  async logout() {
    await this.userManager.signoutRedirect();
  }

  async completeAuthentication() {
    this.user = await this.userManager.signinRedirectCallback();
    this.authStatusSource.next(this.isAuthenticated());
    this.userNameStatusSource.next(this.user.profile.name);
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  get authorizationHeaderValue(): string {
    if (this.user != null) {
      console.log(this.user.access_token);
      return `${this.user.token_type} ${this.user.access_token}`;
    }

    return '';
  }

  private getUserManagerSettings(): UserManagerSettings {
    return {
      authority: 'http://localhost:7889',
      client_id: 'angular',
      redirect_uri: 'http://localhost:4200/auth-callback',
      post_logout_redirect_uri: 'http://localhost:4200/',
      response_type: 'id_token token',
      scope: 'openid profile email api.weather.full_access',
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
    };
  }
}
