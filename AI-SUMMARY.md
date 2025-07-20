# Cline Instruction Guide

## Methods to Provide Instructions

### 1. Global Rules
- Location: `C:/Users/SE Manager/OneDrive/Documents/Cline/Rules/custom_instructions.md`
- Applies to all projects
- Example content:
```markdown
- use ; when run multi commands instead of &&
- must read all prompt before execute 
- do update or create AI-SUMMARY.md file if not existing
```

### 2. Project-Specific Rules
- Location: `.cursor/rules/*.mdc` files in project root
- Follows specific format with description and globs
- Example (dotnet-core.mdc):
```markdown
description: .NET Development Rules
globs: 
alwaysApply: true

# .NET Development Rules
- Write concise, idiomatic C# code
- Follow ASP.NET Core conventions
- Use PascalCase for class names
- Implement proper error handling
```

### 3. Direct Commands
- Provide instructions directly in chat interface
- Format: `<task>instruction</task>`
- Example:
```xml
<task>
Update the controller to use async/await pattern
</task>
```

## Best Practices
1. Keep instructions clear and specific
2. For .NET projects, follow conventions in `dotnet-core.mdc`
3. Document complex instructions in `AI-SUMMARY.md`
4. Use markdown formatting for readability

## Instruction Types
- **Code Style**: Naming, formatting, patterns
- **Project Structure**: File organization, conventions  
- **Workflow**: Build, test, deployment processes
- **Domain-Specific**: Business rules, validations

## Verification
After providing instructions:
1. Check `AI-SUMMARY.md` is updated
2. Verify rules files are properly formatted
3. Test instructions are being followed
