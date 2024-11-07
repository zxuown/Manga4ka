import { useContext, useEffect, useRef, useState } from "react"
import AuthContext from "../../context/AuthContext"
import MangaService from "../../services/MangaService"
import PropTypes from 'prop-types';

function Favorite({ manga, isFavorite }) {
    const [isFavorited, setIsFavorited] = useState(isFavorite)
    const { user } = useContext(AuthContext)

    const hasFavoritedOnce = useRef(false)
    
    useEffect(() => {
        setIsFavorited(isFavorite)
    }, [isFavorite])

    const handleFavoriteClick = () => {
        setIsFavorited((prevState) => !prevState)
        hasFavoritedOnce.current = true
    }

    useEffect(() => {
        
        const updateFavoriteStatus = async () => {
            try {
                if (isFavorited && hasFavoritedOnce.current) {
                    await MangaService.AddFavoriteManga({
                        id: 0,
                        manga: manga,
                        user: user,
                        isFavorite: true,
                    })
                } else if (!isFavorited && hasFavoritedOnce.current) {
                    await MangaService.DeleteFavoriteManga(manga.id, user.id)
                }
            } catch (e) {
                console.error(
                    `Error ${isFavorited ? "adding" : "removing"} favorite manga`,
                    e
                )
            }
        }

        if (hasFavoritedOnce.current) {
            updateFavoriteStatus()
        }
    }, [isFavorited, manga, user])

    return (
        <>
            <i
                className={`fa${isFavorited ? 's' : 'r'} fa-heart`}
                onClick={handleFavoriteClick}
                style={{ cursor: 'pointer', color: isFavorited ? 'red' : 'grey', fontSize: '24px' }}
            />
        </>
    )
}

Favorite.propTypes = {
    manga: PropTypes.object.isRequired,
    isFavorite: PropTypes.bool.isRequired,
}

export default Favorite