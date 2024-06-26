﻿using Dapper;
using ReadEase_C_.Interface;
using ReadEase_C_.Models;
using WebApplication1.Models;

namespace Books.Services

{
    public class BookService : IBookService
    {
        private readonly IConnectionService _connectionService;
        public BookService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }


        private IEnumerable<BookModel> QueryBooks(string query, object? parameters = null)
        {
            using var connection = _connectionService.GetConnection();
            return connection.Query<BookModel>(query, parameters);
        }

        private void Execute(string query, object? parameters = null)
        {
            using var connection = _connectionService.GetConnection();
            connection.Execute(query, parameters);
        }

        public IEnumerable<BookModel> GetAllBooksForUser(int userId)
        {
            string getQuery = "SELECT * FROM BOOKS WHERE UserId=@UserId";
            return QueryBooks(getQuery, new { UserId = userId });
        }

        public IEnumerable<BookModel> GetSingleBook(int bookId)
        {
            string getQuery = "SELECT * FROM BOOKS WHERE BookId=@bookId";
            return QueryBooks(getQuery, new { BookId=bookId });
        }
        public IEnumerable<BookModel> GetPaginatedBooks(int id, int pageNumber = 1, int pageSize = 10)
        {
            int startIndex = (pageNumber - 1) * pageSize;
            string getQuery = "SELECT * FROM BOOKS WHERE UserId=@Id ORDER BY BookId OFFSET @startIndex ROWS FETCH NEXT @pageSize ROWS ONLY;";
            return QueryBooks(getQuery, new {id,startIndex,pageSize });
        }

        public IEnumerable<BookModel> GetAllBooksFromSearch(int id, string search)
        {
            string searchQuery = $"SELECT * FROM BOOKS WHERE UserId=@Id AND Author LIKE @Search";
            return QueryBooks(searchQuery, new { id, Search = $"{search}%" });
        }

        public IEnumerable<BookModel> GetPaginatedBooksFromSearch(int id, string search, int pageNumber = 1, int pageSize = 10)
        {
            int startIndex = (pageNumber - 1) * pageSize;
            string searchQuery = "SELECT * FROM BOOKS WHERE UserId=@Id AND Author LIKE @Search ORDER BY BookId OFFSET @startIndex ROWS FETCH NEXT @pageSize ROWS ONLY;";
            return QueryBooks(searchQuery, new { id, Search = $"{search}%",  startIndex,  pageSize });
        }

        public void InsertBook(BookModel book, int id)
        {
            string insertQuery = "INSERT INTO Books (Title, Author, UserId, CategoryId) VALUES (@Title, @Author, @UserId, @CategoryId);";
            Execute(insertQuery, new { book.Title,  book.Author, UserId = id, book.CategoryId });
        }

        public void DeleteBook(int id)
        {
            string deleteQuery = "DELETE FROM Books WHERE bookId=@id";
            Execute(deleteQuery, new { id });
        }

        public IEnumerable<BookModel> SearchAndCategoryAll(int UserId, string? search = null, string? searchTitle = null, List<int>? categories = null)
        {
            string getQuery = "SELECT * FROM Books WHERE UserId=@Id ";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", UserId);

            // If search term is provided, add it to the query and parameters
            if (!string.IsNullOrEmpty(search))
            {
                getQuery += "AND AUTHOR LIKE @search ";
                parameters.Add("@search", $"{search}%");
            }

            if (!string.IsNullOrEmpty(searchTitle))
            {
                getQuery += "AND TITLE LIKE @searchTitle ";
                parameters.Add("@searchTitle", $"{searchTitle}%");
            }

            // If categories are provided, add them to the query and parameters
            if (categories != null && categories.Any())
            {
                getQuery += "AND CategoryId IN @categories ";
                parameters.Add("@categories", categories);
            }

            getQuery += "ORDER BY CategoryId";

            return QueryBooks(getQuery, parameters);
        }



        public (IEnumerable<BookModel>, int) SearchAndCategory(int UserId, string? search, string? searchTitle = null, List<int>? categories = null, int pageNumber = 1, int pageSize = 10)
        {
            int startIndex = (pageNumber - 1) * pageSize;
            var getQuery = "SELECT * FROM Books WHERE UserId=@Id ";
            var countQuery = "SELECT COUNT(*) FROM Books WHERE UserId=@Id ";
            var _connection = _connectionService.GetConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", UserId);

            // If search term is provided, add it to the query and parameters
            if (!string.IsNullOrEmpty(search))
            {
                getQuery += "AND AUTHOR LIKE @search ";
                countQuery += "AND AUTHOR LIKE @search ";
                parameters.Add("@search", $"{search}%");
            }

            if (!string.IsNullOrEmpty(searchTitle))
            {
                getQuery += "AND TITLE LIKE @searchTitle ";
                countQuery += "AND TITLE LIKE @searchTitle ";
                parameters.Add("@searchTitle", $"{searchTitle}%");
            }

            // If categories are provided, add them to the query and parameters
            if (categories != null && categories.Any())
            {
                getQuery += "AND CategoryId IN @categories ";
                countQuery += "AND CategoryId IN @categories ";
                parameters.Add("@categories", categories);
            }

            getQuery += "ORDER BY CategoryId OFFSET @startIndex ROWS FETCH NEXT @pageSize ROWS ONLY ";
            parameters.Add("@startIndex", startIndex);
            parameters.Add("@pageSize", pageSize);
            int totalCount = _connection.QueryFirstOrDefault<int>(countQuery, parameters);

            var books = QueryBooks(getQuery, parameters);

            return (books, totalCount); 
        }



        public void UpdateBook(UpdateBook book)
        {
            var connection = _connectionService.GetConnection();
            string sql = "UPDATE Books SET ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(book.NewTitle))
            {
                sql += "Title = @NewTitle, ";
                parameters.Add("@NewTitle", book.NewTitle);
            }

            if (!string.IsNullOrEmpty(book.NewAuthor))
            {
                sql += "Author = @NewAuthor, ";
                parameters.Add("@NewAuthor", book.NewAuthor);
            }

            if (book.NewCategory != 0) // Check if the category is different from 0
            {
                sql += "CategoryId = @NewCategory, ";
                parameters.Add("@NewCategory", book.NewCategory);
            }

            // Remove the trailing comma and space
            sql = sql.TrimEnd(',', ' ');

            sql += " WHERE BookId = @BookId";
            parameters.Add("@BookId", book.Id);

            connection.Execute(sql, parameters);
        }


        public void DeleteBooksByUser(int id)
        {
            var connection = _connectionService.GetConnection();
            string deleteBooks = "DELETE FROM BOOKS WHERE UserId = @id";
            connection.Execute(deleteBooks, new { id });

        }

    }
}
