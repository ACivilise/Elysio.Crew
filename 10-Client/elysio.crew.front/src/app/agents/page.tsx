"use client";

import { useEffect, useState } from "react";
import { getAgents, deleteAgent, createAgent, updateAgent } from "@/services/api";
import type { AgentDTO } from "@/models";
import { AgentForm } from "../../components/AgentForm";
import RightPanel from "@/components/RightPanel";

export default function AgentsPage() {
  const [agents, setAgents] = useState<AgentDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [isPanelOpen, setIsPanelOpen] = useState(false);

  const fetchAgents = async () => {
    try {
      const response = await getAgents();
      setAgents(response.data);
    } catch (err) {
      setError("Failed to fetch agents");
    } finally {
      setIsLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteAgent(id);
      setAgents(agents.filter((agent) => agent.id !== id));
    } catch (err) {
      setError("Failed to delete agent");
    }
  };

  const handleCreate = async (agent: Omit<AgentDTO, "id" | "createdAt" | "updatedAt" | "rooms" | "messages">) => {
    try {
      await createAgent(agent);
      fetchAgents(); // Refresh the list
      setIsPanelOpen(false); // Close panel after successful creation
    } catch (err) {
      setError("Failed to create agent");
    }
  };

  useEffect(() => {
    fetchAgents();
  }, []);

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Agents</h1>
        <button
          onClick={() => setIsPanelOpen(true)}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          Create Agent
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {agents.map((agent) => (
          <div key={agent.id} className="border rounded-lg p-4 shadow">
            <h2 className="text-xl font-semibold">{agent.name}</h2>
            <p className="text-gray-600 mt-2">
              {agent.description || "No description"}
            </p>
            <div className="mt-4">
              <p>
                <strong>Model:</strong> {agent.model}
              </p>
              <p>
                <strong>Temperature:</strong> {agent.temperature}
              </p>
            </div>
            <div className="mt-4 flex justify-end gap-2">
              <button
                onClick={() => handleDelete(agent.id)}
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
        title="Create New Agent"
      >
        <AgentForm onSubmit={handleCreate} />
      </RightPanel>
    </div>
  );
}
