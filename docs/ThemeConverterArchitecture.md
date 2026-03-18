# Architecture Simplifiée du Theme Converter

## 🎯 Principe Fondamental

**Séparation claire des responsabilités :**
- `IThemeResolver` → **QUEL** brush utiliser ? (résolution)
- `IThemeBrushService` → **COMMENT** le transformer ? (transformations)

## 📊 Flux de Conversion

```
XAML Markup Extension
         ↓
    [values] + [ThemeBrushParameters]
         ↓
   ThemeConverter.Convert()
         ↓
         ├─→ IThemeResolver.Resolve(role?, context?, brushKey?, customBrush?, ...)
         │   └─→ Retourne IBrush "brut" (sans transformations)
         │
         └─→ IThemeBrushService.GetBrush(rawBrush, opacity, contrast, darken, lighten)
             └─→ Retourne IBrush transformé (final)
```

## 🏗️ Structure

### Avant (complexe)
```
ThemeConverter
    ├─ IThemeBrushService
    ├─ IThemeRoleResolver
    │   └─ gère roles + transformations ❌
    └─ IThemeContextResolver
        └─ gère contexts + transformations ❌

ThemeBrushParameters
    ├─ ThemeRoleParameters
    └─ ThemeContextParameters
```

### Après (simplifié)
```
ThemeConverter
    ├─ IThemeBrushService → transformations uniquement ✅
    └─ IThemeResolver → résolution roles ET contexts ✅

ThemeBrushParameters (unifié) ✅
```

## 📝 Exemples d'Usage

### 1. Résolution d'un Role
```csharp
// values[0] = ThemeRole.Primary
// values[1] = customBrush (optionnel)
// values[2] = control
// values[3] = foreground

var rawBrush = resolver.Resolve(
    role: ThemeRole.Primary,
    context: null,
    brushKey: null,
    customBrush: values[1] as IBrush,
    foreground: values[3] as IBrush,
    control: values[2] as Control
);

var finalBrush = brushService.GetBrush(rawBrush, "0.8", contrast: false, darken: 0.1, lighten: null);
```

### 2. Résolution d'un Context
```csharp
// values[0] = ThemeContext.Contrast
// values[1] = control
// values[2] = foreground
// parameters.BrushKey = "Primary.Background"

var rawBrush = resolver.Resolve(
    role: null,
    context: ThemeContext.Contrast,
    brushKey: "Primary.Background",
    customBrush: null,
    foreground: values[2] as IBrush,
    control: values[1] as Control
);

var finalBrush = brushService.GetBrush(rawBrush, null, contrast: true, darken: null, lighten: null);
```

### 3. Brush Direct
```csharp
// values[0] = IBrush
// Pas de résolution nécessaire

var rawBrush = values[0] as IBrush;
var finalBrush = brushService.GetBrush(rawBrush, "0.5", contrast: false, darken: null, lighten: 0.2);
```

## 🔍 Responsabilités Détaillées

### IThemeResolver (resolution logique)
- ✅ Déterminer quel brush utiliser selon le role
- ✅ Déterminer quel brush utiliser selon le context
- ✅ Résoudre les brushes "Contrast" (inherited foreground)
- ✅ Résoudre les brushes "Inverse"
- ✅ Retourner le brush "brut" (sans aucune transformation)
- ❌ NE PAS appliquer opacity, contrast, darken, lighten

### IThemeBrushService (transformations)
- ✅ Récupérer un brush par clé de ressource
- ✅ Appliquer l'opacité
- ✅ Appliquer le contraste (brush opposé pour l'accessibilité)
- ✅ Appliquer darken (assombrissement)
- ✅ Appliquer lighten (éclaircissement)
- ❌ NE PAS gérer la logique de résolution (roles/contexts)

### ThemeConverter (orchestration)
- ✅ Extraire les données de `values` selon le type (Role/Context/Brush)
- ✅ Appeler le resolver pour obtenir le brush brut
- ✅ Appeler le service pour transformer le brush
- ✅ Gérer les erreurs et retourner `UnsetValue` si nécessaire
- ❌ NE PAS contenir de logique métier

## 🧪 Testing

### Avant
```csharp
// Difficile de tester car couplage fort avec MyTheme.Current
```

### Après
```csharp
// Facile de tester avec des mocks
var mockResolver = new Mock<IThemeResolver>();
var mockBrushService = new Mock<IThemeBrushService>();

mockResolver.Setup(x => x.Resolve(...)).Returns(someBrush);
mockBrushService.Setup(x => x.GetBrush(...)).Returns(transformedBrush);

var converter = new ThemeConverter(mockBrushService.Object, mockResolver.Object);
var result = converter.Convert(...);
```

## 🎨 Types de Brushes Supportés

| Type | Résolution | Exemple |
|------|-----------|---------|
| **Direct Brush** | Aucune | `{my:ThemeBrush Foreground}` |
| **Role** | Par rôle | `{my:ThemeRole Primary}` |
| **Context** | Par contexte + clé | `{my:ThemeContext Primary.Background}` |
| **Foreground** | Binding TextElement | `{my:Foreground Self}` |

## 📦 Fichiers Modifiés

- ✅ `ThemeConverter.cs` - Simplifié avec 1 resolver
- ✅ `IThemeResolver.cs` - Interface unifiée
- ✅ `ThemeResolver.cs` - Implémentation unifiée
- ✅ `ThemeBrushParameters` - Type unifié
- ✅ Markup Extensions - Mise à jour signatures
- ❌ `IThemeRoleResolver.cs` - Supprimé
- ❌ `IThemeContextResolver.cs` - Supprimé
- ❌ `ThemeRoleResolver.cs` - Supprimé
- ❌ `ThemeContextResolver.cs` - Supprimé
- ❌ `ThemeRoleParameters` - Supprimé
- ❌ `ThemeContextParameters` - Supprimé

## ✨ Résultat

**Avant** : 9 fichiers, 3 interfaces, 3 implémentations, 3 types de paramètres  
**Après** : 5 fichiers, 2 interfaces, 1 implémentation, 1 type de paramètres  

**Réduction de complexité : ~45%** 🎉
