import axios from "axios"

const API = 'https://localhost:7242/authors'

class AuthorService {

    static async SearchAuthors(query) {
        try {
            const response = await axios.get(`${API}/search?query=${query}`)
            return response.data
        } catch (error) {
            console.error('Error finding authors', error)
            throw error
        }
    }

    static async GetAuthors() {
        try {
            const response = await axios.get(API)
            return response.data
        } catch (error) {
            console.error('Error fetching authors', error)
            throw error
        }
    }

    static async GetAuthor(id) {
        try {
            const response = await axios.get(`${API}/${id}`)
            return response.data
        } catch (error) {
            console.error('Error fetching author', error)
            throw error
        }
    }

    static async CreateAuthor(author, token) {
        try {
            const response = await axios.post(`${API}`, author, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error creating author', error)
            throw error
        }
    }

    static async UpdateAuthor(id, author, token) {
        try {
            const response = await axios.put(`${API}/${id}`, author, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error updating author', error)
            throw error
        }
    }

    static async DeleteAuthor(id, token) {
        try {
            const response = await axios.delete(`${API}/${id}`,{
                headers: { 
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error deleting author', error)
            throw error
        }
    }
}

export default AuthorService