import { useEffect, useState } from "react"
import Swal from 'sweetalert2';
import Rating from 'react-rating-stars-component'
import { useCallback, useContext } from "react";
import { Link, useParams } from "react-router-dom";
import ThemeContext from "../../context/ThemeContext";
import Favorite from "../favorite/favorite";
import MangaService from "../../services/MangaService";
import PdfService from "../../services/PdfService";
import PropTypes from 'prop-types';

function Manga({ filteredManga }) {
    let { genreId, authorId } = useParams();
    const [manga, setManga] = useState([]);
    const { darkMode } = useContext(ThemeContext);

    const handleDelete = useCallback((id) => {
        const fetchMangaDelete = async () => {
            try {
                const fileStatus = await PdfService.deletePdf(id)
                const mangaStatus = await MangaService.DeleteManga(id)
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
        if (filteredManga != undefined) {
            setManga(filteredManga)
        } else {
            const fetchAllManga = async () => {
                try {
                    const manga = await MangaService.GetAllManga()
                    if (genreId) {
                        const filteredManga = manga.filter(manga =>
                            manga.genres.includes(genreId)
                        )
                        setManga(filteredManga)
                    } else if (authorId) {
                        const filteredManga = manga.filter(manga =>
                            manga.authorid == authorId
                        )
                        setManga(filteredManga);
                    } else {
                        setManga(manga);
                    }
                    console.log(manga)
                } catch (e) {
                    console.error("Error loading all manga", e)
                }
            }

            fetchAllManga()
        }

    }, [filteredManga, genreId, authorId])

    return (
        <div className='row justify-content-center'>
            <Link to="/manga/create" className="card-link btn btn-primary">Create</Link>
            <div className="d-flex mt-3 justify-content-center">
                <div className="dropdown">
                    <button className={`btn dropdown-toggle ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        Sort by...
                    </button>
                    <ul className={`dropdown-menu ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                        <li><a className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} href="#">Posted</a></li>
                        <li><a className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} href="#">Rating</a></li>
                        <li><a className={`dropdown-item ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} href="#">Favorite</a></li>
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
                                    <Favorite></Favorite>
                                </div>
                                <p className="card-text">Author: {item.author.name}</p>
                                <p className="card-text">{item.description}<br />Genres:</p>
                                {item.genres.map((genre, idx) => (
                                    <span key={idx} className="badge rounded-pill text-bg-primary me-1">
                                        {genre.title}
                                    </span>
                                ))}
                                <Rating
                                    count={5}
                                    value={5}
                                    size={34}
                                    activeColor="#ffd700"
                                    className="ms-2"
                                    edit={false}
                                />
                                <p className={`card-text`}>
                                    <small className={`${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>Published: {new Date(item.datePublished).toLocaleDateString('en-GB')}</small>
                                </p>
                                <div className="card-footer">
                                    <Link to={`/manga/edit/${item.id}`} className="card-link btn btn-warning">Edit</Link>
                                    <a onClick={() => handleDelete(item.id)} className="card-link btn btn-danger">Delete</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>))}

        </div>
    )
}

Manga.propTypes = {
    filteredManga: PropTypes.arrayOf(PropTypes.object),
};

export default Manga