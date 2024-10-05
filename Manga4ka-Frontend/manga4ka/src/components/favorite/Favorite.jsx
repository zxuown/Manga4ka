import { useState } from "react"

function Favorite() {
    const [isFavorited, setIsFavorited] = useState(false)

    const handleFavoriteClick = () => {
        setIsFavorited(!isFavorited)
    }

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

export default Favorite