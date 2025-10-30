# 🛍️ Katalog Obuće - ASP.NET Core MVC Aplikacija

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-6.0%2B-purple)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-7.0-green)
![SQL Server](https://img.shields.io/badge/SQL_Server-2019%2B-blue)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-blueviolet)

Web aplikacija za online katalog obuće sa administracijom kategorija/korisnika, menadžmentom proizvoda, korpom i porudžbinama. Uloge su razdvojene (Admin, Manager, User) radi jasnih odgovornosti.

---

## 🚀 Brzo pokretanje

### 📋 Preduslovi
- .NET SDK 6.0+ (preporučeno 8.0)
- SQL Server (LocalDB, Developer ili Express)
- Visual Studio 2022

### 🧩 Koraci
1. Kloniraj repozitorijum.
2. U appsettings.json podesi konekcioni string (ConnectionStrings:DefaultConnection).
3. U SQL Server-u pokreni skriptu iz foldera `database/` (kreira šemu i početne podatke).
4. (Opcionalno) Dev HTTPS sertifikat:  
   `dotnet dev-certs https --trust`
5. U Visual Studiju pokreni projekat:  
   Izaberi **“https”** profil iz toolbara i pritisni **Run** (▶).
6. Prijavi se test nalozima (ispod).

---

## 🔐 Demo nalozi

- Admin: `admin@obuca.com` / `admin123`  
- Manager: `manager@obuca.com` / `manager123`
- User: `danilo44619@its.edu.rs` / `danilo44619`

---

## ✅ Funkcionalnosti

- Pregled proizvoda po kategorijama, detalji i pretraga
- Korpa i checkout, pregled porudžbina
- Uloge i dozvole:
  - Administrator: upravlja kategorijama i korisnicima
  - Menadžer: upravlja proizvodima (dodavanje, izmena, brisanje)
  - Korisnik: kupovina (korpa/porudžbine)
- “Nazad” tasteri na ključnim stranicama (lakša navigacija)
- Redosled kategorija (DisplayOrder) dodeljuje se automatski i nije vidljiv
- Upload slika proizvoda u `wwwroot/images`

---

## 🛠️ Tehnologije

- ASP.NET Core MVC
- Entity Framework Core (SQL Server)
- Cookie autentikacija, autorizacija po ulogama
- Bootstrap 5, jQuery

---

## 🗂️ Struktura projekta (ukratko)

- `Controllers/` – Admin, Manager, Account, Product, Cart, Order, Home
- `Models/`, `ViewModels/`
- `Repositories/` – Category, Product, User, Order
- `Services/` – CartService
- `Views/` – Admin, Manager, Product, Cart, Order, Shared…
- `wwwroot/` – css, js, images
- `database/` – SQL skripte (šema i početni podaci)

---

## ▶️ Pokretanje (detaljno)

- Konekcioni string (primer):
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=shoeCatalogdb;Trusted_Connection=True;TrustServerCertificate=True;"
      }
    }

- Pokretanje:
    U Visual Studiju, izaberi **“https"** profil i pritisni **F5** (ili **Ctrl+F5** za start bez debuggera). Aplikacija će se pokrenuti na portovima definisanim u `Properties/launchSettings.json` (npr. https://localhost:7290).

---


## 🖼️ Slike proizvoda

- Upload ide u `wwwroot/images`
- Ako deployuješ na hosting, obezbedi write permisije za taj folder

- Lozinke su radi jednostavnosti čuvane kao plain text (kolona `Password`).  
  U produkciji obavezno uvesti hashing (PBKDF2/BCrypt/Argon2) i politiku jačine lozinke.
- Voditi računa o portovima/profilima (uvek koristi portove koje vidiš u “Now listening on:”).
