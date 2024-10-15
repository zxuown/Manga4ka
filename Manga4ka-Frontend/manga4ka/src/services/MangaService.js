import axios from "axios"

const API = 'https://localhost:7242/manga'

class MangaService {

    static async SearchManga(query) {
        try {
            const response = await axios.get(`${API}/search?query=${query}`)
            return response.data
        } catch (error) {
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

    static async CreateManga(manga, token) {
        try {
            const response = await axios.post(`${API}`, JSON.stringify(manga), {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error creating manga', error)
            throw error
        }
    }

    static async UpdateManga(id, manga, token) {
        try {
            const response = await axios.put(`${API}/${id}`, manga, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error updating manga', error)
            throw error
        }
    }

    static async DeleteManga(id, token) {
        try {
            const response = await axios.delete(`${API}/${id}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        } catch (error) {
            console.error('Error deleting manga', error)
            throw error
        }
    }

    static async SortMangaByPublishedAsc() {
        try {
            const response = await axios.get(`${API}/sortMangaByPublishedAsc`)
            return response.data
        } catch (error) {
            console.error('Error sorting manga by published asc', error)
            throw error
        }
    }

    static async SortMangaByPublishedDesc() {
        try {
            const response = await axios.get(`${API}/sortMangaByPublishedDesc`)
            return response.data
        } catch (error) {
            console.error('Error sorting manga by published desc', error)
            throw error
        }
    }

    static async SortMangaByRatingAsc() {
        try {
            const response = await axios.get(`${API}/sortMangaByRatingAsc`)
            return response.data
        } catch (error) {
            console.error('Error sorting manga by rating asc', error)
            throw error
        }
    }

    static async SortMangaByRatingDesc() {
        try {
            const response = await axios.get(`${API}/sortMangaByRatingDesc`)
            return response.data
        } catch (error) {
            console.error('Error sorting manga by rating des', error)
            throw error
        }
    }

    static async SortMangaByFavorite(userId) {
        try {
            const response = await axios.get(`${API}/sortMangaByFavorite/${userId}`)
            return response.data
        } catch (error) {
            console.error('Error sorting manga by rating des', error)
            throw error
        }
    }

    static async GetAllUserFavoriteManga() {
        try {
            const token = localStorage.getItem("token")
            const response = await axios.get(`${API}/favoriteManga`, {
                headers:
                {
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.data
        } catch (error) {
            console.error('Error getting favorite manga by user', error)
            throw error
        }
    }

    static async AddFavoriteManga(favoriteManga) {
        try {
            const response = await axios.post(`${API}/favoriteManga`, favoriteManga, {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            return response.status
        } catch (error) {
            console.error('Error adding favorite manga', error)
            throw error
        }
    }

    static async DeleteFavoriteManga(mangaId, userId) {
        try {
            const response = await axios.delete(`${API}/favoriteManga/${mangaId}/${userId}`)
            return response.status
        } catch (error) {
            console.error('Error deleting favorite manga', error)
            throw error
        }
    }
}

export default MangaService