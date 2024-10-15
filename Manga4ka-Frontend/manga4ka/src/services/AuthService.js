import axios from "axios";
import { jwtDecode } from "jwt-decode";

const API = 'https://localhost:7242/account';

class AuthService {

    static isTokenExpired(token) {
        if (!token) return true;

       const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000;
        console.log(decodedToken)
       return decodedToken.exp < currentTime;
    }


    static async Register(registerData) {
        try {
            const response = await axios.post(`${API}/register`, registerData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            return response.status
        } catch (error) {
            console.error('Register error', error)
            throw error
        }
    }

    static async Login(loginData) {
        try {
            const response = await axios.post(`${API}/login`, loginData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            localStorage.setItem("token", response.data.token)
            return response.status
        } catch (error) {
            console.error('Login error', error)
            throw error
        }
    }

    static async GetCurrentUser() {
        try {
            const token = localStorage.getItem("token")
            const response = await axios.get(`${API}/currentUser`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.data
        } catch (error) {
            console.error('Error getting current user', error)
            throw error
        }
    }

    static Logout() {
        try {
            localStorage.removeItem("token")
        } catch (error) {
            console.error('Logout error', error)
            throw error
        }
    }
}

export default AuthService