import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { adminApi, Hall } from "../services/api";

const AdminHallFormPage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;
  const [loading, setLoading] = useState(isEdit);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [name, setName] = useState("");
  const [addressLine, setAddressLine] = useState("");
  const [village, setVillage] = useState("");
  const [taluk, setTaluk] = useState("");
  const [district, setDistrict] = useState("");
  const [postalCode, setPostalCode] = useState("");
  const [capacity, setCapacity] = useState("");
  const [priceFrom, setPriceFrom] = useState("");
  const [priceTo, setPriceTo] = useState("");
  const [ownerName, setOwnerName] = useState("");
  const [ownerPhone, setOwnerPhone] = useState("");
  const [alternatePhone, setAlternatePhone] = useState("");
  const [googleMapsUrl, setGoogleMapsUrl] = useState("");
  const [isAc, setIsAc] = useState(false);
  const [hasDiningHall, setHasDiningHall] = useState(false);
  const [hasParking, setHasParking] = useState(false);
  const [hasRooms, setHasRooms] = useState(false);
  const [isPublished, setIsPublished] = useState(false);
  const [isVerified, setIsVerified] = useState(false);
  const [imageUrls, setImageUrls] = useState<string>("");

  useEffect(() => {
    if (isEdit && id) {
      const fetchHall = async () => {
        try {
          setLoading(true);
          const hallId = parseInt(id);
          const hall = await adminApi.getHallById(hallId);
          
          setName(hall.name);
          setAddressLine(hall.addressLine);
          setVillage(hall.village);
          setTaluk(hall.taluk);
          setDistrict(hall.district);
          setPostalCode(hall.postalCode || "");
          setCapacity(hall.capacity.toString());
          setPriceFrom(hall.priceFrom?.toString() || "");
          setPriceTo(hall.priceTo?.toString() || "");
          setOwnerName(hall.ownerName);
          setOwnerPhone(hall.ownerPhone);
          setAlternatePhone("");
          setGoogleMapsUrl(hall.googleMapsUrl || "");
          setIsAc(hall.isAc);
          setHasDiningHall(hall.hasDiningHall);
          setHasParking(hall.hasParking);
          setHasRooms(hall.hasRooms);
          setIsPublished(hall.isPublished);
          setIsVerified(hall.isVerified);
          setImageUrls(hall.images.join("\n"));
        } catch (err) {
          setError(err instanceof Error ? err.message : 'Failed to load hall');
        } finally {
          setLoading(false);
        }
      };

      fetchHall();
    }
  }, [isEdit, id]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      setSubmitting(true);
      setError(null);

      const images = imageUrls
        .split("\n")
        .map((x) => x.trim())
        .filter(Boolean);

      const payload = {
        name,
        addressLine,
        village,
        taluk,
        district,
        postalCode: postalCode || undefined,
        capacity: Number(capacity),
        priceFrom: priceFrom ? Number(priceFrom) : undefined,
        priceTo: priceTo ? Number(priceTo) : undefined,
        ownerName,
        ownerPhone,
        alternatePhone: alternatePhone || undefined,
        googleMapsUrl: googleMapsUrl || undefined,
        isAc,
        hasDiningHall,
        hasParking,
        hasRooms,
        isPublished,
        isVerified,
        imageUrls: images,
      };

      if (isEdit && id) {
        await adminApi.updateHall(parseInt(id), payload);
      } else {
        await adminApi.createHall(payload);
      }

      navigate('/admin/halls');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to save hall');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-slate-50 flex items-center justify-center">
        <p className="text-slate-600">Loading hall details...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-slate-50">
      <header className="bg-white border-b border-slate-200">
        <div className="max-w-4xl mx-auto px-4 py-3 flex items-center justify-between">
          <h1 className="font-semibold text-slate-900 text-base">
            {isEdit ? 'Edit Hall' : 'Add / Edit Hall'}
          </h1>
          <button 
            onClick={() => navigate('/admin/halls')}
            className="text-xs text-slate-500"
          >
            Back to list
          </button>
        </div>
      </header>

      <main className="max-w-4xl mx-auto px-4 pb-10">
        {error && (
          <div className="mt-4 p-2 bg-red-50 border border-red-200 rounded text-xs text-red-600">
            {error}
          </div>
        )}
        <form
          className="mt-4 bg-white rounded-2xl shadow-sm p-4 space-y-4"
          onSubmit={handleSubmit}
        >
          {/* Basic info */}
          <section>
            <h2 className="text-sm font-semibold text-slate-800 mb-2">
              Basic details
            </h2>
            <div className="grid gap-3 sm:grid-cols-2">
              <div className="sm:col-span-2">
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Hall name
                </label>
                <input
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Address Line
                </label>
                <input
                  value={addressLine}
                  onChange={(e) => setAddressLine(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Village / Area
                </label>
                <input
                  value={village}
                  onChange={(e) => setVillage(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Taluk
                </label>
                <input
                  value={taluk}
                  onChange={(e) => setTaluk(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  District
                </label>
                <input
                  value={district}
                  onChange={(e) => setDistrict(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Postal Code (optional)
                </label>
                <input
                  value={postalCode}
                  onChange={(e) => setPostalCode(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Capacity (max guests)
                </label>
                <input
                  type="number"
                  value={capacity}
                  onChange={(e) => setCapacity(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
            </div>
          </section>

          {/* Pricing */}
          <section>
            <h2 className="text-sm font-semibold text-slate-800 mb-2">
              Pricing
            </h2>
            <div className="grid gap-3 sm:grid-cols-2">
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Price from (₹)
                </label>
                <input
                  type="number"
                  value={priceFrom}
                  onChange={(e) => setPriceFrom(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Price to (₹)
                </label>
                <input
                  type="number"
                  value={priceTo}
                  onChange={(e) => setPriceTo(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
            </div>
          </section>

          {/* Facilities */}
          <section>
            <h2 className="text-sm font-semibold text-slate-800 mb-2">
              Facilities
            </h2>
            <div className="grid grid-cols-2 sm:grid-cols-4 gap-2 text-xs text-slate-700">
              <label className="inline-flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={isAc}
                  onChange={(e) => setIsAc(e.target.checked)}
                  className="rounded border-slate-300 text-blue-600 focus:ring-blue-500"
                />
                AC Hall
              </label>
              <label className="inline-flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={hasDiningHall}
                  onChange={(e) => setHasDiningHall(e.target.checked)}
                  className="rounded border-slate-300 text-blue-600 focus:ring-blue-500"
                />
                Dining Hall
              </label>
              <label className="inline-flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={hasParking}
                  onChange={(e) => setHasParking(e.target.checked)}
                  className="rounded border-slate-300 text-blue-600 focus:ring-blue-500"
                />
                Parking
              </label>
              <label className="inline-flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={hasRooms}
                  onChange={(e) => setHasRooms(e.target.checked)}
                  className="rounded border-slate-300 text-blue-600 focus:ring-blue-500"
                />
                Rooms
              </label>
            </div>
          </section>

          {/* Owner details */}
          <section>
            <h2 className="text-sm font-semibold text-slate-800 mb-2">
              Owner contact
            </h2>
            <div className="grid gap-3 sm:grid-cols-2">
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Owner name
                </label>
                <input
                  value={ownerName}
                  onChange={(e) => setOwnerName(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Owner phone
                </label>
                <input
                  value={ownerPhone}
                  onChange={(e) => setOwnerPhone(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="10-digit mobile"
                  required
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Alternate Phone (optional)
                </label>
                <input
                  value={alternatePhone}
                  onChange={(e) => setAlternatePhone(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
            </div>
          </section>

          {/* Status */}
          <section>
            <h2 className="text-sm font-semibold text-slate-800 mb-2">
              Status
            </h2>
            <div className="grid grid-cols-2 gap-2 text-xs text-slate-700">
              <label className="inline-flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={isPublished}
                  onChange={(e) => setIsPublished(e.target.checked)}
                  className="rounded border-slate-300 text-blue-600 focus:ring-blue-500"
                />
                Published
              </label>
              <label className="inline-flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={isVerified}
                  onChange={(e) => setIsVerified(e.target.checked)}
                  className="rounded border-slate-300 text-blue-600 focus:ring-blue-500"
                />
                Verified
              </label>
            </div>
          </section>

          {/* Location + images */}
          <section>
            <h2 className="text-sm font-semibold text-slate-800 mb-2">
              Maps & photos
            </h2>
            <div className="space-y-3">
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Google Maps URL
                </label>
                <input
                  value={googleMapsUrl}
                  onChange={(e) => setGoogleMapsUrl(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 mb-1">
                  Image URLs (one per line)
                </label>
                <textarea
                  value={imageUrls}
                  onChange={(e) => setImageUrls(e.target.value)}
                  className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  rows={4}
                  placeholder="https://example.com/image1.jpg&#10;https://example.com/image2.jpg"
                />
              </div>
            </div>
          </section>

          <div className="pt-3 border-t border-slate-100 flex justify-end gap-2">
            <button
              type="button"
              onClick={() => navigate('/admin/halls')}
              className="rounded-full border border-slate-300 text-slate-700 text-sm px-4 py-2"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={submitting}
              className="rounded-full bg-blue-600 text-white text-sm font-semibold px-5 py-2 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {submitting ? 'Saving...' : 'Save hall'}
            </button>
          </div>
        </form>
      </main>
    </div>
  );
};

export default AdminHallFormPage;
