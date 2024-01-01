import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http'; // Import HttpClient

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private http: HttpClient
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    if (this.loginForm.valid) {
      const username = this.loginForm.get('username')?.value;
      const password = this.loginForm.get('password')?.value;

      const encodedUsername = username;
      const encodedPassword = password;

      const url = `https://localhost:7225/api/auth?username=${encodedUsername}&password=${encodedPassword}`;

      this.http.post(url, {}, { responseType: 'text' }).subscribe({
        next: (response: string) => {
          console.log(response);
          localStorage.setItem('AuthToken', response);
          this.router.navigate(['/redirect']);
        },
        error: (error: any) => {
          console.error('Authentication failed', error);
        }
      });
    }
  }
}
