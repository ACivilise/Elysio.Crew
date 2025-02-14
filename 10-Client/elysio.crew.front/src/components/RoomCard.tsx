import type { RoomDTO } from "@/models";

interface RoomCardProps {
  room: RoomDTO;
  onDelete: (id: string) => void;
  onEdit: (room: RoomDTO) => void;
}

export const RoomCard = ({ room, onDelete, onEdit }: RoomCardProps) => {
  return (
    <div className="border border-gray-700 rounded-lg p-4 bg-gray-800 shadow">
      <h2 className="text-xl font-semibold text-gray-100">{room.name}</h2>
      <p className="text-gray-400 mt-2">
        {room.description || "No description"}
      </p>
      <p className="text-sm text-gray-500 mt-2">
        Created: {new Date(room.createdAt).toLocaleDateString()}
      </p>
      <div className="mt-4 flex justify-end gap-2">
        <button
          onClick={() => onEdit(room)}
          className="bg-blue-600 text-gray-100 px-3 py-1 rounded hover:bg-blue-700 transition-colors flex items-center gap-1"
        >
          <span className="material-icons text-sm">edit</span>
          Edit
        </button>
        <button
          onClick={() => onDelete(room.id)}
          className="bg-red-600 text-gray-100 px-3 py-1 rounded hover:bg-red-700 transition-colors flex items-center gap-1"
        >
          <span className="material-icons text-sm">delete</span>
          Delete
        </button>
      </div>
    </div>
  );
};