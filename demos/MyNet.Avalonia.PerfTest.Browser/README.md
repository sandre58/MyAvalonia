# MyNet.Avalonia.PerfTest.Browser

Point d'entrée WebAssembly pour l'application de test de performance Avalonia.

## ⚠️ Prérequis

Ce projet nécessite l'installation du workload WebAssembly pour .NET :

```bash
dotnet workload install wasm-tools
```

Une fois installé, réajoutez le projet à la solution :

```bash
dotnet sln MyAvalonia.sln add "demos\MyNet.Avalonia.PerfTest.Browser\MyNet.Avalonia.PerfTest.Browser.csproj"
```

## Lancement

```bash
dotnet run
```

L'application sera accessible à l'adresse `http://localhost:5000` (ou le port indiqué dans la console).

## Première compilation

⚠️ La première compilation peut prendre **plusieurs minutes** car :
- Le SDK WebAssembly doit télécharger les outils natifs
- La compilation en WebAssembly est plus longue
- Les optimisations natives sont activées (`WasmBuildNative=true`)

## Publication

Pour publier l'application pour un déploiement web :

```bash
dotnet publish -c Release -o publish
```

Les fichiers seront dans le dossier `publish/wwwroot` et peuvent être déployés sur n'importe quel serveur web statique.

## Navigateurs supportés

L'application fonctionne sur tous les navigateurs modernes supportant WebAssembly :
- Chrome/Edge (recommandé)
- Firefox
- Safari 14+

## Notes de performance

Les performances WebAssembly sont généralement **inférieures** aux performances Desktop :
- Overhead de la couche WebAssembly
- Limitations du navigateur
- Pas d'accès direct au GPU (selon le navigateur)

Utilisez cette version principalement pour :
- Tester la compatibilité cross-platform
- Déployer facilement sans installation
- Comparer les performances WebAssembly vs Desktop

## Debugging

Pour déboguer dans le navigateur :
1. Lancez l'application avec `dotnet run`
2. Ouvrez les DevTools (F12)
3. Les logs sont dans la console
4. Utilisez le Profiler du navigateur pour des analyses détaillées
