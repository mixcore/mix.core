using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;

namespace Mix.MCP.Lib.Prompts
{
    [McpServerPromptType]
    public class GeneratePrompt
    {
        [McpServerPrompt, Description("CreateMixDbData prompt for generating code")]
        public static ChatMessage GenerateCodePrompt(
            [Description("Requirements for code generation")] string requirements,
            [Description("Programming language to use")] string programmingLanguage = "C#") =>
            new(ChatRole.User, """
                Please generate code in {programmingLanguage} based on the following requirements:
                {requirements}

                Requirements for the generated code:
                1. Must be complete and executable
                2. Must include necessary import statements
                3. Must include error handling
                4. Must follow best practices for {programmingLanguage}
                5. Must include comments explaining the code
                6. Must be well-structured and easy to read
                7. Must handle edge cases
                8. Must include input validation
                9. Must follow SOLID principles
                10. Must include proper logging
                11. Must be secure and follow security best practices
                12. Must be performant and optimized
                """);

        [McpServerPrompt, Description("CreateMixDbData prompt for generating documentation")]
        public static ChatMessage GenerateDocumentationPrompt(
            [Description("Code to document")] string code,
            [Description("Type of documentation to generate")] string documentationType = "XML") =>
            new(ChatRole.User, """
                Please generate {documentationType} documentation for the following code:
                {code}

                Requirements for the documentation:
                1. Must be complete and detailed
                2. Must include descriptions for all classes, methods, and properties
                3. Must include examples if necessary
                4. Must follow the {documentationType} documentation standard
                5. Must be well-structured and easy to read
                6. Must include parameter descriptions
                7. Must include return value descriptions
                8. Must include exception documentation if applicable
                9. Must include usage examples
                10. Must follow the language's documentation conventions
                """);

        [McpServerPrompt, Description("CreateMixDbData prompt for generating unit tests")]
        public static ChatMessage GenerateUnitTestsPrompt(
            [Description("Code to generate tests for")] string code,
            [Description("Testing framework to use")] string testingFramework = "xUnit") =>
            new(ChatRole.User, """
                Please generate unit tests using {testingFramework} for the following code:
                {code}

                Requirements for the tests:
                1. Must cover all important test cases
                2. Must include necessary setup and teardown
                3. Must include assertions
                4. Must follow best practices for {testingFramework}
                5. Must be well-structured and easy to read
                6. Must include comments explaining the tests
                7. Must handle edge cases and error conditions
                8. Must include proper test naming conventions
                9. Must use appropriate test attributes and decorators
                10. Must include test data setup if needed
                """);
    }
} 