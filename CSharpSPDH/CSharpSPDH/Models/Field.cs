using System.Collections.Generic;
using System.Text;

namespace CSharpSPDH.Models
{
    /// <summary>
    /// Component of each SPDH Message
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Default Field Separator Value --> \u001
        /// Override to change
        /// </summary>
        public virtual string FieldSeparator { get; } = "\u001c";
        /// <summary>
        /// Name of SPDH Field
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value of SPDH Field
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// SubFields of Current Field as a Field Collection
        /// </summary>
        public List<Field> SubFields { get; set; } = new List<Field>();

        /// <summary>
        /// Called by Message parse Method in order to build string of Fields or Fields with SubFields.
        /// </summary>
        /// <returns>string representation of builded field value</returns>
        public override string ToString()
        {
            var fieldBuider = new StringBuilder();

            if (SubFields.Count > 0)
            {
                fieldBuider.Append(Name);
                SubFields.ForEach(field => fieldBuider.Append($"{FieldSeparator}{field.Name}{field.Value}"));
            }
            else
                fieldBuider.Append($"{Name}{Value}");

            return fieldBuider.ToString();
        }
    }
}
