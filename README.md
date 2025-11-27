# System Zarządzania Biblioteką – aplikacja webowa

## Opis projektu

System Zarządzania Biblioteką to aplikacja webowa umożliwiająca zarządzanie biblioteką w trybie klient-serwer. System obsługuje różne role użytkowników i zapewnia pełną funkcjonalność w zakresie zarządzania książkami, wypożyczeniami oraz kontami użytkowników.

## Funkcjonalności

### System użytkowników i role

- Rejestracja i logowanie użytkowników
- Trzy role: **Czytelnik**, **Bibliotekarz**, **Administrator**
- Ograniczenie dostępu do danych zgodnie z rolą użytkownika
- Bezpieczne haszowanie haseł

### Zarządzanie książkami

- Przeglądanie listy książek z filtrowaniem, sortowaniem i wyszukiwaniem
- Dodawanie, edytowanie i usuwanie książek (dostępne dla Bibliotekarza/Administratora)
- Obsługa kopii książek i ich stanu dostępności

### Zarządzanie wypożyczeniami

- Rejestracja nowych wypożyczeń przez Bibliotekarza
- Monitorowanie statusu wypożyczeń przez Czytelnika (termin zwrotu, historia wypożyczeń)
- Rejestracja zwrotów książek

### Administracja danymi biblioteki

- Pełne zarządzanie danymi autorów, gatunków, oddziałów i regałów (Administrator)
- Walidacja danych wprowadzanych przez użytkowników
- Generowanie raportów książek z wykorzystaniem wzorca Strategy

### Interfejs użytkownika

- Responsywny frontend wykorzystujący HTML, CSS i JavaScript
- Intuicyjna nawigacja dostosowana do roli użytkownika

## Technologie

- **Backend**: ASP.NET Core MVC (.NET 6)
- **ORM**: Entity Framework Core
- **Baza danych**: PostgreSQL
- **Frontend**: HTML, CSS, JavaScript
- **Uwierzytelnianie**: Cookie Authentication
- **Haszowanie haseł**: BCrypt.Net-Next
- **Sesje**: Microsoft.AspNetCore.Session

## Wzorce architektoniczne

- **Strategy Pattern** - wykorzystany do generowania różnych typów raportów książek (np. książki według oddziału i roku, książki według oddziału i gatunku, suma stron według oddziału)
- **Factory Pattern** - wykorzystany do tworzenia odpowiednich strategii raportów

## Struktura projektu

```
BibliotekaOnline/
├── Controllers/          # Kontrolery MVC obsługujące logikę aplikacji
├── Models/              # Modele danych i ViewModele
├── Views/               # Widoki Razor
├── Services/            # Serwisy biznesowe (strategie raportów)
└── Context/             # DbContext dla Entity Framework Core
```

## Wymagania

- .NET 6.0 SDK
- PostgreSQL (wersja 12 lub nowsza)
- IDE obsługujące .NET (Visual Studio, Rider, VS Code)

## Instalacja i uruchomienie

1. **Sklonuj repozytorium** lub pobierz projekt

2. **Skonfiguruj bazę danych** w pliku `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=Localhost;Port=5432;DataBase=Biblioteka;User Id=postgres;Password=postgres"
}
```

3. **Zastosuj migracje** bazy danych:

```bash
dotnet ef database update
```

4. **Uruchom aplikację**:

```bash
dotnet run
```

5. Otwórz przeglądarkę i przejdź do adresu wyświetlonego w konsoli

## Konta użytkowników

Do testowania aplikacji dostępne są następujące konta:

- **Czytelnik**: Utwórz własne konto przez formularz rejestracji
- **Bibliotekarz**:
  - Login: `pwisniewski`
  - Hasło: `haslo789`
- **Administrator**:
  - Login: `admin`
  - Hasło: `zaq1@WSX`

## Efekt końcowy

Aplikacja umożliwia pełną obsługę biblioteki online, zapewnia bezpieczeństwo danych użytkowników oraz intuicyjne zarządzanie książkami i wypożyczeniami w zależności od roli użytkownika.
