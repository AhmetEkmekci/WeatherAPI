using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTR.Abstraction.Entity
{
    public interface IEntity
    {
        public DateTime Created { get; set; }
    }
}
