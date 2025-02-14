"use client";

import { useEffect, useState, useCallback } from "react";
import { getAgents, deleteAgent, createAgent, updateAgent } from "@/services/api";
import type { AgentDTO } from "@/models";
import { AgentForm } from "../../components/AgentForm";
import RightPanel from "@/components/RightPanel";
import { AgentCard } from "@/components/AgentCard";

export default function AgentsPage() {
  const [agents, setAgents] = useState<AgentDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [isPanelOpen, setIsPanelOpen] = useState(false);
  const [editingAgent, setEditingAgent] = useState<AgentDTO | null>(null);

  const fetchAgents = useCallback(async (retryCount = 0) => {
    setIsLoading(true);
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
      setIsLoading(false);
    }
  }, []);

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

  const handleEdit = async (agent: AgentDTO) => {
    setEditingAgent(agent);
    setIsPanelOpen(true);
  };

  const handleUpdate = async (agent: Omit<AgentDTO, "id" | "createdAt" | "updatedAt" | "rooms" | "messages">) => {
    try {
      if (!editingAgent) return;
      await updateAgent(editingAgent.id, agent);
      fetchAgents();
      setIsPanelOpen(false);
      setEditingAgent(null);
    } catch (err) {
      setError("Failed to update agent");
    }
  };

  const handlePanelClose = () => {
    setIsPanelOpen(false);
    setEditingAgent(null);
  };

  useEffect(() => {
    let mounted = true;

    const loadData = async () => {
      if (mounted) {
        await fetchAgents();
      }
    };

    loadData();

    return () => {
      mounted = false;
    };
  }, [fetchAgents]);

  if (isLoading) return (
    <div className="flex justify-center items-center min-h-screen bg-gray-900">
      <div className="text-gray-100">Loading agents...</div>
    </div>
  );

  if (error) return (
    <div className="flex justify-center items-center min-h-screen bg-gray-900">
      <div className="text-red-400">Error: {error}</div>
    </div>
  );

  return (
    <div className="p-8 bg-gray-900 min-h-screen">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-100">Agents</h1>
        <button
          onClick={() => setIsPanelOpen(true)}
          className="bg-blue-600 text-gray-100 px-4 py-2 rounded hover:bg-blue-700 transition-colors"
        >
          Create Agent
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {agents.map((agent) => (
          <AgentCard 
            key={agent.id} 
            agent={agent} 
            onDelete={handleDelete}
            onEdit={handleEdit}
          />
        ))}
      </div>

      <RightPanel
        isOpen={isPanelOpen}
        onClose={handlePanelClose}
        title={editingAgent ? "Edit Agent" : "Create New Agent"}
      >
        <AgentForm 
          onSubmit={editingAgent ? handleUpdate : handleCreate}
          initialValues={editingAgent || undefined}
        />
      </RightPanel>
    </div>
  );
}
