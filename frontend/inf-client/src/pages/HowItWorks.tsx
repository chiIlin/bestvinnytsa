import { Users, Building2, Search } from 'lucide-react';

const HowItWorks = () => {
  return (
    <div className="min-h-screen flex flex-col">

      <section className="flex-1 py-16 px-4">
        <div className="container mx-auto max-w-3xl">
          <div className="bg-white/60 backdrop-blur-sm rounded-2xl border border-ua-pink-light p-8 md:p-12">
            <div className="text-center mb-12">
              <h1 className="text-4xl font-bold mb-4 gradient-text">
                Як це працює
              </h1>
              <p className="text-xl text-gray-600">
                UA Influencer Connect — це платформа, яка допомагає українським інфлюенсерам та брендам легко знаходити одне одного та ефективно співпрацювати.
              </p>
            </div>

            <div className="space-y-10">
              {/* Для інфлюенсерів */}
              <div className="flex flex-col md:flex-row items-start gap-4 md:gap-6">
                <div className="flex-shrink-0 w-14 h-14 flex items-center justify-center rounded-xl bg-gradient-to-r from-ua-pink to-ua-pink-soft">
                  <Users className="h-7 w-7 text-white" />
                </div>
                <div>
                  <h2 className="text-2xl font-semibold mb-2">Для інфлюенсерів</h2>
                  <ul className="list-disc ml-6 text-gray-700 space-y-1">
                    <li>Створіть профіль та розкажіть про себе, свою аудиторію та соціальні мережі.</li>
                    <li>Додавайте свої категорії, місто, контактні дані та статистику.</li>
                    <li>Отримуйте пропозиції співпраці від брендів напряму через платформу.</li>
                  </ul>
                </div>
              </div>

              {/* Для брендів */}
              <div className="flex flex-col md:flex-row items-start gap-4 md:gap-6">
                <div className="flex-shrink-0 w-14 h-14 flex items-center justify-center rounded-xl bg-gradient-to-r from-ua-blue to-ua-blue-soft">
                  <Building2 className="h-7 w-7 text-white" />
                </div>
                <div>
                  <h2 className="text-2xl font-semibold mb-2">Для брендів</h2>
                  <ul className="list-disc ml-6 text-gray-700 space-y-1">
                    <li>Створіть акаунт бренду та опишіть свою компанію, продукти й цілі співпраці.</li>
                    <li>Шукайте інфлюенсерів за категоріями, містом, кількістю підписників та іншими фільтрами.</li>
                    <li>Звʼязуйтеся з креаторами напряму для обговорення співпраці.</li>
                  </ul>
                </div>
              </div>

              {/* Зручний пошук та комунікація */}
              <div className="flex flex-col md:flex-row items-start gap-4 md:gap-6">
                <div className="flex-shrink-0 w-14 h-14 flex items-center justify-center rounded-xl bg-gradient-to-r from-ua-pink-soft to-ua-blue-light">
                  <Search className="h-7 w-7 text-white" />
                </div>
                <div>
                  <h2 className="text-2xl font-semibold mb-2">Зручний пошук та комунікація</h2>
                  <ul className="list-disc ml-6 text-gray-700 space-y-1">
                    <li>Використовуйте фільтри для швидкого пошуку потрібних профілів.</li>
                    <li>Обирайте найкращих партнерів для своїх проєктів.</li>
                    <li>Комунікуйте безпосередньо через контактні дані профілю.</li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

    </div>
  );
};

export default HowItWorks;