using BookWorm.Data.UnitOfWork;
using BookWorm.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ViewResult> Details(int id)
        {
            var book = await _unitOfWork.BookRepository.GetBookByIdAsync(id);
            return View(book);
        }
    }
}
