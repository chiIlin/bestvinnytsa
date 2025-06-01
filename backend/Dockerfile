# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./src/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
COPY . .

# Додаємо цю команду для заміни VITE_API_URL у .env або .env.production
RUN sed -i 's|VITE_API_URL=.*|VITE_API_URL=http://backend:5112|' .env || echo "VITE_API_URL=http://backend:5112" >> .env

RUN chmod +x entrypoint.sh
EXPOSE 8080
CMD ["sh", "./entrypoint.sh"]
