import { Injectable } from '@angular/core';
import { CanActivate, Router} from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AletifyService } from '../_services/alertify.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private alertify: AletifyService ) {}
  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.alertify.error('You shall not pass!!!');
    this.router.navigate(['/home']);
    return false;
  }
}
