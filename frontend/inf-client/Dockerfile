FROM node:20-alpine
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN chmod +x entrypoint.sh   # <---- Add this line
EXPOSE 8080
CMD ["sh", "./entrypoint.sh"]
