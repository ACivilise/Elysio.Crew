import axios from "axios";

const baseURL = "https://localhost:7056/api";

const api = axios.create({
  baseURL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Agents
export const getAgents = () => api.get("/agents");
export const getAgent = (id: string) => api.get(`/agents/${id}`);
export const createAgent = (data: any) => api.post("/agents", data);
export const updateAgent = (id: string, data: any) =>
  api.put(`/agents/${id}`, data);
export const deleteAgent = (id: string) => api.delete(`/agents/${id}`);

// Rooms
export const getRooms = () => api.get("/rooms");
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
