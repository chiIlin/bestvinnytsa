import { Users, Building2, Star, TrendingUp } from 'lucide-react';

const StatsSection = () => {
  const stats = [
    {
      icon: Users,
      number: '1,200+',
      label: 'Інфлюенсерів',
      description: 'Активних українських креаторів',
      color: 'from-ua-pink to-ua-pink-soft'
    },
    {
      icon: Building2,
      number: '350+',
      label: 'Брендів',
      description: 'Довіряють нашій платформі',
      color: 'from-ua-blue to-ua-blue-soft'
    },
    {
      icon: Star,
      number: '95%',
      label: 'Задоволення',
      description: 'Рейтинг успішних кампаній',
      color: 'from-ua-pink-soft to-ua-blue-light'
    },
    {
      icon: TrendingUp,
      number: '24 год',
      label: 'Швидкість',
      description: 'Середній час відгуку',
      color: 'from-ua-blue-light to-ua-pink'
    }
  ];

  return (
    <section className="py-20 px-4">
      <div className="container mx-auto">
        <div className="text-center mb-16">
          <h2 className="text-4xl font-bold mb-4">
            <span className="gradient-text">Наші досягнення</span>
          </h2>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            Цифри, які говорять про ефективність нашої платформи
          </p>
        </div>

        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-8">
          {stats.map((stat, index) => (
            <div 
              key={index}
              className="text-center p-8 bg-white/60 backdrop-blur-sm rounded-2xl border border-ua-pink-light card-hover animate-fade-in"
              style={{ animationDelay: `${index * 0.1}s` }}
            >
              <div className={`w-16 h-16 bg-gradient-to-r ${stat.color} rounded-2xl flex items-center justify-center mx-auto mb-6`}>
                <stat.icon className="h-8 w-8 text-white" />
              </div>
              
              <div className="text-4xl font-bold gradient-text mb-2">
                {stat.number}
              </div>
              
              <div className="text-xl font-semibold text-gray-800 mb-2">
                {stat.label}
              </div>
              
              <p className="text-gray-600 text-sm">
                {stat.description}
              </p>

              {/* Якщо тут є бейджі, додай контейнер */}
              {/* <div className="flex flex-wrap gap-2 justify-center mt-2">
                {stat.badges?.map(badge => (
                  <Badge key={badge}>{badge}</Badge>
                ))}
              </div> */}
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default StatsSection;
