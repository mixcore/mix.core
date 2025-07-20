# Best Practices for Building Websites with Mix CMS

This guide outlines a structured, best-practice workflow for planning, building, and developing websites using the Mix CMS platform. By following these phases, your team can leverage the full potential of the platform, from AI-driven content creation to deep custom development.

This process is designed to align with the specialized documentation provided in this directory.

---

## The Development Workflow: A Phased Approach

### Phase 1: Planning and Foundation

**Goal:** Define the project's vision, scope, and structure. Understand the core capabilities of Mix CMS.

**Audience:** Project Managers, AI Agents, Lead Developers.

**Process:**
1.  **Define Project Requirements:** Clearly outline the website's purpose, target audience, key features, and overall goals.
2.  **Understand the Platform:** Before building, review the **`mix-ai-agent.md`** guide to understand the fundamental concepts and architecture of Mix CMS. This knowledge is crucial for effective planning and for knowing what the AI can build out-of-the-box.
3.  **Create a Sitemap and Feature List:** Map out the website's pages, user flows, and required functionalities.

**Key Document:**
- **[mix-ai-agent.md](./mix-ai-agent.md)**: Start here to grasp core concepts.

---

### Phase 2: AI-Powered Site Construction

**Goal:** Rapidly build the core website structure, pages, and content using the AI assistant and MCP (Model-driven Content Protocol) tools.

**Audience:** AI Agents, Content Creators.

**Process:**
1.  **Follow Step-by-Step Workflows:** Use the detailed workflows in **`mix-ai-agent.md`** to perform tasks like creating databases, defining content structures, and generating pages.
2.  **Utilize MCP Commands:** Execute MCP commands to interact with the CMS programmatically. This is the primary method for AI-driven development.
3.  **Troubleshoot and Refine:** Use the troubleshooting guide within the AI agent documentation to resolve common issues.

**Key Document:**
- **[mix-ai-agent.md](./mix-ai-agent.md)**: Your complete guide for building and content creation.

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

## Summary Workflow

1.  **Plan & Understand:** Read `mix-ai-agent.md` to understand the platform's concepts.
2.  **Build with AI:** Use `mix-ai-agent.md` to construct the site.
3.  **Customize with Code:** Use `developer-guide.md` for custom C# development.
4.  **Refer & Verify:** Use `mcp-tools-reference.md` throughout the process for quick lookups.
