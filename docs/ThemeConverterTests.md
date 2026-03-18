# Examples de Tests pour l'Architecture Simplifiée

## Tests Unitaires pour IThemeResolver

```csharp
using Moq;
using Xunit;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Palettes;

public class ThemeResolverTests
{
    private readonly Mock<IThemeBrushService> _mockBrushService;
    private readonly ThemeResolver _resolver;

    public ThemeResolverTests()
    {
        _mockBrushService = new Mock<IThemeBrushService>();
        _resolver = new ThemeResolver(_mockBrushService.Object);
    }

    [Fact]
    public void Resolve_WithRoleDefault_ReturnsCustomBrush()
    {
        // Arrange
        var customBrush = new SolidColorBrush(Colors.Red);

        // Act
        var result = _resolver.Resolve(
            role: ThemeRole.Default,
            context: null,
            brushKey: null,
            customBrush: customBrush,
            foreground: null,
            control: null
        );

        // Assert
        Assert.Equal(customBrush, result);
    }

    [Fact]
    public void Resolve_WithRolePrimary_CallsBrushService()
    {
        // Arrange
        var expectedBrush = new SolidColorBrush(Colors.Blue);
        _mockBrushService
            .Setup(x => x.GetBrush("Primary"))
            .Returns(expectedBrush);

        // Act
        var result = _resolver.Resolve(
            role: ThemeRole.Primary,
            context: null,
            brushKey: null,
            customBrush: null,
            foreground: null,
            control: null
        );

        // Assert
        Assert.Equal(expectedBrush, result);
        _mockBrushService.Verify(x => x.GetBrush("Primary"), Times.Once);
    }

    [Fact]
    public void Resolve_WithContextDefault_ReturnsBrushFromKey()
    {
        // Arrange
        var expectedBrush = new SolidColorBrush(Colors.Green);
        _mockBrushService
            .Setup(x => x.GetBrush("Primary.Background"))
            .Returns(expectedBrush);

        // Act
        var result = _resolver.Resolve(
            role: null,
            context: ThemeContext.Default,
            brushKey: "Primary.Background",
            customBrush: null,
            foreground: null,
            control: null
        );

        // Assert
        Assert.Equal(expectedBrush, result);
        _mockBrushService.Verify(x => x.GetBrush("Primary.Background"), Times.Once);
    }

    [Fact]
    public void Resolve_WithContextContrast_ResolvesForegroundAndAppliesOpacity()
    {
        // Arrange
        var foregroundBrush = new SolidColorBrush(Colors.White);
        var expectedBrush = new SolidColorBrush(Colors.White) { Opacity = 0.8 };
        
        _mockBrushService
            .Setup(x => x.GetOpacity("Primary.Background"))
            .Returns(0.8);
        _mockBrushService
            .Setup(x => x.GetBrush(foregroundBrush, "0.8"))
            .Returns(expectedBrush);

        // Act
        var result = _resolver.Resolve(
            role: null,
            context: ThemeContext.Contrast,
            brushKey: "Primary.Background",
            customBrush: null,
            foreground: foregroundBrush,
            control: null
        );

        // Assert
        Assert.NotNull(result);
        _mockBrushService.Verify(x => x.GetOpacity("Primary.Background"), Times.Once);
        _mockBrushService.Verify(x => x.GetBrush(foregroundBrush, "0.8"), Times.Once);
    }

    [Fact]
    public void Resolve_WithNullParameters_ReturnsCustomBrush()
    {
        // Arrange
        var customBrush = new SolidColorBrush(Colors.Yellow);

        // Act
        var result = _resolver.Resolve(
            role: null,
            context: null,
            brushKey: null,
            customBrush: customBrush,
            foreground: null,
            control: null
        );

        // Assert
        Assert.Equal(customBrush, result);
    }
}
```

## Tests d'Intégration pour ThemeConverter

