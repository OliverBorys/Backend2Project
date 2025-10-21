# Backend2Project

Ett RESTful Web API byggt med **ASP.NET Core 9**, **Entity Framework Core** och **JWT-autentisering**.  
API:et hanterar **produkter, kategorier och användare** samt stödjer rollbaserad åtkomst (admin/customer).  
Swagger används för automatiskt genererad API-dokumentation.

## Funktioner

- CRUD för **Produkter** (`/api/products`)
- CRUD för **Kategorier** (`/api/categories`)
- CRUD för **Användare** (`/api/users`)
- Inloggning med JWT-token (`/api/auth/token`)
- Rollbaserad autentisering (`[Authorize(Roles = "admin")]`)
- Publika GET-anrop på produkter och kategorier
- Swagger UI för testning och dokumentation

## Tekniker

| Teknik				| Användning				|
|-----------------------|---------------------------|
| ASP.NET Core			| API & Controllers			|
| Entity Framework Core | Databas & ORM				|
| MS SQL Server			| Databas					|
| JWT (JSON Web Token)	| Autentisering				|
| Swagger / Swashbuckle | Dokumentation & test		|
| Dependency Injection	| Services och datalager	|

## Installation och uppstart

### 1. Klona projektet
```bash
git clone <repo-url>
cd Backend2Project
```

### 2. Konfigurera `appsettings.json`
```json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=TestProjekt;Trusted_Connection=True;TrustServerCertificate=True"
},
"Jwt": {
  "SigningKey": "o0yXzXlq8N3m9JwJw2JZbqv8l8o8N3Qy8m9c2JwVxjU="
}
```

### 3. (Om migrationer används)
```bash
dotnet ef database update
```

### 4. Starta API:t
```bash
dotnet run
```

## Swagger – API-dokumentation

När projektet körs, öppna:  
```
http://localhost:<port>/swagger
```

Där kan du se alla endpoints och testa direkt via webbläsaren.

## Autentisering med JWT

### Använd token i Swagger
- Klicka på **Authorize** (uppe till höger)
- Klistra in **endast token**

## Autorisering

| Endpoint					| Kräver token | Kräver admin |
|---------------------------|--------------|--------------|
| GET /api/products			| ❌		   | ❌			  |
| POST /api/products		| ✅		   | ✅			  |
| PATCH /api/products/{id}	| ✅		   | ✅			  |
| DELETE /api/products/{id} | ✅		   | ✅			  |
| GET /api/categories		| ❌		   | ❌			  |
| POST /api/categories		| ✅		   | ✅			  |
| GET /api/users			| ❌		   | ❌			  |
| DELETE /api/users/{id}	| ✅		   | ✅			  |


## Kommande förbättringar

- Hashning av lösenord (BCrypt)


## Sammanfattning

Detta API erbjuder en komplett backend med autentisering, rollbaserad access, 
produkter/kategorier/användare och Swagger-dokumentation.  
Perfekt grund för en e-handel eller administrativ applikation.