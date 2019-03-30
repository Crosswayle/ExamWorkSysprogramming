using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
	public class EFcontext : DbContext
	{
		public EFcontext() : base("DBConnection")
		{
			
		}
		public DbSet<User> Users { get; set; }
	}
}
