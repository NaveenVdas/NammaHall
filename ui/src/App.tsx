import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import HomePage from './components/HomePage'
import HallListPage from './components/HallListPage'
import HallDetailPage from './components/HallDetailPage'
import BookingPage from './components/BookingPage'
import BookingConfirmationPage from './components/BookingConfirmationPage'
import AdminLoginPage from './components/AdminLoginPage'
import AdminHallListPage from './components/AdminHallListPage'
import AdminHallFormPage from './components/AdminHallFormPage'

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/halls" element={<HallListPage />} />
        <Route path="/hall/:id" element={<HallDetailPage />} />
        <Route path="/book/:id" element={<BookingPage />} />
        <Route path="/booking-confirmed" element={<BookingConfirmationPage />} />
        <Route path="/admin/login" element={<AdminLoginPage />} />
        <Route path="/admin/halls" element={<AdminHallListPage />} />
        <Route path="/admin/halls/new" element={<AdminHallFormPage />} />
        <Route path="/admin/halls/edit/:id" element={<AdminHallFormPage />} />
      </Routes>
    </Router>
  )
}

export default App

