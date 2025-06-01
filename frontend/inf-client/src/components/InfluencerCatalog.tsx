import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import {
  Button,
  Badge,
  Input,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/base-ui';
import { Search, Instagram, MessageCircle, MapPin, Users, Eye } from 'lucide-react';

const mockInfluencers = [
  {
    id: 1,
    name: 'Анна Коваленко',
    bio: 'Fashion блогер з Києва. Ділюся стилем та трендами моди.',
    city: 'Київ',
    followers: 25000,
    engagement: '4.2%',
    categories: ['Мода', 'Стиль життя'],
    instagram: '@anna_style_ua',
    avatar: 'https://images.unsplash.com/photo-1494790108755-2616b612b47c?w=150&h=150&fit=crop&crop=face'
  },
  {
    id: 2,
    name: 'Максим Петренко',
    bio: 'Travel блогер. Показую красу України та світу. він чіловий чел',
    city: 'Львів',
    followers: 18500,
    engagement: '5.8%',
    categories: ['Подорожі', 'Фотографія'],
    instagram: '@max_travels',
    avatar: 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&h=150&fit=crop&crop=face'
  },
  {
    id: 3,
    name: 'Олена Сидорова',
    bio: 'Фітнес тренер та нутриціолог. Здорове харчування і спорт.',
    city: 'Одеса',
    followers: 32000,
    engagement: '6.1%',
    categories: ['Фітнес', 'Здоров\'я'],
    instagram: '@elena_fit',
    avatar: 'https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=150&h=150&fit=crop&crop=face'
  },
  {
    id: 4,
    name: 'Дмитро Іваненко',
    bio: 'Tech блогер. Огляди гаджетів та технологічні новинки.',
    city: 'Харків',
    followers: 22000,
    engagement: '3.9%',
    categories: ['Технології', 'Гаджети'],
    instagram: '@dmitro_tech',
    avatar: 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150&h=150&fit=crop&crop=face'
  },
  {
    id: 5,
    name: 'Марія Бойко',
    bio: 'Beauty блогер. Makeup tutorials та огляди косметики.',
    city: 'Дніпро',
    followers: 41000,
    engagement: '7.2%',
    categories: ['Краса', 'Макіяж'],
    instagram: '@maria_beauty',
    avatar: 'https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=150&h=150&fit=crop&crop=face'
  },
  {
    id: 6,
    name: 'Артем Мельник',
    bio: 'Food блогер з Києва. Рецепти та огляди ресторанів.',
    city: 'Київ',
    followers: 19500,
    engagement: '4.8%',
    categories: ['Їжа', 'Ресторани'],
    instagram: '@artem_foodie',
    avatar: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&h=150&fit=crop&crop=face'
  }
];

const categories = ['Всі', 'Мода', 'Стиль життя', 'Подорожі', 'Фотографія', 'Фітнес', 'Здоров\'я', 'Технології', 'Гаджети', 'Краса', 'Макіяж', 'Їжа', 'Ресторани'];
const cities = ['Всі міста', 'Київ', 'Львів', 'Одеса', 'Харків', 'Дніпро'];

const InfluencerCatalog = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('Всі');
  const [selectedCity, setSelectedCity] = useState('Всі міста');
  const [minFollowers, setMinFollowers] = useState('');

  const filteredInfluencers = mockInfluencers.filter(influencer => {
    const matchesSearch = influencer.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         influencer.bio.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesCategory = selectedCategory === 'Всі' || influencer.categories.includes(selectedCategory);
    const matchesCity = selectedCity === 'Всі міста' || influencer.city === selectedCity;
    const matchesFollowers = !minFollowers || influencer.followers >= parseInt(minFollowers);
    
    return matchesSearch && matchesCategory && matchesCity && matchesFollowers;
  });

  return (
    <section className="py-16 px-4">
      <div className="container mx-auto">
        <div className="text-center mb-12">
          <h2 className="text-4xl font-bold mb-4">
            <span className="gradient-text">Каталог інфлюенсерів</span>
          </h2>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            Знайдіть ідеального інфлюенсера для вашого бренду серед перевірених українських креаторів
          </p>
        </div>

        {/* Filters */}
        <div className="bg-white/60 backdrop-blur-sm rounded-2xl p-6 mb-8 border border-ua-pink-light">
          <div className="grid md:grid-cols-4 gap-4">
            <div className="relative">
              <Search className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
              <Input
                placeholder="Пошук за ім'ям або біо..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10 border-ua-blue-light focus:border-ua-pink"
              />
            </div>
            
            <Select value={selectedCategory} onValueChange={setSelectedCategory}>
              <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                <SelectValue placeholder="Категорія" />
              </SelectTrigger>
              <SelectContent className="bg-white border-ua-blue-light scroll-smooth max-h-72 overflow-y-auto">
                {categories.map(category => (
                  <SelectItem key={category} value={category}>{category}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            
            <Select value={selectedCity} onValueChange={setSelectedCity}>
              <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                <SelectValue placeholder="Місто" />
              </SelectTrigger>
              <SelectContent className="bg-white border-ua-blue-light scroll-smooth max-h-72 overflow-y-auto">
                {cities.map(city => (
                  <SelectItem key={city} value={city}>{city}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            
            <Input
              type="number"
              placeholder="Мін. підписники"
              value={minFollowers}
              onChange={(e) => setMinFollowers(e.target.value)}
              className="border-ua-blue-light focus:border-ua-pink"
            />
          </div>
        </div>

        {/* Results */}
        <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredInfluencers.map(influencer => (
            <Card key={influencer.id} className="bg-white/80 backdrop-blur-sm border-ua-pink-light transition-all duration-300 hover:shadow-lg hover:-translate-y-1 will-change-transform">
              <CardHeader className="text-center">
                <div className="w-20 h-20 rounded-full mx-auto mb-4 overflow-hidden bg-gradient-to-r from-ua-pink to-ua-blue p-0.5">
                  <div className="w-full h-full rounded-full overflow-hidden bg-white">
                    <img 
                      src={influencer.avatar} 
                      alt={influencer.name}
                      className="w-full h-full object-cover"
                    />
                  </div>
                </div>
                <CardTitle className="text-xl font-semibold">{influencer.name}</CardTitle>
                <CardDescription className="text-gray-600">{influencer.bio}</CardDescription>
              </CardHeader>
              
              <CardContent className="space-y-4">
                <div className="flex items-center justify-between text-sm">
                  <div className="flex items-center text-gray-500 min-w-0">
                    <MapPin className="h-4 w-4 mr-1 flex-shrink-0" />
                    <span className="truncate">{influencer.city}</span>
                  </div>
                  <div className="flex items-center text-gray-500 min-w-0">
                    <Instagram className="h-4 w-4 mr-1 flex-shrink-0" />
                    <span className="truncate">{influencer.instagram}</span>
                  </div>
                </div>
                
                <div className="flex items-center justify-between">
                  <div className="flex items-center text-sm">
                    <Users className="h-4 w-4 mr-1 text-ua-blue flex-shrink-0" />
                    <span className="font-semibold">{influencer.followers.toLocaleString()}</span>
                  </div>
                  <div className="flex items-center text-sm">
                    <Eye className="h-4 w-4 mr-1 text-ua-pink flex-shrink-0" />
                    <span className="font-semibold">{influencer.engagement}</span>
                  </div>
                </div>
                
                <div className="flex flex-wrap gap-2">
                  {influencer.categories.map(category => (
                    <Badge 
                      key={category} 
                      variant="secondary" 
                      className="bg-ua-blue-light text-ua-blue text-xs"
                    >
                      {category}
                    </Badge>
                  ))}
                </div>
                
                <Button 
                  className="w-full bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white"
                >
                  <MessageCircle className="h-4 w-4 mr-2" />
                  Зв'язатися
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>

        {filteredInfluencers.length === 0 && (
          <div className="text-center py-12">
            <p className="text-xl text-gray-500">Інфлюенсерів за вашими критеріями не знайдено</p>
            <p className="text-gray-400 mt-2">Спробуйте змінити фільтри пошуку</p>
          </div>
        )}
      </div>
    </section>
  );
};

export default InfluencerCatalog;
