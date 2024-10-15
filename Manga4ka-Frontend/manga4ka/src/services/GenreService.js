import axios from "axios"

const API = 'https://localhost:7242/genres'

class GenreService {

    static async SearchGenres(query) {
        try {
            const response = await axios.get(`${API}/search?query=${query}`)
            return response.data
        } catch (error) {
            console.error('Error finding genres', error)
            throw error
        }
    }

    static async GetGenres() {
        try {
            const response = await axios.get(API)
            return response.data
        } catch (error) {
            console.error('Error fetching genres', error)
            throw error
        }
    }

    static async GetGenre(id) {
        try {
            const response = await axios.get(`${API}/${id}`)
            return response.data
        } catch (error) {
            console.error('Error fetching genre', error)
            throw error
        }
    }

    static async CreateGenre(genre, token) {
        try {
            const response = await axios.post(`${API}`, genre, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error posting genre', error)
            throw error
        }
    }

    static async EditGenre(id, genre, token) {
        try {
            const response = await axios.put(`${API}/${id}`, genre, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error editing genre', error)
            throw error
        }
    }

    static async DeleteGenre(id, token) {
        try {
            console.log(token)
            const response = await axios.delete(`${API}/${id}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error deleting genre', error)
            throw error
        }
    }
}

export default GenreService