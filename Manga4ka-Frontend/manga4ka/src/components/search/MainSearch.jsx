import { useContext, useEffect, useState } from "react"
import ThemeContext from "../../context/ThemeContext"

function MainSearch({ onSearch }) {
    const { darkMode } = useContext(ThemeContext)
    const [search, setSearch] = useState("")

    const handleChange = (e) => {
        setSearch(e.target.value.toLowerCase())
    }

    useEffect(() => {
        const debounce = setTimeout(() => {
            onSearch(search)
            console.log(search)
        }, 1000)

        return () => clearTimeout(debounce)
    }, [search])

    return (
        <>
            <div className="d-flex mx-auto" style={{ maxWidth: "300px", width: "100%" }}>
                <input onChange={handleChange} className={`form-control me-2 ${darkMode ? 'dark-mode' : ''}`} type="search" placeholder="Search..." aria-label="Search" />
            </div>
        </>
    )
}

export default MainSearch