using Group_18_Final_Project.Models;
using Group_18_Final_Project.Utilities;
using Group_18_Final_Project.dal;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Seeding
{
	public static class SeedEmployees
	{
		public static void SeedAllEmployees(AppDbContext db)
		{
			if (db.Users.Count() == 28)
			{
				throw new NotSupportedException("The database already contains all 28 customers!");
			}

			Int32 intEmployeesAdded = 0;
			String empName = "Begin"; //helps to keep track of error on repos
			List<User> Employees = new List<User>();

			try
			{
				User e1 = new User();
				e1.UserID = GenerateUserID.GetNextUserID(db);
				e1.Password = "401661146";
				e1.LastName = "Baker";
				e1.FirstName = "Christopher";
				e1.Address = "1245 Lake Libris Dr.";
				e1.UserType = "Manager";
				e1.City = "Cedar Park";
				e1.State = "TX";
				e1.ZipCode = 78613;
				e1.PhoneNumber = 3395325649;
				e1.Email = "c.baker@bevosbooks.com";
				Employees.Add(e1);

				User e2 = new User();
				e2.UserID = GenerateUserID.GetNextUserID(db);
				e2.Password = "1112221212";
				e2.LastName = "Barnes";
				e2.FirstName = "Susan";
				e2.Address = "888 S. Main";
				e2.UserType = "Employee";
				e2.City = "Kyle";
				e2.State = "TX";
				e2.ZipCode = 78640;
				e2.PhoneNumber = 9636389416;
				e2.Email = "s.barnes@bevosbooks.com";
				Employees.Add(e2);

				User e3 = new User();
				e3.UserID = GenerateUserID.GetNextUserID(db);
				e3.Password = "4445554343";
				e3.LastName = "Garcia";
				e3.FirstName = "Hector";
				e3.Address = "777 PBR Drive";
				e3.UserType = "Employee";
				e3.City = "Austin";
				e3.State = "TX";
				e3.ZipCode = 78712;
				e3.PhoneNumber = 4547135738;
				e3.Email = "h.garcia@bevosbooks.com";
				Employees.Add(e3);

				User e4 = new User();
				e4.UserID = GenerateUserID.GetNextUserID(db);
				e4.Password = "797348821";
				e4.LastName = "Ingram";
				e4.FirstName = "Brad";
				e4.Address = "6548 La Posada Ct.";
				e4.UserType = "Employee";
				e4.City = "Austin";
				e4.State = "TX";
				e4.ZipCode = 78705;
				e4.PhoneNumber = 5817343315;
				e4.Email = "b.ingram@bevosbooks.com";
				Employees.Add(e4);

				User e5 = new User();
				e5.UserID = GenerateUserID.GetNextUserID(db);
				e5.Password = "8889993434";
				e5.LastName = "Jackson";
				e5.FirstName = "Jack";
				e5.Address = "222 Main";
				e5.UserType = "Employee";
				e5.City = "Austin";
				e5.State = "TX";
				e5.ZipCode = 78760;
				e5.PhoneNumber = 8241915317;
				e5.Email = "j.jackson@bevosbooks.com";
				Employees.Add(e5);

				User e6 = new User();
				e6.UserID = GenerateUserID.GetNextUserID(db);
				e6.Password = "341553365";
				e6.LastName = "Jacobs";
				e6.FirstName = "Todd";
				e6.Address = "4564 Elm St.";
				e6.UserType = "Employee";
				e6.City = "Georgetown";
				e6.State = "TX";
				e6.ZipCode = 78628;
				e6.PhoneNumber = 2477822475;
				e6.Email = "t.jacobs@bevosbooks.com";
				Employees.Add(e6);

				User e7 = new User();
				e7.UserID = GenerateUserID.GetNextUserID(db);
				e7.Password = "9099099999";
				e7.LastName = "Jones";
				e7.FirstName = "Lester";
				e7.Address = "999 LeBlat";
				e7.UserType = "Employee";
				e7.City = "Austin";
				e7.State = "TX";
				e7.ZipCode = 78747;
				e7.PhoneNumber = 4764966462;
				e7.Email = "l.jones@bevosbooks.com";
				Employees.Add(e7);

				User e8 = new User();
				e8.UserID = GenerateUserID.GetNextUserID(db);
				e8.Password = "5554443333";
				e8.LastName = "Larson";
				e8.FirstName = "Bill";
				e8.Address = "1212 N. First Ave";
				e8.UserType = "Employee";
				e8.City = "Round Rock";
				e8.State = "TX";
				e8.ZipCode = 78665;
				e8.PhoneNumber = 3355258855;
				e8.Email = "b.larson@bevosbooks.com";
				Employees.Add(e8);

				User e9 = new User();
				e9.UserID = GenerateUserID.GetNextUserID(db);
				e9.Password = "770097399";
				e9.LastName = "Lawrence";
				e9.FirstName = "Victoria";
				e9.Address = "6639 Bookworm Ln.";
				e9.UserType = "Employee";
				e9.City = "Austin";
				e9.State = "TX";
				e9.ZipCode = 78712;
				e9.PhoneNumber = 7511273054;
				e9.Email = "v.lawrence@bevosbooks.com";
				Employees.Add(e9);

				User e10 = new User();
				e10.UserID = GenerateUserID.GetNextUserID(db);
				e10.Password = "2223332222";
				e10.LastName = "Lopez";
				e10.FirstName = "Marshall";
				e10.Address = "90 SW North St";
				e10.UserType = "Employee";
				e10.City = "Austin";
				e10.State = "TX";
				e10.ZipCode = 78729;
				e10.PhoneNumber = 7477907070;
				e10.Email = "m.lopez@bevosbooks.com";
				Employees.Add(e10);

				User e11 = new User();
				e11.UserID = GenerateUserID.GetNextUserID(db);
				e11.Password = "775908138";
				e11.LastName = "MacLeod";
				e11.FirstName = "Jennifer";
				e11.Address = "2504 Far West Blvd.";
				e11.UserType = "Employee";
				e11.City = "Austin";
				e11.State = "TX";
				e11.ZipCode = 78705;
				e11.PhoneNumber = 2621216845;
				e11.Email = "j.macleod@bevosbooks.com";
				Employees.Add(e11);

				User e12 = new User();
				e12.UserID = GenerateUserID.GetNextUserID(db);
				e12.Password = "101529845";
				e12.LastName = "Markham";
				e12.FirstName = "Elizabeth";
				e12.Address = "7861 Chevy Chase";
				e12.UserType = "Employee";
				e12.City = "Austin";
				e12.State = "TX";
				e12.ZipCode = 78785;
				e12.PhoneNumber = 5028075807;
				e12.Email = "e.markham@bevosbooks.com";
				Employees.Add(e12);

				User e13 = new User();
				e13.UserID = GenerateUserID.GetNextUserID(db);
				e13.Password = "463566718";
				e13.LastName = "Martinez";
				e13.FirstName = "Gregory";
				e13.Address = "8295 Sunset Blvd.";
				e13.UserType = "Employee";
				e13.City = "Austin";
				e13.State = "TX";
				e13.ZipCode = 78712;
				e13.PhoneNumber = 1994708542;
				e13.Email = "g.martinez@bevosbooks.com";
				Employees.Add(e13);

				User e14 = new User();
				e14.UserID = GenerateUserID.GetNextUserID(db);
				e14.Password = "1112223232";
				e14.LastName = "Mason";
				e14.FirstName = "Jack";
				e14.Address = "444 45th St";
				e14.UserType = "Employee";
				e14.City = "Austin";
				e14.State = "TX";
				e14.ZipCode = 78701;
				e14.PhoneNumber = 1748136441;
				e14.Email = "j.mason@bevosbooks.com";
				Employees.Add(e14);

				User e15 = new User();
				e15.UserID = GenerateUserID.GetNextUserID(db);
				e15.Password = "353308615";
				e15.LastName = "Miller";
				e15.FirstName = "Charles";
				e15.Address = "8962 Main St.";
				e15.UserType = "Employee";
				e15.City = "Austin";
				e15.State = "TX";
				e15.ZipCode = 78709;
				e15.PhoneNumber = 8999319585;
				e15.Email = "c.miller@bevosbooks.com";
				Employees.Add(e15);

				User e16 = new User();
				e16.UserID = GenerateUserID.GetNextUserID(db);
				e16.Password = "7776665555";
				e16.LastName = "Nguyen";
				e16.FirstName = "Mary";
				e16.Address = "465 N. Bear Cub";
				e16.UserType = "Employee";
				e16.City = "Austin";
				e16.State = "TX";
				e16.ZipCode = 78734;
				e16.PhoneNumber = 8716746381;
				e16.Email = "m.nguyen@bevosbooks.com";
				Employees.Add(e16);

				User e17 = new User();
				e17.UserID = GenerateUserID.GetNextUserID(db);
				e17.Password = "1911919111";
				e17.LastName = "Rankin";
				e17.FirstName = "Suzie";
				e17.Address = "23 Dewey Road";
				e17.UserType = "Employee";
				e17.City = "Austin";
				e17.State = "TX";
				e17.ZipCode = 78712;
				e17.PhoneNumber = 5239029525;
				e17.Email = "s.rankin@bevosbooks.com";
				Employees.Add(e17);

				User e18 = new User();
				e18.UserID = GenerateUserID.GetNextUserID(db);
				e18.Password = "353904746";
				e18.LastName = "Rhodes";
				e18.FirstName = "Megan";
				e18.Address = "4587 Enfield Rd.";
				e18.UserType = "Employee";
				e18.City = "Austin";
				e18.State = "TX";
				e18.ZipCode = 78729;
				e18.PhoneNumber = 1232139514;
				e18.Email = "m.rhodes@bevosbooks.com";
				Employees.Add(e18);

				User e19 = new User();
				e19.UserID = GenerateUserID.GetNextUserID(db);
				e19.Password = "454776657";
				e19.LastName = "Rice";
				e19.FirstName = "Eryn";
				e19.Address = "3405 Rio Grande";
				e19.UserType = "Manager";
				e19.City = "Austin";
				e19.State = "TX";
				e19.ZipCode = 78746;
				e19.PhoneNumber = 2706602803;
				e19.Email = "e.rice@bevosbooks.com";
				Employees.Add(e19);

				User e20 = new User();
				e20.UserID = GenerateUserID.GetNextUserID(db);
				e20.Password = "700002943";
				e20.LastName = "Rogers";
				e20.FirstName = "Allen";
				e20.Address = "4965 Oak Hill";
				e20.UserType = "Manager";
				e20.City = "Austin";
				e20.State = "TX";
				e20.ZipCode = 78705;
				e20.PhoneNumber = 4139645586;
				e20.Email = "a.rogers@bevosbooks.com";
				Employees.Add(e20);

				User e21 = new User();
				e21.UserID = GenerateUserID.GetNextUserID(db);
				e21.Password = "500987810";
				e21.LastName = "Saunders";
				e21.FirstName = "Sarah";
				e21.Address = "332 Avenue C";
				e21.UserType = "Employee";
				e21.City = "Austin";
				e21.State = "TX";
				e21.ZipCode = 78733;
				e21.PhoneNumber = 9036349587;
				e21.Email = "s.saunders@bevosbooks.com";
				Employees.Add(e21);

				User e22 = new User();
				e22.UserID = GenerateUserID.GetNextUserID(db);
				e22.Password = "500830084";
				e22.LastName = "Sewell";
				e22.FirstName = "William";
				e22.Address = "2365 51st St.";
				e22.UserType = "Manager";
				e22.City = "Austin";
				e22.State = "TX";
				e22.ZipCode = 78755;
				e22.PhoneNumber = 7224308314;
				e22.Email = "w.sewell@bevosbooks.com";
				Employees.Add(e22);

				User e23 = new User();
				e23.UserID = GenerateUserID.GetNextUserID(db);
				e23.Password = "223449167";
				e23.LastName = "Sheffield";
				e23.FirstName = "Martin";
				e23.Address = "3886 Avenue A";
				e23.UserType = "Employee";
				e23.City = "San Marcos";
				e23.State = "TX";
				e23.ZipCode = 78666;
				e23.PhoneNumber = 9349192978;
				e23.Email = "m.sheffield@bevosbooks.com";
				Employees.Add(e23);

				User e24 = new User();
				e24.UserID = GenerateUserID.GetNextUserID(db);
				e24.Password = "7776661111";
				e24.LastName = "Silva";
				e24.FirstName = "Cindy";
				e24.Address = "900 4th St";
				e24.UserType = "Employee";
				e24.City = "Austin";
				e24.State = "TX";
				e24.ZipCode = 78758;
				e24.PhoneNumber = 4874328170;
				e24.Email = "c.silva@bevosbooks.com";
				Employees.Add(e24);

				User e25 = new User();
				e25.UserID = GenerateUserID.GetNextUserID(db);
				e25.Password = "363998335";
				e25.LastName = "Stuart";
				e25.FirstName = "Eric";
				e25.Address = "5576 Toro Ring";
				e25.UserType = "Employee";
				e25.City = "Austin";
				e25.State = "TX";
				e25.ZipCode = 78758;
				e25.PhoneNumber = 1967846827;
				e25.Email = "e.stuart@bevosbooks.com";
				Employees.Add(e25);

				User e26 = new User();
				e26.UserID = GenerateUserID.GetNextUserID(db);
				e26.Password = "904440929";
				e26.LastName = "Tanner";
				e26.FirstName = "Jeremy";
				e26.Address = "4347 Almstead";
				e26.UserType = "Employee";
				e26.City = "Austin";
				e26.State = "TX";
				e26.ZipCode = 78712;
				e26.PhoneNumber = 5923026779;
				e26.Email = "j.tanner@bevosbooks.com";
				Employees.Add(e26);

				User e27 = new User();
				e27.UserID = GenerateUserID.GetNextUserID(db);
				e27.Password = "934778452";
				e27.LastName = "Taylor";
				e27.FirstName = "Allison";
				e27.Address = "467 Nueces St.";
				e27.UserType = "Employee";
				e27.City = "Austin";
				e27.State = "TX";
				e27.ZipCode = 78727;
				e27.PhoneNumber = 7246195827;
				e27.Email = "a.taylor@bevosbooks.com";
				Employees.Add(e27);

				User e28 = new User();
				e28.UserID = GenerateUserID.GetNextUserID(db);
				e28.Password = "393412631";
				e28.LastName = "Taylor";
				e28.FirstName = "Rachel";
				e28.Address = "345 Longview Dr.";
				e28.UserType = "Manager";
				e28.City = "Austin";
				e28.State = "TX";
				e28.ZipCode = 78746;
				e28.PhoneNumber = 9071236087;
				e28.Email = "r.taylor@bevosbooks.com";
				Employees.Add(e28);

				//loop through Employees
				foreach (User emp in Employees)
				{
					//set name of customer to help debug
					empName = emp.Email;

					//see if employee exists in database
					User dbEmp = db.Users.FirstOrDefault(b => b.Email == emp.Email);

					if (dbEmp == null) //employee does not exist in database
					{
						db.Users.Add(emp);
						db.SaveChanges();
						intEmployeesAdded += 1;
					}
					else
					{
						dbEmp.UserID = emp.UserID;
						dbEmp.Email = emp.Email;
						dbEmp.Password = emp.Password;
						dbEmp.FirstName = emp.FirstName;
						dbEmp.LastName = emp.LastName;
						dbEmp.Address = emp.Address;
						dbEmp.City = emp.City;
						dbEmp.State = emp.State;
						dbEmp.ZipCode = emp.ZipCode;
						dbEmp.PhoneNumber = emp.PhoneNumber;
						dbEmp.UserType = emp.UserType;
						db.Update(dbEmp);
						db.SaveChanges();
					}
				}
			}
			catch
			{
				String msg = "Employees added:" + intEmployeesAdded + "; Error on ";
				throw new InvalidOperationException(msg);
			}
		}
	}
}
