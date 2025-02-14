import { useState, useEffect } from "react";
import type { ConversationDTO, RoomDTO } from "@/models";

interface ConversationFormProps {
  rooms: RoomDTO[];
  onSubmit: (conversation: { id?: string; name: string; roomId: string; room?: RoomDTO }) => Promise<void>;
  initialValues?: ConversationDTO;
}

export function ConversationForm({ rooms, onSubmit, initialValues }: ConversationFormProps) {
  const [name, setName] = useState("");
  const [roomId, setRoomId] = useState("");

  useEffect(() => {
    if (initialValues) {
      setName(initialValues.name);
      setRoomId(initialValues.roomId);
    }
  }, [initialValues]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const selectedRoom = rooms.find(room => room.id === roomId);

    await onSubmit({
      id: initialValues?.id,
      name,
      roomId,
      room: selectedRoom
    });
    
    if (!initialValues) {
      setName("");
      setRoomId("");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label htmlFor="name" className="block text-sm font-medium text-gray-300">
          Name
        </label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-600 bg-gray-800 text-gray-100 px-3 py-2 focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
          required
        />
      </div>
      <div>
        <label htmlFor="roomId" className="block text-sm font-medium text-gray-300">
          Select Room
        </label>
        <select
          id="roomId"
          value={roomId}
          onChange={(e) => setRoomId(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-600 bg-gray-800 text-gray-100 px-3 py-2 focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
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
        className="bg-blue-600 text-gray-100 px-4 py-2 rounded hover:bg-blue-700 transition-colors"
      >
        {initialValues ? 'Update Conversation' : 'Create Conversation'}
      </button>
    </form>
  );
}