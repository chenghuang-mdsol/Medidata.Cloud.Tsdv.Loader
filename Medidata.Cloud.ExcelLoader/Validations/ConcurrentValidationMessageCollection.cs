using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    internal class ConcurrentValidationMessageCollection : IValidationMessageCollection
    {
        private readonly ConcurrentBag<IValidationMessage> _list = new ConcurrentBag<IValidationMessage>(); 
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
            foreach (var item in items)
            {
                _list.Add(item);
            }
            return this;
        }
    }
}