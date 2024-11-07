import { useContext, useEffect, useState } from 'react';
import Swal from 'sweetalert2';
import { Document, Page, pdfjs } from 'react-pdf';
import { useParams } from 'react-router-dom';
import Rating from 'react-rating-stars-component'
import ThemeContext from '../../context/ThemeContext';
import AuthContext from '../../context/AuthContext';
import 'react-pdf/dist/esm/Page/AnnotationLayer.css';
import 'react-pdf/dist/esm/Page/TextLayer.css';
import { useSwipeable } from 'react-swipeable';
import CommentService from '../../services/CommentService';
import MangaService from '../../services/MangaService';
import RatingService from '../../services/RatingService';

pdfjs.GlobalWorkerOptions.workerSrc = new URL(
    'pdfjs-dist/build/pdf.worker.min.mjs',
    import.meta.url,
).toString();

function ReadManga() {
    const { mangaId } = useParams()
    const { darkMode } = useContext(ThemeContext)
    const { user } = useContext(AuthContext)
    const [text, setText] = useState('')
    const [comments, setComments] = useState([])
    const [manga, setManga] = useState({})
    const [hideComments, setShowComments] = useState(localStorage.getItem("hideComments") == 'true' ? true : false)
    const [rating, setRating] = useState(0)
    const [isUserRated, setIsUserRated] = useState(false)
    const [numPages, setNumPages] = useState(null);
    const [pageNumber, setPageNumber] = useState(1);
    const [windowWidth, setWindowWidth] = useState(window.innerWidth)

    const onDocumentLoadSuccess = ({ numPages }) => {
        setNumPages(numPages);
    }

    const goToPrevPage = () =>
        setPageNumber(pageNumber - 1 <= 1 ? 1 : pageNumber - 1)

    const goToNextPage = () =>
        setPageNumber(
            pageNumber + 1 >= numPages ? numPages : pageNumber + 1,
        )

    const fetchAllComments = async () => {
        try {
            const comments = await CommentService.GetAllCommentsByMangaId(mangaId)
            setComments(comments)
        } catch (e) {
            console.error("Error loading all comments by manga id", e)
        }
    }

    const handleAddComment = () => {
        if (text.trim() !== '') {
            const fetchAddComment = async () => {
                try {
                    const commentDto = {
                        id: 0,
                        user: user,
                        manga: manga,
                        text: text,
                        datePublished: new Date()
                    }
                    const status = await CommentService.CreateComment(commentDto, localStorage.getItem('token'))
                    if (status == 200) {
                        fetchAllComments()
                        setText('')
                    }
                } catch (e) {
                    console.error("Error adding comment", e)
                }
            }
            fetchAddComment()
        }
    }

    const handleDeleteComment = (id) => {
        const fetchDeleteComment = async () => {
            try {
                console.log(id)
                const status = await CommentService.DeleteComment(id, localStorage.getItem('token'))
                if (status == 200) {
                    setComments(comments.filter(x => x.id != id))
                    Swal.fire({
                        title: 'Success!',
                        text: 'You deleted comment!',
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
        fetchDeleteComment()
    }

    const handleHideComments = () => {
        setShowComments(!hideComments)
        localStorage.setItem("hideComments", !hideComments)
    }

    const handleRatingChange = (newRating) => {
        const fetchAddRating = async () => {
            try {
                const status = await RatingService.AddRating({
                    id: 0,
                    user: user,
                    manga: manga,
                    value: newRating
                }, localStorage.getItem('token'))
                if (status == 200) {
                    setIsUserRated(true)
                    Swal.fire({
                        title: 'Success!',
                        text: 'Thank you for your oppinion!',
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

        fetchAddRating()
    }

    const handlers = useSwipeable({
        onSwipedLeft: () => goToNextPage(),
        onSwipedRight: () => goToPrevPage(),
        preventDefaultTouchmoveEvent: true,
        delta: 150,
        trackMouse: true,
    })

    useEffect(() => {
        const fetchManga = async () => {
            try {
                const manga = await MangaService.GetManga(mangaId)
                setManga(manga)
            } catch (e) {
                console.error("Error loading all comments by manga id", e)
            }
        }

        const fetchIsUserRated = async () => {
            try {
                const data = await RatingService.IsUserRated(mangaId, user.id, localStorage.getItem('token'))
                setIsUserRated(data)
            } catch (e) {
                console.error("Error getting user rated", e)
            }
        }

        fetchAllComments()
        fetchManga()
        fetchIsUserRated()
    }, [mangaId])

    const pdfWidth = windowWidth < 768 ? windowWidth * 0.9 : 600

    return (
        <div className="mt-3 d-flex justify-content-center">
            <div {...handlers} style={{ width: `${pdfWidth}px`, margin: 'auto' }}>
                <div className={`${isUserRated ? 'd-none' : ''} d-flex justify-content-center align-items-center`}>
                    <span className="me-2">Rate the manga:</span>
                    <Rating
                        count={5}
                        value={0}
                        size={34}
                        activeColor="#ffd700"
                        onChange={handleRatingChange}
                    />
                </div>
                <div className="d-flex justify-content-between mb-3">
                    <button className='btn btn-primary' onClick={goToPrevPage} disabled={pageNumber === 1}>
                        Prev
                    </button>
                    <p>
                        Page {pageNumber} of {numPages}
                    </p>
                    <button className='btn btn-primary' onClick={goToNextPage} disabled={pageNumber === numPages}>
                        Next
                    </button>
                </div>
                <div style={{ height: '50%' }}>
                    <Document
                        file={`https://localhost:7242/pdffile/file?id=${mangaId}`}
                        onLoadSuccess={onDocumentLoadSuccess}
                    >
                        <Page
                            pageNumber={pageNumber}
                            width={pdfWidth} // Set width to adjust to screen size
                        />
                    </Document>
                </div>
                <div className='mt-3 d-flex justify-content-center'>
                    <span className={`${hideComments ? 'd-none' : 'fs-3 badge rounded-pill text-bg-primary'}`}>
                        Comments:
                    </span>
                    <div className="mt-2 ms-4 form-check form-switch">
                        <input className="form-check-input" checked={hideComments} onChange={handleHideComments} type="checkbox" role="switch" id="flexSwitchCheckDefault" />
                        <label className={`${darkMode ? 'text-white' : ''}`}>Hide comments</label>
                    </div>
                </div>

                <div className={`${hideComments ? 'd-none' : 'mt-3 border overflow-auto'} ${darkMode ? 'bg-dark text-white' : ''}`}
                    style={{ height: '400px', width: `${pdfWidth}px` }}>
                    <div>
                        {comments.map((comment, index) => (
                            <div key={index} className="card m-4">
                                <div className={`card-body ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                                    <div className="d-flex justify-content-between">
                                        <div className="d-flex flex-row align-items-center">
                                            {comment.user.avatarUrl ? <img
                                                src={comment.user.avatarUrl}
                                                alt="avatar"
                                                width="25"
                                                height="25"
                                            /> : <img
                                                src="https://as2.ftcdn.net/v2/jpg/05/18/52/05/1000_F_518520569_XpICYoat3e0OdAw9SiSx3X9H4O88ix2X.jpg"
                                                alt="avatar"
                                                width="25"
                                                height="25"
                                            />}
                                            <p className="small mb-0 ms-2">{comment.user.name}</p>
                                        </div>
                                    </div>
                                    <p>{comment.text}<br />Published: {new Date(comment.datePublished).toLocaleString('de-DE')}</p>
                                    {user.id == comment.user.id ?
                                        <button
                                            className="btn btn-danger ms-2"
                                            onClick={() => handleDeleteComment(comment.id)}>
                                            Delete
                                        </button> : ""}
                                </div>
                            </div>
                        ))}

                    </div>
                </div>
                <div className={`${hideComments ? 'd-none' : 'd-flex m-2'}`}>
                    <input
                        type="text"
                        className="form-control ms-3"
                        value={text}
                        onChange={(e) => setText(e.target.value)}
                        placeholder="Comment..."
                    />
                    <button
                        className="btn btn-primary ms-2"
                        onClick={handleAddComment}>
                        Submit
                    </button>
                </div>
            </div>
        </div>
    )
}

export default ReadManga;
