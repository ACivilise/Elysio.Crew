import { useState, useEffect } from 'react';
import RightPanel from '@/components/RightPanel';
import { type ConversationDTO } from '@/models';
import { RolesEnum } from '@/models/enums/RolesEnum';
import { startConversationStream } from '@/services/api';

interface Message {
  content: string;
  role: RolesEnum;
  agentName?: string;
}

interface AgentConversationPanelProps {
  isOpen: boolean;
  onClose: () => void;
  conversation: ConversationDTO;
}

export function AgentConversationPanel({ isOpen, onClose, conversation }: AgentConversationPanelProps) {
  const [messages, setMessages] = useState<Message[]>([]);
  const [initialMessage, setInitialMessage] = useState('');
  const [isStreaming, setIsStreaming] = useState(false);

  const getRoleStyles = (role: RolesEnum) => {
    switch (role) {
      case RolesEnum.User:
        return 'bg-blue-900 ml-auto';
      case RolesEnum.Error:
        return 'bg-red-900';
      case RolesEnum.Copywriter:
        return 'bg-green-900';
      case RolesEnum.ArtDirector:
        return 'bg-purple-900';
      default:
        return 'bg-gray-800';
    }
  };

  const getRoleLabel = (role: RolesEnum) => {
    switch (role) {
      case RolesEnum.User:
        return 'You';
      case RolesEnum.Copywriter:
        return 'Copywriter';
      case RolesEnum.ArtDirector:
        return 'Art Director';
      case RolesEnum.Error:
        return 'Error';
      default:
        return role;
    }
  };

  const startConversation = async () => {
    if (!initialMessage.trim()) return;
    
    setIsStreaming(true);
    setMessages([{ content: initialMessage, role: RolesEnum.User }]);

    try {
      const response = await startConversationStream(conversation.id, initialMessage);
      const reader = response.body?.getReader();
      if (!reader) return;

      while (true) {
        const { done, value } = await reader.read();
        if (done) break;

        // Parse the SSE data
        const text = new TextDecoder().decode(value);
        const lines = text.split('\n');
        
        for (const line of lines) {
          if (line.startsWith('data: ')) {
            try {
              const jsonData = JSON.parse(line.slice(6));
              console.log('Message:', line);
              const {content, role, agentName} = jsonData;
              setMessages(prev => [...prev, { 
                content, 
                role: (role as RolesEnum) || jsonData.role,
                agentName: agentName
              }]);
            } catch (error) {
              console.error('Error parsing message:', error);
            }
          }
        }
      }
    } catch (error) {
      console.error('Error streaming conversation:', error);
      setMessages(prev => [...prev, { 
        content: 'An error occurred while processing the conversation.', 
        role: RolesEnum.Error 
      }]);
    } finally {
      setIsStreaming(false);
    }
  };

  return (
    <RightPanel isOpen={isOpen} onClose={onClose} title={`Chat - ${conversation.name}`}>
      <div className="flex flex-col h-full">
        <div className="flex-1 overflow-y-auto space-y-4 mb-4">
          {messages.map((message, index) => (
            <div key={index} className="flex flex-col gap-1">
              <div className="text-sm text-gray-400 ml-2">
                {getRoleLabel(message.role)}
                {message.agentName && ` - ${message.agentName}`}
              </div>
              <div className={`p-3 rounded-lg ${getRoleStyles(message.role)} max-w-[80%]`}>
                <p className="text-gray-100">{message.content}</p>
              </div>
            </div>
          ))}
        </div>
        
        {messages.length === 0 && (
          <div className="flex-1">
            <textarea
              className="w-full h-32 p-3 bg-gray-800 text-gray-100 rounded-lg resize-none"
              placeholder="Enter your message to start the conversation..."
              value={initialMessage}
              onChange={(e) => setInitialMessage(e.target.value)}
              disabled={isStreaming}
            />
            <button
              className="mt-4 w-full bg-blue-600 text-gray-100 px-4 py-2 rounded hover:bg-blue-700 transition-colors disabled:opacity-50"
              onClick={startConversation}
              disabled={isStreaming || !initialMessage.trim()}
            >
              {isStreaming ? 'Streaming...' : 'Start Conversation'}
            </button>
          </div>
        )}
      </div>
    </RightPanel>
  );
}