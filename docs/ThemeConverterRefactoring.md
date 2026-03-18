# Theme Converter Refactoring - Architecture Simplifiée

## Vue d'ensemble

Le `ThemeConverter` a été refactorisé pour suivre une architecture simple et claire basée sur la séparation des responsabilités :
- **Résolution** (quel brush ?) → `IThemeResolver`
- **Transformation** (comment le modifier ?) → `IThemeBrushService`

## Architecture Finale

```
┌─────────────────────┐
│  ThemeConverter     │  Orchestrateur léger
└──────────┬──────────┘
           │
           ├─→ IThemeResolver.Resolve()      → Retourne le brush "brut"
           │                                    (QUEL brush utiliser ?)
           │
           └─→ IThemeBrushService.GetBrush()  → Applique les transformations
                                                 (opacity, contrast, darken, lighten)
```

## Composants

### 1. **IThemeBrushService** (existant)
- **Location**: `src\MyNet.Avalonia.Theme\Palettes\IThemeBrushService.cs`
- **Responsabilité**: Transformation des brushes (opacity, contrast, darken, lighten)
- **Méthodes**:
  - `GetBrush(string path, ...)` - Récupère un brush par chemin de ressource
  - `GetBrush(IBrush brush, ...)` - Transforme un brush existant
  - `GetOpacity(string? opacityKey)` - Résout les valeurs d'opacité

### 2. **IThemeResolver** (nouveau - simplifié)
- **Location**: `src\MyNet.Avalonia.Theme\Palettes\IThemeResolver.cs`
- **Implémentation**: `ThemeResolver`
- **Responsabilité**: Résoudre les rôles ET les contextes (unified)
- **Méthode unique**:
  ```csharp
  IBrush? Resolve(
      ThemeRole? role,           // null si c'est un context
      ThemeContext? context,     // null si c'est un role
      string? brushKey,          // pour contexts (passé via values)
      IBrush? customBrush,       // pour roles (passé via values)
      IBrush? foreground,        // pour contrast
      Control? control           // pour inherited foreground
  );
  ```

### 3. **ThemeConverter** (simplifié)
- **Location**: `src\MyNet.Avalonia.Theme\Converters\Internals\ThemeConverter.cs`
- **Responsabilité**: Orchestrer la conversion en 2 étapes
  1. Résoudre le brush (via `IThemeResolver`)
  2. Appliquer les transformations (via `IThemeBrushService`)
- **Dépendances**:
  - `IThemeBrushService` - pour les transformations
  - `IThemeResolver` - pour la résolution

### 4. **ThemeBrushParameters** (unifié)
- **Location**: `src\MyNet.Avalonia.Theme\Converters\Internals\ThemeConverter.cs`
- **Un seul type de paramètres** pour tous les cas :
  ```csharp
  public record ThemeBrushParameters(
      string? BrushKey,     // Pour les contextes (passé depuis values)
      string? Opacity,      // Opacité à appliquer
      bool Contrast,        // Utiliser le contraste
      double? Darken,       // Facteur d'assombrissement
      double? Lighten       // Facteur d'éclaircissement
  );
  ```

## Simplifications Réalisées

✅ **Un seul type de paramètres** (plus de hiérarchie `ThemeRoleParameters` / `ThemeContextParameters`)  
✅ **Un seul resolver** avec une seule méthode (plus de `IThemeRoleResolver` / `IThemeContextResolver`)  
✅ **Séparation claire** : résolution vs transformation  
✅ **Resolver sans transformation** : retourne des brushes "bruts"  
✅ **BrushKey dans values** : pas dans tous les constructeurs de paramètres  

## Avantages

✅ Architecture ultra-simplifiée  
✅ Responsabilités claires : resolver = "QUEL brush ?", service = "COMMENT le transformer ?"  
✅ Testabilité : mock facile de `IThemeResolver` et `IThemeBrushService`  
✅ Réutilisabilité : le resolver peut être utilisé ailleurs  
✅ Maintenabilité : moins de types, moins de code, plus clair  
✅ Pas de duplication de logique  

## Utilisation

Le `ThemeConverter` peut être utilisé de deux façons :

1. **Instance par défaut** (backward compatible):
```csharp
ThemeConverter.Default
```

2. **Instance personnalisée avec DI** (pour les tests):
```csharp
var converter = new ThemeConverter(brushService, resolver);
```

## Markup Extensions

Les extensions XAML ont été mises à jour pour utiliser le nouveau `ThemeBrushParameters` unifié :

- **ForegroundExtension** : `BrushKey = null` (binding foreground)
- **ThemeBrushExtension** : `BrushKey = null` (binding direct)
- **ThemeContextExtension** : `BrushKey = Path` (clé de ressource)
- **ThemeRoleExtension** : `BrushKey = null` (résolution par rôle)

## Migration Notes

- ✅ Tout le code existant continue de fonctionner (backward compatible)
- ✅ `ThemeBrushParameters` remplace `ThemeRoleParameters` et `ThemeContextParameters`
- ✅ Le converter maintient le même comportement de conversion
- ✅ Architecture 3x plus simple qu'avant
