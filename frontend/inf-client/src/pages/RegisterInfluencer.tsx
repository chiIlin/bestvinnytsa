import { useState } from 'react';
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
import { Users, Camera, Instagram, Youtube } from 'lucide-react';
import axios from 'axios';

const RegisterInfluencer = () => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    phone: '',
    city: '',
    bio: '',
    categories: '',
    instagram: '',
    youtube: '',
    tiktok: '',
    telegram: ''
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const categories = [
    'Мода', 'Красота', 'Стиль життя', 'Подорожі', 'Їжа', 'Фітнес', 
    'Здоров\'я', 'Технології', 'Фотографія', 'Бізнес', 'Освіта', 'Розваги'
  ];

  const cities = [
    'Київ', 'Львів', 'Одеса', 'Харків', 'Дніпро', 'Запоріжжя', 
    'Вінниця', 'Полтава', 'Чернігів', 'Черкаси', 'Інше'
  ];

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    try {
      await axios.post('http://localhost:5112/api/auth/register_person', {
        email: formData.email,
        password: formData.password,
        fullName: formData.name,
        phoneNumber: formData.phone,
        city: formData.city,
        biography: formData.bio,
        contentCategories: formData.categories,
        instagramHandle: formData.instagram,
        youtubeHandle: formData.youtube,
        tiktokHandle: formData.tiktok,
        telegramHandle: formData.telegram
      });
      setSuccess('Реєстрація успішна! Тепер ви можете увійти.');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Помилка реєстрації');
    }
  };

  return (
    <div className="min-h-screen">
      <section className="py-16 px-4">
        <div className="container mx-auto max-w-2xl">
          <div className="text-center mb-12">
            <div className="w-16 h-16 bg-gradient-to-r from-ua-pink to-ua-pink-soft rounded-2xl flex items-center justify-center mx-auto mb-6">
              <Users className="h-8 w-8 text-white" />
            </div>
            <h1 className="text-4xl font-bold mb-4">
              <span className="gradient-text">Реєстрація інфлюенсера</span>
            </h1>
            <p className="text-xl text-gray-600">
              Приєднуйтесь до спільноти українських креаторів
            </p>
          </div>

          <Card className="bg-white/80 backdrop-blur-sm border-ua-pink-light">
            <CardHeader>
              <CardTitle>Розкажіть про себе</CardTitle>
              <CardDescription>
                Заповніть інформацію про ваш профіль та аудиторію
              </CardDescription>
            </CardHeader>
            
            <CardContent className="space-y-6">
              <form onSubmit={handleRegister}>
                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <Label htmlFor="name">Повне ім'я *</Label>
                    <Input 
                      id="name"
                      placeholder="Ваше ім'я та прізвище"
                      className="border-ua-blue-light focus:border-ua-pink"
                      value={formData.name}
                      onChange={e => setFormData({ ...formData, name: e.target.value })}
                    />
                  </div>
                  <div>
                    <Label htmlFor="email">Email *</Label>
                    <Input 
                      id="email"
                      type="email"
                      placeholder="your@email.com"
                      className="border-ua-blue-light focus:border-ua-pink"
                      value={formData.email}
                      onChange={e => setFormData({ ...formData, email: e.target.value })}
                    />
                  </div>
                  <div>
                    <Label htmlFor="password">Пароль *</Label>
                    <Input 
                      id="password"
                      type="password"
                      placeholder="Введіть пароль"
                      className="border-ua-blue-light focus:border-ua-pink"
                      value={formData.password}
                      onChange={e => setFormData({ ...formData, password: e.target.value })}
                    />
                  </div>
                </div>

                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <Label htmlFor="phone">Телефон</Label>
                    <Input 
                      id="phone"
                      placeholder="+380 (50) 123-45-67"
                      className="border-ua-blue-light focus:border-ua-pink"
                      value={formData.phone}
                      onChange={e => setFormData({ ...formData, phone: e.target.value })}
                    />
                  </div>
                  <div>
                    <Label htmlFor="city">Місто *</Label>
                    <Select value={formData.city} onValueChange={(value) => setFormData({ ...formData, city: value })}>
                      <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                        <SelectValue placeholder="Оберіть місто" />
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
                    placeholder="Розкажіть про себе, ваш стиль та те, що робить ваш контент унікальним..."
                    className="border-ua-blue-light focus:border-ua-pink min-h-[100px]"
                    value={formData.bio}
                    onChange={e => setFormData({ ...formData, bio: e.target.value })}
                  />
                </div>

                <div>
                  <Label htmlFor="categories">Категорії контенту *</Label>
                  <Select value={formData.categories} onValueChange={(value) => setFormData({ ...formData, categories: value })}>
                    <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                      <SelectValue placeholder="Оберіть основні категорії" />
                    </SelectTrigger>
                    <SelectContent className="bg-white border-ua-blue-light scroll-smooth max-h-72 overflow-y-auto">
                      {categories.map(category => (
                        <SelectItem key={category} value={category}>{category}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>

                <div className="space-y-4">
                  <h3 className="text-lg font-semibold">Соціальні мережі</h3>
                  
                  <div className="grid md:grid-cols-2 gap-4">
                    <div>
                      <Label htmlFor="instagram">Instagram *</Label>
                      <div className="relative">
                        <Instagram className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                        <Input 
                          id="instagram"
                          placeholder="@your_instagram"
                          className="pl-10 border-ua-blue-light focus:border-ua-pink"
                          value={formData.instagram}
                          onChange={e => setFormData({ ...formData, instagram: e.target.value })}
                        />
                      </div>
                    </div>
                    <div>
                      <Label htmlFor="youtube">YouTube</Label>
                      <div className="relative">
                        <Youtube className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                        <Input 
                          id="youtube"
                          placeholder="@your_channel"
                          className="pl-10 border-ua-blue-light focus:border-ua-pink"
                          value={formData.youtube}
                          onChange={e => setFormData({ ...formData, youtube: e.target.value })}
                        />
                      </div>
                    </div>
                  </div>

                  <div className="grid md:grid-cols-2 gap-4">
                    <div>
                      <Label htmlFor="tiktok">TikTok</Label>
                      <Input 
                        id="tiktok"
                        placeholder="@your_tiktok"
                        className="border-ua-blue-light focus:border-ua-pink"
                        value={formData.tiktok}
                        onChange={e => setFormData({ ...formData, tiktok: e.target.value })}
                      />
                    </div>
                    <div>
                      <Label htmlFor="telegram">Telegram</Label>
                      <Input 
                        id="telegram"
                        placeholder="@your_telegram"
                        className="border-ua-blue-light focus:border-ua-pink"
                        value={formData.telegram}
                        onChange={e => setFormData({ ...formData, telegram: e.target.value })}
                      />
                    </div>
                  </div>
                </div>

                {error && <div className="text-red-500">{error}</div>}
                {success && <div className="text-green-600">{success}</div>}

                <div className="pt-6">
                  <Button
                    type="submit"
                    className="w-full bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white py-3 text-lg font-semibold"
                  >
                    <Camera className="h-5 w-5 mr-2" />
                    Створити профіль
                  </Button>
                  
                  <p className="text-sm text-gray-500 text-center mt-4">
                    Вже маєте акаунт? {' '}
                    <Link to="/login" className="text-ua-pink hover:text-ua-pink-soft font-medium">
                      Увійдіть тут
                    </Link>
                  </p>
                </div>
              </form>
            </CardContent>
          </Card>
        </div>
      </section>
    </div>
  );
};

export default RegisterInfluencer;
