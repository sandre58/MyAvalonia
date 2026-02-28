# MyNet.Avalonia.PerfTest

Application de test de performance pour Avalonia avec le thème Fluent.

## Structure du projet

Le projet est divisé en trois parties :

- **MyNet.Avalonia.PerfTest** : Bibliothèque principale contenant l'application et tous les ViewModels/Views
- **MyNet.Avalonia.PerfTest.Desktop** : Point d'entrée pour l'application Desktop (Windows, macOS, Linux)
- **MyNet.Avalonia.PerfTest.Browser** : Point d'entrée pour l'application WebAssembly (navigateur) - **⚠️ Nécessite l'installation du workload wasm-tools**

### Installation du workload Browser (optionnel)

Pour utiliser le projet Browser, installez le workload WebAssembly :

```bash
dotnet workload install wasm-tools
```

Puis réajoutez le projet à la solution :

```bash
dotnet sln MyAvalonia.sln add "demos\MyNet.Avalonia.PerfTest.Browser\MyNet.Avalonia.PerfTest.Browser.csproj"
```

## Objectif

Cette application permet de tester les performances de navigation dans une application Avalonia utilisant uniquement le thème **Fluent** de base, sans thème personnalisé. L'objectif est de déterminer si les problèmes de performance observés sont liés au thème personnalisé ou à Avalonia lui-même.

## Utilisation

### Exécution Desktop

```bash
cd demos/MyNet.Avalonia.PerfTest.Desktop
dotnet run
```

ou en mode Release pour des mesures plus précises :

```bash
cd demos/MyNet.Avalonia.PerfTest.Desktop
dotnet run -c Release
```

### Exécution Browser (WebAssembly)

```bash
cd demos/MyNet.Avalonia.PerfTest.Browser
dotnet run
```

Puis ouvrez votre navigateur à l'adresse indiquée (généralement `http://localhost:5000`)

## Caractéristiques

### Pages de test

1. **Home** - Page d'accueil avec les instructions
2. **DataGrid** - 500 éléments avec 10 colonnes pour tester les performances de grille
3. **Complex Layout** - 20 sections avec 15 items chacune (300 éléments au total) pour tester les layouts complexes
4. **List** - 1000 éléments dans une liste virtualisée pour tester le scrolling
5. **Forms** - Formulaire complexe avec de nombreux contrôles interactifs

### Performance Monitor

L'application inclut un moniteur de performance intégré qui affiche :
- **Temps de navigation** : Mesure le temps nécessaire pour charger et afficher une nouvelle page
- **Page actuelle** : Affiche la page actuellement chargée
- **Historique des performances** : Panneau latéral montrant les 20 dernières navigations avec temps et mémoire

Le moniteur effectue un garbage collection avant chaque navigation pour obtenir des mesures plus précises.

## Dépendances

- **Avalonia** : Framework UI
- **Avalonia.Themes.Fluent** : Thème Fluent de base (pas de thème personnalisé)
- **MyNet.Avalonia** : Bibliothèque de base (sans le thème MyNet)
- **ReactiveUI** : Pour le pattern MVVM

## Notes de performance

### Optimisations appliquées

- Utilisation de `CompiledBindings` par défaut
- Virtualisation des listes (ListBox utilise la virtualisation par défaut)
- Garbage collection forcé avant chaque navigation pour des mesures cohérentes
- Délai de 50ms après la navigation pour s'assurer que le rendu est terminé

### Métriques à surveiller

- **Navigation Home** : Devrait être très rapide (< 50ms)
- **Navigation DataGrid** : Peut prendre plus de temps en raison du nombre d'éléments (100-300ms)
- **Navigation Complex Layout** : Temps variable selon la complexité du rendu (200-500ms)
- **Navigation List** : Devrait être rapide grâce à la virtualisation (< 100ms)
- **Navigation Forms** : Temps modéré (50-150ms)

### Différences Desktop vs Browser

Les performances peuvent varier entre Desktop et Browser :
- **Desktop** : Généralement plus rapide, accès direct au système
- **Browser** : Peut être plus lent en raison de WebAssembly et du JavaScript

## Comparaison avec le thème personnalisé

Utilisez cette application comme base de comparaison pour identifier si les problèmes de performance viennent :
- Du thème personnalisé (MyNet.Avalonia.Theme)
- Des contrôles personnalisés
- De l'architecture de l'application
- D'Avalonia lui-même
- De la plateforme (Desktop vs Browser)

