using BookWorm.Data.UnitOfWork;
using BookWorm.Models;
using BookWorm.Models.DTOs;
using BookWorm.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class AuthorController : Controller
    {
        private const string NotFoundMessage = "The Page You Were Looking for Was Not Found!";
        private const string UnexpectedErrorMessage = "Something Unexpected Has Happened!";
        private readonly IUnitOfWork _unitOfWork;


        public AuthorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ViewResult> Details(int authorId)
        {
            try
            {
                var author = await _unitOfWork.AuthorRepository.GetAuthorWithPhotoAsync(authorId);
                //TODO:log details
                return author != null ? View(author) : View("Error",new ErrorViewModel(NotFoundMessage));
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
        public async Task<IActionResult> Create([FromForm] AuthorDto authorDto)
        {
            try
            {
                var author = new Author(authorDto.Name)
                {
                    About = authorDto.About,
                    Photo = authorDto.PhotoFile == null ? null : new ApplicationImage()
                    {
                        FileName = authorDto.PhotoFile.FileName,
                        FileSize = authorDto.PhotoFile.Length
                    }
                };
                if (author.Photo != null)
                {
                    Console.WriteLine(author.Photo.FilePath);
                    await FileManager.SavePhotoAsync(author.Photo.Id, authorDto.PhotoFile!);
                }
                await _unitOfWork.AuthorRepository.AddAsync(author);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Author", new {authorId = author.Id});
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
