---
name: clean-code
description: 'Ensure code follows Clean Code principles for readability, maintainability, and enterprise-grade quality.'
---

# Clean Code Principles

Your task is to ensure code follows Clean Code principles for readability,
maintainability, and enterprise-grade quality. Review the code and suggest
improvements that make it cleaner, simpler, and easier to understand.

## Naming

- Use intention-revealing names — names should answer why something exists, what it does, and how it is used
- Avoid disinformation — don't use names that mislead (e.g., `accountList` for a non-list type)
- Make meaningful distinctions — avoid noise words like `data`, `info`, `manager`, `processor` unless genuinely descriptive
- Use pronounceable names — avoid abbreviations that require mental translation
- Use searchable names — single-letter variables and numeric constants are not searchable; use named constants
- Avoid encodings — don't prefix member variables (`m_`, `s_`) or type names (`IFoo`) unless following a project convention
- Class names should be nouns or noun phrases: `Customer`, `WikiPage`, `AddressParser`
- Method names should be verbs or verb phrases: `PostPayment`, `DeletePage`, `SaveAsync`
- Pick one word per concept and stick to it — don't use `fetch`, `retrieve`, and `get` interchangeably

## Functions / Methods

- Functions should do one thing — if a function does more than one thing, extract into smaller functions
- Keep functions small — aim for under 20 lines; anything longer is a candidate for extraction
- One level of abstraction per function — don't mix high-level logic with low-level details in the same method
- Prefer fewer arguments — zero (niladic) is ideal; one (monadic) is good; two (dyadic) is acceptable; three or more (triadic) should be avoided
- Avoid flag arguments — a boolean parameter signals the function does two things; split it into two functions
- Avoid output arguments — functions should return values, not modify arguments
- No side effects — a function should do what its name says and nothing else
- Command-Query Separation (CQS) — a function either changes state (command) or returns a value (query), never both
- Don't Repeat Yourself (DRY) — duplication is the root of all evil; extract reusable logic

## Classes

- Classes should be small — measured by responsibilities, not lines; a class should have only one reason to change (SRP)
- Single Responsibility Principle (SRP) — one class, one purpose
- Open/Closed Principle (OCP) — open for extension, closed for modification; use polymorphism and abstractions
- Liskov Substitution Principle (LSP) — subtypes must be substitutable for their base types without altering correctness
- Interface Segregation Principle (ISP) — many specific interfaces are better than one general-purpose interface
- Dependency Inversion Principle (DIP) — depend on abstractions, not concretions; inject dependencies
- High cohesion — instance variables should be used by most methods in the class
- Low coupling — minimize dependencies between classes; use interfaces and dependency injection

## Comments

- Comments should not be needed — good code is self-documenting; a comment is a failure to express in code
- Legal comments are acceptable — copyright, license headers
- Informative comments are acceptable — explaining complex regex or algorithm intent
- Never leave commented-out code — use source control; delete dead code
- Avoid redundant comments — `// increments i` above `i++` adds no value
- Don't use comments to compensate for bad code — refactor instead
- Use XML doc comments (`<summary>`, `<param>`, `<returns>`) for all public APIs

## Error Handling

- Use exceptions rather than return codes
- Create informative error messages — provide context about what failed and why
- Define exception classes based on caller needs — wrap third-party exceptions at the boundary
- Don't return `null` — return empty collections, `Optional<T>`, or throw; returning null forces null checks everywhere
- Don't pass `null` — if a method receives null, it should fail fast with `ArgumentNullException`
- Use specific exception types — never throw or catch `Exception` directly
- Fail fast — validate inputs at the entry point and throw early

## Objects and Data Structures

- Hide internal structure — expose behavior, not data (Law of Demeter)
- Law of Demeter — a method should only call methods of: the object itself, its parameters, objects it creates, and its direct dependencies (no `a.B.C.DoSomething()`)
- Data Transfer Objects (DTOs) — pure data structures with no behavior; appropriate for API boundaries
- Active Records — DTOs with simple query methods; keep business logic in separate domain objects
- Prefer immutability — use `record`, `readonly`, and `init` properties where possible

## Boundaries

- Isolate third-party code — wrap external libraries behind interfaces; never let them bleed into your domain
- Use the Adapter pattern — translate between external APIs and your internal abstractions
- Write boundary tests — learn and document third-party behavior through tests

## Unit Tests

- Follow the Three Laws of TDD:
  1. Write no production code until you have a failing test
  2. Write only enough test code to fail
  3. Write only enough production code to pass the failing test
- Keep tests clean — test code is as important as production code
- One assert per test — each test verifies one concept
- FIRST principle:
  - **Fast** — tests must run quickly
  - **Independent** — tests must not depend on each other
  - **Repeatable** — tests must be reproducible in any environment
  - **Self-Validating** — tests must return a boolean result (pass/fail)
  - **Timely** — tests should be written just before the production code

## Code Smells to Eliminate

- **Long Method** — extract into smaller, focused methods
- **Large Class** — split into multiple classes with clear responsibilities
- **Long Parameter List** — introduce a parameter object or builder
- **Divergent Change** — one class changed for multiple different reasons; split it
- **Shotgun Surgery** — one change causes modifications in many classes; consolidate
- **Feature Envy** — a method uses data from another class more than its own; move it
- **Data Clumps** — groups of variables that always appear together; create a class
- **Primitive Obsession** — replace primitives with domain types (e.g., `Email`, `Money`, `PhoneNumber`)
- **Switch Statements** — replace with polymorphism
- **Parallel Inheritance Hierarchies** — refactor to remove duplication
- **Lazy Class** — a class that doesn't do enough; collapse or inline it
- **Speculative Generality** — code written "just in case"; delete unused abstractions
- **Temporary Field** — an instance variable only set sometimes; extract into a class or method parameter
- **Message Chains** — Law of Demeter violation; introduce intermediary
- **Middle Man** — a class that only delegates; remove it or inline
- **Inappropriate Intimacy** — two classes know too much about each other; reduce coupling
- **Duplicate Code** — extract into a shared method, base class, or utility

## Formatting

- Vertical openness — use blank lines to separate conceptually distinct blocks
- Vertical density — related lines of code should be close together
- Vertical ordering — caller functions should appear before callees (newspaper metaphor)
- Horizontal formatting — keep lines under 120 characters
- Indentation — always use consistent indentation; never align code with extra spaces
- Team rules — a team agrees on a style guide and everyone follows it (enforced by `.editorconfig` and analyzers)

## Enterprise Patterns

- Repository Pattern — isolate data access logic behind an interface; swap implementations without touching business logic
- Unit of Work — group database operations into a single transaction boundary
- CQRS — separate read models from write models for scalability and clarity
- Domain Events — communicate state changes across bounded contexts without tight coupling
- Result Pattern — return `Result<T>` from operations instead of throwing exceptions in expected failure paths
- Guard Clauses — validate preconditions at the top of a method and return/throw early to reduce nesting
- Value Objects — encapsulate primitive values with validation and equality semantics (e.g., `Email`, `Money`)
- Aggregate Root — define clear ownership boundaries in the domain; only access child entities through the root

When reviewing code, identify violations of these principles and provide specific,
actionable refactoring suggestions that improve readability, reduce complexity,
and increase long-term maintainability.
