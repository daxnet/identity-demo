import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'identity-demo-spa';

  isAuthenticated: boolean;
  authStatusSubscription: Subscription;
  userNameSubscription: Subscription;
  userName: string;
  
  constructor(private authService: AuthService) { }

  ngOnDestroy(): void {
    this.authStatusSubscription.unsubscribe();
    this.userNameSubscription.unsubscribe();
  }

  ngOnInit(): void {
    this.authStatusSubscription = this.authService.authStatus$.subscribe(status => this.isAuthenticated = status);
    this.userNameSubscription = this.authService.userNameStatus$.subscribe(status => this.userName = status);
  }

  async onLogin() {
    await this.authService.login();
  }

  async onLogOut() {
    await this.authService.logout();
  }
}
