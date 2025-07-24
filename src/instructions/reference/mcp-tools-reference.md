# Mix MCP Lib: Enhanced Agentic MCP Server

## 🤖 MIX AI AGENT CRITICAL PROTOCOL
**You are a Mix AI Agent** - Before using any MCP tools:

1. **Check MCP Server Support** - Verify the task can be handled by Mix.Mcp.Services
2. **Use MCP Tools First** - Primary interface for all Mix CMS operations
3. **Maintain Agent Identity** - Stay in Mix AI Agent character
4. **Document MCP Usage** - Record tools used and effectiveness
5. **Agent-First Approach** - Leverage agentic capabilities of the MCP server

## Architecture Overview

The Mix MCP Lib implements an advanced agentic server architecture for handling user requests, planning, task execution, and conversational interactions. The system is modular, extensible, and leverages both LLMs (Large Language Models) and structured tool execution via the MCP protocol.

**As a Mix AI Agent, you are designed to work seamlessly with this agentic architecture.**

---

## ?? Latest Enhancements (v2.0)

### Knowledge & Resource Enrichment Features

#### 1. **Knowledge Base Integration**
- **IKnowledgeBaseService** and **KnowledgeBaseService**: Pluggable knowledge base for documentation, FAQs, and domain-specific information
- **Features:**
  - Contextual search for relevant information
  - Category-based knowledge organization (tools, planning, database, content)
  - Planning context generation for agents
  - Cached results for performance optimization
  - Default knowledge entries for Mix CMS operations

#### 2. **Semantic Search Capabilities**
- **ISemanticSearchService** and **SemanticSearchService**: Advanced search capabilities for documents and content
- **Features:**
  - Semantic similarity search using text analysis algorithms
  - Document indexing and retrieval
  - Similarity-based recommendations
  - **New MCP Tool:** `SemanticSearchTool` for direct access via MCP protocol

#### 3. **Enhanced Resource Caching**
- **IResourceCacheService**: Intelligent caching with statistics and pattern-based invalidation
- **Features:**
  - Cache statistics and performance monitoring
  - Pattern-based cache invalidation
  - Async factory functions for cache-miss scenarios
  - Configurable cache expiry policies

#### 4. **Class Structure Discovery Tool**
- **New MCP Tool:** `ClassStructureTool` for direct access via MCP protocol
- **Features:**
  - `GetClassStructure(className)`: Returns the structure of any class (properties, methods, attributes) by its full name
  - JSON-formatted output for agentic consumption
  - Enables agents and users to understand how to use any class in the system

---

## Key Agent Classes

### RoutingAgent
- **Role:** Entry point for user requests. Classifies intent (chat vs. plan) using LLM and routes to the appropriate agent.
- **Logic:**
  - Uses LLM to classify user input as either a normal conversation or a planning/multi-step request
  - Delegates to ChatAgent or PlanningAgent accordingly
  - **Enhanced:** Now supports knowledge base integration for better intent classification

### PlanningAgent
- **Role:** Handles multi-step/planning requests with enhanced knowledge integration.
- **Logic:**
  - Uses LLM to break down user input into actionable prompts, considering available MCP tools
  - **Enhanced:** Queries knowledge base for relevant context before planning
  - Executes each prompt sequentially via TaskAgent, passing context/results between steps
  - Publishes progress/results via MQTT
  - Returns a summary of the plan execution

### TaskAgent
- **Role:** Executes individual tasks, including direct tool invocation and command handling.
- **Logic:**
  - Analyzes input to determine if it maps to an MCP tool or a command (start, status, complete, etc.)
  - If a tool is identified, calls the MCP tool and logs the result
  - Maintains task state and history with enhanced memory management
  - **Enhanced:** Can access knowledge base for better tool selection
  - Falls back to LLM for general responses if no tool/command matches

### ChatAgent
- **Role:** Handles conversational (non-planning) interactions with knowledge integration.
- **Logic:**
  - Maintains conversation history
  - **Enhanced:** Can search knowledge base for relevant information
  - Builds prompts for LLM based on conversation context
  - Returns LLM-generated responses

---

## Enhanced MCP Tools

### SemanticSearchTool
- **SearchAsync:** Performs semantic search with configurable similarity thresholds
- **FindSimilarAsync:** Finds documents similar to provided text
- **IndexDocumentAsync:** Adds new documents to the search index
- **Features:** JSON-formatted responses, error handling, parameter validation

### ClassStructureTool
- **GetClassStructure:** Returns the structure of any class by its full name
- **Features:** JSON output includes properties, methods, and attributes for agentic usage
- **Use Case:** Enables agents and users to understand and interact with any class in the system

### Knowledge-Enhanced Planning
- Agents now leverage knowledge base for better context understanding
- Planning decisions include relevant documentation and best practices
- Improved tool selection based on historical knowledge and patterns

---

