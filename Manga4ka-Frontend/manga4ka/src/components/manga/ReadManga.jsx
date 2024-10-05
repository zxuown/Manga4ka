import { useContext, useEffect, useState } from 'react';
// import { Worker, Viewer } from '@react-pdf-viewer/core';
// import '@react-pdf-viewer/core/lib/styles/index.css';
// import '@react-pdf-viewer/default-layout/lib/styles/index.css';
// import '@react-pdf-viewer/toolbar/lib/styles/index.css';
import Swal from 'sweetalert2';
import { Document, Page, pdfjs } from 'react-pdf';
import { useParams } from 'react-router-dom';
import Rating from 'react-rating-stars-component'
import ThemeContext from '../../context/ThemeContext';
import 'react-pdf/dist/esm/Page/AnnotationLayer.css';
import { useSwipeable } from 'react-swipeable';
import MangaService from '../../services/MangaService';

pdfjs.GlobalWorkerOptions.workerSrc = new URL(
    'pdfjs-dist/build/pdf.worker.min.mjs',
    import.meta.url,
).toString();

function ReadManga() {
    const { mangaId } = useParams()
    const { darkMode } = useContext(ThemeContext)
    const [comment, setComment] = useState('')
    const [comments, setComments] = useState([])
    const [rating, setRating] = useState(0)
    const [numPages, setNumPages] = useState(null);
    const [pageNumber, setPageNumber] = useState(1);
    const [windowWidth, setWindowWidth] = useState(window.innerWidth)

    const onDocumentLoadSuccess = ({ numPages }) => {
        setNumPages(numPages);
    };

    const goToPrevPage = () =>
        setPageNumber(pageNumber - 1 <= 1 ? 1 : pageNumber - 1);

    const goToNextPage = () =>
        setPageNumber(
            pageNumber + 1 >= numPages ? numPages : pageNumber + 1,
        );

    const handleAddComment = () => {
        if (comment.trim() !== '') {
            setComments((prevComments) => [...prevComments,
            comment + ' ' + new Date().toLocaleTimeString()])
            setComment('');
        }
    }

    const handleRatingChange = (newRating) => {
        setRating(newRating)
        Swal.fire({
            title: 'Success!',
            text: 'Thank you for your oppinion!',
            icon: 'success',
            confirmButtonText: 'Close'
        })
    }

    const handlers = useSwipeable({
        onSwipedLeft: () => goToNextPage(),
        onSwipedRight: () => goToPrevPage(),
        preventDefaultTouchmoveEvent: true,
        delta: 150,
        trackMouse: true,
    });

    const pdfWidth = windowWidth < 768 ? windowWidth * 0.9 : 600

    return (
        <div className="mt-3 d-flex justify-content-center">
            <div {...handlers} style={{ width: `${pdfWidth}px`, margin: 'auto' }}>
                <div className={`${rating > 0 ? 'd-none' : ''} d-flex justify-content-center align-items-center`}>
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
                    <span className="fs-3 badge rounded-pill text-bg-primary">
                        Comments:
                    </span>
                </div>

                <div className={`mt-3 border overflow-auto ${darkMode ? 'bg-dark text-white' : ''}`}
                    style={{ height: '400px', width: `${pdfWidth}px` }}>
                    <div>
                        {comments.map((comment, index) => (
                            <div key={index} className="card m-4">
                                <div className={`card-body ${darkMode ? 'bg-dark text-white' : 'bg-light'}`}>
                                    <div className="d-flex justify-content-between">
                                        <div className="d-flex flex-row align-items-center">
                                            <img
                                                src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/img%20(4).webp"
                                                alt="avatar"
                                                width="25"
                                                height="25"
                                            />
                                            <p className="small mb-0 ms-2">User</p>
                                        </div>
                                    </div>
                                    <p>{comment}</p>
                                </div>
                            </div>
                        ))}

                    </div>
                </div>
                <div className="d-flex m-2">
                    <input
                        type="text"
                        className="form-control ms-3"
                        value={comment}
                        onChange={(e) => setComment(e.target.value).toString()}
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
    );
}

export default ReadManga;
