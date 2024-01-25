using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Maui.Factories
{
    internal abstract class FactoryBase<T>
    {
        protected List<T> createdObjects = new();

        public T? CreateObject()
        {
            var obj = CreateDefaultObject();
            if (obj != null)
            {
                createdObjects.Add(obj);
            }

            return obj;
        }

        protected abstract T? CreateDefaultObject();


    }
}
