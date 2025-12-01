import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { adminApi, Hall } from "../services/api";

const AdminHallListPage: React.FC = () => {
  const navigate = useNavigate();
  const [halls, setHalls] = useState<Hall[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchHalls = async () => {
      try {
        setLoading(true);
        const data = await adminApi.getHalls();
        setHalls(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load halls');
      } finally {
        setLoading(false);
      }
    };

    fetchHalls();
  }, []);

  const handleDelete = async (hallId: number) => {
    if (!confirm('Are you sure you want to delete this hall?')) return;

    try {
      await adminApi.deleteHall(hallId);
      setHalls(halls.filter(h => h.id !== hallId));
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Failed to delete hall');
    }
  };

  const formatLocation = (hall: Hall) => {
    return `${hall.village}, ${hall.district}`;
  };

  const formatPriceRange = (hall: Hall) => {
    if (hall.priceFrom && hall.priceTo) {
      return `₹${(hall.priceFrom / 1000).toFixed(0)}k – ₹${(hall.priceTo / 1000).toFixed(0)}k`;
    } else if (hall.priceFrom) {
      return `From ₹${(hall.priceFrom / 1000).toFixed(0)}k`;
    }
    return "Price on request";
  };

  const formatCapacity = (hall: Hall) => {
    return `Up to ${hall.capacity}`;
  };

  return (
    <div className="min-h-screen bg-slate-50">
      {/* Admin header */}
      <header className="bg-white border-b border-slate-200">
        <div className="max-w-5xl mx-auto px-4 py-3 flex items-center justify-between">
          <h1 className="font-semibold text-slate-900 text-base">
            Admin – Halls
          </h1>
          <div className="flex items-center gap-3">
            <button className="text-xs text-slate-500">
              Bookings
            </button>
            <button 
              onClick={() => {
                localStorage.removeItem('adminToken');
                localStorage.removeItem('adminDisplayName');
                navigate('/admin/login');
              }}
              className="text-xs text-slate-500"
            >
              Logout
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-5xl mx-auto px-4 pb-10">
        <div className="flex items-center justify-between mt-4">
          <h2 className="text-sm font-semibold text-slate-800">
            All halls
          </h2>
          <button 
            onClick={() => navigate('/admin/halls/new')}
            className="rounded-full bg-blue-600 text-white text-xs font-semibold px-4 py-2"
          >
            + Add new hall
          </button>
        </div>

        {loading ? (
          <p className="text-sm text-slate-500 mt-4">Loading halls...</p>
        ) : error ? (
          <p className="text-sm text-red-500 mt-4">{error}</p>
        ) : (
          <div className="mt-3 bg-white rounded-2xl shadow-sm overflow-hidden">
            <table className="min-w-full text-xs">
              <thead className="bg-slate-50 text-slate-500">
                <tr>
                  <th className="px-3 py-2 text-left font-semibold">
                    Name
                  </th>
                  <th className="px-3 py-2 text-left font-semibold">
                    Location
                  </th>
                  <th className="px-3 py-2 text-left font-semibold">
                    Price
                  </th>
                  <th className="px-3 py-2 text-left font-semibold">
                    Capacity
                  </th>
                  <th className="px-3 py-2 text-left font-semibold">
                    Status
                  </th>
                  <th className="px-3 py-2 text-right font-semibold">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody>
                {halls.map((hall, idx) => (
                  <tr
                    key={hall.id}
                    className={idx % 2 === 0 ? "bg-white" : "bg-slate-50"}
                  >
                    <td className="px-3 py-2 align-top">
                      <p className="font-semibold text-slate-800">
                        {hall.name}
                      </p>
                      <p className="text-[11px] text-slate-500">
                        ID: {hall.id}
                      </p>
                    </td>
                    <td className="px-3 py-2 align-top text-slate-600">
                      {formatLocation(hall)}
                    </td>
                    <td className="px-3 py-2 align-top text-slate-600">
                      {formatPriceRange(hall)}
                    </td>
                    <td className="px-3 py-2 align-top text-slate-600">
                      {formatCapacity(hall)}
                    </td>
                    <td className="px-3 py-2 align-top">
                      <span
                        className={`inline-flex items-center px-2 py-0.5 rounded-full text-[11px] ${
                          hall.isPublished
                            ? "bg-green-100 text-green-700"
                            : "bg-slate-200 text-slate-700"
                        }`}
                      >
                        {hall.isPublished ? "Published" : "Hidden"}
                      </span>
                    </td>
                    <td className="px-3 py-2 align-top text-right">
                      <div className="inline-flex gap-2">
                        <button 
                          onClick={() => navigate(`/admin/halls/edit/${hall.id}`)}
                          className="text-[11px] text-blue-600"
                        >
                          Edit
                        </button>
                        <button className="text-[11px] text-slate-500">
                          Availability
                        </button>
                        <button 
                          onClick={() => handleDelete(hall.id)}
                          className="text-[11px] text-red-500"
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
                {halls.length === 0 && (
                  <tr>
                    <td
                      colSpan={6}
                      className="px-3 py-4 text-center text-slate-500"
                    >
                      No halls added yet.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}
      </main>
    </div>
  );
};

export default AdminHallListPage;
