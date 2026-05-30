# MyAvalonia Architecture

# Overview

MyAvalonia is a modular Avalonia UI framework providing:

- reusable controls
- behaviors
- dialogs
- navigation infrastructure
- theming support
- localization integration
- validation infrastructure
- UI composition helpers
- developer productivity components

The framework is designed for:

- desktop applications
- cross-platform applications
- business applications
- reusable component libraries

The primary goals are:

- consistency
- reusability
- extensibility
- maintainability
- developer experience
- performance
- MVVM friendliness

---

# Architectural Principles

## Avalonia First

The framework is built specifically for Avalonia.

Components should embrace Avalonia concepts:

- StyledProperty
- DirectProperty
- DataTemplates
- ControlTheme
- Behaviors
- Attached Properties
- Compiled Bindings

Avoid introducing abstractions that hide Avalonia unnecessarily.

---

## MVVM First

All components must be usable within MVVM applications.

Controls should:

- support binding
- avoid code-behind business logic
- expose state through properties
- expose actions through commands when appropriate

The framework must remain ViewModel-friendly.

---

## Control-Centric Design

Controls are the primary building blocks.

Controls should:

- expose clear APIs
- be easily stylable
- remain composable
- avoid hidden behaviors

A control should solve a specific UI problem.

Avoid "god controls".

---

## Composition Over Inheritance

Prefer:

- composition
- behaviors
- attached properties
- templates

Avoid deep inheritance hierarchies.

Inheritance should only be used when it provides clear value.

---

## Framework Independence

UI controls should remain independent from application-specific concepts.

Avoid dependencies on:

- business logic
- persistence
- networking
- domain models

Controls should remain reusable across projects.

---

# Module Organization

## MyNet.Avalonia

Contains:

- common infrastructure
- commands
- converters
- markup extensions
- common abstractions
- localization integration
- translation extensions
- culture-aware controls
- localized bindings

Dependencies should remain minimal.

---

## MyNet.Avalonia.Controls

Contains:

- attached properties
- behaviors
- reusable controls
- editors
- selectors
- navigation controls
- collection controls
- data presentation controls

Controls should remain self-contained.

---

## MyNet.Avalonia.Themes

Contains:

- attached properties
- markup extensions
- themes
- styles
- resources
- icons
- visual assets

Visual concerns belong here.

---

## MyNet.Avalonia.Extended

Contains:

- dialog infrastructure
- dialog services
- modal workflows
- dialog hosts
- navigation infrastructure
- busy infrastructure
- theming infrastructure

Implements higher-level UI services from MyNet.UI.

---

# Property Design

## Styled Properties First

Use StyledProperty whenever styling or binding is expected.

Prefer:

- StyledProperty
- AvaloniaProperty metadata
- validation callbacks

Avoid unnecessary DirectProperty usage.

---

## Direct Properties

Use DirectProperty only when:

- performance is critical
- a backing field is required
- the property is not intended for styling

---

## Attached Properties

Attached properties should:

- solve cross-cutting UI concerns
- remain discoverable
- avoid hidden behavior

Avoid creating attached properties that act as service locators.

---

# Control API Design

Control APIs should be:

- explicit
- discoverable
- strongly typed
- designer friendly

Avoid:

- magic strings
- ambiguous flags
- hidden side effects

---

# Styling and Themes

Every control should be stylable.

Controls should:

- expose theme resources
- support ControlTheme
- support application-level customization

Visual customization should not require subclassing.

---

# Performance

Performance is important but should not compromise usability.

Avoid:

- unnecessary visual tree traversal
- repeated template lookups
- excessive event subscriptions
- unnecessary allocations

Optimize:

- item controls
- virtualization scenarios
- bindings
- rendering paths

---

# Behaviors

Behaviors should:

- be reusable
- be composable
- solve a single concern

Avoid large multi-purpose behaviors.

---

# Services

Services should:

- use dependency injection
- expose interfaces
- avoid static mutable state

UI services should remain testable.

---

# Localization

Localization must be supported throughout the framework.

Controls should:

- react to culture changes
- support runtime language switching
- avoid cached localized strings

Localization should feel automatic from the consumer perspective.

---

# Validation

Validation infrastructure should:

- integrate with Avalonia validation mechanisms
- support IDataErrorInfo
- support INotifyDataErrorInfo

Validation should remain MVVM-friendly.

---

# Testing Strategy

The framework prioritizes:

- control testing
- visual behavior testing
- property testing
- interaction testing

Public controls should have dedicated tests.

---

# Public API Stability

Public APIs are long-term contracts.

Breaking changes should be minimized.

Controls should evolve through:

- additive APIs
- extensibility points
- optional behaviors

Avoid redesigning public APIs without strong justification.

---

# Long-Term Goals

The framework aims to provide:

- a consistent Avalonia ecosystem
- highly reusable controls
- modern MVVM support
- strong localization support
- customizable themes
- high-performance UI components
- excellent developer experience