import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams, Link } from "react-router-dom";
import { hallApi, Hall } from "../services/api";

type HallListItemProps = {
  id: number;
  name: string;
  location: string;
  price: string;
  capacity: string;
  imageUrl: string;
  verified?: boolean;
  facilities: string[];
  ownerPhone: string;
};

const HallListItem: React.FC<HallListItemProps> = ({
  id,
  name,
  location,
  price,
  capacity,
  imageUrl,
  verified,
  facilities,
  ownerPhone,
}) => {
  const navigate = useNavigate();
  
  return (
    <div className="bg-white rounded-2xl shadow-sm overflow-hidden flex flex-col sm:flex-row">
      <img
        src={imageUrl}
        alt={name}
        className="h-40 w-full sm:w-40 object-cover"
      />
      <div className="p-3 flex-1 flex flex-col justify-between">
        <div>
          <div className="flex items-center justify-between gap-2">
            <h3 className="font-semibold text-slate-800 text-sm sm:text-base">
              {name}
            </h3>
            {verified && (
              <span className="bg-green-500 text-white text-[10px] px-2 py-0.5 rounded-full">
                ✔ Verified
              </span>
            )}
          </div>
          <p className="text-xs text-slate-500 mt-0.5">{location}</p>
          <p className="text-sm mt-1">
            <span className="font-semibold">{price}</span>{" "}
            <span className="text-xs text-slate-500">per day</span>
          </p>
          <p className="text-xs text-slate-500">{capacity}</p>
          <div className="mt-2 flex flex-wrap gap-1">
            {facilities.map((facility) => (
              <span
                key={facility}
                className="px-2 py-0.5 rounded-full bg-indigo-50 text-[11px] text-slate-600"
              >
                {facility}
              </span>
            ))}
          </div>
        </div>
        <div className="mt-3 flex gap-2">
          <button 
            onClick={() => navigate(`/hall/${id}`)}
            className="flex-1 rounded-full bg-blue-600 text-white text-xs sm:text-sm font-semibold py-2"
          >
            View details
          </button>
          <a
            href={`https://wa.me/${ownerPhone}?text=${encodeURIComponent(`Hi, I'm interested in ${name}`)}`}
            className="flex-1 rounded-full border border-blue-600 text-blue-600 text-xs sm:text-sm font-semibold py-2 text-center"
          >
            WhatsApp
          </a>
        </div>
      </div>
    </div>
  );
};

const HallListPage: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [halls, setHalls] = useState<Hall[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchHalls = async () => {
      try {
        setLoading(true);
        const params: any = {};
        const district = searchParams.get('district');
        const taluk = searchParams.get('taluk');
        const village = searchParams.get('village');
        const date = searchParams.get('date');
        const search = searchParams.get('search');

        if (district) params.district = district;
        if (taluk) params.taluk = taluk;
        if (village) params.village = village;
        if (date) params.date = date;
        if (search) params.search = search;

        const data = await hallApi.getHalls(params);
        setHalls(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load halls');
      } finally {
        setLoading(false);
      }
    };

    fetchHalls();
  }, [searchParams]);

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

  const getFacilities = (hall: Hall): string[] => {
    const facilities: string[] = [];
    if (hall.isAc) facilities.push('AC Hall');
    if (hall.hasDiningHall) facilities.push('Dining Hall');
    if (hall.hasParking) facilities.push('Parking');
    if (hall.hasRooms) facilities.push('Rooms');
    return facilities;
  };

  const location = searchParams.get('village') || searchParams.get('taluk') || searchParams.get('district') || 'All Locations';

  return (
    <div className="min-h-screen bg-slate-50">
      <header className="bg-white border-b border-slate-200">
        <div className="max-w-5xl mx-auto px-4 py-3 flex items-center justify-between gap-3">
          <h1 className="font-semibold text-slate-800 text-base">
            Halls in {location}
          </h1>
          <div className="flex items-center gap-3">
            <Link 
              to="/admin/login" 
              className="text-xs text-slate-500 hover:text-slate-700"
            >
              Admin
            </Link>
            <button 
              onClick={() => navigate('/')}
              className="text-xs text-blue-600"
            >
              Change location
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-5xl mx-auto px-4 pb-20">
        {/* Filter bar */}
        <div className="mt-3 flex flex-wrap gap-2 text-xs">
          <button className="px-3 py-1 rounded-full bg-blue-600 text-white font-semibold">
            All
          </button>
          <button className="px-3 py-1 rounded-full bg-white border border-slate-200 text-slate-700">
            Up to 300 guests
          </button>
          <button className="px-3 py-1 rounded-full bg-white border border-slate-200 text-slate-700">
            300 - 800 guests
          </button>
          <button className="px-3 py-1 rounded-full bg-white border border-slate-200 text-slate-700">
            AC halls
          </button>
        </div>

        {/* Result list */}
        <div className="mt-3 space-y-3">
          {loading ? (
            <p className="text-sm text-slate-500 mt-4">Loading halls...</p>
          ) : error ? (
            <p className="text-sm text-red-500 mt-4">{error}</p>
          ) : halls.length === 0 ? (
            <p className="text-sm text-slate-500 mt-4">
              No halls found. Try another area or date.
            </p>
          ) : (
            halls.map((hall) => (
              <HallListItem
                key={hall.id}
                id={hall.id}
                name={hall.name}
                location={formatLocation(hall)}
                price={formatPrice(hall)}
                capacity={formatCapacity(hall)}
                verified={hall.isVerified}
                imageUrl={hall.images[0] || 'https://via.placeholder.com/400x300?text=Hall+Image'}
                facilities={getFacilities(hall)}
                ownerPhone={hall.ownerPhone}
              />
            ))
          )}
        </div>
      </main>
    </div>
  );
};

export default HallListPage;
