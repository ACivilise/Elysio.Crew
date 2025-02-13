import { useState } from "react";
import type { RoomDTO, AgentDTO } from "@/models";

interface RoomFormProps {
  agents: AgentDTO[];
  onSubmit: (room: Omit<RoomDTO, "id" | "createdAt" | "updatedAt" | "conversations">) => Promise<void>;
}

export function RoomForm({ agents, onSubmit }: RoomFormProps) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [selectedAgents, setSelectedAgents] = useState<string[]>([]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Filter the full agent objects based on selected IDs
    const selectedAgentObjects = agents.filter(agent => 
      selectedAgents.includes(agent.id)
    );

    await onSubmit({
      name,
      description,
      agents: selectedAgentObjects
    });
    
    // Reset form
    setName("");
    setDescription("");
    setSelectedAgents([]);
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
        <label htmlFor="description" className="block text-sm font-medium">
          Description
        </label>
        <textarea
          id="description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
        />
      </div>
      <div>
        <label className="block text-sm font-medium">Select Agents</label>
        <div className="mt-2 space-y-2">
          {agents.map((agent) => (
            <label key={agent.id} className="flex items-center space-x-2">
              <input
                type="checkbox"
                checked={selectedAgents.includes(agent.id)}
                onChange={(e) => {
                  setSelectedAgents(
                    e.target.checked
                      ? [...selectedAgents, agent.id]
                      : selectedAgents.filter((id) => id !== agent.id)
                  );
                }}
                className="rounded border-gray-300"
              />
              <span>{agent.name}</span>
            </label>
          ))}
        </div>
      </div>
      <button
        type="submit"
        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
      >
        Create Room
      </button>
    </form>
  );
}