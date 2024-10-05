import Swal from 'sweetalert2'
import '@sweetalert2/theme-dark/dark.css'
import { useContext, useState } from "react"
import AuthorService from '../../services/AuthorService'
import ImageService from '../../services/ImageService'
import ThemeContext from '../../context/ThemeContext'

function AuthorCreateForm() {
    const emptyAuthor = {
        id: 0,
        image: '',
        name: '',
        lastname: '',
        dateOfBirth: '',
        description: ''
    }

    const [author, setAuthor] = useState(emptyAuthor)

    const {darkMode} = useContext(ThemeContext)

    const handleChange = (e) => {
        const { name, value } = e.target
        setAuthor({
            ...author,
            [name]: value
        })
    }

    const handleImageChange = async (e) => {
        const file = e.target.files[0]
        if (file) {
            const base64Image = await ImageService.ConvertImageToBase64(file)
            setAuthor({
                ...author,
                image: base64Image
            })
        }
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        author.id = 0
        console.log(author)
        const fetchCreateAuthor = async () => {
            try {
                const status = await AuthorService.CreateAuthor(author)
                if (status === 200) {
                    setAuthor(emptyAuthor)
                    Swal.fire({
                        title: 'Success!',
                        text: 'You added author!',
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
        fetchCreateAuthor()
    }

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
                        <label htmlFor="name" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Name</label>
                        <input
                            type="text"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="name"
                            name="name"
                            value={author.name}
                            onChange={handleChange}
                            placeholder="Enter Title"
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="lastname" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Lastname</label>
                        <input
                            type="text"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="lastname"
                            name="lastname"
                            value={author.lastname}
                            onChange={handleChange}
                            placeholder="Enter Second name"
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="dateOfBirth" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Date Of Birth</label>
                        <input
                            type="date"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="dateOfBirth"
                            name="dateOfBirth"
                            value={author.dateOfBirth}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="description" className={`form-label ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Description</label>
                        <textarea
                            className={`form-control ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}
                            id="description"
                            name="description"
                            value={author.description}
                            onChange={handleChange}
                            placeholder="Enter Description"
                            rows="3"
                            required
                        />
                    </div>
                    <button type="submit" className="btn btn-primary">Submit</button>
                </form>
            </div>
        </>
    )
}

export default AuthorCreateForm