// src/App.js
import React, { useEffect, useState } from "react";
import { fetchWeather } from "./services/api";

const App = () => {
    const [weather, setWeather] = useState([]);

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
        </div>
    );
};

export default App;
