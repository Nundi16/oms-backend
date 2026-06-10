Kövessük ezt a mappa struktúrát pls.

OMS-BACKEND/
├── src/                      # Forráskód (Projektek pl OMS.Infrastructure, OMS.Application)
├── docker/                   # Docker Images, egyéb
├── tests/                    # Automatizált tesztek
├── docs/                     # Dokumentáció
├── Directory.Build.props     # Közös MSBuild beállítások
├── Directory.Packages.props  # Központi csomag-verziókezelés (CPM)
├── WorkflowManagement.sln
├── .gitignore
└── README.md

## Docker Compose

A projekt egy `docker-compose.yaml` fájlt tartalmaz, amely a helyi fejlesztéshez
szükséges infrastruktúrát indítja el. A compose három szolgáltatást definiál:

| Szolgáltatás   | Container név                  | Leírás                                                              | Portok      |
| -------------- | ------------------------------ | ------------------------------------------------------------------ | ----------- |
| `postgres`     | `workflow-management-postgres` | Az alkalmazás fő PostgreSQL adatbázisa (`oms` adatbázis).          | `5432:5432` |
| `authdb`       | `oms-authdb`                   | A FusionAuth saját PostgreSQL adatbázisa (nincs kifelé publikálva). | –           |
| `fusionauth`   | `oms-fusionauth`               | FusionAuth identity provider, kickstart konfigurációval indítva.    | `9011:9011` |

A szolgáltatások a `app` Docker hálózaton kommunikálnak egymással, az adatok pedig
a következő named volume-okban perzisztálódnak: `postgres_data`, `authdb_data`,
`fusionauth_config`.

A FusionAuth saját image-ből épül (`docker/fusionauth.dockerfile`), amely a
`docker/fusionauth.kickstart.json` konfigurációt használja a tenantok,
alkalmazások, email sablonok és az admin felhasználó automatikus létrehozásához.

### Indítás és leállítás

```bash
# Az összes szolgáltatás indítása a háttérben (szükség esetén build-eli a FusionAuth image-et)
docker compose up -d --build

# Logok követése
docker compose logs -f fusionauth

# Állapot ellenőrzése
docker compose ps

# Leállítás (az adatok megmaradnak)
docker compose down

# Leállítás az adatok (volume-ok) törlésével
docker compose down -v
```

> A `postgres` és az `authdb` egyaránt a `postgres` / `postgres`
> felhasználó/jelszó párost használja. A `postgres` szolgáltatáshoz a host
> gépről a `localhost:5432` címen lehet csatlakozni (adatbázis: `oms`).

### FusionAuth bejelentkezés

A FusionAuth admin felület az alábbi címen érhető el, miután a konténerek
elindultak (az első indulás a kickstart lefutása miatt eltarthat egy percig):

- **URL:** http://localhost:9011
- **Felhasználó (e-mail):** `admin@email.invalid`
- **Jelszó:** `dZ0pRfcWXi2teWZTxsOWbNcHk9dbTuJu`

> Az admin hitelesítő adatok a `docker/fusionauth.kickstart.json` fájlban vannak
> definiálva (`initialAdminPassword`, illetve a regisztrált admin e-mail cím).