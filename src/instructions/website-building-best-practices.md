# Best Practices for Building Websites with Mix CMS

This guide outlines a structured, best-practice workflow for planning, building, and developing websites using the Mix CMS platform. By following these phases, your team can leverage the full potential of the platform, from **Mix AI Agent-driven content creation** to deep custom development.

## 🤖 MIX AI AGENT METHODOLOGY
**This process is designed for Mix AI Agents** with these core principles:

1. **MCP Server First**: Always check Mix MCP Server capabilities before suggesting alternatives
2. **Agent Identity**: Maintain Mix AI Agent character throughout all phases
3. **Mix.Mcp.Services Primary**: Use MCP tools as the primary interface
4. **Document MCP Usage**: Record which tools were effective in each phase

This process is designed to align with the specialized documentation provided in this directory.

---

## The Development Workflow: A Phased Approach

### Phase 1: Planning and Foundation (Mix AI Agent)

**Goal:** Define the project's vision, scope, and structure. Understand the core capabilities of Mix CMS and MCP Server.

**Audience:** Project Managers, **Mix AI Agents**, Lead Developers.

**Mix AI Agent Process:**
1.  **Define Project Requirements:** Clearly outline the website's purpose, target audience, key features, and overall goals.
2.  **Check MCP Server Capabilities:** Before planning, verify which tasks can be handled by Mix.Mcp.Services
3.  **Understand the Platform:** Review the **Mix AI Agent documentation** to understand fundamental concepts and MCP architecture.
4.  **Create a Sitemap and Feature List:** Map out the website's pages, user flows, and required functionalities.
5.  **Verify MCP Support**: Ensure planned features align with available MCP tools

**Key Document:**
- **[AI-AGENT-START-HERE.md](./AI-AGENT-START-HERE.md)**: Primary entry point for Mix AI Agents

---

### Phase 2: Mix AI Agent-Powered Site Construction

**Goal:** Rapidly build the core website structure, pages, and content using the **Mix AI Agent** and MCP (Model Context Protocol) tools.

**Audience:** **Mix AI Agents**, Content Creators.

**Mix AI Agent Process:**
1.  **Follow MCP-First Protocol:** Always check MCP Server support before suggesting alternatives
2.  **Use Step-by-Step Workflows:** Leverage detailed workflows in **AI Workflows Complete Guide**
3.  **Execute MCP Commands:** Use Mix.Mcp.Services as primary interface for CMS operations
4.  **Handle Dynamic Content:** Create MixDb tables for repetitive content using `CreateDatabaseFromPrompt`
5.  **Document MCP Usage:** Record which MCP tools were used and their effectiveness
6.  **Maintain Agent Identity:** Stay in Mix AI Agent character throughout

**Key Document:**
- **[ai-workflows-complete.md](./workflows/ai-workflows-complete.md)**: Complete guide for Mix AI Agent workflows

---

### Phase 3: Custom Development and Extension

**Goal:** Implement custom business logic, advanced features, and unique designs that go beyond the standard capabilities of the AI tools.

**Audience:** C# Developers.

**Process:**
1.  **Consult the Developer Guide:** The **`developer-guide.md`** is the essential resource for all custom development.
2.  **Implement Custom Logic:** Follow the documented `.NET 9` and `C# 13` patterns to write clean, maintainable, and performant code.
3.  **Develop Custom UI:** Build custom user experiences using Razor Pages.
4.  **Extend AI Capabilities:** If the existing MCP tools are insufficient, follow the guide to create new MCP tools for the AI to use.
5.  **Adhere to Code Style:** Maintain consistency across the project by following the established code style guidelines.

**Key Document:**
- **[developer-guide.md](./developer-guide.md)**: The primary technical guide for developers.

---

### Phase 4: Ongoing Reference and Refinement

**Goal:** Maintain development velocity and accuracy by using quick references for specific tool details.

**Audience:** All Roles (AI Agents and Developers).

**Process:**
- When you need to verify a specific parameter, check a response format, or see a usage example for an MCP tool, use the **`mcp-tools-reference.md`**. This is faster than searching through the comprehensive guides.

**Key Document:**
- **[mcp-tools-reference.md](./mcp-tools-reference.md)**: Your go-to for quick, consolidated tool information.

---

## Key Best Practices

### Dynamic Content Over Static Hard-coding

**Principle:** Always use MixDb tables for repetitive or list-based content instead of hard-coding data directly into templates.

**When to Apply:**
- Product listings
- Team member profiles
- Testimonials or reviews
- Service offerings (navigation, feature lists, etc.)
- Service offerings
- Portfolio items
- FAQ entries
- Any content that might need frequent updates

**Benefits:**
- **Maintainability:** Update content through the CMS without touching code
- **Scalability:** Easily add/remove items without template changes
- **Consistency:** Ensure uniform data structure across all items
- **Future-proofing:** Enable features like search, filtering, and pagination

**Implementation:**
1. Use `CreateDatabaseFromPrompt` to define the data structure
2. Use `CreateManyMixDbData` to populate initial data
3. Use `SearchMixDbRequestModel` in templates to fetch and display data dynamically

**Example:** Instead of hard-coding team members in a module template, create a "team_members" table with fields for name, position, bio, and photo_url, then render them dynamically.

---

## Summary Workflow

1.  **Plan & Understand:** Read `mix-ai-agent.md` to understand the platform's concepts.
2.  **Build with AI:** Use `mix-ai-agent.md` to construct the site.
3.  **Customize with Code:** Use `developer-guide.md` for custom C# development.
4.  **Refer & Verify:** Use `mcp-tools-reference.md` throughout the process for quick lookups.
