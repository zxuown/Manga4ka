import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import '@fortawesome/fontawesome-free/css/all.min.css';
import { BrowserRouter, Route, Routes } from "react-router-dom"
import { useEffect, useState } from "react";
import Manga from './components/manga/Manga';
import MangaCreateForm from './components/manga/MangaCraeteForm';
import MangaEditForm from './components/manga/MangaEditForm';
import ReadManga from './components/manga/ReadManga';
import Genre from './components/genre/Genre';
import Authors from './components/author/Authors';
import Author from './components/author/Author';
import Navbar from './components/Navbar';
import GenreCreateForm from './components/genre/GenreCreateForm';
import GenreEditForm from './components/genre/GenreEditForm';
import AuthorCreateForm from './components/author/AuthorCreateForm';
import AuthorEditForm from './components/author/AuthorEditForm';
import MainSearchResult from './components/search/MainSearchResult';
import Login from './components/auth/Login';
import Register from './components/auth/Register';
import ThemeContext from './context/ThemeContext';
import AuthContext from './context/AuthContext';

function App() {
  const [darkMode, setDarkMode] = useState(() => {
    return localStorage.getItem("darkMode") === "true" ? true : false
  })

  const [user, setUser] = useState({})

  const toggleDarkMode = () => {
    setDarkMode(prevMode => {
      localStorage.setItem("darkMode", !prevMode)
      return !prevMode
    })
  }


  useEffect(() => {
    darkMode ?
      document.body.classList.add("dark-mode") :
      document.body.classList.remove("dark-mode")
  }, [darkMode])

  return (
    <ThemeContext.Provider value={{ darkMode, toggleDarkMode }}>
      <AuthContext.Provider value={{ user, setUser }}>
        <BrowserRouter>
          <Navbar></Navbar>
          <Routes>

            <Route path='/' element={<Manga></Manga>}></Route>
            <Route path='/search' element={<MainSearchResult></MainSearchResult>}></Route>

            <Route path='/login' element={<Login></Login>}></Route>
            <Route path='/register' element={<Register></Register>}></Route>

            <Route path='/manga/create' element={<MangaCreateForm></MangaCreateForm>}></Route>
            <Route path='/manga/edit/:mangaId' element={<MangaEditForm></MangaEditForm>}></Route>
            <Route path='/:mangaId' element={<ReadManga />}></Route>

            <Route path='/genres' element={<Genre></Genre>}></Route>
            <Route path='/genres/:genreId' element={<Manga></Manga>}></Route>
            <Route path='/genres/create' element={<GenreCreateForm></GenreCreateForm>}></Route>
            <Route path='/genres/edit/:genreId' element={<GenreEditForm></GenreEditForm>}></Route>

            <Route path='/authors' element={<Authors></Authors>}></Route>
            <Route path='/authors/:authorId' element={<Author></Author>}></Route>
            <Route path='/authors/create' element={<AuthorCreateForm></AuthorCreateForm>}></Route>
            <Route path='/authors/edit/:authorId' element={<AuthorEditForm></AuthorEditForm>}></Route>

          </Routes>
        </BrowserRouter>
      </AuthContext.Provider>
    </ThemeContext.Provider>

  );
}

export default App;
