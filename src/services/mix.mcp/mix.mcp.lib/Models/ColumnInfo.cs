using Mix.Constant.Enums;

namespace Mix.MCP.Lib.Models
{
    /// <summary>
    /// Column information structure
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// Column name (system name in snake_case)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data type of the column
        /// </summary>
        public MixDataType DataType { get; set; }

        /// <summary>
        /// Whether the column is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Description of the column's purpose
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Default value for the column
        /// </summary>
        public string DefaultValue { get; set; }
    }
} 