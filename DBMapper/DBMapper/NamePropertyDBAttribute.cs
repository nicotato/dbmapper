using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.DBMapper
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NamePropertyDB : Attribute
    {
        // Storage for the UnmanagedType value. 
        public string thisType;

        // Set the unmanaged type in the constructor. 
        public NamePropertyDB(string type)
        {
            thisType = type;
        }

        // Define a property to get and set the UnmanagedType value. 
        public string Name
        {
            get { return thisType; }
            set { thisType = value; }
        }
    }
}
