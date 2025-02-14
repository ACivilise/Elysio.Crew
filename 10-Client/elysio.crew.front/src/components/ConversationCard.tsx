import type { ConversationDTO, RoomDTO } from "@/models";

interface ConversationCardProps {
  conversation: ConversationDTO;
  rooms: RoomDTO[];
  onDelete: (id: string) => void;
  onEdit: (conversation: ConversationDTO) => void;
}

export const ConversationCard = ({ conversation, rooms, onDelete, onEdit }: ConversationCardProps) => {
  return (
    <div className="border border-gray-700 rounded-lg p-4 bg-gray-800 shadow">
      <h2 className="text-xl font-semibold text-gray-100">{conversation.name}</h2>
      <p className="text-gray-400 mt-2">
        Room: {rooms.find(r => r.id === conversation.roomId)?.name || conversation.roomId}
      </p>
      <p className="text-sm text-gray-500">
        Created: {new Date(conversation.createdAt).toLocaleDateString()}
      </p>
      <div className="mt-4 flex justify-end gap-2">
        <button
          onClick={() => onEdit(conversation)}
          className="bg-blue-600 text-gray-100 px-3 py-1 rounded hover:bg-blue-700 transition-colors flex items-center gap-1"
        >
          <span className="material-icons text-sm">edit</span>
          Edit
        </button>
        <button
          onClick={() => onDelete(conversation.id)}
          className="bg-red-600 text-gray-100 px-3 py-1 rounded hover:bg-red-700 transition-colors flex items-center gap-1"
        >
          <span className="material-icons text-sm">delete</span>
          Delete
        </button>
      </div>
    </div>
  );
};