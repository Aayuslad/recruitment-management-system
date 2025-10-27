# Contributing Guidelines

Thank you for contributing! Please follow these rules to keep our workflow clean and consistent.

---

## 📌 Branch Naming Convention

We use **prefix-based branch names**:

- `feat/<short-description>` → New features  
  Example: `feat/authentication-module`

- `fix/<short-description>` → Bug fixes  
  Example: `fix/login-crash`

- `chore/<short-description>` → Maintenance tasks (configs, dependencies, formatting, etc.)  
  Example: `chore/update-dotnet-sdk`

- `hotfix/<short-description>` → Urgent fixes in production  
  Example: `hotfix/critical-db-issue`

> ✅ Use **kebab-case** (lowercase with dashes) for the `<short-description>` part.  
> ❌ Don’t use spaces or uppercase letters in branch names.

---

## 🔀 Pull Request (PR) Rules

1. **Create a PR for every branch** – no direct commits to `main`.  
2. **PR Title Format:**  
   Use the same convention as branch type.  
   Example:  
   - `feat: add authentication module`  
   - `fix: resolve login crash`
3. **Description Must Include:**
   - A short summary of the change.
   - Related issue number (if any).
   - Steps to test the change.
4. **Reviews Required:**  
   - At least 1 reviewer approval before merging.
5. **Squash & Merge Only:**  
   Keep commit history clean by squashing commits when merging.

---

Happy coding! 🚀
