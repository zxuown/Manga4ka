class ImageService {
    static async ConvertImageToBase64(image) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader()
            reader.onloadend = () => {
                resolve(reader.result)
            }
            reader.onerror = (error) => reject(error)
            reader.readAsDataURL(image)
        })
    }
}

export default ImageService