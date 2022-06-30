using FinalProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Threading.Tasks;
using CodeID.Helper;

namespace FinalProject.Facade
{
    public class ReturnBookFacade : BasesFacade<ReturnBook>
    {
        public async Task<bool> ProccessReturn(ReturnBook returnbook)
        {
            try
            {
                var issuebook = Context.IssueBook.SingleOrDefault(a => a.IssueBookID == returnbook.IssueBookID);
                var book = Context.Books.SingleOrDefault(b => b.BookID == issuebook.BookID);
                if (issuebook is null) 
                {
                    throw new Exception("Issue Book not found!");
                }

                //update stok buku dan ubah status peminjaman
                issuebook.Status = false;
                book.Stock += 1;
                var retproccess = Context.ReturnBook.Add(returnbook);

                return await Context.SaveChangesAsync() > 0;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
