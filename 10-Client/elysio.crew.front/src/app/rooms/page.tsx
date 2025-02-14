"use client";

import { useEffect, useState, useCallback } from "react";
import { getRooms, deleteRoom, createRoom, updateRoom, getAgents } from "@/services/api";
import type { RoomDTO, AgentDTO } from "@/models";
import { RoomForm } from "../../components/RoomForm";
import RightPanel from "@/components/RightPanel";
import { RoomCard } from "@/components/RoomCard";

export default function RoomsPage() {
  const [rooms, setRooms] = useState<RoomDTO[]>([]);
  const [agents, setAgents] = useState<AgentDTO[]>([]);
  const [isLoadingRooms, setIsLoadingRooms] = useState(true);
  const [isLoadingAgents, setIsLoadingAgents] = useState(true);
  const [error, setError] = useState("");
  const [isPanelOpen, setIsPanelOpen] = useState(false);
  const [editingRoom, setEditingRoom] = useState<RoomDTO | null>(null);

  const fetchRooms = useCallback(async (retryCount = 0) => {
    setIsLoadingRooms(true);
    try {
      const response = await getRooms(true); // Always include agents since we need them for RoomCard
      setRooms(response.data);
      setError("");
    } catch (err: any) {
      const errorMessage = err.message || 'Unknown error';
      console.error("Error fetching rooms:", err);
      
      if (retryCount < 2 && err.originalError?.code === 'ECONNABORTED') {
        // Wait 2 seconds before retrying
        await new Promise(resolve => setTimeout(resolve, 2000));
        return fetchRooms(retryCount + 1);
      }
      
      setError(`Failed to fetch rooms: ${errorMessage}`);
    } finally {
      setIsLoadingRooms(false);
    }
  }, []);

  const fetchAgents = useCallback(async (retryCount = 0) => {
    setIsLoadingAgents(true);
    try {
      const response = await getAgents();
      setAgents(response.data);
      setError("");
    } catch (err: any) {
      const errorMessage = err.message || 'Unknown error';
      console.error("Error fetching agents:", err);
      
      if (retryCount < 2 && err.originalError?.code === 'ECONNABORTED') {
        // Wait 2 seconds before retrying
        await new Promise(resolve => setTimeout(resolve, 2000));
        return fetchAgents(retryCount + 1);
      }
      
      setError(`Failed to fetch agents: ${errorMessage}`);
    } finally {
      setIsLoadingAgents(false);
    }
  }, []);

  const handleCreate = async (room: Omit<RoomDTO, "id" | "createdAt" | "updatedAt" | "conversations">) => {
    try {
      await createRoom(room);
      await fetchRooms();
      setIsPanelOpen(false);
    } catch (err: any) {
      setError(`Failed to create room: ${err.message || 'Unknown error'}`);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteRoom(id);
      setRooms(rooms.filter((room) => room.id !== id));
    } catch (err: any) {
      setError(`Failed to delete room: ${err.message || 'Unknown error'}`);
    }
  };

  const handleEdit = async (room: RoomDTO) => {
    setEditingRoom(room);
    setIsPanelOpen(true);
  };

  const handleUpdate = async (room: Omit<RoomDTO, "id" | "createdAt" | "updatedAt" | "conversations">) => {
    try {
      if (!editingRoom) return;
      const updateData = {
        id: editingRoom.id,
        name: room.name,
        description: room.description,
        agentIds: room.agents.map(agent => agent.id)
      };
      await updateRoom(editingRoom.id, updateData);
      await fetchRooms();
      setIsPanelOpen(false);
      setEditingRoom(null);
    } catch (err: any) {
      setError(`Failed to update room: ${err.message || 'Unknown error'}`);
    }
  };

  const handlePanelClose = () => {
    setIsPanelOpen(false);
    setEditingRoom(null);
  };

  useEffect(() => {
    let mounted = true;

    const loadData = async () => {
      if (mounted) {
        await fetchRooms();
        await fetchAgents();
      }
    };

    loadData();

    return () => {
      mounted = false;
    };
  }, [fetchRooms, fetchAgents]);

  if (isLoadingRooms || isLoadingAgents) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gray-900">
        <div className="text-gray-100">
          {isLoadingRooms ? "Loading rooms..." : "Loading agents..."}
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gray-900">
        <div className="text-red-400">Error: {error}</div>
      </div>
    );
  }

  return (
    <div className="p-8 bg-gray-900 min-h-screen">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-100">Rooms</h1>
        <button
          onClick={() => setIsPanelOpen(true)}
          className="bg-blue-600 text-gray-100 px-4 py-2 rounded hover:bg-blue-700 transition-colors"
        >
          Create Room
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {rooms.map((room) => (
          <RoomCard
            key={room.id}
            room={room}
            onDelete={handleDelete}
            onEdit={handleEdit}
          />
        ))}
      </div>

      <RightPanel
        isOpen={isPanelOpen}
        onClose={handlePanelClose}
        title={editingRoom ? "Edit Room" : "Create New Room"}
      >
        <RoomForm 
          agents={agents} 
          onSubmit={editingRoom ? handleUpdate : handleCreate}
          initialValues={editingRoom || undefined}
        />
      </RightPanel>
    </div>
  );
}
