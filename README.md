# SklepWelniany 

Aplikacja webowa typu **ASP.NET Core MVC** przedstawiająca prosty sklep internetowy. Aplikacja demonstruje działanie koszyka zakupowego, autoryzacji użytkowników oraz operacji CRUD z wykorzystaniem **Entity Framework Core** i bazy danych **SQL Server**.

> **Projekt nie zawiera integracji z bramką płatności – skupia się na logice sklepu, koszyku oraz zamówieniach.**

---

## Funkcjonalności 

System umożliwia:

- przeglądanie produktów,
- filtrowanie produktów według kategorii,
- wyszukiwanie produktów,
- dodawanie produktów do koszyka,
- składanie zamówień,
- autoryzację użytkowników (administrator /użytkownik),
- zarządzanie produktami przez administratora.                                

---

## Tech stack 

- ASP.NET Core MVC (.NET 8 lub nowszy)
- MS SQL Server
- ASP.NET Identity (uwierzytelnianie i role)                                        
- Bootstrap (warstwa frontendowa)

---

## Wymagania systemowe 

- .NET SDK 8.0 lub nowszy
- Visual Studio 2022 lub nowsze (projekt tworzony w Visual Studio 2025)
- SQL Server (LocalDB lub inna instancja)
- SQL Server Management Studio 2021

---

## Jak uruchomić projekt? 

### 1. Klonowanie repozytorium

```bash
git clone https://github.com/Falisty1612/SklepWelniany
```

---

### 2. Konfiguracja projektu

1. Otwórz plik rozwiązania `.slnx` w Visual Studio.
2. Otwórz plik `appsettings.json`.
3. Zmień nazwę serwera bazy danych ("YOUR_SERVER_NAME") w connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SklepWelniany;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

---

### 3. Migracje bazy danych

W **Package Manager Console** wykonaj polecenie:

```powershell
Update-Database
```

 Przy pierwszym uruchomieniu aplikacji:
- baza danych zostanie utworzona automatycznie,
- zostaną utworzone wymagane tabele,
- aplikacja będzie gotowa do użycia.

---

### 4. Uruchomienie aplikacji

**Uruchom projekt z poziomu Visual Studio.**

---

## Konta testowe 

### Administrator

- **Email:** `admin@localhost.com`
- **Hasło:** `Admin@99`

Administrator może:                                                                                     
- dodawać produkty,
- edytować produkty,
- usuwać produkty,
- zarządzać danymi w systemie,
- wyświeltać zamówinia i edytować
- zarządzać stanem magazynu

---

### Użytkownik

- Konto użytkownika można utworzyć poprzez formularz **Register** dostępny w aplikacji.

Zwykły użytkownik może:
- przeglądać produkty,
- filtrować produkty po kategoriach,
- wyszukiwać produkty,
- dodawać produkty do koszyka,
- składać zamówienia.

---

## Formularze i walidacja 

W aplikacji zaimplementowano formularze:

- logowania,
- rejestracji,
- składania zamówienia,
- wyszukiwania produktów.

Zastosowane mechanizmy walidacji:

- `[Required]`
- `[MaxLength]`
- `[NotMapped]`
- `[Table]`
- mechanizmy autoryzacji i uwierzytelniania użytkowników.                                      

---

## Struktura bazy danych 

Aplikacja wykorzystuje **Entity Framework Core** do trwałego zapisu danych.                 

Encje w systemie i relacje między nimi:                                                                        

**Na dole pliku zamieszono screeny z diagramami**



---

##API (CRUD)

Projekt zawiera kontroler ** API** dla encji **Product**.

Dostępne endpointy:

- `GET /api/products` – pobranie listy produktów
- `GET /api/products/{id}` – pobranie produktu po ID
- `POST /api/products` – dodanie produktu (Administrator)
- `PUT /api/products/{id}` – edycja produktu (Administrator)
- `DELETE /api/products/{id}` – usunięcie produktu (Administrator)

API korzysta z tej samej bazy danych oraz warstwy Entity Framework co aplikacja MVC.

---

## Autorzy 

- Wojciech Pruchnicki
- Szymon Rogowski

## Digramy bazy danych

![Diagram 1]("diagramy_bd/1screen.png")
![Diagram 2]("diagramy_bd/2screen.png")
![Diagram 3]("diagramy_bd/3screen.png")


