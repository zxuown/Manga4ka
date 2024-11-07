import axios from "axios";

const API = "https://localhost:7242/ratings"

class RatingService {

    static async GetRatings() {
        try {
            const response = await axios.get(API)
            return response.data
        } catch (error) {
            console.error('Error fetching ratings', error)
            throw error
        }
    }

    static async GetAverageRating(mangaId) {
        try {
            const response = await axios.get(`${API}/average/${mangaId}`)
            return response.data
        } catch (error) {
            console.error('Error fetching average ratings', error)
            throw error
        }
    }

    static async IsUserRated(mangaId, userId, token) {
        try {
            const response = await axios.get(`${API}/userRate/${mangaId}/${userId}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.data
        } catch (error) {
            console.error('Error fetching average ratings', error)
            throw error
        }
    }

    static async AddRating(rating, token) {
        try {
            const data = await axios.post(API, rating, {
                headers: {
                    "Content-Type": "application/json",
                    'Authorization': `Bearer ${token}`
                }
            })
            return data.status
        } catch (error) {
            console.error("Error adding rating", error);
            throw error;
        }
    }
}

export default RatingService