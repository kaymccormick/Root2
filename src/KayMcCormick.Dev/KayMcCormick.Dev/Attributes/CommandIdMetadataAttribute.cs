using System;

namespace KayMcCormick.Dev.Attributes
{
    public class CommandIdMetadataAttribute : Attribute
    {
        private Guid _guid;

        public CommandIdMetadataAttribute(string commandId)
        {
            if (Guid.TryParse(commandId, out var guid))
            {
                _guid = guid;
            }
        }

        public Guid CommandId => _guid;
    }
}