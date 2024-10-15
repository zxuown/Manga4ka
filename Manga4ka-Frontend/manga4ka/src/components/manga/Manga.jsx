import { useEffect, useState } from "react"
import Swal from 'sweetalert2';
import Rating from 'react-rating-stars-component'
import { useCallback, useContext } from "react";
import { Link, useParams } from "react-router-dom";
import ThemeContext from "../../context/ThemeContext";
import AuthContext from "../../context/AuthContext";
import MangaService from "../../services/MangaService";
import PdfService from "../../services/PdfService";
import PropTypes from 'prop-types';
import Favorite from "../favorite/Favorite";
import RatingService from "../../services/RatingService";

function Manga({ filteredManga }) {
    let { genreId, authorId } = useParams()
    const [manga, setManga] = useState([])
    const [ratings, setRatings] = useState({})
    const [favoriteManga, setFavoriteManga] = useState([])
    const { darkMode } = useContext(ThemeContext)
    const { user } = useContext(AuthContext)

    const fetchAverageRating = async (mangaId) => {
        try {
            const value = await RatingService.GetAverageRating(mangaId)
            console.log(value)
            return value
        } catch (e) {
            console.error("Error loading average rating", e)
        }
    }

    const handleSortMangaByPublishedAsc = async () => {
        try {
            const data = await MangaService.SortMangaByPublishedAsc()
            setManga(data)
        } catch (e) {
            console.error("Error sorting manga by published asc", e)
        }
    }

    const handleSortMangaByPublishedDesc = async () => {
        try {
            const data = await MangaService.SortMangaByPublishedDesc()
            setManga(data)
        } catch (e) {
            console.error("Error sorting manga by published desc", e)
        }
    }

    const handleSortMangaByRatingAsc = async () => {
        try {
            const data = await MangaService.SortMangaByRatingAsc()
            setManga(data)
        } catch (e) {
            console.error("Error sorting manga by published desc", e)
        }
    }

    const handleSortMangaByRatingDesc = async () => {
        try {
            const data = await MangaService.SortMangaByRatingDesc()
            console.log(data)
            setManga(data)
        } catch (e) {
            console.error("Error sorting manga by published desc", e)
        }
    }

    const handleSortMangaByFavorite = async () => {
        try {
            const data = await MangaService.SortMangaByFavorite(user.id)
            setManga(data)
        } catch (e) {
            console.error("Error sorting manga by published desc", e)
        }
    }

    const handleDelete = useCallback((id) => {
        const fetchMangaDelete = async () => {
            try {
                const fileStatus = await PdfService.deletePdf(id, localStorage.getItem('token'))
                const mangaStatus = await MangaService.DeleteManga(id, localStorage.getItem('token'))
                if (mangaStatus === 200 && fileStatus === 200) {
                    setManga(prevManga => prevManga.filter(item => item.id !== id))
                    Swal.fire({
                        title: 'Success!',
                        text: 'You deleted manga!',
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

        fetchMangaDelete()
    }, [])

    useEffect(() => {
        if (filteredManga) {
            setManga(filteredManga)
        } else {
            const fetchAllManga = async () => {
                try {
                    const manga = await MangaService.GetAllManga()
                    if (genreId) {
                        const filteredManga = manga.filter(manga =>
                            manga.genres.some(genre => genre.id == genreId)
                        )
                        setManga(filteredManga)
                    } else if (authorId) {
                        const filteredManga = manga.filter(manga =>
                            manga.author.id == authorId
                        )
                        setManga(filteredManga);
                    } else {
                        setManga(manga);
                    }
                } catch (e) {
                    console.error("Error loading all manga", e)
                }
            }

            fetchAllManga()
        }
        const fetchAllUserFavoriteManga = async () => {
            if (localStorage.getItem("token")) {
                try {
                    const data = await MangaService.GetAllUserFavoriteManga()
                    console.log(data)
                    setFavoriteManga(data)
                } catch (e) {
                    console.error("Error loading all user favorite manga", e)
                }
            }
        }
        fetchAllUserFavoriteManga()
    }, [filteredManga, genreId, authorId])

    useEffect(() => {
        const fetchRatings = async () => {
            const mangaRatings = {}
            for (let item of manga) {
                mangaRatings[item.id] = await fetchAverageRating(item.id)
            }
            setRatings(mangaRatings)
        }
        if (manga.length > 0) {
            fetchRatings()
        }
    }, [manga])

    const isMangaFavorite = (mangaId) => {
        return favoriteManga.some(favManga => favManga.manga.id === mangaId)
    }

    return (
        <div className='row justify-content-center'>
            {
                user !== undefined && Array.isArray(user.roles) && user.roles.some(x => x === "Admin") ?
                    <Link to="/manga/create" className="card-link btn btn-primary">Create</Link> : ""
            }

            <div className="d-flex mt-3 justify-content-center">
                <div className="dropdown">
                    <button className={`btn dropdown-toggle ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        Sort by...
                    </button>
                    <ul className={`dropdown-menu ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                        <li><button onClick={handleSortMangaByPublishedAsc} className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Published (asc)</button></li>
                        <li><button onClick={handleSortMangaByPublishedDesc} className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} href="#">Published (desc)</button></li>
                        <li><button onClick={handleSortMangaByRatingAsc} className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} href="#">Rating (asc)</button></li>
                        <li><button onClick={handleSortMangaByRatingDesc} className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} href="#">Rating (desc)</button></li>
                        <li><button onClick={handleSortMangaByFavorite} className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} href="#">Favorite</button></li>
                    </ul>
                </div>
            </div>

            {manga.map((item, index) => (
                <div key={index} className={`card mb-3 m-3 col-md-4 ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} style={{ maxWidth: 400 }}>
                    <div className="row g-0">
                        <Link className="col-md-4" to={"/" + item.id}>
                            <img src={item.image} style={{ height: 200 }} className="img-fluid rounded-start" alt="..." />
                        </Link>
                        <div className="col-md-8">
                            <div className={`card-body ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                                <div className="d-flex">
                                    <h5 className={`me-3 card-title ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>{item.title}</h5>
                                    <Favorite manga={item}
                                        isFavorite={isMangaFavorite(item.id)}>
                                    </Favorite>
                                </div>
                                <p className="card-text">Author: {item.author.name}</p>
                                <p className="card-text">{item.description}<br />Genres:</p>
                                {item.genres.map((genre, idx) => (
                                    <span key={idx} className="badge rounded-pill text-bg-primary me-1">
                                        {genre.title}
                                    </span>
                                ))}
                                {ratings[item.id] !== undefined && (
                                    <Rating
                                        count={5}
                                        value={ratings[item.id]}
                                        size={34}
                                        activeColor="#ffd700"
                                        className="ms-2"
                                        edit={false}
                                    />
                                )}

                                <p className={`card-text`}>
                                    <small className={`${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Published: {new Date(item.datePublished).toLocaleDateString('de-DE')}</small>
                                </p>
                                {
                                     Array.isArray(user.roles) && user.roles.some(x => x === "Admin") ?
                                        <div className="card-footer">
                                            <Link to={`/manga/edit/${item.id}`} className="card-link btn btn-warning">Edit</Link>
                                            <a onClick={() => handleDelete(item.id)} className="card-link btn btn-danger">Delete</a>
                                        </div> : ""
                                }
                            </div>
                        </div>
                    </div>
                </div>))}

        </div>
    )
}

Manga.propTypes = {
    filteredManga: PropTypes.arrayOf(PropTypes.object),
}

export default Manga