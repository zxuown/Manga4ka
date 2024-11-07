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
                <div className={`card mb-4 shadow-lg ${darkMode ? 'bg-dark text-white border-secondary' : 'bg-light'}`} style={{ borderRadius: '12px', overflow: 'hidden' }}>
                    <div className="row g-0">
                        <div className="col-md-3 d-flex align-items-center justify-content-center p-3" style={{ backgroundColor: darkMode ? '#333' : '#f8f9fa' }}>
                            <img src={author.image} className="img-fluid rounded-circle" style={{ maxWidth: '120px', border: `4px solid ${darkMode ? '#444' : '#ddd'}` }} alt={author.name} />
                        </div>
                        <div className="col-md-9">
                            <div className={`card-body ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                                <h4 className="card-title fw-bold mb-2">{`${author.name} ${author.lastname}`}</h4>
                                <p className="card-text"><strong>Date of Birth:</strong> {new Date(author.dateOfBirth).toLocaleDateString()}</p>
                                <p className="card-text" style={{ fontSize: '1rem', lineHeight: '1.6' }}>{author.description}</p>
                            </div>
                        </div>
                    </div>
                </div>

                <h2 className={`fs-3 fw-semibold ${darkMode ? 'text-light' : 'text-dark'} mb-4`}>{`${author.name} ${author.lastname}'s Works:`}</h2>

                <Manga />
            </div>
        </>



    );
}

export default Author;
