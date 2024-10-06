using BookWorm.Data.UnitOfWork;
using BookWorm.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public BookController(ILogger<BookController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ViewResult> Details(int id)
        {
            var book = await _unitOfWork.BookRepository.GetBookByIdAsync(id);
            return View(book);
        }


        [HttpGet]
        public async Task<ViewResult> Create()
        {
            ViewBag.Genres = await _unitOfWork.GenreRepository.GetAllIncludeAsync(genre => genre.Photo);
            ViewBag.Authors = await _unitOfWork.AuthorRepository.GetAllIncludeAsync(author => author.Photo);
            ViewBag.Awards = await _unitOfWork.AwardRepository.GetAllIncludeAsync(award => award.AwardPhoto);
            ViewBag.Publishers =
                await _unitOfWork.PublisherRepository.GetAllIncludeAsync(publisher => publisher.PublisherLogo);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            try
            {
                await _unitOfWork.BookRepository.AddAsync(book);
                return RedirectToAction("Details","Book",new {id = book.Id});
            }
            catch (OperationCanceledException e)
            {
                return View("Error");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e,"unexpected behavior");
                return View("Error");
            }
        }
    }
}
