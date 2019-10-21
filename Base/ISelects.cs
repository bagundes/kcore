using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KCore.Base
{
    public interface ISelects : IList, ICollection, IEnumerable
    {
        new Model.Select this[int index] { get; }
        Model.Select this[string value] { get; }
        //Model.Select this[string name, string subreportName] { get; }

        void Add(Model.Select select);
        new void Add(object value);
        void Add(object value, string text, bool @default = false, bool encrypt = false);
        void AddEncrypted(object value);

    }
}