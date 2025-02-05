# Création de migrations

Ouvrir le "Package Manager Console" dans Visual Studio pour éxécuter les commandes:

Installer les outils de migrations 

```bash
dotnet tool install --global dotnet-ef --version 9.0.0

```
Si déjà installer mettre jours les outils de migrations 

```bash
dotnet tool update --global dotnet-ef --version 9.0.0

```

Vérifier la version installée 
```bash
dotnet ef --version

```

Ajouter une migration
```bash
dotnet ef migrations add init --verbose --project .\40-Data\Elysio.Data\Elysio.Data.csproj --startup-project .\20-API\Elysio.API\Elysio.API.csproj
```
