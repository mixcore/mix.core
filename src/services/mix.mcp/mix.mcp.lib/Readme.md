# Mix MCP Lib: Agentic MCP Server Overview

## Architecture Overview

The Mix MCP Lib implements an agentic server architecture for handling user requests, planning, task execution, and conversational interactions. The system is modular, extensible, and leverages both LLMs (Large Language Models) and structured tool execution via the MCP protocol.

---

## Key Agent Classes

### RoutingAgent
- **Role:** Entry point for user requests. Classifies intent (chat vs. plan) using LLM and routes to the appropriate agent.
- **Logic:**
  - Uses LLM to classify user input as either a normal conversation or a planning/multi-step request.
  - Delegates to ChatAgent or PlanningAgent accordingly.

### PlanningAgent
- **Role:** Handles multi-step/planning requests.
- **Logic:**
  - Uses LLM to break down user input into actionable prompts, considering available MCP tools.
  - Executes each prompt sequentially via TaskAgent, passing context/results between steps.
  - Publishes progress/results via MQTT.
  - Returns a summary of the plan execution.

### TaskAgent
- **Role:** Executes individual tasks, including direct tool invocation and command handling.
- **Logic:**
  - Analyzes input to determine if it maps to an MCP tool or a command (start, status, complete, etc.).
  - If a tool is identified, calls the MCP tool and logs the result.
  - Maintains task state and history.
  - Falls back to LLM for general responses if no tool/command matches.

### ChatAgent
- **Role:** Handles conversational (non-planning) interactions.
- **Logic:**
  - Maintains conversation history.
  - Builds prompts for LLM based on conversation context.
  - Returns LLM-generated responses.

---

## Conceptual Visualization

```
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
|ChatAgent|   |PlanningAgent|
+--------+   +-------------+
                 |
                 v
           +-------------+
           |  TaskAgent  |  <-- Executes tasks, calls MCP tools, manages state/history
           +-------------+
```

- **RoutingAgent**: Decides if the request is a chat or a plan.
- **ChatAgent**: Handles chat, maintains context.
- **PlanningAgent**: Decomposes complex requests, orchestrates multi-step plans, uses TaskAgent for execution.
- **TaskAgent**: Executes atomic tasks, invokes MCP tools, manages task lifecycle.

---

## Key Concepts

- **Agentic Orchestration**: Modular agents, each with a clear responsibility.
- **LLM Integration**: Used for intent classification, prompt decomposition, and conversational responses.
- **MCP Tool Discovery/Execution**: TaskAgent and PlanningAgent leverage available MCP tools for actionable tasks.
- **State & History Management**: Each agent maintains relevant state (e.g., conversation history, task state/history).
- **Extensibility**: New tools and agent types can be added with minimal changes to the routing/orchestration logic.

---

This architecture enables flexible, intelligent handling of both simple and complex user requests, leveraging both LLMs and structured tool execution.
