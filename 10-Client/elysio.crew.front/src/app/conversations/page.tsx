"use client";

import { useEffect, useState } from "react";
import { getConversations, deleteConversation, createConversation, getRooms } from "@/services/api";
import type { ConversationDTO, RoomDTO } from "@/models";
import { ConversationForm } from "../../components/ConversationForm";
import RightPanel from "@/components/RightPanel";

export default function ConversationsPage() {
  const [conversations, setConversations] = useState<ConversationDTO[]>([]);
  const [rooms, setRooms] = useState<RoomDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [isPanelOpen, setIsPanelOpen] = useState(false);

  const fetchConversations = async () => {
    try {
      const response = await getConversations();
      setConversations(response.data);
    } catch (err) {
      setError("Failed to fetch conversations");
    }
  };

  const fetchRooms = async () => {
    try {
      const response = await getRooms();
      setRooms(response.data);
    } catch (err) {
      setError("Failed to fetch rooms");
    }
  };

  const handleCreate = async (conversation: Omit<ConversationDTO, "id" | "createdAt" | "updatedAt" | "messages">) => {
    try {
      await createConversation(conversation);
      fetchConversations();
      setIsPanelOpen(false);
    } catch (err) {
      setError("Failed to create conversation");
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteConversation(id);
      setConversations(conversations.filter((conversation) => conversation.id !== id));
    } catch (err) {
      setError("Failed to delete conversation");
    }
  };

  useEffect(() => {
    Promise.all([fetchConversations(), fetchRooms()]).finally(() => setIsLoading(false));
  }, []);

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Conversations</h1>
        <button
          onClick={() => setIsPanelOpen(true)}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          Create Conversation
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {conversations.map((conversation) => (
          <div key={conversation.id} className="border rounded-lg p-4 shadow">
            <h2 className="text-xl font-semibold">{conversation.name}</h2>
            <p className="text-gray-600 mt-2">
              Room: {rooms.find(r => r.id === conversation.roomId)?.name || conversation.roomId}
            </p>
            <p className="text-sm text-gray-500">
              Created: {new Date(conversation.createdAt).toLocaleDateString()}
            </p>
            <div className="mt-4 flex justify-end gap-2">
              <button
                onClick={() => handleDelete(conversation.id)}
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
        title="Create New Conversation"
      >
        <ConversationForm rooms={rooms} onSubmit={handleCreate} />
      </RightPanel>
    </div>
  );
}
