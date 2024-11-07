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
    const [ratings, setRatings] = useState([])
    const [favoriteManga, setFavoriteManga] = useState([])
    const [mangaGenres, setMangaGenres] = useState([])
    const [publishedSort, setPublishedSort] = useState(true)
    const [ratingSort, setRatingSort] = useState(true)
    const { darkMode } = useContext(ThemeContext)
    const { user } = useContext(AuthContext)

    const fetchAverageRating = async (mangaId) => {
        try {
            const value = await RatingService.GetAverageRating(mangaId)
            return value
        } catch (e) {
            console.error("Error loading average rating", e)
        }
    }

    const handleSortMangaByPublishedAsc = async () => {
        try {
            const data = await MangaService.SortMangaByPublishedAsc()
            setManga(data)
            setPublishedSort(false)
        } catch (e) {
            console.error("Error sorting manga by published asc", e)
        }
    }

    const handleSortMangaByPublishedDesc = async () => {
        try {
            const data = await MangaService.SortMangaByPublishedDesc()
            setManga(data)
            setPublishedSort(true)
        } catch (e) {
            console.error("Error sorting manga by published desc", e)
        }
    }

    const handleSortMangaByRatingAsc = async () => {
        try {
            const data = await MangaService.SortMangaByRatingAsc()
            setManga(data)
            setRatingSort(false)
        } catch (e) {
            console.error("Error sorting manga by published desc", e)
        }
    }

    const handleSortMangaByRatingDesc = async () => {
        try {
            const data = await MangaService.SortMangaByRatingDesc()
            setManga(data)
            setRatingSort(true)
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
                    const mangaGenres = await MangaService.GetMangaGenres()
                    if (genreId) {
                        const filteredManga = manga.filter(x => mangaGenres.some(mangaGenre => mangaGenre.manga.id == x.id && mangaGenre.genre.id == genreId))
                        setManga(filteredManga)
                    } else if (authorId) {
                        const filteredManga = manga.filter(manga =>
                            manga.author.id == authorId
                        )
                        setManga(filteredManga);
                    } else {
                        setManga(manga);
                    }
                    setMangaGenres(mangaGenres)
                } catch (e) {
                    console.error("Error loading all manga or manga genres", e)
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
        const fetchRatings = async (mangaData) => {
            const mangaRatings = []
            for (let item of mangaData) {
                mangaRatings.push({ id: item.id, rating: await fetchAverageRating(item.id) })
            }
            setRatings(mangaRatings)
        }

        if (filteredManga && filteredManga.length > 0) {
            fetchRatings(filteredManga)
        } else if (manga.length > 0) {
            fetchRatings(manga)
        }
    }, [filteredManga, manga])

    const isMangaFavorite = (mangaId) => {
        return favoriteManga.some(favManga => favManga.manga.id === mangaId)
    }

    return (
        <div className="row justify-content-center">
            {user && Array.isArray(user.roles) && user.roles.includes("Admin") && (
                <Link to="/manga/create" className="btn btn-success mb-3">+ Create Manga</Link>
            )}

            <div className="d-flex mt-3 justify-content-center">
                <div className="dropdown">
                    <button
                        className={`btn dropdown-toggle ${darkMode ? 'bg-secondary text-white' : 'bg-light'}`}
                        type="button"
                        data-bs-toggle="dropdown"
                        aria-expanded="false">
                        Sort by...
                    </button>
                    <ul className={`dropdown-menu ${darkMode ? 'bg-secondary text-white' : 'bg-light'}`}>
                        {publishedSort ? <li><button onClick={handleSortMangaByPublishedAsc} className={`dropdown-item ${darkMode ? 'text-white' : 'text-dark'}`}>Published (asc)</button></li> : <li><button onClick={handleSortMangaByPublishedDesc} className={`dropdown-item ${darkMode ? 'text-white' : 'text-dark'}`}>Published (desc)</button></li>}
                        {ratingSort ?
                            <li><button onClick={handleSortMangaByRatingAsc} className={`dropdown-item ${darkMode ? 'text-white' : 'text-dark'}`}>Rating (asc)</button></li>
                            : <li><button onClick={handleSortMangaByRatingDesc} className={`dropdown-item ${darkMode ? 'text-white' : 'text-dark'}`}>Rating (desc)</button></li>}
                        <li><button onClick={handleSortMangaByFavorite} className={`dropdown-item ${darkMode ? 'text-white' : 'text-dark'}`}>Favorite</button></li>
                    </ul>
                </div>
            </div>

            {manga.map((item, index) => (
                <div
                    key={index}
                    className={`card mb-4 m-3 col-md-4 ${darkMode ? 'bg-dark text-white border-secondary' : 'bg-light text-dark shadow-sm'}`}
                    style={{ maxWidth: 400, borderRadius: '15px', overflow: 'hidden' }}>
                    <div className="row g-0">
                        <Link className="col-md-4" to={`/${item.id}`}>
                            <img
                                src={item.image}
                                style={{ height: 200, borderRadius: '15px 0 0 15px' }}
                                className="img-fluid"
                                alt={`${item.title} cover`} />
                        </Link>
                        <div className="col-md-8">
                            <div className="card-body">
                                <div className="d-flex align-items-center mb-2">
                                    <h5 className={`me-3 card-title ${darkMode ? 'text-light' : 'text-dark'}`}>{item.title}</h5>
                                    <Favorite manga={item} isFavorite={isMangaFavorite(item.id)} />
                                </div>
                                <p className="card-text mb-1">Author: {item.author.name}</p>
                                <p className="card-text mb-1">{item.description}</p>
                                <div>
                                    {mangaGenres.filter(x => x.manga.id === item.id).map((mangaGenre, idx) => (
                                        <span key={idx} className="badge rounded-pill bg-primary me-1 mb-1">
                                            {mangaGenre.genre.title}
                                        </span>
                                    ))}
                                </div>
                                {ratings && ratings.length > 0 && (
                                    <Rating
                                        key={item.id}
                                        count={5}
                                        value={ratings.find(x => x.id === item.id)?.rating || 0}
                                        size={20}
                                        activeColor="#ffd700"
                                        className="ms-2 mt-2"
                                        edit={false}
                                    />
                                )}
                                <p className="card-text mt-2">
                                    <small className={`${darkMode ? 'text-white ' : ''}`}>Published: {new Date(item.datePublished).toLocaleDateString('de-DE')}</small>
                                </p>
                                {Array.isArray(user.roles) && user.roles.includes("Admin") && (
                                    <div className="card-footer bg-transparent border-top-0 d-flex justify-content-around">
                                        <Link to={`/manga/edit/${item.id}`} className="btn btn-warning btn-sm">Edit</Link>
                                        <button onClick={() => handleDelete(item.id)} className="btn btn-danger btn-sm">Delete</button>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            ))}
        </div>

    )
}

Manga.propTypes = {
    filteredManga: PropTypes.arrayOf(PropTypes.object),
}

export default Manga