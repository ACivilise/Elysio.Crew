import { useState, useEffect } from "react";
import type { RoomDTO, AgentDTO } from "@/models";

interface RoomFormProps {
  agents: AgentDTO[];
  onSubmit: (room: { id?: string; name: string; description?: string; agents: AgentDTO[] }) => Promise<void>;
  initialValues?: RoomDTO;
}

export function RoomForm({ agents, onSubmit, initialValues }: RoomFormProps) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [selectedAgents, setSelectedAgents] = useState<string[]>([]);

  useEffect(() => {
    if (initialValues) {
      setName(initialValues.name);
      setDescription(initialValues.description || "");
      setSelectedAgents(initialValues.agents.map(agent => agent.id));
    }
  }, [initialValues]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Filter the full agent objects based on selected IDs
    const selectedAgentObjects = agents.filter(agent => 
      selectedAgents.includes(agent.id)
    );

    await onSubmit({
      id: initialValues?.id,
      name,
      description,
      agents: selectedAgentObjects
    });
    
    if (!initialValues) {
      // Only reset if we're creating a new room
      setName("");
      setDescription("");
      setSelectedAgents([]);
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
        <label htmlFor="description" className="block text-sm font-medium text-gray-300">
          Description
        </label>
        <textarea
          id="description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-600 bg-gray-800 text-gray-100 px-3 py-2 focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
        />
      </div>
      <div>
        <label className="block text-sm font-medium text-gray-300">Select Agents</label>
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
                className="rounded border-gray-600 bg-gray-800 text-blue-500 focus:ring-blue-500 focus:ring-offset-gray-900"
              />
              <span className="text-gray-300">{agent.name}</span>
            </label>
          ))}
        </div>
      </div>
      <button
        type="submit"
        className="bg-blue-600 text-gray-100 px-4 py-2 rounded hover:bg-blue-700 transition-colors"
      >
        {initialValues ? 'Update Room' : 'Create Room'}
      </button>
    </form>
  );
}