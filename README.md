# Accounting Program

## Opis
Aplikacja **Accounting Program** to proste narzędzie do zarządzania fakturami, stworzone z wykorzystaniem ASP.NET MVC 8.0. Umożliwia tworzenie, edycję, przeglądanie oraz usuwanie faktur wraz z ich szczegółowymi pozycjami.

## Funkcjonalności
- **Tworzenie faktur**: Użytkownicy mogą tworzyć nowe faktury, dodając niezbędne informacje takie jak dane klienta, datę wystawienia faktury oraz pozycje faktury.
- **Edycja faktur**: Faktury mogą być edytowane, co obejmuje zarówno dane główne faktury, jak i jej szczegółowe pozycje.
- **Przegląd faktur**: Użytkownicy mogą przeglądać listę wszystkich wystawionych faktur w systemie.
- **Usuwanie faktur**: Faktury mogą być usuwane z systemu, z dodatkowymi ograniczeniami dostępu dla tej funkcji.

## Technologie
Projekt został zrealizowany z wykorzystaniem:
- ASP.NET MVC 8.0
- Entity Framework Core 8.0 dla operacji na bazie danych
- MySQL jako system zarządzania bazą danych
- Bootstrap 4 dla frontendu

## Uruchamianie projektu
Aby uruchomić projekt lokalnie, wymagane jest środowisko .NET 8.0 oraz MySQL. Konfiguracja połączenia z bazą danych jest zarządzana przez plik `appsettings.json`, gdzie użytkownik może dostosować parametry połączenia zgodnie z lokalnym środowiskiem bazodanowym.
