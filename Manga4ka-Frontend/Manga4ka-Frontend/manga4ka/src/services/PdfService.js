import axios from "axios";

const API = "https://localhost:7242/pdffile"

class PdfService {
    static async uploadPdf(file, token) {
        const formData = new FormData();
        formData.append('file', file);
        try {
            const response = await axios.post(`${API}/upload`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    'Authorization': `Bearer ${token}`
                }
            });
            return response.data.url;
        } catch (error) {
            console.error("Error uploading PDF file", error);
            throw error;
        }
    }

    static async deletePdf(mangaId, token){
        try{
            const response = await axios.delete(`${API}/delete/${mangaId}`, {
                headers:{
                    'Authorization': `Bearer ${token}`
                }
            })
            return response.status
        }catch(error){
            console.error('Error deleting pdf file', error)
            throw error
        }
    }
};

export default PdfService;
