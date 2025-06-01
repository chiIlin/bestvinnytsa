import { Toaster } from "@/components/ui/toaster";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "@/components/Header";
import Footer from "@/components/Footer";
import Index from "./pages/Index";
import Catalog from "./pages/Catalog";
import RegisterInfluencer from "./pages/RegisterInfluencer";
import RegisterBrand from "./pages/RegisterBrand";
import Login from "./pages/Login";
import NotFound from "./pages/NotFound";
import HowItWorks from "./pages/HowItWorks";
import ScrollToTop from "@/components/ScrollToTop";
import About from "./pages/About"; // Import the About component
import InfluencerProfile from "@/pages/InfluencerProfile";
import BrandProfile from "@/pages/BrandProfile";

const queryClient = new QueryClient();

const App = () => (
  <QueryClientProvider client={queryClient}>
    <TooltipProvider>
      <Toaster />
      <Sonner />
      <Router>
        <Header />
        <ScrollToTop />
        <Routes>
          <Route path="/" element={<Index />} />
          <Route path="/catalog" element={<Catalog />} />
          <Route path="/register-influencer" element={<RegisterInfluencer />} />
          <Route path="/register-brand" element={<RegisterBrand />} />
          <Route path="/how-it-works" element={<HowItWorks />} />
          <Route path="/login" element={<Login />} />
          <Route path="/about" element={<About />} /> {/* Add the About route */}
          <Route path="/influencer-profile" element={<InfluencerProfile />} />
          <Route path="/brand-profile" element={<BrandProfile />} />
          {/* ADD ALL CUSTOM ROUTES ABOVE THE CATCH-ALL "*" ROUTE */}
          <Route path="*" element={<NotFound />} />
        </Routes>
        <Footer />
      </Router>
    </TooltipProvider>
  </QueryClientProvider>
);

export default App;
