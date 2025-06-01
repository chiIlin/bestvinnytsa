import { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import {
  Button,
  Input,
  Badge,
} from '@/components/ui/base-ui';
import {
  Card,
  CardContent,
  CardHeader,
} from '@/components/ui/card';
import {
  Avatar,
  AvatarFallback,
  AvatarImage,
} from '@/components/ui/avatar';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Send, ArrowLeft, Search, MoreVertical } from 'lucide-react';
import axios from 'axios';

interface Message {
  id: string;
  senderId: string;
  text: string;
  timestamp: Date;
  isRead: boolean;
}

interface Chat {
  id: string;
  participantName: string;
  participantAvatar?: string;
  participantType: 'influencer' | 'brand';
  lastMessage: string;
  lastMessageTime: Date;
  unreadCount: number;
  isOnline: boolean;
  participants: string[]; // ДОДАЙТЕ ЦЕ
}

const Messages = () => {
  const { userId } = useParams();
  const [selectedChat, setSelectedChat] = useState<string | null>(null);
  const [newMessage, setNewMessage] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [chats, setChats] = useState<Chat[]>([]);
  const [messages, setMessages] = useState<Record<string, Message[]>>({});
  const [loadingChats, setLoadingChats] = useState(false);
  const [loadingMessages, setLoadingMessages] = useState(false);
  const [currentUserId, setCurrentUserId] = useState<string | null>(null);

  console.log("userId from params:", userId);

  // Завантаження чатів при монтуванні
  useEffect(() => {
    const fetchChats = async () => {
      setLoadingChats(true);
      try {
        const token = localStorage.getItem('token');
        const res = await axios.get('http://localhost:5112/api/messages/chats', {
          headers: { Authorization: `Bearer ${token}` }
        });
        console.log('Чати з бекенду:', res.data);
        const payload = JSON.parse(atob(token.split('.')[1]));
        const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
        setCurrentUserId(userId); // Зберігаємо userId у стані
        // Для кожного чату визначаємо співрозмовника
        const mapped = res.data.map((chat: any) => ({
          ...chat,
          lastMessageTime: chat.lastMessageTime ? new Date(chat.lastMessageTime) : undefined
        }));
        setChats(mapped);
      } catch (err) {
        // TODO: обробка помилок
      } finally {
        setLoadingChats(false);
      }
    };
    fetchChats();
  }, []);

  // Завантаження повідомлень для вибраного чату
  useEffect(() => {
    if (!selectedChat) return;
    const fetchMessages = async () => {
      setLoadingMessages(true);
      try {
        const token = localStorage.getItem('token');
        const res = await axios.get(`http://localhost:5112/api/messages/${selectedChat}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        // Перетворюємо рядки дат у Date
        const parsed = res.data.map((m: any) => ({
          ...m,
          timestamp: new Date(m.timestamp)
        }));
        setMessages(prev => ({ ...prev, [selectedChat]: parsed }));
      } catch (err) {
        // TODO: обробка помилок
      } finally {
        setLoadingMessages(false);
      }
    };
    fetchMessages();
  }, [selectedChat]);

  useEffect(() => {
    if (userId && chats.length > 0) {
      // Шукаємо чат з цим userId
      const chat = chats.find(chat => chat.participants.includes(userId));
      if (chat) {
        setSelectedChat(chat.id);
      } else {
        // Якщо чату нема — створюємо новий
        const createChat = async () => {
          const token = localStorage.getItem('token');
          const res = await axios.post('http://localhost:5112/api/messages/create', 
            { participantId: userId },
            { headers: { Authorization: `Bearer ${token}` } }
          );
          setChats(prev => [...prev, res.data]);
          setSelectedChat(res.data.id);
        };
        createChat();
      }
    }
  }, [userId, chats]);

  const filteredChats = chats; // без фільтрації по імені

  const selectedChatData = selectedChat ? chats.find(chat => chat.id === selectedChat) : null;
  const chatMessages = selectedChat ? messages[selectedChat] || [] : [];

  // Відправка повідомлення
  const handleSendMessage = async () => {
    if (newMessage.trim() && selectedChat) {
      try {
        const token = localStorage.getItem('token');
        const res = await axios.post(
          `http://localhost:5112/api/messages/${selectedChat}`,
          { text: newMessage },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        // Додаємо нове повідомлення до списку
        setMessages(prev => ({
          ...prev,
          [selectedChat]: [
            ...(prev[selectedChat] || []),
            { ...res.data, timestamp: new Date(res.data.timestamp) } // <-- Додаємо це!
          ]
        }));
        setNewMessage('');
      } catch (err) {
        // TODO: обробка помилок
      }
    }
  };

  const formatTime = (date: Date) => {
    return date.toLocaleTimeString('uk-UA', {
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const formatDate = (date?: Date) => {
    if (!date) return '';
    const today = new Date();
    const yesterday = new Date(today);
    yesterday.setDate(yesterday.getDate() - 1);

    if (date.toDateString() === today.toDateString()) {
      return formatTime(date);
    } else if (date.toDateString() === yesterday.toDateString()) {
      return 'Вчора';
    } else {
      return date.toLocaleDateString('uk-UA', {
        day: '2-digit',
        month: '2-digit',
      });
    }
  };

  return (
    <div className="min-h-screen">
      <section className="py-8 px-4">
        <div className="container mx-auto max-w-7xl">
          <div className="flex items-center justify-between mb-8">
            <div>
              <h1 className="text-3xl font-bold">
                <span className="gradient-text">Повідомлення</span>
              </h1>
              <p className="text-gray-600 mt-2">Спілкуйтеся з інфлюенсерами та брендами</p>
            </div>
            <Button
              variant="outline"
              asChild
              className="border-ua-blue text-ua-blue hover:bg-ua-blue hover:text-white"
            >
              <Link to="/catalog">
                <ArrowLeft className="h-4 w-4 mr-2" />
                До каталогу
              </Link>
            </Button>
          </div>

          <div className="grid lg:grid-cols-3 gap-6 h-[600px]">
            {/* Список чатів */}
            <Card className="lg:col-span-1 bg-white/80 backdrop-blur-sm border-ua-pink-light">
              <CardHeader className="pb-4">
                <div className="relative">
                  <Search className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                  <Input
                    placeholder="Пошук чатів..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="pl-10 border-ua-blue-light focus:border-ua-pink"
                  />
                </div>
              </CardHeader>
              <CardContent className="p-0">
                <ScrollArea className="h-[480px]">
                  {filteredChats.map((chat) => (
                    <div
                      key={chat.id}
                      onClick={() => setSelectedChat(chat.id)}
                      className={`p-4 border-b border-gray-100 cursor-pointer hover:bg-gray-50 transition-colors ${
                        selectedChat === chat.id ? 'bg-ua-pink-light/20 border-l-4 border-l-ua-pink' : ''
                      }`}
                    >
                      <div className="flex items-start space-x-3">
                        <div className="relative">
                          <Avatar className="w-12 h-12">
                            <AvatarImage src={chat.participantAvatar} />
                            <AvatarFallback className="bg-gradient-to-r from-ua-pink to-ua-pink-soft text-white">
                              {chat.participantName.split(' ').map(n => n[0]).join('')}
                            </AvatarFallback>
                          </Avatar>
                          {chat.isOnline && (
                            <div className="absolute -bottom-1 -right-1 w-4 h-4 bg-green-400 border-2 border-white rounded-full"></div>
                          )}
                        </div>
                        <div className="flex-1 min-w-0">
                          <div className="flex items-center justify-between">
                            <h3 className="font-medium text-gray-900 truncate">
                              {chat.participantName}
                            </h3>
                            <span className="text-xs text-gray-500">
                              {formatDate(chat.lastMessageTime)}
                            </span>
                          </div>
                          <div className="flex items-center justify-between mt-1">
                            <Badge
                              variant="outline"
                              className={`text-xs ${
                                chat.participantType === 'influencer'
                                  ? 'border-ua-pink text-ua-pink'
                                  : 'border-ua-blue text-ua-blue'
                              }`}
                            >
                              {chat.participantType === 'influencer' ? 'Інфлюенсер' : 'Бренд'}
                            </Badge>
                            {chat.unreadCount > 0 && (
                              <Badge className="bg-ua-pink text-white">
                                {chat.unreadCount}
                              </Badge>
                            )}
                          </div>
                          <p className="text-sm text-gray-600 truncate mt-1">
                            {chat.lastMessage}
                          </p>
                        </div>
                      </div>
                    </div>
                  ))}
                </ScrollArea>
              </CardContent>
            </Card>

            {/* Вікно чату */}
            <Card className="lg:col-span-2 bg-white/80 backdrop-blur-sm border-ua-pink-light">
              {selectedChatData ? (
                <>
                  {/* Заголовок чату */}
                  <CardHeader className="border-b border-gray-100">
                    <div className="flex items-center justify-between">
                      <div className="flex items-center space-x-3">
                        <div className="relative">
                          <Avatar className="w-10 h-10">
                            <AvatarImage src={selectedChatData.participantAvatar} />
                            <AvatarFallback className="bg-gradient-to-r from-ua-pink to-ua-pink-soft text-white">
                              {selectedChatData.participantName.split(' ').map(n => n[0]).join('')}
                            </AvatarFallback>
                          </Avatar>
                          {selectedChatData.isOnline && (
                            <div className="absolute -bottom-1 -right-1 w-3 h-3 bg-green-400 border-2 border-white rounded-full"></div>
                          )}
                        </div>
                        <div>
                          <h3 className="font-medium text-gray-900">
                            {selectedChatData.participantName}
                          </h3>
                          <p className="text-sm text-gray-500">
                            {selectedChatData.isOnline ? 'В мережі' : 'Не в мережі'}
                          </p>
                        </div>
                      </div>
                      <Button variant="ghost" size="icon">
                        <MoreVertical className="h-4 w-4" />
                      </Button>
                    </div>
                  </CardHeader>

                  {/* Повідомлення */}
                  <CardContent className="p-0 flex flex-col h-[480px]">
                    <ScrollArea className="flex-1 p-4">
                      <div className="space-y-4">
                        {chatMessages.map((message) => {
                          const isMine = message.senderId === currentUserId;
                          return (
                            <div
                              key={message.id}
                              className={`flex ${isMine ? 'justify-end' : 'justify-start'}`}
                            >
                              <div
                                className={`max-w-[70%] p-3 rounded-lg ${
                                  isMine
                                    ? 'bg-gradient-to-r from-ua-pink to-ua-pink-soft text-white'
                                    : 'bg-gray-100 text-gray-900'
                                }`}
                              >
                                <p className="text-sm">{message.text}</p>
                                <p
                                  className={`text-xs mt-1 ${
                                    isMine ? 'text-white/80' : 'text-gray-500'
                                  }`}
                                >
                                  {formatTime(message.timestamp)}
                                </p>
                              </div>
                            </div>
                          );
                        })}
                      </div>
                    </ScrollArea>

                    {/* Поле для введення повідомлення */}
                    <div className="border-t border-gray-100 p-4">
                      <div className="flex space-x-2">
                        <Input
                          placeholder="Введіть повідомлення..."
                          value={newMessage}
                          onChange={(e) => setNewMessage(e.target.value)}
                          onKeyDown={(e) => e.key === 'Enter' && handleSendMessage()}
                          className="flex-1 border-ua-blue-light focus:border-ua-pink"
                        />
                        <Button
                          onClick={handleSendMessage}
                          disabled={!newMessage.trim()}
                          className="bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white"
                        >
                          <Send className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>
                  </CardContent>
                </>
              ) : (
                <CardContent className="flex items-center justify-center h-full">
                  <div className="text-center">
                    <div className="w-16 h-16 bg-gradient-to-r from-ua-pink to-ua-pink-soft rounded-full flex items-center justify-center mx-auto mb-4">
                      <Send className="h-8 w-8 text-white" />
                    </div>
                    <h3 className="text-lg font-medium text-gray-900 mb-2">
                      Оберіть чат для початку спілкування
                    </h3>
                    <p className="text-gray-500">
                      Виберіть розмову зі списку, щоб почати переписку
                    </p>
                  </div>
                </CardContent>
              )}
            </Card>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Messages;