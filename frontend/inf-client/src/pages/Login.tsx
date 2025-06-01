import { Link, useNavigate } from 'react-router-dom';
import { Button, Input, Label, Separator } from '@/components/ui/base-ui';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { LogIn, Users, Building2 } from 'lucide-react';
import axios from "axios";
import { useState } from "react";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(""); // Додаємо стан для успіху
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    try {
      const response = await axios.post("http://localhost:5112/api/auth/login", {
        email,
        password,
      });
      console.log("Role from backend:", response.data.role);
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("role", response.data.role);
      
      // Оповіщаємо про зміну стану аутентифікації
      window.dispatchEvent(new Event("authStateChanged"));
      
      setSuccess("Вхід успішний!");
      if (response.data.role === "company") {
        navigate("/brand-profile");
      } else if (response.data.role === "influencer") {
        navigate("/influencer-profile");
      }
    } catch (err: any) {
      setError(err.response?.data?.message || "Помилка входу");
    }
  };

  return (
    <div className="min-h-screen">
      
      <section className="py-16 px-4">
        <div className="container mx-auto max-w-md">
          <div className="text-center mb-12">
            <div className="w-16 h-16 bg-gradient-to-r from-ua-pink to-ua-blue rounded-2xl flex items-center justify-center mx-auto mb-6">
              <LogIn className="h-8 w-8 text-white" />
            </div>
            <h1 className="text-4xl font-bold mb-4">
              <span className="gradient-text">Вхід</span>
            </h1>
            <p className="text-xl text-gray-600">
              Увійдіть до свого акаунту
            </p>
          </div>

          <Card className="bg-white/80 backdrop-blur-sm border-ua-pink-light">
            <CardHeader>
              <CardTitle>З поверненням!</CardTitle>
              <CardDescription>
                Введіть свої дані для входу в акаунт
              </CardDescription>
            </CardHeader>
            
            <CardContent className="space-y-6">
              <form onSubmit={handleLogin}>
                <div>
                  <Label htmlFor="email">Email</Label>
                  <Input 
                    id="email"
                    type="email"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                    placeholder="your@email.com"
                    className="border-ua-blue-light focus:border-ua-pink"
                  />
                </div>

                <div>
                  <Label htmlFor="password">Пароль</Label>
                  <Input 
                    id="password"
                    type="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    placeholder="Введіть пароль"
                    className="border-ua-blue-light focus:border-ua-pink"
                  />
                </div>

                <div className="text-right">
                  <Link 
                    to="/forgot-password" 
                    className="text-sm text-ua-blue hover:text-ua-blue-soft"
                  >
                    Забули пароль?
                  </Link>
                </div>

                <Button 
                  type="submit"
                  className="w-full bg-gradient-to-r from-ua-pink to-ua-blue hover:from-ua-pink-soft hover:to-ua-blue-soft text-white py-3 text-lg font-semibold"
                >
                  Увійти
                </Button>

                {error && <div className="text-red-500">{error}</div>}
                {success && <div className="text-green-600">{success}</div>} {/* Додаємо під формою */}

                <div className="relative">
                  <div className="absolute inset-0 flex items-center">
                    <Separator className="w-full" />
                  </div>
                  <div className="relative flex justify-center text-xs uppercase">
                    <span className="bg-white px-2 text-gray-500">або</span>
                  </div>
                </div>

                <div className="space-y-3">
                  <p className="text-center text-gray-600 mb-4">Ще немає акаунту?</p>
                  
                  <Link to="/register-influencer">
                    <Button 
                      variant="outline" 
                      className="w-full border-ua-pink text-ua-pink hover:bg-ua-pink hover:text-white"
                    >
                      <Users className="h-4 w-4 mr-2" />
                      Реєстрація як інфлюенсер
                    </Button>
                  </Link>
                  
                  <Link to="/register-brand" className="mt-[5px] block">
                    <Button 
                      variant="outline" 
                      className="w-full border-ua-blue text-ua-blue hover:bg-ua-blue hover:text-white"
                    >
                      <Building2 className="h-4 w-4 mr-2" />
                      Реєстрація як бренд
                    </Button>
                  </Link>
                </div>
              </form>
            </CardContent>
          </Card>
        </div>
      </section>
      
    </div>
  );
};

export default Login;
