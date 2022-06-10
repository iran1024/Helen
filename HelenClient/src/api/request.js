import axios from "axios";

const service = axios.create({
    baseURL: 'http://127.0.0.1:4523/mock/874533',
    timeout: 216000,
    withCredentials: false,
});

export default service;