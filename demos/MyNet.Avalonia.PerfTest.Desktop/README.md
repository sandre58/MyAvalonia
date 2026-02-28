# MyNet.Avalonia.PerfTest.Desktop

Point d'entrée Desktop pour l'application de test de performance Avalonia.

## Lancement

```bash
dotnet run
```

ou en mode Release pour des mesures plus précises :

```bash
dotnet run -c Release
```

## Depuis Visual Studio

1. Définir **MyNet.Avalonia.PerfTest.Desktop** comme projet de démarrage
2. Appuyez sur F5 pour lancer

## Depuis Visual Studio Code

1. Ouvrez le dossier racine du workspace
2. Appuyez sur F5 ou utilisez le menu Debug
3. Sélectionnez "Launch PerfTest Desktop"

## Plateformes supportées

- Windows (x64, ARM64)
- macOS (x64, ARM64)
- Linux (x64, ARM64)

## Notes

Pour des mesures de performance précises :
- Utilisez toujours le mode **Release**
- Fermez les autres applications
- Lancez plusieurs fois pour éviter les variations de JIT
- Attendez quelques secondes avant de commencer les tests
