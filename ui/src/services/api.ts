const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:5001/api';

export interface Hall {
  id: number;
  name: string;
  addressLine: string;
  village: string;
  taluk: string;
  district: string;
  postalCode?: string;
  capacity: number;
  priceFrom?: number;
  priceTo?: number;
  isAc: boolean;
  hasDiningHall: boolean;
  hasParking: boolean;
  hasRooms: boolean;
  ownerName: string;
  ownerPhone: string;
  googleMapsUrl?: string;
  images: string[];
}

// API returns array directly, not wrapped

export interface BookingCreateRequest {
  hallId: number;
  customerName: string;
  customerPhone: string;
  eventDate: string; // YYYY-MM-DD
  guestCount?: number;
  notes?: string;
}

export interface BookingCreateResponse {
  bookingId: number;
  summary: string;
}

export interface Booking {
  id: number;
  hallId: number;
  hallName: string;
  customerName: string;
  customerPhone: string;
  guestCount?: number;
  notes?: string;
  eventDate: string;
  status: number;
  paymentStatus: number;
  advanceAmount?: number;
  createdAtUtc: string;
}

export interface AdminLoginRequest {
  email: string;
  password: string;
}

export interface AdminLoginResponse {
  token: string;
  displayName: string;
}

export interface AdminHallCreateRequest {
  name: string;
  addressLine: string;
  village: string;
  taluk: string;
  district: string;
  postalCode?: string;
  capacity: number;
  priceFrom?: number;
  priceTo?: number;
  isAc: boolean;
  hasDiningHall: boolean;
  hasParking: boolean;
  hasRooms: boolean;
  ownerName: string;
  ownerPhone: string;
  alternatePhone?: string;
  googleMapsUrl?: string;
  isPublished: boolean;
  isVerified: boolean;
  imageUrls: string[];
}

// Public API
export const hallApi = {
  getHalls: async (params?: {
    district?: string;
    taluk?: string;
    village?: string;
    date?: string;
    minGuests?: number;
    maxGuests?: number;
    minPrice?: number;
    maxPrice?: number;
    search?: string;
  }): Promise<Hall[]> => {
    const queryParams = new URLSearchParams();
    if (params?.district) queryParams.append('district', params.district);
    if (params?.taluk) queryParams.append('taluk', params.taluk);
    if (params?.village) queryParams.append('village', params.village);
    if (params?.date) queryParams.append('date', params.date);
    if (params?.minGuests) queryParams.append('minGuests', params.minGuests.toString());
    if (params?.maxGuests) queryParams.append('maxGuests', params.maxGuests.toString());
    if (params?.minPrice) queryParams.append('minPrice', params.minPrice.toString());
    if (params?.maxPrice) queryParams.append('maxPrice', params.maxPrice.toString());
    if (params?.search) queryParams.append('search', params.search);

    const response = await fetch(`${API_BASE_URL}/halls?${queryParams}`);
    if (!response.ok) throw new Error('Failed to fetch halls');
    const data = await response.json();
    return data.halls || [];
  },

  getHallById: async (hallId: number): Promise<Hall> => {
    const response = await fetch(`${API_BASE_URL}/halls/${hallId}`);
    if (!response.ok) throw new Error('Failed to fetch hall');
    return await response.json();
  },

  checkAvailability: async (hallId: number, date: string): Promise<boolean> => {
    const response = await fetch(`${API_BASE_URL}/halls/${hallId}/availability?date=${date}`);
    if (!response.ok) throw new Error('Failed to check availability');
    const data = await response.json();
    return data.isAvailable;
  },
};

export const bookingApi = {
  createBooking: async (request: BookingCreateRequest): Promise<BookingCreateResponse> => {
    const response = await fetch(`${API_BASE_URL}/bookings`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request),
    });
    if (!response.ok) {
      if (response.status === 409) throw new Error('Hall is not available on this date');
      throw new Error('Failed to create booking');
    }
    return await response.json();
  },

  getBookingById: async (bookingId: number): Promise<Booking> => {
    const response = await fetch(`${API_BASE_URL}/bookings/${bookingId}`);
    if (!response.ok) throw new Error('Failed to fetch booking');
    return await response.json();
  },
};

// Admin API
export const adminApi = {
  login: async (request: AdminLoginRequest): Promise<AdminLoginResponse> => {
    const response = await fetch(`${API_BASE_URL}/admin/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request),
    });
    if (!response.ok) throw new Error('Invalid credentials');
    return await response.json();
  },

  getHalls: async (params?: {
    search?: string;
    district?: string;
    taluk?: string;
    village?: string;
    isPublished?: boolean;
  }): Promise<Hall[]> => {
    const queryParams = new URLSearchParams();
    if (params?.search) queryParams.append('search', params.search);
    if (params?.district) queryParams.append('district', params.district);
    if (params?.taluk) queryParams.append('taluk', params.taluk);
    if (params?.village) queryParams.append('village', params.village);
    if (params?.isPublished !== undefined) queryParams.append('isPublished', params.isPublished.toString());

    const token = localStorage.getItem('adminToken');
    const response = await fetch(`${API_BASE_URL}/admin/halls?${queryParams}`, {
      headers: token ? { 'Authorization': `Bearer ${token}` } : {},
    });
    if (!response.ok) throw new Error('Failed to fetch halls');
    const data = await response.json();
    return data.halls || [];
  },

  getHallById: async (hallId: number): Promise<Hall> => {
    const token = localStorage.getItem('adminToken');
    const response = await fetch(`${API_BASE_URL}/admin/halls/${hallId}`, {
      headers: token ? { 'Authorization': `Bearer ${token}` } : {},
    });
    if (!response.ok) throw new Error('Failed to fetch hall');
    return await response.json();
  },

  createHall: async (request: AdminHallCreateRequest): Promise<number> => {
    const token = localStorage.getItem('adminToken');
    const response = await fetch(`${API_BASE_URL}/admin/halls`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        ...(token ? { 'Authorization': `Bearer ${token}` } : {}),
      },
      body: JSON.stringify(request),
    });
    if (!response.ok) throw new Error('Failed to create hall');
    return await response.json();
  },

  updateHall: async (hallId: number, request: AdminHallCreateRequest): Promise<void> => {
    const token = localStorage.getItem('adminToken');
    const response = await fetch(`${API_BASE_URL}/admin/halls/${hallId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        ...(token ? { 'Authorization': `Bearer ${token}` } : {}),
      },
      body: JSON.stringify(request),
    });
    if (!response.ok) throw new Error('Failed to update hall');
  },

  deleteHall: async (hallId: number): Promise<void> => {
    const token = localStorage.getItem('adminToken');
    const response = await fetch(`${API_BASE_URL}/admin/halls/${hallId}`, {
      method: 'DELETE',
      headers: token ? { 'Authorization': `Bearer ${token}` } : {},
    });
    if (!response.ok) throw new Error('Failed to delete hall');
  },
};

