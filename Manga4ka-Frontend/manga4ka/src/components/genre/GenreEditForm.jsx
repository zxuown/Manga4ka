import Swal from 'sweetalert2';
import '@sweetalert2/theme-dark/dark.css';
import { useContext, useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import GenreService from '../../services/GenreService';
import ImageService from '../../services/ImageService';
import ThemeContext from '../../context/ThemeContext';

function GenreEditForm() {
    const [genre, setGenre] = useState({})
    const { genreId } = useParams()

    const { darkMode } = useContext(ThemeContext)

    const handleChange = (e) => {
        const { name, value } = e.target
        setGenre({
            ...genre,
            [name]: value
        })
    }

    const handleImageChange = async (e) => {
        const file = e.target.files[0]
        if (file) {
            const base64Image = await ImageService.ConvertImageToBase64(file)
            setGenre({
                ...genre,
                image: base64Image
            })
        }
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        const fetchEditGenre = async () => {
            try {
                const status = await GenreService.EditGenre(genreId, genre)
                if (status === 200) {
                    setGenre({});
                    Swal.fire({
                        title: 'Success!',
                        text: 'You edited a genre!',
                        icon: 'success',
                        confirmButtonText: 'Close'
                    })
                }
            } catch (e) {
                Swal.fire({
                    title: `Error : ${e}!`,
                    text: 'Something went wrong!',
                    icon: 'error',
                    confirmButtonText: 'Close'
                })
            }
        }
        fetchEditGenre()
    }

    useEffect(() => {
        const fetchGenre = async () => {
            try {
                const genre = await GenreService.GetGenre(genreId)
                setGenre(genre)
            }
            catch (e) {
                Swal.fire({
                    title: `Error : ${e}!`,
                    text: 'Something went wrong!',
                    icon: 'error',
                    confirmButtonText: 'Close'
                })
            }
        }
        fetchGenre()
    }, [genreId])

    return (
        <>
            <div className="container">
                <form onSubmit={handleSubmit} className={`mt-4 manga-form ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                    <div className="mb-3">
                        <label htmlFor="image" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Image URL</label>
                        <input
                            type="file"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="image"
                            name="image"
                            onChange={handleImageChange}
                            placeholder="Enter Image URL"
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="title" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Title</label>
                        <input
                            type="text"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="title"
                            name="title"
                            value={genre.title}
                            onChange={handleChange}
                            placeholder="Enter Title"
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="description" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Description</label>
                        <textarea
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="description"
                            name="description"
                            value={genre.description}
                            onChange={handleChange}
                            placeholder="Enter Description"
                            rows="3"
                        />
                    </div>
                    <button type="submit" className="btn btn-primary">Submit</button>
                </form>
            </div>
        </>

    )
}

export default GenreEditForm