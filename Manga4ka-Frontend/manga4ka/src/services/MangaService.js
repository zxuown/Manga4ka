import axios from "axios"

const API = 'https://localhost:7242/manga'

class MangaService {

    static async SearchManga(query){
        try{
            const response = await axios.get(`${API}/search?query=${query}`)
            return response.data
        }catch(error){
            console.error('Error fetching all manga', error)
            throw error
        }
    }

    static async GetAllManga() {
        try {
            const response = await axios.get(API)
            return response.data
        } catch (error) {
            console.error('Error fetching all manga', error)
            throw error
        }
    }

    static async GetManga(id) {
        try {
            const response = await axios.get(`${API}/${id}`)
            return response.data
        } catch (error) {
            console.error('Error fetching manga', error)
            throw error
        }
    }

    static async CreateManga(manga) {
        try {
            const response = await axios.post(`${API}`, JSON.stringify(manga), {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            return response.status
        } catch (error) {
            console.error('Error creating manga', error)
            throw error
        }
    }

    static async UpdateManga(id, manga) {
        try {
            const response = await axios.put(`${API}/${id}`, manga, {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            return response.status
        } catch (error) {
            console.error('Error updating manga', error)
            throw error
        }
    }

    static async DeleteManga(id) {
        try {
            const response = await axios.delete(`${API}/${id}`)
            return response.status
        } catch (error) {
            console.error('Error deleting manga', error)
            throw error
        }
    }
}

export default MangaService