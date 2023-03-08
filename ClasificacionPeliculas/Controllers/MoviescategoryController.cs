using ClasificacionPeliculas.Models;
using ClasificacionPeliculasModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClasificacionPeliculas.Controllers
{
    public class MoviescategoryController : Controller
    {
        public IActionResult Index()
        {
            MoviesContext _moviesContext = new MoviesContext();
            IEnumerable<ClasificacionPeliculasModel.Moviescategory> moviescategories =
                (from mc in _moviesContext.Moviescategories
                 join m in _moviesContext.Movies on mc.MovieId equals m.Id
                 join c in _moviesContext.Categories on mc.CategoryId equals c.Id
                 select new ClasificacionPeliculasModel.Moviescategory
                 {
                     Id = mc.Id,
                     MovieName = m.Title,
                     CategoryName = c.Name
                 }).ToList();
            return View(moviescategories);
        }

        public IActionResult Create()
        {
            MoviesContext _moviesContext = new MoviesContext();
            IEnumerable<ClasificacionPeliculasModel.Movie> movies = (from mc in _moviesContext.Movies
                                                                     select new ClasificacionPeliculasModel.Movie
                                                                     {
                                                                         Id = mc.Id,
                                                                         Title = mc.Title,
                                                                     }).ToList();
            IEnumerable<ClasificacionPeliculasModel.Category> categories = (from c in _moviesContext.Categories
                                                                            select new ClasificacionPeliculasModel.Category
                                                                            {
                                                                                Id = c.Id,
                                                                                Name = c.Name
                                                                            }).ToList();
            ClasificacionPeliculasModel.Moviescategory moviescategory = new ClasificacionPeliculasModel.Moviescategory();
            moviescategory.Categories = categories.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();
            moviescategory.Movies = movies.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Title
            }).ToList();
            moviescategory.Movie = (from m in _moviesContext.Movies
                                    where m.Id == movies.FirstOrDefault().Id
                                    select new ClasificacionPeliculasModel.Movie
                                    {
                                        ReleaseDate = m.ReleaseDate,
                                        Duration = m.Duration,
                                        Director = m.Director,
                                        Actors = m.Actors
                                    }).FirstOrDefault();
            return View(moviescategory);
        }
        [HttpPost]
        public IActionResult Create (int MovieId,  int CategoryId)
        {
            Models.Moviescategory moviescategory = new Models.Moviescategory
            {
                MovieId = MovieId,
                CategoryId = CategoryId
            };
            MoviesContext _moviesContext = new MoviesContext();
            _moviesContext.Moviescategories.Add(moviescategory);
            _moviesContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            MoviesContext _moviesContext = new MoviesContext();
            if (id == null || _moviesContext.Moviescategories == null)
            {
                return NotFound();
            }

            var movie = await _moviesContext.Moviescategories.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            IEnumerable<ClasificacionPeliculasModel.Movie> movies = (from mc in _moviesContext.Movies
                                                                     select new ClasificacionPeliculasModel.Movie
                                                                     {
                                                                         Id = mc.Id,
                                                                         Title = mc.Title,
                                                                     }).ToList();
            IEnumerable<ClasificacionPeliculasModel.Category> categories = (from c in _moviesContext.Categories
                                                                            select new ClasificacionPeliculasModel.Category
                                                                            {
                                                                                Id = c.Id,
                                                                                Name = c.Name
                                                                            }).ToList();
            ClasificacionPeliculasModel.Moviescategory moviescategory = new ClasificacionPeliculasModel.Moviescategory();
            moviescategory.MovieId = movie.MovieId;
            moviescategory.CategoryId = movie.CategoryId;
            moviescategory.Categories = categories.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = (s.Id == movie.CategoryId)
            }).ToList();
            moviescategory.Movies = movies.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Title,
                Selected = (s.Id == movie.MovieId)
            }).ToList();
            moviescategory.Movie = (from m in _moviesContext.Movies
                                    where m.Id == movies.FirstOrDefault().Id
                                    select new ClasificacionPeliculasModel.Movie
                                    {
                                        ReleaseDate = m.ReleaseDate,
                                        Duration = m.Duration,
                                        Director = m.Director,
                                        Actors = m.Actors
                                    }).FirstOrDefault();
            return View(moviescategory);
        }
        [HttpPost]
        public IActionResult Edit(int Id, int MovieId, int CategoryId)
        {
            MoviesContext _moviesContext = new MoviesContext();
            Models.Moviescategory moviescategory = _moviesContext.Moviescategories.FirstOrDefault(s => s.Id == Id);
            if (moviescategory != null)
            {
                moviescategory.MovieId = MovieId;
                moviescategory.CategoryId = CategoryId;
                _moviesContext.Moviescategories.Update(moviescategory);
                _moviesContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            MoviesContext _moviesContext = new MoviesContext();
            if (id == null || _moviesContext.Moviescategories == null)
            {
                return NotFound();
            }
            ClasificacionPeliculasModel.Moviescategory? moviescategories =
                (from mc in _moviesContext.Moviescategories
                 join m in _moviesContext.Movies on mc.MovieId equals m.Id
                 join c in _moviesContext.Categories on mc.CategoryId equals c.Id
                 where mc.Id == id
                 select new ClasificacionPeliculasModel.Moviescategory
                 {
                     Id = mc.Id,
                     MovieName = m.Title,
                     CategoryName = c.Name
                 }).FirstOrDefault();
            if (moviescategories == null)
            {
                return NotFound();
            }
            IEnumerable<ClasificacionPeliculasModel.Movie> movies = (from mc in _moviesContext.Movies
                                                                     select new ClasificacionPeliculasModel.Movie
                                                                     {
                                                                         Id = mc.Id,
                                                                         Title = mc.Title,
                                                                     }).ToList();
            moviescategories.Movie = (from m in _moviesContext.Movies
                                    where m.Id == movies.FirstOrDefault().Id
                                    select new ClasificacionPeliculasModel.Movie
                                    {
                                        ReleaseDate = m.ReleaseDate,
                                        Duration = m.Duration,
                                        Director = m.Director,
                                        Actors = m.Actors
                                    }).FirstOrDefault();
            return View(moviescategories);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            MoviesContext _moviesContext = new MoviesContext();
            if (_moviesContext.Moviescategories == null)
            {
                return Problem("Entity set 'MoviesContext.Movies'  is null.");
            }
            var movie = await _moviesContext.Moviescategories.FindAsync(id);
            if (movie != null)
            {
                _moviesContext.Moviescategories.Remove(movie);
            }

            await _moviesContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
