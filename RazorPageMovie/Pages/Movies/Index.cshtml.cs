using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPageMovie.Pages_Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; } = default!;
         public SelectList Years { get; set; } = default!;


        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }


         [BindProperty(SupportsGet = true)]
         public string? SelectedYear { get; set; }





        public async Task OnGetAsync()
        {
            // <snippet_search_linqQuery>
              IQueryable<string> genreQuery = _context.Movie
                            .OrderBy(m => m.Genre)
                            .Select(m => m.Genre)
                            .Distinct();
            // </snippet_search_linqQuery>


            // Definning yearQuery p
            IQueryable<string> yearQuery = _context.Movie
                .OrderByDescending(m => m.ReleaseDate)
                .Select(m => m.ReleaseDate.Year.ToString())
                .Distinct();



            var movies = from m in _context.Movie
                         select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }

            if (!string.IsNullOrEmpty(SelectedYear))
            {
                if (int.TryParse(SelectedYear, out int selectedYearInt))
                {
                    movies = movies.Where(x => x.ReleaseDate.Year == selectedYearInt);
                }
            }





            // <snippet_search_selectList>
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
             Years = new SelectList(await yearQuery.ToListAsync());
            // </snippet_search_selectList>
            Movie = await movies.ToListAsync();
            
        }
    }
}
