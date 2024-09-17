using BookWorm.Data.UnitOfWork;
using BookWorm.Models;
using BookWorm.Models.DTOs;
using BookWorm.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class GenreController : Controller
    {
        private const string NotFoundMessage = "The Page You Were Looking for Was Not Found!";
        private const string UnexpectedErrorMessage = "Something Unexpected Has Happened!";
        private readonly IUnitOfWork _unitOfWork;


        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ViewResult> Details(int id)
        {
            try
            {
                var genre = await _unitOfWork.GenreRepository.GetGenreWithAssociatedBooksAsync(id);
                //TODO:log details
                return genre != null ? View(genre) : View("Error",new ErrorViewModel(NotFoundMessage));
            }
            catch (Exception e)
            {  
                //unexpected error
                return View("Error",new ErrorViewModel(UnexpectedErrorMessage));
            }
        }


        [HttpGet]
        public ViewResult Create() => View();


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] GenreDto genreDto)
        {
            try
            {
                var genre = new Genre(genreDto.Name)
                {
                    Description = genreDto.Description,
                    Photo = new ApplicationImage()
                    {
                        FileName = genreDto.PhotoFile.FileName,
                        FileSize = genreDto.PhotoFile.Length
                    }
                };
                await FileManager.SavePhotoAsync(genre.Photo.Id, genreDto.PhotoFile);
                await _unitOfWork.GenreRepository.AddAsync(genre);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Genre", new {id = genre.Id});
            }
            catch (OperationCanceledException e)
            {
                return View("Error");
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
    }
}
