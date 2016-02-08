using System.Collections;
using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    internal class ValidationMessageCollection : IValidationMessageCollection
    {
        private readonly List<IValidationMessage> _list = new List<IValidationMessage>();
        public IEnumerator<IValidationMessage> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IValidationMessageCollection Add(IValidationMessage item)
        {
            _list.Add(item);
            return this;
        }

        public IValidationMessageCollection AddRange(IEnumerable<IValidationMessage> items)
        {
            _list.AddRange(items);
            return this;
        }
    }
}