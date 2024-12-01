import React, { useEffect, useState } from "react";
import { fetchWeather } from "./services/api";
import { Button, DatePicker } from 'antd';

// Define a TypeScript interface for the weather data structure
interface WeatherData {
  date: string; // Assuming date is a string, adjust if it's a different type
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

const App: React.FC = () => {
  // Specify the type of the weather state
  const [weather, setWeather] = useState<WeatherData[]>([]);

  useEffect(() => {
    const loadData = async () => {
      const data = await fetchWeather();
      setWeather(data);
    };
    loadData();
  }, []);

  return (
      <div>
        <h1>Weather Forecast</h1>
        <ul>
          {weather.map((item, index) => (
              <li key={index}>{item.summary}</li>
          ))}
        </ul>

        <Button type="primary">PRESS ME</Button>
        <DatePicker placeholder="select date" />

      </div>
  );
};

export default App;
