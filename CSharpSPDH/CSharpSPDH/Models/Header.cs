using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharpSPDH.Models
{
    public class Header
    {
        protected readonly string _headerMessage;
        /// <summary>
        /// Used For Generation of new Header
        /// </summary>
        public Header()
        {

        }
        /// <summary>
        /// The given string will be parsed and will be assigned to each property
        /// </summary>
        /// <param name="headerMessage">SPDH Header message only</param>
        public Header(string headerMessage)
        {
            _headerMessage = headerMessage;
            ParseHeader();
        }
        /// <summary>
        /// 2 Alphanumeric Characters
        /// </summary>
        [StringLength(2, ErrorMessage = "The DeviceType value cannot exceed 2 characters.")]
        public virtual string DeviceType { get; set; } = "9.";
        /// <summary>
        /// 2 numeric characters
        /// </summary>
        [StringLength(2, ErrorMessage = "The TransmissionNumber value cannot exceed 2 characters.")]
        public virtual string TransmissionNumber { get; set; } = string.Empty;
        /// <summary>
        /// 16 Alphanumeric Characters 
        /// </summary>
        [StringLength(16, ErrorMessage = "The TerminalId value cannot exceed 16 characters.")]
        public virtual string TerminalId { get; set; } = string.Empty;
        /// <summary>
        /// 6 Alphanumeric Characters
        /// </summary>
        [StringLength(6, ErrorMessage = "The EmployeeId value cannot exceed 6 characters.")]
        public virtual string EmployeeId { get; set; } = string.Empty;
        /// <summary>
        /// 6 Alphanumeric Characters, has default value
        /// </summary>
        [StringLength(6, ErrorMessage = "The CurrentDate value cannot exceed 6 characters.")]
        public virtual string CurrentDate { get; set; } = DateTime.Now.ToString("yyMMdd");
        /// <summary>
        /// 6 Alphanumeric Characters, has default value
        /// </summary>
        [StringLength(6, ErrorMessage = "The CurrentTime value cannot exceed 6 characters.")]
        public virtual string CurrentTime { get; set; } = DateTime.Now.ToString("hhmmss");
        /// <summary>
        /// 1 Alphanumeric Character
        /// </summary>
        [StringLength(1, ErrorMessage = "The MessageType value cannot exceed 1 characters.")]
        public virtual string MessageType { get; set; } = string.Empty;
        /// <summary>
        /// 1 Alphanumeric Character
        /// </summary>
        [StringLength(1, ErrorMessage = "The MessageSubType value cannot exceed 1 characters.")]
        public virtual string MessagSubType { get; set; } = string.Empty;
        /// <summary>
        /// 2 Alphanumeric Characters
        /// </summary>
        [StringLength(2, ErrorMessage = "The TransactionCode value cannot exceed 2 characters.")]
        public virtual string TransactionCode { get; set; } = string.Empty;
        /// <summary>
        /// 1 Alphanumeric Character
        /// </summary>
        [StringLength(1, ErrorMessage = "The ProcessingFlag1 value cannot exceed 1 characters.")]
        public virtual string ProcessingFlag1 { get; set; } = string.Empty;
        /// <summary>
        /// 1 Alphanumeric Character
        /// </summary>
        [StringLength(1, ErrorMessage = "The ProcessingFlag2 value cannot exceed 1 characters.")]
        public virtual string ProcessingFlag2 { get; set; } = string.Empty;
        /// <summary>
        /// 1 Alphanumeric Character
        /// </summary>
        [StringLength(1, ErrorMessage = "The ProcessingFlag3 value cannot exceed 1 characters.")]
        public virtual string ProcessingFlag3 { get; set; } = string.Empty;
        /// <summary>
        /// 3 Alphanumeric Characters with default value of Success "000"
        /// </summary>
        [StringLength(3, ErrorMessage = "The Responce Code value cannot exceed 3 characters.")]
        public virtual string ResponseCode { get; set; } = "000";

        /// <summary>
        /// Overriden base object ToString Method
        /// </summary>
        /// <returns>A concatenation of all properties of the Header</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            PropertyInfo[] headerProperties = typeof(Header).GetProperties();

            foreach (PropertyInfo property in headerProperties)
            {
                var stringLengthAttr = (StringLengthAttribute)property.GetCustomAttributes(typeof(StringLengthAttribute)).First();
                var valueToAppend = property.GetValue(this, null).ToString();
                valueToAppend = valueToAppend.Length == stringLengthAttr.MaximumLength
                    ? valueToAppend : valueToAppend.PadRight(stringLengthAttr.MaximumLength, ' ');
                builder.Append(valueToAppend);
            }

            if (builder.Length != 48)
                throw new ArgumentOutOfRangeException($"Message {builder.ToString()} has invalid length of {builder.Length} instead of 48");
            else
                return builder.ToString();
        }

        public virtual void ParseHeader()
        {
            var subStringPoint = 0;
            PropertyInfo[] headerProperties = typeof(Header).GetProperties();

            foreach (PropertyInfo property in headerProperties)
            {
                var stringLengthAttr = (StringLengthAttribute)property.GetCustomAttributes(typeof(StringLengthAttribute)).First();
                property.SetValue(this, _headerMessage.Substring(subStringPoint, stringLengthAttr.MaximumLength));
                subStringPoint += stringLengthAttr.MaximumLength;
            }
        }
    }
}
