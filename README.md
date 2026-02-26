# URLShortener API

API REST in ASP.NET Core per accorciare URL e reindirizzare verso l’indirizzo originale.

## Stack

- ASP.NET Core (`net10.0`)
- Entity Framework Core + SQLite
- OpenAPI (spec JSON in ambiente Development)
- DotNetEnv per caricare variabili da file `.env`

## Funzionalità

- Creazione short link tramite codice casuale da 6 caratteri alfanumerici
- Persistenza URL in database SQLite (`urls.db`)
- Redirect `302` verso l’URL originale a partire dal codice corto
- CORS configurabile verso frontend locale

## Struttura endpoint

Base URL locale (profilo HTTP):

- `http://localhost:5227`

Endpoint disponibili:

- `POST /URL` → crea uno short URL
- `GET /URL/{shortCode}` → esegue redirect verso URL originale

> Nota: il route prefix usa il nome del controller (`URLController`), quindi il path è `/URL`.

## Avvio progetto

Prerequisiti:

- .NET SDK 10

1. Ripristina pacchetti:

```bash
dotnet restore
```

2. (Opzionale) applica migration se il database non esiste:

```bash
dotnet ef database update
```

3. Avvia l’API:

```bash
dotnet run
```

L’app espone l’API su `http://localhost:5227` (configurazione in `Properties/launchSettings.json`).

## Variabili ambiente

Il progetto legge le variabili da `.env` all’avvio.

Esempio:

```env
FRONTEND_URL=http://127.0.0.1
FRONTEND_PORT=5173
```

Questi valori sono usati per la policy CORS consentita.

## Esempi utilizzo

### 1) Crea short URL

```bash
curl -X POST http://localhost:5227/URL \
  -H "Content-Type: application/json" \
  -d '{"originalUrl":"example.com"}'
```

Risposta attesa (esempio):

```json
{
  "id": 1,
  "shortURL": "aB3kPq",
  "originalURL": "https://example.com",
  "createdAt": "2026-02-26T10:15:30.0000000"
}
```

### 2) Apri short URL

```bash
curl -i http://localhost:5227/URL/aB3kPq
```

Risposta: redirect HTTP `302` con header `Location` verso l’URL originale.

## OpenAPI

In ambiente Development la specifica OpenAPI è disponibile su:

- `GET /openapi/v1.json`

## Database

Il database SQLite locale contiene la tabella `Urls` con campi:

- `Id` (PK autoincrement)
- `ShortURL`
- `OriginalURL`
- `CreatedAt`

## Note utili

- Il progetto non abilita il redirect automatico HTTP→HTTPS (`UseHttpsRedirection` è commentato).
- Se usi un frontend su porta diversa, aggiorna `FRONTEND_URL`/`FRONTEND_PORT` nel `.env`.
