# Lembrol 2.0 Backend
## How to run

```bash
  dotnet ef migrations add InitialCreate
  ef database update
  dotnet build
  dotnet run
```

## Commits and Branches guidelines

- https://www.conventionalcommits.org/en/v1.0.0/

```yaml
feat: A new feature for the user, not a new feature for build script.
fix: Bug fix for the user, not a fix to a build script.
docs: Changes to the documentation.
style: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc).
refactor: A code change that neither fixes a bug nor adds a feature.
perf: A code change that improves performance.
test: Adding missing tests or correcting existing tests.
chore: Changes to the build process or auxiliary tools and libraries such as documentation generation.
build: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm).
ci: Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs).
```