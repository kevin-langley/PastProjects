using Group_18_Final_Project.Models;
using Group_18_Final_Project.DAL;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Group_18_Final_Project.Utilities;

namespace Group_18_Final_Project.Seeding
{
	public static class SeedCustomers
	{
		public static void SeedAllCustomers(AppDbContext db)
		{
			if (db.Users.Count() == 52)
			{
				throw new NotSupportedException("The database already contains all 52 customers!");
			}

			Int32 intCustomersAdded = 0;
			String custName = "Begin"; //helps to keep track of error on repos
			List<User> Customers = new List<User>();

			try
			{
				User c1 = new User();
                c1.Password = "bookworm";
				c1.LastName = "Baker";
				c1.FirstName = "Christopher";
				c1.Address = "1898 Schurz Alley";
				c1.City = "Austin";
				c1.State = "TX";
				c1.ZipCode = 78705.ToString();
				c1.PhoneNumber = 5725458641.ToString();
				c1.Email = "cbaker@example.com";
                c1.UserName = c1.Email;
                Customers.Add(c1);


                User c2 = new User();
                c2.Password = "potato";
				c2.LastName = "Banks";
				c2.FirstName = "Michelle";
				c2.Address = "97 Elmside Pass";
				c2.City = "Austin";
				c2.State = "TX";
				c2.ZipCode = 78712.ToString();
				c2.PhoneNumber = 9867048435.ToString();
				c2.Email = "banker@longhorn.net";
                c2.UserName = c2.Email;
                Customers.Add(c2);

				User c3 = new User();

                c3.Password = "painting";
				c3.LastName = "Broccolo";
				c3.FirstName = "Franco";
				c3.Address = "88 Crowley Circle";
				c3.City = "Austin";
				c3.State = "TX";
				c3.ZipCode = 78786.ToString();
				c3.PhoneNumber = 6836109514.ToString();
				c3.Email = "franco@example.com";
                c3.UserName = c3.Email;
                Customers.Add(c3);

				User c4 = new User();

                c4.Password = "texas1";
				c4.LastName = "Chang";
				c4.FirstName = "Wendy";
				c4.Address = "56560 Sage Junction";
				c4.City = "Eagle Pass";
				c4.State = "TX";
				c4.ZipCode = 78852.ToString();
				c4.PhoneNumber = 7070911071.ToString();
				c4.Email = "wchang@example.com";
                c4.UserName = c4.Email;
                Customers.Add(c4);

				User c5 = new User();
                c5.Password = "Anchorage";
				c5.LastName = "Chou";
				c5.FirstName = "Lim";
				c5.Address = "60 Lunder Point";
				c5.City = "Austin";
				c5.State = "TX";
				c5.ZipCode = 78729.ToString();
				c5.PhoneNumber = 1488907687.ToString();
				c5.Email = "limchou@gogle.com";
                c5.UserName = c5.Email;
                Customers.Add(c5);

				User c6 = new User();
				c6.Password = "aggies";
                c6.LastName = "Dixon";
				c6.FirstName = "Shan";
				c6.Address = "9448 Pleasure Avenue";
				c6.City = "Georgetown";
				c6.State = "TX";
				c6.ZipCode = 78628.ToString();
				c6.PhoneNumber = 6899701824.ToString();
				c6.Email = "shdixon@aoll.com";
                c6.UserName = c6.Email;
                Customers.Add(c6);

				User c7 = new User();
				c7.Password = "hampton1";
                c7.LastName = "Evans";
				c7.FirstName = "Jim Bob";
				c7.Address = "51 Emmet Parkway";
				c7.City = "Austin";
				c7.State = "TX";
				c7.ZipCode = 78705.ToString();
				c7.PhoneNumber = 9986825917.ToString();
				c7.Email = "j.b.evans@aheca.org";
                c7.UserName = c7.Email;
                Customers.Add(c7);

				User c8 = new User();
                c8.Password = "longhorns";
				c8.LastName = "Feeley";
				c8.FirstName = "Lou Ann";
				c8.Address = "65 Darwin Crossing";
				c8.City = "Austin";
				c8.State = "TX";
				c8.ZipCode = 78704.ToString();
				c8.PhoneNumber = 3464121966.ToString();
				c8.Email = "feeley@penguin.org";
                c8.UserName = c8.Email;
                Customers.Add(c8);

				User c9 = new User();
                c9.Password = "mustangs";
				c9.LastName = "Freeley";
				c9.FirstName = "Tesa";
				c9.Address = "7352 Loftsgordon Court";
				c9.City = "College Station";
				c9.State = "TX";
				c9.ZipCode = 77840.ToString();
				c9.PhoneNumber = 6581357270.ToString();
				c9.Email = "tfreeley@minnetonka.ci.us";
                c9.UserName = c9.Email;
                Customers.Add(c9);

				User c10 = new User();
                c10.Password = "onetime";
				c10.LastName = "Garcia";
				c10.FirstName = "Margaret";
				c10.Address = "7 International Road";
				c10.City = "Austin";
				c10.State = "TX";
				c10.ZipCode = 78756.ToString();
				c10.PhoneNumber = 3767347949.ToString();
				c10.Email = "mgarcia@gogle.com";
                c10.UserName = c10.Email;
                Customers.Add(c10);

				User c11 = new User();
                c11.Password = "pepperoni";
				c11.LastName = "Haley";
				c11.FirstName = "Charles";
				c11.Address = "8 Warrior Trail";
				c11.City = "Austin";
				c11.State = "TX";
				c11.ZipCode = 78746.ToString();
				c11.PhoneNumber = 2198604221.ToString();
				c11.Email = "chaley@thug.com";
                c11.UserName = c11.Email;
                Customers.Add(c11);

				User c12 = new User();
                c12.Password = "raiders";
				c12.LastName = "Hampton";
				c12.FirstName = "Jeffrey";
				c12.Address = "9107 Lighthouse Bay Road";
				c12.City = "Austin";
				c12.State = "TX";
				c12.ZipCode = 78756.ToString();
				c12.PhoneNumber = 1222185888.ToString();
				c12.Email = "jeffh@sonic.com";
                c12.UserName = c12.Email;
                Customers.Add(c12);

				User c13 = new User();
                c13.Password = "jhearn22";
				c13.LastName = "Hearn";
				c13.FirstName = "John";
				c13.Address = "59784 Pierstorff Center";
				c13.City = "Liberty";
				c13.State = "TX";
				c13.ZipCode = 77575.ToString();
				c13.PhoneNumber = 5123071976.ToString();
				c13.Email = "wjhearniii@umich.org";
                c13.UserName = c13.Email;
                Customers.Add(c13);

				User c14 = new User();
                c14.Password = "hickhickup";
				c14.LastName = "Hicks";
				c14.FirstName = "Anthony";
				c14.Address = "932 Monica Way";
				c14.City = "San Antonio";
				c14.State = "TX";
				c14.ZipCode = 78203.ToString();
				c14.PhoneNumber = 1211949601.ToString();
				c14.Email = "ahick@yaho.com";
                c14.UserName = c14.Email;
                Customers.Add(c14);

				User c15 = new User();
                c15.Password = "ingram2015";
				c15.LastName = "Ingram";
				c15.FirstName = "Brad";
				c15.Address = "4 Lukken Court";
				c15.City = "New Braunfels";
				c15.State = "TX";
                c15.ZipCode = 78132.ToString();
                c15.PhoneNumber = 1372121569.ToString();
				c15.Email = "ingram@jack.com";
                c15.UserName = c15.Email;
                Customers.Add(c15);

				User c16 = new User();
                c16.Password = "toddy25";
				c16.LastName = "Jacobs";
				c16.FirstName = "Todd";
				c16.Address = "7 Susan Junction";
				c16.City = "New York";
				c16.State = "NY";
				c16.ZipCode = 10101.ToString();
				c16.PhoneNumber = 8543163836.ToString();
				c16.Email = "toddj@yourmom.com";
                c16.UserName = c16.Email;
                Customers.Add(c16);

				User c17 = new User();
                c17.Password = "something";
				c17.LastName = "Lawrence";
				c17.FirstName = "Victoria";
				c17.Address = "669 Oak Junction";
				c17.City = "Lockhart";
				c17.State = "TX";
				c17.ZipCode = 78644.ToString();
				c17.PhoneNumber = 3214163359.ToString();
				c17.Email = "thequeen@aska.net";
                c17.UserName = c17.Email;
                Customers.Add(c17);

				User c18 = new User();
                c18.Password = "Password1";
				c18.LastName = "Lineback";
				c18.FirstName = "Erik";
				c18.Address = "099 Luster Point";
				c18.City = "Kingwood";
				c18.State = "TX";
				c18.ZipCode = 77325.ToString();
				c18.PhoneNumber = 2505265350.ToString();
				c18.Email = "linebacker@gogle.com";
                c18.UserName = c18.Email;
                Customers.Add(c18);

				User c19 = new User();
                c19.Password = "aclfest2017";
				c19.LastName = "Lowe";
				c19.FirstName = "Ernest";
				c19.Address = "35473 Hansons Hill";
				c19.City = "Beverly Hills";
				c19.State = "CA";
				c19.ZipCode = 90210.ToString();
				c19.PhoneNumber = 4070619503.ToString();
				c19.Email = "elowe@netscare.net";
                c19.UserName = c19.Email;
                Customers.Add(c19);

				User c20 = new User();
                c20.Password = "nothinggood";
				c20.LastName = "Luce";
				c20.FirstName = "Chuck";
				c20.Address = "4 Emmet Junction";
				c20.City = "Navasota";
				c20.State = "TX";
				c20.ZipCode = 77868.ToString();
				c20.PhoneNumber = 7358436110.ToString();
				c20.Email = "cluce@gogle.com";
                c20.UserName = c20.Email;
                Customers.Add(c20);

				User c21 = new User();
                c21.Password = "whatever";
				c21.LastName = "MacLeod";
				c21.FirstName = "Jennifer";
				c21.Address = "3 Orin Road";
				c21.City = "Austin";
				c21.State = "TX";
				c21.ZipCode = 78712.ToString();
				c21.PhoneNumber = 7240178229.ToString();
				c21.Email = "mackcloud@george.com";
                c21.UserName = c21.Email;
                Customers.Add(c21);

				User c22 = new User();
                c22.Password = "snowsnow";
				c22.LastName = "Markham";
				c22.FirstName = "Elizabeth";
				c22.Address = "8171 Commercial Crossing";
				c22.City = "Austin";
				c22.State = "TX";
				c22.ZipCode = 78712.ToString();
				c22.PhoneNumber = 2495200223.ToString();
				c22.Email = "cmartin@beets.com";
                c22.UserName = c22.Email;
                Customers.Add(c22);

				User c23 = new User();
                c23.Password = "whocares";
				c23.LastName = "Martin";
				c23.FirstName = "Clarence";
				c23.Address = "96 Anthes Place";
				c23.City = "Schenectady";
				c23.State = "NY";
				c23.ZipCode = 12345.ToString();
				c23.PhoneNumber = 4086179161.ToString();
				c23.Email = "clarence@yoho.com";
                c23.UserName = c23.Email;
                Customers.Add(c23);

				User c24 = new User();
                c24.Password = "xcellent";
				c24.LastName = "Martinez";
				c24.FirstName = "Gregory";
				c24.Address = "10 Northridge Plaza";
				c24.City = "Austin";
				c24.State = "TX";
				c24.ZipCode = 78717.ToString();
				c24.PhoneNumber = 9371927523.ToString();
				c24.Email = "gregmartinez@drdre.com";
                c24.UserName = c24.Email;
                Customers.Add(c24);

				User c25 = new User();
                c25.Password = "mydogspot";
				c25.LastName = "Miller";
				c25.FirstName = "Charles";
				c25.Address = "87683 Schmedeman Circle";
				c25.City = "Austin";
				c25.State = "TX";
				c25.ZipCode = 78727.ToString();
				c25.PhoneNumber = 5954063857.ToString();
				c25.Email = "cmiller@bob.com";
                c25.UserName = c25.Email;
                Customers.Add(c25);

				User c26 = new User();
                c26.Password = "spotmydog";
				c26.LastName = "Nelson";
				c26.FirstName = "Kelly";
				c26.Address = "3244 Ludington Court";
				c26.City = "Beaumont";
				c26.State = "TX";
				c26.ZipCode = 77720.ToString();
				c26.PhoneNumber = 8929209512.ToString();
				c26.Email = "knelson@aoll.com";
                c26.UserName = c26.Email;
                Customers.Add(c26);

				User c27 = new User();
                c27.Password = "joejoejoe";
				c27.LastName = "Nguyen";
				c27.FirstName = "Joe";
				c27.Address = "4780 Talisman Court";
				c27.City = "San Marcos";
				c27.State = "TX";
				c27.ZipCode = 78667.ToString();
				c27.PhoneNumber = 9226301774.ToString();
				c27.Email = "joewin@xfactor.com";
                c27.UserName = c27.Email;
                Customers.Add(c27);

				User c28 = new User();
                c28.Password = "billyboy";
				c28.LastName = "O'Reilly";
				c28.FirstName = "Bill";
				c28.Address = "4154 Delladonna Plaza";
				c28.City = "Bergheim";
				c28.State = "TX";
				c28.ZipCode = 78004.ToString();
				c28.PhoneNumber = 2537646912.ToString();
				c28.Email = "orielly@foxnews.cnn";
                c28.UserName = c28.Email;
                Customers.Add(c28);

				User c29 = new User();
                c29.Password = "radgirl";
				c29.LastName = "Radkovich";
				c29.FirstName = "Anka";
				c29.Address = "72361 Bayside Drive";
				c29.City = "Austin";
				c29.State = "TX";
				c29.ZipCode = 78789.ToString();
				c29.PhoneNumber = 2182889379.ToString();
				c29.Email = "ankaisrad@gogle.com";
                c29.UserName = c29.Email;
                Customers.Add(c29);

				User c30 = new User();
                c30.Password = "meganr34";
				c30.LastName = "Rhodes";
				c30.FirstName = "Megan";
				c30.Address = "76875 Hoffman Point";
				c30.City = "Orlando";
				c30.State = "FL";
				c30.ZipCode = 32830.ToString();
				c30.PhoneNumber = 9532396075.ToString();
				c30.Email = "megrhodes@freserve.co.uk";
                c30.UserName = c30.Email;
                Customers.Add(c30);

				User c31 = new User();
                c31.Password = "ricearoni";
				c31.LastName = "Rice";
				c31.FirstName = "Eryn";
				c31.Address = "048 Elmside Park";
				c31.City = "South Padre Island";
				c31.State = "TX";
				c31.ZipCode = 78597.ToString();
				c31.PhoneNumber = 7303815953.ToString();
				c31.Email = "erynrice@aoll.com";
                c31.UserName = c31.Email;
                Customers.Add(c31);

				User c32 = new User();
                c32.Password = "alaskaboy";
				c32.LastName = "Rodriguez";
				c32.FirstName = "Jorge";
				c32.Address = "01 Browning Pass";
				c32.City = "Austin";
				c32.State = "TX";
				c32.ZipCode = 78744.ToString();
				c32.PhoneNumber = 3677322422.ToString();
				c32.Email = "jorge@noclue.com";
                c32.UserName = c32.Email;
                Customers.Add(c32);


				User c33 = new User();
                c33.Password = "bunnyhop";
				c33.LastName = "Rogers";
				c33.FirstName = "Allen";
				c33.Address = "844 Anderson Alley";
				c33.City = "Canyon Lake";
				c33.State = "TX";
				c33.ZipCode = 78133.ToString();
				c33.PhoneNumber = 3911705385.ToString();
				c33.Email = "mrrogers@lovelyday.com";
                c33.UserName = c33.Email;
                Customers.Add(c33);

				User c34 = new User();
                c34.Password = "dustydusty";
				c34.LastName = "Saint-Jean";
				c34.FirstName = "Olivier";
				c34.Address = "1891 Docker Point";
				c34.City = "Austin";
				c34.State = "TX";
				c34.ZipCode = 78779.ToString();
				c34.PhoneNumber = 7351610920.ToString();
				c34.Email = "stjean@athome.com";
                c34.UserName = c34.Email;
                Customers.Add(c34);

				User c35 = new User();
				c35.Password = "jrod2017";
				c35.LastName = "Saunders";
				c35.FirstName = "Sarah";
				c35.Address = "1469 Upham Road";
				c35.City = "Austin";
				c35.State = "TX";
				c35.ZipCode = 78720.ToString();
				c35.PhoneNumber = 5269661692.ToString();
				c35.Email = "saunders@pen.com";
                c35.UserName = c35.Email;
                Customers.Add(c35);

				User c36 = new User();
                c36.Password = "martin1234";
				c36.LastName = "Sewell";
				c36.FirstName = "William";
				c36.Address = "1672 Oak Valley Circle";
				c36.City = "Austin";
				c36.State = "TX";
				c36.ZipCode = 78705.ToString();
				c36.PhoneNumber = 1875727246.ToString();
				c36.Email = "willsheff@email.com";
                c36.UserName = c36.Email;
                Customers.Add(c36);

				User c37 = new User();
                c37.Password = "penguin12";
				c37.LastName = "Sheffield";
				c37.FirstName = "Martin";
				c37.Address = "816 Kennedy Place";
				c37.City = "Round Rock";
				c37.State = "TX";
				c37.ZipCode = 78680.ToString();
				c37.PhoneNumber = 1394323615.ToString();
				c37.Email = "sheffiled@gogle.com";
                c37.UserName = c37.Email;
                Customers.Add(c37);

				User c38 = new User();
				c38.Password = "rogerthat";
				c38.LastName = "Smith";
				c38.FirstName = "John";
				c38.Address = "0745 Golf Road";
				c38.City = "Austin";
				c38.State = "TX";
				c38.ZipCode = 78760.ToString();
				c38.PhoneNumber = 6645937874.ToString();
				c38.Email = "johnsmith187@aoll.com";
                c38.UserName = c38.Email;
                Customers.Add(c38);

				User c39 = new User();
                c39.Password = "smitty444";
				c39.LastName = "Stroud";
				c39.FirstName = "Dustin";
				c39.Address = "505 Dexter Plaza";
				c39.City = "Sweet Home";
				c39.State = "TX";
				c39.ZipCode = 77987.ToString();
				c39.PhoneNumber = 6470254680.ToString();
				c39.Email = "dustroud@mail.com";
                c39.UserName = c39.Email;
                Customers.Add(c39);

				User c40 = new User();
                c40.Password = "stewball";
				c40.LastName = "Stuart";
				c40.FirstName = "Eric";
				c40.Address = "585 Claremont Drive";
				c40.City = "Corpus Christi";
				c40.State = "TX";
				c40.ZipCode = 78412.ToString();
				c40.PhoneNumber = "7701621022";
				c40.Email = "estuart@anchor.net";
                c40.UserName = c40.Email;
                Customers.Add(c40);

				User c41 = new User();
                c41.Password = "slowwind";
				c41.LastName = "Stump";
				c41.FirstName = "Peter";
				c41.Address = "89035 Welch Circle";
				c41.City = "Pflugerville";
				c41.State = "TX";
				c41.ZipCode = 78660.ToString();
				c41.PhoneNumber = "2181960061";
				c41.Email = "peterstump@noclue.com";
                c41.UserName = c41.Email;
                Customers.Add(c41);

				User c42 = new User();
                c42.Password = "tanner5454";
				c42.LastName = "Tanner";
				c42.FirstName = "Jeremy";
				c42.Address = "4 Stang Trail";
				c42.City = "Austin";
				c42.State = "TX";
				c42.ZipCode = 78702.ToString();
				c42.PhoneNumber = "9908469499";
				c42.Email = "jtanner@mustang.net";
                c42.UserName = c42.Email;
                Customers.Add(c42);

				User c43 = new User();
                c43.Password = "allyrally";
				c43.LastName = "Taylor";
				c43.FirstName = "Allison";
				c43.Address = "726 Twin Pines Avenue";
				c43.City = "Austin";
				c43.State = "TX";
				c43.ZipCode = 78713.ToString();
				c43.PhoneNumber = "7011918647";
				c43.Email = "taylordjay@aoll.com";
                c43.UserName = c43.Email;
                Customers.Add(c43);

				User c44 = new User();
                c44.Password = "taylorbaylor";
				c44.LastName = "Taylor";
				c44.FirstName = "Rachel";
				c44.Address = "06605 Sugar Drive";
				c44.City = "Austin";
				c44.State = "TX";
				c44.ZipCode = 78712.ToString();
				c44.PhoneNumber = "8937910053";
				c44.Email = "rtaylor@gogle.com";
                c44.UserName = c44.Email;
                Customers.Add(c44);

				User c45 = new User();
                c45.Password = "teeoff22";
				c45.LastName = "Tee";
				c45.FirstName = "Frank";
				c45.Address = "3567 Dawn Plaza";
				c45.City = "Austin";
				c45.State = "TX";
				c45.ZipCode = 78786.ToString();
				c45.PhoneNumber = "6394568913";
				c45.Email = "teefrank@noclue.com";
                c45.UserName = c45.Email;
                Customers.Add(c45);

				User c46 = new User();
                c46.Password = "tucksack1";
				c46.LastName = "Tucker";
				c46.FirstName = "Clent";
				c46.Address = "704 Northland Alley";
				c46.City = "San Antonio";
				c46.State = "TX";
				c46.ZipCode = 78279.ToString();
                c46.PhoneNumber = "2676838676";
				c46.Email = "ctucker@alphabet.co.uk";
                c46.UserName = c46.Email;
				Customers.Add(c46);

				User c47 = new User();
                c47.Password = "meow88";
				c47.LastName = "Velasco";
				c47.FirstName = "Allen";
				c47.Address = "72 Harbort Point";
				c47.City = "Navasota";
				c47.State = "TX";
				c47.ZipCode = 77868.ToString();
				c47.PhoneNumber = "3452909754";
				c47.Email = "avelasco@yoho.com";
                c47.UserName = c47.Email;
                Customers.Add(c47);

				User c48 = new User();
                c48.Password = "vinovino";
				c48.LastName = "Vino";
				c48.FirstName = "Janet";
				c48.Address = "1 Oak Valley Place";
				c48.City = "Boston";
				c48.State = "MA";
				c48.ZipCode = 02114.ToString();
				c48.PhoneNumber = "8567089194";
				c48.Email = "vinovino@grapes.com";
                c48.UserName = c48.Email;
                Customers.Add(c48);

				User c49 = new User();
                c49.Password = "gowest";
				c49.LastName = "West";
				c49.FirstName = "Jake";
				c49.Address = "48743 Banding Parkway";
				c49.City = "Marble Falls";
				c49.State = "TX";
				c49.ZipCode = 78654.ToString();
                c49.PhoneNumber = "6260784394";
				c49.Email = "westj@pioneer.net";
                c49.UserName = c49.Email;
                Customers.Add(c49);

				User c50 = new User();
                c50.Password = "louielouie";
				c50.LastName = "Winthorpe";
				c50.FirstName = "Louis";
				c50.Address = "96850 Summit Crossing";
				c50.City = "Austin";
				c50.State = "TX";
				c50.ZipCode = 78730.ToString();
				c50.PhoneNumber = "3733971174";
				c50.Email = "winner@hootmail.com";
                c50.UserName = c50.Email;

                Customers.Add(c50);

				User c51 = new User();
                c51.Password = "woodyman1";
				c51.LastName = "Wood";
				c51.FirstName = "Reagan";
				c51.Address = "18354 Bluejay Street";
				c51.City = "Austin";
				c51.State = "TX";
				c51.ZipCode = 78712.ToString();
				c51.PhoneNumber = "8433359800";
				c51.Email = "rwood@voyager.net";
                c51.UserName = c51.Email;
				Customers.Add(c51);

				//loop through customers
				foreach (User cust in Customers)
				{
					//set name of customer to help debug
					custName = cust.Email;
                    
                    cust.UserName = cust.Email;

					//see if customer exists in database
					User dbCust = db.Users.FirstOrDefault(b => b.Email == cust.Email);

					if (dbCust == null) //customer does not exist in database
					{
						db.Users.Add(cust);
						db.SaveChanges();
						intCustomersAdded += 1;
					}
					else
					{
						//dbCust.UserID = cust.UserID;
						dbCust.Email = cust.Email;
						dbCust.Password = cust.Password;
						dbCust.FirstName = cust.FirstName;
						dbCust.LastName = cust.LastName;
						dbCust.Address = cust.Address;
						dbCust.City = cust.City;
						dbCust.State = cust.State;
						dbCust.ZipCode = cust.ZipCode;
						dbCust.PhoneNumber = cust.PhoneNumber;
						dbCust.UserType = cust.UserType;
                        dbCust.ActiveUser = true;
                        dbCust.UserName = cust.Email;
						db.Update(dbCust);
						db.SaveChanges();
					}
				}
            }
            catch (Exception e)
            {
                String msg = "Books added:" + intCustomersAdded + "; Error on " + custName;
                InvalidOperationException e2 = new InvalidOperationException(msg, e);
                throw e2;
            }
        }
	}
}
