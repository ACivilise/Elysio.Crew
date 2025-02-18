import axios, { AxiosError } from "axios";

const baseURL = `${process.env.NEXT_PUBLIC_API_URL}/api`;

const api = axios.create({
  baseURL,
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 30000, // Increased to 30 seconds
});

// Add response interceptor for better error handling
api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.code === 'ECONNABORTED') {
      return Promise.reject({ 
        message: 'The request took too long to complete. Please try again.',
        originalError: error
      });
    }
    if (error.response?.status === 404) {
      return Promise.reject({ 
        message: 'The requested resource was not found.',
        originalError: error
      });
    }
    if (!error.response) {
      return Promise.reject({ 
        message: 'Unable to connect to the server. Please check your connection.',
        originalError: error
      });
    }
    return Promise.reject({
      message: (error.response.data as any)?.message || 'An unexpected error occurred',
      originalError: error
    });
  }
);

// Agents
export const getAgents = () => api.get("/agents");
export const getAgent = (id: string) => api.get(`/agents/${id}`);
export const createAgent = (data: any) => api.post("/agents", data);
export const updateAgent = (id: string, data: any) =>
  api.put(`/agents/${id}`, data);
export const deleteAgent = (id: string) => api.delete(`/agents/${id}`);

// Rooms
export const getRooms = (includeAgents: boolean = false) => api.get(`/rooms?agents=${includeAgents}`);
export const getRoom = (id: string) => api.get(`/rooms/${id}`);
export const createRoom = (data: any) => api.post("/rooms", data);
export const updateRoom = (id: string, data: any) =>
  api.put(`/rooms/${id}`, data);
export const deleteRoom = (id: string) => api.delete(`/rooms/${id}`);

// Conversations
export const getConversations = () => api.get("/conversations");
export const getConversation = (id: string) => api.get(`/conversations/${id}`);
export const createConversation = (data: any) =>
  api.post("/conversations", data);
export const updateConversation = (id: string, data: any) =>
  api.put(`/conversations/${id}`, data);
export const deleteConversation = (id: string) =>
  api.delete(`/conversations/${id}`);

export const startConversationStream = (conversationId: string, initialMessage: string) => 
  fetch(`${baseURL}/conversations/${conversationId}/stream`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ conversationId, initialMessage }),
  });
