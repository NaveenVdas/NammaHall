import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { bookingApi, Booking } from "../services/api";

const BookingConfirmationPage: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [booking, setBooking] = useState<Booking | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const bookingId = searchParams.get('bookingId');
    if (bookingId) {
      const fetchBooking = async () => {
        try {
          setLoading(true);
          const data = await bookingApi.getBookingById(parseInt(bookingId));
          setBooking(data);
        } catch (err) {
          setError(err instanceof Error ? err.message : 'Failed to load booking');
        } finally {
          setLoading(false);
        }
      };
      fetchBooking();
    } else {
      // If no bookingId, try to get from localStorage (set during booking creation)
      const bookingIdFromStorage = localStorage.getItem('lastBookingId');
      if (bookingIdFromStorage) {
        const fetchBooking = async () => {
          try {
            setLoading(true);
            const data = await bookingApi.getBookingById(parseInt(bookingIdFromStorage));
            setBooking(data);
          } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to load booking');
          } finally {
            setLoading(false);
          }
        };
        fetchBooking();
      } else {
        setLoading(false);
        setError('No booking ID found');
      }
    }
  }, [searchParams]);

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-IN', { year: 'numeric', month: 'long', day: 'numeric' });
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-slate-50 flex items-center justify-center">
        <p className="text-slate-600">Loading booking details...</p>
      </div>
    );
  }

  if (error || !booking) {
    return (
      <div className="min-h-screen bg-slate-50 flex items-center">
        <main className="max-w-md mx-auto px-4 py-10">
          <div className="bg-white rounded-2xl shadow-md p-5 text-center">
            <p className="text-red-500">{error || 'Booking not found'}</p>
            <button 
              onClick={() => navigate('/')}
              className="mt-4 block w-full rounded-full border border-slate-300 text-slate-700 text-sm py-2"
            >
              Back to home
            </button>
          </div>
        </main>
      </div>
    );
  }

  const whatsappSupportLink = `https://wa.me/919999999999?text=${encodeURIComponent(
    `Hi, I have a question about my hall booking ${booking.id}.`
  )}`;

  return (
    <div className="min-h-screen bg-slate-50 flex items-center">
      <main className="max-w-md mx-auto px-4 py-10">
        <div className="bg-white rounded-2xl shadow-md p-5 text-center">
          <div className="mx-auto mb-3 flex h-12 w-12 items-center justify-center rounded-full bg-green-100">
            <span className="text-green-600 text-2xl">✔</span>
          </div>
          <h1 className="text-lg font-semibold text-slate-900">
            Booking request received
          </h1>
          <p className="mt-1 text-sm text-slate-600">
            We will call you and the hall owner shortly to confirm
            availability and final price.
          </p>

          <div className="mt-4 bg-slate-50 rounded-xl p-3 text-left text-xs text-slate-700">
            <p className="font-semibold text-slate-800">
              Booking ID: {booking.id}
            </p>
            <p className="mt-1">{booking.hallName}</p>
            <p className="mt-1">
              Event date:{" "}
              <span className="font-semibold">
                {formatDate(booking.eventDate)}
              </span>
            </p>
            <p className="mt-1">
              Name: {booking.customerName} ({booking.customerPhone})
            </p>
            {booking.guestCount && (
              <p className="mt-1">
                Guests: {booking.guestCount}
              </p>
            )}
          </div>

          <div className="mt-4 space-y-2">
            <a
              href={whatsappSupportLink}
              className="block w-full rounded-full bg-green-500 text-white text-sm font-semibold py-2"
            >
              💬 Ask on WhatsApp
            </a>
            <button 
              onClick={() => navigate('/')}
              className="block w-full rounded-full border border-slate-300 text-slate-700 text-sm py-2"
            >
              Back to home
            </button>
          </div>

          <p className="mt-3 text-[11px] text-slate-400">
            Keep your booking ID safe. Our team may ask for it while
            assisting you.
          </p>
        </div>
      </main>
    </div>
  );
};

export default BookingConfirmationPage;
