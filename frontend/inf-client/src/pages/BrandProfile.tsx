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
import { Building2, Globe, Users, Upload } from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import axios from 'axios';

const BrandProfile = () => {
  const { toast } = useToast();
  const [formData, setFormData] = useState({
    company: '',
    contactName: '',
    email: '',
    phone: '',
    website: '',
    industry: '',
    companySize: '',
    description: '',
    budget: '',
    targetAudience: ''
  });

  const [logoPreview, setLogoPreview] = useState<string>('');
  const [loading, setLoading] = useState(true);

  const industries = [
    'Мода та одяг', 'Краса та косметика', 'Їжа та напої', 'Технології', 
    'Фітнес та здоров\'я', 'Подорожі', 'Освіта', 'Фінанси', 'Нерухомість', 
    'Автомобілі', 'Розваги', 'Інше'
  ];

  const companySizes = [
    'Стартап (1-10 співробітників)',
    'Мала компанія (11-50 співробітників)', 
    'Середня компанія (51-200 співробітників)',
    'Велика компанія (200+ співробітників)'
  ];

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

        const response = await axios.get('http://localhost:5112/api/profile/company', {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });

        const profile = response.data;
        setFormData({
          company: profile.companyName || '',
          contactName: profile.contactPerson || '',
          email: profile.email || '',
          phone: profile.companyPhone || '',
          website: profile.website || '',
          industry: profile.industry || '',
          companySize: profile.companySize || '',
          description: profile.companyDescription || '',
          budget: profile.budgetRange || '',
          targetAudience: profile.targetAudience || ''
        });
        
        // ДОДАЙТЕ: завантаження логотипу
        if (profile.photoUrl) {
          setLogoPreview(`http://localhost:5112${profile.photoUrl}`);
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

  // ДОДАЙТЕ функцію завантаження логотипу
  const handleUploadLogo = async (file: File) => {
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

      const response = await axios.post('http://localhost:5112/api/profile/company/photo', formData, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        }
      });

      setLogoPreview(`http://localhost:5112${response.data.photoUrl}`);
      
      toast({
        title: "Логотип завантажено",
        description: "Логотип компанії успішно оновлено.",
      });
    } catch (error) {
      console.error('Error uploading logo:', error);
      toast({
        title: "Помилка",
        description: "Не вдалося завантажити логотип",
        variant: "destructive"
      });
    }
  };

  // ОНОВІТЬ handleLogoChange
  const handleLogoChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      // Попередній перегляд
      const reader = new FileReader();
      reader.onload = (e) => {
        setLogoPreview(e.target?.result as string);
      };
      reader.readAsDataURL(file);
      
      // Завантаження на сервер
      await handleUploadLogo(file);
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

      await axios.put('http://localhost:5112/api/profile/company', {
        companyName: formData.company,
        contactPerson: formData.contactName,
        companyPhone: formData.phone,
        website: formData.website,
        industry: formData.industry,
        companySize: formData.companySize,
        companyDescription: formData.description,
        budgetRange: formData.budget,
        targetAudience: formData.targetAudience
      }, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      toast({
        title: "Профіль оновлено",
        description: "Дані вашої компанії успішно збережено.",
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
          <div className="w-16 h-16 bg-gradient-to-r from-ua-blue to-ua-blue-soft rounded-2xl flex items-center justify-center mx-auto mb-6">
            <Building2 className="h-8 w-8 text-white" />
          </div>
          <h1 className="text-4xl font-bold mb-4">
            <span className="gradient-text">Профіль бренду</span>
          </h1>
          <p className="text-xl text-gray-600">
            Керуйте інформацією вашої компанії
          </p>
        </div>

        <div className="grid lg:grid-cols-3 gap-8">
          {/* Logo Section */}
          <div className="lg:col-span-1">
            <Card className="bg-white/80 backdrop-blur-sm border-ua-blue-light">
              <CardHeader>
                <CardTitle className="text-center">Логотип компанії</CardTitle>
              </CardHeader>
              <CardContent className="text-center space-y-4">
                <Avatar className="w-32 h-32 mx-auto rounded-lg">
                  <AvatarImage src={logoPreview} className="rounded-lg" />
                  <AvatarFallback className="text-2xl bg-gradient-to-r from-ua-blue to-ua-blue-soft text-white rounded-lg">
                    {formData.company ? formData.company.split(' ').map(n => n[0]).join('') : 'C'}
                  </AvatarFallback>
                </Avatar>
                
                <div>
                  <input
                    type="file"
                    accept="image/*"
                    onChange={handleLogoChange}
                    className="hidden"
                    id="logo-upload"
                  />
                  <Label htmlFor="logo-upload">
                    <Button
                      type="button"
                      variant="outline"
                      className="cursor-pointer border-ua-blue text-ua-blue hover:bg-ua-blue hover:text-white"
                      asChild
                    >
                      <span>
                        <Upload className="h-4 w-4 mr-2" />
                        Завантажити логотип
                      </span>
                    </Button>
                  </Label>
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Company Information */}
          <div className="lg:col-span-2">
            <Card className="bg-white/80 backdrop-blur-sm border-ua-blue-light">
              <CardHeader>
                <CardTitle>Інформація про компанію</CardTitle>
                <CardDescription>
                  Оновіть дані вашого бренду
                </CardDescription>
              </CardHeader>
              
              <CardContent className="space-y-6">
                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <Label htmlFor="company">Назва компанії *</Label>
                    <Input 
                      id="company"
                      value={formData.company}
                      onChange={(e) => handleInputChange('company', e.target.value)}
                      className="border-ua-blue-light focus:border-ua-pink"
                    />
                  </div>
                  <div>
                    <Label htmlFor="contact-name">Контактна особа *</Label>
                    <Input 
                      id="contact-name"
                      value={formData.contactName}
                      onChange={(e) => handleInputChange('contactName', e.target.value)}
                      className="border-ua-blue-light focus:border-ua-pink"
                    />
                  </div>
                </div>

                <div className="grid md:grid-cols-2 gap-4">
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
                  <div>
                    <Label htmlFor="phone">Телефон *</Label>
                    <Input 
                      id="phone"
                      value={formData.phone}
                      onChange={(e) => handleInputChange('phone', e.target.value)}
                      className="border-ua-blue-light focus:border-ua-pink"
                    />
                  </div>
                </div>

                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <Label htmlFor="website">Веб-сайт</Label>
                    <div className="relative">
                      <Globe className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                      <Input 
                        id="website"
                        value={formData.website}
                        onChange={(e) => handleInputChange('website', e.target.value)}
                        className="pl-10 border-ua-blue-light focus:border-ua-pink"
                      />
                    </div>
                  </div>
                  <div>
                    <Label htmlFor="industry">Галузь *</Label>
                    <Select value={formData.industry} onValueChange={(value) => handleInputChange('industry', value)}>
                      <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent className="bg-white border-ua-blue-light">
                        {industries.map(industry => (
                          <SelectItem key={industry} value={industry}>{industry}</SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                </div>

                <div>
                  <Label htmlFor="company-size">Розмір компанії</Label>
                  <Select value={formData.companySize} onValueChange={(value) => handleInputChange('companySize', value)}>
                    <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent className="bg-white border-ua-blue-light">
                      {companySizes.map(size => (
                        <SelectItem key={size} value={size}>{size}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>

                <div>
                  <Label htmlFor="description">Опис компанії *</Label>
                  <Textarea 
                    id="description"
                    value={formData.description}
                    onChange={(e) => handleInputChange('description', e.target.value)}
                    className="border-ua-blue-light focus:border-ua-pink min-h-[100px]"
                  />
                </div>

                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <Label htmlFor="budget">Орієнтовний бюджет (місячний)</Label>
                    <Select value={formData.budget} onValueChange={(value) => handleInputChange('budget', value)}>
                      <SelectTrigger className="border-ua-blue-light focus:border-ua-pink">
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent className="bg-white border-ua-blue-light">
                        <SelectItem value="under-10k">До 10,000 грн</SelectItem>
                        <SelectItem value="10k-25k">10,000 - 25,000 грн</SelectItem>
                        <SelectItem value="25k-50k">25,000 - 50,000 грн</SelectItem>
                        <SelectItem value="50k-100k">50,000 - 100,000 грн</SelectItem>
                        <SelectItem value="over-100k">Понад 100,000 грн</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                  <div>
                    <Label htmlFor="target-audience">Цільова аудиторія</Label>
                    <Input 
                      id="target-audience"
                      value={formData.targetAudience}
                      onChange={(e) => handleInputChange('targetAudience', e.target.value)}
                      className="border-ua-blue-light focus:border-ua-pink"
                    />
                  </div>
                </div>

                <div className="flex gap-4 pt-6">
                  <Button 
                    onClick={handleSave}
                    className="flex-1 bg-gradient-to-r from-ua-blue to-ua-blue-soft hover:from-ua-blue-soft hover:to-ua-blue text-white py-3 text-lg font-semibold"
                  >
                    <Users className="h-5 w-5 mr-2" />
                    Зберегти зміни
                  </Button>
                  
                  <Button 
                    variant="outline"
                    asChild
                    className="border-ua-pink text-ua-pink hover:bg-ua-pink hover:text-white"
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

export default BrandProfile;
