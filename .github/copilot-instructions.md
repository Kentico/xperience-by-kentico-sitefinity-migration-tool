Custom Instructions for GitHub Copilot
These instructions provide guidelines for generating code in this repository. Follow them strictly to ensure consistency, quality, and maintainability.

Adhere to SOLID Principles
All generated code must follow the SOLID principles:

Single Responsibility Principle (SRP): A class should have only one reason to change. Ensure each class or method handles a single responsibility.
Open-Closed Principle (OCP): Classes should be open for extension but closed for modification. Use interfaces and abstractions to extend behavior without altering existing code.
Liskov Substitution Principle (LSP): Derived classes must be substitutable for their base classes without altering the correctness of the program. Avoid breaking inheritance hierarchies.
Interface Segregation Principle (ISP): Clients should not be forced to depend on interfaces they do not use. Prefer multiple small interfaces over large ones.
Dependency Inversion Principle (DIP): High-level modules should not depend on low-level modules; both should depend on abstractions. Use dependency injection to decouple components.
When suggesting code, explicitly check and comment on how these principles are applied. For Dependency Inversion Principle (DIP), perform explicit checks for proper dependency injection usage, such as constructor injection, ensuring abstractions are injected rather than concrete implementations, and verifying that services are registered correctly in the DI container (e.g., using Microsoft.Extensions.DependencyInjection in .NET).

Apply .NET Code Analysis Rules and Standards
Generate code that complies with .NET code analysis rules as outlined in the .NET Fundamentals documentation. Aim for the "Recommended" analysis mode, where more rules are enabled as warnings for better code quality.

Key Rule Categories and Guidelines
Code Quality (CAxxxx): Prioritize security, performance, design, and reliability. Examples:
Performance: Use efficient methods like AsSpan instead of range-based indexers (CA1831).
Reliability: Avoid problematic patterns like stackalloc in loops (CA2014) or incorrect stream reads (CA2022).
Usage: Follow API best practices, e.g., rethrow exceptions to preserve stack details (CA2200).
Code Style (IDExxxx): Maintain consistent style for readability. Examples:
Require accessibility modifiers (IDE0040).
Enforce naming conventions, formatting, and indentation as per .editorconfig settings.
Additional Standards
Formatting: Enforce code style on build. Use consistent bracing, spacing.
Naming: Use descriptive, consistent names following PascalCase for types/methods, camelCase for parameters.
Performance: Avoid unnecessary allocations and optimize for efficiency.
Reliability: Ensure code is robust, handles errors gracefully, and avoids runtime issues.
Security: Follow secure coding practices to prevent vulnerabilities.
If code violates any rule, suggest fixes or refactor to comply. Reference specific rule IDs in comments if relevant.

Do not leave empty lines with whitespace characters only; just have the empty line without any whitespace characters.

Do not leave whitespace characters at the end of the line; just have the line without any whitespace characters at the end.

HTML Markup Guidelines
When creating HTML markup, ensure it is accessibility compliant and W3C compliant. Follow these practices:

Use semantic HTML elements (e.g., <header>, <nav>, <main>, <footer>) to improve structure and accessibility.
Include ARIA attributes where necessary (e.g., aria-label, role) for dynamic content or non-standard elements.
Ensure proper alt text for images (alt attribute), labels for form inputs, and keyboard navigation support.
Validate HTML against W3C standards using tools like the W3C Markup Validation Service.
Follow MDN Web Docs for HTML best practices.
Error Handling and Logging
Implement proper error handling and logging in all code:

Use structured exception handling with try-catch blocks, catching specific exceptions where possible instead of generic Exception.
Rethrow exceptions when appropriate to preserve stack traces (reference CA2200).
Integrate logging using libraries like Microsoft.Extensions.Logging or Serilog for .NET projects.
Log errors, warnings, and informational messages with context (e.g., user ID, request details) at appropriate levels.
Handle edge cases, null values, and invalid inputs gracefully to prevent crashes.
Follow .NET best practices for error handling.
Adopt Project Standards
Align generated code with standards used in the project:

Review existing codebase for patterns in naming, architecture, and tooling before suggesting changes.
Use project-specific configurations from files like .editorconfig, .csproj, or global.json.
Ensure compatibility with the project's target framework (e.g., .NET 8 or later).
Integrate with existing dependencies and avoid introducing breaking changes.
Assume most projects are hosted and deployed on Azure; prioritize Azure services and integrations where applicable.
Additional Best Practices for Efficiency, Accuracy, and Maintainability
To make coding easier, more efficient, and accurate, incorporate these guidelines:

Unit Testing: Always suggest accompanying unit tests using frameworks like xUnit or NUnit. Aim for high code coverage and test edge cases.
Documentation: Add XML comments for public members and inline comments for complex logic. Follow .NET documentation standards.
Performance Optimization: Profile code where applicable and use tools like BenchmarkDotNet for measurements.
Security Best Practices: Sanitize inputs, use HTTPS, and follow OWASP guidelines.
Version Control Integration: Use Git for source control. Suggest commit messages following Conventional Commits. Integrate with platforms like GitHub, GitLab, or Bitbucket for repositories, pull requests, and CI/CD pipelines (https://git-scm.com/docs; https://docs.github.com/; https://docs.gitlab.com/; https://bitbucket.org/product/guides).
Tooling: Leverage Visual Studio features like IntelliSense and extensions for productivity.
CSS and JavaScript Integration: If applicable, ensure CSS is modular and JavaScript is ES6+ compliant, following MDN docs (https://developer.mozilla.org/en-US/docs/Web/CSS and https://developer.mozilla.org/en-US/docs/Web/JavaScript).
API Design: For APIs, use RESTful principles and OpenAPI/Swagger for documentation.
Database Interactions: Use Entity Framework Core for SQL Server integrations, with proper query optimization.
Azure and Cloud: Since most projects are done using Azure, follow Azure best practices for deployment, CI/CD, storage, and services. Use Azure App Service for web hosting, Azure Functions for serverless, Azure SQL Database for data, and Azure DevOps for pipelines. Prioritize Azure-native tools and integrations (https://learn.microsoft.com/en-us/azure/; https://learn.microsoft.com/en-us/azure/app-service/; https://learn.microsoft.com/en-us/azure/devops/).
AI Integration: For AI features, use what's available in Azure, such as Azure AI Services (including Azure OpenAI, Azure Cognitive Services for vision/speech/text), Azure Machine Learning, or Microsoft Semantic Kernel for .NET integrations. Reference Azure AI documentation and avoid non-Azure alternatives unless specified (https://learn.microsoft.com/en-us/azure/ai-services/; https://learn.microsoft.com/en-us/azure/machine-learning/; https://learn.microsoft.com/en-us/semantic-kernel/).
Reference the project's resources and documentation URLs for consistency (see Resources and Documentation URLs for Grok Project Technologies for detailed links).

kentico xbyk umt documentation: https://docs.xperience.io/xperience-developing-apps/using-the-xperience-umt
kentico xbyk documentation: https://docs.xperience.io/xperience-developing-apps/using-the-xperience-by-kentico 