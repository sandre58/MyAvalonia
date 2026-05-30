# MyNet.Avalonia Source Generator Guidelines

# Goals

Source generators should improve:

- developer productivity
- compile-time safety
- discoverability
- maintainability
- consistency
- performance

Generators should eliminate repetitive code while preserving explicit APIs.

Generated code should feel native to Avalonia.

---

# Architectural Philosophy

Generators are tools that support the framework.

They must not become a second framework hidden behind the public API.

Prefer:

- generated boilerplate
- generated registrations
- generated metadata
- generated helpers

Avoid:

- generated magic behavior
- hidden runtime infrastructure
- surprising code generation

Developers should understand what is generated.

---

# Incremental Generators Only

All generators must implement:

```csharp
IIncrementalGenerator
```

Avoid legacy generators.

---

# Deterministic Generation

Generated output must always be deterministic.

The same source code must always produce identical generated files.

Avoid:

- unstable ordering
- random identifiers
- environment-dependent generation

---

# Generated Code Quality

Generated code is production code.

Generated code must:

- compile without warnings
- support nullable reference types
- support trimming
- support NativeAOT
- remain readable
- remain debuggable

Generated code should follow all MyNet conventions.

---

# Generated File Naming

Generated file names should be predictable.

Examples:

```text
PersonViewModel.Localization.g.cs
GridControl.Metadata.g.cs
DialogRegistry.g.cs
```

Avoid anonymous or hash-based file names.

---

# Generated Member Naming

Generated members should:

- be explicit
- avoid collisions
- remain stable over time

Avoid generated names that expose Roslyn implementation details.

---

# Avalonia Property Generation

Generators may be used to reduce property boilerplate.

Generated properties should:

- expose AvaloniaProperty fields
- preserve Avalonia conventions
- remain inspectable by developers

Generated code must never hide property behavior.

---

# Localization Generation

Generators should be preferred over reflection-based localization systems.

Prefer generating:

- translation accessors
- strongly typed resources
- localized bindings
- localization metadata

Avoid string-based lookup APIs whenever possible.

---

# Control Registration

Generators may be used to generate:

- control registries
- dialog registrations
- view registrations
- navigation mappings

Prefer generated registration over runtime scanning.

Avoid assembly scanning when a compile-time alternative exists.

---

# MVVM Support

Generators may assist MVVM development.

Examples:

- command generation
- property generation
- localization generation
- validation generation

Generated APIs should remain compatible with standard MVVM patterns.

---

# Reflection Policy

Avoid runtime reflection whenever a compile-time alternative exists.

Prefer:

- generated metadata
- generated registries
- generated mappings
- generated accessors

Reflection should never be required in performance-sensitive UI paths.

---

# Diagnostics

Generators must provide meaningful diagnostics.

Diagnostics should:

- explain the problem
- explain how to fix it
- identify the affected symbol

Avoid silent failures.

---

# Diagnostics Categories

Diagnostics should be grouped into categories such as:

- Usage
- Configuration
- Localization
- MVVM
- Performance

---

# Generated Registries

Generated registries should be preferred over runtime discovery.

Examples:

- dialog registries
- view registries
- control registries
- localization registries

Registries should:

- be immutable
- be thread-safe
- support fast lookups

---

# Design-Time Support

Generated code should support:

- IDE tooling
- IntelliSense
- XAML authoring
- debugging

Generators must not degrade the developer experience.

---

# Thread Safety

Generators must be thread-safe.

Avoid:

- mutable static state
- shared mutable caches

Incremental transforms must remain deterministic.

---

# AOT and Trimming

Generated infrastructure must support:

- trimming
- NativeAOT
- linker friendliness

Avoid generated code that introduces hidden reflection requirements.

---

# XAML Integration

Generators should integrate naturally with XAML.

Prefer:

- strongly typed APIs
- generated bindings
- generated resource accessors

Avoid requiring custom XAML syntax when standard Avalonia mechanisms are sufficient.

---

# Public API Design

Generated APIs should:

- look handwritten
- follow Avalonia conventions
- be easy to discover
- be easy to debug

Consumers should not need to understand the generator implementation.

---

# Testing

All generators must have dedicated tests.

Tests should verify:

- generated code
- diagnostics
- incremental behavior
- edge cases

---

# Snapshot Testing

Snapshot tests should validate:

- generated structure
- naming consistency
- nullable correctness
- deterministic output

---

# Long-Term Goals

The long-term generator strategy is:

- reduce boilerplate
- improve compile-time safety
- improve XAML developer experience
- reduce runtime discovery
- improve localization support
- improve MVVM productivity
- support trimming and NativeAOT
- keep generated code simple and predictable