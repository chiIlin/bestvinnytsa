
import HeroSection from '@/components/HeroSection';
import StatsSection from '@/components/StatsSection';
import InfluencerCatalog from '@/components/InfluencerCatalog';

const Index = () => {
  return (
    <div className="min-h-screen">
      <HeroSection />
      <StatsSection />
      <div className="bg-gradient-to-b from-white to-ua-pink-light">
        <InfluencerCatalog />
      </div>
    </div>
  );
};

export default Index;
