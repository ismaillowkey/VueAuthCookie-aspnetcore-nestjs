# Auth example with vue and netjs/asp net core
Example for vue 2+typescript and nestjs/asp net core 6 as backend for Authentication with cookie
nestjs/asp net core using sqlite for database

## Run app
- Asp net core run on port 5000
- Nestjs run on port 3000
- Vue 2 run on port 8080

## Run asp net core 6 as backend
Restore package 
```
cd Backend/aspnetcore6/CookieAuthJwtRefresh
dotnet restore
```
run app (dev mode)
```
dotnet watch
```

## Run nestjs as backend
Restore package 
```
cd Backend\nestjs
npm install
```
run app (dev mode)
```
npm run start:dev
```

## Run vue 2 as frontend
restore package
```
cd Frontend
npm install
```
run app (dev mode)
```
npm run serve
```

## Change backend in vue
open file helpers/axios.ts and change
```
axios.defaults.baseURL = 'http://localhost:5000/api'
```

- for asp net core as backend http://localhost:5000/api
- for nestjs as backend http://localhost:3000/api



## For testing mail server
Download and run Papercut-SMTP [https://github.com/ChangemakerStudios/Papercut-SMTP]