import { useLocation } from "react-router-dom"
import Manga from "../manga/Manga"
import Authors from "../author/Authors"
import Genre from "../genre/Genre"

function MainSearchResult() {
    const location = useLocation()
    const { state } = location
    console.log("State:" + state)
    return (
        <>
            {state ?
                state.filteredManga.length > 0 ? (
                    <Manga filteredManga={state.filteredManga} />
                ) : (
                     state.filteredAuthors.length > 0 ? (
                        <Authors filteredAuthors={state.filteredAuthors} />
                    ) : (
                         state.filteredGenres.length > 0 ? (
                            <Genre filteredGenres={state.filteredGenres} />
                        ) : (
                            <Manga />
                        )
                    )
                )
                : <Manga />}

        </>
    )
}
export default MainSearchResult