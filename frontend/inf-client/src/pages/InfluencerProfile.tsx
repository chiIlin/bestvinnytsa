import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import {
  Button,
  Input,
  Textarea,
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
  Label,
} from '@/components/ui/base-ui';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
// ДОДАЙТЕ ЦІ ІМПОРТИ:
import { Users, Camera, Instagram, Youtube, Upload, MessageCircle } from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import axios from 'axios';

const InfluencerProfile = () => {
  const { toast } = useToast();
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    phone: '',
    city: '',
    bio: '',
    categories: '',
    instagram: '',
    youtube: '',
    tiktok: '',
    telegram: '',
    // Додаємо поля для кількості підписників
    instagramFollowers: '',
    youtubeFollowers: '',
    tiktokFollowers: '',
    telegramFollowers: ''
  });

  const [avatarPreview, setAvatarPreview] = useState<string>('');
  const [loading, setLoading] = useState(true);

  const categories = [
    'Мода', 'Красота', 'Стиль життя', 'Подорожі', 'Їжа', 'Фітнес', 
    'Здоров\'я', 'Технології', 'Фотографія', 'Бізнес', 'Освіта', 'Розваги'
  ];

  const cities = [
    'Київ', 'Львів', 'Одеса', 'Харків', 'Дніпро', 'Запоріжжя', 
    'Вінниця', 'Полтава', 'Чернігів', 'Черкаси', 'Інше'
  ];

  // Функція для форматування числа підписників
  const formatFollowersNumber = (value: string) => {
    const numValue = value.replace(/\D/g, ''); // Видаляємо все окрім цифр
    if (numValue === '') return '';
    return Number(numValue).toLocaleString('uk-UA'); // Форматуємо з пробілами
  };

  // Функція для обробки введення підписників
  const handleFollowersChange = (field: string, value: string) => {
    const formattedValue = formatFollowersNumber(value);
    setFormData(prev => ({ ...prev, [field]: formattedValue }));
  };

  // Функція для конвертації форматованого числа назад в число
  const parseFollowersNumber = (value: string) => {
    if (!value) return null;
    return parseInt(value.replace(/\s/g, ''), 10) || null;
  };

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          toast({
            title: "Помилка",
            description: "Необхідно увійти в систему",
            variant: "destructive"
          });
          return;
        }

        const response = await axios.get('http://localhost:5112/api/profile/influencer', {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });

        const profile = response.data;
        setFormData({
          name: profile.fullName || '',
          email: profile.email || '',
          phone: profile.phoneNumber || '',
          city: profile.city || '',
          bio: profile.biography || '',
          categories: profile.contentCategories || '',
          instagram: profile.instagramHandle || '',
          youtube: profile.youtubeHandle || '',
          tiktok: profile.tiktokHandle || '',
          telegram: profile.telegramHandle || '',
          // ОНОВЛЮЄМО: завантажуємо підписників з бекенду
          instagramFollowers: profile.instagramFollowers ? profile.instagramFollowers.toLocaleString('uk-UA') : '',
          youtubeFollowers: profile.youtubeFollowers ? profile.youtubeFollowers.toLocaleString('uk-UA') : '',
          tiktokFollowers: profile.tiktokFollowers ? profile.tiktokFollowers.toLocaleString('uk-UA') : '',
          telegramFollowers: profile.telegramFollowers ? profile.telegramFollowers.toLocaleString('uk-UA') : ''
        });
        
        // ДОДАЙТЕ: завантаження фото профілю
        if (profile.photoUrl) {
          setAvatarPreview(`http://localhost:5112${profile.photoUrl}`);
        }
      } catch (error) {
        console.error('Error fetching profile:', error);
        toast({
          title: "Помилка",
          description: "Не вдалося завантажити профіль",
          variant: "destructive"
        });
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, [toast]);

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  // ВИДАЛІТЬ ЦЮ ФУНКЦІЮ (стара версія):
  // const handleAvatarChange = (event: React.ChangeEvent<HTMLInputElement>) => {
  //   const file = event.target.files?.[0];
  //   if (file) {
  //     const reader = new FileReader();
  //     reader.onload = (e) => {
  //       setAvatarPreview(e.target?.result as string);
  //     };
  //     reader.readAsDataURL(file);
  //   }
  // };

  // ДОДАЙТЕ цю функцію для завантаження фото на сервер
  const handleUploadPhoto = async (file: File) => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        toast({
          title: "Помилка",
          description: "Необхідно увійти в систему",
          variant: "destructive"
        });
        return;
      }

      const formData = new FormData();
      formData.append('file', file);

      const response = await axios.post('http://localhost:5112/api/profile/influencer/photo', formData, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        }
      });

      setAvatarPreview(`http://localhost:5112${response.data.photoUrl}`);
      
      toast({
        title: "Фото завантажено",
        description: "Ваше фото профілю успішно оновлено.",
      });
    } catch (error) {
      console.error('Error uploading photo:', error);
      toast({
        title: "Помилка",
        description: "Не вдалося завантажити фото",
        variant: "destructive"
      });
    }
  };

  // ЗАЛИШІТЬ ТІЛЬКИ ЦЮ ВЕРСІЮ handleAvatarChange (оновлену):
  const handleAvatarChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      // Попередній перегляд
      const reader = new FileReader();
      reader.onload = (e) => {
        setAvatarPreview(e.target?.result as string);
      };
      reader.readAsDataURL(file);
      
      // Завантаження на сервер
      await handleUploadPhoto(file);
    }
  };

  const handleSave = async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        toast({
          title: "Помилка",
          description: "Необхідно увійти в систему",
          variant: "destructive"
        });
        return;
      }

      await axios.put('http://localhost:5112/api/profile/influencer', {
        fullName: formData.name,
        phoneNumber: formData.phone,
        city: formData.city,
        biography: formData.bio,
        contentCategories: formData.categories,
        instagramHandle: formData.instagram,
        youtubeHandle: formData.youtube,
        tiktokHandle: formData.tiktok,
        telegramHandle: formData.telegram,
        // ДОДАЄМО: відправляємо підписників на бекенд
        instagramFollowers: parseFollowersNumber(formData.instagramFollowers),
        youtubeFollowers: parseFollowersNumber(formData.youtubeFollowers),
        tiktokFollowers: parseFollowersNumber(formData.tiktokFollowers),
        telegramFollowers: parseFollowersNumber(formData.telegramFollowers)
      }, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      toast({
        title: "Профіль оновлено",
        description: "Ваші зміни успішно збережено.",
      });
    } catch (error) {
      console.error('Error updating profile:', error);
      toast({
        title: "Помилка",
        description: "Не вдалося оновити профіль",
        variant: "destructive"
      });
    }
  };

  if (loading) {
    return (
      <section className="py-16 px-4">
        <div className="container mx-auto max-w-4xl text-center">
          <p>Завантаження профілю...</p>
        </div>
      </section>
    );
  }

  return (
    <section className="py-16 px-4">
      <div className="container mx-auto max-w-4xl">
        <div className="text-center mb-12">
          <div className="w-16 h-16 bg-gradient-to-r from-ua-pink to-ua-pink-soft rounded-2xl flex items-center justify-center mx-auto mb-6">
            <Users className="h-8 w-8 text-white" />
          </div>
          <h1 className="text-4xl font-bold mb-4">
            <span className="gradient-text">Профіль інфлюенсера</span>
          </h1>
          <p className="text-xl text-gray-600">
            Керуйте своїм профілем та налаштуваннями
          </p>
        </div>
        <div className="grid lg:grid-cols-3 gap-8">
          {/* Avatar Section */}
          <div className="lg:col-span-1">
            <Card className="bg-white/80 backdrop-blur-sm border-ua-pink-light">
              <CardHeader>
                <CardTitle className="text-center">Фото профілю</CardTitle>
              </CardHeader>
              <CardContent className="text-center space-y-4">
                <Avatar className="w-32 h-32 mx-auto">
                  <AvatarImage src={avatarPreview} />
                  <AvatarFallback className="text-2xl bg-gradient-to-r from-ua-pink to-ua-pink-soft text-white">
                    {formData.name ? formData.name.split(' ').map(n => n[0]).join('') : 'U'}
                  </AvatarFallback>
                </Avatar>
                <div>
                  <input
                    type="file"
                    accept="image/*"
                    onChange={handleAvatarChange}
                    className="hidden"
                    id="avatar-upload"
                  />
                  <Label htmlFor="avatar-upload">
                    <Button
                      type="button"
                      variant="outline"
                      className="cursor-pointer border-ua-pink text-ua-pink hover:bg-ua-pink hover:text-white"
                      asChild
                    >
                      <span>
                        <Upload className="h-4 w-4 mr-2" />
                        Завантажити фото
                      </span>
                    </Button>
                  </Label>
                </div>
              </CardContent>
            </Card>
          </div>
          {/* Profile Information */}
          <div className="lg:col-span-2">
            <Card className="bg-white/80 backdrop-blur-sm border-ua-pink-light">
              <CardHeader>
                <CardTitle>Особиста інформація</CardTitle>
                <CardDescription>
                  Оновіть дані вашого профілю
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <Label htmlFor="name">Повне ім'я *</Label>
                    <Input 
                      id="name"
                      value={formData.name}
                      onChange={(e) => handleInputChange('name', e.target.value)}
                      className="border-ua-blue-light focus:border-ua-pink"
                    />
                  </div>
                  <div>
                    <Label htmlFor="email">Email *</Label>
                    <Input 
                      id="email"
                      type="email"
                      value={formData.email}
                      disabled
                      className="border-ua-blue-light focus:border-ua-pink bg-gray-50"
                    />
                  </div>
                </div>
                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <Label htmlFor="phone">Телефон</Label>
                    <Input 
                      id="phone"
                      value={formData.phone}
                      onChange={(e) => handleInputChange('phone', e.target.value)}
                      className="border-ua-blue-light focus:border-ua-pink"
                    />
                  </div>
                  <div>
                    <Label htmlFor="city">Місто *</Label>
                    <Select value={formData.city} onValueChange={(value) => handleInputChange('city', value)}>
                      <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent className="bg-white border-ua-blue-light">
                        {cities.map(city => (
                          <SelectItem key={city} value={city}>{city}</SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                </div>
                <div>
                  <Label htmlFor="bio">Біографія *</Label>
                  <Textarea 
                    id="bio"
                    value={formData.bio}
                    onChange={(e) => handleInputChange('bio', e.target.value)}
                    className="border-ua-blue-light focus:border-ua-pink min-h-[100px]"
                  />
                </div>
                <div>
                  <Label htmlFor="categories">Категорії контенту *</Label>
                  <Select value={formData.categories} onValueChange={(value) => handleInputChange('categories', value)}>
                    <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent className="bg-white border-ua-blue-light">
                      {categories.map(category => (
                        <SelectItem key={category} value={category}>{category}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-4">
                  <h3 className="text-lg font-semibold">Соціальні мережі та аудиторія</h3>
                  
                  {/* Instagram */}
                  <div className="border border-ua-pink-light rounded-lg p-4 space-y-3">
                    <div className="flex items-center gap-2 mb-3">
                      <Instagram className="h-5 w-5 text-pink-500" />
                      <span className="font-medium">Instagram</span>
                    </div>
                    <div className="grid md:grid-cols-2 gap-4">
                      <div>
                        <Label htmlFor="instagram">Нікнейм *</Label>
                        <Input 
                          id="instagram"
                          value={formData.instagram}
                          onChange={(e) => handleInputChange('instagram', e.target.value)}
                          placeholder="@your_instagram"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                      <div>
                        <Label htmlFor="instagram-followers">Кількість підписників</Label>
                        <Input 
                          id="instagram-followers"
                          value={formData.instagramFollowers}
                          onChange={(e) => handleFollowersChange('instagramFollowers', e.target.value)}
                          placeholder="10 000"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                    </div>
                  </div>

                  {/* YouTube */}
                  <div className="border border-red-200 rounded-lg p-4 space-y-3">
                    <div className="flex items-center gap-2 mb-3">
                      <Youtube className="h-5 w-5 text-red-500" />
                      <span className="font-medium">YouTube</span>
                    </div>
                    <div className="grid md:grid-cols-2 gap-4">
                      <div>
                        <Label htmlFor="youtube">Нікнейм</Label>
                        <Input 
                          id="youtube"
                          value={formData.youtube}
                          onChange={(e) => handleInputChange('youtube', e.target.value)}
                          placeholder="@your_youtube"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                      <div>
                        <Label htmlFor="youtube-followers">Кількість підписників</Label>
                        <Input 
                          id="youtube-followers"
                          value={formData.youtubeFollowers}
                          onChange={(e) => handleFollowersChange('youtubeFollowers', e.target.value)}
                          placeholder="5 000"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                    </div>
                  </div>

                  {/* TikTok */}
                  <div className="border border-gray-300 rounded-lg p-4 space-y-3">
                    <div className="flex items-center gap-2 mb-3">
                      <Camera className="h-5 w-5 text-gray-700" />
                      <span className="font-medium">TikTok</span>
                    </div>
                    <div className="grid md:grid-cols-2 gap-4">
                      <div>
                        <Label htmlFor="tiktok">Нікнейм</Label>
                        <Input 
                          id="tiktok"
                          value={formData.tiktok}
                          onChange={(e) => handleInputChange('tiktok', e.target.value)}
                          placeholder="@your_tiktok"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                      <div>
                        <Label htmlFor="tiktok-followers">Кількість підписників</Label>
                        <Input 
                          id="tiktok-followers"
                          value={formData.tiktokFollowers}
                          onChange={(e) => handleFollowersChange('tiktokFollowers', e.target.value)}
                          placeholder="15 000"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                    </div>
                  </div>

                  {/* Telegram */}
                  <div className="border border-blue-200 rounded-lg p-4 space-y-3">
                    <div className="flex items-center gap-2 mb-3">
                      <MessageCircle className="h-5 w-5 text-blue-500" />
                      <span className="font-medium">Telegram канал</span>
                    </div>
                    <div className="grid md:grid-cols-2 gap-4">
                      <div>
                        <Label htmlFor="telegram">Нікнейм каналу</Label>
                        <Input 
                          id="telegram"
                          value={formData.telegram}
                          onChange={(e) => handleInputChange('telegram', e.target.value)}
                          placeholder="@your_telegram"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                      <div>
                        <Label htmlFor="telegram-followers">Кількість підписників</Label>
                        <Input 
                          id="telegram-followers"
                          value={formData.telegramFollowers}
                          onChange={(e) => handleFollowersChange('telegramFollowers', e.target.value)}
                          placeholder="2 000"
                          className="border-ua-blue-light focus:border-ua-pink"
                        />
                      </div>
                    </div>
                  </div>
                </div>

                <div className="flex gap-4 pt-6">
                  <Button 
                    onClick={handleSave}
                    className="flex-1 bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white py-3 text-lg font-semibold"
                  >
                    <Camera className="h-5 w-5 mr-2" />
                    Зберегти зміни
                  </Button>
                  <Button 
                    variant="outline"
                    asChild
                    className="border-ua-blue text-ua-blue hover:bg-ua-blue hover:text-white"
                  >
                    <Link to="/catalog">
                      Переглянути каталог
                    </Link>
                  </Button>
                </div>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </section>
  );
};

export default InfluencerProfile;
