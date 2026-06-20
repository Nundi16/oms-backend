# AuthEvents SignalR Hub – Frontend Integration Guide

## 1. Hub overview

| Property | Value |
| --- | --- |
| Endpoint | **`/hubs/auth-events`** (relatív az API base URL-hez) |
| Dev API base | `http://localhost:5243` (HTTP) vagy `https://localhost:7134` (HTTPS) |
| Transport | SignalR (WebSockets preferált, automata fallback Server-Sent Events / Long Polling) |
| Auth | **Kötelező** FusionAuth JWT (`[Authorize]` a hubon) |
| Üzenet típusa | szerver → kliens, **csak a bejelentkezett userednek** (per-user fan-out) |

A hub jelenleg nem fogad parancsot a kliensektől, csak push-ol. Ha a token érvénytelen / hiányzik, a connect 401-gyel elesik.

## 2. Üzenet szerződés

A szerver egyetlen kliens-metódust hív, amit fel kell iratkozni rá: **`AuthEvent`**.

A payload JSON-ként (a SignalR camelCase-re konvertálja a property neveket):

```json
{
  "type": "RoleChanged",
  "userId": "c1a2b3c4-5678-9abc-def0-123456789abc",
  "source": "user.registration.update",
  "occurredAt": "2025-01-15T12:34:56.789+00:00"
}
```

Mezők:

| Mező | Típus | Jelentés |
| --- | --- | --- |
| `type` | `string` | Szemantikus esemény típus. Jelenleg **mindig** `"RoleChanged"`. |
| `userId` | `string` (GUID) | A FusionAuth `sub` (= a beérkező user id). Mindig a saját felhasználódé lesz, mert a szerver per-user routol. |
| `source` | `string` | A FusionAuth nyers event típus (`user.registration.create` / `update` / `delete`). |
| `occurredAt` | `string` (ISO 8601, UTC offset) | Szerver oldali időbélyeg. |

Új típusok (`PasswordChanged`, `Locked`, …) később jöhetnek **anélkül**, hogy a `type` az új értékkel megjelenne — a kliens legyen elnézõ az ismeretlen `type` értékkel.

## 3. Auth — hogyan kerül be a token

A WebSocket upgrade nem tud `Authorization` header-t küldeni, ezért a SignalR az `access_token` query paramétert használja. A `.NET` oldal explicit kezeli ezt a `/hubs/*` prefixre, így ha JavaScript klienst használsz, csak az `accessTokenFactory`-t kell megadni:

```typescript
import {
  HubConnectionBuilder,
  HttpTransportType,
  LogLevel,
  HubConnectionState,
} from "@microsoft/signalr";

const connection = new HubConnectionBuilder()
  .withUrl(`${API_BASE_URL}/hubs/auth-events`, {
	// A SignalR betölti a tokent minden negotiate / websocket connectre.
	accessTokenFactory: () => oidc.getAccessToken(),
	// Opcionális: ha tudod, hogy WebSocket lesz, ezzel kihagyhatod a negotiate-et.
	// transport: HttpTransportType.WebSockets,
	// skipNegotiation: true,
  })
  .withAutomaticReconnect([0, 2_000, 5_000, 10_000, 30_000])
  .configureLogging(LogLevel.Information)
  .build();
```

> Az `accessTokenFactory` minden új connect / reconnect előtt lefut, így ha közben frissítettél tokent (silent refresh), az új token kerül a query-be.

## 4. Életciklus + minta implementáció (React/TS)

```typescript
useEffect(() => {
  let cancelled = false;

  connection.on("AuthEvent", (n: AuthEventNotification) => {
	// 1. Minden AuthEvent után csendben frissíts access tokent,
	//    hogy az új role-ok már az új JWT-ben legyenek.
	void refreshAccessTokenSilently().then(() => {
	  // 2. Mosd újra a permission-függő UI-t, query cache-t.
	  queryClient.invalidateQueries({ queryKey: ["me"] });
	  queryClient.invalidateQueries({ queryKey: ["permissions"] });

	  // 3. Opcionális UX: jelezd a usernek, hogy változtak a jogosultságai.
	  toast.info("Frissültek a jogosultságaid.");
	});
  });

  connection.onreconnecting((err) => console.warn("SignalR reconnecting", err));
  connection.onreconnected((id) => console.info("SignalR reconnected", id));
  connection.onclose((err) => console.warn("SignalR closed", err));

  (async () => {
	try {
	  if (connection.state === HubConnectionState.Disconnected) {
		await connection.start();
	  }
	} catch (err) {
	  if (!cancelled) console.error("SignalR start failed", err);
	}
  })();

  return () => {
	cancelled = true;
	connection.off("AuthEvent");
	void connection.stop();
  };
}, []);

type AuthEventNotification = {
  type: "RoleChanged" | string;
  userId: string;          // GUID
  source: string;          // user.registration.create | update | delete
  occurredAt: string;      // ISO 8601
};
```

