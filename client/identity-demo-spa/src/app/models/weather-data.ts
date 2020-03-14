export class WeatherData {
    constructor(public temperatureF: number,
        public temperatureC: number,
        private summary: string,
        private date: string) { }
}
