"use client";

import { useEffect, useState, useCallback } from "react";
import { getConversations, deleteConversation, createConversation, updateConversation, getRooms } from "@/services/api";
import type { ConversationDTO, RoomDTO } from "@/models";
import { ConversationForm } from "../../components/ConversationForm";
import RightPanel from "@/components/RightPanel";
import { ConversationCard } from "@/components/ConversationCard";

export default function ConversationsPage() {
  const [conversations, setConversations] = useState<ConversationDTO[]>([]);
  const [rooms, setRooms] = useState<RoomDTO[]>([]);
  const [isLoadingConversations, setIsLoadingConversations] = useState(true);
  const [isLoadingRooms, setIsLoadingRooms] = useState(true);
  const [error, setError] = useState("");
  const [isPanelOpen, setIsPanelOpen] = useState(false);
  const [editingConversation, setEditingConversation] = useState<ConversationDTO | null>(null);

  const fetchConversations = useCallback(async (retryCount = 0) => {
    setIsLoadingConversations(true);
    try {
      const response = await getConversations();
      setConversations(response.data);
      setError("");
    } catch (err: any) {
      const errorMessage = err.message || 'Unknown error';
      console.error("Error fetching conversations:", err);
      
      if (retryCount < 2 && err.originalError?.code === 'ECONNABORTED') {
        // Wait 2 seconds before retrying
        await new Promise(resolve => setTimeout(resolve, 2000));
        return fetchConversations(retryCount + 1);
      }
      
      setError(`Failed to fetch conversations: ${errorMessage}`);
    } finally {
      setIsLoadingConversations(false);
    }
  }, []);

  const fetchRooms = useCallback(async (retryCount = 0) => {
    setIsLoadingRooms(true);
    try {
      const response = await getRooms();
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

  const handleCreate = async (conversation: Omit<ConversationDTO, "id" | "createdAt" | "updatedAt" | "messages">) => {
    try {
      await createConversation(conversation);
      await fetchConversations();
      setIsPanelOpen(false);
    } catch (err: any) {
      setError(`Failed to create conversation: ${err.message || 'Unknown error'}`);
    }
  };

  const handleEdit = async (conversation: ConversationDTO) => {
    setEditingConversation(conversation);
    setIsPanelOpen(true);
  };

  const handleUpdate = async (conversation: Omit<ConversationDTO, "id" | "createdAt" | "updatedAt" | "messages">) => {
    try {
      if (!editingConversation) return;
      await updateConversation(editingConversation.id, conversation);
      await fetchConversations();
      setIsPanelOpen(false);
      setEditingConversation(null);
    } catch (err: any) {
      setError(`Failed to update conversation: ${err.message || 'Unknown error'}`);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteConversation(id);
      setConversations(conversations.filter((conversation) => conversation.id !== id));
    } catch (err: any) {
      setError(`Failed to delete conversation: ${err.message || 'Unknown error'}`);
    }
  };

  const handlePanelClose = () => {
    setIsPanelOpen(false);
    setEditingConversation(null);
  };

  useEffect(() => {
    let mounted = true;

    const loadData = async () => {
      if (mounted) {
        await fetchRooms();
        await fetchConversations();
      }
    };

    loadData();

    return () => {
      mounted = false;
    };
  }, [fetchRooms, fetchConversations]);

  if (isLoadingConversations || isLoadingRooms) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gray-900">
        <div className="text-gray-100">
          {isLoadingRooms ? "Loading rooms..." : "Loading conversations..."}
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
        <h1 className="text-2xl font-bold text-gray-100">Conversations</h1>
        <button
          onClick={() => setIsPanelOpen(true)}
          className="bg-blue-600 text-gray-100 px-4 py-2 rounded hover:bg-blue-700 transition-colors"
        >
          Create Conversation
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {conversations.map((conversation) => (
          <ConversationCard
            key={conversation.id}
            conversation={conversation}
            rooms={rooms}
            onDelete={handleDelete}
            onEdit={handleEdit}
          />
        ))}
      </div>

      <RightPanel
        isOpen={isPanelOpen}
        onClose={handlePanelClose}
        title={editingConversation ? "Edit Conversation" : "Create New Conversation"}
      >
        <ConversationForm 
          rooms={rooms} 
          onSubmit={editingConversation ? handleUpdate : handleCreate}
          initialValues={editingConversation || undefined}
        />
      </RightPanel>
    </div>
  );
}
