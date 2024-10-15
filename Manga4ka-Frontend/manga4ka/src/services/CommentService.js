import axios from "axios"

const API = 'https://localhost:7242/comments'

class CommentService {

    static async GetAllCommentsByMangaId(mangaId) {
        try {
            const response = await axios.get(`${API}/mangaId/${mangaId}`)
            return response.data
        } catch (error) {
            console.error('Error fetching all comments', error)
            throw error
        }
    }

    static async GetComment(id) {
        try {
            const response = await axios.get(`${API}/${id}`)
            return response.data
        } catch (error) {
            console.error('Error fetching comment', error)
            throw error
        }
    }

    static async CreateComment(comment, token) {
        try {
            const response = await axios.post(`${API}`, JSON.stringify(comment), {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error creating comment', error)
            throw error
        }
    }

    static async UpdateComment(id, comment, token) {
        try {
            const response = await axios.put(`${API}/${id}`, comment, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error updating comment', error)
            throw error
        }
    }

    static async DeleteComment(id, token) {
        try {
            const response = await axios.delete(`${API}/${id}`,{
                headers: { 
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error deleting comment', error)
            throw error
        }
    }
}

export default CommentService