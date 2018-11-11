using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization.Models
{
    public class DoctorsView : IComparable<DoctorsView>
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int CompareTo(DoctorsView other)
        {
            return (this.LastName + this.FirstName).ToLower().CompareTo((other.LastName + other.FirstName).ToLower());
        }
    }
}
