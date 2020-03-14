import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }

  async ngOnInit() {
    await this.authService.completeAuthentication();
    this.router.navigate(['/home']);
  }

}
