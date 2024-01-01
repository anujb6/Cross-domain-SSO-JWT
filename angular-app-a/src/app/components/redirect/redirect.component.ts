import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-redirect',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.css']
})
export class RedirectComponent {
  constructor(private httpClient: HttpClient) { }

  redirect() {
    const token = localStorage.getItem('AuthToken');
    const url = `https://localhost:7225/api/add/${token}`;

    const headers = new HttpHeaders({
      'responseType': 'text'
    });

    this.httpClient.post(url, null, { headers, withCredentials: true }).subscribe({
      next: (response: any) => {
        window.location.href = "http://localhost:57054/";
      },
      error: (error: any) => {
        console.error('Authentication failed', error);
      }
    });
  }
}
