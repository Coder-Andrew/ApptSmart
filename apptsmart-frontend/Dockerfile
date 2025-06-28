FROM node:20-alpine AS deps
WORKDIR /app
COPY package*.json ./
RUN npm ci

FROM node:20-alpine AS build
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build --no-lint

FROM node:20-alpine AS runner
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5100
CMD ["npm", "start"]