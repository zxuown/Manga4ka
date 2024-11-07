import { Link, useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useContext, useState } from 'react';
import ThemeContext from '../../context/ThemeContext';
import Swal from 'sweetalert2';
import AuthService from '../../services/AuthService';
import ImageService from '../../services/ImageService'

function Register() {
  const [formData, setFormData] = useState({
    login: '',
    name: '',
    email: '',
    image: '',
    password: '',
    confirmPassword: '',
  })

  const navigate = useNavigate()

  const { darkMode } = useContext(ThemeContext)

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }))
  }

  const handleImageChange = async (e) => {
    const file = e.target.files[0]
    if (file) {
      const base64Image = await ImageService.ConvertImageToBase64(file)
      setFormData((prev) => ({
        ...prev,
        image: base64Image,
      }))
    }
  }

  const handleSubmit = (e) => {
    e.preventDefault();
    if (formData.password === formData.confirmPassword) {
      console.log(formData);
      const fetchRegister = async () => {
        try {
          const status = await AuthService.Register(formData)
          if (status == 201) {
            Swal.fire({
              title: 'Success!',
              text: 'Registration was successful, now you can log in!',
              icon: 'success',
              confirmButtonText: 'Close'
            }).then(navigate("/login"))
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
      fetchRegister()
    } else {
      console.error("Passwords don't match");
    }
  };

  return (
    <div className={`auth-wrapper d-flex justify-content-center align-items-center vh-100 ${darkMode ? 'dark-mode' : ''}`}>
      <div className={`auth-inner p-5 shadow-lg ${darkMode ? 'bg-dark text-white' : ''}`}>
        <h2 className="text-center mb-4">Register</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group mb-3">
            <label className={darkMode ? 'dark-mode text-white' : ''}>Login <span className="badge text-bg-danger">* can't change</span></label>
            <input
              type="text"
              className={`form-control ${darkMode ? 'bg-dark text-white' : ''}`}
              placeholder="Enter login"
              name="login"
              value={formData.login}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group mb-3">
            <label className={darkMode ? 'dark-mode text-white' : ''}>Name</label>
            <input
              type="text"
              className={`form-control ${darkMode ? 'bg-dark text-white' : ''}`}
              placeholder="Enter name"
              name="name"
              value={formData.name}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group mb-3">
            <label className={darkMode ? 'dark-mode text-white' : ''}>Email</label>
            <input
              type="email"
              className={`form-control ${darkMode ? 'bg-dark text-white' : ''}`}
              placeholder="Enter email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group mb-3">
            <label className={darkMode ? 'dark-mode text-white' : ''}>Image</label>
            <input
              type="file"
              className={`form-control ${darkMode ? 'bg-dark text-white' : ''}`}
              name="image"
              onChange={handleImageChange}
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

          <div className="form-group mb-3">
            <label className={darkMode ? 'dark-mode text-white' : ''}>Confirm Password</label>
            <input
              type="password"
              className={`form-control ${darkMode ? 'bg-dark text-white' : ''}`}
              placeholder="Confirm password"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleInputChange}
              required
            />
          </div>

          <button type="submit" className={`btn btn-primary btn-block w-100 ${darkMode ? 'dark-mode' : ''}`}>
            Register
          </button>
          <p className="text-center mt-3">
            Already have an account? <Link to="/login">Sign In</Link>
          </p>
        </form>
      </div>
    </div>
  );
}

export default Register;
