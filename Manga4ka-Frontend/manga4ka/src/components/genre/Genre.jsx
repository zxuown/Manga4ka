import { useEffect, useContext, useState, useCallback } from "react"
import Swal from 'sweetalert2';
import { Link } from "react-router-dom"
import ThemeContext from "../../context/ThemeContext";
import GenreService from '../../services/GenreService';
import PropTypes from 'prop-types';
import AuthContext from "../../context/AuthContext";

function Genre({ filteredGenres }) {
    const [genres, setGenres] = useState([])
    const { darkMode } = useContext(ThemeContext)
    const { user } = useContext(AuthContext)

    const handleDelete = useCallback((id) => {
        const fetchGenreDelete = async () => {
            try {
                const status = await GenreService.DeleteGenre(id, localStorage.getItem('token'))
                setGenres(genres => genres.filter(genre => genre.id != id))
                if (status === 200) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'You deleted genre use!',
                        icon: 'success',
                        confirmButtonText: 'Close'
                    })
                }
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
        fetchGenreDelete()
    }, [])

    useEffect(() => {
        if (filteredGenres != undefined) {
            setGenres(filteredGenres)
        } else {
            const fetchGenres = async () => {
                try {
                    const genres = await GenreService.GetGenres()
                    setGenres(genres)
                } catch (e) {
                    console.error("Error loading genres", e)
                }
            }
            fetchGenres()
        }
    }, [filteredGenres])

    return (
        <>
            <div className="row justify-content-center">
                {
                    user !== undefined && Array.isArray(user.roles) && user.roles.some(x => x == "Admin") ? <Link to={`/genres/create`} className="card-link btn btn-primary">Create</Link>
                        : ""
                }
                {genres.map((genre, index) => (
                    <div key={index} className={`card mb-3 m-3 col-md-2  ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                        <Link to={`/genres/${genre.id}`}>
                            <img className="card-img-top" style={{ height: 150 }} src={genre.image} alt="Card image cap" />
                        </Link>
                        <div className={`card-body ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                            <h5 className={`card-title ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>{genre.title}</h5>
                            <p className="card-text">{genre.description}</p>
                        </div>
                        {
                            Array.isArray(user.roles) && user.roles.some(x => x === "Admin") ?
                                <div className="card-footer">
                                    <Link to={`/genres/edit/${genre.id}`} className="card-link btn btn-warning">Edit</Link>
                                    <a onClick={() => handleDelete(genre.id)} className="card-link btn btn-danger">Delete</a>
                                </div> : ""
                        }
                    </div>
                ))}
            </div>
        </>
    )
}

Genre.propTypes = {
    filteredGenres: PropTypes.arrayOf(PropTypes.object),
}

export default Genre