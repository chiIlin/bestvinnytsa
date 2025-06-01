import { Link } from 'react-router-dom';
import { Button } from '@/components/ui/base-ui';
import { Users, Building2, Star, TrendingUp, Heart } from 'lucide-react';

const HeroSection = () => {
  return (
    <section className="py-20 px-4">
      <div className="container mx-auto text-center">
        <div className="max-w-4xl mx-auto animate-fade-in">
          <h1 className="text-5xl md:text-6xl font-bold mb-6 leading-tight">
            <span className="gradient-text">Об'єднуємо</span><br />
            <span className="text-gray-800">українських інфлюенсерів</span><br />
            <span className="text-gray-800">із</span> <span className="gradient-text">брендами</span>
          </h1>
          
          <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto leading-relaxed">
            Платформа для ефективної співпраці між мікро-інфлюенсерами та місцевими брендами. 
            Знайдіть ідеального партнера для вашого бізнесу або творчості.
          </p>

          <div className="flex flex-col sm:flex-row gap-4 justify-center mb-12">
            <Link to="/register-influencer">
              <Button 
                size="lg" 
                className="bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white px-8 py-4 text-lg font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
              >
                <Users className="h-5 w-5 mr-2" />
                Я інфлюенсер
              </Button>
            </Link>
            <Link to="/register-brand">
              <Button 
                size="lg" 
                variant="outline"
                className="border-2 border-ua-blue text-ua-blue hover:bg-ua-blue hover:text-white px-8 py-4 text-lg font-semibold transition-all duration-300"
              >
                <Building2 className="h-5 w-5 mr-2" />
                Я бренд
              </Button>
            </Link>
          </div>

          <div className="grid md:grid-cols-3 gap-8 mt-16">
            <div className="text-center p-6 bg-white/60 backdrop-blur-sm rounded-2xl border border-ua-pink-light card-hover">
              <div className="w-12 h-12 bg-gradient-to-r from-ua-pink to-ua-pink-soft rounded-xl flex items-center justify-center mx-auto mb-4">
                <Heart className="h-6 w-6 text-white" />
              </div>
              <h3 className="text-xl font-semibold mb-3 text-gray-800">Автентичність</h3>
              <p className="text-gray-600">Справжні українські інфлюенсери з активною та залученою аудиторією</p>
            </div>

            <div className="text-center p-6 bg-white/60 backdrop-blur-sm rounded-2xl border border-ua-blue-light card-hover">
              <div className="w-12 h-12 bg-gradient-to-r from-ua-blue to-ua-blue-soft rounded-xl flex items-center justify-center mx-auto mb-4">
                <TrendingUp className="h-6 w-6 text-white" />
              </div>
              <h3 className="text-xl font-semibold mb-3 text-gray-800">Ефективність</h3>
              <p className="text-gray-600">Швидкий пошук та зв'язок із потрібними інфлюенсерами за лічені хвилини</p>
            </div>

            <div className="text-center p-6 bg-white/60 backdrop-blur-sm rounded-2xl border border-ua-pink-light card-hover">
              <div className="w-12 h-12 bg-gradient-to-r from-ua-pink-soft to-ua-blue-light rounded-xl flex items-center justify-center mx-auto mb-4">
                <Star className="h-6 w-6 text-white" />
              </div>
              <h3 className="text-xl font-semibold mb-3 text-gray-800">Якість</h3>
              <p className="text-gray-600">Перевірені профілі та детальна інформація про кожного інфлюенсера</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default HeroSection;
