import { Component, OnInit } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { WeatherData } from '../models/weather-data';

@Component({
  selector: 'app-api',
  templateUrl: './api.component.html',
  styleUrls: ['./api.component.css']
})
export class ApiComponent implements OnInit {

  data: WeatherData[];

  constructor(private api: WeatherService) { }

  ngOnInit() {
    this.api.getWeather()
      .subscribe(ret => this.data = ret);
  }
}
