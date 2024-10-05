import { useContext, useEffect, useState } from "react";
import Manga from "../manga/Manga";
import { useParams } from "react-router-dom";
import ThemeContext from "../../context/ThemeContext";
import Rating from 'react-rating-stars-component'
import Swal from 'sweetalert2';
import AuthorService from '../../services/AuthorService'

function Author() {
    const [author, setAuthor] = useState({})
    const [rating, setRating] = useState(0)
    const { authorId } = useParams()
    const { darkMode } = useContext(ThemeContext)


    useEffect(() => {
        const fetchAuthor = async () => {
            try {
                const author = await AuthorService.GetAuthor(authorId)
                setAuthor(author)
            } catch (e) {
                console.error("Error loading author", e)
            }
        }
        fetchAuthor() 
    }, [authorId])

    const handleRatingChange = (newRating) => {
        setRating(newRating)
        Swal.fire({
            title: 'Success!',
            text: 'Thank you for your oppinion!',
            icon: 'success',
            confirmButtonText: 'Close'
        })
    }

    return (
        <>
            <div className="container mt-4">
                <div className={`${rating > 0 ? 'd-none' : ''} d-flex justify-content-center align-items-center`}>
                    <span className="me-2">Rate the author:</span>
                    <Rating
                        count={5}
                        value={0}
                        size={34}
                        activeColor="#ffd700"
                        onChange={handleRatingChange}
                    />
                </div>
                <div className={`card mb-3 ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                    <div className="row g-0">
                        <div className="col-md-2">
                            <img src={author.image} className="img-fluid rounded-start" alt={author.name} />
                        </div>
                        <div className="col-md-8">
                            <div className={`card-body ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                                <h5 className={`card-title ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>{author.name} {author.lastname}</h5>
                                <p className="card-text"><strong>Date of Birth:</strong> {author.dateOfBirth}</p>
                                <p className="card-text">{author.description}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <h1 className={`${darkMode ? 'text-white' : ''}`}>{author.name + ' ' + author.lastname} works:</h1>
            </div>
            <Manga></Manga>
        </>


    );
}

export default Author;
