import type { AgentDTO } from "../interfaces/AgentDTO";
import type { ConversationDTO } from "../interfaces/ConversationDTO";

export type RoomDTO = {
  id: string;
  createdAt: string;
  updatedAt: string;
  name: string;
  description?: string;
  agents: AgentDTO[];
  conversations: ConversationDTO[];
};
