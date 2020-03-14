import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { WeatherData } from '../models/weather-data';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private httpClient: HttpClient, private authService: AuthService) { }

  getWeather(): Observable<WeatherData[]> {
    const authHeaderValue = this.authService.authorizationHeaderValue;
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: authHeaderValue
      })
    };

    return this.httpClient.get<WeatherData[]>('http://localhost:9000/api/weather', httpOptions);
  }
}
