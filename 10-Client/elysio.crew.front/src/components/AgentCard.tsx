import type { AgentDTO } from "@/models";

interface AgentCardProps {
  agent: AgentDTO;
  onDelete: (id: string) => void;
  onEdit: (agent: AgentDTO) => void;
}

export const AgentCard = ({ agent, onDelete, onEdit }: AgentCardProps) => {
  return (
    <div className="border border-gray-700 rounded-lg p-4 bg-gray-800 shadow">
      <h2 className="text-xl font-semibold text-gray-100">{agent.name}</h2>
      <p className="text-gray-400 mt-2">
        {agent.description || "No description"}
      </p>
      <div className="mt-4 text-gray-300">
        <p>
          <strong>Model:</strong> {agent.model}
        </p>
        <p>
          <strong>Temperature:</strong> {agent.temperature}
        </p>
      </div>
      <div className="mt-4 flex justify-end gap-2">
        <button
          onClick={() => onEdit(agent)}
          className="bg-blue-600 text-gray-100 px-3 py-1 rounded hover:bg-blue-700 transition-colors flex items-center gap-1"
        >
          <span className="material-icons text-sm">edit</span>
          Edit
        </button>
        <button
          onClick={() => onDelete(agent.id)}
          className="bg-red-600 text-gray-100 px-3 py-1 rounded hover:bg-red-700 transition-colors flex items-center gap-1"
        >
          <span className="material-icons text-sm">delete</span>
          Delete
        </button>
      </div>
    </div>
  );
};