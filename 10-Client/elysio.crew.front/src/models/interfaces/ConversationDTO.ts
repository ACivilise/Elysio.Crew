import type { RoomDTO } from "../interfaces/RoomDTO";
import type { MessageDTO } from "../interfaces/MessageDTO";

export type ConversationDTO = {
  id: string;
  createdAt: string;
  updatedAt: string;
  name: string;
  roomId: string;
  room?: RoomDTO;
  messages: MessageDTO[];
};
