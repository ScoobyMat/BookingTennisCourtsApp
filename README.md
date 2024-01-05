# BookingTennisCourts

## Opis Projektu

**BookingTennisCourts** to aplikacja umożliwiająca rezerwację kortów tenisowych. Użytkownicy mogą przeglądać dostępność kortów, a zalogowani użytkownicy mają możliwość dokonywania rezerwacji.

## Funkcje

- Rezerwacja kortu na określony dzień i godzinę.
- Możliwość dodawania nowych kortów przez administratora.
- Panel administracyjny umożliwiający podgląd wszystkich rezerwacji i zarządzanie kortami.

## Technologie

- ASP.NET Core
- Entity Framework Core
- C#
- HTML, CSS, JavaScript

## Instalacja

1. Sklonuj repozytorium na swój lokalny komputer.
2. Uruchom Visual Studio lub inne środowisko programistyczne.
3. Otwórz projekt i zainstaluj zależności.
4. Uruchom aplikację.

```bash
dotnet run
```
## Konfiguracja

1. W pliku `appsettings.json` skonfiguruj połączenie do bazy danych.

```json
"ConnectionStrings": {
  "DefaultConnection": "YourConnectionString"
}
```
## Użycie

### Rezerwacja Kortu

1. Zaloguj się do aplikacji, jeśli nie masz konta, zarejestruj się.
2. Przejdź do sekcji "Rezerwacje".
3. Wybierz kort, datę i godzinę, a następnie kliknij przycisk "Zarezerwuj".
4. Twoja rezerwacja zostanie dodana do systemu, a ty zostaniesz przekierowany do listy swoich rezerwacji.

### Dodawanie Nowych Kortów (Dla Administratorów)

1. Zaloguj się jako administrator.
2. Przejdź do panelu administracyjnego.
3. Wybierz sekcję "Korty"
4. Dodaj nowy kort, podając wymagane informacje, takie jak nazwa kortu i dostępność.

### Panel Administracyjny

Administratorzy mają dodatkowe uprawnienia do zarządzania kortami i rezerwacjami. Mają podgląd na rezerwacjie wszystkich użytkowników, mogą dodawać nowe korty i podejmować inne działania administracyjne.




