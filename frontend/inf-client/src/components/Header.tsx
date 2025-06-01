import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/base-ui";
import { Menu, X, Users, Building2 } from "lucide-react";
import axios from "axios";

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [role, setRole] = useState<string | null>(null);
  const navigate = useNavigate();

  // Функція для перевірки валідності токена
  const validateToken = async () => {
    const token = localStorage.getItem("token");
    const role = localStorage.getItem("role");
    
    if (!token || !role) {
      setIsLoggedIn(false);
      setRole(null);
      return;
    }

    try {
      // Вибираємо правильний ендпоінт залежно від ролі
      const endpoint = role === "company" 
        ? 'http://localhost:5112/api/profile/company'
        : 'http://localhost:5112/api/profile/influencer';
        
      await axios.get(endpoint, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      
      // Якщо запит успішний, токен валідний
      setIsLoggedIn(true);
      setRole(role);
    } catch (error) {
      // Якщо запит неуспішний (401, 403), токен недійсний
      console.log("Token validation failed:", error);
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      setIsLoggedIn(false);
      setRole(null);
      window.dispatchEvent(new Event("authStateChanged"));
    }
  };

  // Функція для оновлення стану аутентифікації
  const updateAuthState = () => {
    const token = localStorage.getItem("token");
    if (token) {
      validateToken();
    } else {
      setIsLoggedIn(false);
      setRole(null);
    }
  };

  useEffect(() => {
    // Початкова перевірка
    updateAuthState();

    // Додаємо слухач подій для localStorage
    const handleStorageChange = (e: StorageEvent) => {
      if (e.key === "token" || e.key === "role") {
        updateAuthState();
      }
    };

    // Додаємо кастомний слухач для програмних змін localStorage
    const handleAuthChange = () => {
      updateAuthState();
    };

    window.addEventListener("storage", handleStorageChange);
    window.addEventListener("authStateChanged", handleAuthChange);

    return () => {
      window.removeEventListener("storage", handleStorageChange);
      window.removeEventListener("authStateChanged", handleAuthChange);
    };
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    setIsLoggedIn(false);
    setRole(null);
    navigate("/");
    // Оповіщаємо про зміну стану
    window.dispatchEvent(new Event("authStateChanged"));
  };

  return (
    <header className="bg-white/80 backdrop-blur-md border-b border-ua-pink-light sticky top-0 z-50">
      <div className="container mx-auto px-4 py-4">
        <div className="flex items-center justify-between">
          <Link to="/" className="flex items-center space-x-2">
            <div className="w-8 h-8 bg-gradient-to-r from-ua-pink to-ua-blue rounded-lg flex items-center justify-center">
              <Users className="h-5 w-5 text-white" />
            </div>
            <span className="text-xl font-bold gradient-text">
              UA Influencer Connect
            </span>
          </Link>

          {/* Desktop Navigation */}
          <nav className="hidden md:flex items-center space-x-8">
            <Link
              to="/catalog"
              className="text-gray-700 hover:text-ua-pink transition-colors"
            >
              Каталог
            </Link>
            <Link
              to="/how-it-works"
              className="text-gray-700 hover:text-ua-pink transition-colors"
            >
              Як це працює
            </Link>
            {/* <Link to="/pricing" className="text-gray-700 hover:text-ua-pink transition-colors">
              Тарифи
            </Link> */}
          </nav>

          {/* Desktop Auth Buttons */}
          <div className="hidden md:flex items-center space-x-3">
            {isLoggedIn ? (
              <div className="flex gap-2">
                <button
                  onClick={handleLogout}
                  className="bg-ua-blue text-white px-4 py-2 rounded hover:bg-ua-blue-soft transition"
                >
                  Вийти
                </button>
                <Link to={role === "company" ? "/brand-profile" : "/influencer-profile"}>
                  <button className="bg-ua-pink text-white px-4 py-2 rounded hover:bg-ua-pink-soft transition">
                    Профіль
                  </button>
                </Link>
              </div>
            ) : (
              <>
                <Link to="/login">
                  <Button variant="ghost" className="text-ua-blue hover:text-ua-pink">
                    Увійти
                  </Button>
                </Link>
                <Link to="/register-influencer">
                  <Button className="bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white">
                    <Users className="h-4 w-4 mr-2" />
                    Інфлюенсер
                  </Button>
                </Link>
                <Link to="/register-brand">
                  <Button className="bg-gradient-to-r from-ua-blue to-ua-blue-soft hover:from-ua-blue-soft hover:to-ua-blue text-white">
                    <Building2 className="h-4 w-4 mr-2" />
                    Бренд
                  </Button>
                </Link>
              </>
            )}
          </div>

          {/* Mobile Menu Button */}
          <button
            className="md:hidden p-2"
            onClick={() => setIsMenuOpen(!isMenuOpen)}
          >
            {isMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
          </button>
        </div>

        {/* Mobile Navigation */}
        {isMenuOpen && (
          <div className="md:hidden mt-4 pb-4 border-t border-ua-pink-light pt-4">
            <nav className="flex flex-col space-y-3">
              <Link
                to="/catalog"
                className="text-gray-700 hover:text-ua-pink transition-colors py-2"
              >
                Каталог
              </Link>
              <Link
                to="/how-it-works"
                className="text-gray-700 hover:text-ua-pink transition-colors py-2"
              >
                Як це працює
              </Link>
              {/* <Link to="/pricing" className="text-gray-700 hover:text-ua-pink transition-colors py-2">
                Тарифи
              </Link> */}
              <div className="flex flex-col space-y-2 pt-3 border-t border-ua-pink-light">
                {isLoggedIn ? (
                  <div className="flex flex-col gap-2">
                    <button
                      onClick={handleLogout}
                      className="bg-ua-blue text-white px-4 py-2 rounded hover:bg-ua-blue-soft transition"
                    >
                      Вийти
                    </button>
                    <Link to={role === "company" ? "/brand-profile" : "/influencer-profile"}>
                      <button className="bg-ua-pink text-white px-4 py-2 rounded hover:bg-ua-pink-soft transition">
                        Профіль
                      </button>
                    </Link>
                  </div>
                ) : (
                  <>
                    <Link to="/login">
                      <Button variant="ghost" className="w-full text-ua-blue hover:text-ua-pink">
                        Увійти
                      </Button>
                    </Link>
                    <Link to="/register-influencer">
                      <Button className="w-full bg-gradient-to-r from-ua-pink to-ua-pink-soft hover:from-ua-pink-soft hover:to-ua-pink text-white">
                        <Users className="h-4 w-4 mr-2" />
                        Інфлюенсер
                      </Button>
                    </Link>
                    <Link to="/register-brand">
                      <Button className="w-full bg-gradient-to-r from-ua-blue to-ua-blue-soft hover:from-ua-blue-soft hover:to-ua-blue text-white">
                        <Building2 className="h-4 w-4 mr-2" />
                        Бренд
                      </Button>
                    </Link>
                  </>
                )}
              </div>
            </nav>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;
