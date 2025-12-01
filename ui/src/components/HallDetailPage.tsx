import React, { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import { hallApi, Hall } from "../services/api";

const HallDetailPage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const [hall, setHall] = useState<Hall | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDate, setSelectedDate] = useState("");

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

  const formatLocation = (hall: Hall) => {
    return `${hall.village}, ${hall.district}`;
  };

  const formatPriceRange = (hall: Hall) => {
    if (hall.priceFrom && hall.priceTo) {
      return `₹${hall.priceFrom.toLocaleString()} – ₹${hall.priceTo.toLocaleString()} per day`;
    } else if (hall.priceFrom) {
      return `From ₹${hall.priceFrom.toLocaleString()} per day`;
    }
    return "Price on request";
  };

  const formatCapacity = (hall: Hall) => {
    return `Up to ${hall.capacity} guests`;
  };

  const getFacilities = (hall: Hall): string[] => {
    const facilities: string[] = [];
    if (hall.isAc) facilities.push('AC Hall');
    if (hall.hasDiningHall) facilities.push('Dining Hall');
    if (hall.hasParking) facilities.push('Parking');
    if (hall.hasRooms) facilities.push('Rooms');
    return facilities;
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-slate-50 flex items-center justify-center">
        <p className="text-slate-600">Loading hall details...</p>
      </div>
    );
  }

  if (error || !hall) {
    return (
      <div className="min-h-screen bg-slate-50">
        <header className="bg-white border-b border-slate-200">
          <div className="max-w-4xl mx-auto px-4 py-3">
            <button 
              onClick={() => navigate(-1)}
              className="text-sm text-slate-600"
            >
              ← Back
            </button>
          </div>
        </header>
        <main className="max-w-4xl mx-auto px-4 py-8">
          <p className="text-red-500">{error || 'Hall not found'}</p>
        </main>
      </div>
    );
  }

  const whatsappLink = `https://wa.me/${hall.ownerPhone}?text=${encodeURIComponent(
    `Namaskara, I am interested in booking ${hall.name} for a function.`
  )}`;

  return (
    <div className="min-h-screen bg-slate-50">
      {/* Top bar */}
      <header className="bg-white border-b border-slate-200">
        <div className="max-w-4xl mx-auto px-4 py-3 flex items-center justify-between">
          <button 
            onClick={() => navigate(-1)}
            className="text-sm text-slate-600"
          >
            ← Back
          </button>
          <h1 className="font-semibold text-slate-800 text-sm">
            Hall details
          </h1>
          <Link 
            to="/admin/login" 
            className="text-xs text-slate-500 hover:text-slate-700"
          >
            Admin
          </Link>
        </div>
      </header>

      <main className="max-w-4xl mx-auto px-4 pb-24">
        {/* Image gallery */}
        <section className="mt-3">
          <div className="grid grid-cols-3 gap-2 rounded-2xl overflow-hidden">
            {hall.images.length > 0 ? (
              <>
                <img
                  src={hall.images[0]}
                  alt={hall.name}
                  className="col-span-2 h-48 sm:h-64 w-full object-cover"
                />
                <div className="flex flex-col gap-2">
                  {hall.images.slice(1).map((img, idx) => (
                    <img
                      key={idx}
                      src={img}
                      alt={hall.name}
                      className="h-24 sm:h-32 w-full object-cover"
                    />
                  ))}
                </div>
              </>
            ) : (
              <div className="col-span-3 h-48 sm:h-64 bg-slate-200 flex items-center justify-center">
                <p className="text-slate-400">No images available</p>
              </div>
            )}
          </div>
        </section>

        {/* Header info */}
        <section className="mt-4">
          <div className="flex items-start justify-between gap-2">
            <div>
              <h2 className="font-semibold text-slate-900 text-lg">
                {hall.name}
              </h2>
              <p className="text-sm text-slate-500 mt-0.5">
                {formatLocation(hall)}
              </p>
              <p className="text-sm text-slate-600 mt-1">
                <span className="font-semibold">{formatCapacity(hall)}</span>
              </p>
            </div>
            <div className="text-right">
              <p className="text-xs text-slate-500">Price range</p>
              <p className="font-semibold text-slate-900 text-sm">
                {formatPriceRange(hall)}
              </p>
            </div>
          </div>
        </section>

        {/* Facilities */}
        <section className="mt-3">
          <h3 className="text-sm font-semibold text-slate-800 mb-2">
            Facilities
          </h3>
          <div className="flex flex-wrap gap-2">
            {getFacilities(hall).map((facility) => (
              <span
                key={facility}
                className="px-3 py-1 rounded-full bg-indigo-50 text-[11px] text-slate-700"
              >
                {facility}
              </span>
            ))}
          </div>
        </section>

        {/* Availability + booking hint */}
        <section className="mt-4">
          <h3 className="text-sm font-semibold text-slate-800 mb-2">
            Check availability
          </h3>
          <div className="bg-white rounded-2xl shadow-sm p-3">
            <p className="text-xs text-slate-600 mb-2">
              Select your date and contact us. We will confirm with the hall
              owner and send you the status on WhatsApp or call.
            </p>
            <div className="flex flex-col sm:flex-row gap-2">
              <input
                type="date"
                value={selectedDate}
                onChange={(e) => setSelectedDate(e.target.value)}
                className="flex-1 rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <button 
                onClick={() => {
                  if (selectedDate) {
                    navigate(`/book/${hall.id}?date=${selectedDate}`);
                  } else {
                    navigate(`/book/${hall.id}`);
                  }
                }}
                className="sm:w-40 rounded-full bg-blue-600 text-white text-sm font-semibold py-2"
              >
                Check with owner
              </button>
            </div>
          </div>
        </section>

        {/* Map link */}
        {hall.googleMapsUrl && (
          <section className="mt-4">
            <h3 className="text-sm font-semibold text-slate-800 mb-1">
              Location
            </h3>
            <a
              href={hall.googleMapsUrl}
              target="_blank"
              rel="noreferrer"
              className="inline-flex items-center gap-1 text-xs text-blue-600 underline"
            >
              📍 View on Google Maps
            </a>
          </section>
        )}
      </main>

      {/* Mobile sticky contact bar */}
      <div className="fixed bottom-0 left-0 right-0 bg-white shadow-[0_-4px_12px_rgba(15,23,42,0.12)] px-4 py-2 flex gap-2 md:hidden">
        <a
          href={`tel:+${hall.ownerPhone}`}
          className="flex-1 rounded-full border border-slate-300 text-slate-700 text-sm py-2 text-center"
        >
          📞 Call
        </a>
        <a
          href={whatsappLink}
          className="flex-1 rounded-full bg-green-500 text-white text-sm py-2 text-center font-semibold"
        >
          💬 WhatsApp
        </a>
      </div>
    </div>
  );
};

export default HallDetailPage;
