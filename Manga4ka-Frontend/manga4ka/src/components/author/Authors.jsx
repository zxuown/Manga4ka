import Swal from 'sweetalert2'
import '@sweetalert2/theme-dark/dark.css'
import { Link } from "react-router-dom"
import { useContext, useEffect, useState } from "react";
import ThemeContext from '../../context/ThemeContext';
import Rating from 'react-rating-stars-component'
import AuthorService from '../../services/AuthorService';
import PropTypes from 'prop-types';
import AuthContext from '../../context/AuthContext';

function Authors({ filteredAuthors }) {
    const [authors, setAuthors] = useState([])
    const { darkMode } = useContext(ThemeContext)
    const { user } = useContext(AuthContext)

    const handleDelete = (id) => {
        const fetchDeleteAuthor = async () => {
            try {
                const status = await AuthorService.DeleteAuthor(id, localStorage.getItem('token'))
                setAuthors(authors => authors.filter(author => author.id != id))
                console.log(status)
                if (status === 200) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'You deleted author!',
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
        fetchDeleteAuthor()
    }

    useEffect(() => {
        if (filteredAuthors != undefined) {
            setAuthors(filteredAuthors)
        } else {
            const fetchAuthors = async () => {
                try {
                    const authors = await AuthorService.GetAuthors()
                    setAuthors(authors)
                } catch (e) {
                    console.error("Error loading authors", e)
                }
            }
            fetchAuthors()
        }

    }, [filteredAuthors])

    return (
        <>
            <div className='row justify-content-center'>
                {
                    user !== undefined && Array.isArray(user.roles) && user.roles.some(x => x == "Admin") ?
                        <Link to={`/authors/create`} className="card-link btn btn-primary">Create</Link>
                        : ""
                }
                {authors.map((author, index) => (
                    <div key={index} className={`card mb-3 m-3 col-md-3 ${darkMode ? 'bg-dark text-white' : 'bg-light'}`} style={{ maxWidth: 400 }}>
                        <div className="row g-0">
                            <div className="col-md-4">
                                <Link to={`/authors/${author.id}`}>
                                    <img src={author.image} className="img-fluid rounded-start" style={{ height: 150 }} alt="..." />
                                </Link>
                            </div>
                            <div className="col-md-8">
                                <div className="card-body">
                                    <h5 className="card-title">{author.name} {author.lastname}</h5>
                                    <p className="card-text">{author.description}</p>
                                    <Rating
                                        count={5}
                                        value={5}
                                        size={34}
                                        activeColor="#ffd700"
                                        className="ms-2"
                                        edit={false}
                                    />
                                </div>
                                {
                                     Array.isArray(user.roles) && user.roles.some(x => x === "Admin") ?
                                        <div className="card-footer">
                                            <Link to={`/authors/edit/${author.id}`} className="card-link btn btn-warning">Edit</Link>
                                            <a onClick={() => handleDelete(author.id)} className="card-link btn btn-danger">Delete</a>
                                        </div> : ""
                                }
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </>
    )
}

Authors.propTypes = {
    filteredAuthors: PropTypes.arrayOf(PropTypes.object)
}

export default Authors