import React, { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { hallApi, Hall } from "../services/api";

type HallCardProps = {
  id: number;
  name: string;
  location: string;
  price: string;
  capacity: string;
  verified?: boolean;
  imageUrl: string;
};

const HallCard: React.FC<HallCardProps> = ({
  id,
  name,
  location,
  price,
  capacity,
  verified,
  imageUrl,
}) => {
  const navigate = useNavigate();
  
  return (
    <div className="bg-white rounded-2xl shadow-md overflow-hidden hover:shadow-lg transition-shadow">
      <div className="relative">
        <img
          src={imageUrl}
          alt={name}
          className="h-40 w-full object-cover"
        />
        {verified && (
          <span className="absolute top-2 right-2 bg-green-500 text-white text-xs px-2 py-1 rounded-full">
            ✔ Verified
          </span>
        )}
      </div>
      <div className="p-3">
        <h3 className="font-semibold text-slate-800 text-sm line-clamp-1">
          {name}
        </h3>
        <p className="text-xs text-slate-500">{location}</p>
        <p className="mt-1 text-sm">
          <span className="font-semibold">{price}</span>{" "}
          <span className="text-slate-500 text-xs">per day</span>
        </p>
        <p className="text-xs text-slate-500 mt-1">{capacity}</p>
        <button 
          onClick={() => navigate(`/hall/${id}`)}
          className="mt-2 w-full rounded-full bg-blue-600 text-white text-sm font-semibold py-2"
        >
          View details
        </button>
      </div>
    </div>
  );
};

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const [halls, setHalls] = useState<Hall[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchHalls = async () => {
      try {
        setLoading(true);
        const data = await hallApi.getHalls();
        // Get first 6 published halls for homepage
        setHalls(data.slice(0, 6));
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load halls');
      } finally {
        setLoading(false);
      }
    };

    fetchHalls();
  }, []);

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

  const formatCapacity = (hall: Hall) => {
    return `Up to ${hall.capacity} guests`;
  };

  return (
    <div className="min-h-screen bg-slate-50">
      {/* Top bar */}
      <header className="bg-gradient-to-r from-blue-600 to-blue-700 text-white">
        <div className="max-w-5xl mx-auto px-4 py-3 flex items-center justify-between">
          <h1 className="font-semibold text-lg">
            🌸 Namma Hall Finder
          </h1>
          <div className="flex items-center gap-3">
            <Link 
              to="/admin/login" 
              className="text-xs text-blue-100 hover:text-white underline opacity-80 hover:opacity-100"
            >
              Admin
            </Link>
            <span className="text-xs text-blue-100">
              Simple booking for rural areas
            </span>
          </div>
        </div>
      </header>

      {/* Main content */}
      <main className="max-w-5xl mx-auto px-4 pb-24 pt-6">
        {/* Search card */}
        <section>
          <div className="bg-white rounded-2xl shadow-xl p-4">
            <h2 className="font-semibold text-slate-800 mb-3">
              Find a hall for your function
            </h2>
            <div className="grid gap-3 md:grid-cols-4">
              <div className="md:col-span-2">
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Location
                </label>
                <input
                  type="text"
                  placeholder="Village / Town / Taluk"
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Event date
                </label>
                <input
                  type="date"
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Guests
                </label>
                <select className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500">
                  <option>Any</option>
                  <option>Up to 300</option>
                  <option>300 - 800</option>
                  <option>800+</option>
                </select>
              </div>
            </div>
            <div className="mt-3 flex gap-2">
              <button 
                onClick={() => navigate('/halls')}
                className="flex-1 rounded-full bg-blue-600 text-white font-semibold text-sm py-2.5"
              >
                Search halls
              </button>
              <button className="hidden md:block rounded-full border border-blue-600 text-blue-600 text-sm font-semibold px-4">
                Use my location
              </button>
            </div>
          </div>
        </section>

        {/* Popular section */}
        <section className="mt-6">
          <div className="flex items-center justify-between mb-2">
            <h2 className="font-semibold text-slate-800 text-base">
              Halls near you
            </h2>
            <Link to="/halls" className="text-xs text-slate-500">
              View all
            </Link>
          </div>
          {loading ? (
            <p className="text-sm text-slate-500">Loading halls...</p>
          ) : error ? (
            <p className="text-sm text-red-500">{error}</p>
          ) : halls.length === 0 ? (
            <p className="text-sm text-slate-500">No halls available at the moment.</p>
          ) : (
            <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
              {halls.map((hall) => (
                <HallCard
                  key={hall.id}
                  id={hall.id}
                  name={hall.name}
                  location={formatLocation(hall)}
                  price={formatPrice(hall)}
                  capacity={formatCapacity(hall)}
                  verified={hall.isVerified}
                  imageUrl={hall.images[0] || 'https://via.placeholder.com/400x300?text=Hall+Image'}
                />
              ))}
            </div>
          )}
        </section>
      </main>

      {/* Mobile bottom bar - generic contact/help */}
      <div className="fixed bottom-0 left-0 right-0 bg-white shadow-[0_-4px_12px_rgba(15,23,42,0.12)] px-4 py-2 flex gap-2 md:hidden">
        <button className="flex-1 rounded-full border border-slate-300 text-slate-700 text-sm py-2">
          📞 Call support
        </button>
        <a
          href="https://wa.me/919999999999?text=Hi%2C%20I%20need%20help%20to%20find%20a%20marriage%20hall"
          className="flex-1 rounded-full bg-blue-600 text-white text-center text-sm py-2 font-semibold"
        >
          💬 WhatsApp us
        </a>
      </div>
    </div>
  );
};

export default HomePage;
