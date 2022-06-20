using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

[Route("/books")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IRepository<Book> _repository;
    private readonly FileSaverService _fileSaver;

    public HomeController(IRepository<Book> repository, FileSaverService fileSaver)
    {
        _repository = repository;
        _fileSaver = fileSaver;
    }

    [HttpGet]
    public IActionResult Read()
    {
        try
        {
            return new JsonResult(_repository.GetAll());
        }
        catch (HttpException ex)
        {
            return StatusCodeCheck(ex);
        }
    }
    [HttpPost]
    public IActionResult Create([FromBody] Book book)
    {
        try
        {
            _repository.Create(book);
            return new JsonResult("Book was successfully added!");
        }
        catch (HttpException ex)
        {
            return StatusCodeCheck(ex);
        }
    }
    [HttpPost("img")]
    public async Task<IActionResult> AddImage([FromForm] IFormFile uploadedFile)
    {
        try
        {
            await _fileSaver.Save(uploadedFile, "books/img");
            return StatusCode(200);
        }
        catch(HttpException ex)
        {
            return StatusCodeCheck(ex);
        }
    }
    [HttpPut]
    public IActionResult Update([FromBody] Book book)
    {
        try
        {
            _repository.Update(book);
            return new JsonResult("Book was successfully updated!");
        }
        catch (HttpException ex)
        {
            return StatusCodeCheck(ex);
        }
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _repository.Delete(id);
            return new JsonResult("Book was successfully deleted!");
        }
        catch (HttpException ex)
        {
            return StatusCodeCheck(ex);
        }
    }

    private IActionResult StatusCodeCheck(HttpException ex)
    {
        if (ex.StatusCode > 200 && ex.StatusCode < 300)
        {
            return StatusCode(ex.StatusCode);
        }
        else
        {
            Response.StatusCode = ex.StatusCode;
            return new JsonResult(new HttpException(ex.Message, ex.StatusCode));
        }
    }
}
