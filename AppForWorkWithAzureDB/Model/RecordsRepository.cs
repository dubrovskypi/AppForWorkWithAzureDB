using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class RecordsRepository
    {
        private TestDBEntities context = new TestDBEntities();

        public IEnumerable<TestTable> Games
        {
            get { return context.TestTables; }
        }

    }
}
