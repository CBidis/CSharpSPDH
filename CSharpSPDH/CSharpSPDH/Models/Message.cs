using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSPDH.Models
{
    /// <summary>
    /// Use this Class when you want to Generate or Parse SPDH Messages with Fields and SubFields.
    /// </summary>
    public class Message
    {
        protected readonly string _spdhMessage;
        protected readonly List<Field> _spdhFields;
        protected string _modifiedSpdhMessage;

        /// <summary>
        /// Default Field Separator Value --> \u001
        /// Override to change
        /// </summary>
        public virtual string FieldSeparator { get; } = "\u001c";
        /// <summary>
        /// Default End Message Character --> \u0003
        /// Override to change
        /// </summary>
        public virtual string EndMessageText { get; } = "\u0003";

        public Header Header { get; private set; }
        /// <summary>
        /// This constructor is used in order to parse the SPDH message.
        /// </summary>
        /// <param name="spdhMessage">the SPDH message</param>
        /// <param name="spdhFields">A list of Field that will be parsed from the Message</param>
        public Message(string spdhMessage, List<Field> spdhFields)
        {
            _spdhMessage = spdhMessage;
            _spdhFields = spdhFields;
            ParseMessage();
        }

        /// <summary>
        ///  Use this contructor in order to Parse simple SPDH responses that contain no SubFields.
        /// </summary>
        /// <param name="spdhMessage">the SPDH message</param>
        public Message(string spdhMessage)
        {
            _spdhMessage = spdhMessage;
            ParseSimpleMessage();
        }

        /// <summary>
        /// This constructor is used in order to build the SPDH message
        /// </summary>
        /// <param name="header">Header object that will be stringified in the building of Message</param>
        public Message(Header header) => Header = header;

        /// <summary>
        /// A Dictionary containing the Fields of the SPDH Message, Key is the Field Letter
        /// </summary>
        public virtual List<Field> Fields { get; set; } = new List<Field>();


        /// <summary>
        /// Overriden base object ToString Method
        /// </summary>
        /// <returns>A concatenation of Message and Fields properties.</returns>
        public override string ToString()
        {
            var spdhBuilder = new StringBuilder();

            spdhBuilder.Append(Header.ToString());
            Fields.ForEach(field => spdhBuilder.Append($"{FieldSeparator}{field.ToString()}"));

            return spdhBuilder.ToString();
        }

        public virtual void ParseMessage()
        {
            Header = new Header(_spdhMessage.Substring(0, 48));
            _modifiedSpdhMessage = _spdhMessage.Substring(49);
            Fields = ParseFields(_spdhFields);
        }

        public virtual void ParseSimpleMessage()
        {
            Header = new Header(_spdhMessage.Substring(0, 48));
            _modifiedSpdhMessage = _spdhMessage.Substring(49);
            Fields = SplitMessage();
        }

        public virtual List<Field> SplitMessage()
        {
            var parsedFields = new List<Field>();

            if (string.IsNullOrEmpty(_modifiedSpdhMessage))
                return parsedFields;

            var splittedMessageFields = _modifiedSpdhMessage.Split(FieldSeparator.ToCharArray()).ToList();
            foreach (var messageField in splittedMessageFields)
            {
                var newField = new Field
                {
                    Name = messageField.Substring(0, 1),
                    Value = messageField.Substring(1).Replace(EndMessageText, string.Empty)
                };

                parsedFields.Add(newField);
            }
            return parsedFields;
        }

        public virtual List<Field> ParseFields(List<Field> fieldsToParse)
        {
            var parsedFields = new List<Field>();
            fieldsToParse.ForEach(field => parsedFields.Add(ParseField(field)));
            return parsedFields;
        }

        public virtual Field ParseField(Field field)
        {
            var indexOfField = _modifiedSpdhMessage.IndexOf(field.Name);

            if (indexOfField == -1)
                return field;

            if (field.SubFields.Count > 0)
            {
                field.SubFields = ParseFields(field.SubFields);
                return field;
            }

            var messageFromFieldLeft = _modifiedSpdhMessage.Substring(indexOfField);
            var indexOfFirstDot = messageFromFieldLeft.IndexOf(FieldSeparator);

            //Then we are probably at end Line character so
            if (indexOfFirstDot == -1)
            {
                var indexOfEndCharacter = messageFromFieldLeft.IndexOf(EndMessageText);

                if (indexOfEndCharacter == -1)
                    throw new ArgumentException($"For field {field.Name} could not find field or End separator '.' in message {_spdhMessage}");
                else
                    indexOfFirstDot = indexOfEndCharacter;
            }


            IEnumerable<char> fieldValueChars = messageFromFieldLeft.Skip(field.Name.Length).Take(indexOfFirstDot - 1);
            field.Value = new string(fieldValueChars.ToArray());

            _modifiedSpdhMessage = _modifiedSpdhMessage.Substring(indexOfFirstDot);

            return field;
        }
    }
}