```csharp
using Moq;
using Xunit;
using Avalonia;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Converters.Internals;
using MyNet.Avalonia.Theme.Palettes;

public class ThemeConverterIntegrationTests
{
    private readonly Mock<IThemeBrushService> _mockBrushService;
    private readonly Mock<IThemeResolver> _mockResolver;
    private readonly ThemeConverter _converter;

    public ThemeConverterIntegrationTests()
    {
        _mockBrushService = new Mock<IThemeBrushService>();
        _mockResolver = new Mock<IThemeResolver>();
        _converter = new ThemeConverter(_mockBrushService.Object, _mockResolver.Object);
    }

    [Fact]
    public void Convert_WithThemeRole_ResolvesAndTransforms()
    {
        // Arrange
        var rawBrush = new SolidColorBrush(Colors.Blue);
        var transformedBrush = new SolidColorBrush(Colors.Blue) { Opacity = 0.8 };
        var parameters = new ThemeBrushParameters(null, "0.8", false, null, null);

        _mockResolver
            .Setup(x => x.Resolve(ThemeRole.Primary, null, null, null, null, null))
            .Returns(rawBrush);
        _mockBrushService
            .Setup(x => x.GetBrush(rawBrush, "0.8", false, null, null))
            .Returns(transformedBrush);

        var values = new object[] { ThemeRole.Primary };

        // Act
        var result = _converter.Convert(values, typeof(IBrush), parameters, null);

        // Assert
        Assert.Equal(transformedBrush, result);
        _mockResolver.Verify(x => x.Resolve(ThemeRole.Primary, null, null, null, null, null), Times.Once);
        _mockBrushService.Verify(x => x.GetBrush(rawBrush, "0.8", false, null, null), Times.Once);
    }

    [Fact]
    public void Convert_WithThemeContext_ResolvesAndTransforms()
    {
        // Arrange
        var rawBrush = new SolidColorBrush(Colors.Green);
        var transformedBrush = new SolidColorBrush(Colors.Green);
        var parameters = new ThemeBrushParameters("Primary.Background", null, false, 0.1, null);

        _mockResolver
            .Setup(x => x.Resolve(null, ThemeContext.Default, "Primary.Background", null, null, null))
            .Returns(rawBrush);
        _mockBrushService
            .Setup(x => x.GetBrush(rawBrush, null, false, 0.1, null))
            .Returns(transformedBrush);

        var values = new object[] { ThemeContext.Default };

        // Act
        var result = _converter.Convert(values, typeof(IBrush), parameters, null);

        // Assert
        Assert.Equal(transformedBrush, result);
        _mockResolver.Verify(x => x.Resolve(null, ThemeContext.Default, "Primary.Background", null, null, null), Times.Once);
        _mockBrushService.Verify(x => x.GetBrush(rawBrush, null, false, 0.1, null), Times.Once);
    }

    [Fact]
    public void Convert_WithDirectBrush_OnlyTransforms()
    {
        // Arrange
        var directBrush = new SolidColorBrush(Colors.Red);
        var transformedBrush = new SolidColorBrush(Colors.Red) { Opacity = 0.5 };
        var parameters = new ThemeBrushParameters(null, "0.5", true, null, 0.2);

        _mockBrushService
            .Setup(x => x.GetBrush(directBrush, "0.5", true, null, 0.2))
            .Returns(transformedBrush);

        var values = new object[] { directBrush };

        // Act
        var result = _converter.Convert(values, typeof(IBrush), parameters, null);

        // Assert
        Assert.Equal(transformedBrush, result);
        _mockResolver.Verify(x => x.Resolve(It.IsAny<ThemeRole?>(), It.IsAny<ThemeContext?>(), It.IsAny<string>(), It.IsAny<IBrush>(), It.IsAny<IBrush>(), It.IsAny<Control>()), Times.Never);
        _mockBrushService.Verify(x => x.GetBrush(directBrush, "0.5", true, null, 0.2), Times.Once);
    }

    [Fact]
    public void Convert_WithEmptyValues_ReturnsUnsetValue()
    {
        // Arrange
        var parameters = new ThemeBrushParameters(null, null, false, null, null);
        var values = new object[0];

        // Act
        var result = _converter.Convert(values, typeof(IBrush), parameters, null);

        // Assert
        Assert.Equal(AvaloniaProperty.UnsetValue, result);
    }

    [Fact]
    public void Convert_WithNullBrushFromResolver_ReturnsUnsetValue()
    {
        // Arrange
        var parameters = new ThemeBrushParameters(null, null, false, null, null);
        var values = new object[] { ThemeRole.Custom };

        _mockResolver
            .Setup(x => x.Resolve(ThemeRole.Custom, null, null, null, null, null))
            .Returns((IBrush)null);

        // Act
        var result = _converter.Convert(values, typeof(IBrush), parameters, null);

        // Assert
        Assert.Equal(AvaloniaProperty.UnsetValue, result);
        _mockBrushService.Verify(x => x.GetBrush(It.IsAny<IBrush>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<double?>(), It.IsAny<double?>()), Times.Never);
    }
}
```

