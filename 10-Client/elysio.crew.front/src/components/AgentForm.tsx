import { useState, useEffect } from "react";
import type { AgentDTO } from "@/models";

interface AgentFormProps {
  onSubmit: (agent: Omit<AgentDTO,  "createdAt" | "updatedAt" | "rooms" | "messages">) => Promise<void>;
  initialValues?: AgentDTO;
}

export function AgentForm({ onSubmit, initialValues }: AgentFormProps) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [model, setModel] = useState("");
  const [temperature, setTemperature] = useState(0.7);
  const [prompt, setPrompt] = useState("");

  useEffect(() => {
    if (initialValues) {
      setName(initialValues.name);
      setDescription(initialValues.description || "");
      setModel(initialValues.model);
      setTemperature(initialValues.temperature);
      setPrompt(initialValues.prompt);
    }
  }, [initialValues]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await onSubmit({
      id: initialValues?.id ?? "",
      name,
      description,
      model,
      temperature,
      prompt
    });
    
    if (!initialValues) {
      setName("");
      setDescription("");
      setModel("");
      setTemperature(0.7);
      setPrompt("");
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
        <label htmlFor="prompt" className="block text-sm font-medium text-gray-300">
          Prompt
        </label>
        <textarea
          id="prompt"
          value={prompt}
          onChange={(e) => setPrompt(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-600 bg-gray-800 text-gray-100 px-3 py-2 focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
          required
        />
      </div>
      <div>
        <label htmlFor="model" className="block text-sm font-medium text-gray-300">
          Model
        </label>
        <input
          type="text"
          id="model"
          value={model}
          onChange={(e) => setModel(e.target.value)}
          className="mt-1 block w-full rounded-md border border-gray-600 bg-gray-800 text-gray-100 px-3 py-2 focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
          required
        />
      </div>
      <div>
        <label htmlFor="temperature" className="block text-sm font-medium text-gray-300">
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
          className="mt-1 block w-full rounded-md border border-gray-600 bg-gray-800 text-gray-100 px-3 py-2 focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
          required
        />
      </div>
      <button
        type="submit"
        className="bg-blue-600 text-gray-100 px-4 py-2 rounded hover:bg-blue-700 transition-colors"
      >
        Create Agent
      </button>
    </form>
  );
}