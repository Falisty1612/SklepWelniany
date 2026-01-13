# SklepWelniany

## Opis projektu

Aplikacja webowa typu **ASP.NET Core MVC** przedstawiająca prosty sklep internetowy z produktami. Projekt został wykonany zgodnie z wzorcem **MVC (Model–View–Controller)** oraz z wykorzystaniem **Entity Framework Core** i relacyjnej bazy danych **SQL Server**.

System umożliwia:

- przeglądanie produktów,
- filtrowanie produktów według kategorii,
- wyszukiwanie produktów,
- obsługę koszyka,
- składanie zamówień,
- autoryzację użytkowników (administrator / użytkownik),
- zarządzanie produktami przez administratora,
- dostęp do **REST API CRUD** dla głównej encji **Product**.

## Wymagania systemowe

- .NET 8 lub nowszy  
- SQL Server (LocalDB lub inna instancja SQL Server)  
- SQL Server Management Studio 2021  
- Visual Studio 2022 lub nowsze (projekt tworzony w Visual Studio 2025)

## Instalacja i uruchomienie

1. Sklonuj repozytorium z GitHub:
   ```
   https://github.com/Falisty1612/SklepWelniany
   ```

2. Otwórz projekt w Visual Studio.

3. Sprawdź plik `appsettings.json` i **zmień nazwę serwera bazy danych** w łańcuchu połączenia na lokalną instancję SQL Server:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SklepWelniany;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

4. W konsoli menedżera pakietów wykonaj migracje:
   ```
   Update-Database
   ```

5. Baza danych zostanie utworzona automatycznie.

6. Uruchom projekt z poziomu Visual Studio (F5).

## Konta testowe

### Administrator
- **Email:** `admin@localhost.com`  
- **Hasło:** `Admin@99`  

Administrator posiada uprawnienia do:
- dodawania produktów,
- edytowania produktów,
- usuwania produktów,
- zarządzania danymi w systemie.

### Użytkownik
- Konto może zostać utworzone poprzez formularz **Register** dostępny na stronie aplikacji.

Zwykły użytkownik może:
- przeglądać produkty,
- filtrować produkty według kategorii,
- wyszukiwać produkty,
- dodawać produkty do koszyka,
- składać zamówienia.

## Funkcjonalności aplikacji

### Produkty
- Wyświetlanie listy produktów.
- Filtrowanie po kategoriach.
- Wyszukiwanie produktów.
- Formularze: dodawanie, edycja, usuwanie (dostępne tylko dla administratora).

### Koszyk
- Dodawanie produktów do koszyka.
- Przeglądanie zawartości koszyka.
- Składanie zamówień na podstawie zawartości koszyka.

### Zamówienia
- Składanie zamówień przez użytkownika.
- Zapis zamówień do bazy danych.
- Powiązanie zamówień z użytkownikiem.

### Autoryzacja użytkowników
- Logowanie i rejestracja użytkowników.
- Rozróżnienie ról:
  - **Administrator**
  - **Zwykły użytkownik**

Dostęp do wybranych funkcji aplikacji jest ograniczony na podstawie roli użytkownika.

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
- mechanizmy uwierzytelniania i autoryzacji użytkowników.

## Struktura bazy danych

Aplikacja wykorzystuje **Entity Framework Core** do trwałego zapisu danych.

Encje w systemie:
- **User** – użytkownicy aplikacji,
- **Product** – produkty w sklepie,
- **Category** – kategorie produktów,
- **Order** – zamówienia użytkowników.

Relacje:
- Product → Category (wiele do jednego),
- Order → User,
- Order → Product.

## REST API (CRUD)

Projekt zawiera **kontroler API CRUD** dla encji **Product**.

Dostępne endpointy:

- `GET /api/products` – pobranie listy produktów  
- `GET /api/products/{id}` – pobranie produktu po ID  
- `POST /api/products` – dodanie nowego produktu (Administrator)  
- `PUT /api/products/{id}` – edycja produktu (Administrator)  
- `DELETE /api/products/{id}` – usunięcie produktu (Administrator)  

API korzysta z tej samej bazy danych oraz warstwy Entity Framework co aplikacja MVC.

---

Dokumentacja przygotowana na potrzeby projektu ASP.NET MVC.

