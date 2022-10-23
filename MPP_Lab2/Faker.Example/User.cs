using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.Example
{
    internal class User
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public DateTime BirthTime { get; set; }
        public User(int age, string name, DateTime dateTime)
        {
            Age = age;
            Name = name;
            BirthTime = dateTime;
        }
        public override string ToString()
        {
            return $"{Name} : {Age} : {BirthTime}";
        }
    }
}
