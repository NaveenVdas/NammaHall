import React, { useEffect, useState } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { hallApi, bookingApi, Hall } from "../services/api";

const BookingPage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const [searchParams] = useSearchParams();
  const [hall, setHall] = useState<Hall | null>(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [eventDate, setEventDate] = useState(searchParams.get('date') || "");
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [guests, setGuests] = useState("");
  const [notes, setNotes] = useState("");

  useEffect(() => {
    const fetchHall = async () => {
      if (!id) {
        setError('Invalid hall ID');
        setLoading(false);
        return;
      }

      try {
        setLoading(true);
        const hallId = parseInt(id);
        const data = await hallApi.getHallById(hallId);
        setHall(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load hall details');
      } finally {
        setLoading(false);
      }
    };

    fetchHall();
  }, [id]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!hall || !id) return;

    try {
      setSubmitting(true);
      setError(null);
      
      const response = await bookingApi.createBooking({
        hallId: hall.id,
        customerName: name,
        customerPhone: phone,
        eventDate: eventDate,
        guestCount: guests ? parseInt(guests) : undefined,
        notes: notes || undefined,
      });

      localStorage.setItem('lastBookingId', response.bookingId.toString());
      navigate(`/booking-confirmed?bookingId=${response.bookingId}`);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create booking');
    } finally {
      setSubmitting(false);
    }
  };

  const formatLocation = (hall: Hall) => {
    return `${hall.village}, ${hall.district}`;
  };

  const formatPrice = (hall: Hall) => {
    if (hall.priceFrom && hall.priceTo) {
      return `₹${hall.priceFrom.toLocaleString()} – ₹${hall.priceTo.toLocaleString()}`;
    } else if (hall.priceFrom) {
      return `From ₹${hall.priceFrom.toLocaleString()}`;
    }
    return "Price on request";
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-slate-50 flex items-center justify-center">
        <p className="text-slate-600">Loading...</p>
      </div>
    );
  }

  if (error && !hall) {
    return (
      <div className="min-h-screen bg-slate-50">
        <header className="bg-white border-b border-slate-200">
          <div className="max-w-3xl mx-auto px-4 py-3">
            <button 
              onClick={() => navigate(-1)}
              className="text-sm text-slate-600"
            >
              ← Back
            </button>
          </div>
        </header>
        <main className="max-w-3xl mx-auto px-4 py-8">
          <p className="text-red-500">{error}</p>
        </main>
      </div>
    );
  }

  if (!hall) return null;

  return (
    <div className="min-h-screen bg-slate-50">
      {/* Header */}
      <header className="bg-white border-b border-slate-200">
        <div className="max-w-3xl mx-auto px-4 py-3 flex items-center justify-between">
          <button 
            onClick={() => navigate(-1)}
            className="text-sm text-slate-600"
          >
            ← Back
          </button>
          <h1 className="font-semibold text-slate-800 text-sm">
            Book this hall
          </h1>
          <span className="text-xs text-slate-400" />
        </div>
      </header>

      <main className="max-w-3xl mx-auto px-4 pb-24">
        {/* Hall summary */}
        <section className="mt-4 bg-white rounded-2xl shadow-sm p-4">
          <h2 className="font-semibold text-slate-900 text-base">
            {hall.name}
          </h2>
          <p className="text-xs text-slate-500">
            {formatLocation(hall)}
          </p>
          <p className="mt-1 text-sm text-slate-700">
            {formatPrice(hall)}
          </p>
          <p className="mt-1 text-[11px] text-slate-500">
            We will confirm availability with the hall owner before final
            booking.
          </p>
        </section>

        {/* Booking form */}
        <section className="mt-4 bg-white rounded-2xl shadow-sm p-4">
          <h3 className="text-sm font-semibold text-slate-800 mb-3">
            Your details
          </h3>
          {error && (
            <div className="mb-3 p-2 bg-red-50 border border-red-200 rounded text-xs text-red-600">
              {error}
            </div>
          )}
          <form className="space-y-3" onSubmit={handleSubmit}>
            <div>
              <label className="block text-xs font-semibold text-slate-600 mb-1">
                Full name
              </label>
              <input
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}
                className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Name of person booking"
                required
              />
            </div>

            <div>
              <label className="block text-xs font-semibold text-slate-600 mb-1">
                Mobile number
              </label>
              <input
                type="tel"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="10-digit mobile number"
                required
              />
            </div>

            <div className="grid gap-3 sm:grid-cols-2">
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Event date
                </label>
                <input
                  type="date"
                  value={eventDate}
                  onChange={(e) => setEventDate(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Approx. guests
                </label>
                <input
                  type="number"
                  value={guests}
                  onChange={(e) => setGuests(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="e.g. 500"
                />
              </div>
            </div>

            <div>
              <label className="block text-xs font-semibold text-slate-600 mb-1">
                Notes (optional)
              </label>
              <textarea
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
                className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                rows={3}
                placeholder="Any special requirements?"
              />
            </div>

            <div className="pt-2 border-t border-slate-100 mt-3">
              <p className="text-[11px] text-slate-500 mb-2">
                By continuing, you agree that we will contact you and the
                hall owner to confirm availability and pricing.
              </p>
              <button
                type="submit"
                disabled={submitting}
                className="w-full rounded-full bg-blue-600 text-white text-sm font-semibold py-2.5 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {submitting ? 'Submitting...' : 'Request booking'}
              </button>
            </div>
          </form>
        </section>
      </main>
    </div>
  );
};

export default BookingPage;