## Conceptual Visualization
+-------------------+
|   User Request    |
+-------------------+
         |
         v
+-------------------+
|   RoutingAgent    |  <-- Classifies intent (chat/plan)
+-------------------+
   |             |
   v             v
+--------+   +-------------+
|ChatAgent|   |PlanningAgent|  <-- Enhanced with Knowledge Base
+--------+   +-------------+
   |             |
   v             v
+----------+ +-------------+
|Knowledge |  |  TaskAgent  |  <-- Executes tasks, calls MCP tools
|Base      |  +-------------+
+----------+       |
   |               v
   v         +-------------+
+----------+  | Semantic    |
|Resource  |  | Search      |
|Cache     |  | Service     |
+----------+  +-------------+
- **RoutingAgent**: Decides if the request is a chat or a plan
- **ChatAgent**: Handles chat with knowledge integration
- **PlanningAgent**: Decomposes complex requests with contextual knowledge
- **TaskAgent**: Executes atomic tasks, invokes MCP tools
- **Knowledge Base**: Provides contextual information for better decision making
- **Semantic Search**: Advanced search capabilities for content discovery
- **Resource Cache**: Intelligent caching for performance optimization
- **Class Structure Tool**: Enables agents to discover and use class structures

---

## Key Concepts

- **Agentic Orchestration**: Modular agents, each with a clear responsibility
- **LLM Integration**: Used for intent classification, prompt decomposition, and conversational responses
- **MCP Tool Discovery/Execution**: TaskAgent and PlanningAgent leverage available MCP tools for actionable tasks
- **Knowledge-Driven Planning**: Agents use contextual knowledge for better decision making
- **Semantic Understanding**: Advanced search and similarity matching capabilities
- **Intelligent Caching**: Performance optimization through smart resource caching
- **State & History Management**: Each agent maintains relevant state with enhanced context
- **Extensibility**: New tools, knowledge sources, and agent types can be added with minimal changes

---

## Technical Implementation

### Service Registration// Enhanced service registration in AgentExtensions.cs
builder.Services.TryAddSingleton<IKnowledgeBaseService, KnowledgeBaseService>();
builder.Services.TryAddSingleton<ISemanticSearchService, SemanticSearchService>();
builder.Services.TryAddSingleton<IResourceCacheService, ResourceCacheService>();
### Knowledge Base Usage// Agents automatically get knowledge context
var context = await GetKnowledgeContextAsync(userInput, "planning");

// Search for specific information
var knowledge = await SearchKnowledgeAsync("database operations", 5);
### Semantic Search via MCP// Via MCP tool
var results = await semanticSearchTool.SearchAsync("content management", 10, 0.7);

// Direct service usage
var similar = await semanticSearchService.FindSimilarAsync(documentContent, 5);
### Resource Caching// Get or create with factory
var expensiveData = await cacheService.GetOrCreateAsync(
    "expensive_computation", 
    async ct => await ComputeExpensiveData(ct),
    TimeSpan.FromMinutes(30));
---

## Benefits

### ?? **Performance Improvements**
- **Intelligent Caching**: Reduces API calls and database queries
- **Optimized Memory Management**: Enhanced session memory with statistics
- **Async Operations**: Non-blocking knowledge and search operations

### ?? **Enhanced Intelligence**
- **Context-Aware Planning**: Better task decomposition with domain knowledge
- **Semantic Understanding**: Advanced content discovery and similarity matching
- **Knowledge-Driven Decisions**: Agents leverage historical patterns and best practices

### ?? **Developer Experience**
- **Extensible Architecture**: Easy to add new knowledge sources and tools
- **Comprehensive Logging**: Detailed monitoring and debugging capabilities
- **Type Safety**: Full nullable reference type support with .NET 9

### ?? **Mix CMS Integration**
- **Multi-Tenant Aware**: All services respect tenant isolation
- **MixDb Patterns**: Consistent with existing database patterns
- **Content Management**: Specialized knowledge for pages, posts, modules, and templates

---

## Getting Started

1. **Service Registration**: Services are automatically registered via `AddAgents()` extension
2. **Knowledge Base**: Pre-populated with Mix CMS operational knowledge
3. **Semantic Search**: Ready-to-use with sample documents
4. **MCP Tools**: Available immediately via MCP protocol
5. **Class Structure Tool**: Instantly discover class structures for agentic usage

---

## Future Enhancements

- **Vector Database Integration**: Replace text-based similarity with proper embeddings
- **Advanced Analytics**: Enhanced metrics and performance monitoring
- **Machine Learning**: Adaptive knowledge base with user feedback loops
- **Multi-Language Support**: Internationalization for knowledge base content

---

This enhanced architecture enables more intelligent, context-aware handling of user requests, leveraging knowledge bases, semantic search, intelligent caching, and class structure discovery for improved performance and accuracy in Mix CMS operations.