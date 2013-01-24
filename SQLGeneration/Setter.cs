using System;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a column being set to a value to the command.
    /// </summary>
    public class Setter : ISetter
    {
        private readonly IColumn _column;
        private readonly IProjectionItem _value;

        /// <summary>
        /// Initializes a new instance of a Setter.
        /// </summary>
        /// <param name="column">The name of the column to set.</param>
        /// <param name="value">The value to set the column to.</param>
        public Setter(IColumn column, IProjectionItem value)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            _column = column;
            _value = value;
        }

        /// <summary>
        /// Gets the column being set.
        /// </summary>
        public IColumn Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Gets the value that the column is being set to.
        /// </summary>
        public IProjectionItem Value
        {
            get { return _value; }
        }

        string ISetter.GetSetterText(BuilderContext context)
        {
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(context);
            return formatter.GetUnaliasedReference(_column) + " = " + formatter.GetUnaliasedReference(_value);
        }
    }
}
