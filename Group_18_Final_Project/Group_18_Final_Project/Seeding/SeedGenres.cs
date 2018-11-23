using System;
using System.Collections.Generic;
using Group_18_Final_Project.Models;
using System.Linq;
using Group_18_Final_Project.dal;

namespace Group_18_Final_Project.Seeding
{
    public static class SeedGenres
    {
        public static void SeedAllGenres(AppDbContext db)
        {
            //check to see if all the genres have already been added
            if (db.Genres.Count() == 12)
            {
                //exit the program - we don't need to do any of this
                NotSupportedException ex = new NotSupportedException("Genre record count is already 21!");
                throw ex;
            }
            Int32 intGenresAdded = 0;
            try
            {
                //Create a list of genres
                List<Genre> Genres = new List<Genre>();

                Genre g1 = new Genre { GenreName = "Adventure" };
                g1.GenreID = 5001;
                Genres.Add(g1);

                Genre g2 = new Genre { GenreName = "Contemporary Fiction" };
                g2.GenreID = 5002;
                Genres.Add(g2);

                Genre g3 = new Genre { GenreName = "Fantasy" };
                g3.GenreID = 5003;
                Genres.Add(g3);

                Genre g4 = new Genre { GenreName = "Historical Fiction" };
                g4.GenreID = 5004;
                Genres.Add(g4);

                Genre g5 = new Genre { GenreName = "Horror" };
                g5.GenreID = 5005;
                Genres.Add(g5);

                Genre g6 = new Genre { GenreName = "Humor" };
                g6.GenreID = 5006;
                Genres.Add(g6);

                Genre g7 = new Genre { GenreName = "Mystery" };
                g7.GenreID = 5007;
                Genres.Add(g7);

                Genre g8 = new Genre { GenreName = "Romance" };
                g8.GenreID = 5008;
                Genres.Add(g8);

                Genre g9 = new Genre { GenreName = "Science Fiction" };
                g9.GenreID = 5009;
                Genres.Add(g9);

                Genre g10 = new Genre { GenreName = "Shakespeare" };
                g10.GenreID = 5010;
                Genres.Add(g10);

                Genre g11 = new Genre { GenreName = "Suspense" };
                g11.GenreID = 5011;
                Genres.Add(g11);

                Genre g12 = new Genre { GenreName = "Thriller" };
                g12.GenreID = 5012;
                Genres.Add(g12);

                Genre l;
      
                //loop through the list and see which (if any) need to be added
                foreach (Genre gen in Genres)
                {
                    //see if the Genre already exists in the database
                    l = db.Genres.FirstOrDefault(x => x.GenreName == gen.GenreName);

                    //Genre was not found
                    if (l == null)
                    {
                        //Add the Genre
                        db.Genres.Add(gen);
                        db.SaveChanges();
                        intGenresAdded += 1;
                    }

                }
            }
            catch
            {
                String msg = "Genres Added: " + intGenresAdded.ToString();
                throw new InvalidOperationException(msg);
            }

        }
    }
}
