# OrderManagementApi — Demo (Intentionally Insecure)

Demo ASP.NET Core Web API for the course "Demo: Performing Threat Modeling on an API".
This project is intentionally insecure to surface common threats.

## How to run

From the project folder:

```powershell
cd "ProjectFolderPath"
dotnet run
```

The app uses an in-memory database and prints two demo tokens on startup. You can also obtain tokens via the login endpoint.

## Endpoints

1. Login (get token)

POST /api/auth/login
Body: { "email": "user@example.com", "password": "password" }

Response: { "token": "..." }

2. Create order (authenticated user)

POST /api/orders
Headers: Authorization: Bearer <token>
Body: { "totalAmount": 12.34, "status": "Optional" }

3. Get order by id (INTENTIONALLY INSECURE: no ownership check)

GET /api/orders/{id}
Headers: Authorization: Bearer <token>

4. List my orders (filters by current user)

GET /api/orders
Headers: Authorization: Bearer <token>

5. Admin: list all orders (INTENTIONALLY INSECURE: only checks authentication)

GET /api/admin/orders
Headers: Authorization: Bearer <token>

6. Admin: delete order

DELETE /api/admin/orders/{id}
Headers: Authorization: Bearer <token>

## Example curl flows

Login as user:

```bash
curl -X POST https://localhost:5001/api/auth/login -H "Content-Type: application/json" -d '{"email":"user@example.com","password":"password"}'
```

Login as admin:

```bash
curl -X POST https://localhost:5001/api/auth/login -H "Content-Type: application/json" -d '{"email":"admin@example.com","password":"password"}'
```

Create order (replace <token>):

```bash
curl -X POST https://localhost:5001/api/orders -H "Content-Type: application/json" -H "Authorization: Bearer <token>" -d '{"totalAmount":99.95}'
```

Get an order by id (INTENTIONALLY INSECURE):

```bash
curl https://localhost:5001/api/orders/<order-id> -H "Authorization: Bearer <token>"
```

Get all orders as admin:

```bash
curl https://localhost:5001/api/admin/orders -H "Authorization: Bearer <token>"
```


## Notes (intentionally insecure)

- Plain-text passwords in DB (no hashing).
- Token store is in-memory, tokens have no expiry.
- No authorization checks on sensitive operations (broken access control examples).
- Detailed errors and stack traces enabled in development.
- No rate limiting, CORS or security headers.

Use this project for threat modeling exercises only — do not run in production.
