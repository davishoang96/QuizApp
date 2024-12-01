import axios from "axios";

const API_BASE_URL = "https://localhost:3477";

export const fetchWeather = async () => {
    const response = await axios.get(`${API_BASE_URL}/weatherforecast`);
    return response.data;
};
