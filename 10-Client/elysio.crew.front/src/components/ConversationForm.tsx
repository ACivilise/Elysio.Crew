import { useState } from "react";
import type { ConversationDTO, RoomDTO } from "@/models";

interface ConversationFormProps {
  rooms: RoomDTO[];
  onSubmit: (conversation: Omit<ConversationDTO, "id" | "createdAt" | "updatedAt" | "messages">) => Promise<void>;
}

export function ConversationForm({ rooms, onSubmit }: ConversationFormProps) {
  const [name, setName] = useState("");
  const [roomId, setRoomId] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Find the selected room object
    const selectedRoom = rooms.find(room => room.id === roomId);

    await onSubmit({
      name,
      roomId,
      room: selectedRoom
    });
    
    // Reset form
    setName("");
    setRoomId("");
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label htmlFor="name" className="block text-sm font-medium">
          Name
        </label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
          required
        />
      </div>
      <div>
        <label htmlFor="roomId" className="block text-sm font-medium">
          Select Room
        </label>
        <select
          id="roomId"
          value={roomId}
          onChange={(e) => setRoomId(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
          required
        >
          <option value="">Select a room...</option>
          {rooms.map((room) => (
            <option key={room.id} value={room.id}>
              {room.name}
            </option>
          ))}
        </select>
      </div>
      <button
        type="submit"
        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
      >
        Create Conversation
      </button>
    </form>
  );
}