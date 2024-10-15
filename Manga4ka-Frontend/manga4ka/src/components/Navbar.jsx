import { useContext, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import MainSearch from "./search/MainSearch";
import ThemeContext from "../context/ThemeContext";
import MangaService from "../services/MangaService";
import GenreService from "../services/GenreService";
import AuthorService from "../services/AuthorService";
import AuthService from "../services/AuthService";
import AuthContext from "../context/AuthContext";
import Swal from "sweetalert2";

function Navbar() {
    const { darkMode, toggleDarkMode } = useContext(ThemeContext)
    const { user, setUser } = useContext(AuthContext)
    const navigate = useNavigate()

    const handleLogout = () => {
        try {
            AuthService.Logout()
            setUser({})
        } catch (e) {
            console.error('Logout error', e)
        }
    }
    const handleSearch = (query) => {
        const fetchSearchResults = async () => {
            try {
                const mangaData = await MangaService.SearchManga(query)
                const genresData = await GenreService.SearchGenres(query)
                const authorsData = await AuthorService.SearchAuthors(query)
                navigate('/search', {
                    state: {
                        filteredManga: mangaData,
                        filteredGenres: genresData,
                        filteredAuthors: authorsData
                    }
                });

            } catch (e) {
                console.error("Error searching", e)
            }
        };

        if (!query) {
            navigate("/")
        } else {
            fetchSearchResults()
        }
    }

    useEffect(() => {
        const fetchCurrentUser = async () => {
          try {
            const user = await AuthService.GetCurrentUser()
            setUser(user)
            console.log(user)
          } catch (e) {
            console.error("Current user error", e)
          }
        }
        if (localStorage.getItem("token")) {
          if (AuthService.isTokenExpired(localStorage.getItem("token"))) {
            Swal.fire({
              title: 'Session Expired',
              text: 'Your session has expired, please log in again.',
              icon: 'warning',
              confirmButtonText: 'Login'
            }).then(() => {
              navigate("/login")
            })
          } else {
            fetchCurrentUser()
          }
        }
      }, [navigate, setUser])

    return (
        <>
            <nav className={`navbar navbar-light bg-light navbar-expand-lg ${darkMode ? 'bg-dark' : 'bg-light'}`}>
                <div className="container-fluid">
                    <Link className="navbar-brand" to="/">
                        <img src="/Manga4kaMainLogo.png" alt="" width="50" height="50" className="d-inline-block align-text-top" />
                    </Link>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarSupportedContent">
                        <div className="d-flex justify-content-between w-100">

                            <ul className="navbar-nav">
                                <li className="nav-item">
                                    <Link className={`nav-link ${darkMode ? 'dark-mode' : ''}`} aria-current="page" to="/">Manga4ka</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className={`nav-link ${darkMode ? 'dark-mode' : ''}`} to="/genres">Genres</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className={`nav-link ${darkMode ? 'dark-mode' : ''}`} to="/authors">Authors</Link>
                                </li>
                            </ul>

                            <MainSearch onSearch={handleSearch}></MainSearch>

                            <ul className="navbar-nav">
                                <div className="nav-link form-check form-switch">
                                    <input className="form-check-input" type="checkbox"
                                        role="switch" id="flexSwitchCheckDefault"
                                        onChange={toggleDarkMode}
                                        checked={darkMode} />
                                    <label className="form-check-label" htmlFor="flexSwitchCheckDefault">
                                        <i className={`fas fa-${darkMode ? 'moon' : 'sun'} me-2`}></i>
                                    </label>
                                </div>
                                {user.name ? (
                                    <>
                                        <li className="nav-item">
                                            <span className={`nav-link ${darkMode ? 'dark-mode' : ''}`}>
                                                <i className="fas fa-user"></i> {user.name}
                                            </span>
                                        </li>
                                        <li className="nav-item">
                                            <button className={`btn nav-link ${darkMode ? 'dark-mode' : ''}`} onClick={handleLogout}>
                                                <i className="fas fa-sign-out-alt"></i> Logout
                                            </button>
                                        </li>
                                    </>
                                ) : (
                                    <>
                                        <li className="nav-item me-3">
                                            <Link className={`nav-link ${darkMode ? 'dark-mode' : ''}`} to='/register'>
                                                <i className="fas fa-user-plus"></i> Sign Up
                                            </Link>
                                        </li>
                                        <li className="nav-item">
                                            <Link className={`nav-link ${darkMode ? 'dark-mode' : ''}`} to='/login'>
                                                <i className="fas fa-sign-in-alt"></i> Login
                                            </Link>
                                        </li>
                                    </>
                                )}
                            </ul>
                        </div>
                    </div>
                </div>
            </nav>
        </>
    );
}

export default Navbar;
