import type { AgentDTO } from "../interfaces/AgentDTO";
import type { ConversationDTO } from "../interfaces/ConversationDTO";
import { RolesEnum } from "../enums/RolesEnum";

export type MessageDTO = {
  id: string;
  createdAt: string;
  updatedAt: string;
  role: RolesEnum;
  content: string;
  agentId?: string;
  agent?: AgentDTO;
  conversationId: string;
  conversation?: ConversationDTO;
};
