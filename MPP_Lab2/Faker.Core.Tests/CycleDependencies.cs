using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.Core.Tests
{
    public class C
    {
        public D d { get; set; }
        public string s { get; set; }
    }

    public class D
    {
        public E e { get; set; }
    }

    public class E
    {
        public C c { get; set; }
    }
}
