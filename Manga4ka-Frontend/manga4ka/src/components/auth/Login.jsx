import { Link, useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useContext, useState } from 'react';
import ThemeContext from '../../context/ThemeContext';
import Swal from 'sweetalert2';
import AuthService from '../../services/AuthService';
import AuthContext from '../../context/AuthContext';

function Login() {
    const [formData, setFormData] = useState({ loginOrEmail: '', password: '' });

    const { darkMode } = useContext(ThemeContext)
    const { setUser } = useContext(AuthContext)

    const navigate = useNavigate()

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const fetchRequests = async () => {
            try {
                const status = await AuthService.Login(formData)
                const user = await AuthService.GetCurrentUser()
                setUser(user)
                if (status == 200) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Log in was successful, welcome!',
                        icon: 'success',
                        confirmButtonText: 'Close'
                    }).then(navigate("/"))
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
        fetchRequests()
    };

    return (
        <div className={`auth-wrapper d-flex justify-content-center align-items-center vh-100 ${darkMode ? 'dark-mode' : ''}`}>
            <div className={`auth-inner p-5 shadow-lg ${darkMode ? 'bg-dark text-white' : ''}`}>
                <h2 className="text-center mb-4">Login</h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-group mb-3">
                        <label className={darkMode ? 'dark-mode text-white' : ''}>Login or email</label>
                        <input
                            type="text"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : ''}`}
                            placeholder="Enter email or username"
                            name="loginOrEmail"
                            value={formData.email}
                            onChange={handleInputChange}
                            required
                        />
                    </div>

                    <div className="form-group mb-3">
                        <label className={darkMode ? 'dark-mode text-white' : ''}>Password</label>
                        <input
                            type="password"
                            className={`form-control ${darkMode ? 'bg-dark text-white' : ''}`}
                            placeholder="Enter password"
                            name="password"
                            value={formData.password}
                            onChange={handleInputChange}
                            required
                        />
                    </div>

                    <button type="submit" className={`btn btn-primary btn-block w-100 ${darkMode ? 'dark-mode' : ''}`}>
                        Login
                    </button>
                    <p className="forgot-password text-right mt-3">
                        Forgot <a href="#">password?</a>
                    </p>
                    <p className={`text-center mt-3 ${darkMode ? 'text-white' : ''}`}>
                        Don't have an account? <Link to="/register">Sign Up</Link>
                    </p>
                </form>
            </div>
        </div>
    );
}

export default Login;
