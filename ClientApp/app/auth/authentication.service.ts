import { Injectable } from '@angular/core';
import { Repository } from '../models/repository';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { Router } from '@angular/router';

@Injectable()
export class AuthenticationService {
  constructor(private repo: Repository, private router: Router) { }

  authenticated = false;
  name: string;
  password: string;
  callbackUrl: string;

  login(): Observable<any> {
    this.authenticated = false;
    return this.repo.login(this.name, this.password)
      .map(response => {
        if (response.ok) {
          this.authenticated = true;
          this.password = null;
          this.router.navigateByUrl(this.callbackUrl || "/admin/overview");
        }
        return this.authenticated;
      })
      .catch((e: any)   => {
        this.authenticated = false;
        return Observable.of(false);
  
      });
  }

  logout(): void {
    this.authenticated = false;
    this.repo.logout();
    this.router.navigateByUrl("/login");
  }
}
