# Крестики-нолики 

## Правила

- создание новой игры,
- ходы двух игроков,
- фиксация победы или ничьей.
- размер поля и условия победы задаются через переменные окружения

## Описание

- Архитектурные решения: REST, Clean Architeccture, SOLID, DDD, Result pattern, Controllers, Idempotent Filter, RFC 7807
- Тестирование: xUnit, Moq, Testcontainers

## API

### Эндпоинт игр `POST api/v1/games`
Request:
```json
{
 // No request body
}
```
Response:
```json
"id": "int"
```

### Эндпоинт игр `GET api/v1/games/{gameId:int}`

Endpoint: `POST /locations`

Request:
```json
{
 // No request body
}
```
Response:
```json
{
    "id": "int",
    "size": "int",
    "winLength": "int",
    "nextSymbol": "int",
    "winner": "int",
    "status": "int",
    "createdAt": "DateTime",
    "version": "int",
    "moves": []
}
```

### Эндпоинт игр `POST api/v1/games/{gameId:int}/moves`
Request:
```json
{
 // No request body
}
```
Response:
```json
"id": "long"
```