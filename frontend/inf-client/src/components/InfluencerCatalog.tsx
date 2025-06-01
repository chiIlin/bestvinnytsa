import { useEffect, useState } from 'react';
import axios from 'axios';
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
import { Link } from 'react-router-dom';

const categories = ['Всі', 'Мода', 'Стиль життя', 'Подорожі', 'Фотографія', 'Фітнес', 'Здоров\'я', 'Технології', 'Гаджети', 'Краса', 'Макіяж', 'Їжа', 'Ресторани'];
const cities = ['Всі міста', 'Київ', 'Львів', 'Одеса', 'Харків', 'Дніпро'];

const InfluencerCatalog = () => {
  const [influencers, setInfluencers] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('Всі');
  const [selectedCity, setSelectedCity] = useState('Всі міста');
  const [minFollowers, setMinFollowers] = useState('');

  useEffect(() => {
    const fetchInfluencers = async () => {
      setLoading(true);
      try {
        const res = await axios.get('http://localhost:5112/api/profile/influencers');
        console.log('Дані з беку:', res.data); // Додаємо логування
        const mapped = res.data.map((inf: any) => ({
          id: inf.id,
          name: inf.fullName,
          bio: inf.biography,
          city: inf.city,
          categories: (inf.contentCategories || '').split(',').map((c: string) => c.trim()).filter(Boolean),
          instagram: inf.instagramHandle,
          followers: inf.instagramFollowers || 0,
          engagement: '',
          avatar: inf.photoUrl || '/default-avatar.png'
        }));
        setInfluencers(mapped);
      } catch (err) {
        setInfluencers([]);
        console.error('Помилка при отриманні інфлюенсерів:', err); // Додаємо логування помилки
      } finally {
        setLoading(false);
      }
    };
    fetchInfluencers();
  }, []);

  const filteredInfluencers = influencers.filter(influencer => {
    const matchesSearch =
      influencer.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      influencer.bio?.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesCategory =
      selectedCategory === 'Всі' || influencer.categories.includes(selectedCategory);
    const matchesCity =
      selectedCity === 'Всі міста' || influencer.city === selectedCity;
    const matchesFollowers =
      !minFollowers || influencer.followers >= parseInt(minFollowers);

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
                  asChild
                  className="w-full bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white"
                >
                  <Link to={`/messages/${influencer.id}`}>
                    <MessageCircle className="h-4 w-4 mr-2" />
                    Зв'язатися
                  </Link>
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
