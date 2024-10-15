import { useContext, useEffect, useState } from "react"
import Swal from 'sweetalert2';
import '@sweetalert2/theme-dark/dark.css';
import MangaService from "../../services/MangaService";
import GenreService from "../../services/GenreService";
import AuthorService from "../../services/AuthorService";
import ImageService from "../../services/ImageService";
import PdfService from "../../services/PdfService"
import ThemeContext from "../../context/ThemeContext";

function MangaCreateForm() {
    const emptyManga = {
        id: '',
        image: '',
        title: '',
        description: '',
        datePublished: '',
        author: {},
        genres: [],
        pdfile: ''
    }

    const [manga, setManga] = useState(emptyManga)
    const [genres, setGenres] = useState([])
    const [authors, setAuthors] = useState([])

    const {darkMode} = useContext(ThemeContext)

    const handleChange = (e) => {
        const { name, value } = e.target
        setManga({
            ...manga,
            [name]: value
        })
    }

    const handleImageChange = async (e) => {
        const file = e.target.files[0]
        if (file) {
            const base64Image = await ImageService.ConvertImageToBase64(file)
            setManga({
                ...manga,
                image: base64Image
            })
        }
    }

    const handlePdfUpload = async (e) => {
        const file = e.target.files[0]
    
        if (file) {
            try {
                const pdfUrl = await PdfService.uploadPdf(file, localStorage.getItem('token'))
                console.log(pdfUrl)
                setManga({
                    ...manga,
                    pdfile: pdfUrl
                });
    
                console.log("PDF uploaded successfully. URL:", pdfUrl);
            } catch (e) {
                console.error("Error uploading the PDF file:", e);
            }
        }
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        manga.id = 0
        manga.author = authors.find(author => author.id == manga.author)
        manga.genres = genres.filter(genre => manga.genres.includes(genre.id.toString()))
        console.log(manga)
        const fetchCreateManga = async () => {
            try {
                const status = await MangaService.CreateManga(manga, localStorage.getItem('token'))
                if (status === 200) {
                    setManga(emptyManga)
                    Swal.fire({
                        title: 'Success!',
                        text: 'You added manga!',
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
        fetchCreateManga()
    }

    const handleGenreChange = (e) => {
        const selectedGenres = Array.from(e.target.selectedOptions, option => option.value)
        setManga({
            ...manga,
            genres: selectedGenres
        })
    }

    useEffect(() => {
        const fetchGenres = async () => {
            try {
                const genres = await GenreService.GetGenres()
                setGenres(genres)
            } catch (e) {
                console.error("Error loading genres", e)
            }
        }

        const fetchAuthors = async () => {
            try {
                const authors = await AuthorService.GetAuthors()
                setAuthors(authors)
            } catch (e) {
                console.error("Error loading authors", e)
            }
        }

        fetchGenres()
        fetchAuthors()
    }, [])

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
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="title" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Title</label>
                        <input
                            type="text"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="title"
                            name="title"
                            value={manga.title}
                            onChange={handleChange}
                            placeholder="Enter Title"
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="description" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Description</label>
                        <textarea
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="description"
                            name="description"
                            value={manga.description}
                            onChange={handleChange}
                            placeholder="Enter Description"
                            rows="3"
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="posted" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Posted Date</label>
                        <input
                            type="date"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="datePublished"
                            name="datePublished"
                            value={manga.datePublished}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <label className={`input-group-text ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Author</label>
                        </div>
                        <select name="author" onChange={handleChange} className={`custom-select ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} id="inputGroupSelect01">
                            <option selected>Choose...</option>
                            {authors.map((author, index) => (
                                <option key={index} value={author.id}>{author.name} {author.secondName}</option>
                            ))}
                        </select>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="genres" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Genres</label>
                        <select
                            multiple
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="genres"
                            name="genres"
                            value={manga.genres}
                            onChange={handleGenreChange}
                        >
                            {genres.map((genre) => (
                                <option key={genre.id} value={genre.id}>
                                    {genre.title}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="pdfile" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Manga PDF File</label>
                        <input
                            type="file"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            accept=".pdf"
                            onChange={handlePdfUpload} 
                        />
                    </div>

                    <button type="submit" className="btn btn-primary">Submit</button>
                </form>
            </div>
        </>
    )
}

export default MangaCreateForm