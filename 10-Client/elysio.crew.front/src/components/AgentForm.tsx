import { useState } from "react";
import type { AgentDTO } from "@/models";

interface AgentFormProps {
  onSubmit: (agent: Omit<AgentDTO, "id" | "createdAt" | "updatedAt" | "rooms" | "messages">) => Promise<void>;
}

export function AgentForm({ onSubmit }: AgentFormProps) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [model, setModel] = useState("");
  const [temperature, setTemperature] = useState(0.7);
  const [prompt, setPrompt] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await onSubmit({
      name,
      description,
      model,
      temperature,
      prompt
    });
    
    // Reset form
    setName("");
    setDescription("");
    setModel("");
    setTemperature(0.7);
    setPrompt("");
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
        <label htmlFor="prompt" className="block text-sm font-medium">
          Prompt
        </label>
        <textarea
          id="prompt"
          value={prompt}
          onChange={(e) => setPrompt(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
          required
        />
      </div>
      <div>
        <label htmlFor="model" className="block text-sm font-medium">
          Model
        </label>
        <input
          type="text"
          id="model"
          value={model}
          onChange={(e) => setModel(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
          required
        />
      </div>
      <div>
        <label htmlFor="temperature" className="block text-sm font-medium">
          Temperature
        </label>
        <input
          type="number"
          id="temperature"
          value={temperature}
          onChange={(e) => setTemperature(Number(e.target.value))}
          step="0.1"
          min="0"
          max="2"
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
          required
        />
      </div>
      <button
        type="submit"
        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
      >
        Create Agent
      </button>
    </form>
  );
}