## Tests de Scénarios Réels

```csharp
public class RealWorldScenarioTests
{
    [Fact]
    public void Scenario_PrimaryButtonWithOpacity()
    {
        // Simule : <Button Background="{my:ThemeRole Primary, Opacity=0.9}" />
        var mockBrushService = new Mock<IThemeBrushService>();
        var mockResolver = new Mock<IThemeResolver>();
        var converter = new ThemeConverter(mockBrushService.Object, mockResolver.Object);

        var primaryBrush = new SolidColorBrush(Colors.Blue);
        var finalBrush = new SolidColorBrush(Colors.Blue) { Opacity = 0.9 };

        mockResolver
            .Setup(x => x.Resolve(ThemeRole.Primary, null, null, null, null, null))
            .Returns(primaryBrush);
        mockBrushService
            .Setup(x => x.GetBrush(primaryBrush, "0.9", false, null, null))
            .Returns(finalBrush);

        var values = new object[] { ThemeRole.Primary };
        var parameters = new ThemeBrushParameters(null, "0.9", false, null, null);

        var result = converter.Convert(values, typeof(IBrush), parameters, null);

        Assert.Equal(finalBrush, result);
    }

    [Fact]
    public void Scenario_ContrastTextOnColoredBackground()
    {
        // Simule : <TextBlock Foreground="{my:ThemeContext Primary.Background, Context=Contrast}" />
        var mockBrushService = new Mock<IThemeBrushService>();
        var mockResolver = new Mock<IThemeResolver>();
        var converter = new ThemeConverter(mockBrushService.Object, mockResolver.Object);

        var contrastBrush = new SolidColorBrush(Colors.White);
        var finalBrush = new SolidColorBrush(Colors.White);

        mockResolver
            .Setup(x => x.Resolve(null, ThemeContext.Contrast, "Primary.Background", null, It.IsAny<IBrush>(), It.IsAny<Control>()))
            .Returns(contrastBrush);
        mockBrushService
            .Setup(x => x.GetBrush(contrastBrush, null, false, null, null))
            .Returns(finalBrush);

        var values = new object[] { ThemeContext.Contrast, null, null };
        var parameters = new ThemeBrushParameters("Primary.Background", null, false, null, null);

        var result = converter.Convert(values, typeof(IBrush), parameters, null);

        Assert.Equal(finalBrush, result);
    }
}
```

## Conseils pour les Tests

### ✅ À Tester

1. **Résolution** : Vérifier que le bon brush est résolu selon le role/context
2. **Transformation** : Vérifier que les transformations sont appliquées correctement
3. **Séparation** : S'assurer que la résolution et la transformation sont bien séparées
4. **Edge Cases** : Null values, empty values, invalid parameters

### ❌ À Éviter

1. Ne pas tester l'implémentation interne de `IThemeBrushService` (c'est un mock)
2. Ne pas tester Avalonia framework (bindings, converters, etc.)
3. Ne pas tester les MarkupExtensions (ce sont des intégrations)

### 🎯 Stratégie

- **Tests unitaires** pour `ThemeResolver` (avec mock de `IThemeBrushService`)
- **Tests d'intégration** pour `ThemeConverter` (avec mocks des deux dépendances)
- **Tests de scénarios** pour valider les cas d'usage réels
