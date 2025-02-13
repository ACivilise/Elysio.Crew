import type { RoomDTO } from "../interfaces/RoomDTO";
import type { MessageDTO } from "../interfaces/MessageDTO";

export type AgentDTO = {
  id: string;
  createdAt: string;
  updatedAt: string;
  name: string;
  prompt: string;
  temperature: number;
  model: string;
  description?: string;
  rooms: RoomDTO[];
  messages: MessageDTO[];
};