A `connection`-t **singletonként** tartsd (pl. Zustand / Redux / React context), ne minden komponens-mountra építsd újra.

## 5. Token frissítés mintázat

`AuthEvent`-re a legbiztonságosabb a teljes silent refresh, mert a role-ok csak új access tokenben jelennek meg:

```typescript
async function refreshAccessTokenSilently() {
  // pl. oidc-client-ts:
  const user = await userManager.signinSilent();
  return user?.access_token;
}
```

Ha a silent refresh elbukik (pl. SSO session lejárt), normál login flow-ra kell visszaesni.

## 6. Reconnect / hibakezelés szabályok

| Esemény | Mit kell tenni |
| --- | --- |
| `onreconnecting` | UI badge `Reconnecting…`, semmilyen állapotot ne dobj el. |
| `onreconnected` | Egy biztonsági `invalidateQueries(["permissions"])`, mert lehet, hogy a reconnect alatt érkezett egy event. |
| `onclose` (nem reconnecting) | `withAutomaticReconnect` 0..30s után feladja → indíts újra `connection.start()`-tal, exponenciális backoff-fal. |
| `start()` 401 | A token lejárt – futtasd a silent refresh-et, majd próbálj újra. |
| `start()` egyéb hiba | Logold, próbáld újra, ne blokkold a UI-t. |

## 7. CORS / cookie-k

Dev környezetben a backend **`DevelopmentAll`** policy minden originből, hitelesítő adatokkal engedi a kapcsolatot, így `localhost:4200`, `localhost:4300` stb. mind működik. Production-ben whitelistre lesz szükség — a SignalR negotiate `withCredentials`-szal megy, ha a base URL és az oldal cross-origin.

## 8. Sanity check, ha nem érkezik üzenet

1. **Bejelentkezve vagy?** A hub `[Authorize]`. Connect logban ne legyen 401.
2. **A token tartalmaz `sub`-ot?** A szerver a `sub` claim szerint routol. Dekódold a JWT-t (jwt.io) — `sub` GUID kell legyen.
3. **Tényleg a saját userednek küldték az eseményt?** A FusionAuth `user.registration.update` az **érintett user**-re küld. Ha admin felületen másik user role-ját változtatod, a notification a **másik** userhez megy — nem az admin böngészőjébe.
4. **Backend log.** A `FusionAuthWebhookController` `LogInformation`-t ír: `"Forwarded FusionAuth user.registration.update to AuthEventsHub for user {UserId}"`. Ha ez nincs meg, a webhook szintű probléma (Docker hálózat / secret) — nem a SignalR oldal.
5. **Browser DevTools → Network → WS.** Látnod kell egy `/hubs/auth-events?id=...` WebSocket-et `101 Switching Protocols` státusszal.

## 9. Mit garantál (és mit nem) a szerver

- **Per-user.** Csak a hatás alatt álló user kapja meg az eventet. Más bejelentkezett userek nem.
- **At-most-once.** Ha a user éppen nincs csatlakozva, az értesítés elveszik (nincs offline queue). A reconnect után érdemes egy „refresh me” query-t futtatni.
- **Ordering.** Időrendi sorrend nem garantált, de a payload `occurredAt`-ot tartalmaz, ha sorrendezni szeretnéd.
- **Csak role-érintő FusionAuth eventek továbbítódnak** (`user.registration.create/update/delete`). Más event típus 200 OK-val nyugtázódik, de nem érkezik kliens értesítés.

## 10. TL;DR copy-paste sablon

```typescript
const connection = new HubConnectionBuilder()
  .withUrl(`${API_BASE_URL}/hubs/auth-events`, {
	accessTokenFactory: () => getAccessToken(),
  })
  .withAutomaticReconnect()
  .build();

connection.on("AuthEvent", async (n) => {
  await refreshAccessTokenSilently();
  queryClient.invalidateQueries({ queryKey: ["me"] });
});

await connection.start();
```

Ennyi kell ahhoz, hogy a role-változás után a frontend automatikusan újrarajzolódjon. Ha bármelyik részhez (pl. Angular, Vue, vanilla JS kliens) kell konkrét snippet, szólj.
