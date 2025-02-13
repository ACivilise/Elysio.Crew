"use client";

import { useEffect, useState } from "react";
import { getRooms, deleteRoom, createRoom, getAgents } from "@/services/api";
import type { RoomDTO, AgentDTO } from "@/models";
import { RoomForm } from "../../components/RoomForm";
import RightPanel from "@/components/RightPanel";

export default function RoomsPage() {
  const [rooms, setRooms] = useState<RoomDTO[]>([]);
  const [agents, setAgents] = useState<AgentDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [isPanelOpen, setIsPanelOpen] = useState(false);

  const fetchRooms = async () => {
    try {
      const response = await getRooms();
      setRooms(response.data);
    } catch (err) {
      setError("Failed to fetch rooms");
    }
  };

  const fetchAgents = async () => {
    try {
      const response = await getAgents();
      setAgents(response.data);
    } catch (err) {
      setError("Failed to fetch agents");
    }
  };

  const handleCreate = async (room: Omit<RoomDTO, "id" | "createdAt" | "updatedAt" | "conversations">) => {
    try {
      await createRoom(room);
      fetchRooms();
      setIsPanelOpen(false);
    } catch (err) {
      setError("Failed to create room");
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteRoom(id);
      setRooms(rooms.filter((room) => room.id !== id));
    } catch (err) {
      setError("Failed to delete room");
    }
  };

  useEffect(() => {
    Promise.all([fetchRooms(), fetchAgents()]).finally(() => setIsLoading(false));
  }, []);

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Rooms</h1>
        <button
          onClick={() => setIsPanelOpen(true)}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          Create Room
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {rooms.map((room) => (
          <div key={room.id} className="border rounded-lg p-4 shadow">
            <h2 className="text-xl font-semibold">{room.name}</h2>
            <p className="text-gray-600 mt-2">
              {room.description || "No description"}
            </p>
            <p className="text-sm text-gray-500 mt-2">
              Created: {new Date(room.createdAt).toLocaleDateString()}
            </p>
            <div className="mt-4 flex justify-end gap-2">
              <button
                onClick={() => handleDelete(room.id)}
                className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600"
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>

      <RightPanel
        isOpen={isPanelOpen}
        onClose={() => setIsPanelOpen(false)}
        title="Create New Room"
      >
        <RoomForm agents={agents} onSubmit={handleCreate} />
      </RightPanel>
    </div>
  );
}
